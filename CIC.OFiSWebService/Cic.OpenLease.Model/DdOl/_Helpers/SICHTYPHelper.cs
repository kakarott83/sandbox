namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOl;
    #endregion

    public static class SICHTYPHelper
    {
        public static SICHTYP GetSichTyp(OlExtendedEntities context, int rang)
        {
            SICHTYP SICHTYP;

            var Query = from sichtyp in context.SICHTYP
                        where sichtyp.RANG == rang
                        select sichtyp;

            SICHTYP = Query.FirstOrDefault();

            if (SICHTYP == null)
            {
                throw new System.Exception("SICHTYP is null");
            }

            return SICHTYP;
        }
    }
}
