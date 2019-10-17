// OWNER WB, 19-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class OBBRIEFHelper
    {
        #region Methods

        public static Cic.OpenLease.Model.DdOl.OBHALTER GetObhalterFromObbrief(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long SYSOBBBRIEF)
        {
            var query = from obhalter in context.OBHALTER
                        where obhalter.OBBRIEF.SYSOBBRIEF == SYSOBBBRIEF
                        select obhalter;

            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.OBHALTER>();
        }
        
        #endregion
    }
}