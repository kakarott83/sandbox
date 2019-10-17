using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Service Provision for BMW
    /// </summary>
    [System.CLSCompliant(true)]
    public class WartungProvisionCalculator : AbstractProvisionCalculator
    {
        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {
            ProvisionDto rval = new ProvisionDto();
            rval.provision = 0;

            PROVRATE tarif = prov.DeliverProvRateAdjusted(param.sysprproduct,param.sysobtyp,param.sysPerole, param.sysBrand, param.rank);

            if(tarif!=null)
                rval.provision = (decimal)tarif.PROVVAL;

            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision);

            return rval;
        }
    }
}