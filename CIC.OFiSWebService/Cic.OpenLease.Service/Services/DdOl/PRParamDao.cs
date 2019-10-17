using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Data Access Object for Insurances
    /// </summary>
    [System.CLSCompliant(true)]
    public class PRParamDao
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DdOlExtended _context;
        private PRPARAMAssembler PRPARAMAssembler = new PRPARAMAssembler();
        private bool noPouvoir = false;
        private Dictionary<string, PRPARAMDto> paramCache = null;
        #endregion

        private static CacheDictionary<String, PRPARAMDto> prParamCache = CacheFactory<String, PRPARAMDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, PRPOUVOIR> prPouvoirCache = CacheFactory<String, PRPOUVOIR>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        
        /// <summary>
        /// Prisma Parameter META-Fields used in AIDA
        /// </summary>
        //Sonderzahlung
        public const string CnstMietvorauszahlungFieldMeta = "LEAKALK_SZ"; //0-99 Prozent
        public const string CnstDepotFieldMeta = "LEAKALK_DEPOT";
        //Restwertbegrenzung
        public const string CnstRestwertFieldMeta = "LEAKALK_RWKALK";//-1 oder 10-100
        public const string CnstZinsEffFieldMeta = "LEAKALK_ZINSEFF_NACHLASS";
        
        public const string CnstZinsNominalFieldMeta = "LEAKALK_ZINSNOMINAL_NACHLASS";
        public const string CnstRestwertPFieldMeta = "LEAKALK_RWKALKP";
        public const string CnstHndlProvPFieldMeta = "KALK_FAKTOR_PROV";

        public const string CnstLaufzeitFieldMeta = "Laufzeit";
        public const string CnstLaufleistungFieldMeta = "Laufleistung";
        public const string CnstLaufzeitMAXFieldMeta = "LAUFZEIT_MAX";
        public const string CnstLaufleistungMAXFieldMeta = "LAUFLEISTUNG_MAX";
        public const string CnstBearbGebNachlassFieldMeta = "LEAKALK_BEARBGEBUEHR_NACHLASS";
        public const string CnstBearbeitungsgebuehrCode = "Bearb_Geb";

        public const string CnstDepotMaxValPFieldMeta = "LEAKALK_DEPOTMAXVALP";//1.1
        public const string CnstDepotAbschlagFieldMeta = "LEAKALK_DEPOTABSCHLAG";//1.1
        public const string CnstDepotMaxEigenPFieldMeta = "LEAKALK_SZDEPOTMAXVALP";//1.1

        //Mindestkreditsumme
        public const string CnstFinanzierungssummeMin = "KALK_BORDER_BGINTERN";//zw 2500 und 100000
        //Minimale Rate
        public const string CnstRateMin = "KALK_BORDER_RATE";//25-10000

        #region Constructors
        public PRParamDao(DdOlExtended context)
        {
            _context = context;
        }
        public PRParamDao(DdOlExtended context, Dictionary<string, PRPARAMDto> pCache, bool disablePouvoirs)
        {
            paramCache = pCache;
            noPouvoir = disablePouvoirs;
            _context = context;
        }
        #endregion

        public static decimal applyNumberBoundaries(PRPARAMDto param, decimal value)
        {
            if (param != null)
            {
                //Change value depending on max value
                if (value > param.MAXVALN)
                {
                    value = param.MAXVALN.GetValueOrDefault();
                }

                //Change value depending on min value
                if (value < param.MINVALN)
                {
                    value = param.MINVALN.GetValueOrDefault();
                }
            }
            return value;
        }
        public static decimal applyPercentBoundaries(PRPARAMDto param, decimal value)
        {
            if (param != null)
            {
                //Change value depending on max value
                if (value > param.MAXVALP)
                {
                    value = param.MAXVALP.GetValueOrDefault();
                }

                //Change value depending on min value
                if (value < param.MINVALP)
                {
                    value = param.MINVALP.GetValueOrDefault();
                }
            }
            return value;
        }

      /*  public PRPARAMDto[] DeliverPrParams(long sysPrProdukt, long sysObTyp, long sysPerole, long sysbrand, long sysobart, bool applyPouvoirs)
        {
            long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(_context, sysPerole, PEROLEHelper.CnstVPRoleTypeNumber);

            // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
            PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(_context, sysbrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();
            if (PrHGroup == null)
                return new PRPARAMDto[0];
            ObTypDao obdao = new ObTypDao();

            List<long> obpath = obdao.getObTypAscendants(sysObTyp);// _context.ExecuteStoreQuery<long>("select sysobtyp from obtyp start with sysobtyp=" + sysObTyp + " connect by prior sysobtypp=sysobtyp", null).ToList();
            

            return DeliverPrParams(sysPrProdukt, sysObTyp, sysPerole, sysbrand, sysobart, sysVpPeRole, PrHGroup.SYSPRHGROUP, 0, obpath, applyPouvoirs,null);

        }*/

     /*   public PRPARAMDto[] DeliverPrParams(long sysPrProdukt, long sysObTyp, long sysPerole, long sysbrand, long sysobart, long sysVpPeRole, long sysPrHGroup, long sysPrKgroup, List<long> obpath, bool applyPouvoirs, String filter)
        {
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto> PrParamDtoList = new List<PRPARAMDto>();
            System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.PRPARAM> PrParamList = new List<PRPARAM>();
         
            System.Collections.Generic.List<PRPARAMDto> paramList = null;
            

           

            try
            {
              

                    KALKTYP KalkTyp = PRPRODUCTHelper.DeliverKalkTyp(_context, sysPrProdukt);
                    PouvoirTriggerDto[] triggers = new PouvoirTriggerDto[5];//for bmw
                    triggers[0] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_BRAND, sysbrand);
                    triggers[1] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_OBART, sysobart);
                    triggers[2] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_PRPRODUCT, sysPrProdukt);
                    triggers[3] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_OBTYP, obpath);
                    triggers[4] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_KALKTYP, KalkTyp.SYSKALKTYP);
                  

                    Cic.OpenLease.Service.PrismaDao prismadao = new Cic.OpenLease.Service.PrismaDao();
                    ObTypDao obDao = new ObTypDao();
                    PrismaParameterBo parbo = new PrismaParameterBo(prismadao, obDao);

                    prKontextDto kontext = new prKontextDto();
                    kontext.perDate = DateTime.Now;
                    //kontext.sysbrand = sysbrand;
                    //kontext.sysprhgroup = sysPrHGroup;
                    //kontext.sysobart = sysobart;
                    //kontext.sysobtyp = sysObTyp;
                    //kontext.sysvpperole = sysVpPeRole;
                    kontext.sysprchannel = 2; //AIDA
                    //kontext.sysprkgroup = sysPrKgroup;
                    kontext.sysprproduct = sysPrProdukt;
                    paramList = parbo.listAvailableParameter(kontext);

                    PouvoirDao pdao = new PouvoirDao(_context);
                    //Add to PrParams List
                    if (paramList != null && paramList.Count > 0)
                    {
                        foreach (PRPARAMDto p in paramList)
                        {
                          
                            if (p == null) continue;
                            if(applyPouvoirs)
                                pdao.applyPouvoir(p, sysPerole, triggers);


                            PrParamDtoList.Add(p);
                        }

                    }
                
            }
            catch
            {
                throw;
            }
           
            return PrParamDtoList.ToArray();
        }*/

        public PRPARAMDto DeliverPrParam(long sysPrProdukt, long sysObTyp, long sysPerole, long sysbrand, string objectfieldmeta, long sysobart)
        {
            return DeliverPrParam(sysPrProdukt, sysObTyp, sysPerole, sysbrand, objectfieldmeta, sysobart, true);
        }


        public PRPOUVOIR DeliverPrPouvoir(long sysPrProdukt, long sysObTyp, long sysPerole, long sysbrand, string objectfieldmeta, long sysobart)
        {
            noPouvoir = true;

            if (noPouvoir) return null;
            String key = string.Join("_", new string[] { sysPrProdukt.ToString(), sysObTyp.ToString(), sysPerole.ToString(), sysbrand.ToString(), objectfieldmeta, sysobart.ToString() });
            if (prPouvoirCache.ContainsKey(key))
                return prPouvoirCache[key];


            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> paramList = null;

            // Note in BMW there is no PKGroup in use
           // string SysPRKGroup = "0";

            try
            {
                // Get VP PeRole
                long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(_context, sysPerole,  PeroleHelper.CnstVPRoleTypeNumber);

                // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(_context, sysbrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();

                //Parameters for query
                if (PrHGroup != null)
                {
                   
                    ObTypDao obDao = new ObTypDao();
                    List<long> obpath = obDao.getObTypAscendants(sysObTyp);//_context.ExecuteStoreQuery<long>("select sysobtyp from obtyp start with sysobtyp=" + sysObTyp + " connect by prior sysobtypp=sysobtyp", null).ToList();
                    KALKTYP KalkTyp = PRPRODUCTHelper.DeliverKalkTyp(_context, sysPrProdukt);

                    PouvoirTriggerDto[] triggers = new PouvoirTriggerDto[5];//for bmw
                    triggers[0] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_BRAND, sysbrand);
                    triggers[1] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_OBART, sysobart);
                    triggers[2] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_PRPRODUCT, sysPrProdukt);
                    triggers[3] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_OBTYP, obpath);
                    if(KalkTyp!=null)
                        triggers[4] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_KALKTYP, KalkTyp.SYSKALKTYP);


                    Cic.OpenOne.Common.DAO.Prisma.IPrismaDao pDao = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
                    Cic.OpenOne.Common.DAO.IObTypDao obDao2 = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                    Cic.OpenOne.Common.DAO.ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                    Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo parbo = new Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo(pDao, obDao2, Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo.CONDITIONS_HCBE);

                    Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
                    kontext.perDate = DateTime.Now;
                    kontext.sysobtyp = sysObTyp;
                    kontext.sysprproduct = sysPrProdukt;
                    kontext.sysobtyp = sysObTyp;
                    kontext.sysobart = sysobart;
                    kontext.sysperole = sysPerole;
                    
                    

                   /* prKontextDto kontext = new prKontextDto();
                    kontext.perDate = DateTime.Now;
                    kontext.sysprchannel = 2; //AIDA
                    kontext.sysprproduct = sysPrProdukt;
                    */


                    paramList = parbo.listAvailableParameter(kontext);
                    paramList = (from p in paramList
                                where p.meta.Equals(objectfieldmeta)
                                select p).ToList();

                    PouvoirDao pdao = new PouvoirDao(_context);
                    //Add to PrParams List
                    if (paramList != null && paramList.Count > 0)
                    {
                        foreach (OpenOne.Common.DTO.Prisma.ParamDto p in paramList)
                        {

                            if (p == null) continue;


                            PRPOUVOIR pouvoir = pdao.DeliverPouvoir(p.sysID, p.sysprfld, sysPerole, triggers);
                            prPouvoirCache[key] = pouvoir;
                            return pouvoir;
                        }

                    }
                    else
                        prPouvoirCache[key] = null;
                }
            }
            catch
            {
                throw;
            }

            return null;
        }


        public PRPARAMDto DeliverPrParam(long sysPrProdukt, long sysObTyp, long sysPerole, long sysbrand, string objectfieldmeta, long sysobart, bool applyPouvoir)
        {

            if (paramCache != null)
            {
                if(paramCache.ContainsKey(objectfieldmeta))
                    return paramCache[objectfieldmeta];
                return null;
            }

            String key = string.Join("_", new string[] { sysPrProdukt.ToString() ,sysObTyp.ToString(), sysPerole.ToString(), sysbrand.ToString(), objectfieldmeta, sysobart.ToString(), applyPouvoir.ToString()});
            if(prParamCache.ContainsKey(key))
                return prParamCache[key];

            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> paramList = null;

         
            try
            {
                // Get VP PeRole
                long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(_context, sysPerole, PeroleHelper.CnstVPRoleTypeNumber);

                // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(_context, sysbrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();
                CalculationDao calcDao = new CalculationDao(_context);
                VartDTO va = calcDao.getVART(sysPrProdukt);
                long sysvart = va.SYSVART;

                //Parameters for query
                if (PrHGroup != null)
                {
                 
                    PouvoirTriggerDto[] triggers = new PouvoirTriggerDto[3];//for bmw
                    triggers[0] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_BRAND, sysbrand);
                    triggers[1] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_OBART, sysobart);
                    triggers[2] = new PouvoirTriggerDto(PouvoirTriggerDto.TRGTYPE_PRPRODUCT, sysPrProdukt);

                    //Execute Query
                    Cic.OpenOne.Common.DAO.Prisma.IPrismaDao pDao = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
                    Cic.OpenOne.Common.DAO.IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                    Cic.OpenOne.Common.DAO.ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                    Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo parbo = new Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo(pDao, obDao, Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo.CONDITIONS_HCBE);

                    Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
                    kontext.perDate = DateTime.Now;
                    kontext.sysobtyp = sysObTyp;
                    kontext.sysprproduct = sysPrProdukt;
                    kontext.sysobart = sysobart;
                    kontext.sysvart = sysvart;
                    kontext.sysperole = sysPerole;
                    //kontext.sysprchannel = 2;
                    //kontext.syskdtyp = syskdtyp;
                    

                   /* prKontextDto kontext = new prKontextDto();
                    kontext.perDate = DateTime.Now;
                    //kontext.sysbrand = sysbrand;
                    //kontext.sysprhgroup = PrHGroup.SYSPRHGROUP;
                    kontext.sysobart = sysobart;
                    //kontext.sysobtyp = sysObTyp;
                    //kontext.sysvpperole = sysVpPeRole;
                    kontext.sysprchannel = 2; //AIDA
                    //kontext.sysprkgroup = 0;
                    kontext.sysprproduct = sysPrProdukt;
                    kontext.sysvart = sysvart;
                    */
                    paramList = parbo.listAvailableParameter(kontext);
                    paramList = (from p in paramList
                                 where p.meta.Equals(objectfieldmeta)
                                 select p).ToList();
                   
                    PouvoirDao pdao = new PouvoirDao(_context);
                    //Add to PrParams List
                    if (paramList != null)
                    {
                        if (paramList.Count > 1)
                        {
                            _log.Warn("More than one Prisma-Field " + objectfieldmeta + " on Product " + sysPrProdukt + " sysobtyp: " + sysObTyp + " sysbrand: " + sysbrand);
                        }
                        if (paramList.Count == 0)
                        {
                            _log.Warn("No Prisma-Field " + objectfieldmeta + " on Product " + sysPrProdukt + " sysobtyp: " + sysObTyp + " sysbrand: " + sysbrand);
                            prParamCache[key] = null;
                            return null;
                        }

                        foreach (Cic.OpenOne.Common.DTO.Prisma.ParamDto p in paramList)
                        {
                           
                            if (p == null) continue;
                           
                            PRPARAMDto pn = new PRPARAMDto();
                            pn.MAXVALN = (decimal)p.maxvaln;
                            pn.MINVALN = (decimal) p.minvaln;
                            pn.MAXVALP = (decimal) p.maxvalp;
                            pn.MINVALP = (decimal) p.minvalp;
                            pn.NAME = p.name;
                            pn.PRFLDOBJECTMETA = p.meta;
                            pn.STEPSIZE = (decimal)p.stepsize;
                            pn.DISABLEDFLAG = p.disabled?1:0;
                            pn.DESCRIPTION = p.name;
                            pn.DEFVALN = (decimal)p.defvaln;
                            pn.DEFVALP = (decimal)p.defvalp;
                            pn.TYP = p.type;
                            pn.PRFLDNAME = p.name;
                            pn.SYSPRFLD = p.sysprfld;
                            pn.SYSPRPARAM = p.sysID;
                            pn.SYSPRPARSET = p.sysprparset;
                            pn.VISIBILITYFLAG = p.visible ? 1 : 0;
                            if(applyPouvoir)
                                pdao.applyPouvoir(pn, sysPerole, triggers);

                            prParamCache[key] = pn;

                            return pn;
                            
                        }

                    }
                }
            }
            catch
            {
                throw;
            }

            return null;
        }



    }
}