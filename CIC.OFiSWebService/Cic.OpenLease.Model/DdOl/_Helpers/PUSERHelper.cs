namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOw;
    #endregion

    [System.CLSCompliant(true)]
    public static class PUSERHelper
    {
        #region Methods

        public static Cic.OpenLease.Model.DdOw.PUSER GetPUSERBySysPUSER(long? sysPUSER)
        {
            Cic.OpenLease.Model.DdOw.PUSER PUSER = null;
            using (Cic.OpenLease.Model.DdOw.OwExtendedEntities Context = new Cic.OpenLease.Model.DdOw.OwExtendedEntities())
            {
                PUSER = Context.PUSER.Where(u => u.SYSPUSER == sysPUSER).FirstOrDefault();
            }
            return PUSER;
        }

        #endregion
    }
}