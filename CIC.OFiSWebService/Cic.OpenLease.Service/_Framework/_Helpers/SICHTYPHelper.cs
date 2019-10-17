namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    #endregion

    public static class SICHTYPHelper
    {
        public static SICHTYP GetSichTyp(DdOlExtended context, int rang)
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
