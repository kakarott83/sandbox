//OWNER WB 28.06.2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;
    
    
    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    
    #endregion

    public static class WFTABLEHelper
    {
        #region Private Contstants
        private const string CnstWftableSysCodeAngebot = "ANGEBOT";
        #endregion



        public static long DeliverSyswftableForAngebot(DdOwExtended context)
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
