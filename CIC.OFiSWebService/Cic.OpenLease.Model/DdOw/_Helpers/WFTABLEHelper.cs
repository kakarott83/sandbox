//OWNER WB 28.06.2010
namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOw;
    using System;
    #endregion

    public static class WFTABLEHelper
    {
        #region Private Contstants
        private const string CnstWftableSysCodeAngebot = "ANGEBOT";
        #endregion



        public static long DeliverSyswftableForAngebot(DdOw.OwExtendedEntities context)
        {
            WFTABLE WFTABLE = null;
           
            try
            {
                
                var WftableQuery = from wftable in context.WFTABLE
                                   where wftable.SYSCODE == CnstWftableSysCodeAngebot
                                   orderby wftable.SYSWFTABLE descending
                                   select wftable;

                WFTABLE = WftableQuery.FirstOrDefault();
            }
            catch
            {
                throw new Exception("Error during getting WFTABLE");
            }

            if (WFTABLE != null)
            {
                return WFTABLE.SYSWFTABLE;
            }
            else
            {
                throw new Exception("WFTABLE for Angebot does not exists");
            }
        }
    }
}
