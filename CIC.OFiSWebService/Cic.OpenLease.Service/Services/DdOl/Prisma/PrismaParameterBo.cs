using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Dynamic;

using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;


namespace Cic.OpenLease.Service
{
    /// <summary>
    /// BO for Prisma Product (including Parameter and Availability) access
    /// </summary>
    public class PrismaParameterBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ThreadSafeDictionary<ConditionLinkType, string> mapConditionLinkToParameter = new ThreadSafeDictionary<ConditionLinkType, string>();//Maps ConditionLinkType to the given parameter name

        private ThreadSafeDictionary<ConditionLinkType, long> mapConditionLinkToPRSETArea = new ThreadSafeDictionary<ConditionLinkType, long>();//Maps ConditionLinkType to the area id
        private Dictionary<long, ConditionLinkType> mapAreaToConditionLink = new Dictionary<long, ConditionLinkType>();

        private DateTime nullDate = new DateTime(1800, 1, 1);

        private ConditionLinkType[] supportedLinks;
        private bool debug = false;

        /// <summary>
        /// Different supported Configurations for Prisma Parameter Condition Links
        /// </summary>

        public static ConditionLinkType[] CONDITIONS_BMW = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.VART};
        public static ConditionLinkType[] CONDITIONS_BANKNOW = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP };

        public static ConditionLinkType[] CONDITIONS = CONDITIONS_BANKNOW;

        private IPrismaDao pDao;
        private IObTypDao obDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obDao"></param>
        public PrismaParameterBo(IPrismaDao pDao, IObTypDao obDao)
         
        {
            this.pDao = pDao;
            this.obDao = obDao;
            this.supportedLinks = CONDITIONS;

            //map the condition link type to the prisma context field name
            mapConditionLinkToParameter[ConditionLinkType.BCHNL] = "sysprchannel";
            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToParameter[ConditionLinkType.PRHGROUPEXT] = "sysperole";
            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToParameter[ConditionLinkType.USETYPE] = "sysprusetype";
            mapConditionLinkToParameter[ConditionLinkType.KDTYP] = "syskdtyp";
            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";
            mapConditionLinkToParameter[ConditionLinkType.PRODUCT] = "sysprproduct";
            mapConditionLinkToParameter[ConditionLinkType.VART] = "sysvart";
            mapConditionLinkToParameter[ConditionLinkType.VTTYP] = "sysvttyp";

            /*
             *  001 = LS (Mandant)
                002 = BRAND
                003 = PRCHANNEL
                010 = VART
                011 = VARTTAB
                012 = VTTYP
                013 = KALKTYP
                014 = PRPRODUCT
                020 = PRHGROUP
                021 = PEROLE
                030 = OBART
                031 = OBTYP
                040 = PRKGROUP
                041 = KDTYP
                050 = FSTYP
                060 = VSTYP
                070 = PRUSETYPE
                99999 = undefined
             */
            mapConditionLinkToPRSETArea[ConditionLinkType.BRAND] = 2;
            mapConditionLinkToPRSETArea[ConditionLinkType.BCHNL] = 3;
            mapConditionLinkToPRSETArea[ConditionLinkType.PRHGROUP] = 20;
            mapConditionLinkToPRSETArea[ConditionLinkType.PRKGROUP] = 40;
            mapConditionLinkToPRSETArea[ConditionLinkType.NONE] = 99999;
            mapConditionLinkToPRSETArea[ConditionLinkType.OBTYP] = 31;
            mapConditionLinkToPRSETArea[ConditionLinkType.OBART] = 30;
            mapConditionLinkToPRSETArea[ConditionLinkType.USETYPE] = 70;
            mapConditionLinkToPRSETArea[ConditionLinkType.LS] = 1;
            mapConditionLinkToPRSETArea[ConditionLinkType.VART] = 10;
            mapConditionLinkToPRSETArea[ConditionLinkType.VARTTAB] = 11;
            mapConditionLinkToPRSETArea[ConditionLinkType.VTTYP] = 12;
            mapConditionLinkToPRSETArea[ConditionLinkType.KALKTYP] = 13;
            mapConditionLinkToPRSETArea[ConditionLinkType.PRODUCT] = 14;
            mapConditionLinkToPRSETArea[ConditionLinkType.PEROLE] = 21;
            mapConditionLinkToPRSETArea[ConditionLinkType.KDTYP] = 41;
            mapConditionLinkToPRSETArea[ConditionLinkType.FSTYP] = 50;
            mapConditionLinkToPRSETArea[ConditionLinkType.VSTYP] = 60;
            //What is 61????
            mapConditionLinkToPRSETArea[ConditionLinkType.INTTYPE] = 9999;//not yet used
            mapConditionLinkToPRSETArea[ConditionLinkType.COMMON] = 99999;

            //Maps the  db-value to the ConditionLink-Type
            foreach (ConditionLinkType k in mapConditionLinkToPRSETArea.Keys)
            {
                mapAreaToConditionLink[mapConditionLinkToPRSETArea[k]] = k;
            }
        }

        /// <summary>
        /// gets the parameters without steplist
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<PRPARAMDto> getParameters(prKontextDto context)
        {
            DateTime perDate = context.perDate;
            if (perDate == null || perDate.Year < nullDate.Year) perDate = DateTime.Now;

            if (debug)
                _log.Debug("listAvailableParams: " + _log.dumpObject(context) + " " + perDate);

           

            IEnumerable<long> parsets = new List<long>();
            IEnumerable<long> prparsets = new List<long>();

            //common parameter sets-tree--------------------------------------------------------------------------------------
            List<ParameterSetConditionLink> allparamSetLinks = pDao.getParamSets();
            List<ParameterSetConditionLink> allparamSetLinksComplete = new List<ParameterSetConditionLink>(allparamSetLinks);

            allparamSetLinks = (from a in allparamSetLinks
                                where                                                                     
                                    (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                &&
                                    (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                orderby a.area
                                select a).ToList();
            if (debug)
                _log.Debug("Amount of all ParamSets for this product: " + allparamSetLinks.Count);

            List<DefaultTreeNode> nodeList = new List<DefaultTreeNode>();
            foreach (ParameterSetConditionLink lnk in allparamSetLinks)
            {
                DefaultTreeNode node = new DefaultTreeNode();
                node.sysid = lnk.sysprparset;
                node.sysparent = lnk.sysparent;
                if (lnk.sysparent == 0) node.sysparent = null;
                node.setData(lnk);
                nodeList.Add(node);
            }
            DefaultTreeNode treeRoot = AbstractSortTreeBo.buildTree(nodeList);
            PreorderEnumeration pe = AbstractSortTreeBo.createPreorderEnumeration(treeRoot);
            List<long> invalidNodes = new List<long>();
            /*
             * Vereinbarung:
             * alle paramsets einer Ebene (mit gleicher parentid) gelten nur für eine bestimmte area
             * area 99999 gilt dann für ebenjene area, wenn keine der anderen sets zum Kontext passt
             */
            while (pe.MoveNext())
            {
                DefaultTreeNode node = (DefaultTreeNode)pe.Current;
                ParameterSetConditionLink lnk = (ParameterSetConditionLink)node.getData();
                if (lnk == null) continue;
                if (!mapAreaToConditionLink.ContainsKey(lnk.area)) continue;

                if (node.getPath().Intersect(invalidNodes).ToList().Count > 0) continue;//dont use a node if its tree-path contains elements marked as invalid

                ConditionLinkType ctype = mapAreaToConditionLink[lnk.area];
                if (!supportedLinks.Contains(ctype)) continue;
                if (!mapConditionLinkToPRSETArea.ContainsKey(ctype)) continue;//avoid new LinkTypes not yet mapped

                //_log.Debug("Parent: " + lnk.sysparent + " id: " + lnk.sysprparset + " area: " + lnk.area);
                try
                {
                    List<long> ContextList = ConditionLink.getParameter(context, ctype, mapConditionLinkToParameter, obDao, perDate);

                    //wenn im Kontext 0 (undefiniert) geschickt wird, wird üblicherweise der link akzeptiert, da es keine Einschränkung gibt (=0)
                    //wenn es nun aber in der gleichen ebene eine parametergruppe mit area 99999(=common) gibt, dann soll diese verwendet werden, wenn im Kontext 0 stand
                    //ausser auf der obersten ebene, hier diese behandlung nicht durchführen!
                    bool checkCommon = ContextList.Count == 1 && ContextList[0] == 0;
                    if (checkCommon && lnk.sysparent != 0) //#2012070910000073
                    {
                        List<ITreeNode> siblings = new List<ITreeNode>(node.getParent().getChildren());
                        bool foundCommon = false;
                        foreach (ITreeNode rnode in siblings)
                        {
                            ParameterSetConditionLink clink = (ParameterSetConditionLink)((DefaultTreeNode)rnode).getData();
                            if (clink.area == mapConditionLinkToPRSETArea[ConditionLinkType.COMMON])
                            {
                                lnk = clink;
                                node = (DefaultTreeNode)rnode;
                                foundCommon = true;
                                break;
                            }
                        }
                        if (!foundCommon)//#2012070910000073
                            continue;
                    }
                    if (!getSetAvailability((List<long>)parsets, lnk, ContextList))//adds the parset as valid
                    {
                        //or disables the tree beneath!
                        invalidNodes.Add(node.sysid);
                    }
                    else
                    {
                        if (ctype != ConditionLinkType.COMMON)//wenn ein nicht-allgemeiner zutrifft, alle siblings (die für die gleiche area gelten müssen, nicht mehr verarbeiten
                        {
                            List<ITreeNode> siblings = new List<ITreeNode>(node.getParent().getChildren());
                            siblings.Remove(node);
                            foreach (ITreeNode rnode in siblings)
                            {
                                invalidNodes.Add(((DefaultTreeNode)rnode).sysid);
                            }
                        }
                    }
                }
                catch (MethodAccessException ex)
                {
                    //explicitely not handled, we ignore the parameter
                    _log.Debug("Explicitly unhandled Exception: " + ex.Message);
                }

            }
            //common parameter sets-tree END--------------------------------------------------------------------------------------



            //parameter sets for product--------------------------------------------------------------------------------------
            //get all paramsets, valid and for product
            //for all paramsets check conditions in this order:
            /*area 
            99		none
            0	sysbrand
            1	sysobtyp
            3	sysprkgroup
            2	sysprhgroup*/
            List<ParameterConditionLink> allparamLinks = pDao.getParamConditionLinks();//prparset

            allparamLinks = (from a in allparamLinks
                             where a.sysprproduct == (long)context.sysprproduct &&
                             (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                              && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                             select a).ToList();
            if (debug)
                _log.Debug("Amount of all ParamSetLinks for this product: " + allparamLinks.Count);
            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {

                long area = mapConditionLinkToPRSETArea[ctype];
                try
                {
                    List<long> ContextList = ConditionLink.getParameter(context, ctype, mapConditionLinkToParameter, obDao, perDate);
                    prparsets = getAvailability(prparsets, allparamLinks, ContextList, perDate, area);
                }
                catch (MethodAccessException ex)
                {
                    //explicitely not handled, we ignore the parameter
                    _log.Debug("Explicitly unhandled Exception: " + ex.Message);
                }
            }



            //parsets now contains the sysprparset of all params 
            //parameter sets for product END--------------------------------------------------------------------------------------



            //Sort the resulting parameters. The parameter-sets-link-list is ordered ascending (last overwrites the first)
            List<long> resultParSets = new List<long>();
            foreach (ParameterSetConditionLink link in allparamSetLinksComplete)
            {
                if (parsets.Contains(link.sysprparset))
                    resultParSets.Add(link.sysprparset);
            }
            foreach (ParameterConditionLink link in allparamLinks)
            {
                if (parsets.Contains(link.sysprparset))
                    resultParSets.Add(link.sysprparset);
            }

            //copy each paramsets' parameter-attribute over each other if value not null


            //Sort the Produkt resulting parameters. The parameter-sets-link-list is ordered ascending (last overwrites the first)
            foreach (ParameterSetConditionLink link in allparamSetLinksComplete)
            {
                if (prparsets.Contains(link.sysprparset))
                    resultParSets.Add(link.sysprparset);
            }
            foreach (ParameterConditionLink link in allparamLinks)
            {
                if (prparsets.Contains(link.sysprparset))
                    resultParSets.Add(link.sysprparset);
            }

            List<PRPARAMDto> allParams = pDao.getParams();
            List<PRPARAMDto> rval = new List<PRPARAMDto>();

            _log.Info("PARAMETERSETS (" + resultParSets.Count + "): " + String.Join(",", resultParSets.ToArray()));

            //copy each paramsets' parameter-attribute over each other if value not null
            foreach (long sysprparset in resultParSets)
            {

                List<PRPARAMDto> pars = (from a in allParams
                                       where a.SYSPRPARSET == sysprparset
                                       select a).ToList();
                _log.Info("PARAMETER AUS PARSET " + sysprparset + ": " + String.Join(",", pars.Select(a => a.SYSPRPARAM).ToArray()));
                if (pars.Count == 0) continue;
                overwriteParameters(rval, pars);
            }

         
            return rval;
        }



        /// <summary>
        /// gets an available prisma parameter for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>

        public PRPARAMDto getParameter(prKontextDto context, String objectmeta)
        {
            List<PRFLD> fields = pDao.getFields();
            fields = fields.Where(f => f.OBJECTMETA != null && f.OBJECTMETA.Equals(objectmeta)).ToList();
            if (fields == null || fields.Count == 0)
            {
                _log.Warn("Prisma Field " + objectmeta + " not found");
                return null;
            }
            if (fields.Count > 1)
            {
                _log.Warn("More than one Prisma Field " + objectmeta + " for Product " + context.sysprproduct + " found");
            }
            PRFLD field = fields[0];

            List<PRPARAMDto> rval = getParameters(context);

            PRPARAMDto param = rval.Where(r => r.SYSPRFLD == field.SYSPRFLD).FirstOrDefault();
            return param;
        }

        /// <summary>
        /// gets an available prisma parameter for the given field code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectmeta"></param>
        /// <returns></returns>
        
        public long getFieldID(prKontextDto context, String objectmeta)
        {
            List<PRFLD> fields = pDao.getFields();
            fields = fields.Where(f => f.OBJECTMETA != null && f.OBJECTMETA.Equals(objectmeta)).ToList();
            if (fields == null || fields.Count == 0)
            {
                _log.Warn("Prisma Field " + objectmeta + " not found");
                return 0;
            }
            if (fields.Count > 1)
            {
                _log.Warn("More than one Prisma Field " + objectmeta + " for Product " + context.sysprproduct + " found");
            }
            PRFLD field = fields[0];

            return fields[0].SYSPRFLD;
        }

        /// <summary>
        /// Returns a List of Available Prisma Parameters
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public List<PRPARAMDto> listAvailableParameter(prKontextDto context)
        {
            // Händler aus Employee-Kennung auslesen
            // potentielle Fehlerquelle, da context.sysperole als user-perole angenommen wird und hier umgebogen wird
            // entweder einen weiteren Parameter in context hinzufügen (hdsysperole)
            // oder von AUSSEN bereits die gewünschte perole korrekt setzen -> wird nun so gemacht!
            //context.sysperole = obDao.getHaendlerByEmployee(context.sysperole);

            //Liste mit Parametern auslesen
            List<PRPARAMDto> rval = getParameters(context);
            //set steplist
           /* foreach (ParamDto param in rval)
            {
                if (param.stepsize == 0 && (param.steplistcsv == null || param.steplistcsv.Length == 0)) continue;
                if (param.type == 0) //num
                {
                    if (param.stepsize > 0)
                    {
                        List<SteplistDto> steps = new List<SteplistDto>();
                        for (double t = param.minvaln; t <= param.maxvaln; t += param.stepsize)
                        {
                            SteplistDto step = new SteplistDto();
                            step.stepval = (long)t;
                            steps.Add(step);
                        }
                        param.steplist = steps.ToArray();
                    }
                    else
                    {
                        List<SteplistDto> steps = new List<SteplistDto>();
                        string[] ssteps = param.steplistcsv.Split(';');
                        foreach (String sstep in ssteps)
                        {
                            SteplistDto step = new SteplistDto();
                            try
                            {
                                step.stepval = long.Parse(sstep);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Wrong Steplist Separator in Param " + param.sysID + ", only semicolon allowed!");
                            }
                            steps.Add(step);
                        }
                        param.steplist = steps.ToArray();
                    }
                }
                else if (param.type == 1) //percent
                {
                    if (param.stepsize > 0)
                    {
                        List<SteplistDto> steps = new List<SteplistDto>();
                        for (double t = param.minvalp; t <= param.maxvalp; t += param.stepsize)
                        {
                            SteplistDto step = new SteplistDto();
                            step.stepval = (long)t;
                            steps.Add(step);
                        }
                        param.steplist = steps.ToArray();
                    }
                    else
                    {
                        List<SteplistDto> steps = new List<SteplistDto>();
                        string[] ssteps = param.steplistcsv.Split(';');
                        foreach (String sstep in ssteps)
                        {
                            SteplistDto step = new SteplistDto();
                            try
                            {
                                step.stepval = long.Parse(sstep);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Wrong Steplist Separator in Param " + param.sysID + ", only semicolon allowed!");
                            }

                            steps.Add(step);
                        }
                        param.steplist = steps.ToArray();

                    }
                }
            }*/
            return rval;

        }


        /// <summary>
        /// overwrites all identical ParamDto in rparam with items from newParams or adds missing items from newParams
        /// </summary>
        /// <param name="rparam">Result </param>
        /// <param name="newParams">Input Items</param>
        private void overwriteParameters(List<PRPARAMDto> rparam, List<PRPARAMDto> newParams)
        {
            foreach (PRPARAMDto np in newParams)
            {
                var q = from PRPARAMDto p in rparam
                        where p.PRFLDOBJECTMETA == np.PRFLDOBJECTMETA
                        select p;
                PRPARAMDto cp = q.FirstOrDefault();
                if (cp == null)
                {
                    rparam.Add(np);
                    continue;
                }
                else
                {
                    rparam.Remove(cp);
                    rparam.Add(np);
                }
            }
        }



        /// <summary>
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailability(IEnumerable<long> resultLinks, List<ParameterConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate, long area)
        {
            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID) && a.area == area

                                               select (long)a.TARGETID).ToList();
            if (debug)
                _log.Debug("Assigned Product Paramsets: " + assignedConditionIds.Count);

            return resultLinks.Union(assignedConditionIds).Distinct();
        }

        /// <summary>
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private IEnumerable<long> getSetAvailability(IEnumerable<long> resultLinks, List<ParameterSetConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate, long area)
        {
            if (debug)
            {
                foreach (ParameterSetConditionLink a in allConditionLinks)
                {
                    _log.Debug("Link: " + a.CONDITIONID + "  Area: " + a.area);
                }
            }
            bool nofilter = conditionKeys.Count == 1 && conditionKeys[0] == 0;

            //this list gets filtered because the link does not match to the given context parameter value
            List<ParameterSetConditionLink> filteredConditionIds = (from a in allConditionLinks
                                                                    where !conditionKeys.Contains(a.CONDITIONID) && !nofilter && a.area == area
                                                                    select a).ToList();
            List<long> removeIds = (from a in filteredConditionIds
                                    select a.sysprparset).ToList();

            //now filter all children of this node, too!
           /* foreach (long rid in removeIds)
            {
                List<ParameterSetConditionLink> children = pDao.getParamSetChildren(rid);
                if (children == null || children.Count == 0) continue;

                List<long> removeNodes = (from a in children
                                          select a.sysprparset).ToList();
                foreach (long rnode in removeNodes)
                {
                    allConditionLinks.RemoveAll(a => a.sysprparset == rnode);
                }
            }*/

            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where (conditionKeys.Contains(a.CONDITIONID) || nofilter) && a.area == area

                                               select (long)a.TARGETID).ToList();
            if (debug)
                _log.Debug("Assigned Paramsets: " + assignedConditionIds.Count);

            return resultLinks.Union(assignedConditionIds).Distinct();
        }

        /// <summary>
        /// Determines if the parameterset-Link is valid for the given context value
        /// If the context has a value of 0, the link is always valid!
        /// This 'wildcard' is only used at this place in prisma, elsewhere it would cause the link to be filtered!
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="link"></param>
        /// <param name="conditionKeys"></param>
        /// <returns></returns>
        private bool getSetAvailability(List<long> resultLinks, ParameterSetConditionLink link, List<long> conditionKeys)
        {
            //hier den link akzeptieren, wenn im Kontext 0 (undefiniert) geschickt wird
            bool nofilter = conditionKeys.Count == 1 && conditionKeys[0] == 0;
            bool rval = false;
            if (conditionKeys.Contains(link.CONDITIONID) || nofilter)
            {
                rval = true; //link is valid!
                if (!resultLinks.Contains(link.TARGETID))
                    resultLinks.Add(link.TARGETID);
            }
            return rval;
        }


    }


}
