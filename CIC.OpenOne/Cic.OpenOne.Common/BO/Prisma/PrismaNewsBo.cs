using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO;
using System.Reflection;
using System.Dynamic;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using AutoMapper;
using AutoMapper.Mappers;
using System.IO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// BO for Prisma Product (including Parameter and Availability) access
    /// </summary>
    public class PrismaNewsBo : AbstractPrismaNewsBO
    {
        private bool debug = false;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private String isoCode;
        private Dictionary<ConditionLinkType, string> mapConditionLinkToParameter = new Dictionary<ConditionLinkType, string>();
        private Dictionary<ConditionLinkType, string> mapConditionLinkToEntity = new Dictionary<ConditionLinkType, string>();

        private ConditionLinkType[] supportedLinks;

        /// <summary>
        /// Different supported Configurations for Prisma Product Condition Links
        /// </summary>
        public static ConditionLinkType[] CONDITIONS_BANKNOW = { ConditionLinkType.BRAND, ConditionLinkType.BCHNL, ConditionLinkType.PRHGROUP };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obDao"></param>
        /// <param name="supportedLinks"></param>
        /// <param name="isoCode">ISO code für Übersetzung</param>
        public PrismaNewsBo(IPrismaDao pDao, IObTypDao obDao, ConditionLinkType[] supportedLinks, String isoCode)
            : base(pDao, obDao)
        {
            this.isoCode = isoCode;
            this.supportedLinks = supportedLinks;

            mapConditionLinkToParameter[ConditionLinkType.BCHNL] = "sysprchannel";
            mapConditionLinkToEntity[ConditionLinkType.BCHNL] = "prclnewsbchnl";

            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToEntity[ConditionLinkType.BRAND] = "prclnewsbr";

            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";
            mapConditionLinkToEntity[ConditionLinkType.PRHGROUP] = "prclnewshg";

            mapConditionLinkToParameter[ConditionLinkType.PRHGROUPEXT] = "sysperole";
            mapConditionLinkToEntity[ConditionLinkType.PRHGROUPEXT] = "prclnewshg";

            //mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            //mapConditionLinkToEntity[ConditionLinkType.PRKGROUP] = "prclprkg";

            //mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            //mapConditionLinkToEntity[ConditionLinkType.OBTYP] = "prclprob";

            //mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            //mapConditionLinkToEntity[ConditionLinkType.OBART] = "prclprobart";

            //mapConditionLinkToParameter[ConditionLinkType.USETYPE] = "sysprusetype";
            //mapConditionLinkToEntity[ConditionLinkType.USETYPE] = "prclprusetype";

            //mapConditionLinkToParameter[ConditionLinkType.KDTYP] = "syskdtyp";
            //mapConditionLinkToEntity[ConditionLinkType.KDTYP] = "prclprktyp";

        }

        /// <summary>
        /// Get News entry
        /// </summary>
        /// <param name="sysprnews">ID</param>
        /// <returns>News</returns>
        override
        public PRNEWS getNews(long sysprnews)
        {
            return pDao.getNews(sysprnews);
        }


        /// <summary>
        /// Returns a List of Available News
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="deliverBinaries">Binaries mitliefern-Flag</param>
        /// <returns></returns>
        override
        public List<AvailableNewsDto> listAvailableNews(prKontextDto context, string isoCode, bool deliverBinaries)
        {
            
            DateTime perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(context.perDate);
            perDate = perDate.Date;
            _log.Debug("listAvailableNews: " + _log.dumpObject(context) + " " + perDate);
            // get ALL news
            List<PRNEWS> allNews = pDao.getNews();
            _log.Debug("Amount of all News: " + allNews.Count);

            // Extract ID's of Valid at this time (FROM/UNTIL)
            IEnumerable<long> allNewsIds = (from a in allNews
                                            where (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                              && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                            select a.SYSPRNEWS).ToList();


            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {
                String tableName = mapConditionLinkToEntity[ctype];

                List<NewsConditionLink> allConditions = pDao.getNewsConditionLinks(tableName);

                _log.Debug("Conditionlinks to " + tableName + ": " + allConditions.Count);
                if (ctype == ConditionLinkType.PRHGROUP)
                    allNewsIds = getAvailability(allNewsIds, allConditions, obDao.getPrhGroupsbyPerole(context.sysperole), perDate);
                
                else
                    allNewsIds = getAvailability(allNewsIds, allConditions, ConditionLink.getParameter(context, ctype, mapConditionLinkToParameter, obDao, perDate), perDate);
            }
            
            // Liste mit Newsdaten erstellen
            List<AvailableNewsDto> rval = new List<AvailableNewsDto>();
            // Durch alle als verfügbar geprüften Einträge laufen
            foreach (long pkey in allNewsIds)
            {
                // Dazugehörigen Nachrichteneintrag holen
                var q = from p in allNews
                        where p.SYSPRNEWS == pkey
                        select p;
                PRNEWS news = q.FirstOrDefault();
                // Sicherstellen das der Eintrag da ist
                if (news != null)
                {
                    // Nachrichtendaten auslesen
                    List<NEWS> newsData = pDao.getNewsData(news.SYSPRNEWS, isoCode);
                    // Sollten keine Nachrichtendaten da sein diesen Punkt übergehen
                    if (newsData == null) continue;
                    // Durch gefundene Daten laufen
                    foreach (NEWS n in newsData)
                    {

                        // Neuen DTO Eintrag erstellen
                        AvailableNewsDto rnews = new AvailableNewsDto();
                        if (news.DATUM != null)
                        {
                            rnews.datum = news.DATUM.Value;
                        }
                        if (news.VALIDFROM.HasValue)
                            rnews.validFrom = news.VALIDFROM.Value;
                        rnews.header = n.DESCRIPTION;
                        rnews.text = n.CONTENTLARGE;
                        if (rnews.text == null)
                            rnews.text = "";
                        int i = rnews.text.IndexOf('\0');
                        if (i >= 0) rnews.text = rnews.text.Substring(0, i);
                        rnews.sysID = n.SYSNEWS;

                        // Nachrichtenattribute auslesen
                        List<NEWSATT> attributes = pDao.getNewsAttributes(n.SYSNEWS);
                        if (attributes != null && attributes.Count > 0)
                        {
                            // Anhangliste erstellen
                            rnews.attachments = new List<AttachmentDto>();
                            // Anhänge hinzufügen
                            foreach (NEWSATT att in attributes)
                            {
                                AttachmentDto attachment = new AttachmentDto();
                                if(deliverBinaries)
                                    attachment.attachment = att.ATTACHMENT;
                                attachment.mimeType = Cic.OpenOne.Common.Util.MimeUtil.GetMIMEType(att.FORMAT);
                                if (att.FORMAT!=null && !att.FORMAT.Equals(""))
                                    attachment.dateiname = Path.GetFileName(att.FORMAT.ToLower());
                                attachment.bezeichnung = att.BEZEICHNUNG;
                                attachment.beschreibung = att.DESCRIPTION;
                                attachment.sysid = att.SYSNEWSATT;
                                rnews.attachments.Add(attachment);
                            }
                        }
                        // Nachrichteneintrag in die Ausgabe einhängen
                        rval.Add(rnews);
                    }
                }
            }




            _log.Debug("News returned: " + rval.Count);
            return rval;
        }



        /// <summary>
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// 
        /// allTargetItems.Except(allConditionIds) = Entfernen aller Einträge mit ConditionLink
        /// 
        /// </summary>
        /// <param name="allTargetItems"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailability(IEnumerable<long> allTargetItems, List<NewsConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate)
        {


            List<long> allConditionIds = (from a in allConditionLinks
                                          where (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                           && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                           && (a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1)
                                          select a.TARGETID).ToList();

            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID)
                                                 && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                                 && (a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1)
                                               select a.TARGETID).ToList();
            _log.Debug("Assigned Products: " + assignedConditionIds.Count);

            // Debugging only
            if (_log.IsDebugEnabled && debug)
            {
                string LeaveList = "";
                foreach (long cID in allConditionIds)
                {
                    if (LeaveList.Length > 0)
                    {
                        LeaveList += "; ";
                    }
                    LeaveList += cID;
                }
                _log.Debug("ExcludeList:" + LeaveList);
                string KeepList = "";
                foreach (long cID in assignedConditionIds)
                {
                    if (KeepList.Length > 0)
                    {
                        KeepList += "; ";
                    }
                    KeepList += cID;
                }
                _log.Debug("IncludeListe:" + KeepList);
                List<long> Excepted = allTargetItems.Except(allConditionIds).ToList();
                if (_log.IsDebugEnabled && debug)
                {
                    string ExceptList = "";
                    foreach (long cID in Excepted)
                    {
                        if (ExceptList.Length > 0)
                        {
                            ExceptList += "; ";
                        }
                        ExceptList += cID;
                    }
                    _log.Debug("Ausschlußliste:" + ExceptList);
                }
                if (_log.IsDebugEnabled && debug)
                {
                    string InitialList = "";
                    foreach (long cID in allTargetItems)
                    {
                        if (InitialList.Length > 0)
                        {
                            InitialList += "; ";
                        }
                        InitialList += cID;
                    }
                    _log.Debug("Ausgangsliste:" + InitialList);
                }
                List<long> Intersected = assignedConditionIds.Intersect(allTargetItems).ToList();
                if (_log.IsDebugEnabled && debug)
                {
                    string IntersectList = "";
                    foreach (long cID in Intersected)
                    {
                        if (IntersectList.Length > 0)
                        {
                            IntersectList += "; ";
                        }
                        IntersectList += cID;
                    }
                    _log.Debug("Schnittmenge:" + IntersectList);
                }
                List<long> Unioned = Excepted.Union(Intersected).ToList();
                if (_log.IsDebugEnabled && debug)
                {
                    string DistinctList = "";
                    foreach (long cID in Unioned)
                    {
                        if (DistinctList.Length > 0)
                        {
                            DistinctList += "; ";
                        }
                        DistinctList += cID;
                    }
                    _log.Debug("Vereinigung:" + DistinctList);
                }
                List<long> Distincted = Excepted.Union(Intersected).ToList();
                if (_log.IsDebugEnabled && debug)
                {
                    string DistinctList = "";
                    foreach (long cID in Distincted)
                    {
                        if (DistinctList.Length > 0)
                        {
                            DistinctList += "; ";
                        }
                        DistinctList += cID;
                    }
                    _log.Debug("Bereinigt:" + DistinctList);
                }
            }
            // allTargetItems.Except(allConditionIds) = Entfernen aller Einträge mit ConditionLink
            // Union(assignedConditionIds.Intersect(allTargetItems)) = Wieder hinzufügen aller positiv evaluierten Condition Links der Zielmenge
            return allTargetItems.Except(allConditionIds).Union(assignedConditionIds.Intersect(allTargetItems)).Distinct();
        }

    }





}
