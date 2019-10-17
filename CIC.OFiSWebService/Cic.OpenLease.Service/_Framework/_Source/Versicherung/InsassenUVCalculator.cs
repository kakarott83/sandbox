using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using System;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates the BMW AIDA InsassenUV
    /// SYSQUOTE - Prämie
    /// </summary>
    [System.CLSCompliant(true)]
    public class InsassenUVCalculator : AbstractVSCalculator
    {
        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            InsuranceResultDto rval = new InsuranceResultDto();

            //Versicherungssteuer
            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);

            long sysquote = vstyp.SYSQUOTE.GetValueOrDefault();
            if (sysquote==0)
                throw new NullReferenceException("SYSQUOTE in InsassenUV nicht konfiguriert für VSTYP "+vstyp.SYSVSTYP);

            decimal praemie = (12 / param.Zahlmodus) * QUOTEDao.deliverQuotePercentValue(sysquote);
            rval.Motorsteuer = 0;

            //Default
            rval.Netto = praemie - (0 * (12 / param.Zahlmodus));
            rval.Versicherungssteuer= rval.Netto * versicherungsSteuerProzent/100;
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);






            return rval;
        }

    }
}