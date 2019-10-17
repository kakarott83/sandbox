using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System.Reflection;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Haftpflicht Provision for BMW
    /// </summary>
    [System.CLSCompliant(true)]
    public class HaftpflichtProvisionCalculator : AbstractProvisionCalculator
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {
            ProvisionDto rval = new ProvisionDto();
            rval.provision = 0;

            PROVRATE tarif = prov.DeliverProvRateAdjusted(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);

            if(tarif!=null)
                rval.provision = param.versicherungspraemieexkl * (decimal)tarif.PROVRATE1/100;
            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision);

            _Log.Debug("HP Provision für " + param.versicherungspraemieexkl + "="+rval.provision);

            return rval;
        }
    }
}