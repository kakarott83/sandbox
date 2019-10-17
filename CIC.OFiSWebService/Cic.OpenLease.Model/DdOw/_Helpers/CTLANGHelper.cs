// OWNER MK, 05-10-2009
namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class CTLANGHelper
    {
        #region Methods
        public static bool Contains(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysCTLANG)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            return owExtendedEntities.CTLANG.Where(par => par.SYSCTLANG == sysCTLANG).Any<CTLANG>();
        }

        public static CTLANG Deliver(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string code)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            var Query = from ctlang in owExtendedEntities.CTLANG
                        where ctlang.ISOCODE.StartsWith(code)
                        orderby ctlang.SYSCTLANG descending
                        select ctlang;
            return Query.FirstOrDefault();
        }
        #endregion
    }
}