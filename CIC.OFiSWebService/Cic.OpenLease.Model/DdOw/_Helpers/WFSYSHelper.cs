namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion
    public class WFSYSHelper
    {


        #region Methods
        public static bool GetDisabled(OwExtendedEntities context)
        {
            int? disabled;
            var Query = from wfsys in context.WFSYS
                        
                        select wfsys.DISABLED;

            disabled = Query.First();

            if (disabled == null)
            {
                throw new System.Exception("Disabled is null");
            }
            return (disabled == 1);
        }

        public static string GetDisabledReason(OwExtendedEntities context)
        {
            string disabledreason ="";
            var Query = from wfsys in context.WFSYS

                        select wfsys.DISABLEDREASON;

            disabledreason = Query.First();

            if (disabledreason == null)
            {
                throw new System.Exception("Disabled is null");
            }
            return disabledreason;
        }
        #endregion
    }
}