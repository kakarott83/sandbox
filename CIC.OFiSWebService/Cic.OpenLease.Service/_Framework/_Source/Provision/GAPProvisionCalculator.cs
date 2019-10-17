using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System.Reflection;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Calculates Provision for GAP Insurance
    /// </summary>
    [System.CLSCompliant(true)]
    public class GAPProvisionCalculator : AbstractProvisionCalculator
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        override
        public ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param)
        {
            ProvisionDto rval = new ProvisionDto();
            rval.provision = 0;

            PROVRATE tarif = prov.DeliverProvRateAdjusted(param.sysprproduct, param.sysobtyp, param.sysPerole, param.sysBrand, param.rank);
            if (tarif != null)
                rval.provision = param.versicherungspraemiegesamt * (decimal)tarif.PROVRATE1 / 100;

            if(param.sysVstyp>0)
            {
                VSTYP vsType = new VSTYPDao(context).getVsTyp(param.sysVstyp);
                if(vsType.ANTEILLS.HasValue)
                {
                    rval.provision *= vsType.ANTEILLS.Value / 100.0M;
                }
            }
            bool leasing = isLeasing(context, param.sysprproduct);
            int lz = 1;
            if (leasing)
                lz = param.laufzeit;
            rval.provision = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval.provision*lz);
            _Log.Debug("GAP Provision für " + param.versicherungspraemieexkl + "=" + rval.provision);

            return rval;
        }
    }
}