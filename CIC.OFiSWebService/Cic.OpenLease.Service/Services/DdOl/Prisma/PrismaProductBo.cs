using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Dynamic;


using System.Diagnostics;

using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// BO for Prisma Product (including Parameter and Availability) access
    /// </summary>
    /*public class PrismaProductBo 
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);

        private ThreadSafeDictionary<ConditionLinkType, string> mapConditionLinkToParameter = new ThreadSafeDictionary<ConditionLinkType, string>();
        private ThreadSafeDictionary<ConditionLinkType, string> mapConditionLinkToEntity = new ThreadSafeDictionary<ConditionLinkType, string>();

        private ConditionLinkType[] supportedLinks;
        private bool debug = false;
        private bool IDEDebug = true;
        //TODO11 AIDA11
        /// <summary>
        /// Different supported Configurations for Prisma Product Condition Links
        /// </summary>
        
        public static ConditionLinkType[] CONDITIONS_BMW = { ConditionLinkType.BCHNL, ConditionLinkType.BRAND, ConditionLinkType.PRPEROLE, ConditionLinkType.PRHGROUP, ConditionLinkType.OBART, ConditionLinkType.OBTYP, ConditionLinkType.MART };
        public static ConditionLinkType[] CONDITIONS_BANKNOW = { ConditionLinkType.PRHGROUPEXT, ConditionLinkType.BRAND, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.USETYPE, ConditionLinkType.KDTYP, ConditionLinkType.OBTYP };

        public static ConditionLinkType[] CONDITIONS = CONDITIONS_BANKNOW;

        private IPrismaDao pDao;
        private IObTypDao obDao;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obDao"></param>
   
      
        public PrismaProductBo(IPrismaDao pDao, IObTypDao obDao)
           
        {
            this.pDao = pDao;
            this.obDao = obDao;

            this.supportedLinks = CONDITIONS;

            mapConditionLinkToParameter[ConditionLinkType.PRPEROLE] = "sysvpperole";
            mapConditionLinkToEntity[ConditionLinkType.PRPEROLE] = "prclprpe";

            mapConditionLinkToParameter[ConditionLinkType.MART] = "sysprmart";
            mapConditionLinkToEntity[ConditionLinkType.MART] = "prclprmart";

            mapConditionLinkToParameter[ConditionLinkType.BCHNL] = "sysprchannel";
            mapConditionLinkToEntity[ConditionLinkType.BCHNL] = "prclprbchnl";

            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToEntity[ConditionLinkType.BRAND] = "prclprbr";

            mapConditionLinkToParameter[ConditionLinkType.PRHGROUPEXT] = "sysperole";
            mapConditionLinkToEntity[ConditionLinkType.PRHGROUPEXT] = "prclprhg";

            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToEntity[ConditionLinkType.OBTYP] = "prclprob";

            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToEntity[ConditionLinkType.OBART] = "prclprobart";

            mapConditionLinkToParameter[ConditionLinkType.USETYPE] = "sysprusetype";
            mapConditionLinkToEntity[ConditionLinkType.USETYPE] = "prclprusetype";

            mapConditionLinkToParameter[ConditionLinkType.KDTYP] = "syskdtyp";
            mapConditionLinkToEntity[ConditionLinkType.KDTYP] = "prclprktyp";

            //not used for banknow:
            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToEntity[ConditionLinkType.PRKGROUP] = "prclprkg";
            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";
            mapConditionLinkToEntity[ConditionLinkType.PRHGROUP] = "prclprhg";

        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Product</returns>
        
        public PRPRODUCTDto getProduct(long sysprproduct)
        {
            return pDao.getProduct(sysprproduct);
        }

   
      

        /// <summary>
        /// Returns a List of Available Products filtered by conditiontypes given
        /// </summary>
        /// <param name="products"></param>
        /// <param name="conditiontypes"></param>
        /// <returns></returns>
        
        public List<PRPRODUCTDto> filterAvailableProducts(List<PRPRODUCTDto> products, long[] conditiontypes)
        {
            List<PRPRODTYPE> ptypes = pDao.getProductTypes();
            List<long> useTypes = new List<long>();
            foreach (PRPRODTYPE p in ptypes)
            {
                if (conditiontypes.Contains(p.CONDITIONTYPE.Value))
                    useTypes.Add(p.SYSPRPRODTYPE);

            }
            List<PRPRODUCTDto> result = new List<PRPRODUCTDto>();

            foreach (PRPRODUCTDto p in products)
            {
                if (useTypes.Contains(p.SYSPRPRODTYPE))
                    result.Add(p);
            }

            return result;
        }

        /// <summary>
        /// Returns a List of Available Products of a given type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="conditiontype"></param>
        /// <returns></returns>
        
        public List<PRPRODUCTDto> listAvailableProducts(prKontextDto context, long conditiontype)
        {
            PRPRODTYPE ptype = pDao.getProductTypes().Where(a => a.CONDITIONTYPE == conditiontype).FirstOrDefault();
            List<PRPRODUCTDto> rval = listAvailableProducts(context);

            if (ptype == null)
                throw new ArgumentException("PRPRODTYPE.conditiontype not available: " + conditiontype);

            var q = from p in rval
                    where p.SYSPRPRODTYPE == ptype.SYSPRPRODTYPE
                    select p;

            return q.ToList();
        }


        /// <summary>
        /// Returns a List of Available Products
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        
        public List<PRPRODUCTDto> listAvailableProducts(prKontextDto context)
        {
            DateTime perDate = context.perDate;
            if (perDate == null || perDate.Year < nullDate.Year) 
                perDate = DateTime.Now;
            if (debug)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug("listAvailableProducts: " + _log.dumpObject(context) + " " + perDate);
                if (IDEDebug)
                    Debug.WriteLine("listAvailableProducts: " + _log.dumpObject(context) + " " + perDate);
            }
            List<PRPRODUCTDto> allProducts = pDao.getProducts();
            if (debug)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug("Amount of all Products: " + allProducts.Count);
                if (IDEDebug)
                    Debug.WriteLine("Amount of all Products: " + allProducts.Count);
            }

            IEnumerable<long> allProductIds = (from a in allProducts
                                               where a.ACTIVEFLAG == 1 && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                               select a.SYSPRPRODUCT).ToList();
            List<long> aTIList = (List<long>)allProductIds;
            //Debugging only
            if (debug)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug("Produktliste (" + allProducts.Count + ")");
                if (IDEDebug)
                    Debug.WriteLine("Produktliste (" + allProducts.Count + ")");
                foreach (PRPRODUCTDto prod in allProducts)
                {
                    if (_log.IsDebugEnabled)
                        _log.Debug("Produkt: " + prod.NAME + ", " + prod.SYSPRPRODUCT);
                    if(IDEDebug)
                        Debug.WriteLine("Produkt: " + prod.NAME + ", " + prod.SYSPRPRODUCT);
                }
            }
            // Debugging only end

            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {
                String tableName = mapConditionLinkToEntity[ctype];

                List<ProductConditionLink> allConditions = pDao.getProductConditionLinks(tableName);
                if (debug)
                {
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("CTYPE: " + ctype.ToString());
                        _log.Debug("Conditionlinks to " + tableName + ": " + allConditions.Count);
                    }
                    if (IDEDebug)
                    {
                        Debug.WriteLine("CTYPE: " + ctype.ToString());
                        Debug.WriteLine("Conditionlinks to " + tableName + ": " + allConditions.Count);
                    }
                }
                // Debugging only
                if (debug)
                {
                    foreach (ProductConditionLink condLink in allConditions)
                    {
                        if (_log.IsDebugEnabled)
                            _log.Debug("Channel:" + condLink.sysbchannel + "Brand:" + condLink.sysbrand + "obart:" + condLink.sysobart + "obtyp:" + condLink.sysobtyp +
                                   "obusetype:" + condLink.sysobusetype + "hgroup:" + condLink.sysprhgroup + "ProductID:" + condLink.SYSPRPRODUCT);
                        if (IDEDebug)
                            Debug.WriteLine("Channel:" + condLink.sysbchannel + "Brand:" + condLink.sysbrand + "obart:" + condLink.sysobart + "obtyp:" + condLink.sysobtyp +
                                   "obusetype:" + condLink.sysobusetype + "hgroup:" + condLink.sysprhgroup + "ProductID:" + condLink.SYSPRPRODUCT);
                    }
                }
                // Debugging only end
                List<long> CondLinks = ConditionLink.getParameter(context, ctype, mapConditionLinkToParameter, obDao, perDate);
                if (context.sysprhgroup != 0 && ctype == ConditionLinkType.PRHGROUPEXT)
                {
                    IEnumerable<long> cLinks = CondLinks;
                    List<long> LimitList = new List<long> { context.sysprhgroup };
                    cLinks = cLinks.Intersect(LimitList).Distinct();
                    CondLinks = cLinks.Distinct().ToList();
                }
                int vorher = allProductIds.Count();
                String idsvorher = String.Join(",", allProductIds.ToArray());
                allProductIds = getAvailability(allProductIds, allConditions, CondLinks, perDate);
                _log.Info("Produkte vor " + ctype + "(" + vorher + "): " + idsvorher + " - nachher(" + allProductIds.Count() + "): " + String.Join(",", allProductIds.ToArray()));
                
            }

           
            // *) alle produkte die nicht in brandlinks vorhanden sind
            // *) die produkte die über brandlink verknüfpt sind
            //alle mit link, der übereinstimmt oder ohne einen link dieser art
            List<PRPRODUCTDto> rval = new List<PRPRODUCTDto>();
            foreach (long pkey in allProductIds)
            {
                var q = from p in allProducts
                        where p.SYSPRPRODUCT == pkey
                        select p;
                PRPRODUCTDto prod = q.FirstOrDefault();
                if (prod != null)
                    rval.Add(prod);
            }

            rval = (from f in rval
                    orderby f.RANGSL, f.NAME
                    select f).ToList();

            if (debug)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug("Services returned: " + rval.Count);
                if (IDEDebug)
                    Debug.WriteLine("Services returned: " + rval.Count);
            }
            return rval;
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
        private IEnumerable<long> getAvailability(IEnumerable<long> allTargetItems, List<ProductConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate)
        {
            // Debugging only
            if (debug)
            {
                foreach (ProductConditionLink link in allConditionLinks)
                {
                    if (_log.IsDebugEnabled)
                        _log.Debug("CLink: channelID: " + link.sysbchannel + " || brand: " + link.sysbrand + " || obart: " + link.sysobart + " || obtyp: " + link.sysobtyp + " || usetyp: " + link.sysobusetype + " || hgroup: " + link.sysprhgroup);
                    if (IDEDebug)
                        Debug.WriteLine("CLink: channelID: " + link.sysbchannel + " || brand: " + link.sysbrand + " || obart: " + link.sysobart + " || obtyp: " + link.sysobtyp + " || usetyp: " + link.sysobusetype + " || hgroup: " + link.sysprhgroup);
                }
            }
            // Debugging only end

            List<long> allConditionIds = (from a in allConditionLinks
                                          where (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                           && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                           && (a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1)
                                          select a.TARGETID).ToList();
            // Debugging only
            if (debug)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug("Excepted Products: " + allConditionIds.Count);
                if (IDEDebug)
                    Debug.WriteLine("Excepted Products: " + allConditionIds.Count);
                string exceptList = "";
                foreach (long cID in allConditionIds)
                {
                    if (exceptList.Length > 0)
                    {
                        exceptList += "; ";
                    }
                    exceptList += cID;
                }
                
                _log.Debug("Except-Liste:" + exceptList);
                foreach (ProductConditionLink a in allConditionLinks)
                {
                    foreach (long b in conditionKeys)
                    {
                        if (a.CONDITIONID == b)
                        {
                            _log.Debug("Accept ConditionKey: " + b + " Link: " + a.CONDITIONID + " Product:" + a.SYSPRPRODUCT);
                            if(IDEDebug)
                                Debug.WriteLine("Accept ConditionKey: " + b + " Link: " + a.CONDITIONID + " Product:" + a.SYSPRPRODUCT);
                        }
                        else
                        {
                            _log.Debug("Reject ConditionKey: " + b + " Link: " + a.CONDITIONID + " Product:" + a.SYSPRPRODUCT);
                            if (IDEDebug)
                                Debug.WriteLine("Reject ConditionKey: " + b + " Link: " + a.CONDITIONID + " Product:" + a.SYSPRPRODUCT);
                        }
                    }
                }
            }
            // Debugging only end

            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID) 
                                                 && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                                 && (a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1) 
                                               select a.TARGETID).ToList();
            // Debugging only
            if(debug)
            {
                _log.Debug("Assigned Products: " + assignedConditionIds.Count);
                if (IDEDebug)
                    Debug.WriteLine("Assigned Products: " + assignedConditionIds.Count);
                string KeepList = "";
                foreach (long cID in assignedConditionIds)
                {
                    if (KeepList.Length > 0)
                    {
                        KeepList += "; ";
                    }
                    KeepList += cID;
                }
                _log.Debug("ConditionId:" + KeepList);
                if (IDEDebug)
                    Debug.WriteLine("ConditionId:" + KeepList);
                List<long> Excepted = allTargetItems.Except(allConditionIds).ToList();
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
                if (IDEDebug)
                    Debug.WriteLine("Ausschlußliste:" + ExceptList);
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
                if (IDEDebug)
                    Debug.WriteLine("Ausgangsliste:" + InitialList);
                List<long> Intersected = assignedConditionIds.Intersect(allTargetItems).ToList();
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
                if (IDEDebug)
                    Debug.WriteLine("Schnittmenge:" + IntersectList);
                List<long> Unioned = Excepted.Union(Intersected).ToList();
                string UnionList = "";
                foreach (long cID in Unioned)
                {
                    if (UnionList.Length > 0)
                    {
                        UnionList += "; ";
                    }
                    UnionList += cID;
                }
                _log.Debug("Vereinigung:" + UnionList);
                if (IDEDebug)
                    Debug.WriteLine("Vereinigung:" + UnionList);
                List<long> Distincted = Excepted.Union(Intersected).ToList();
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
                if (IDEDebug)
                    Debug.WriteLine("Bereinigt:" + DistinctList);
            }

            // Debugging only end

            
           
            // HR: 6.6.2011
            // Durch Combine ersetzt wegen eines Union-Seiteneffekts.
            // Bugfix durch optimierte Version ersetzt
            return allTargetItems.Except(allConditionIds).Union(assignedConditionIds.Intersect(allTargetItems)).Distinct();
           
        }


    }*/
}
