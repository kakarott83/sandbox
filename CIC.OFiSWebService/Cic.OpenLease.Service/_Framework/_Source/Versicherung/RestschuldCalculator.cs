using Cic.One.Utils.DTO;
using Cic.OpenLease.Service.Provision;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.Service.Services.DdOl.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates the Restschuld Insurance
    /// SYSKORRTYP1 - Tarif TFAU monatliche Rate
    /// SYSKORRTYP2 - Tarif TG absicherung Zielrate
    /// </summary>
    [System.CLSCompliant(true)]
    public class RestschuldCalculator : AbstractVSCalculator
    {
        private string op = "+";
        /// <summary>
        /// Calculates the positions for leasing 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysvstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<ServiceAccess.DdOl.InsuranceResultDto> getLeasingPositions(DdOlExtended context, long sysvstyp, InsuranceParameterDto param, VSTYP vstyp)
        {
            List<ServiceAccess.DdOl.InsuranceResultDto> rvallist = new List<ServiceAccess.DdOl.InsuranceResultDto>();
            List<VsTypPosDto> positions = getVSTypPos(sysvstyp, context); 
            KORREKTURDao korr = new KORREKTURDao(context);
            foreach (VsTypPosDto vst in positions)
            {
                ServiceAccess.DdOl.InsuranceResultDto rval = new ServiceAccess.DdOl.InsuranceResultDto();

                decimal versicherungsSteuerProzent = QUOTEDao.deliverQuotePercentValue(vst.sysSteuer);

                long lz = param.LaufzeitFinanzierung;
                if (vstyp.MAXLAUFZEIT.HasValue && lz > vstyp.MAXLAUFZEIT)
                    lz = vstyp.MAXLAUFZEIT.Value;
                decimal faktor = korr.Correct((long)vst.SYSKORRTYP1, 0, op, DateTime.Now, "" + lz, "");

                decimal myBasis = 0;
                if (vst.Basis == 6)
                {
                    myBasis = param.MonatsrateBrutto;
                }
                else if (vst.Basis == 7)
                {
                    myBasis = param.MonatsrateBrutto + param.GAPSumme / param.LaufzeitFinanzierung;
                }
                else if (vst.Basis == 8)
                {
                    myBasis = param.MonatsrateBrutto + param.GAPSumme / param.LaufzeitFinanzierung;// +rsvmonatbrutto;
                }
                else if (vst.Basis == 9)
                {
                    myBasis = param.MonatsrateBrutto * lz;
                }
                else if (vst.Basis == 10)
                {
                    myBasis = param.MonatsrateBrutto * lz + param.GAPSumme;
                }
                else if (vst.Basis == 11)
                {
                    myBasis = param.MonatsrateBrutto * lz + param.GAPSumme;// +rsvmonatbrutto * param.LaufzeitFinanzierung;
                }
                else if (vst.Basis == 12)
                {
                    myBasis = param.Zielrate;
                }
                else
                {
                    _Log.Warn("RSV Calculator VSTYPPOS Basis not supported: " + vst.Basis);
                }


                //decimal praemie = faktor * (param.MonatsrateBrutto * lz + param.GAPSumme) / 100.0M;
                decimal praemie = faktor * (myBasis) / 100.0M;


                //Default
                rval.Netto = praemie;//per monat! inkl. steuer weil in lookuptable
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
                rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;
                rval.Praemie = rval.Praemie_Default;
                rval.rateNeu = param.MonatsrateBrutto;
                rval.sysvstyppos = vst.sysVSTypPos;
                rvallist.Add(rval);
            }
            return rvallist;
        }

        /// <summary>
        /// Calculates the positions for credit
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysvstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<ServiceAccess.DdOl.InsuranceResultDto> getCreditPositions(DdOlExtended context, long sysvstyp, InsuranceParameterDto param, decimal rsvmonatbrutto)
        {
            
            List<ServiceAccess.DdOl.InsuranceResultDto> rvallist = new List<ServiceAccess.DdOl.InsuranceResultDto>();
            List<VsTypPosDto> positions = getVSTypPos(sysvstyp, context); 
            KORREKTURDao korr = new KORREKTURDao(context);
            foreach (VsTypPosDto vst in positions)
            {
                ServiceAccess.DdOl.InsuranceResultDto rval = new ServiceAccess.DdOl.InsuranceResultDto();
                decimal versicherungsSteuerProzent = QUOTEDao.deliverQuotePercentValue(vst.sysSteuer);


                /*
                 * Auflösung Basiswert  (VSTYPOS:Basis)
0	keine
1	Listenpreis
2	WerkAbgabePreis
3	AHK
4	BGExtern
5	BGIntern
6	monatlFinRate (ohne Versicherung)
7	monatlFinRate (inkl.GAP-Vers.)
8	monatlFinRate (inkl.GAP/RSV-Vers.)
9	SummeMtlFinRaten (ohne Versicherung)
10	SummeMtlFinRaten (inkl. GAP-Vers.)
11	SummeMtlFinRaten (inkl. GAP/RSV-Vers.)
12	Ballonrate
13	VersRate
14	Kilowatt
15	Laufleistung
16	Laufzeit
17	Alter
999	sonstige
*/
                decimal myBasis = 0;
                if (vst.Basis == 6)
                {
                    myBasis = param.MonatsrateBrutto;
                }
                else if (vst.Basis == 7)
                {
                    myBasis = param.MonatsrateBrutto + param.GAPSumme / param.LaufzeitFinanzierung;
                }
                else if (vst.Basis == 8)
                {
                    myBasis = param.MonatsrateBrutto + param.GAPSumme / param.LaufzeitFinanzierung + rsvmonatbrutto;
                }
                else if (vst.Basis == 9)
                {
                    myBasis = param.MonatsrateBrutto * param.LaufzeitFinanzierung;
                }
                else if(vst.Basis==10)
                {
                    myBasis = param.MonatsrateBrutto * param.LaufzeitFinanzierung;
                }
                else if (vst.Basis == 11)
                {
                    myBasis = param.MonatsrateBrutto * param.LaufzeitFinanzierung;
                }
                else if (vst.Basis == 12)
                {
                    myBasis = param.Zielrate;
                }
                else
                {
                    _Log.Warn("RSV Calculator VSTYPPOS Basis not supported: " + vst.Basis);
                }

                if (vst.SYSKORRTYP1 == 0)
                    throw new NullReferenceException("SYSKORRTYP1 nicht gesetzt für Restschuld VSTYPPOS " + vst.sysVSTypPos);

                decimal monRate = (decimal)korr.Correct((long)vst.SYSKORRTYP1, 0, op, DateTime.Now, param.Laufzeit.ToString(), "");
                rval.Netto =  (decimal)monRate * myBasis/100.0M;//inkl. steuer weil in lookuptable
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
                rval.Praemie_Default = rval.Netto + rval.Netto * versicherungsSteuerProzent / 100;
                rval.Praemie = rval.Praemie_Default;


                rval.sysvstyppos = vst.sysVSTypPos;
                rvallist.Add(rval);
            }
            return rvallist;
        }

        /// <summary>
        /// calculates RSDV for Leasing
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="vstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ServiceAccess.DdOl.InsuranceResultDto calculateLeasing(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            InsuranceResultDto rval = new InsuranceResultDto();

            KORREKTURDao korr = new KORREKTURDao(context);
            string op = "+";

            decimal versicherungsSteuerProzent = MyDeliverSteuerPers(context);


            long lz = param.LaufzeitFinanzierung;
            if (vstyp.MAXLAUFZEIT.HasValue && lz > vstyp.MAXLAUFZEIT)
                lz = vstyp.MAXLAUFZEIT.Value;
            decimal faktor = korr.Correct((long)vstyp.SYSKORRTYP1, 0, op, DateTime.Now, "" + lz, "");
            decimal praemie = faktor * (param.MonatsrateBrutto * lz + param.GAPSumme) / 100.0M;


            //Default
            rval.Netto = praemie;//per monat! inkl. steuer weil in lookuptable
            rval.positions = getLeasingPositions(context, vstyp.SYSVSTYP, param,vstyp);

            rval.Versicherungssteuer = getSteuerSum(rval.positions);
            if (rval.positions == null || rval.positions.Count == 0)
            {
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            }
            else
            {
                rval.Netto = rval.Netto - rval.Versicherungssteuer;
                versicherungsSteuerProzent = 0;
                if(rval.Netto>0)
                    versicherungsSteuerProzent = rval.Versicherungssteuer / rval.Netto * 100.0M;
            }
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;
            rval.rateNeu = param.MonatsrateBrutto;
            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);


            // Provisionsberechnung
            try
            {
                rval.Provision = 0;

                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.Restschuld, sysBrand, sysPerole, rval.Netto, rval.Praemie, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Restschuldprovision (" + ProvisionTypeConstants.Restschuld + ") nicht konfiguriert", ie);
            }



            return rval;

        }

        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {


            bool leasing = isLeasing(context, param.sysPrProduct);
            if (leasing)
            {
                return calculateLeasing(context, sysPerole, sysBrand, vstyp, param);
            }

            //Credit calculation:
            InsuranceResultDto rval = new InsuranceResultDto();
            KORREKTURDao korr = new KORREKTURDao(context);
            decimal versicherungsSteuerProzent = MyDeliverSteuerPers(context);

            //Tarif TFAU monatliche Rate
            if (vstyp.SYSKORRTYP1 == null)
                throw new NullReferenceException("SYSKORRTYP1 nicht gesetzt für Restschuld VSTYP " + vstyp.SYSVSTYP);

            //Tarif TG absicherung Zielrate
            if (vstyp.SYSKORRTYP2 == null)
                throw new NullReferenceException("SYSKORRTYP2 nicht gesetzt für Restschuld VSTYP " + vstyp.SYSVSTYP);

            double monRate = (double)korr.Correct((long)vstyp.SYSKORRTYP1, 0, op, DateTime.Now, param.Laufzeit.ToString(), "");

            double absichZielrate = (double)korr.Correct((long)vstyp.SYSKORRTYP2, 0, op, DateTime.Now, param.Laufzeit.ToString(), "");

            if (param.Laufzeit <= 0)
                throw new ArgumentException("Laufzeit != [1; für Restschuldversicherung");
            if (param.Zinssatz < 0)
                throw new ArgumentException("Zinssatz != [0;1[ für Restschuldversicherung");
            if (param.Zinssatz >= 1)
                throw new ArgumentException("Zinssatz != ]0;1[ für Restschuldversicherung");
            double zins = (double)param.Zinssatz * 100.0 / 12.0;


            double bw = (double)param.Finanzierungssumme;
            double gesamtversicherungsPraemie = 0;
            double monGesamtfinrate = 0;
            for (int i = 0; i < 5; i++)
            {
                monGesamtfinrate = calcIterationRate(zins, (double)param.Laufzeit, (double)bw, (double)param.Zielrate);
                gesamtversicherungsPraemie = calcPraemie((double)param.Zielrate, absichZielrate, param.Laufzeit, monGesamtfinrate, monRate);
                bw = (double)param.Finanzierungssumme + gesamtversicherungsPraemie;
            }

            rval.rateNeu = (decimal)monGesamtfinrate;

            rval.Netto = (decimal)gesamtversicherungsPraemie;//inkl. steuer weil in lookuptable

            param.MonatsrateBrutto = rval.rateNeu;//needed in positions base calculation
            rval.positions = getCreditPositions(context, vstyp.SYSVSTYP, param, rval.Netto);

            rval.Versicherungssteuer = getSteuerSum(rval.positions);
            if (rval.positions == null || rval.positions.Count == 0)
            {
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            }
            else
            {
                rval.Netto = rval.Netto - rval.Versicherungssteuer;
                versicherungsSteuerProzent = rval.Versicherungssteuer / rval.Netto * 100.0M;
            }

            rval.Praemie_Default = rval.Netto + rval.Netto * versicherungsSteuerProzent / 100;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);

            CalculationMode Mode = CalculationMode.Begin;
            if (param.isCredit)
                Mode = CalculationMode.End;
            rval.Gesamtfinanzierungsrate = (decimal)Cic.One.Utils.BO.Kalkulator.calcRATE((double)gesamtversicherungsPraemie, (double)(param.ZinssatzNominal / 12.0M), (double)param.LaufzeitFinanzierung, (double)0, Mode == CalculationMode.Begin);


            // Provisionsberechnung
            try
            {
                rval.Provision = 0;
                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.Restschuld, sysBrand, sysPerole, rval.Netto, rval.Praemie, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Restschuldprovision (" + ProvisionTypeConstants.Restschuld + ") nicht konfiguriert", ie);
            }


            return rval;
        }

        private double calcPraemie(double sr, double tcfpercentrw, double term, double rate, double percentnorw)
        {
            double lz = term;// -1;
            return (sr * tcfpercentrw + (lz * rate) * (percentnorw)) / 100.0;
        }
        private double calcIterationRate(double zins, double term, double bw, double rw)
        {
            // double effzins =ezins;
            //double zins =(Math.Pow((1+effzins/100.0),(1.0/12.0))-1)*100.0;
            double lz = term;// -1;
            return Kalkulator.calcRATE(bw, zins, lz, rw, Kalkulator.ZAHLMODUS_NACHSCHUESSIG);


        }
    }
}
