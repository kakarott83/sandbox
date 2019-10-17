//OWNER WB 28.06.2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    #endregion

    public static class WFMKATHelper
    {
        #region Private Contstants
        public const string CnstWfmmkatNameAngebot = "Preiskarte";
        public const string CnstWfmmkatNameSonderkalkulationVerkaeufer = "Sonderkalkulation Verkäufer";
        public const string CnstWfmmkatNameSonderkalkulationInnendiest = "Sonderkalkulation Innendienst";
        public const string CnstWfmmkatNameAngebotSubmit = "Angebot Einreichung Kommentar";
        #endregion


        public static WFMMKAT DeliverWfmkat(DdOwExtended context, string kategorieBezeichnung)
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

        public static WFMMKAT DeliverWfmkatForAngebot(DdOwExtended context)
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
