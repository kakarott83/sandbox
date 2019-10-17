using Cic.OpenLease.Service.Provision;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using System;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates the BMW AIDA Kasko Insurance
    /// SYSKORRTYP2 - Bonuskasko (optional) param1: Prämienstufe
    /// SYSKORRTYP1 - Kasko Abschlag, param1: Kaskostufe bzw. Prämienstufe
    /// SYSVG - Prämientabelle
    /// </summary>
    [System.CLSCompliant(true)]
    public class KaskoCalculator : AbstractVSCalculator
    {


        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            InsuranceResultDto rval = new InsuranceResultDto();


            KORREKTURDao korr = new KORREKTURDao(context);
            string op = "+";

            //Bonuskasko - SYSKORRTYP2
            long kaskoStufe = param.Praemienstufe;
            if (vstyp.SYSKORRTYP2 != null)
            {
                kaskoStufe = (long)korr.Correct((long)vstyp.SYSKORRTYP2, 0, op, DateTime.Now, param.Praemienstufe.ToString(), "");
            }
            if (vstyp.SYSKORRTYP1 == null)
                throw new NullReferenceException("Kasko für VSTYP " + vstyp.SYSVSTYP + " hat keinen Korrekturtypen in SYSKORRTYP1");

            decimal kaskoAbschlagProzent = korr.Correct((long)vstyp.SYSKORRTYP1, 0, op, DateTime.Now, kaskoStufe.ToString(), "");


            VGDao vg = new VGDao(context);

            //Prämienfreie sonderausstattung
            decimal sonderausstattung = (decimal)param.Sonderausstattung - (decimal)vstyp.SAPRAEMIENFREI;
            if (sonderausstattung < 0) sonderausstattung = 0;
            //NOVANEU informativ
            decimal wert = param.Listenpreis + sonderausstattung + param.SAPakete + param.Nova + param.ZubehoerFinanziert;

            long sysvg =vstyp.SYSVG.GetValueOrDefault();
            if (sysvg == 0)
                throw new NullReferenceException("Kasko für VSTYP " + vstyp.SYSVSTYP + " hat keine Tabellenzuordnung auf VG");


            decimal praemieProzent = 0;
            vg.deliverVGBoundaries(sysvg, DateTime.Now);
            decimal p1 = wert;
            int mode = VGDao.CnstINTERPOLATION_MAXIMUM;
            if (p1 < vg.xmin)
            {
                p1 = vg.xmin;
                mode = VGDao.CnstINTERPOLATION_MINIMUM;
            }

            try
            {
                praemieProzent = (decimal)vg.deliverVGValue(sysvg, DateTime.Now, decimal.Floor(p1).ToString(), vstyp.CODE, mode);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Prämientabelle Kasko für VSTYP " + vstyp.SYSVSTYP + " (SYSVG=" + sysvg + ") nicht konfiguriert für p1=" + p1 + ", p2=" + vstyp.CODE, ie);
            }


            decimal praemie = praemieProzent / 100 * wert;
            decimal rabatt = kaskoAbschlagProzent / 100;

            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);
            rval.Motorsteuer = 0;

            //Default
            decimal rab = 0;
            if (rabatt > 0) rab = (praemie * rabatt);
            rval.Netto = (praemie - rab - (0 * (12 / param.Zahlmodus))) / param.Zahlmodus;
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);
            /*
            //Expl. Subvention:
            decimal pvorSub = rval.Netto;
            decimal subnetto = MyDeliverSubvention(context, rval.Netto, param.sysPrProduct, Subvention.CnstAREA_INSURANCE, param.SysVSTYP, param.Laufzeit);
            rval.Praemie_Subvention = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(pvorSub - subnetto);

            //Implicit Subvention by Nachlass
            rval.Netto = subnetto - (param.Nachlass * (12 / param.Zahlmodus));
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie = rval.Netto + rval.Versicherungssteuer + rval.Motorsteuer;

            //Round all
            rval.Netto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Netto);
            rval.Praemie_Default = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Praemie_Default);
            rval.Praemie = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Praemie);
            rval.Versicherungssteuer = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Versicherungssteuer);
            rval.Motorsteuer = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.Motorsteuer);





            rval.Netto = (praemie - (praemie / 100 * rabatt) - (param.Nachlass * (12 / param.Zahlmodus))) / param.Zahlmodus;
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie = rval.Netto + rval.Versicherungssteuer;*/


            // Provisionsberechnung
            try
            {
                rval.Provision = 0;

                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.Kasko, sysBrand, sysPerole, rval.Netto * param.Zahlmodus, rval.Praemie * param.Zahlmodus, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Kaskoprovision (" + ProvisionTypeConstants.Kasko + ") nicht konfiguriert", ie);
            }



            return rval;
        }
    }
}