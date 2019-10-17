using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Mappers;
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
    public class PrismaProductBo : AbstractPrismaProductBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private String isoCode;
        private Dictionary<ConditionLinkType, string> mapConditionLinkToParameter = new Dictionary<ConditionLinkType, string>();
        private Dictionary<ConditionLinkType, string> mapConditionLinkToEntity = new Dictionary<ConditionLinkType, string>();

        private ConditionLinkType[] supportedLinks;
        private bool debug = false;

        /// <summary>
        /// Different supported Configurations for Prisma Product Condition Links
        /// </summary>
        public static ConditionLinkType[] CONDITIONS_BANKNOW = { ConditionLinkType.PRHGROUPEXT, ConditionLinkType.BRAND, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.USETYPE, ConditionLinkType.KDTYP, ConditionLinkType.OBTYP };

        /// <summary>
        /// Maps PRPRODTYPE Context Filter to certain conditiontypes used in listAvailableProducts
        /// currently the ids' area sysprprodtypes!!!!
        /// </summary>
        public static Dictionary<Prprodtype, long[]> prodTypeMap = new Dictionary<Prprodtype, long[]>()
        {
          /*  {Prprodtype.STANDARD, new long[]{1,2,4,5,6,7,9}},
            {Prprodtype.SCHNELLCALC, new long[]{3}},
            {Prprodtype.RWV, new long[]{8,9}},
            {Prprodtype.B2CANTRAG, new long[]{1,2,4,5,6,7,9}},
            {Prprodtype.B2C, new long[]{1,2,5,6,7}},*/
            {Prprodtype.STANDARD, new long[]{1,2,4,5,6,7,11}},
            {Prprodtype.SCHNELLCALC, new long[]{3}},
            {Prprodtype.RWV, new long[]{10,11}},
            {Prprodtype.B2CANTRAG, new long[]{1,2,4,5,6,7,11}},
            {Prprodtype.B2C, new long[]{1,2,5,6,7}},
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        /// <param name="obDao"></param>
        /// <param name="transDao"></param>
        /// <param name="supportedLinks"></param>
        /// <param name="isoCode">ISO code für Übersetzung</param>
        public PrismaProductBo(IPrismaDao pDao, IObTypDao obDao, ITranslateDao transDao, ConditionLinkType[] supportedLinks, String isoCode)
            : base(pDao, obDao, transDao)
        {
            this.isoCode = isoCode;
            this.supportedLinks = supportedLinks;
            debug = (_log.IsDebugEnabled);

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
        override
        public PRPRODUCT getProduct(long sysprproduct)
        {
            return pDao.getProduct(sysprproduct, isoCode);
        }

        /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        override
        public VART getVertragsart(long sysprproduct)
        {
            return pDao.getVertragsart(sysprproduct);
        }

        /// <summary>
        /// Returns a sorted flat List for the given Products
        /// will be translated for the given bildwelt
        /// </summary>
        /// <param name="products"></param>
        /// <param name="sysprbildwelt"></param>
        /// <returns></returns>
        override
        public List<AvailableProduktDto> listSortedAvailableProducts(List<PRPRODUCT> products, long sysprbildwelt)
        {
            Configuration cfg = new Configuration(new TypeMapFactory(), MapperRegistry.AllMappers());
            cfg.CreateMap<OpenOne.Common.Model.Prisma.PRPRODUCT, AvailableProduktDto>().ForMember(a => a.sysID, m => m.MapFrom(s => s.SYSPRPRODUCT))
                                                          .ForMember(a => a.beschreibung, m => m.MapFrom(s => s.DESCRIPTION))
                                                          .ForMember(a => a.bezeichnung, m => m.MapFrom(s => s.NAME))
                                                            .ForMember(a => a.code, m => m.MapFrom(s => s.NAMEINTERN));
            //config mapping from PRPRODUCT object to sort to a sortable object
            cfg.CreateMap<OpenOne.Common.Model.Prisma.PRPRODUCT, FlatSortableDto>().ForMember(a => a.sysid, m => m.MapFrom(s => s.SYSPRPRODUCT)).
                                        ForMember(a => a.data, m => m.MapFrom(s => s));

            MappingEngine mapper = new MappingEngine(cfg);

            //create the result objects
            List<AvailableProduktDto> resultProducts = new List<AvailableProduktDto>();

            List<CTLUT_Data> translations = base.transDao.readoutTranslationList("'PRPRODUCT', 'PRBILDWELTV', 'BCHANNEL','VART'", isoCode);

            //build the sorttree
            ISortTreeBo sbo = PrismaBoFactory.getInstance().createSortTreeBo();
            SortTreeNode treeRoot = sbo.getSortTree();
            if (treeRoot.getChildren() == null || treeRoot.getChildren().Count == 0 || products.Count == 0)//nothing to sort
            {

                foreach (PRPRODUCT prod in products)
                {
                    AvailableProduktDto pdto = mapper.Map<OpenOne.Common.Model.Prisma.PRPRODUCT, AvailableProduktDto>(prod);

                    VART vart = getVertragsart(prod.SYSPRPRODUCT);
                    if (vart == null)
                        _log.Warn("Product has no VART:" + prod.SYSPRPRODUCT);

                    pdto.bezeichnung = prod.NAME;
                    pdto.beschreibung = prod.DESCRIPTION;

                    CTLUT_Data Translation = transDao.RetrieveEntry(prod.SYSPRPRODUCT, "PRPRODUCT", translations);
                    if (Translation != null)
                    {
                        pdto.bezeichnung = Translation.Name;
                        pdto.beschreibung = Translation.Description;
                    }

                    pdto.code = (vart!=null?vart.CODE:"NULL") + (pDao.isDiffLeasing(prod.SYSPRPRODUCT) ? "_DIFF" : "");
                    pdto.sysID = prod.SYSPRPRODUCT;
                    pdto.indent = 1;
                    if (prod.SYSVTTYP.HasValue && pDao.getVttypById(prod.SYSVTTYP.Value) != null)
                        pdto.vttypcode = pDao.getVttypById(prod.SYSVTTYP.Value).CODE;
                    pdto.sysprprodtype = prod.SYSPRPRODTYPE.HasValue ? prod.SYSPRPRODTYPE.Value : 0;
                    resultProducts.Add(pdto);
                }
            }
            else //the default - sorted tree
            {
                //map our objects to sortable objects
                List<FlatSortableDto> items = mapper.Map<List<OpenOne.Common.Model.Prisma.PRPRODUCT>, List<FlatSortableDto>>(products);
                try
                {
                    //filter the sorttree with our objects
                    treeRoot = sbo.filterTree(treeRoot, items);
                    if (treeRoot.getChildren() == null || treeRoot.getChildren().Count == 0)
                        throw new Exception("Sorttree doesnt contain the available Products");

                    //build a flat list from our filtered tree
                    List<SortTreeNode> nodes = sbo.flattenTree(treeRoot);
                    List<PrBildweltVDto> bwlist = pDao.getBildweltVertragsarten();

                    foreach (SortTreeNode node in nodes)
                    {
                        AvailableProduktDto pdto = new AvailableProduktDto();
                        if (node.getData() != null)
                        {
                            FlatSortableDto sortable = (FlatSortableDto)node.getData();
                            PRPRODUCT product = (PRPRODUCT)sortable.getData();
                            VART vart = getVertragsart(product.SYSPRPRODUCT);
                            if (vart == null)
                                _log.Warn("Product has no VART:" + product.SYSPRPRODUCT);

                            pdto.bezeichnung = product.NAME;
                            pdto.beschreibung = product.DESCRIPTION;
                            if (node.wftablecode != null && node.wftablecode.Length > 0)
                            {
                                CTLUT_Data Translation = transDao.RetrieveEntry(product.SYSPRPRODUCT, node.wftablecode, translations);
                                if (Translation != null)
                                {
                                    pdto.bezeichnung = Translation.Name;
                                    pdto.beschreibung = Translation.Description;
                                }
                            }
                            pdto.code = (vart!=null?vart.CODE:"") + (pDao.isDiffLeasing(product.SYSPRPRODUCT) ? "_DIFF" : "");
                            pdto.sysID = product.SYSPRPRODUCT;
                            pdto.indent = node.getDepth();
                            if (product.SYSVTTYP.HasValue && pDao.getVttypById(product.SYSVTTYP.Value)!=null)
                                pdto.vttypcode = pDao.getVttypById(product.SYSVTTYP.Value).CODE;
                            pdto.sysprprodtype = product.SYSPRPRODTYPE.HasValue ? product.SYSPRPRODTYPE.Value : 0;
                            resultProducts.Add(pdto);
                        }
                        else if (node.visible)
                        {
                            pdto.bezeichnung = node.bezeichnung;
                            pdto.beschreibung = node.sortTypBezeichnung;

                            if (node.wftablecode != null && node.wftablecode.Length > 0)
                            {
                                String tcode = node.wftablecode;
                                long tid = node.sysid;
                                if (sysprbildwelt > 0 && "VART".Equals(node.wftablecode))
                                {
                                    //map sysvart to sysprbildweltv based on the sysprbildwelt
                                    PrBildweltVDto bwmap = (from c in bwlist
                                                            where c.sysprbildwelt == sysprbildwelt && c.sysvart == tid
                                                            select c).FirstOrDefault();
                                    if (bwmap != null)
                                    {
                                        tcode = "PRBILDWELTV";
                                        tid = bwmap.sysprbildweltv;
                                    }
                                }
                                CTLUT_Data Translation = transDao.RetrieveEntry(tid, tcode, translations);
                                if (Translation != null)
                                {
                                    pdto.bezeichnung = Translation.Name;
                                    pdto.beschreibung = Translation.Description;
                                }
                            }

                            pdto.code = "DISABLED";
                            pdto.sysID = node.syssortpos * -1;//unique negative id
                            pdto.indent = node.getDepth();

                            resultProducts.Add(pdto);
                        }

                    }
                }
                catch (Exception e)
                {
                    _log.Warn("Not applying sort-tree: " + e.Message, e);
                    foreach (PRPRODUCT prod in products)
                    {
                        AvailableProduktDto pdto = mapper.Map<OpenOne.Common.Model.Prisma.PRPRODUCT, AvailableProduktDto>(prod);

                        VART vart = getVertragsart(prod.SYSPRPRODUCT);
                        if (vart == null)
                            _log.Warn("Product has no VART:" + prod.SYSPRPRODUCT);

                        pdto.bezeichnung = prod.NAME;
                        pdto.beschreibung = prod.DESCRIPTION;

                        CTLUT_Data Translation = transDao.RetrieveEntry(prod.SYSPRPRODUCT, "PRPRODUCT", translations);
                        if (Translation != null)
                        {
                            pdto.bezeichnung = Translation.Name;
                            pdto.beschreibung = Translation.Description;
                        }

                        pdto.code = (vart!=null?vart.CODE:"") + (pDao.isDiffLeasing(prod.SYSPRPRODUCT) ? "_DIFF" : "");
                        pdto.sysID = prod.SYSPRPRODUCT;
                        pdto.indent = 1;
                        //pdto.sysvart = vart.SYSVART;
                        pdto.sysprprodtype = prod.SYSPRPRODTYPE.HasValue?prod.SYSPRPRODTYPE.Value:0;
                        resultProducts.Add(pdto);
                    }
                }
            }
            return resultProducts;
        }


        /// <summary>
        /// Returns a List of Available Products
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        override
        public List<PRPRODUCT> listAvailableProducts(prKontextDto context)
        {
            context.sysperole = obDao.getHaendlerByEmployee(context.sysperole);

           
            DateTime perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(context.perDate);
            perDate = perDate.Date;

            List<PRPRODUCT> allProducts = pDao.getProducts(isoCode);

            IEnumerable<long> allProductIds = (from a in allProducts
                                               where a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1 && (a.VALIDFROM == null || a.VALIDFROM <= perDate || a.VALIDFROM <= nullDate)
                                                 && (a.VALIDUNTIL == null || a.VALIDUNTIL >= perDate || a.VALIDUNTIL <= nullDate)
                                               select a.SYSPRPRODUCT).ToList();
            List<long> aTIList = (List<long>)allProductIds;

            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {
                String tableName = mapConditionLinkToEntity[ctype];

                List<ProductConditionLink> allConditions = pDao.getProductConditionLinks(tableName);

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
            List<PRPRODUCT> rval = new List<PRPRODUCT>();
            if (context.sysprproduct > 0)
            {
                if (allProductIds.Contains(context.sysprproduct))
                {
                    List<long> nlist = new List<long>();
                    nlist.Add(context.sysprproduct);
                    allProductIds = nlist;
                }
                else
                    allProductIds = new List<long>();
            }


            long[] conditiontypes = prodTypeMap[context.prprodtype];
            

            foreach (long pkey in allProductIds)
            {
                var q = from p in allProducts
                        where p.SYSPRPRODUCT == pkey
                        select p;
                PRPRODUCT prod = q.FirstOrDefault();
                if (prod == null) continue;

                if (prod.SYSPRPRODTYPE != null)
                {
                    if (conditiontypes.Contains(prod.SYSPRPRODTYPE.Value))
                    {
                        rval.Add(prod);
                    }
                }
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
            // HR: 6.6.2011
            // Durch Combine ersetzt wegen eines Union-Seiteneffekts.
            // Bugfix durch optimierte Version ersetzt
            return allTargetItems.Except(allConditionIds).Union(assignedConditionIds.Intersect(allTargetItems)).Distinct();
        }

        /// <summary>
        /// Returns Bildweltvertragsarten
        /// </summary>
        /// <returns></returns>
        override public List<PrBildweltVDto> getBildweltVertragsarten()
        {
            return pDao.getBildweltVertragsarten();
        }
    }
}