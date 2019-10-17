// OWNER MK, 19-03-2009
namespace Cic.OpenLease.Service.Merge.Prisma
{
    #region Using
    using Cic.One.Utils.Util.Exceptions;
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.Prisma")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class PrismaService : Cic.OpenLease.ServiceAccess.Merge.Prisma.IPrismaService
    {
        #region Constants
        //private int CnstConditionTypeProduct = 1;
        //private int CnstConditionTypeAction = 2;
        #endregion

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        /// <summary>
        /// Verfügbarkeit von Produkten ermitteln.
        /// MK
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProducts(long sysObTyp, long sysObArt, DateTime lieferdatum, long sysmart, long syskdtyp)
        {
            return DeliverAvailablePrProductsFiltered(sysObTyp, sysObArt, null, lieferdatum, sysmart,syskdtyp);
        }

        /// <summary>
        /// Verfügbarkeit von Produkten ermitteln.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductsFiltered(long sysObTyp, long sysObArt, PrParamFilter[] filter, DateTime lieferdatum, long sysmart, long syskdtyp)
        {
            ServiceValidator v = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            Cic.OpenOne.Common.DAO.Prisma.IPrismaDao pDao = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
            Cic.OpenOne.Common.DAO.IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            Cic.OpenOne.Common.DAO.ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            Cic.OpenOne.Common.BO.Prisma.PrismaProductBo bo = new Cic.OpenOne.Common.BO.Prisma.PrismaProductBo(pDao, obDao, transDao, Cic.OpenOne.Common.BO.Prisma.PrismaProductBo.CONDITIONS_HCBE, v.ISOLanguageCode);
            Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
            kontext.perDate = lieferdatum;
            kontext.sysobtyp = sysObTyp;
            kontext.sysobart = sysObArt;
            kontext.sysprchannel = 1;//immer 1
            kontext.sysperole = v.SysPEROLE;
            kontext.syskdtyp = syskdtyp;


            kontext.sysvpperole = v.VpSysPEROLE;// PeroleHelper.FindRootPEROLEByRoleType(context, v.SysPEROLE, PEROLEHelper.CnstVPRoleTypeNumber);
            
            List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT> products = bo.listAvailableProducts(kontext);


            //remove products without VART!!!
            products = (from t in products
                        where t.SYSVART > 0
                        select t).ToList();

            List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto> rval = AutoMapper.Mapper
                .Map<List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT>, List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto>>(products);
            return rval.ToArray();
           
        }


        /// <summary>
        /// Liefert 2 Listen für Neu- und Verlängerungsprodukte
        /// </summary>
        /// <param name="sysVt"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ExtensionProductDto DeliverExtensionProducts(long sysVt)
        {
            return null;
            /* try
            {
                ServiceValidator v = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                PrismaDao pd = new PrismaDao(v.SysPEROLE, v.SysBRAND);
                long[] conditiontypes = { 1,2 };
                long sysObArt = -1;
                long sysmart = 0;

                ExtensionProductDto rval = new ExtensionProductDto();

                ContractExtensionDao ced = new ContractExtensionDao();
                ANGEBOTDto AngebotDto = ced.getExtensionInformation(sysVt);


                PrParamFilter[] filter = new PrParamFilter[3];

                PrParamFilter f = new PrParamFilter();
                f.PRFLDMETA = "VERFUEGBAR_KAUFPREIS";
                f.TYP = 0;
                f.VALUEN = AngebotDto.ANGOBAHKEXTERNBRUTTO.HasValue ? AngebotDto.ANGOBAHKEXTERNBRUTTO.Value : 0;
                filter[0] = f;

                f = new PrParamFilter();
                f.PRFLDMETA = "VERFUEGBAR_KILOMETER";
                f.TYP = 0;
                f.VALUEN = AngebotDto.ANGOBINIKMSTAND.HasValue ? AngebotDto.ANGOBINIKMSTAND.Value : 0;
                filter[1] = f;

                f = new PrParamFilter();
                f.PRFLDMETA = "VERFUEGBAR_ALTERMONAT";
                f.TYP = 0;
                
                
                DateTime erstzul = AngebotDto.ANGOBINIERSTZUL.HasValue ? AngebotDto.ANGOBINIERSTZUL.Value : DateTime.Now;
                DateTime pDate = DateTime.Now;
                if (erstzul.Year < 1801) erstzul = DateTime.Now;//avoid invalid age
                int age = 0;
                while (erstzul.CompareTo(pDate) < 0)
                {
                    erstzul = erstzul.AddMonths(1);
                    age++;
                }
                f.VALUEN = age;
                if (f.VALUEN < 1) f.VALUEN = 1;
                filter[2] = f;

                long syskdtyp = 1;
                using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
                {
                    sysObArt = OBARTHelper.getObartOfType(context, OBARTHelper.CnstObArtUsed);
                    NoVA nv = new NoVA(context);
                    sysmart = nv.getAntriebsartId(AngebotDto.SYSOBTYP.Value);

                    if (AngebotDto.SYSIT.HasValue && AngebotDto.SYSIT.Value > 0)
                    {
                        syskdtyp = context.ExecuteStoreQuery<long>("select syskdtyp from it where sysit="+AngebotDto.SYSIT,null).FirstOrDefault();
                    }
                }


               
                

                //verlängerung nur mit gleichem Produkt, Nutzenleasing nur intern wird über VTDao.isExtendable bereits garantiert
                //aktionsprodukte sind auch zu durchsuchen (conditiontypes 1+2)
                PRPRODUCTDto[] allProducts = pd.DeliverAvailablePrProductsFiltered(AngebotDto.SYSOBTYP.Value, sysObArt, filter, DateTime.Now, null, conditiontypes, sysmart,syskdtyp,v.SysPEROLE);
                rval.verlaengerungsprodukte = (from t in allProducts
                                               where t.SYSKALKTYP == AngebotDto.ANGKALKSYSKALKTYP
                                               select t).ToArray();
                
                 
                //Matrix gültiger Folgeprodukte
                Dictionary<long, long[]> validneuProdukte = new Dictionary<long,long[]>();
                validneuProdukte[42] = new long[] { 44, 50, 39 };
                validneuProdukte[52] = new long[] { 44, 50, 39 };
                validneuProdukte[44] = new long[] { 44, 50, 39 };
                validneuProdukte[40] = new long[] { 50, 39 };
                validneuProdukte[50] = new long[] { 50, 39 };
                validneuProdukte[39] = new long[] { -1 };                
                
                //Produkte ohne Serviceprodukte:
                PRPRODUCTDto[] allneuprodukte = (from t in allProducts
                                    where t.SYSKALKTYP != 47 && t.SYSKALKTYP != 48 && t.SYSKALKTYP != 49 && t.SYSKALKTYP != 54 
                                    select t).ToArray();

                List<PRPRODUCTDto> neuprodukte = new List<PRPRODUCTDto>();
                if (validneuProdukte.ContainsKey(AngebotDto.ANGKALKSYSKALKTYP.Value))
                {
                    long[] validKalkTypen = validneuProdukte[AngebotDto.ANGKALKSYSKALKTYP.Value];
                    foreach (PRPRODUCTDto neuProdukt in allneuprodukte)
                    {
                            if (validKalkTypen.Contains(neuProdukt.SYSKALKTYP))
                                neuprodukte.Add(neuProdukt);
                    }
                }
                rval.neuprodukte = neuprodukte.ToArray();
                

                return rval;
            }
            catch (Exception exception)
            {

                // Log the exception
                _Log.Error("Deliver Extension Product failed: ", exception);

                // Throw the exception
                throw exception;
            }
            */
        }



        /// <summary>
        /// Verfügbarkeit von Aktion ermitteln.
        /// MK
        /// </summary>
        /// <param name="sysObTyp">The sys ob typ.</param>
        /// <param name="sysObArt">The sys ob art.</param>
        /// <param name="sysKalkTyp">The KALKTYP:SYSKALKTYP.</param>
        /// <param name="lieferdatum">The delivery date.</param>
        /// <param name="sysmart">sysmart</param>
        /// <returns>
        /// Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/>
        /// </returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductActions(long sysObTyp, long sysObArt, long sysKalkTyp, DateTime lieferdatum, long sysmart)
        {
            return DeliverAvailablePrProductActionsFiltered(sysObTyp, sysObArt, sysKalkTyp, null, lieferdatum, sysmart);
        }

        /// <summary>
        /// Verfügbarkeit von Aktion ermitteln.
        /// </summary>
        /// <param name="sysObTyp">The sys ob typ.</param>
        /// <param name="sysObArt">The sys ob art.</param>
        /// <param name="sysKalkTyp">The KALKTYP:SYSKALKTYP.</param>
        /// <param name="filter">Product filters.</param>
        /// <param name="lieferdatum">The delivery date.</param>
        /// <param name="sysmart">Sysmart</param>
        /// <returns>
        /// Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/>
        /// </returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductActionsFiltered(long sysObTyp, long sysObArt, long sysKalkTyp, PrParamFilter[] filter, DateTime lieferdatum, long sysmart)
        {
            return new Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[0];
            /*
            // Validate
            ServiceValidator v = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            PrismaDao pd = new PrismaDao(v.SysPEROLE, v.SysBRAND);
            long[] conditiontypes = { 2 };
            return pd.DeliverAvailablePrProductsFiltered(sysObTyp, sysObArt, filter, lieferdatum, null, conditiontypes, sysmart);
            */
        }

        public decimal DeliverZinsBasis(long sysPrProduct, long term, long amound)
        {
            System.DateTime Date = System.DateTime.Now;
            decimal Result;

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            
                return 0;
             
        }

        public decimal DeliverZins(long sysPrProduct, long term, long amound, long sysObTyp, long sysObArt, long sysPrkGroup, long sysINTTYPE)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            PRHGROUP PrHGroup = null;

            // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
            using (DdOlExtended context = new DdOlExtended())
            {
                // Get VP PeRole
                long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, ServiceValidator.SysPEROLE, PeroleHelper.CnstVPRoleTypeNumber);

                // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, ServiceValidator.SysBRAND, sysVpPeRole).FirstOrDefault<PRHGROUP>();


                decimal Result = 0;// ZinsHelper.DeliverZins(context, sysPrProduct, term, amound, sysObTyp, sysObArt, sysPrkGroup, PrHGroup.SYSPRHGROUP, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, sysINTTYPE);

                return Result;
            }
        }


        public PRPARAMDto[] DeliverPrParams(long sysPrProdukt, long sysObTyp, long sysobart, long syskdtyp)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            ServiceValidator v = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


           

            Cic.OpenOne.Common.DAO.Prisma.IPrismaDao pDao = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao();
            Cic.OpenOne.Common.DAO.IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            Cic.OpenOne.Common.DAO.ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo bo = new Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo(pDao, obDao, Cic.OpenOne.Common.BO.Prisma.PrismaParameterBo.CONDITIONS_HCBE);
            Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
            kontext.perDate = DateTime.Now;
            kontext.sysobtyp = sysObTyp;
            kontext.sysprproduct = sysPrProdukt;
            kontext.sysobtyp = sysObTyp;
            kontext.sysobart = sysobart;
            //kontext.sysprchannel = 2;
            kontext.sysperole = v.SysPEROLE;
            kontext.syskdtyp = syskdtyp;
            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> pars = bo.listAvailableParameter(kontext);
       
            try
            {
               
                List<Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto> rval = AutoMapper.Mapper
                    .Map
                    <List<Cic.OpenOne.Common.DTO.Prisma.ParamDto>, List<Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto>>(
                        pars);

                Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto p = (from t in rval
                                                            where t.PRFLDOBJECTMETA.Equals("KALK_BORDER_LZ")
                                                            select t).FirstOrDefault();

                if (p != null)
                    p.PRFLDOBJECTMETA = "Laufzeit";


                p = (from t in rval
                     where t.PRFLDOBJECTMETA.Equals("KALK_BORDER_LL")
                     select t).FirstOrDefault();

                if (p != null)
                    p.PRFLDOBJECTMETA = "Laufleistung";
                else
                {
                    Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto ll = new Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto();
                    ll.PRFLDOBJECTMETA = "Laufleistung";
                    ll.NAME = "Laufleistung";
                    ll.MINVALN = 5000;
                    ll.MAXVALN = 50000;
                    ll.DEFVALN = 10000;
                    ll.STEPSIZE = 5000;
                    rval.Add(ll);
                }

                return rval.ToArray();
            }
            catch (Exception ex)
            {
                _Log.Error("Fetching Prisma Fields failed",ex);
                return new PRPARAMDto[0];
            }
            /*
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                PRParamDao dao = new PRParamDao(context);
                PRPARAMDto[] rval = dao.DeliverPrParams(sysPrProdukt, sysObTyp, ServiceValidator.SysPEROLE, ServiceValidator.SysBRAND, sysobart, true);
                _Log.Debug("Duration of " + MethodBase.GetCurrentMethod().Name + ": " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                return rval;
            }*/

        }

        public PrFldArtDto[] DeliverPrFldArt()
        {
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PrFldArtDto> PRFldArtDtoList = null;
            System.Collections.Generic.List< PRFLDART> PRFldArtList = null;
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            using (DdOlExtended context = new DdOlExtended())
            {

                try
                {

                    object[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "pACTIVEFLAG", Value = 1}, 
                        };

                    //Execute Query
                    PRFldArtList = context.ExecuteStoreQuery<PRFLDART>("SELECT * FROM PRFLDART WHERE ACTIVEFLAG = :pACTIVEFLAG", Parameters).ToList();

                }
                catch
                {
                    throw;
                }
            }

            if (PRFldArtList != null)
            {
                PRFLDARTAssembler PRFLDARTAssembler = new PRFLDARTAssembler();
                PRFldArtDtoList = new List<PrFldArtDto>();
                foreach (PRFLDART PRFldArtLoop in PRFldArtList)
                {
                    // Convert and add to the list
                    PRFldArtDtoList.Add(PRFLDARTAssembler.ConvertToDto(PRFldArtLoop));
                }
            }
            return PRFldArtDtoList.ToArray();
        }
        public ServiceAccess.DdOl.OBTYPDto[] SearchObTyp(string description, SearchParameters searchParameters)
        {
            OBTYP[] OBTYPArray;
            List<OBTYPDto> ObTypDtoList;

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    OBTYPArray = RestOfTheHelpers.SearchTypDescription(context, description, searchParameters.Skip, searchParameters.Top);
                }
            }
            catch (System.Exception Exception)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
            }

            ObTypDtoList = new List<OBTYPDto>();

            if (OBTYPArray != null)
            {

                // New OBTYPAssembler
                OBTYPAssembler Assembler = new OBTYPAssembler();

                foreach (OBTYP ObTypLoop in OBTYPArray)
                {
                    // Convert and add to the list
                    ObTypDtoList.Add(Assembler.ConvertToDto(ObTypLoop));
                }

            }

            return ObTypDtoList.ToArray();
        }

        public ServiceAccess.DdOl.OBARTDto[] SearchObArt(string description)
        {
            OBART[] OBARTArray;
            List<OBARTDto> ObArtDtoList;

            try
            {
                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {
                    OBARTArray = OBARTHelper.SearchDescription(context, description);
                }
            }
            catch (System.Exception Exception)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric,ExceptionUtil.DeliverFlatExceptionMessage(Exception));
            }

            ObArtDtoList = new List<OBARTDto>();

            if (OBARTArray != null)
            {
                // New OBARTAssembler
                OBARTAssembler Assembler = new OBARTAssembler();

                foreach (OBART ObArtLoop in OBARTArray)
                {
                    // Convert and add to the list
                    ObArtDtoList.Add(Assembler.ConvertToDto(ObArtLoop));
                }

            }

            return ObArtDtoList.ToArray();
        }

        public ServiceAccess.DdOl.OBKATDto[] SearchObKat(string description)
        {
            OBKAT[] OBKATArray;
            List<OBKATDto> ObKatDtoList;

            try
            {
                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {
                    OBKATArray = RestOfTheHelpers.SearchKatDescription(context, description);
                }
            }
            catch (System.Exception Exception)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
            }

            ObKatDtoList = new List<OBKATDto>();

            if (OBKATArray != null)
            {
                // New OBKATAssembler
                OBKATAssembler Assembler = new OBKATAssembler();

                foreach (OBKAT ObKatLoop in OBKATArray)
                {
                    // Convert and add to the list
                    ObKatDtoList.Add(Assembler.ConvertToDto(ObKatLoop));
                }
            }

            return ObKatDtoList.ToArray();
        }

        public INTTYPEDto[] DeliverINTTYPE(long sysPrProdukt)
        {
            INTTYPE[] INTTYPEArray = null;
            List<INTTYPEDto> idtoList = new List<INTTYPEDto>();

            using (DdOlExtended context = new DdOlExtended())
            {
                var Query3 = from p in context.PRPRODUCT
                             where p.SYSPRPRODUCT == sysPrProdukt
                             select p.INTTYPE.SYSINTTYPE;
                long inttype = Query3.FirstOrDefault();
                INTTYPEArray = RestOfTheHelpers.SearchInttypes(context, sysPrProdukt);
                foreach (INTTYPE i in INTTYPEArray)
                {
                    INTTYPEDto idto = new INTTYPEDto();
                    idto.sysINTTYPE = i.SYSINTTYPE;
                    idto.Description = i.NAME;
                    if (idto.sysINTTYPE == inttype)
                        idto.DEFAULT = 1;
                    idtoList.Add(idto);
                }
            }

            return idtoList.ToArray();
        }



        public ObArtZinsQuickDto[] DeliverObArtZinsQuick()
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            List<ObArtZinsQuickDto> obArtZinsQuickDtoListe = new List<ObArtZinsQuickDto>();
            try
            {
                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {

                    OBART[] OBARTArray;
                    string description = "";
                    string query = "select prproduct.sysprproduct from prprodtype, prproduct,prclprbr where prclprbr.sysprproduct=prproduct.sysprproduct and prclprbr.sysbrand=" + ServiceValidator.SysBRAND + "  AND prclprbr.ActiveFlag = 1  AND " 
                        // + " (prclprbr.validfrom IS NULL OR prclprbr.validfrom <= TRUNC(SYSDATE))  AND (prclprbr.validuntil IS NULL OR prclprbr.validuntil >= TRUNC(SYSDATE)) "
                        + SQL.CheckCurrentSysDate ("prclprbr")
                        + " AND prproduct.ActiveFlag = 1  AND " 
                        // + " (prproduct.validfrom IS NULL OR prproduct.validfrom <= TRUNC(SYSDATE))  AND (prproduct.validuntil IS NULL OR prproduct.validuntil >= TRUNC(SYSDATE)) "
                        + SQL.CheckCurrentSysDate ("prproduct")
                        + " and prproduct.sysprprodtype=prprodtype.sysprprodtype and prprodtype.conditiontype=3";


                    // Note in BMW there is no PKGroup in use
                    long sysPrkGroup = 0L;


                    //Execute Query
                    long sysPrProduct = context.ExecuteStoreQuery<long>(query).FirstOrDefault<long>();
                    PRHGROUP PrHGroup = null;
                    long sysInttype = PRPRODUCTHelper.DeliverSysInttype(context, sysPrProduct);

                    // Get PrHGroup - in BMW there can be only one - thats why FirstOrDefault

                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, ServiceValidator.SysPEROLE, PeroleHelper.CnstVPRoleTypeNumber);

                    // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                    PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, ServiceValidator.SysBRAND, sysVpPeRole).FirstOrDefault<PRHGROUP>();


                    OBARTArray = OBARTHelper.SearchDescription(context, description);
                    foreach (OBART obArt in OBARTArray)
                    {
                        ObArtZinsQuickDto obArtZinsQuickDto = new ObArtZinsQuickDto();
                        obArtZinsQuickDto.BEZEICHNUNG = obArt.NAME;
                        obArtZinsQuickDto.ZINS = 0;// ZinsHelper.DeliverZins(context,sysPrProduct, 0, 0, 0, obArt.SYSOBART, sysPrkGroup, PrHGroup.SYSPRHGROUP, ServiceValidator.SysBRAND, ServiceValidator.SysPEROLE, sysInttype);
                        obArtZinsQuickDtoListe.Add(obArtZinsQuickDto);


                    }
                }
            }
            catch (System.Exception Exception)
            {
                _Log.Error("Error in DeliverObArtZinsQuick", Exception);
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
            }


            return obArtZinsQuickDtoListe.ToArray();

        }
        #endregion
    }
}