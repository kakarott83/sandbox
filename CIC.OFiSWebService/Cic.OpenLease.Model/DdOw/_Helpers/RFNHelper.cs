
namespace Cic.OpenLease.Model.DdOw
{   
    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class RFNHelper
    {

        public static long DeliverSysRfn(Cic.OpenLease.Model.DdOw.OwExtendedEntities context, string name)
        {
            var Query = from rfn in context.RFN
                        where rfn.NAME.ToUpper() == name.ToUpper()
                        select rfn.SYSRFN;
            return Query.FirstOrDefault<long>();
        }
    }
}
