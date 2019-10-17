// OWNER WB, 18-03-2010
namespace Cic.OpenLease.Model.DdOl
{

    #region Using
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class VTHelper
    {
        #region Methods

        public static Cic.OpenLease.Model.DdOl.OB GetObFromVt(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSID)
        {

            var ObQuery = from vtobject in context.OB
                          where vtobject.VT.SYSID == SYSID
                          orderby vtobject.RANG
                          select vtobject;
            return ObQuery.FirstOrDefault<Cic.OpenLease.Model.DdOl.OB>(); ;
        }

        #endregion

    }
}
