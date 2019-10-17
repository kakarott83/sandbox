// OWNER WB, 10-03-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using CIC.Database.OL.EF6.Model;
    using Cic.OpenOne.Common.Model.DdOl;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGEBOTHelper
    {
        #region Methods
        public static bool Contains(DdOlExtended olExtendedEntities, long sysID)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new Exception("olExtendedEntities");
            }

            return olExtendedEntities.ANGEBOT.Where(par => par.SYSID == sysID).Any<ANGEBOT>();
        }

        public static  ANGEBOT GetAngebot(DdOlExtended context, long? SYSID)
        {
            ANGEBOT ANGEBOT;

            var query = from angebot in context.ANGEBOT
                        where angebot.SYSID == SYSID
                        select angebot;

            ANGEBOT = query.FirstOrDefault();

            if (ANGEBOT == null)
            {
                throw new System.Exception("ANGEBOT is null");
            }

            return ANGEBOT;
        }

        public static  ANGKALK GetAngkalkFromAngebot(DdOlExtended context, long? SYSID)
        {     
            var query = from angkalk in context.ANGKALK
                        where angkalk.SYSANGEBOT == SYSID
                        select angkalk;
            return query.FirstOrDefault< ANGKALK>();
        }

        public static  IT GetITFromAngebot(DdOlExtended context, long? SYSIT)
        {

            var query = from it in context.IT
                        where it.SYSIT == SYSIT
                        select it;
            
            return query.FirstOrDefault< IT>();
        }

     

        #endregion
    }
}