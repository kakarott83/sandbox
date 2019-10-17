using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOl;
    #endregion
    public static class PRPARAMHelper
    {
        #region Methods
        public static PRPARAM GetPrParam(OlExtendedEntities context, string name)
        {
            PRPARAM PRPARAM;
            var Query = from prparam in context.PRPARAM
                        where prparam.NAME.ToUpper() == name.ToUpper()
                        select prparam;

            PRPARAM = Query.FirstOrDefault();

            //Check if PRPARAM is null
            if (PRPARAM == null)
            {
                throw new System.Exception("PRPARAM is null for name " + name);
            }

            return PRPARAM;
        }
        #endregion
    }
}
