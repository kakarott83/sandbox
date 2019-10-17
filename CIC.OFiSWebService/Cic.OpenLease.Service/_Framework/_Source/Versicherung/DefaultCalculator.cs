using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using System;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates a Default Insurance
    /// SYSQUOTE - Prämie
    /// OPTIONAL SYSKORRTYP1, param1: Laufzeit, param2: Finanzierungssumme, Startwert: praemie from SysQUOTE
    /// </summary>
    [System.CLSCompliant(true)]
    public class DefaultCalculator : AbstractVSCalculator
    {
        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            InsuranceResultDto rval = new InsuranceResultDto();

            //Versicherungssteuer
            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);

            decimal praemie = 0;
            long sysquote = vstyp.SYSQUOTE.GetValueOrDefault();
            if (sysquote>0)
                praemie = (12 / param.Zahlmodus) * QUOTEDao.deliverQuotePercentValue(sysquote);

            KORREKTURDao korr = new KORREKTURDao(context);
            string op = "+";

            //Bonuskasko - SYSKORRTYP2
            if (vstyp.SYSKORRTYP1 != null)
            {
                praemie = (decimal)korr.Correct((long)vstyp.SYSKORRTYP1, praemie, op, DateTime.Now, param.Laufzeit.ToString(), param.Finanzierungssumme.ToString());
            }
            //Default
            rval.Netto = praemie - (0 * (12 / param.Zahlmodus));
            rval.Motorsteuer = 0;
            rval.Versicherungssteuer= rval.Netto * versicherungsSteuerProzent/100;
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);

           
            
            rval.Provision = 0;

            
            return rval;
        }

    }
}