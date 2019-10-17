using Cic.One.Utils.DTO;
using Cic.OpenLease.Service.Provision;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.Service.Services.DdOl.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates the GAP Insurance
    /// Gesamtfinanzierungsrate holds monthly payment for fin. duration
    /// </summary>
    [System.CLSCompliant(true)]
    public class GAPCalculator : AbstractVSCalculator
    {
        private string op = "+";

        /// <summary>
        /// Calculates the positions for leasing 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysvstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<ServiceAccess.DdOl.InsuranceResultDto> getLeasingPositions(DdOlExtended context, long sysvstyp, InsuranceParameterDto param)
        {
            KORREKTURDao korr = new KORREKTURDao(context);
            List<ServiceAccess.DdOl.InsuranceResultDto> rval = new List<ServiceAccess.DdOl.InsuranceResultDto>();
            List<VsTypPosDto> positions = getVSTypPos(sysvstyp, context);
            foreach(VsTypPosDto vst in positions)
            {
                ServiceAccess.DdOl.InsuranceResultDto insurance = new ServiceAccess.DdOl.InsuranceResultDto();

                //Faktor enthält steuer!
                decimal faktorl = korr.Correct((long)vst.SYSKORRTYP1, 0, op, DateTime.Now, param.LaufzeitFinanzierung.ToString(), "");

                decimal baseValue = param.Finanzierungssumme;
                if (param.sysKdTyp > 1)
                    baseValue = param.FinanzierungssummeNetto;

                decimal praemiel = faktorl * baseValue / 100.0M;
                decimal versicherungsSteuerProzent = QUOTEDao.deliverQuotePercentValue(vst.sysSteuer);

                insurance.Netto = praemiel;//per monat!                //hier ist aber wegen dem faktor bereits die steuer drin!
                insurance.Netto = insurance.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                insurance.Versicherungssteuer = insurance.Netto * versicherungsSteuerProzent / 100;
                insurance.Praemie_Default = insurance.Netto + insurance.Versicherungssteuer;
                insurance.Gesamtfinanzierungsrate = insurance.Praemie_Default * param.LaufzeitFinanzierung;
                insurance.sysvstyppos = vst.sysVSTypPos;
                insurance.Praemie = insurance.Praemie_Default;
                rval.Add(insurance);
            }
            return rval;
        }

        /// <summary>
        /// Calculates the positions for credit
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysvstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<ServiceAccess.DdOl.InsuranceResultDto> getCreditPositions(DdOlExtended context, VSTYP vstyp, InsuranceParameterDto param)
        {
            
            VGDao vg = new VGDao(context);
            List<ServiceAccess.DdOl.InsuranceResultDto> rvallist = new List<ServiceAccess.DdOl.InsuranceResultDto>();
            List<VsTypPosDto> positions = getVSTypPos(vstyp.SYSVSTYP, context); 
            foreach (VsTypPosDto vst in positions)
            {
                ServiceAccess.DdOl.InsuranceResultDto rval = new ServiceAccess.DdOl.InsuranceResultDto();

                decimal versicherungsSteuerProzent = QUOTEDao.deliverQuotePercentValue(vst.sysSteuer);
             
                int mode = VGDao.CnstINTERPOLATION_OFF;
                vg.deliverVGBoundaries(vst.sysVG1, DateTime.Now);
                decimal faktor = 0;
                try
                {

                    faktor = (decimal)vg.deliverVGValue(vst.sysVG1, DateTime.Now, decimal.Floor(param.Laufzeit).ToString(), vstyp.CODE, mode);
                }
                catch (Exception ie)
                {
                    throw new InvalidOperationException("Prämientabelle GAP für VSTYPPOS " + vst.sysVSTypPos + " (SYSVG=" + vst.sysVG1 + ") nicht konfiguriert für p1=" + param.Laufzeit + ", p2=" + vst.Dimension2, ie);
                }
                //table contains Percent values
                faktor /= 100.0M;

                decimal praemie = faktor * param.Finanzierungssumme;
                _Log.Debug("GAP: " + faktor + "*" + param.Finanzierungssumme + "=" + praemie);

                CalculationMode Mode = CalculationMode.Begin;
                if (param.isCredit)
                    Mode = CalculationMode.End;
                rval.Gesamtfinanzierungsrate = (decimal)Cic.One.Utils.BO.Kalkulator.calcRATE((double)praemie, (double)(param.ZinssatzNominal / 12.0M), (double)param.LaufzeitFinanzierung, (double)0, Mode == CalculationMode.Begin);

                //Default
                rval.Netto = praemie;//per laufzeit!
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));

                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
                rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;
                rval.sysvstyppos = vst.sysVSTypPos;
                rval.Praemie = rval.Praemie_Default;
                rvallist.Add(rval);
            }
            return rvallist;
        }
        /// <summary>
        /// Calculates GAP for Leasing
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPerole"></param>
        /// <param name="sysBrand"></param>
        /// <param name="vstyp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private ServiceAccess.DdOl.InsuranceResultDto calculateLeasing(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            InsuranceResultDto rval = new InsuranceResultDto();
            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);

            
            KORREKTURDao korr = new KORREKTURDao(context);
            

            //Faktor enthält steuer!
            decimal faktorl = korr.Correct((long)vstyp.SYSKORRTYP1, 0, op, DateTime.Now, param.LaufzeitFinanzierung.ToString(), "");
            decimal baseValue = param.Finanzierungssumme;
            if (param.sysKdTyp > 1)
                baseValue = param.FinanzierungssummeNetto;
            decimal praemiel = faktorl * baseValue / 100.0M;


            //Default
            rval.Netto = praemiel;//per monat!                //hier ist aber wegen dem faktor bereits die steuer drin!
            rval.positions = getLeasingPositions(context,vstyp.SYSVSTYP,param);
            
            
            rval.Versicherungssteuer = getSteuerSum(rval.positions);
            if (rval.positions == null || rval.positions.Count == 0)
            {
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
            }
            else
            {
                rval.Netto = rval.Netto-rval.Versicherungssteuer;
                versicherungsSteuerProzent = 0;
                if(rval.Netto != 0)
                    versicherungsSteuerProzent = rval.Versicherungssteuer / rval.Netto * 100.0M;
            }
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);

            rval.Gesamtfinanzierungsrate = rval.Praemie_Default * param.LaufzeitFinanzierung;
            // Provisionsberechnung
            try
            {
                rval.Provision = 0;

                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.GAP, sysBrand, sysPerole, rval.Netto, rval.Praemie, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("GAP Provision (" + ProvisionTypeConstants.GAP + ") nicht konfiguriert", ie);
            }


            return rval;

        }
        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            
            bool leasing = isLeasing(context, param.sysPrProduct);
            if(leasing)
            {
                return calculateLeasing(context, sysPerole, sysBrand, vstyp, param);
            }


            //Credit calculation:
            InsuranceResultDto rval = new InsuranceResultDto();
            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);

            VGDao vg = new VGDao(context);

            long sysvg = vstyp.SYSVG.GetValueOrDefault();
            if (sysvg == 0)
                throw new NullReferenceException("GAP für VSTYP " + vstyp.SYSVSTYP + " hat keine Tabellenzuordnung auf VG");
            int start = DateTime.Now.TimeOfDay.Milliseconds;
            int mode = VGDao.CnstINTERPOLATION_OFF;

            //vg.deliverVGBoundaries(sysvg, DateTime.Now);

            decimal faktor = 0;
            try
            {
                faktor = (decimal)vg.deliverVGValue(sysvg, DateTime.Now, decimal.Floor(param.Laufzeit).ToString(), vstyp.CODE, mode);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Prämientabelle GAP für VSTYP " + vstyp.SYSVSTYP + " (SYSVG=" + sysvg + ") nicht konfiguriert für p1=" + param.Laufzeit + ", p2=" + vstyp.CODE, ie);
            }
            //table contains Percent values
            faktor /= 100.0M;

            decimal praemie = faktor * param.Finanzierungssumme ;
            _Log.Debug("GAP: "+faktor+"*"+param.Finanzierungssumme+"="+praemie+" duration: "+(DateTime.Now.TimeOfDay.Milliseconds-start));
            
            CalculationMode Mode = CalculationMode.Begin;
            if(param.isCredit)
                Mode = CalculationMode.End;
            rval.Gesamtfinanzierungsrate = (decimal)Cic.One.Utils.BO.Kalkulator.calcRATE((double)praemie, (double)(param.ZinssatzNominal / 12.0M), (double)param.LaufzeitFinanzierung, (double)0, Mode == CalculationMode.Begin);

            //Default
            rval.Netto = praemie;//per laufzeit!
            

            rval.positions = getCreditPositions(context,vstyp,param);
            rval.Versicherungssteuer = getSteuerSum(rval.positions);
            if (rval.positions == null || rval.positions.Count == 0)
            {
                rval.Netto = rval.Netto / (1 + (decimal)(versicherungsSteuerProzent / 100));
                rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;

            }
            else
            {
                rval.Netto = rval.Netto - rval.Versicherungssteuer;
                if (rval.Netto != 0)
                    versicherungsSteuerProzent = rval.Versicherungssteuer/rval.Netto*100.0M;
                else
                    versicherungsSteuerProzent = 0;
            }
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);

            // Provisionsberechnung
            try
            {
                rval.Provision = 0;
                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.GAP, sysBrand, sysPerole, rval.Netto, rval.Praemie, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("GAP Provision (" + ProvisionTypeConstants.GAP + ") nicht konfiguriert", ie);
            }

            return rval;

        }

    }
}