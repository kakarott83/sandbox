using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// BO for Prisma Product (including Parameter and Availability) access
    /// </summary>
    public class PrismaServiceBo : AbstractPrismaServiceBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private Dictionary<ConditionLinkType, string> mapConditionLinkToParameter = new Dictionary<ConditionLinkType, string>();
        private Dictionary<ConditionLinkType, string> mapConditionLinkToEntity = new Dictionary<ConditionLinkType, string>();

        private static ConditionLinkType[] supportedLinks = { ConditionLinkType.PRKGROUP, ConditionLinkType.OBART, ConditionLinkType.OBTYP, ConditionLinkType.KDTYP };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obDao"></param>
        /// <param name="translateDao">Translate DAO</param>
        public PrismaServiceBo(IPrismaServiceDao pDao, IObTypDao obDao, ITranslateDao translateDao)
            : base(pDao, obDao, translateDao)
        {
            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToEntity[ConditionLinkType.PRKGROUP] = "prclvstypkgrp";

            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToEntity[ConditionLinkType.OBART] = "prclvstypobart";

            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToEntity[ConditionLinkType.OBTYP] = "prclvstypobtyp";

            mapConditionLinkToParameter[ConditionLinkType.KDTYP] = "syskdtyp";
            mapConditionLinkToEntity[ConditionLinkType.KDTYP] = "prclvstypkdtyp";
        }


        /// <summary>
        /// Returns a List of Available Products
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns></returns>
        override
        public List<AvailableServiceDto> listAvailableServices(srvKontextDto context, string isoCode)
        {
            // Übersetzungen einlesen
            List<CTLUT_Data> translations = base.translateDao.readoutTranslationList("'VSTYP', 'RSVTYP', 'FSTYP'", isoCode);

            //DateTime perDate = context.perDate;
            //if (perDate == null || perDate.Year < nullDate.Year) perDate = DateTime.Now;

            DateTime perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(context.perDate);

            _log.Debug("listAvailableServices: " + _log.dumpObject(context));
            // Link informationen der Verknüpfung Produkt/Versicherungstyp holen.
            List<PRVSDto> allServices = pDao.getVSTYPForProduct(perDate, context.sysprprodukt);
            _log.Debug("Amount of all Services: " + allServices.Count);

            List<AvailableServiceDto> rval = new List<AvailableServiceDto>();
            // alle verfügbaren Dienste holen
            getAvailableServices(rval, allServices, context, perDate);
            PRRSVCODE code = pDao.getPrrsvCodeByPrProdukt(perDate,context.sysprprodukt);

            // Übersetzen
            foreach (AvailableServiceDto item in rval)
            {
                // Ticket#2012102610000034 : falsche Texte in Ratenabsicherung
                // fstyp gibts nicht und rsvtyp wurde nicht mehr verwendet
                CTLUT_Data Translation = translateDao.RetrieveEntry(item.sysID, "VSTYP", translations);
                if (Translation != null)
                {
                    item.bezeichnung = Translation.Bezeichnung;
                    item.code = Translation.Name;
                    item.beschreibung = Translation.Description;
                }
                item.paketCode = code.CODE;
            }
            _log.Debug("Services returned: " + rval.Count);

            return rval;
        }

        /// <summary>
        /// checks the availabilites for the given service-ids
        /// </summary>
        /// <param name="rval">return value</param>
        /// <param name="allServices">all service-ids to check</param>
        /// <param name="context"></param>
        /// <param name="perDate"></param>
        private void getAvailableServices(List<AvailableServiceDto> rval, List<PRVSDto> allServices, srvKontextDto context, DateTime perDate)
        {
            // Alle Versicherungs ID's auslesen
            IEnumerable<long> allServiceIds = (from a in allServices
                                               where a.SYSPRPRODUCT == context.sysprprodukt && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                               select a.SYSVSTYP).ToList();

            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {
                String tableName = mapConditionLinkToEntity[ctype];

                // alle Service Condition Links auslesen.
                List<ServiceConditionLink> allConditions = pDao.getServiceConditionLinks(tableName);

                _log.Debug("Conditionlinks to " + tableName + ": " + allConditions.Count);

                // Wenn keine Konditionen gegeben sind, gelten alle, nichts wird ausgesiebt.
                if (allConditions.Count != 0)
                {
                    // Werte aus dem Kontext auslesen
                    List<long> ConditionKeys = ConditionLink.getParameter(context, ctype, mapConditionLinkToParameter, obDao, perDate);
                    // Verfügbarkeit prüfen
                    allServiceIds = getAvailability(allServiceIds, allConditions, ConditionKeys, perDate);
                }

            }


            // Versicherungstyp auslesen
            List<VSTYP> allInsurances = pDao.getVSTYP();

            foreach (long pkey in allServiceIds)
            {
                var q = from p in allServices
                        where p.SYSVSTYP == pkey
                        select p;

                PRVSDto prvs = q.FirstOrDefault();

                var vsq = from p in allInsurances
                          where p.SYSVSTYP == pkey
                          select p;

                VSTYP vs = vsq.FirstOrDefault();
                if (vs == null)
                {
                    _log.Warn("No SYSVSTYP " + pkey + " found");
                    continue;
                }

                //assign the source for selected/editable state - currently the insurance position is the source for the editable flag
                prvs.FLAGDEFAULT = vs.FLAGDEFAULT.HasValue ? 0 : vs.FLAGDEFAULT.Value;
                prvs.EDITABLE = prvs.DISABLEDPOS == 0;
                prvs.CODE = vs.CODE;
                prvs.BEZEICHNUNG = vs.BEZEICHNUNG;
                prvs.BESCHREIBUNG = vs.BESCHREIBUNG;
                if (vs.SYSVSART.HasValue)
                {
                    VSART vsa = pDao.getVSART(vs.SYSVSART.Value);
                    if (vsa != null)
                    {
                        if (ServiceType.RIP.ToString().Equals(vsa.CODE))
                            prvs.SERVICETYPE = ServiceType.RIP;
                        else if (ServiceType.AUA.ToString().Equals(vsa.CODE))
                            prvs.SERVICETYPE = ServiceType.AUA;
                    }
                }
                rval.Add(Mapper.Map<PRVSDto, AvailableServiceDto>(prvs));
            }
        }

        /// <summary>
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// </summary>
        /// <param name="allTargetItems"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailability(IEnumerable<long> allTargetItems, List<ServiceConditionLink> allConditionLinks,
                                                  List<long> conditionKeys, DateTime perDate)
        {
            foreach (ServiceConditionLink a in allConditionLinks)
            {
                _log.Debug("Link: " + a.CONDITIONID);
            }

            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID)
                                                 && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                                 && (a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1)
                                               select a.TARGETID).ToList();
            _log.Debug("Assigned Services: " + assignedConditionIds.Count);

            return allTargetItems.Union(assignedConditionIds).Distinct();
        }
    }
}