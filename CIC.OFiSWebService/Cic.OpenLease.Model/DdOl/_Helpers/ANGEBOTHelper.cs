// OWNER WB, 10-03-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    
    using System.Collections.Generic;
    using Cic.Basic.Data.Objects;
    using Cic.OpenLease.Model.DdOw;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public static class ANGEBOTHelper
    {
        #region Methods
        public static bool Contains(OlExtendedEntities olExtendedEntities, long sysID)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new Exception("olExtendedEntities");
            }

            return olExtendedEntities.ANGEBOT.Where(par => par.SYSID == sysID).Any<ANGEBOT>();
        }

        public static Cic.OpenLease.Model.DdOl.ANGEBOT GetAngebot(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSID)
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

        public static Cic.OpenLease.Model.DdOl.ANGKALK GetAngkalkFromAngebot(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSID)
        {     
            var query = from angkalk in context.ANGKALK
                        where angkalk.SYSANGEBOT == SYSID
                        select angkalk;
            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ANGKALK>();
        }

        public static Cic.OpenLease.Model.DdOl.IT GetITFromAngebot(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long? SYSIT)
        {

            var query = from it in context.IT
                        where it.SYSIT == SYSIT
                        select it;
            
            return query.FirstOrDefault<Cic.OpenLease.Model.DdOl.IT>();
        }

     

        #endregion
    }
}