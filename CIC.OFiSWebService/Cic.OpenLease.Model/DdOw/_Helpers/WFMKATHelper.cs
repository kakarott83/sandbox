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

    public static class WFMKATHelper
    {
        #region Private Contstants
        public const string CnstWfmmkatNameAngebot = "Preiskarte";
        public const string CnstWfmmkatNameSonderkalkulationVerkaeufer = "Sonderkalkulation Verkäufer";
        public const string CnstWfmmkatNameSonderkalkulationInnendiest = "Sonderkalkulation Innendienst";
        public const string CnstWfmmkatNameAngebotSubmit = "Angebot Einreichung Kommentar";
        #endregion


        public static WFMMKAT DeliverWfmkat(DdOw.OwExtendedEntities context, string kategorieBezeichnung)
        {
            WFMMKAT WFMKAT;
            try
            {
                var WfmmkatQuery = from wfmmkat in context.WFMMKAT
                                   where wfmmkat.BESCHREIBUNG == kategorieBezeichnung
                                   orderby wfmmkat.SYSWFMMKAT descending
                                   select wfmmkat;
                WFMKAT = WfmmkatQuery.FirstOrDefault();

            }
            catch
            {
                throw new Exception("Error during getting WFMKAT for " + kategorieBezeichnung);
            }

            return WFMKAT;
        }

        public static WFMMKAT DeliverWfmkatForAngebot(DdOw.OwExtendedEntities context)
        {
            WFMMKAT WFMKAT;
            try
            {
                var WfmmkatQuery = from wfmmkat in context.WFMMKAT
                                   where wfmmkat.BESCHREIBUNG == CnstWfmmkatNameAngebot
                                   orderby wfmmkat.SYSWFMMKAT descending
                                   select wfmmkat;
               WFMKAT =  WfmmkatQuery.FirstOrDefault();

            }
            catch
            {
                throw new Exception("Error during getting WFMKAT");
            }

            return WFMKAT;
        }
    }
}
