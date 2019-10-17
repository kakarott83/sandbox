// OWNER MK, 02-02-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    public class PLZHelper
    {
        #region Methods
        public static List<PLZ> SearchPlz(OlExtendedEntities context, string plz)
        {
            List<PLZ> PLZList = new List<PLZ>();

            var Query = from plzrow in context.PLZ
                        where plzrow.PLZ1.StartsWith(plz)
                        orderby plzrow.ORT
                        select plzrow;

            PLZList = Query.ToList();

            return PLZList;
        }
        #endregion
    }
}
