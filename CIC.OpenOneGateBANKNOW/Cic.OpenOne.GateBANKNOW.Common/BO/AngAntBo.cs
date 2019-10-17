using AutoMapper;
using AutoMapper.Mappers;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using CIC.Database.OW.EF6.Model;
using CIC.Database.PRISMA.EF6.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Offer/Application Relevant Functions
    /// </summary>
    public class AngAntBo : AbstractAngAntBo
    {
        private DateTime nullDate = new DateTime(1800, 1, 1);

        /// <summary>
        /// Configsource für Porsche Partner Service
        /// </summary>
        public static readonly String CONFIGSOURCE_PFC = "PFC2.0";

        // KDTYP.sysKdTyp
        /// <summary>
        /// KDTYPID_PRIVAT
        /// </summary>
        public const int KDTYPID_PRIVAT = 1;
        public const int KDTYPTYP_PRIVAT = 1;
        /// <summary>
        /// KDTYPID_FIRMA
        /// </summary>
        public const int KDTYPID_FIRMA = 12;
        public const int KDTYPTYP_FIRMA = 3;

        // antrag.kalkulation.angAntKalkDto.sysobusetype
        const int OBUSETYPEID_Privat = 2;
        const int OBUSETYPEID_Gewerblich = 3;
        const int OBUSETYPEID_DemoLeasing = 4;

        // antrag.angAntObDto.sysobart
        const int OBARTID_NEU = 12;
        const int OBARTID_OCCASION = 13;

        // antrag.angAntObDto.sysobtyp
        const int OBTYPID_PKW = 1;
        const int OBTYPID_Zubehoer = 10;

        const String AUSLAUSWEISCODE_C = "2";
        const String AUSLAUSWEISCODE_G = "3";

        const String WOHNSITZ_CH = "CH";
        const String WOHNSITZ_LI = "LI";

        const String NATIONALITAET_CH = "CH";
        const String NATIONALITAET_LI = "LI";

        const String KORRADRESSE_CH = "CH";

        const String BERUFLICHCODE_UNBEFRISTET = "1";

        

        private const String SZ_SCHWELLE_QUOTE = "SZ_SCHWELLE_QUOTE";

        private const int KREDITLIMITE_INNERHALB_KKG = 80000;

        //Für R8 soll nur STANDARD, LUXUS, PREMIUM und EXOTEN verwendet werden
        //private const String v_clusterNames = "STANDARD,LUXUS,EXOTEN,PREMIUM,MOTO";
        private const String v_clusterNames = "STANDARD,LUXUS,EXOTEN,PREMIUM";
        

        private const String EL_BETRAG_MAX = "EL_BETRAG_MAX";
        private const String EL_BETRAG_MIN = "EL_BETRAG_MIN";
        private const String EL_PROZENT_MAX = "EL_PROZENT_MAX";
        private const String EL_PROZENT_MIN = "EL_PROZENT_MIN";
        private const String PROF_MAX = "PROF_MAX";
        private const String PROF_MIN = "PROF_MIN";

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// AngAntBo Constructor
        /// </summary>
        /// <param name="pDao">Angebot/Antrag DAO</param>
        /// <param name="kundeDao">Kunde DAO</param>
        /// <param name="prismaParameterBo">PrismaParameter Bo</param>
        /// <param name="translateBo">Uebersetzungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <param name="vgDao">VG DAO</param>
        /// <param name="eaihotDao">EAIHOT DAO</param>
        public AngAntBo(IAngAntDao pDao, IKundeDao kundeDao, IPrismaParameterBo prismaParameterBo, ITranslateBo translateBo, IQuoteDao quoteDao, IVGDao vgDao, IEaihotDao eaihotDao, ITransactionRisikoBo trBo)
            : base(pDao, kundeDao, prismaParameterBo, translateBo, quoteDao, vgDao, eaihotDao, trBo)
        {
        }

        /// <summary>
        /// Delivers the Auflagen for the Antrag
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public override String[] getAuflagen(long sysid, String isoCode)
        {
            return angAntDao.getAuflagen(sysid, isoCode);
        }

        /// <summary>
        /// Delivers the Antrag stati history
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public override ZustandDto[] getZustaende(long sysid, String isoCode)
        {
            return angAntDao.getZustaende(sysid, isoCode);
        }

        /// <summary>
        /// Delivers the "real"zustand composed of ZUSTAND and ATTRIBUT for a Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public override String getAntragZustand(long sysid)
        {
            return angAntDao.getAntragZustand(sysid);
        }

        /// <summary>
        /// Transfers Offer Dto into Antrag Dto
        /// If there is more than one calculation base given, the function returns an exception to indicate that it's not possible to 
        /// determine which calculation has to be taken.
        /// </summary>
        /// <param name="angebot">AngebotDto is required to only contain one Calculation</param>
        /// <returns>Antrag Output</returns>
        public override AntragDto processAngebotToAntrag(AngebotDto angebot)
        {
            _log.Debug("Starting to Convert Angebot to Antrag.");


            IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("processAngebotToAntrag", delegate (MapperConfigurationExpression cfg) {
                cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>();
                cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>();
                cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>();
            });*/



            AntragDto retVal = mapper.Map<AngebotDto, AntragDto>(angebot);
            //BNRZEHN-951
            retVal.antrag = angebot.angebot;
            if (String.IsNullOrEmpty(retVal.antrag))//not given externally, fetch it
            {
                    using (DdOlExtended context = new DdOlExtended())
                    {
                        DbConnection con = (context.Database.Connection);
                        retVal.antrag = con.Query<String>("select angebot from angebot where sysid=:sysid", new { sysid = angebot.sysid }).FirstOrDefault();
                    }
            }
            retVal.sysid = 0;
            if (retVal.angAntObDto != null)
            {
                retVal.angAntObDto.sysob = 0;
            }

            if (angebot.angAntVars == null || angebot.angAntVars.Count == 0)
            {
                _log.Error("No Calculation in Angebot.");
                throw new ApplicationException("No Calculation in Angebot.");
            }

            AngAntVarDto variante = angebot.angAntVars[0];
            if (angebot.angAntVars.Count > 1)
            {
                variante = (from k in angebot.angAntVars
                            where k.inantrag > 0
                            select k).FirstOrDefault();
            }

            if (variante == null)
            {
                _log.Error("No active calculation in Angebot.");
                throw new ApplicationException("No Calculation in Angebot.");
            }
            KalkulationDto activeKalk = variante.kalkulation;
            if (angebot.sysprhgroup.HasValue)
            activeKalk.angAntKalkDto.sysprhgrp = angebot.sysprhgroup.Value;


            //configure for deep clone, source and target are equal
            mapper = Mapper.Instance;

           /*
            if (angebot.erfassungsclient == AngAntDao.ERFASSUNGSCLIENT_MA)
            {
                mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("processAngebotToAntragMA", delegate (MapperConfigurationExpression cfg)
                {
                    cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>()
                           .ForMember(dest => dest.zinseff, opt => opt.Ignore());

                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
                    cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
                });
            }
            else
            {
                mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("processAngebotToAntragNONMA", delegate (MapperConfigurationExpression cfg)
                {
                    cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();

                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
                    cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
                    cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
                });


            }*/

            //clone
            retVal.kalkulation = mapper.Map<KalkulationDto, KalkulationDto>(activeKalk);

            //set primary keys to zero in copy
            retVal.kalkulation.angAntKalkDto.sysangvar = 0;
            retVal.kalkulation.angAntKalkDto.syskalk = 0;
            // Ticket#2012081510000041
            if(angebot.erfassungsclient == AngAntDao.ERFASSUNGSCLIENT_MA)
                retVal.kalkulation.angAntKalkDto.zinseff = 0;

            foreach (AngAntProvDto prov in retVal.kalkulation.angAntProvDto)
            {
                prov.sysprov = 0;
                prov.sysangvar = 0;
            }
            foreach (AngAntSubvDto subv in retVal.kalkulation.angAntSubvDto)
            {
                subv.sysangsubv = 0;
                subv.sysangvar = 0;
            }
            foreach (AngAntVsDto vs in retVal.kalkulation.angAntVsDto)
            {
                vs.sysangvs = 0;
                vs.sysangvar = 0;
            }

            retVal.zustand = getAntragZustand(retVal.sysid);

            angAntDao.setAngebotZustandAttribute(angebot.sysid, "AngebottoAntrag");
           
            return retVal;
        }

        /// <summary>
        /// copyNotizenAngebotToAntrag
        /// Ticket#2012083110000047 — Übernahme Memos in den Antrag 
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        /// <param name="antragErfClient"></param>
        override public void copyNotizenAngebotToAntrag(long angebotSysId, long antragSysId, long antragerfassungsclient)
        {
            if (antragerfassungsclient == AngAntDao.ERFASSUNGSCLIENT_MA || antragerfassungsclient == AngAntDao.ERFASSUNGSCLIENT_ONE || antragerfassungsclient == AngAntDao.ERFASSUNGSCLIENT_DMR)
            {
                this.angAntDao.copyNotizenAngebotToAntrag(angebotSysId, antragSysId);
            }
        }

        /// <summary>
        /// Übernahme Dokumente vom Angebot in den Antrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        override public void copyDms(long angebotSysId, long antragSysId)
        {
            this.angAntDao.copyDms(angebotSysId, antragSysId);
        }

        /// <summary>
        /// Create or Update an Offer
        /// </summary>
        /// <param name="ang">Offer Data</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>New Offer data</returns>
        override
        public AngebotDto createOrUpdateAngebot(AngebotDto ang, long sysperole)
        {
            if (ang.sysid == 0)
            {
                return createAngebot(ang, sysperole);
            }
            else
            {
                return updateAngebot(ang, sysperole);
            }
        }

        /// <summary>
        /// Create Offer
        /// </summary>
        /// <param name="angebotInput">Offer Data</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>New Offer Data</returns>
        override
        public AngebotDto createAngebot(AngebotDto angebotInput, long sysperole)
        {
            //new angebot, update pkz-angebot reference, fetch angebot
            AngebotDto rval = this.angAntDao.createAngebot(angebotInput, sysperole);
            if (angebotInput.kunde != null && rval.kunde==null)
            {
                rval.kunde = this.kundeDao.getKunde(rval.kunde.sysit,rval.sysid);
            }
            Cic.OpenOne.Common.BO.Search.SearchCache.entityChanged("ANGEBOT");

            
            return rval;

        }

        /// <summary>
        /// Create Offer
        /// </summary>
        /// <param name="angebotInput">Offer Data</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>New Offer Data</returns>
        override
        public AngebotDto createAngebot(AngebotDto angebotInput, int? aktivKz, int? endeKz, long sysperole)
        {
            AngebotDto rval = this.angAntDao.createAngebot(angebotInput, aktivKz, endeKz, sysperole);
            if (angebotInput.kunde != null && rval.kunde == null)
            {
                rval.kunde = this.kundeDao.getKunde(rval.kunde.sysit);
            }
            Cic.OpenOne.Common.BO.Search.SearchCache.entityChanged("ANGEBOT");
            return rval;
        }

        /// <summary>
        /// Leeres Antrag anhand von SysNkk erzeugen.
        /// </summary>
        /// <param name="sysNkk"></param>
        /// <param name="syswfuser"></param>
        /// <param name="sysperole"></param>
        /// <param name="ISOlanguageCode"></param>
        /// <returns>Antrag Id</returns>
        public override AntragDto createAntragFromNkk(long sysNkk, long syswfuser, long sysperole, string ISOlanguageCode)
        {
            var antrag = new AntragDto()
            {
                vertriebsweg = "Fahrzeugfinanzierung vermittelt",
                sysprchannel = 1
            };

            AngAntObDto angAntOb = null;
            angAntOb = angAntDao.getObjektdatenFromNkk(sysNkk);

            if (angAntOb == null)
            {
                _log.Warn(string.Format("OB with SYSNKK = {0} does not have a SYSOBTYP.", sysNkk));
                var vinCode = angAntDao.getVinCodeFromNkk(sysNkk);
                if (!string.IsNullOrEmpty(vinCode))
                {
                    try
                    {
                        angAntOb = getObjektdatenByVIN(vinCode, syswfuser, ISOlanguageCode, sysNkk, "NKK");
                        if (angAntOb.sysobtyp == 0)
                        {
                            _log.Warn("Could not match VIN with SYSOBTYP.");
                        }
                    }
                    catch (Exception)
                    {
                        _log.Warn(string.Format("Could not load Objekt by VIN {0}.", vinCode));
                    }
                }
            }
            if (angAntOb == null)
            {
                _log.Warn("Fallback to loading the AngAntOb from OB/OBBRIEF.");
                angAntOb = angAntDao.getObjektdatenFromOB(sysNkk);
            }

            if (angAntOb == null)
            {
                _log.Warn("No AngAntOb found, service is not creating a new application.");
                throw new ArgumentException(string.Format("Could not create antrag from SYSNKK {0}.\n\n1. OB with SYSNKK {0} does not have a SYSOBTYP\n2. There is no vin code, the vin code could not be matched with the corresponding sysobtyp or the vin request did not return a result\n3. And no OB/OBBRIEF with SYSNKK {0} exists.", sysNkk));
            }

            angAntOb.sysnkk = sysNkk;
            angAntOb.ahk = 0;
            angAntOb.ahkBrutto = 0;
            angAntOb.ahkExternBrutto = 0;
            angAntOb.ahkUst = 0;

            antrag.angAntObDto = angAntOb;
            
            return MyCreateAntrag(antrag, sysperole);
        }

        /// <summary>
        /// Get Offer
        /// </summary>
        /// <param name="sysid">Sys ID</param>
        /// <returns>Offer Data</returns>
        override
        public AngebotDto getAngebot(long sysid)
        {
            return this.angAntDao.getAngebot(sysid);
        }


		/// <summary>
		/// Returns Vorname und Name from SYSWFUSER (WfUser)
		/// </summary>
		/// <param name="sysWfUser"></param>
		/// <returns></returns>
		override
		public String getWfUserBezeichnung (long? sysid)
        {
			return this.angAntDao.getWfUserBezeichnung (sysid);
        }

        /// <summary>
        /// Update Offer
        /// </summary>
        /// <param name="ang">Offer Data</param>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>New Offer Data</returns>
        override
        public AngebotDto updateAngebot(AngebotDto ang, long sysperole)
        {
            AngebotDto rval = angAntDao.updateAngebot(ang, sysperole);
            if (ang.kunde != null && rval.kunde == null)
            {
                rval.kunde = this.kundeDao.getKunde(rval.kunde.sysit,rval.sysid);
            }
            
            
            return rval;
        }

        /// <summary>
        /// Angebot löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        override
        public void deleteAngebot(long sysid)
        {
            this.angAntDao.deleteAngebot(sysid);
        }

        /// <summary>
        /// Angebot kopieren
        /// </summary>
        /// <param name="sysid">Angebot</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="isoCode">isoCode</param>
        /// <returns>Angebot</returns>
        override public AngebotDto copyAngebot(long sysid, long sysperole, String isoCode)
        {
            AngebotDto angebotToCopy = this.getAngebot(sysid);

            // Ticket#2012083110000029 
            String angZustand = (angebotToCopy.zustand == null ? String.Empty : angebotToCopy.zustand.Trim());
            String angAttribut = (angebotToCopy.attribut == null ? String.Empty : angebotToCopy.attribut.Trim());
            if (!(angAttribut.Equals(AngebotAttribut.Abgelaufen.ToString()) 
                || angZustand.Equals(AngebotZustand.Gedruckt.ToString()) 
                || angZustand.Equals(AngebotZustand.Neu.ToString())))
            {
                throw new System.ApplicationException("Angebot cannot be copied (Zustand=" + angZustand + ", Attribut=" + angAttribut + " ).");
            }

            // Produkt-Gültigkeit prüfen
            int countPrProducts = angebotToCopy.angAntVars.Count();
            int countInvalidPrProducts = 0;
            angebotToCopy.errortext = new List<string>();
            foreach (var angAntVar in angebotToCopy.angAntVars)
            {
                long sysPrProduct = angAntVar.kalkulation.angAntKalkDto.sysprproduct;

                IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                PRPRODUCT prProduct = prismaDao.getProduct(sysPrProduct, isoCode);

                DateTime perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                if ((prProduct != null) && (prProduct.VALIDUNTIL != null))
                {
                    if (perDate != null && prProduct.VALIDUNTIL > nullDate && prProduct.VALIDUNTIL < perDate)
                    {
                        countInvalidPrProducts++;
                        angebotToCopy.errortext.Add("Product " + sysPrProduct + " is not valid");
                    }
                }
            }
            if (countPrProducts > 0 && countPrProducts == countInvalidPrProducts)
            {
                throw new System.ApplicationException("All Products have expired. Angebot cannot be copied.");
            }

            angebotToCopy.sysid = 0;
            AngebotDto newAngebot = createAngebot(angebotToCopy, 1, 0, sysperole);
            newAngebot.errortext = angebotToCopy.errortext;
            return newAngebot;
        }

        /// <summary>
        /// Antrag holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Antragsdaten</returns>
        public override AntragDto getAntrag(long sysid,long sysperole)
        {
            return this.angAntDao.getAntrag(sysid,sysperole);
        }
        
        /// <summary>
        /// Antrag kopieren
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">sysperole</param>
        /// <param name="b2b">b2b</param>
        /// <returns>Antragsdaten</returns>
        public override AntragDto copyAntrag(long sysid, long sysperole, bool b2b)
        {
            AntragDto copied = this.getAntrag(sysid,sysperole);
            return MyCopyAntrag(copied, sysperole, b2b);
        }

        /// <summary>
        /// Antrag löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        public override void deleteAntrag(long sysid)
        {
            this.angAntDao.deleteAntrag(sysid);
        }

        /// <summary>
        /// Antrag erstellen oder updaten
        /// </summary>
        /// <param name="antrag">Antrag Eingang</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Antrag Ausgang</returns>
        public override AntragDto createOrUpdateAntrag(AntragDto antrag, long sysperole)
        {
            if (antrag.sysid == 0)
            {
                return MyCreateAntrag(antrag, sysperole);
            }
            else
            {
                return MyUpdateAntrag(antrag, sysperole);
            }
        }

       
       
        /// <summary>
        /// Moves Kremo from antrag to antrag-copy, if none found on antrag, uses kremo on Angebot
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <param name="sysAntrag"></param>
        private void transferKREMOForCopy(long sysAngebot, long sysAntrag)
        {
            if (sysAngebot == 0) return;
            KREMOInDto kin = DAO.CommonDaoFactory.getInstance().getKremoDao().FindBySysAntrag(sysAntrag);
            if(kin==null)
                kin = DAO.CommonDaoFactory.getInstance().getKremoDao().FindBySysAngebot(sysAngebot);

            if (kin == null) return;

            kin.SysAntrag = sysAntrag;
            kin.SysAngebot = 0;
            DAO.CommonDaoFactory.getInstance().getKremoDao().SaveKREMOInDto(kin);//creates a new KREMO
        }

        /// <summary>
        /// Angebot einreichen
        /// </summary>
        /// <param name="angebot">Eingehendes Angebot</param>
        /// <param name="user">Benutzerkennung</param>
        /// <param name="isocode">Sprach ID</param>
        public override void processAngebotEinreichung(AngebotDto angebot, long user, string isocode)
        {
            angAntDao.processAngebotEinreichung(angebot, user, isocode);
        }

        /// <summary>
        /// Antrag einreichen und daraus einen Vertrag generieren.
        /// </summary>
        /// <param name="antrag">Eingehender Antrag</param>
        /// <param name="syswfuser">Benutzerkennung</param>
        /// <param name="isocode">Sprach ID</param>
        public override void processAntragEinreichung(AntragDto antrag, long syswfuser, string isocode)
        {
            if (antrag == null)
            {
                throw new Exception("Parameter antrag was empty");
            }

            if (antrag.kalkulation == null)
            {
                throw new Exception("Parameter kalkulation was empty");
            }

            this.angAntDao.processAntragEinreichung(antrag, syswfuser, isocode);
            //BRNEUN CR45
            try
            {
                EaihotDto eaiOutput = new EaihotDto();
                eaiOutput = new EaihotDto()
                {
                    CODE = "AUTOCHECK",
                    OLTABLE = "ANTRAG",
                    SYSOLTABLE = antrag.sysid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = "START",
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",

                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not AUTOCHECK. EAIHOT", ex);
            }

            

        }


        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">Schlüssel</param>
        /// <returns>Daten</returns>
        public override AngAntObDto getObjektdaten(long sysobtyp)
        {
            return angAntDao.getObjektdaten(sysobtyp);
        }

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Daten</returns>
        public override AngAntObDto getObjektdaten(String key)
        {
            return angAntDao.getObjektdaten(key);
        }


        /// <summary>
        /// Objektdaten aus EurotaxVin
        /// </summary>
        /// <param name="vinCode">vinCode</param>
        /// <param name="syswfuser">syswfuser</param>
        /// <param name="ISOlanguageCode">ISOlanguageCode</param>
        /// <returns></returns>
        public override AngAntObDto getObjektdatenByVIN(String vinCode, long syswfuser, string ISOlanguageCode, long sysid = 0, string area = "")
        {
            EurotaxVinOutDto outDto = new EurotaxVinOutDto();
            EurotaxVinInDto inDto = new EurotaxVinInDto();
            inDto.vinCode = vinCode;
            inDto.ISOCountryCode = DAO.Auskunft.EurotaxVinRef.ISOcountryType.CH;
            try
            {
                string isolang = ISOlanguageCode.Substring(0, 2).ToUpper();
                inDto.ISOLanguageCode = (DAO.Auskunft.EurotaxVinRef.ISOlanguageType)Enum.Parse(typeof(DAO.Auskunft.EurotaxVinRef.ISOlanguageType), isolang);

            }
            catch
            {
                _log.Info("ISOLanguageCode not valid, assumed default language DE ");
                inDto.ISOLanguageCode = DAO.Auskunft.EurotaxVinRef.ISOlanguageType.DE;
            }

            inDto.syswfuser = syswfuser;
            inDto.sysid = sysid; //BNRZW-1724
            inDto.area = area; //BNRZW-1724
            outDto = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultEurotaxBo().GetVinDecode(inDto);
            if (outDto != null)
            {
                if (outDto.statusCode == 4)
                {
                    _log.Error("Error in VinDecode. Status of the processing : " + outDto.statusMsg);
                    throw new ApplicationException("E_00010_Cannot_Map_VIN");
                }
                if (outDto.statusCode != 0)
                {

                    _log.Error("Error in VinDecode. Status of the processing : " + outDto.statusMsg);
                    throw new ApplicationException("E_00011_Exception_VINDECODE");
                }
                try
                {
                    long sysobtyp = 0;
                    if (outDto.vehicle[0].TypeETGCode!= null && outDto.vehicle[0].TypeETGCode.Length >0)
                    {
                       sysobtyp =  angAntDao.getObtypBySchwacke(outDto.vehicle[0].TypeETGCode);
                    }
                    
                    AngAntObDto antObDto = new AngAntObDto();
                    antObDto = getObjektdaten(sysobtyp);
                    antObDto.serie = outDto.vinCode;
                    antObDto.schwacke = outDto.vehicle[0].TypeETGCode;
                    antObDto.brief.fident = outDto.vinCode;
                    if (outDto.colourDescription != null && outDto.colourDescription.Length > 255)
                    {
                        antObDto.farbeA = outDto.colourDescription.Substring(0, 255);
                    }
                    else
                    {
                        antObDto.farbeA = outDto.colourDescription;
                    }
                    antObDto.brief.impcode = outDto.vOC;
                    antObDto.polsterfarbe = outDto.upholsteryDescription;


                    //Zusatzausstattung
                    List<AngAntObAustDto> aust = new List<AngAntObAustDto>();
                    string serienText = "";
                    string sonderText = "";
                    string herstellerText = "";
                    double sonderBetrag = 0;
                    string priceString = "";

                    foreach (var item in outDto.equipment)
                    {
                        if (item.DeliveryFlag == 0 || item.DeliveryFlag == 1 || item.DeliveryFlag == 3)
                        {
                            
                            serienText += Regex.Replace(item.Description.Replace("<BR>"," "),@"\s{2}"," ") + "<BR>";
                        }
                        if (item.DeliveryFlag == 2)
                        {

                            priceString = " (CHF " + item.PriceBrutto + ")<BR>";
                            sonderText += Regex.Replace(item.Description.Replace("<BR>"," "),@"\s{2}"," ") + priceString;
                            sonderBetrag += (double)item.PriceBrutto;
                        }
                    }

                    int last = 0;
                  
                    if (sonderText.Length > 7998)
                    {
                        sonderText = sonderText.Substring(0, 7998);
                        last = sonderText.LastIndexOf("<BR>");
                        sonderText = sonderText.Substring(0, last + 4);
                    }
                    if (serienText.Length > 3999)
                    {
                        serienText = serienText.Substring(0, 3999);
                        last = serienText.LastIndexOf("<BR>");
                        serienText = serienText.Substring(0, last + 4 );
                    } 

                    if (serienText.Length > 0)
                    {
                        AngAntObAustDto serienaustattung = new AngAntObAustDto();
                        serienaustattung.freitext2 = serienText;
                        serienaustattung.snr = "10";
                        serienaustattung.source = "VINSEARCH";
                        aust.Add(serienaustattung);

                    }

                    if (sonderText.Length > 0)
                    {
                        AngAntObAustDto sonderausstattung = new AngAntObAustDto();
                        sonderausstattung.freitext2 = sonderText;
                        sonderausstattung.betrag = sonderBetrag;
                        sonderausstattung.snr = "20";
                        sonderausstattung.source = "VINSEARCH";
                        aust.Add(sonderausstattung);
                    }

                    foreach (var item in outDto.manufUnknownEquipment)
                    {
                        if (item.EquipmentDesc != null && item.EquipmentDesc.Length > 0)
                        {
                            herstellerText +=  Regex.Replace(item.OrderCode.Replace("<BR>"," "),@"\s{2}"," ") + " " + Regex.Replace(item.EquipmentDesc .Replace("<BR>"," "),@"\s{2}"," ") + "<BR>";
                        }
                    }

                    if (herstellerText.Length > 0)
                    {
                        AngAntObAustDto herstellerdaten = new AngAntObAustDto();
                        herstellerdaten.freitext2 = herstellerText;
                        herstellerdaten.snr = "30";
                        herstellerdaten.source = "VINSEARCH";
                        aust.Add(herstellerdaten);

                    }

                    AngAntObAustDto zusatzausstattung = new AngAntObAustDto();
                    zusatzausstattung.snr = "40";
                    zusatzausstattung.source = "VINSEARCH";
                    aust.Add(zusatzausstattung);


                    antObDto.aust = aust;
                    antObDto.zubehoerBrutto = sonderBetrag;

                    return antObDto;

                }
                catch (Exception e)
                {
                    _log.Error("Exception in getObjektdatenByVIN!");
                     throw new ApplicationException("E_00011_Exception_VINDECODE",e);
                }
            }
            else
                {
                    _log.Error("Exception in getObjektdatenByVIN!");
                     throw new ApplicationException("E_00011_Exception_VINDECODE");
                }
        }



        /// <summary>
        /// preisschildDruck
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override oFinVariantenDruckenDto finanzierungsvariantenDrucken(iFinVariantenDruckenDto input)
        {
            try
            {
                oFinVariantenDruckenDto rval = new oFinVariantenDruckenDto();

                EaihotDto eaiOutput = new EaihotDto();


               

                eaiOutput = new EaihotDto()
                {
                    CODE = "FINVORSCHLAG_B2B",
                    OLTABLE = "ANTRAG",
                    SYSOLTABLE = input.sysid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = input.sysid.ToString(),
                    INPUTPARAMETER2 = input.ISOLanguageCode,

                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",

                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);



                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                while (eaiOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaiOutput = eaihotDao.getEaihot(eaiOutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);

                }


                if (eaiOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {

                    if (eaiOutput.OUTPUTPARAMETER1 != null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.message = eaiOutput.OUTPUTPARAMETER1;
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("NOK"))
                        {
                            rval.frontid = "FINVORSCHLAGDRUCK_UNSUCCESSFUL";
                            rval.hfile = null;
                        }
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("OK"))
                        {
                            rval.frontid = "FINVORSCHLAGDRUCK_SUCCESSFUL";
                            EaihfileDto eaiHFile = eaihotDao.getEaiHotFile(eaiOutput.SYSEAIHOT);
                            if (eaiHFile != null)
                            {
                                rval.hfile = eaiHFile.EAIHFILE;
                            }
                        }


                    }
                }
                return rval;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not FINVORSCHLAGdDruck. EAIHOT", ex);
            }

        }



        /// <summary>
        /// preisschildDruck
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override oPreisschildDruckDto preisschildDruck(iPreisschildDruckDto input)
        {
            try
            {
                oPreisschildDruckDto rval = new oPreisschildDruckDto();
               
                EaihotDto eaiOutput = new EaihotDto();
             

                string art =  !input.preisInklusive? "OHNE_PREIS": (input.sysAngVar >0) ? "MUSTERKALK" : "INKL_PREIS";


                eaiOutput = new EaihotDto()
                {
                    CODE = "SOAP_DOK_PREISSCHILD",
                    OLTABLE = "ANGEBOT",
                    SYSOLTABLE = input.sysid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = input.sysAngVar.ToString(),
                    INPUTPARAMETER2 = input.ISOLanguageCode,
                    INPUTPARAMETER3 = art,
                    INPUTPARAMETER4 = input.herkunft,
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",

                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);



                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                while (eaiOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaiOutput = eaihotDao.getEaihot(eaiOutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);

                }


                if (eaiOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {

                    if (eaiOutput.OUTPUTPARAMETER1 != null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.message = eaiOutput.OUTPUTPARAMETER1;
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("NOK"))
                        {
                            rval.frontid = "PREISSCHILDDRUCK_UNSUCCESSFUL";
                            rval.hfile = null;
                        }
                        if (eaiOutput.OUTPUTPARAMETER1.Equals("OK"))
                        {
                            rval.frontid = "PREISSCHILDDRUCK_SUCCESSFUL";
                            EaihfileDto eaiHFile = eaihotDao.getEaiHotFile(eaiOutput.SYSEAIHOT);
                            if (eaiHFile != null)
                            {
                                rval.hfile = eaiHFile.EAIHFILE;
                            }
                        }
                       

                    }
                }
                return rval;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not preisschildDruck. EAIHOT", ex);
            }

        }

        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = DateTime.Now - oldDate;

            if (ts > timeOut)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// getAntragBezeichnungen
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public override AntragDto getAntragBezeichnungen(AntragDto antrag)
        {
            return angAntDao.getAntragBezeichnungen(antrag);
        }

        /// <summary>
        /// speichern sysvttyp in Angebot
        /// </summary>
        /// <param name="sysangebot"></param>
        public override void updateVttypinAngebot(long sysangebot)
        {
            angAntDao.updateVttypinAngebot(sysangebot);

        }

        /// <summary>
        /// Erstellt einen neuen Vertrag, der eine Restwertverlängerung des gegebenen Vertrags darstellt
        /// </summary>
        /// <param name="cctx">service context</param>
        /// <param name="sysVorvertrag">original contract's syscode</param>
        /// <returns>extended contract</returns>
        public override AntragDto createExtendedContract(CredentialContext cctx, long sysVorvertrag, int wsclient)
        {
            IRounding round = RoundingFactory.createRounding();
            AntragDto ExtendedContract = new DTO.AntragDto();
            cctx.validateService();

            //check if the contract is allowed to be extended
            /* BNRELF-1462
             
             * if (!angAntDao.checkRwVerlVerfuegbarWeb(sysVorvertrag))
              throw new ApplicationException("F_000011_RW_VERL_NICHT_VERFUEGBAR");
            */
            //use custom query to get previous contract in order to be able to hotfix at go live
            VertragDto vorvertrag = BOFactory.getInstance().createVertragBo().getVertragForExtension(sysVorvertrag);
            vorvertrag.isExtendible = true; //we already checked

            long sysobusetype = 0;

            //create new request from old contract
            if (vorvertrag.angAntObDto != null)
            {
                ExtendedContract = this.angAntDao.getAntrag(vorvertrag.angAntObDto.sysantrag,cctx.getMembershipInfo().sysPEROLE);
                sysobusetype = ExtendedContract.kalkulation.angAntKalkDto.sysobusetype;
                ExtendedContract.sysid = 0;
            }
            if (ExtendedContract.angAntObDto == null)
            {
                ExtendedContract.angAntObDto = new AngAntObDto();
            }
            ExtendedContract.sysvorvt = sysVorvertrag;
            ExtendedContract.sysid = 0;
            ExtendedContract.antrag = "";
            ExtendedContract.contractextcount++;
            ExtendedContract.nrvorvt = vorvertrag.bezeichnung;
            ExtendedContract.angAntObDto.liefer = vorvertrag.ende;
            if (ExtendedContract.angAntObDto.liefer != null)
            {
                ExtendedContract.angAntObDto.liefer.Value.AddDays(1);
            }
            ExtendedContract.erfassung = DateTime.Today;
            ExtendedContract.fform = vorvertrag.bezeichnung;
            ExtendedContract.options = new AngAntOptionDto();
            ExtendedContract.options.int02 = vorvertrag.sysid;
            long sysprproduct = 0;
            if (vorvertrag.kalkulation != null)
            {
                if (vorvertrag.kalkulation.angAntKalkDto != null)
                {
                    ExtendedContract.angAntObDto.ahk = vorvertrag.kalkulation.angAntKalkDto.rw;

                    //Brutto = Netto + Mehrwertsteuer
                    sysprproduct = ExtendedContract.kalkulation.angAntKalkDto.sysprproduct;
                    double mwst = 0;
                    IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                    long sysvart = 0;
                    VART vart = prismaDao.getVertragsart(sysprproduct);
                    if (vart != null)
                    {
                        sysvart = vart.SYSVART;
                    }
                    IMwStDao mwstDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao();          
                    mwst = BOFactory.getInstance().createVertragBo().getMWST(sysVorvertrag, ExtendedContract.angAntObDto.liefer == null ? DateTime.Today : ExtendedContract.angAntObDto.liefer.Value);
                    
                    ExtendedContract.angAntObDto.ahkBrutto = round.RoundCHF(round.getGrossValue(vorvertrag.kalkulation.angAntKalkDto.rw, mwst));
                    
                }
                if (vorvertrag.kalkulation.angAntAblDto != null && vorvertrag.kalkulation.angAntAblDto.Count > 0)
                {
                    //// ALT:
                    //ExtendedContract.ratevorvt = round.RoundCHF(vorvertrag.kalkulation.angAntAblDto[0].aktuelleRate);
                    
                    // rh 20161115: neu: Add MWSt:
                    double mwst = 0.0;
                    double aktuelleRate = vorvertrag.kalkulation.angAntAblDto[0].aktuelleRate;
                    
                    mwst = BOFactory.getInstance().createVertragBo().getMWST (sysVorvertrag, DateTime.Today);       // rh: GET aktuelle MWSt
                    ExtendedContract.ratevorvt = round.RoundCHF (round.getGrossValue (aktuelleRate, mwst));         // rh: GET brutto Rate
                }

                ExtendedContract.rwvorvt = vorvertrag.rwvorvt;
                ExtendedContract.zubehoervorvt = vorvertrag.zubehoervorvt;
            }
            ExtendedContract.angAntObDto.sysob = 0;
            ExtendedContract.angAntObDto.sysobart = 13; //Occasion
            ExtendedContract.angAntObDto.sysantrag = 0;
            ExtendedContract.angAntObDto.sysangebot = 0;
            ExtendedContract.angAntObDto.brief.sysid = 0;
            if (vorvertrag.angAntObDto != null)
            {
                ExtendedContract.angAntObDto.brief.fident = vorvertrag.angAntObDto.serie;
                if (vorvertrag.angAntObDto.brief != null)
                {
                    ExtendedContract.angAntObDto.brief.stammnummer = vorvertrag.angAntObDto.brief.fident;
                }
            }
            ExtendedContract.angAntObDto.ubnahmeKm += ExtendedContract.angAntObDto.jahresKm * vorvertrag.lz/12;

            ExtendedContract.kalkulation = new KalkulationDto();
            ExtendedContract.kalkulation.angAntAblDto = new List<AngAntAblDto>();
            AngAntAblDto abloeseVorvertrag = new AngAntAblDto();
            abloeseVorvertrag.betrag = vorvertrag.rw;
            abloeseVorvertrag.sysvorvt = sysVorvertrag;
            abloeseVorvertrag.sysabltyp = 1;
            abloeseVorvertrag.datkalkper = DateTime.Today;
            abloeseVorvertrag.datkalk = DateTime.Today;
            abloeseVorvertrag.aktuelleRate = vorvertrag.ltrate;
            //abloeseVorvertrag.aktuelleRate=0;
            ExtendedContract.kalkulation.angAntAblDto.Add(abloeseVorvertrag);
            ExtendedContract.kalkulation.angAntKalkDto = new AngAntKalkDto();
            ExtendedContract.kalkulation.angAntKalkDto.sysantrag = 0;
            ExtendedContract.kalkulation.angAntKalkDto.sysobusetype = sysobusetype; 
            if (ExtendedContract.kalkulation.angAntKalkDto.sysobusetype == 0)
            {
                throw new Exception("F_000012_NUTZUNGSART_NICHT_VERFUEGBAR");
            }

            ExtendedContract.kalkulation.angAntKalkDto.verrechnungFlag = false;
            ExtendedContract.sysit = vorvertrag.sysit;
            ExtendedContract.syskd = vorvertrag.syskd;
            KundeDto kunde = new KundeDto();

           if (wsclient == AngAntDao.ERFASSUNGSCLIENT_MA)
           {
               kunde = this.kundeDao.getKundeBySysKd(ExtendedContract.kunde.syskd);
           }
           else
           {
               kunde = this.kundeDao.getKunde(ExtendedContract.kunde.sysit);
           }

            ExtendedContract.kunde = kunde;

           ExtendedContract.erfassungsclient = wsclient;

           ExtendedContract.kunde.zusatzdaten = null;


           KundeDto mitantragsteller = null;
           if (wsclient == AngAntDao.ERFASSUNGSCLIENT_MA)
           {
               if (ExtendedContract.mitantragsteller != null && ExtendedContract.mitantragsteller.syskd > 0)
               {
                   mitantragsteller = this.kundeDao.getKundeBySysKd(ExtendedContract.mitantragsteller.syskd);
               }
           }
           else
           {
               if (ExtendedContract.mitantragsteller != null && ExtendedContract.mitantragsteller.sysit > 0)
               {
                   mitantragsteller = this.kundeDao.getKunde(ExtendedContract.mitantragsteller.sysit);
               }
           }
           if (mitantragsteller != null)
           {
               ExtendedContract.mitantragsteller = mitantragsteller;
               ExtendedContract.mitantragsteller.zusatzdaten = null;
           }


            // BNRFZ-1410 SOLL: ANTRAG:SYSVT nicht mit der SYSID des Vorvertrags füllen
           ExtendedContract.sysvt = 0;
            //no calculation
            //save request
           ExtendedContract = createOrUpdateAntrag(ExtendedContract, cctx.getMembershipInfo().sysPEROLE);
           angAntDao.createZusatzdaten4ExtendedContract(kunde.sysit, kunde.syskd, ExtendedContract.sysid);

           if (mitantragsteller != null)
           {
               this.angAntDao.createZusatzdaten4ExtendedContract(mitantragsteller.sysit, mitantragsteller.syskd, ExtendedContract.sysid);
           }

            if (ExtendedContract != null)
            {
                    // Zustandsraum
                    EaihotDto eaihotOutput = new EaihotDto();
                    eaihotOutput = new EaihotDto()
                    {
                        CODE = "VT2ANTRAG",
                        OLTABLE = "ANTRAG",
                        SYSOLTABLE = ExtendedContract.sysid,
                        SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        EVE = 1,
                        //EVALEXPRESSION = "_F(\'VT_RWVERL_NEU\',EAIHOT:SYSOLTABLE,\'\',\'\',\'\',\'\')",
                        INPUTPARAMETER1 = sysVorvertrag.ToString(),

                        PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                        HOSTCOMPUTER = "*"

                    };
                    eaihotOutput = eaihotDao.createEaihot(eaihotOutput);
                    DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, 120);
                    while (eaihotOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                    {
                        eaihotOutput = eaihotDao.getEaihot(eaihotOutput.SYSEAIHOT);
                        System.Threading.Thread.Sleep(10000);

                    }
                    if (eaihotOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready)
                    {
                        throw new Exception("Timeout bei Verarbeitung der Verlängerung");
                    }
                    else
                    {
                        ExtendedContract = getAntrag(ExtendedContract.sysid, cctx.getMembershipInfo().sysPEROLE);
                    }
            }
           

            return ExtendedContract;
        }

        #region My Methods

        /// <summary>
        /// Checks if the contract can be extended
        /// Throws exception with explanation if extension is not allowed.
        /// </summary>
        /// <param name="vorvertrag">contract to be extended</param>
        private void ContractExtensionAllowed(VertragDto vorvertrag)
        {
            if (vorvertrag == null)
            {
                throw new NullReferenceException("Could not find the contract.");
            }

            if (vorvertrag.ende.Value.AddMonths(6) < DateTime.Now)
            {
                throw new ArgumentException("The term of contract expires in more than six months in the future.");
            }

            if (vorvertrag.zustand.Equals("saldiert"))
            {
                throw new ArgumentException("The contract is already balanced.");
            }

            if (!vorvertrag.isExtendible)
            {
                throw new ArgumentException("The contract is marked as not extendible.");
            }
        }

        private AntragDto MyCreateAntrag(AntragDto antrag, long sysperole)
        {
            AntragDto rval = this.angAntDao.createAntrag(antrag, sysperole);
            /* if (antrag.kunde != null)
             {
                 rval.kunde = this.kundeDao.getKunde(rval.kunde.sysit);
             }*/
             
            if(!String.IsNullOrEmpty(antrag.antrag) && rval.sysAngebot>0)//wenn antrag aus einem angebot erzeugt wurde, kremo mitkopieren
                transferKREMOForCopy(rval.sysAngebot, rval.sysid);

            if (antrag.kunde != null)
            {
                if(rval.kunde.sysit>0)
                    rval.kunde = kundeDao.getKundeViaAntragID(rval.kunde.sysit, rval.sysid);
                else 
                    rval.kunde = this.kundeDao.getKunde(rval.kunde.sysit);
            }
            if (antrag.mitantragsteller != null)
            {
                if (rval.mitantragsteller.sysit > 0)
                    rval.mitantragsteller = kundeDao.getKundeViaAntragID(rval.mitantragsteller.sysit, rval.sysid);
                else
                    rval.mitantragsteller = this.kundeDao.getKunde(rval.mitantragsteller.sysit);
            }

            Cic.OpenOne.Common.BO.Search.SearchCache.entityChanged("ANTRAG");
            return rval;
        }

        private AntragDto MyCopyAntrag(AntragDto antrag, long sysperole, bool b2b)
        {
            int? aktivKz = 1;
            int? endeKz = 0;

            AntragDto rval = this.angAntDao.copyAntrag(antrag, aktivKz, endeKz, sysperole, b2b);
            transferKREMOForCopy(rval.sysAngebot, rval.sysid);
            if (antrag.kunde != null)
            {
                if (antrag.kunde.sysit > 0)
                {
                    rval.kunde = kundeDao.getKundeViaAntragID(rval.kunde.sysit, rval.sysid);
                    //rval.kunde = kundeDao.getKunde(rval.kunde.sysit);
                }
                else
                {
                    rval.kunde = antrag.kunde;
                }
            }
            

            return rval;
        }

        private AntragDto MyUpdateAntrag(AntragDto antrag, long sysperole)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            AntragDto rval = this.angAntDao.updateAntrag(antrag, sysperole);
            _log.Debug("Duration Antrag Update: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

            if (antrag.kunde != null)
            {
                rval.kunde = kundeDao.getKundeViaAntragID(rval.kunde.sysit, rval.sysid);
                _log.Debug("Duration Kunde Load: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            if (antrag.mitantragsteller != null)
            {
                rval.mitantragsteller = kundeDao.getKundeViaAntragID(rval.mitantragsteller.sysit, rval.sysid);
                _log.Debug("Duration MA load: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
            return rval;
        }
        #endregion


        #region Pruefung


        /// <summary>
        /// checkAngebot
        /// </summary>
        /// <param name="angAntKalkDto">Angebot Daten</param>
        /// <param name="kontext">kontext</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="angAntObDto">angAntObDto</param>
        /// <returns></returns>
        public override ocheckAntAngDto checkAngebot(KalkulationDto kalkulation, prKontextDto kontext, String isoCode, AngAntObSmallDto angAntObDto)
        {
            /* //NOT YET SAVED IN DB!
            String secCode = "RULEENGINE_PRODUKTPRUEFUNG_B2B_ANGEBOT";
            Boolean useRuleEngine = AppConfig.Instance.getBooleanEntry(secCode, "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
            

            if (useRuleEngine)
            {
                String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", secCode, "USE_RULESET", "");

                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[4];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].LookupVariableName = "PP";
                vars[0].VariableName = "SPRACHE";
                vars[0].Value = isoCode;

                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(sysid, "ANGEBOT", new String[] { "qRULES" }, ruleCode, vars, syswfuser, null);

                ocheckAntAngDto rval = new ocheckAntAngDto();
                rval.errortext = new List<string>();
                rval.code = new List<string>();
                rval.status = ocheckAntAngDto.STATUS_GREEN;

                CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto queue = (from f in queueResult
                                                                            where f.Name.Equals("qRULES")
                                                                            select f).FirstOrDefault();

                foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto record in queue.Records)
                {
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeVar = getQueueRecordValue("F01", record);
                    if (codeVar == null) continue;

                    rval.code.Add(codeVar.Value);

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto errText = getQueueRecordValue("F02", record);
                    rval.errortext.Add(errText.Value);


                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeBoundary = getQueueRecordValue("F03", record);//zb.b 250000

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeStatus = getQueueRecordValue("F04", record);

                    if (rval.status.Equals(ocheckAntAngDto.STATUS_RED)) continue;
                    if (codeStatus == null || codeStatus.Value == null || codeStatus.Equals("gruen")) continue;

                    if (codeStatus.Value.Equals("red"))
                        rval.status = ocheckAntAngDto.STATUS_RED;
                    if (codeStatus.Value.Equals("gelb"))
                        rval.status = ocheckAntAngDto.STATUS_YELLOW;


                }



                return rval;
            }*/



            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            AngAntKalkDto angAntKalkDto = kalkulation.angAntKalkDto;

            ocheckAntAngDto outDto = new ocheckAntAngDto();
            outDto.errortext = new List<string>();
            outDto.code = new List<string>();
            outDto.status = ocheckAntAngDto.STATUS_GREEN;

            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            long sysvart = 0;

            VART vart = prismaDao.getVertragsart((long)angAntKalkDto.sysprproduct);
            if (vart == null)
            {
                throw new ArgumentException("The vart is null because angAntKalkDto sysprproduct is 0");
            }
            // VertragsArt ermitteln
            sysvart = vart.SYSVART;

            VartDto vartDto = angAntDao.getVart(sysvart);
            kontext.sysvart = sysvart;
            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> paramsInput = prismaParameterBo.listAvailableParameter(kontext);

            // Definition
            #region Flags-Definitionen
            bool berechnen = true;
            bool FF = kontext.sysprchannel == 1L;        // Fahrzeugfinanzierung
            bool KF = kontext.sysprchannel == 2L;        // Kreditfinanzierung

            bool KREDIT_CLASSIC = false;
            bool KREDIT_DISPO = false;
            bool KREDIT_EXPRESS = false;
            bool KREDIT_FIX = false;
            bool FF_LEASING = false;
            bool FF_TZK_x = false;
            //bool KREDIT_DISPO_BEID = false;
            //bool KREDIT_DISPO_PLUS = false; // CREDIT_now Card
            bool KREDIT_DISPO_BEIDE = false;

            if (vartDto != null && vartDto.code != null)
            {
                // sysVart=	1, 	LEASE-now	=	LEASING
                // sysVart=	2, 	CREDIT-now Fix	=	KREDIT_FIX
                // sysVart=	3, 	CREDIT-now Classic	=	KREDIT_CLASSIC
                // sysVart=	4, 	CREDIT-now Express	=	KREDIT_EXPRESS
                // sysVart=	5, 	CREDIT-now Teilzahlungskaufvertrag	=	TZK
                // sysVart=	6, 	Fester Vorschuss	=	FV
                // sysVart=	7, 	CREDIT-now Dispo	=	KREDIT_DISPO
                // sysVart=	8, 	CREDIT-now Carfinance Plus	=	TZK_PLUS

                KREDIT_CLASSIC = vartDto.code.Equals("KREDIT_CLASSIC");
                KREDIT_DISPO = vartDto.code.Equals("KREDIT_DISPO");
                KREDIT_EXPRESS = vartDto.code.Equals("KREDIT_EXPRESS");
                KREDIT_FIX = vartDto.code.Equals("KREDIT_FIX");
                FF_LEASING = vartDto.code.Equals("LEASING");
                FF_TZK_x = vartDto.code.Contains("TZK");        // TZK oder TZK_PLUS
            }

            bool KB1 = KF && berechnen && KREDIT_CLASSIC;
            bool KB2 = KF && berechnen && KREDIT_CLASSIC;
            bool KB3 = KF && berechnen && KREDIT_CLASSIC;
            bool KB4 = KF && berechnen && KREDIT_DISPO;
            bool KB5 = KF && berechnen && KREDIT_DISPO;
            bool KB6 = KF && berechnen && KREDIT_EXPRESS;
            bool KB7 = KF && berechnen && KREDIT_EXPRESS;

            bool KN1 = KF && KREDIT_DISPO;
            bool KN2 = KF && KREDIT_EXPRESS;
            bool KN3 = KF && KREDIT_CLASSIC;

            bool KD1 = KF && KREDIT_EXPRESS;
            bool KD2 = KF && KREDIT_DISPO;
            bool KD3 = KF && KREDIT_EXPRESS;

            bool KA1 = KF && berechnen && KREDIT_CLASSIC;
            bool KA2 = KF && berechnen && KREDIT_EXPRESS;
            bool KA3 = KF && berechnen && KREDIT_DISPO;

            bool KR1 = KF && KREDIT_DISPO;
            bool KR2 = KF && KREDIT_EXPRESS;
            bool KR3 = KF && KREDIT_CLASSIC;

            bool KL1 = KF && (KREDIT_CLASSIC || KREDIT_DISPO || KREDIT_EXPRESS);
            bool KL4 = KF && berechnen && (KREDIT_CLASSIC || KREDIT_DISPO || KREDIT_EXPRESS);
            bool KL5 = KF && berechnen && KREDIT_EXPRESS;
            bool KL6 = KF && berechnen && KREDIT_DISPO;

            bool FA1 = FF && berechnen && FF_LEASING;
            bool FA2 = FF && berechnen && FF_TZK_x;
            bool FA3 = FF && berechnen && KREDIT_CLASSIC;

            bool FB1 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB2 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB3 = FF && berechnen && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB4 = FF && FF_LEASING;
            bool FB5 = FF && FF_LEASING;
            bool FB6 = FF && FF_LEASING;
            bool FB7 = FF && FF_LEASING;
            bool FB8 = FF && KREDIT_CLASSIC;
            bool FB9 = FF && KREDIT_CLASSIC;

            bool FN1 = FF && (FF_LEASING || FF_TZK_x);
            bool FN2 = FF && KREDIT_CLASSIC;

            bool FD1 = FF && berechnen && FF_LEASING;
            bool FD2 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FD3 = FF && FF_LEASING;


            bool FRW1 = FF && berechnen && FF_LEASING;
            bool FRW2 = FF && berechnen && FF_LEASING;
            bool FRW3 = FF && berechnen && FF_TZK_x;
            bool RWA = FF && FF_LEASING;

            bool FR1 = FF && FF_LEASING;
            bool FR2 = FF && FF_TZK_x;
            bool FR3 = FF && KREDIT_CLASSIC;

            bool FO1 = FF && berechnen && FF_LEASING;
            bool FO2 = FF && berechnen && FF_LEASING;
            bool FO3 = FF && FF_LEASING;

            bool FL1 = FF && berechnen && FF_LEASING;
            bool FL2 = FF && berechnen && (FF_TZK_x || KREDIT_CLASSIC);
            bool FL3 = FF && berechnen && FF_LEASING;


            #endregion //Definition

            //antrag.angAntObDto
            //erstzulassung
            //ubnahmeKm

            //PRODUKTPRÜFUNG 
            #region PARAMETER PRÜFUNG

            // True wenn KM-Stand für Fahrzeug unterschritten (FO3):
            bool kalkBorderUb_KmObjFlag = false;
            // True wenn Alter für Fahrzeug unterschritten (FO3):
            bool kalkBorderUb_AlterObjFlag = false;

            double minimalBetrag = 0;
            double minLZAUA = 0;
            double minLZRIP = 0;

            foreach (var prParam in paramsInput)
            {
                if (EnumUtil.GetStringValue(PrismaParameters.KALK_BORDER_LZ_AUA).Equals(prParam.meta))
                {
                    minLZAUA = prParam.minvaln;
                    continue;
                }
                if (EnumUtil.GetStringValue(PrismaParameters.KALK_BORDER_LZ_RIP).Equals(prParam.meta))
                {
                    minLZRIP = prParam.minvaln;
                    continue;
                }
                //Porsche Partner Service max-Abweichung Rate zu Originaleinreichung prüfen
                if (EnumUtil.GetStringValue(PrismaParameters.KALK_RATE_DERIVATION).Equals(prParam.meta))
                {
                    if (angAntKalkDto != null && angAntObDto!=null)
                    {
                        if(CONFIGSOURCE_PFC.Equals(angAntObDto.configsource))
                        {
                            if (MyIsGreaterThanMaxVal(Math.Abs(angAntKalkDto.rateBrutto-angAntObDto.rateBruttoOrg), prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_PFC", isoCode, "Es ist eine Ratenabweichung >{0} CHF aufgetreten. Bitte führen Sie eine Neuberechnung durch!"), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "PFC");
                            }
                        }
                    }
                }
                // B = Betragsgrenzen -------------------------
                if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderRate).Equals(prParam.meta))
                {
                    if (KB3 || FB3)
                    {
                        if (angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_KB3I", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung - Mindesthöhe für Kreditrate.", ocheckAntAngDto.STATUS_RED,"KB3I");
                        }
                        else
                        {
                            if (MyIsSmallerThanMinVal(angAntKalkDto.rateBrutto, prParam.minvaln))
                            {
                                if (KB3)
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB3", isoCode, "Mindesthöhe für Kreditrate wurde unterschritten. Mindesthöhe = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB3");
                                }

                                if (FB3)
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB3", isoCode, "Mindesthöhe für Kreditrate wurde unterschritten. Mindesthöhe = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB3");
                                }
                            }
                            else
                                if (MyIsGreaterThanMaxVal(angAntKalkDto.rateBrutto, prParam.maxvaln))
                                {
                                    if (KB3)
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB3_Max", isoCode, "Höhe der Kreditrate wurde überschritten. Maximale Höhe = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB3");
                                    }

                                    if (FB3)
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB3_Max", isoCode, "Höhe der Kreditrate wurde überschritten. Maximale Höhe = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "FB3");
                                    }
                                }
                        }
                    }
                }

                // R = Restwert -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderRW).Equals(prParam.meta))
                {   //FR1 FR3
                    if (FRW1 || FRW3)
                    {
                        if (angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FRWI", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung Minimaler Restwert", ocheckAntAngDto.STATUS_RED, "FRWI");
                        }
                        else
                        {   // FR1
                            if (FRW1)
                            {
                                if (MyIsSmallerThanMinVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.minvalp)))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW1", isoCode, "Minimaler Restwert von {0} % wurde unterschritten."), prParam.minvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.maxvalp)))
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW1_Max", isoCode, "Maximaler Restwert von {0} % wurde überschritten."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
                                    }
                            }

                            // FR3
                            if (FRW3 && angAntKalkDto.rwBrutto > 0)
                            {
                                double betrag = MyCalcBetragToCompare_FRW3(angAntKalkDto, berechnen);

                                if (MyIsSmallerThanMinVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(betrag, prParam.minvalp)))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW3", isoCode, "Mindestrestrate von {0} % wurde unterschritten."), prParam.minvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(betrag, prParam.maxvalp)))
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW3_Max", isoCode, "Maximale Restrate von {0} % wurde überschritten."), prParam.maxvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                                    }
                            }
                        }
                    }
                }

                // FRW2 = Restwert -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderRWschwelleKaution).Equals(prParam.meta))
                {
                    if (FRW2)
                    {
                        if (angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FRWI", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung Minimaler Restwert.", ocheckAntAngDto.STATUS_RED, "FRWI");
                        }
                        else
                        {
                            if (angAntKalkDto.depot == 0 || angAntKalkDto.depot < angAntKalkDto.rwBrutto)
                            {
                                if (MyIsSmallerThanMinVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.minvalp)))       // FR2
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW2", isoCode, "Mindestrestwert von {0} %  wurde unterschritten. Bitte erfassen Sie eine Kaution in Höhe des Restwertes oder passen Sie den Restwert entsprechend an."), prParam.minvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW2");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(angAntKalkDto.rwBrutto, MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.maxvalp)))       // FR2
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FRW2_Max", isoCode, "Maximaler Restwert von {0} %  wurde überschritten. Bitte erfassen Sie eine Kaution in Höhe des Restwertes oder passen Sie den Restwert entsprechend an."), prParam.maxvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW2");
                                    }
                            }
                        }
                    }
                }

                // D = Diverse (Spezialregelungen) -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderSonderZahlungProzent).Equals(prParam.meta))
                {
                    // Höhe der ersten Rate > x% des Barkaufpreises
                    if (FD1)
                    {
                        double maxVal = MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.maxvalp);
                        double minVal = MyCalcPercentToValue(angAntKalkDto.bginternbrutto, prParam.minvalp);
                        if (angAntKalkDto != null)
                        {
                            if (MyIsGreaterThanMaxVal(angAntKalkDto.szBrutto, maxVal))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FD1", isoCode, "Erste grosse Leasingrate darf maximal {0} % des Barkaufpreises betragen."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                            }
                            else
                                if (MyIsSmallerThanMinVal(angAntKalkDto.szBrutto, minVal))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FD1_Min", isoCode, "Erste grosse Leasingrate muss minimal {0} % des Barkaufpreises betragen."), prParam.minvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                                }
                        }
                    }
                }

                //B = Betragsgrenze -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderBgIntern).Equals(prParam.meta))
                {
                    if (angAntKalkDto != null)
                    {
                        // überschritten
                        if (FB1)
                        {
                            if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB12(angAntKalkDto, berechnen), prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB1", isoCode, "Maximaler Kreditbetrag wurde überschritten. Maximaler Kreditbetrag = {0} (FB1)."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED,"FB1");
                            }
                        }

                        if (FB7)
                        {
                            if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB4567(angAntKalkDto, berechnen), prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB7", isoCode, "Finanzierungsbetrag wurde überschritten. Maximaler Finanzierungsbetrag = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "FB7");
                            }
                        }

                        //if (FB9 && kontext.sysobtyp == OBTYPID_Zubehoer)
                        //    if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB89(angAntKalkDto, berechnen), prParam.maxvaln))
                        //        MyAddErrorMessage(outDto, "CHECKANTRAG_FB9", isoCode, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),"Finanzierungsbetrag für Zubehörsfinanzierung wurde überschritten. Maximaler Finanzierungsbetrag = {0}.", prParam.maxvaln), ocheckAntAngDto.STATUS_RED, ");

                        if (MyIsGreaterThanMaxVal(angAntKalkDto.bginternbrutto, prParam.maxvaln))
                        {
                            if (KB1)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB1", isoCode, "Maximaler Kreditbetrag für CREDIT-now Classic überschritten. Maximaler Kreditbetrag = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB1");
                            }
                            if (KB4)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB4", isoCode, "Maximaler Kreditlimite wurde überschritten. Maximale Kreditlimite = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB4");
                            }
                            if (KB7)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB7", isoCode, "Maximaler Kreditbetrag wurde überschritten. Maximaler Kreditbetrag = {0} (KB7)."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB7");
                            }
                        }


                        // Unterschritten
                        minimalBetrag = prParam.minvaln;
                        if (FB2)
                        {
                            if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB12(angAntKalkDto, berechnen), prParam.minvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB2", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (FB2)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB2");
                            }
                        }

                        if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB4567(angAntKalkDto, berechnen), prParam.minvaln))
                        {
                            if (FB4 && kontext.sysobart == OBARTID_NEU)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB4", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB4)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB4");
                            }
                            if (FB5 && kontext.sysobart == OBARTID_OCCASION)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB5", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB5)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB5");
                            }
                            if (FB6 && angAntKalkDto.sysobusetype == OBUSETYPEID_DemoLeasing)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FB6", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB6)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB6");
                            }
                        }

                        //if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB89(angAntKalkDto, berechnen), prParam.minvaln))
                        //    if (FB8 && kontext.sysobtyp == OBTYPID_Zubehoer)
                        //        MyAddErrorMessage(outDto, "CHECKANTRAG_FB8", isoCode, String.Format("Minimaler Finanzierungsbetrag für Zubehörsfinanzierung wurde unterschritten. Minimaler Finanzierungsbetrag = {0}.", prParam.minvaln), ocheckAntAngDto.STATUS_RED, ");

                        if (MyIsSmallerThanMinVal(angAntKalkDto.bginternbrutto, prParam.minvaln))
                        {
                            if (KB2)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB2", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (KB2)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB2");
                            }
                            if (KB5)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB5", isoCode, "Mindestkreditlimite wurde unterschritten. Mindestlimite = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB5");
                            }
                            if (KB6)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KB6", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (KB6)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB6");
                            }
                        }
                    }
                }
            }

            // O = Objekt Neuwagen Km und Alter -------------------------
            if (FO3 && angAntObDto != null)
            {
                if (kontext.sysobart == OBARTID_OCCASION && kalkBorderUb_AlterObjFlag == true && kalkBorderUb_KmObjFlag == true)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_FO3KMAGE", isoCode, "Aufgrund Alter oder KM-Stand des Objektes handelt es sich um ein Neuwagen.", ocheckAntAngDto.STATUS_RED, "FO3");
                }
            }

            #endregion PARAMETERPRÜFUNG

            #region CALCPRUEFUNG
            if ((kontext.sysvart == 1) && (angAntKalkDto.szBrutto > 0) && (angAntKalkDto.szBrutto < angAntKalkDto.rateBrutto))
            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("F_00006_SZSmallerRateException", isoCode, "Anzahlung {0} kleiner Folgerate {1}"), angAntKalkDto.szBrutto, angAntKalkDto.rateBrutto), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
            }
            if ((angAntKalkDto.szBrutto + angAntKalkDto.rwBrutto) > angAntKalkDto.bginternbrutto)
            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("F_00007_SZRWGreatPurchPExc", isoCode, "Summe aus Sonderzahlung {0} und Restwert {1} größer Barkaufpreis {2}"), angAntKalkDto.szBrutto, angAntKalkDto.rwBrutto, angAntKalkDto.bginternbrutto), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
            }
            if (angAntKalkDto.rateBrutto < 0)
            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("F_00005_NegativeRateException", isoCode, "Negative Rate {0}"), angAntKalkDto.rateBrutto), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
            }
            if (angAntKalkDto.rapratebruttoMin < 0)
            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("F_00004_NegativeRateException", isoCode, "Negative Rate RAP Min {0}"), angAntKalkDto.rapratebruttoMin), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
            }
            if (angAntKalkDto.rapratebruttoMax < 0)
            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("F_00003_NegativeRateException", isoCode, "Negative Rate RAP Max {0}"), angAntKalkDto.rapratebruttoMax), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
            }

            if (KREDIT_DISPO || KREDIT_CLASSIC)
            {
                if (angAntKalkDto.bginternbrutto < angAntKalkDto.auszahlung)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_ABLOESEN", isoCode, "Kreditbetrag/Kreditlimite aufgrund Ablöse/Auszahlungsbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
                }
            }

            if (prismaDao.isDiffLeasing((long)angAntKalkDto.sysprproduct))
            {
                double[] zinsen = new double[1];
                double zinsbasis = angAntKalkDto.bginternbrutto;
                double quotepercent = quoteDao.getQuote(SZ_SCHWELLE_QUOTE);
                if (quotepercent > 0)
                {
                    if (angAntKalkDto.szBrutto > (quotepercent / 100 * zinsbasis))
                    {
                        zinsbasis -= angAntKalkDto.szBrutto;
                    }
                }
                KundenzinsDto zinsDto = prismaDao.getKundenzins(angAntKalkDto.sysprproduct, angAntKalkDto.lz, zinsbasis);
                //BNR13 BNRDR-2211  Hardware-spezifisches Rundungsproblem (1.9 <> 1.900000)
                if (angAntKalkDto.zinscust > zinsDto.maxrate + 0.0000001 || angAntKalkDto.zinscust < zinsDto.minrate - 0.0000001)
                {
                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_KUNDENZINS", isoCode, "Kundenzins darf zwischen {0} und {1} liegen"), zinsDto.minrate, zinsDto.maxrate), ocheckAntAngDto.STATUS_RED, "CALCPRUEFUNG");
                }
            }


            #endregion CALCPRUEFUNG

            #region versicherung
            if (kalkulation.angAntVsDto != null)
            { 
                string vsartcode = "";
                foreach (AngAntVsDto vs in kalkulation.angAntVsDto)
                {
                    vsartcode = angAntDao.getVsArtCode(vs.sysvstyp);
                    bool vsArt_aua = vsartcode.Equals(ServiceType.AUA.ToString());
                    bool vsArt_rip = vsartcode.Equals(ServiceType.RIP.ToString());
                    bool vsArt_ra = angAntDao.istRatenabsicherung((long)kalkulation.angAntKalkDto.sysprproduct, vs.sysvstyp);
                    bool KV4 = KF && vsArt_ra && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE);

                    bool FV7 = FF && vsArt_ra && (KREDIT_CLASSIC || FF_TZK_x || FF_LEASING);


                    if (KV4 && vsArt_aua && minLZAUA > 0 && kalkulation.angAntKalkDto.lz < minLZAUA)
                    { //contract duration
                         MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KV4_AUA", isoCode, "Ratenabsicherung (AUA) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZAUA.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "KV4_AUA");

                    }
                    else if (KV4 && vsArt_rip && minLZRIP > 0 && kalkulation.angAntKalkDto.lz < minLZRIP)
                    { //contract duration
                         MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KV4_RIP", isoCode, "Ratenabsicherung (RIP) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZRIP.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "KV4_RIP");

                    }
                    else if (FV7 && vsArt_aua && minLZAUA > 0 && kalkulation.angAntKalkDto.lz < minLZAUA)
                    {

                        //contract duration
                         MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"),MyTranslate("CHECKANTRAG_FV7_AUA",isoCode, "Ratenabsicherung (AUA) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZAUA.ToString("0.######")) , ocheckAntAngDto.STATUS_RED, "FV7_AUA");


                    }
                    else if (FV7 && vsArt_rip && minLZRIP > 0 && kalkulation.angAntKalkDto.lz < minLZRIP)
                    {
                        //contract duration
                         MyAddErrorMessage2(outDto,String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FV7_RIP",isoCode, "Ratenabsicherung (RIP) nur bei Laufzeiten ab {0}  Monaten möglich"),  minLZRIP.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "FV7_RIP");

                    }


                }

                //RWA
                if (RWA)
                {

                    //Wenn eine obligatorische RWA-Objektversicherung vorliegt, soll die Deckungssumme (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die Deckungssumme (ANTVS:DECKUNGSSUMME) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                    //: „Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.“. 
                    AngAntVsDto rwavs = (from f in kalkulation.angAntVsDto
                                         where f.serviceType == ServiceType.RWA
                                         select f).FirstOrDefault();
                    if (rwavs != null)
                    {

                        int? optionalRWA = (from f in PrismaDaoFactory.getInstance().getPrismaServiceDao().getVSTYP()
                                            where f.SYSVSTYP == rwavs.sysvstyp
                                            select f.FLAGPAUSCHAL).FirstOrDefault();
                        if (optionalRWA.HasValue && optionalRWA.Value == 1 && rwavs.deckungssumme == 0)
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR4", isoCode, "Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.")), ocheckAntAngDto.STATUS_RED, "FR4");
                        }
                        //Wenn eine obligatorische RWA-Objektversicherung vorliegt  , soll die RWA-Indikation (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die RWA-Indikation (ANTOB:RWBASE) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                        //: „Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.“. 
                        if (optionalRWA.HasValue && optionalRWA.Value == 1 && kalkulation.angAntKalkDto.rwBase == 0)
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR5", isoCode, "Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.")), ocheckAntAngDto.STATUS_RED, "FR5");
                        }
                    }
                }
            }


            #endregion versicherung

            #region Fest
            //FD2 Spezialregelung  Kundeart<>Privatkunde
            if (FD2 && angAntKalkDto != null && (kontext.syskdtyp == KDTYPID_FIRMA))
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_FD2", isoCode, "Kein Angebot aufgrund Kundenart zugelassen.", ocheckAntAngDto.STATUS_RED, "FD2");
            }

            if (FD3 && angAntKalkDto != null && angAntKalkDto.sysobusetype == OBUSETYPEID_Privat && (kontext.syskdtyp == KDTYPID_FIRMA))
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_FD3", isoCode, "Aufgrund Kundenart (Unternehmenskunde) ist nur gewerbliche Nutzung zugelassen.", ocheckAntAngDto.STATUS_RED, "FD3");
            }

            #endregion Fest
            return outDto;
        }


        /// <summary>
        /// Quick access to a certain queue record value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        private static CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto getQueueRecordValue(String name, CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto record)
        {
            return  (from r in record.Values
                            where r.VariableName.Equals(name)
                            select r).FirstOrDefault();
        }

        /// <summary>
        /// checkAntragById / PRODUKTPRÜFUNG
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="sysvart">Vertragsart</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="b2b">B2B Flag</param>
        /// <param name="nurallgemeine">nurallgemeine</param>
        /// <param name="sysprprod">id des Produktes</param>
        /// <param name="syswfuser">syswfuser</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public override ocheckAntAngDto checkAntragByIdErweiterung(long sysid, long sysvart, String isoCode, bool b2b, bool nurallgemeine, long sysprprod, long syswfuser, long sysperole)
        {

            AntragDto antrag = getAntrag(sysid,sysperole);
            bool FF = antrag.sysprchannel == 1L;        // Fahrzeugfinanzierung
            bool KF = antrag.sysprchannel == 2L;        // Kreditfinanzierung
            bool FF_LEASING = false;
            bool FF_TZK_x = false;
            bool KREDIT_CLASSIC = false;
            bool KREDIT_EXPRESS = false;
            bool KREDIT_FIX = false;
            bool KREDIT_DISPO = false;
            bool KREDIT_DISPO_PLUS = false; // CREDIT_now Card
            bool KREDIT_DISPO_BEIDE = false;
            bool VTTYP_CASA = false;
            bool VTTYP_DIPLOMA = false;

            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();

            DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
            long angAntKalkDtoSysprproduct = 0;
            if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null)
            {
                angAntKalkDtoSysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
            }
            if (sysprprod == 0)
                sysprprod = angAntKalkDtoSysprproduct;

            if (sysvart==0)
            {
                VART va = prismaDao.getVertragsart(sysprprod);
                if(va!=null)
                    sysvart = va.SYSVART;
            }
            string vsartcode = "";
            VartDto vartDto = angAntDao.getVart(sysvart);
            if (vartDto != null && vartDto.code != null)
            {
                // sysVart=	1, 	LEASE-now	=	LEASING
                // sysVart=	2, 	CREDIT-now Fix	=	KREDIT_FIX
                // sysVart=	3, 	CREDIT-now Classic	=	KREDIT_CLASSIC
                // sysVart=	4, 	CREDIT-now Express	=	KREDIT_EXPRESS
                // sysVart=	5, 	CREDIT-now Teilzahlungskaufvertrag	=	TZK
                // sysVart=	6, 	Fester Vorschuss	=	FV
                // sysVart=	7, 	CREDIT-now Dispo	=	KREDIT_DISPO
                // sysVart=	8, 	CREDIT-now Carfinance Plus	=	TZK_PLUS
                // sysVart=	9, 	CREDIT-now Dispo Plus	=  KREDIT_DISPOPLUS

                KREDIT_CLASSIC = vartDto.code.Equals("KREDIT_CLASSIC");
                KREDIT_EXPRESS = vartDto.code.Equals("KREDIT_EXPRESS");
                KREDIT_FIX = vartDto.code.Equals("KREDIT_FIX");
                KREDIT_DISPO = vartDto.code.Equals("KREDIT_DISPO");
                KREDIT_DISPO_PLUS = vartDto.code.Equals("KREDIT_DISPOPLUS");// CREDIT_now Card
                KREDIT_DISPO_BEIDE = KREDIT_DISPO || KREDIT_DISPO_PLUS;

                FF_LEASING = vartDto.code.Equals("LEASING");
                FF_TZK_x = vartDto.code.Contains("TZK");        // TZK oder TZK_PLUS
            }
            


            //EL by Ruleengine
            Boolean useRuleEngine = AppConfig.Instance.getBooleanEntry("RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULEENGINE", false, "ANTRAGSASSISTENT");//AppConfig.Instance.getBooleanEntry("RULEENGINE_EXPECTEDLOSS", "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
            String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_EXPECTEDLOSS", "USE_RULESET", "");

            if(FF)
            {
                //useRuleEngine = AppConfig.Instance.getBooleanEntry("RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
                ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULESET", "");
            }

            if (!b2b && useRuleEngine)
            {
                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].VariableName = "PP";
                vars[0].LookupVariableName = "SPRACHE";
                vars[0].Value = isoCode;
                BPEBo.getQueueData(sysid, "ANTRAG", new String[] { "qRULES" }, ruleCode, vars, syswfuser, null);
            }


            String secCode = "RULEENGINE_PRODUKTPRUEFUNG_B2B";
            if (!b2b)
                secCode = "RULEENGINE_PRODUKTPRUEFUNG_EAI";

            //useRuleEngine = AppConfig.Instance.getBooleanEntry(secCode, "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
            bool berechnen = !nurallgemeine;

            if (FF && berechnen && (FF_LEASING || FF_TZK_x) && !b2b)
            {
                trBo.remostaffelAnlegen(sysid, sysprprod,sysperole);  //BNR11 BNRELF-2106 REMO-Staffel immer angelegt
            }

            //Produktprüfung by RuleEngine
            if (useRuleEngine)
            {
                ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", secCode, "USE_RULESET", "");

                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[4];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].VariableName = "PP";
                vars[0].LookupVariableName = "SPRACHE";
                vars[0].Value = isoCode;

                vars[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[1].VariableName = "PP";
                vars[1].LookupVariableName = "ALLGEMEIN";
                vars[1].Value = nurallgemeine?"1":"0";

                vars[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[2].VariableName = "PP";
                vars[2].LookupVariableName = "SYSPRPRODUCT";
                vars[2].Value =""+ sysprprod;

                vars[3] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[3].VariableName = "PP";
                vars[3].LookupVariableName = "SYSVART";
                vars[3].Value = "" + sysvart;

                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(sysid, "ANTRAG", new String[] { "qRULES" }, ruleCode, vars, syswfuser, null);

                ocheckAntAngDto rval = new ocheckAntAngDto();
                rval.errortext = new List<string>();
                rval.code = new List<string>();
                rval.status = ocheckAntAngDto.STATUS_GREEN;

                CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto queue = (from f in queueResult
                                                                            where f.Name.Equals("qRULES")
                                                                            select f).FirstOrDefault();

                foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto record in queue.Records)
                {
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeVar = getQueueRecordValue("F01", record);
                    if (codeVar == null) continue;

                    rval.code.Add(codeVar.Value);

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto errText = getQueueRecordValue("F02", record);
                    rval.errortext.Add(errText.Value);


                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeBoundary = getQueueRecordValue("F03", record);//zb.b 250000

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto codeStatus = getQueueRecordValue("F04", record);

                    if (rval.status.Equals(ocheckAntAngDto.STATUS_RED)) continue;
                    if (codeStatus==null || codeStatus.Value==null|| codeStatus.Value.Equals("green") || codeStatus.Value.Equals("gruen")) continue;

                    if (codeStatus.Value.Equals("red")|| codeStatus.Value.Equals("rot"))
                        rval.status = ocheckAntAngDto.STATUS_RED;
                    if (codeStatus.Value.Equals("gelb")|| codeStatus.Value.Equals("yellow"))
                        rval.status = ocheckAntAngDto.STATUS_YELLOW;


                }

               


                return rval;
            }

            //-------------------------csharp Produktprüfung-------------------------------------

            IKundenRisikoBo krBo = BOFactory.getInstance().createKundenRisikoBO(isoCode);
            

            //Setup prKontext--------------------------------------------------
            prKontextDto kontext = new prKontextDto();

            

            
            String vtTYPcode = String.Empty;
            String vtTYPart = String.Empty;

            //BNRSIEBEN-409
            VTTYP vtTYP = null;
            if (sysprprod == 0)
            {
                if (antrag.sysvttyp != null && antrag.sysvttyp != 0)
                {
                    kontext.sysvttyp = antrag.sysvttyp.Value;
                    vtTYP = prismaDao.getVttypById(kontext.sysvttyp);
                }
                else
                {
                    vtTYP = prismaDao.getVttyp(angAntKalkDtoSysprproduct);
                }
            }
            else
            {
                vtTYP = prismaDao.getVttyp(sysprprod);
            }

            if (vtTYP != null)
            {
                kontext.sysvttyp = vtTYP.SYSVTTYP;
                vtTYPcode = vtTYP.CODE;
                vtTYPart = vtTYP.UNTERART;
            }

            if (nurallgemeine)
            {
                if (sysvart == 0)
                {
                    // VertragsArt ermitteln

                    VART vart = prismaDao.getVertragsart(angAntKalkDtoSysprproduct);
                    if (vart == null)
                    {
                        throw new ArgumentException("The vart is null because Antrag (SYSID = " + sysid + ") kalkulation AngAntKalkDtos sysprproduct is 0.");
                    }
                    sysvart = vart.SYSVART;
                }
            }
            else
            {
                // STAS Ticket 2011122010000472
                // VertragsArt ermitteln
                VART vart = prismaDao.getVertragsart(angAntKalkDtoSysprproduct);
                if (vart == null && sysvart == 0)
                {
                    throw new ArgumentException("The vart is null because Antrag (SYSID = " + sysid + ") kalkulation AngAntKalkDtos sysprproduct is 0.");
                }
                if (sysvart == 0 && vart != null)
                {
                    sysvart = vart.SYSVART;
                    kontext.sysprproduct = angAntKalkDtoSysprproduct;
                }
                if (sysvart != 0 && vart != null && sysvart == vart.SYSVART)
                {
                    kontext.sysprproduct = angAntKalkDtoSysprproduct;
                }
            }

            if (antrag.sysprchannel != null)
            {
                kontext.sysprchannel = (long)antrag.sysprchannel;
            }

            kontext.sysvart = sysvart;

            if (antrag.angAntObDto != null)
            {
                kontext.sysobart = antrag.angAntObDto.sysobart;
                kontext.sysobtyp = antrag.angAntObDto.sysobtyp;
            }
            //END Setup prKontext--------------------------------------------------

            
     
            bool KEL1 = KF && berechnen && !b2b;


            ocheckAntAngDto outDto = new ocheckAntAngDto();
            outDto.errortext = new List<string>();
            outDto.code = new List<string>();
            outDto.status = ocheckAntAngDto.STATUS_GREEN;

            //BNOWR8
            long? sysvg = angAntDao.getSysVglgd(sysid);
            String scorebezeichnung = angAntDao.getScorebezeichnung(sysid);
            VClusterParamDto clusterParam = null;
            VClusterDto cluster = null;
            VgDto vg = null;
            int? fkp = null, pkp = null, umsp = null, bwgp = null, strap = null, fel1r = null;
            if (sysvg != null && sysvg > 0)
            {
                vg = vgDao.getVg((long)sysvg);

            }

            // KUNDE
            KundePlusDto kundePlusDto = new KundePlusDto();
            if (b2b)
            {
                kundePlusDto = kundeDao.getItPlusbySysAntrag(antrag.sysid, antrag.sysit);
            }
            else
            {
                kundePlusDto = kundeDao.getKundebySysAntrag(antrag.sysid, antrag.syskd);
            }

            if (kundePlusDto != null)
            {
                // Leerzeichen und Nulls entfernen
                kundePlusDto.auslausweisCode = MyRemoveSpaces(kundePlusDto.auslausweisCode);
                if (kundePlusDto.landWohnsitz != null)
                {
                    kundePlusDto.landWohnsitz.iso = MyRemoveSpaces(kundePlusDto.landWohnsitz.iso);
                }
                if (kundePlusDto.pkz != null)
                {
                    kundePlusDto.pkz.beruflichCode = MyRemoveSpaces(kundePlusDto.pkz.beruflichCode);
                }
                if (kundePlusDto.landNationalitaet != null)
                {
                    kundePlusDto.landNationalitaet.iso = MyRemoveSpaces(kundePlusDto.landNationalitaet.iso);
                }

                antrag.kunde = Mapper.Map<KundePlusDto, KundeDto>(kundePlusDto);
            }
            if (antrag.kunde != null)
            {
                kontext.syskdtyp = antrag.kunde.syskdtyp;
            }
            kontext.sysbrand = antrag.sysbrand;
            kontext.sysprkgroup = angAntDao.getPrkgroupByAntragID(sysid);
            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> paramsInput = prismaParameterBo.listAvailableParameter(kontext);



            // Definition
            #region Flags-Definitionen

            // EL-Kalk soll aber nur dann erfolgen, wenn ein EAIPAR-Statement (EAIPAR:CODE = 'EL_KALK') zum Antrag eine 1 liefert 
            bool EL_KALK = trBo.getEL_KALKFlag(sysid);
            EL_KALK = (antrag.kalkulation.angAntKalkDto.lz < 3) ? false : EL_KALK;


            if (!String.IsNullOrEmpty(vtTYPcode))
            {
                VTTYP_CASA = vtTYPart.Equals("CASA");
                VTTYP_DIPLOMA = vtTYPart.ToUpper().Equals("DIPLOMA");
            }



            bool KB1 = KF && berechnen && KREDIT_CLASSIC;
            bool KB2 = KF && berechnen && KREDIT_CLASSIC;
            bool KB3 = KF && berechnen && KREDIT_CLASSIC;
            bool KB4 = KF && berechnen && KREDIT_DISPO_BEIDE;
            bool KB5 = KF && berechnen && KREDIT_DISPO_BEIDE;
            bool KB6 = KF && berechnen && KREDIT_EXPRESS;
            bool KB7 = KF && berechnen && KREDIT_EXPRESS;
            bool KB8 = KF && berechnen && KREDIT_DISPO_PLUS;
            bool KB9 = KF && berechnen && KREDIT_DISPO_PLUS;

            bool KN1 = KF && KREDIT_DISPO_BEIDE;
            bool KN2 = KF && KREDIT_EXPRESS;
            bool KN3 = KF && KREDIT_CLASSIC;
            bool KN4 = KF && berechnen && KREDIT_DISPO_PLUS;

            bool KD1 = KF && KREDIT_EXPRESS;
            bool KD2 = KF && KREDIT_DISPO_BEIDE;
            bool KD3 = KF && KREDIT_EXPRESS;
            bool KD4 = KF && KREDIT_EXPRESS;
            bool KD5 = berechnen && (KREDIT_DISPO_BEIDE || KREDIT_CLASSIC);

            bool KA1 = KF && berechnen && KREDIT_CLASSIC;
            bool KA2 = KF && berechnen && KREDIT_EXPRESS;
            bool KA3 = KF && berechnen && KREDIT_DISPO_BEIDE;

            bool KR1 = KF && KREDIT_DISPO_BEIDE;
            bool KR2 = KF && KREDIT_EXPRESS;
            bool KR3 = KF && KREDIT_CLASSIC;

            bool KL1 = KF && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE || KREDIT_EXPRESS);
            // Ticket#2012062010000115 : Der CR muss noch auf die Kreditart Classic eingeschränkt werden.
            // für KREDIT_DISPO und KREDIT_EXPRESS gilt die alte Logik
            bool KL4 = KF && berechnen && (KREDIT_DISPO_BEIDE || KREDIT_EXPRESS);
            bool KL4_CLASSIC = KF && berechnen && KREDIT_CLASSIC;

            bool KL5 = KF && berechnen && KREDIT_EXPRESS;
            bool KL6 = KF && berechnen && KREDIT_DISPO_BEIDE;
            bool KL7 = KF && berechnen && KREDIT_EXPRESS;
            bool KL8 = KF && berechnen && KREDIT_DISPO_BEIDE;

            bool FA1 = FF && berechnen && FF_LEASING;
            bool FA2 = FF && berechnen && FF_TZK_x;
            bool FA3 = FF && berechnen && KREDIT_CLASSIC;

            bool FB1 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB2 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB3 = FF && berechnen && (FF_TZK_x || KREDIT_CLASSIC);
            bool FB4 = FF && FF_LEASING;
            bool FB5 = FF && FF_LEASING;
            bool FB6 = FF && FF_LEASING;
            bool FB7 = FF && FF_LEASING;
            bool FB8 = FF && KREDIT_CLASSIC;
            bool FB9 = FF && KREDIT_CLASSIC;

            bool FN1 = FF && (FF_LEASING || FF_TZK_x);
            bool FN2 = FF && KREDIT_CLASSIC;

            bool FD1 = FF && berechnen && FF_LEASING;
            bool FD2 = FF && (FF_TZK_x || KREDIT_CLASSIC);
            bool FD3 = FF && FF_LEASING;
            bool FD4 = FF && FF_LEASING;
            bool FD5 = FF;

            bool FRW1 = FF && berechnen && FF_LEASING;
            bool FRW2 = FF && berechnen && FF_LEASING;
            bool FRW3 = FF && berechnen && FF_TZK_x;

            bool FR1 = FF && FF_LEASING;
            bool FR2 = FF && FF_TZK_x;
            bool FR3 = FF && KREDIT_CLASSIC;
            bool RWA = FF && FF_LEASING;

            bool FO1 = FF && berechnen && FF_LEASING;
            bool FO2 = FF && berechnen && FF_LEASING;
            bool FO3 = FF && FF_LEASING;

            bool FL1 = FF && berechnen && FF_LEASING;
            bool FL2 = FF && berechnen && (FF_TZK_x || KREDIT_CLASSIC);
            bool FL3 = FF && berechnen && FF_LEASING;

            bool FEL1 = FF && berechnen && (FF_LEASING || FF_TZK_x) && vg != null && v_clusterNames.Contains(vg.name);
            bool FEL2 = FF && berechnen && (FF_LEASING || FF_TZK_x) && vg != null && v_clusterNames.Contains(vg.name);

            

            
            int straccount = 0;
            string fform = angAntDao.getFform(sysid);
            straccount = angAntDao.getStraccount(antrag.sysVM);

            bool ums = (fform != "") && fform != null;
            bool stra = (straccount == 1);
            bool bwg = (angAntDao.getFlagBwgarantie(antrag.sysid) == 1);


            if (scorebezeichnung == null || scorebezeichnung.Equals(""))
            {
                FEL1 = false;
                FEL2 = false;
            }

            if (FEL1 || FEL2)
            {
                try
                {
                   
                    clusterParam = getClusterParam(vg, scorebezeichnung);
                    fkp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "FK", 0);
                    pkp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "PK", 0);
                    umsp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "UMS", 0);
                    bwgp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "BWG", 0);
                    strap = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "STRA", 0);
                    fel1r = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "FEL1R", 0);

                }
                catch (Exception ex)
                {
                    _log.Info("Gültige Cluster Parameter nicht gefunden " + ex);
                    FEL1 = false;
                    FEL2 = false;
                }
                bool PK = antrag.kunde != null ? (antrag.kunde.syskdtyp == KDTYPID_PRIVAT && pkp == 1) : false;
                bool FK = antrag.kunde != null ? (antrag.kunde.syskdtyp == KDTYPID_FIRMA && fkp == 1) : false;

                FEL1 = FEL1 && (PK || FK);
                FEL2 = FEL2 && (PK || FK);
            }

            if (FF && berechnen && (FF_LEASING || FF_TZK_x) && !b2b)
            {
                if (EL_KALK)
                {
                    trBo.deleteVarianteUndDERulsByAntrag(sysid);
                    cluster = trBo.getV_cluster(sysid, true,sysprprod,sysperole);
                    angAntDao.saveClusterInAntrag(sysid, cluster);
                }
                else
                {

                        trBo.deleteVarianteUndDERulsByAntrag(sysid);
                        VClusterDto vClusterDtotemp = new VClusterDto();
                        vClusterDtotemp.v_el_betrag = 0;
                        vClusterDtotemp.v_el_prozent = 0;
                        vClusterDtotemp.v_prof = 0;
                        angAntDao.saveClusterInAntrag(sysid, vClusterDtotemp);
                        trBo.remostaffelAnlegen(sysid,sysprprod,sysperole);  //BNR11 BNRELF-2106 REMO-Staffel immer angelegt
                        FEL1 = false;
                        FEL2 = false;

                }

            }

            bool isclustersaved = false;
            if (KEL1)
            {
                if (krBo.KRBerechnen(sysid))
                {
                    trBo.deleteVarianteUndDERulsByAntrag(sysid);
                    cluster = krBo.getELValues(sysid, kontext);
                    angAntDao.saveClusterInAntrag(sysid, cluster);
                    isclustersaved = true;
                }
                else
                {
                    VClusterDto vClusterDtotemp = new VClusterDto();
                    vClusterDtotemp.v_el_betrag = 0;
                    vClusterDtotemp.v_el_prozent = 0;
                    vClusterDtotemp.v_prof = 0;
                    angAntDao.saveClusterInAntrag(sysid, vClusterDtotemp);
                    KEL1 = false;
                }
            }




            bool RWGA = !(FF && FF_LEASING);

            #endregion //Definition

            //PRODUKTPRÜFUNG 
            #region PARAMETER PRÜFUNG

            // True wenn KM-Stand für Fahrzeug unterschritten (FO3):
            bool kalkBorderUb_KmObjFlag = false;
            // True wenn Alter für Fahrzeug unterschritten (FO3):
            bool kalkBorderUb_AlterObjFlag = false;
            double maxAUA = 65;
            double maxRIP = 70;
            double minLZAUA = 0;
            double minLZRIP = 0;

            double minimalBetrag = 0;
            foreach (var prParam in paramsInput)
            {
                if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndalterAUA).Equals(prParam.meta))
                {
                    maxAUA = prParam.maxvaln;
                    continue;
                }
                if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndalterRIP).Equals(prParam.meta))
                {
                    maxRIP = prParam.maxvaln;
                    continue;
                }

                if (EnumUtil.GetStringValue(PrismaParameters.KALK_BORDER_LZ_AUA).Equals(prParam.meta))
                {
                    minLZAUA = prParam.minvaln;
                    continue;
                }
                if (EnumUtil.GetStringValue(PrismaParameters.KALK_BORDER_LZ_RIP).Equals(prParam.meta))
                {
                    minLZRIP = prParam.minvaln;
                    continue;
                }

                if (EnumUtil.GetStringValue(PrismaParameters.CasaEigentuemerSeitTagen).Equals(prParam.meta))
                {
                    if (VTTYP_CASA)
                    {
                        DateTime eigentSeit = angAntDao.getEigentuemerSeit(antrag.sysid);

                        int tage = ((TimeSpan)(antrag.erfassung - eigentSeit)).Days;
                        if (tage >= prParam.minvaln)
                        {
                            // Green
                        }
                        else
                        {
                            // Red
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_CASA", isoCode, "Dauer von {0} Tagen Eigentümer-Seit unterschritten."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "CASA");
                        }
                    }
                }

                if (EnumUtil.GetStringValue(PrismaParameters.DiplomaKundenalter).Equals(prParam.meta))
                {
                    if (VTTYP_DIPLOMA)
                    {
                        if (kundePlusDto.gebdatum != null)
                        {
                            double jahre = MyGetJahreDiff(DateTime.Now, (DateTime)kundePlusDto.gebdatum);
                            if (jahre > prParam.minvaln)
                            {
                                // Green
                            }
                            else
                            {
                                // Red
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_DIPLOMA", isoCode, "Meldungstext: Mindestalter von {0} Jahren unterschritten."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "DIPLOMA");
                            }
                        }
                    }
                }



                // KALK_BORDER_INITLADUNG
                if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderInitLadung).Equals(prParam.meta))
                {
                    // Parameter prüfen nur wenn Kartenauszahlung > 0 (wenn 0 dann Grün)
                    if (antrag.kalkulation.angAntKalkDto.initLadung > 0)
                    {
                        if (KB9 && antrag.kalkulation.angAntKalkDto != null)
                        {
                            if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.initLadung, prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB9", isoCode, "Kartenauszahlung darf den Betrag von {0} nicht überschreiten."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB9");
                            }
                        }
                        if (KB8 && antrag.kalkulation.angAntKalkDto != null)
                        {
                            if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.initLadung, prParam.minvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB8", isoCode, "Kartenauszahlung darf den Betrag von {0} nicht unterschreiten."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB8");
                            }
                        }
                    }
                }

                //A = Alter -------------------------
                if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndalterKunde).Equals(prParam.meta) && berechnen)
                {   //KA1||KA2||KA3||FA1||FA2||FA3
                    if (antrag.kunde != null && antrag.kunde.syskdtyp == KDTYPID_PRIVAT && antrag.kalkulation.angAntKalkDto != null)
                    {
                        double endAlterKunde = MyGetEndAlterKunde(aktuell.AddMonths(antrag.kalkulation.angAntKalkDto.lz), kundePlusDto.gebdatum);
                        if (MyIsGreaterThanMaxVal(endAlterKunde, prParam.maxvaln))
                        {
                            if (KA1 || KA2 || KA3)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_KA", isoCode, "Maximales Alter bei Vertragsende überschritten.", ocheckAntAngDto.STATUS_RED, "KA");
                            }

                            if (FA1 || FA2 || FA3)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FA", isoCode, "Maximales Alter bei Vertragsende überschritten.", ocheckAntAngDto.STATUS_RED, "FA");
                            }
                        }
                        else
                            if (MyIsSmallerThanMinVal(endAlterKunde, prParam.minvaln))
                            {
                                if (KA1 || KA2 || KA3)
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_KA_Min", isoCode, "Minimales Alter bei Vertragsende unterschritten.", ocheckAntAngDto.STATUS_RED, "KA");
                                }

                                if (FA1 || FA2 || FA3)
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FA_Min", isoCode, "Minimales Alter bei Vertragsende unterschritten.", ocheckAntAngDto.STATUS_RED, "FA");
                                }
                            }
                    }
                }

                // R = Risikoklasse -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderKundenScore).Equals(prParam.meta))
                {   // FR1, FR2 ,FR3, KR1, KR2, KR3
                    int scoreInDedetail = angAntDao.getScoreInDedetailBySysantrag(sysid);
                    if (scoreInDedetail > 0)
                    {
                        if (MyIsSmallerThanMinVal(scoreInDedetail, prParam.minvaln))
                        {
                            if (KR1 || KR2 || KR3)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_KR", isoCode, "Kundenbonität für Produktangebot nicht ausreichend.", ocheckAntAngDto.STATUS_YELLOW, "KR");
                            }

                            if (FR1 || FR2 || FR3)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FR", isoCode, "Kundenbonität für Produktangebot nicht ausreichend.", ocheckAntAngDto.STATUS_YELLOW, "FR");
                            }
                        }
                    }
                }

                // B = Betragsgrenzen -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderRate).Equals(prParam.meta))
                {
                    if (KB3 || FB3)
                    {
                        if (antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_KB3I", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung - Mindesthöhe für Kreditrate.", ocheckAntAngDto.STATUS_RED, "KB3I");
                        }
                        else
                        {
                            if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.rateBrutto, prParam.minvaln))
                            {
                                if (KB3)
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB3", isoCode, "Mindesthöhe für Kreditrate wurde unterschritten. Mindesthöhe = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB3");
                                }

                                if (FB3)
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB3", isoCode, "Mindesthöhe für Kreditrate wurde unterschritten. Mindesthöhe = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB3");
                                }
                            }
                            else
                                if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.rateBrutto, prParam.maxvaln))
                                {
                                    if (KB3)
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB3_Max", isoCode, "Höhe der Kreditrate wurde überschritten. Maximale Höhe = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB3");
                                    }

                                    if (FB3)
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB3_Max", isoCode, "Höhe der Kreditrate wurde überschritten. Maximale Höhe = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "FB3");
                                    }
                                }
                        }
                    }
                }

                // R = Restwert -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderRW).Equals(prParam.meta))
                {   //FR1 FR3
                    if (FRW1 || FRW3)
                    {
                        if (antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FRWI", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung Minimaler Restwert", ocheckAntAngDto.STATUS_RED, "FRWI");
                        }
                        else
                        {   // FR1
                            if (FRW1)
                            {
                                if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.minvalp)))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW1", isoCode, "Minimaler Restwert von {0} % wurde unterschritten."), prParam.minvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.maxvalp)))
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW1_Max", isoCode, "Maximaler Restwert von {0} % wurde überschritten."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
                                    }
                            }

                            // FR3
                            if (FRW3 && antrag.kalkulation.angAntKalkDto.rwBrutto > 0)
                            {
                                double betrag = MyCalcBetragToCompare_FRW3(antrag.kalkulation.angAntKalkDto, berechnen);

                                if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(betrag, prParam.minvalp)))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW3", isoCode, "Mindestrestrate von {0} % wurde unterschritten."), prParam.minvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(betrag, prParam.maxvalp)))
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW3_Max", isoCode, "Maximale Restrate von {0} % wurde überschritten."), prParam.maxvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                                    }
                            }
                        }
                    }
                }

                // FRW2 = Restwert -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderRWschwelleKaution).Equals(prParam.meta))
                {
                    if (FRW2)
                    {
                        if (antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FRWI", isoCode, "Kein Angebot aufgrund unvollständiger Daten Prüfung Minimaler Restwert.", ocheckAntAngDto.STATUS_RED, "FRWI");
                        }
                        else
                        {
                            if (antrag.kalkulation.angAntKalkDto.depot == 0 || antrag.kalkulation.angAntKalkDto.depot < antrag.kalkulation.angAntKalkDto.rwBrutto)
                            {
                                if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.minvalp)))       // FR2
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW2", isoCode, "Mindestrestwert von {0} %  wurde unterschritten. Bitte erfassen Sie eine Kaution in Höhe des Restwertes oder passen Sie den Restwert entsprechend an."), prParam.minvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW2");
                                }
                                else
                                    if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.rwBrutto, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.maxvalp)))       // FR2
                                    {
                                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW2_Max", isoCode, "Maximaler Restwert von {0} %  wurde überschritten. Bitte erfassen Sie eine Kaution in Höhe des Restwertes oder passen Sie den Restwert entsprechend an."), prParam.maxvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW2");
                                    }
                            }
                        }
                    }
                }

                // O = Objekt Alter  -------------------------       
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderEndAlterObj).Equals(prParam.meta) && berechnen)
                {   //F01
                    if (FO1)
                    {
                        // bool maclientNeedsEZL = (antrag.angAntObDto.erstzulassung == null && !b2b);
                        bool b2bClientNeedsEZLWhenNotNew = (kontext.sysobart != OBARTID_NEU && b2b && antrag.angAntObDto.erstzulassung == null);
                        bool b2bClientNoNeedEZLWhenNew = (kontext.sysobart == OBARTID_NEU && b2b && antrag.angAntObDto.erstzulassung == null);
                        DateTime? erstzulassungTempFO1 = converterB2BNullDate(antrag.angAntObDto.erstzulassung);
                        //O = Objekt
                        //FO1
                        if (b2bClientNoNeedEZLWhenNew)
                        {
                            ;//dont check this when b2b has a new vehicle and no ezl
                        }
                        else if (antrag.angAntObDto == null || b2bClientNeedsEZLWhenNotNew || antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FO1I", isoCode, "Kein Angebot aufgrund unvollständiger Daten  - Prüfung Maximalalter.", ocheckAntAngDto.STATUS_RED, "FO1I");
                        }
                        else
                        {

                            double monatDiff = MyGetMonatDiff(aktuell.AddMonths(antrag.kalkulation.angAntKalkDto.lz), erstzulassungTempFO1);
                            if (MyIsGreaterThanMaxVal(monatDiff, prParam.maxvaln))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FO1", isoCode, "Kein Angebot aufgrund Überschreitung Maximalalter bei Ende der Vertragslaufzeit.", ocheckAntAngDto.STATUS_YELLOW, "FO1");
                            }
                            else
                                if (MyIsSmallerThanMinVal(monatDiff, prParam.minvaln))
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FO1_Min", isoCode, "Kein Angebot aufgrund Unterschreitung Minimalalter bei Ende der Vertragslaufzeit.", ocheckAntAngDto.STATUS_YELLOW, "FO1");
                                }
                        }
                    }
                }

                //O = Objekt KM -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndll).Equals(prParam.meta))
                {
                    if (FO2)
                    {
                        //FO2
                        if (antrag.angAntObDto == null || antrag.kalkulation == null || antrag.kalkulation.angAntKalkDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FO2I", isoCode, "Kein Angebot aufgrund unvollständiger Daten - Maximaler Kilometer.", ocheckAntAngDto.STATUS_RED, "FO2I");
                        }
                        else
                        {
                            double ubnahmeKm = antrag.kalkulation.angAntKalkDto.ll * ((float)antrag.kalkulation.angAntKalkDto.lz / (float)12) + antrag.angAntObDto.ubnahmeKm;
                            if (MyIsGreaterThanMaxVal(ubnahmeKm, prParam.maxvaln))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FO2", isoCode, "Kein Angebot aufgrund Überschreitung maximaler Kilometer am Vertragsende.", ocheckAntAngDto.STATUS_YELLOW, "FO2");
                            }
                            else
                                if (MyIsSmallerThanMinVal(ubnahmeKm, prParam.minvaln))
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FO2_Min", isoCode, "Kein Angebot aufgrund Unterschreitung minimaler Kilometer am Vertragsende.", ocheckAntAngDto.STATUS_YELLOW, "FO2");
                                }
                        }
                    }
                }

                // O = Objekt Neuwagen KM -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderUbnahmekm).Equals(prParam.meta))
                {
                    if (FO3)
                    {
                        if (antrag.angAntObDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FO2INEW", isoCode, "Kein Angebot aufgrund unvollständiger Daten - Maximalwerte KM-Stand für Fahrzeug.", ocheckAntAngDto.STATUS_RED, "FO3");
                        }
                        else
                        {
                            if (MyIsGreaterThanMaxVal(antrag.angAntObDto.ubnahmeKm, prParam.maxvaln))
                            {
                                MyAddErrorMessage(outDto, "CCHECKANTRAG_FO3KM", isoCode, "Maximaler KM-Stand für Fahrzeug überschritten.", ocheckAntAngDto.STATUS_RED, "FO3");
                            }
                            else
                                if (MyIsSmallerThanMinVal(antrag.angAntObDto.ubnahmeKm, prParam.minvaln))
                                {
                                    if (antrag.angAntObDto.sysobart == OBARTID_NEU)
                                    {
                                        MyAddErrorMessage(outDto, "CCHECKANTRAG_FO3KM_Min", isoCode, "Minimaler KM-Stand für Fahrzeug unterschritten.", ocheckAntAngDto.STATUS_RED, "FO3");
                                    }
                                    else
                                    {
                                        kalkBorderUb_KmObjFlag = true;
                                    }
                                }
                        }
                    }
                }

                // O = Objekt Neuwagen Alter -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderUbAlterObj).Equals(prParam.meta))
                {
                    if (FO3)
                    {
                        if (antrag.angAntObDto == null)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FO1INEW", isoCode, "Kein Angebot aufgrund unvollständiger Daten - Maximalwerte Alter für Fahrzeug.", ocheckAntAngDto.STATUS_RED, "FO3");
                        }
                        else
                        {
                            DateTime? erstzulassungTemp = converterB2BNullDate(antrag.angAntObDto.erstzulassung);

                            // Wenn die Erstzulassung in der Zukunft liegt, dann soll die Regel nie greifen
                            if (erstzulassungTemp < DateTime.Now.Date)
                            {
                                double monatDiff = MyGetMonatDiff(aktuell, erstzulassungTemp);
                                if (MyIsGreaterThanMaxVal(monatDiff, prParam.maxvaln))
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FO3AGE", isoCode, "Maximales Alter für Fahrzeug überschritten.", ocheckAntAngDto.STATUS_RED, "FO3");
                                }
                                else
                                    if (MyIsSmallerThanMinVal(monatDiff, prParam.minvaln))
                                    {
                                        if (antrag.angAntObDto.sysobart == OBARTID_NEU)
                                        {
                                            MyAddErrorMessage(outDto, "CHECKANTRAG_FO3AGE_Min", isoCode, "Minimales Alter für Fahrzeug unterschritten.", ocheckAntAngDto.STATUS_RED, "FO3");
                                        }
                                        else
                                        {
                                            kalkBorderUb_AlterObjFlag = true;
                                        }
                                    }
                            }
                        }
                    }
                }

                // D = Diverse (Spezialregelungen) -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.kalkBorderSonderZahlungProzent).Equals(prParam.meta))
                {
                    // Höhe der ersten Rate > x% des Barkaufpreises
                    if (FD1)
                    {
                        double maxVal = MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.maxvalp);
                        double minVal = MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.minvalp);
                        if (antrag.kalkulation.angAntKalkDto != null)
                        {
                            if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.szBrutto, maxVal))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FD1", isoCode, "Erste grosse Leasingrate darf maximal {0} % des Barkaufpreises betragen."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                            }
                            else
                                if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.szBrutto, minVal))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FD1_Min", isoCode, "Erste grosse Leasingrate muss minimal {0} % des Barkaufpreises betragen."), prParam.minvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                                }
                        }
                    }
                }

                //B = Betragsgrenze -------------------------
                else if (EnumUtil.GetStringValue(PrismaParameters.KalkBorderBgIntern).Equals(prParam.meta))
                {
                    // _log.Debug(String.Format("KALK_BORDER_BGINTERN: checkAntragByIdErweiterung(sysid = {0}, sysvart = {1}, isoCode = {2} ,b2b = {3}, nurallgemeine = {4})", sysid,  sysvart,  isoCode,  b2b,  nurallgemeine));

                    if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null)
                    {
                        // überschritten
                        if (FB1)
                        {
                            if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB12(antrag.kalkulation.angAntKalkDto, berechnen), prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB1", isoCode, "Maximaler Kreditbetrag wurde überschritten. Maximaler Kreditbetrag = {0} (FB1)."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "FB1");
                            }
                        }

                        if (FB7)
                        {
                            if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto, berechnen), prParam.maxvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB7", isoCode, "Finanzierungsbetrag wurde überschritten. Maximaler Finanzierungsbetrag = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "FB7");
                            }
                        }

                        if (MyIsGreaterThanMaxVal(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.maxvaln))
                        {
                            if (KB1)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB1", isoCode, "Maximaler Kreditbetrag für CREDIT-now Classic überschritten. Maximaler Kreditbetrag = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB1");
                            }
                            if (KB4)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB4", isoCode, "Maximaler Kreditlimite wurde überschritten. Maximale Kredilimite = {0}."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB4");
                            }
                            if (KB7)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB7", isoCode, "Maximaler Kreditbetrag wurde überschritten. Maximaler Kreditbetrag = {0} (KB7)."), prParam.maxvaln), ocheckAntAngDto.STATUS_RED, "KB7");
                            }
                        }


                        // Unterschritten
                        minimalBetrag = prParam.minvaln;
                        if (FB2)
                        {
                            // _log.Debug(String.Format("FB2:prParam.minvaln = {0}, bginternbrutto = {1}", prParam.minvaln, antrag.kalkulation.angAntKalkDto.bginternbrutto));

                            if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB12(antrag.kalkulation.angAntKalkDto, berechnen), prParam.minvaln))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB2", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (FB2)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB2");
                            }
                        }

                        if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto, berechnen), prParam.minvaln))
                        {
                            // _log.Debug(String.Format("FB456: prParam.minvaln = {0}, bginternbrutto = {1}", prParam.minvaln, antrag.kalkulation.angAntKalkDto.bginternbrutto));
                            // _log.Debug(String.Format("FB456: MyCalcBetragToCompare_FB4567() = {0}", MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto, berechnen)));

                            if (FB4 && antrag.angAntObDto.sysobart == OBARTID_NEU)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB4", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB4)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB4");
                            }
                            if (FB5 && antrag.angAntObDto.sysobart == OBARTID_OCCASION)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB5", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB5)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB5");
                            }
                            if (FB6 && antrag.kalkulation.angAntKalkDto.sysobusetype == OBUSETYPEID_DemoLeasing)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB6", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB6)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "FB6");
                            }
                        }

                        if (MyIsSmallerThanMinVal(antrag.kalkulation.angAntKalkDto.bginternbrutto, prParam.minvaln))
                        {
                            if (KB2)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB2", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (KB2)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB2");
                            }
                            if (KB5)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB5", isoCode, "Mindestkreditlimite wurde unterschritten. Mindestlimite = {0}."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB5");
                            }
                            if (KB6)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KB6", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (KB6)."), prParam.minvaln), ocheckAntAngDto.STATUS_RED, "KB6");
                            }
                        }
                    }
                }

                // CR 27169
                else if (EnumUtil.GetStringValue(PrismaParameters.MaxLimitProzent).Equals(prParam.meta))
                {
                    if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null && kundePlusDto != null)
                    {
                        // KL7 = KF && berechnen && KREDIT_EXPRESS
                        if (KL7 && !isclustersaved)
                        {
                            // If ANTKALK:BGINTERNBRUTTO <= (MAX(KREMO:KREDINKKG)*MAXVAL)
                            // then = <<green>>
                            // else = <<red>>
                            if (antrag.kalkulation.angAntKalkDto.bginternbrutto > MyCalcPercentToValue(kundePlusDto.kredinkkg, prParam.maxvalp))
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KL7",
                                    isoCode, "Eingegebener Kreditbetrag überschreitet zulässiges Maximum aufgrund Budget ({0}% von Kreditlimite)."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "KL7");
                            }
                        }
                        // KL8 = KF && berechnen && KREDIT_DISPO
                        if (KL8 && !isclustersaved)
                        {
                            // Liefert diese Methode einen Wert größer 0, dann wurde eine fakultative Ratenabsicherung gewählt
                            if (angAntDao.getFakultativeRatenabsicherung(sysid) > 0)
                            {
                                if (antrag.kalkulation.angAntKalkDto.bginternbrutto > MyCalcPercentToValue(kundePlusDto.kredinkkg, prParam.maxvalp))
                                {
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KL8",
                                        isoCode, "Eingegebene Kreditlimite überschreitet zulässiges Maximum von {0}% (Saldoabsicherung)."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "KL8");
                                }
                            }
                        }
                    }
                    // KD4 = KF && KREDIT_EXPRESS
                    if (KD4 && kundePlusDto != null && kundePlusDto.pkz != null && kundePlusDto.pkz.beruflichCode != null)
                    {
                        // If PKZ.BERUFLICHCODE not in ( 3 (Selbständig), 5 (Hausfrau), 8 (Privatier), 9 (Stellenlos) )
                        // then = <<green>>
                        // else = <<red>>
                        String[] berCodesBlackList = { "3", "5", "8", "9" };
                        if (berCodesBlackList.Contains(kundePlusDto.pkz.beruflichCode.Trim()))
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KD4",
                                isoCode, "Abschluss aufgrund Beschäftigungsverhältnis nicht möglich."), prParam.maxvalp), ocheckAntAngDto.STATUS_RED, "KD4");
                        }
                    }
                }
            }

            // O = Objekt Neuwagen Km und Alter -------------------------
            if (FO3 && antrag.angAntObDto != null)
            {
                if (antrag.angAntObDto.sysobart == OBARTID_OCCASION && kalkBorderUb_AlterObjFlag == true && kalkBorderUb_KmObjFlag == true)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_FO3KMAGE", isoCode, "Aufgrund Alter oder KM-Stand des Objektes handelt es sich um ein Neuwagen.", ocheckAntAngDto.STATUS_RED, "FO3");
                }
            }

            #endregion PARAMETERPRÜFUNG

            #region ProduktPrüfung Hardcodiert

            #region Nationalität Prüfung ----------------------------------
            if (antrag.kunde != null)
            {
                /*  //KN1 R11 Defekt 10974
                  if (KN1)
                  {
                      if (kundePlusDto.landNationalitaet != null && !MyNationalitaetPruefung(kundePlusDto.landNationalitaet.iso) && !antrag.kunde.auslausweisCode.Equals(AUSLAUSWEISCODE_C))
                          MyAddErrorMessage(outDto, "CHECKANTRAG_KN1", isoCode, "Kein Angebot aufgrund Nationalitätenregelung möglich.", ocheckAntAngDto.STATUS_RED, "KN1");
                  }else */
                //KN2
                 if (KN2)
                {
                    if (kundePlusDto.landNationalitaet != null && !MyNationalitaetPruefung(kundePlusDto.landNationalitaet.iso) && !antrag.kunde.auslausweisCode.Equals(AUSLAUSWEISCODE_C))
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_KN2", isoCode, "Kein Angebot aufgrund Nationalitätenregelung möglich.", ocheckAntAngDto.STATUS_RED, "KN2");
                    }
                }

                //KN3 FN2
                else if (KN3 || FN2)
                {
                    if ((kundePlusDto.landWohnsitz != null) && (!MyDomizilPruefung(kundePlusDto.landWohnsitz.iso)) &&
                       ((kundePlusDto.landNationalitaet != null) && (!kundePlusDto.landNationalitaet.iso.Equals(NATIONALITAET_CH)) &&
                       !antrag.kunde.auslausweisCode.Equals(AUSLAUSWEISCODE_G)))
                    {
                        if (KN3)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_KN3", isoCode, "Domizil muss CH oder LI sein oder Kunde im Besitze eines G-Ausweises.", ocheckAntAngDto.STATUS_RED, "KN3");
                        }

                        if (FN2)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FN2", isoCode, "Domizil muss CH oder LI sein oder Kunde im Besitze eines G-Ausweises.", ocheckAntAngDto.STATUS_RED, "FN2");
                        }
                    }
                }
                //FN1
                else if (FN1)
                {
                    if (kundePlusDto.landWohnsitz != null && !MyDomizilPruefung(kundePlusDto.landWohnsitz.iso))
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FN1", isoCode, "Kein Angebot aufgrund Wohnsitz möglich.", ocheckAntAngDto.STATUS_RED, "FN1");
                    }
                }

                if (KN4)
                {
                    if ((kundePlusDto.landWohnsitz != null && kundePlusDto.landWohnsitz.iso == WOHNSITZ_CH) &&
                        (kundePlusDto.korrAdresse == null || kundePlusDto.korrAdresse.iso == KORRADRESSE_CH))
                    {
                        // Grün
                    }
                    else
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_KN4", isoCode, "Für die Vertragsart CREDIT-now Card muss die Domizil- und Korrespondenzadresse in der Schweiz liegen.", ocheckAntAngDto.STATUS_RED, "KN4");
                    }
                }
            }
            #endregion Nationalität Prüfung ----------------------------------


            if (antrag.kalkulation != null)
            {

                #region Versicherung -------------------------

                if (antrag.kalkulation.angAntVsDto != null)
                {
                    bool FV6 = FF && berechnen && FF_LEASING;

                    //FV6
                    if (FV6 && antrag.kunde != null)
                    {
                        if (antrag.kalkulation.angAntVsDto.Count(o => o.serviceType != ServiceType.RWA) > 0 && antrag.kunde.syskdtyp == KDTYPID_FIRMA)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FV6", isoCode, "Ratenabsicherung nur für Privatpersonen zugelassen.", ocheckAntAngDto.STATUS_RED, "FV6");
                        }
                    }

                    if (kundePlusDto != null)
                    {
                        
                        double minDuration = quoteDao.getQuote("PPILZMIN"); //minimum duration Ratenabsicherung
                        foreach (AngAntVsDto vs in antrag.kalkulation.angAntVsDto)
                        {
                            vsartcode = angAntDao.getVsArtCode(vs.sysvstyp);

                            bool vsArt_aua = vsartcode.Equals(ServiceType.AUA.ToString());
                            bool vsArt_rip = vsartcode.Equals(ServiceType.RIP.ToString());
                            bool vsArt_ra = angAntDao.istRatenabsicherung(sysprprod, vs.sysvstyp);

                            bool KV1 = KF && berechnen && vsArt_aua && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE || KREDIT_FIX);
                            bool KV2 = KF && berechnen && vsArt_aua && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE || KREDIT_FIX);
                            bool KV3 = KF && berechnen && vsArt_aua && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE || KREDIT_FIX);
                            bool KV4 = KF && berechnen && vsArt_ra && (KREDIT_CLASSIC || KREDIT_DISPO_BEIDE);

                            bool FV1 = FF && berechnen && vsArt_aua && (KREDIT_CLASSIC || FF_TZK_x || FF_LEASING);
                            bool FV2 = FF && berechnen && vsArt_rip && (FF_LEASING);
                            bool FV3 = FF && berechnen && vsArt_aua && (KREDIT_CLASSIC || FF_TZK_x || FF_LEASING);
                            bool FV4 = FF && berechnen && vsArt_aua && (KREDIT_CLASSIC || FF_TZK_x || FF_LEASING);
                            bool FV5 = FF && berechnen && vsArt_rip && FF_LEASING;
                            bool FV7 = FF && berechnen && vsArt_ra && (KREDIT_CLASSIC || FF_TZK_x || FF_LEASING);
                          

                            //RA_domizilPruefung KV1/FV1/FV2 berechnen
                            if (KV1 && kundePlusDto.landWohnsitz != null && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_CH))// && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_LI))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_KV1", isoCode, "Ratenabsicherung aufgrund gewähltem Domizilland nicht möglich (gilt immer nur für den ersten Antragsteller).", ocheckAntAngDto.STATUS_RED, "KV1");
                            }

                            if ((FV1) && kundePlusDto.landWohnsitz != null && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_CH))// && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_LI))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FV1", isoCode, "Ratenabsicherung aufgrund gewähltem Domizilland nicht möglich (gilt immer nur für den ersten Antragsteller).", ocheckAntAngDto.STATUS_RED, "FV1");
                            }
                            if ((FV2) && kundePlusDto.landWohnsitz != null && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_CH))// && !kundePlusDto.landWohnsitz.iso.Equals(WOHNSITZ_LI))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FV2", isoCode, "Ratenabsicherung aufgrund gewähltem Domizilland nicht möglich (gilt immer nur für den ersten Antragsteller).", ocheckAntAngDto.STATUS_RED, "FV2");
                            }

                            //RA_Arbeitsverhältnis KV2 berechnen
                            if (KV2 && kundePlusDto.pkz != null && kundePlusDto.pkz.beruflichCode != null && !kundePlusDto.pkz.beruflichCode.Equals(BERUFLICHCODE_UNBEFRISTET))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_KV2_1", isoCode, "Ratenabsicherung aufgrund Arbeitsverhältnis nicht möglich (Haupteinkommen).", ocheckAntAngDto.STATUS_RED, "KV2");
                            }

                            if (KV2 && kundePlusDto.pkz != null && kundePlusDto.pkz.beruflichCode2 != null && !kundePlusDto.pkz.beruflichCode2.Equals("0"))
                            {
                                if (!kundePlusDto.pkz.beruflichCode2.Equals(BERUFLICHCODE_UNBEFRISTET))
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_KV2_2", isoCode, "Ratenabsicherung aufgrund Arbeitsverhältnis nicht möglich (Nebeneinkommen).", ocheckAntAngDto.STATUS_RED, "KV2");
                                }
                            }

                            //RA_Arbeitsverhältnis FV3 berechnen
                            if (FV3 && kundePlusDto.pkz != null && kundePlusDto.pkz.beruflichCode != null && !kundePlusDto.pkz.beruflichCode.Equals(BERUFLICHCODE_UNBEFRISTET))
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FV3_1", isoCode, "Ratenabsicherung aufgrund Arbeitsverhältnis nicht möglich (Haupteinkommen).", ocheckAntAngDto.STATUS_RED, "FV3");
                            }

                            if (FV3 && kundePlusDto.pkz != null && kundePlusDto.pkz.beruflichCode2 != null && !kundePlusDto.pkz.beruflichCode2.Equals("0"))
                            {
                                if (!kundePlusDto.pkz.beruflichCode2.Equals(BERUFLICHCODE_UNBEFRISTET))
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FV3_2", isoCode, "Ratenabsicherung aufgrund Arbeitsverhältnis nicht möglich (Nebeneinkommen).", ocheckAntAngDto.STATUS_RED, "FV3");
                                }
                            }

                            //FV4-KV3 = KALK_BORDER_ENDALTERKUNDE_AUA
                            //RA_Endalter KV3 berechnen
                            if (KV3 && (antrag.kunde.syskdtyp == KDTYPID_PRIVAT) && antrag.kalkulation.angAntKalkDto != null)
                            {
                                if (MyGetEndAlterKunde(aktuell.AddMonths(antrag.kalkulation.angAntKalkDto.lz), antrag.kunde.gebdatum) > maxAUA)
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_KV3", isoCode, "Ratenabsicherung aufgrund Überschreitung maximal zugelassenem Alter bei Ablauf des Kreditvertrages nicht möglich.", ocheckAntAngDto.STATUS_RED, "KV3");
                                }
                            }
                            //RA_Endalter FV4 berechnen
                            if (FV4 && (antrag.kunde.syskdtyp == KDTYPID_PRIVAT) && antrag.kalkulation.angAntKalkDto != null)
                            {
                                if (MyGetEndAlterKunde(aktuell.AddMonths(antrag.kalkulation.angAntKalkDto.lz), antrag.kunde.gebdatum) > maxAUA)
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FV4", isoCode, "Ratenabsicherung aufgrund Überschreitung maximal zugelassenem Alter bei Ablauf des Kreditvertrages nicht möglich.", ocheckAntAngDto.STATUS_RED, "FV4");
                                }
                            }
                            //KALK_BORDER_ENDALTERKUNDE_RIP
                            else if (FV5 && (antrag.kunde.syskdtyp == KDTYPID_PRIVAT) && antrag.kalkulation.angAntKalkDto != null)
                            {
                                if (MyGetEndAlterKunde(aktuell.AddMonths(antrag.kalkulation.angAntKalkDto.lz), antrag.kunde.gebdatum) > maxRIP)
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FV5", isoCode, "Ratenabsicherung aufgrund Überschreitung maximal zugelassenem Alter bei Ablauf des Kreditvertrages nicht möglich.", ocheckAntAngDto.STATUS_RED, "FV5");
                                }
                            }
                            
                            if (KV4 && vsArt_aua && minLZAUA>0 && antrag.kalkulation.angAntKalkDto.lz < minLZAUA)
                            { //contract duration
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KV4_AUA", isoCode, "Ratenabsicherung (AUA) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZAUA.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "KV4_AUA");

                            }
                            else if (KV4 && vsArt_rip && minLZRIP > 0 && antrag.kalkulation.angAntKalkDto.lz < minLZRIP)
                            { //contract duration
                                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_KV4_RIP", isoCode, "Ratenabsicherung (RIP) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZRIP.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "KV4_RIP");

                            }
                            else if (FV7 && vsArt_aua && minLZAUA > 0 && antrag.kalkulation.angAntKalkDto.lz < minLZAUA)
                            { 
                                
                                //contract duration
                                     MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FV7_AUA", isoCode, "Ratenabsicherung (AUA) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZAUA.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "FV7_AUA");

                            }
                            else if (FV7 && vsArt_rip && minLZRIP > 0 && antrag.kalkulation.angAntKalkDto.lz < minLZRIP)
                            {
                                //contract duration
                                 MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FV7_RIP", isoCode, "Ratenabsicherung (RIP) nur bei Laufzeiten ab {0}  Monaten möglich"), minLZRIP.ToString("0.######")), ocheckAntAngDto.STATUS_RED, "FV7_RIP");

                            }

                        }
                    }

                    //RWA
                    if (RWA)
                    {
                        //Wenn eine obligatorische RWA-Objektversicherung vorliegt, soll die Deckungssumme (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die Deckungssumme (ANTVS:DECKUNGSSUMME) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                        //: „Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.“. 
                        AngAntVsDto rwavs = (from f in antrag.kalkulation.angAntVsDto
                                             where f.serviceType == ServiceType.RWA
                                             select f).FirstOrDefault();
                        if (rwavs != null)
                        {

                            int? optionalRWA = (from f in PrismaDaoFactory.getInstance().getPrismaServiceDao().getVSTYP()
                                         where f.SYSVSTYP == rwavs.sysvstyp
                                         select f.FLAGPAUSCHAL).FirstOrDefault();
                            if (optionalRWA.HasValue && optionalRWA.Value==1 && rwavs.deckungssumme == 0)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR4", isoCode, "Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.")), ocheckAntAngDto.STATUS_RED, "FR4");
                            }
                            //Wenn eine obligatorische RWA-Objektversicherung vorliegt  , soll die RWA-Indikation (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die RWA-Indikation (ANTOB:RWBASE) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                            //: „Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.“. 
                            if (optionalRWA.HasValue && optionalRWA.Value == 1 && antrag.kalkulation.angAntKalkDto.rwBase == 0)
                            {
                                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR5", isoCode, "Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.")), ocheckAntAngDto.STATUS_RED, "FR5");
                            }
                        }
                    }
                }
                #endregion Versicherung -------------------------

                // Ablösen
                // KD5 = (berechnen && (KREDIT_DISPO_BEIDE || KREDIT_CLASSIC))
                if (KD5)
                {
                    if (antrag.kalkulation.angAntKalkDto != null)
                    {
                        // Für KREDIT_CLASSIC wird nichts addiert
                        double abloesen = MyCalcAbloesen(antrag.kalkulation.angAntAblDto);
                        if (KREDIT_DISPO)
                        {
                            // angAntKalkDto.auszahlung = der Erste AuszahlungsBetrag
                            abloesen += antrag.kalkulation.angAntKalkDto.auszahlung;
                        }
                        if (KREDIT_DISPO_PLUS)
                        {
                            // initLadung = Kartenauszahlung
                            // if Kreditbetrag < Ablösen + Auszahlungbetrag + Initladung, then STATUS_RED
                            abloesen += antrag.kalkulation.angAntKalkDto.auszahlung + antrag.kalkulation.angAntKalkDto.initLadung;
                        }
                        // angAntKalkDto.bginternbrutto = Kreditlimite
                        if (antrag.kalkulation.angAntKalkDto.bginternbrutto < abloesen)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_ABLOESEN", isoCode, "Kreditbetrag/Kreditlimite aufgrund Ablöse/Auszahlungsbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "ABLOESEN");
                        }
                    }
                }
            }

            #region D = Diverse -------------------------
            //KD1
            if (KD1 && kundePlusDto != null && kundePlusDto.anzVExpress > 0)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_KD1", isoCode, "Kein Angebot aufgrund laufendem Vertrag möglich.", ocheckAntAngDto.STATUS_RED, "KD1");
            }

            //KD2
            if (KD2 && kundePlusDto != null)
            {
                if (kundePlusDto.anzVDispo > 1)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_KD2", isoCode, "Kein Angebot aufgrund laufendem Vertrag möglich.", ocheckAntAngDto.STATUS_RED, "KD2");
                }
                else
                    if (kundePlusDto.anzVDispo == 1)
                    {
                        int countv = 0;

                        if (antrag.kalkulation != null && antrag.kalkulation.angAntAblDto != null)
                        {
                            long sysVorVt = 0;

                            foreach (var antAbl in antrag.kalkulation.angAntAblDto)
                            {
                                if (antAbl.sysvorvt > 0)
                                {
                                    sysVorVt = antAbl.sysvorvt;
                                }
                                VertragDao vertragDao = new VertragDao();
                                VertragDto vertragDto = vertragDao.getVertragDetails(sysVorVt);
                                if (vertragDto != null && vertragDto.syskd != null)
                                {
                                    // select syskd from vt where sysid = antrag.kalkulation.angAntAblDto[0].sysvorvt ;        // eigenablöse
                                    if (isAbsvorVtEqualVt(kundePlusDto.vDispo, vertragDto.sysid) && (long)vertragDto.syskd == antrag.syskd)
                                    {
                                        countv++;
                                        break;
                                    }
                                }
                            }

                        }
                        if (countv < 1)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_KD2", isoCode, "Kein Angebot aufgrund laufendem Vertrag möglich.", ocheckAntAngDto.STATUS_RED, "KD2");
                        }
                    }
            }

            //KD3
            if (KD3 && antrag.kalkulation.angAntAblDto != null && antrag.kalkulation.angAntAblDto.Count() > 0)
            {
                double abloesebetraege = 0;
                foreach (var antAbl in antrag.kalkulation.angAntAblDto)
                {
                    abloesebetraege += antAbl.betrag;
                }
                if (abloesebetraege > 0)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_KD3", isoCode, "Auf gewählter Vertragsart können keine Ablösen erfasst werden.", ocheckAntAngDto.STATUS_RED, "KD3");
                }
            }

            //FD2 Spezialregelung  Kundeart<>Privatkunde
            if (FD2 && antrag.kalkulation.angAntKalkDto != null && (antrag.kunde.syskdtyp == KDTYPID_FIRMA))
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_FD2", isoCode, "Kein Angebot aufgrund Kundenart zugelassen.", ocheckAntAngDto.STATUS_RED, "FD2");
            }

            if (FD3 && antrag.kalkulation.angAntKalkDto != null && antrag.kalkulation.angAntKalkDto.sysobusetype == OBUSETYPEID_Privat && antrag.kunde != null && antrag.kunde.syskdtyp == KDTYPID_FIRMA)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_FD3", isoCode, "Aufgrund Kundenart (Unternehmenskunde) ist nur gewerbliche Nutzung zugelassen.", ocheckAntAngDto.STATUS_RED, "FD3");
            }

            //FD4 Spezialregelung DemoLeasing nutzungsart <> Privat Nutzung
            if (FD4 && antrag.kalkulation.angAntKalkDto != null && (antrag.kalkulation.angAntKalkDto.sysobusetype == OBUSETYPEID_DemoLeasing) && antrag.kunde != null && antrag.kunde.syskdtyp == KDTYPID_PRIVAT)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_FD4", isoCode, "Bei Demoleasing muss zwingend ein Unternehmenskunde mit gewerblicher Nutzung des Objektes erfasst werden.", ocheckAntAngDto.STATUS_YELLOW, "FD4");
            }

            //FD5 = FF BNRNEUN-472 FD5 immer für  FF prüfen
            //SummeAblöse>0  and TZK oder KREDIT_CLASSIC auf rot
            //Auszahlungbetrag < SummeAblöse auf rot
            if (FD5)
            {
                double abloesebetrag = 0;
                if (antrag.kalkulation != null)
                {
                    abloesebetrag = MyCalcAbloesen(antrag.kalkulation.angAntAblDto);
                }
                if (abloesebetrag > 0)
                {
                    if (!FF_LEASING || (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto.bginternbrutto < abloesebetrag))
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FD5", isoCode, "Produkt ungleich Leasing oder Barkaufpreis ist kleiner als Ablösebetrag", ocheckAntAngDto.STATUS_RED, "FD5");
                    }
                }
            }

            // L = Limit (Budget)-------------------------------------
            if (KL1 && !isclustersaved && kundePlusDto != null && ((kundePlusDto.kredinkkg != null && kundePlusDto.kredinkkg == 0) || (kundePlusDto.budget != null && kundePlusDto.budget < 0)))
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_KL1", isoCode, "Das Budget ist für den gewählten Kreditbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "KL1");
            }

            // bginternbrutto = KreditBetrag
            // KKG = KreditKonsumGesetz
            // 80000 = KreditLimite innerhalb KKG
            // kredINkkg = Maximale Kreditlimite innerhalb KKG
            // PPI = Betrag Ratenabsicherung = calcRsvgesamt
            // CR Nummer: CRMGT00024922 (CRMGT00024922_Korrektur_Budgetberechnung .doc)
            // Ticket#2012062010000115 : Der CR muss noch auf die Kreditart Classic eingeschränkt werden:
            // BNRACHT-593
            if ((KL4 || KL4_CLASSIC) && kundePlusDto != null && antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null && !b2b && kundePlusDto.kredinkkgMax != null && !isclustersaved)
            {


                if (antrag.kalkulation.angAntKalkDto.bginternbrutto + antrag.kalkulation.angAntKalkDto.calcRsvgesamt <= kundePlusDto.kredinkkgMax)
                {
                    // Grün
                }
                else
                {
                    // Rot
                    MyAddErrorMessage(outDto, "CHECKANTRAG_KL4", isoCode, "Hinweismeldung: Das Budget ist für den gewählten Kreditbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "KL4");
                }

            }


            if (KL5 && !isclustersaved && kundePlusDto != null && kundePlusDto.kredinkkg != null && kundePlusDto.kredinkkg < minimalBetrag)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_KL5", isoCode, "Budget für Abschluss CREDIT-now Express nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "KL5");
            }

            if (KL6 && !isclustersaved && kundePlusDto != null && kundePlusDto.kredinkkg != null && kundePlusDto.kredinkkg < minimalBetrag)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_KL6", isoCode, "Budget für Abschluss CREDIT-now Dispo nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "KL6");
            }

            if (antrag.kunde != null && antrag.kunde.syskdtyp == KDTYPID_PRIVAT && kundePlusDto != null && kundePlusDto.budget != null)
            {
                if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null)
                {
                    // Fix für Prüfung auf rateBrutto + Absicherungspraemien durch Versicherungsprämien
                    antrag.kalkulation.angAntKalkDto.rateBruttoInklAbsicherung = antrag.kalkulation.angAntKalkDto.rateBrutto;
                    if (antrag.kalkulation.angAntVsDto != null)
                    {
                        foreach (var antVS in antrag.kalkulation.angAntVsDto)
                        {
                            antrag.kalkulation.angAntKalkDto.rateBruttoInklAbsicherung += antVS.praemie;
                        }
                    }

                    if (antrag.kalkulation.angAntKalkDto.rateBruttoInklAbsicherung >= kundePlusDto.budget)
                    {
                        if (FL1 && antrag.kkgpflicht == true)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FL", isoCode, "Das Budget ist für den gewählten Kreditbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "FL");
                        }

                        if (FL3 && antrag.kkgpflicht == false)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FL", isoCode, "Das Budget ist für den gewählten Kreditbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_YELLOW, "FL");
                        }
                    }

                    if (FL2)
                    {
                        // Ticket#2012042310000088 : CR 23675 - Korrektur Budgetberechnung für Kreditprodukte in der Fahrzeugfinanzierung
                        // Budgetüberschuss = KREMO.Budget1 wenn es nur einen AS gibt und
                        // Budgetüberschuss = KREMO.Saldo wenn es 2 AS gibt
                        // kundePlusDto.budget wird in KundeDao.MyFillKremoFields ermittelt, es gilt auch :
                        // wenn es 2 AS gibt dann KREMO.Saldo > 0 und wenn es nur einen AS gibt dann KREMO.Saldo = 0
                        double? fiktiveRate = kundePlusDto.rateBerechNeu36;
                        if (antrag.kkgpflicht == false)
                        {
                            fiktiveRate = kundePlusDto.rateBerechNeu;
                        }

                        // kundePlusDto.budget ist BudgetÜberschuss
                        if (fiktiveRate > kundePlusDto.budget)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FL", isoCode, "Das Budget ist für den gewählten Kreditbetrag nicht ausreichend.", ocheckAntAngDto.STATUS_RED, "FL");
                        }
                    }
                }
            }
            // Ticket#2012011710000023 Restwert Garant
            if (RWGA && !b2b && antrag.sysrwga > 0)
            {
                MyAddErrorMessage(outDto, "CHECKANTRAG_RWGA", isoCode, "Auswahl aufgrund abweichendem RW-Garant nicht möglich.", ocheckAntAngDto.STATUS_RED, "RWGA");
            }
            #endregion D = Diverse -------------------------


            #region Cluster
            ums = (ums && umsp == 1);
            bwg = (bwg && bwgp == 1);
            stra = (stra && strap == 1);




            if (FEL1 && !b2b && cluster != null && clusterParam != null)
            {
                if (cluster.v_el_prozent > clusterParam.v_el_prozent.maxvaln)
                {
                    if (ums || bwg)
                        //yellow
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechneter Expected Loss in Prozent darf nicht größer als " + clusterParam.v_el_prozent.maxvaln + "% sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                    }

                    else
                    if (fel1r != 1)
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechneter Expected Loss in Prozent darf nicht größer als " + clusterParam.v_el_prozent.maxvaln + "% sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                    }
                    else
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete berechneter Expected Loss in Prozent darf nicht größer als " + clusterParam.v_el_prozent.maxvaln + " sein", ocheckAntAngDto.STATUS_RED, "FEL1");
                    }
                }

                else
                    if (cluster.v_el_betrag > clusterParam.v_el_betrag.maxvaln)
                    {
                        if (ums || bwg)
                            //yellow
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                        }
                        else
                        if (fel1r != 1)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                        }
                        else
                            //red
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_RED, "FEL1");
                        }
                    }

                    else
                        if (clusterParam.v_prof.minvaln > cluster.v_prof) // PROF. neg.
                        {
                            if (fel1r != 1)
                            {
                                if (stra || ums || bwg)
                                {
                                    //grün
                                }
                                else
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                                }
                            }
                            else
                            if (stra || ums || bwg)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein", ocheckAntAngDto.STATUS_YELLOW, "FEL1");
                            }
                            else
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein", ocheckAntAngDto.STATUS_RED, "FEL1");
                            }
                        }
            }



            if (FEL2 && !b2b && cluster != null && clusterParam != null)
            {
                if (cluster.v_el_prozent > clusterParam.v_el_prozent.minvaln && cluster.v_el_prozent <= clusterParam.v_el_prozent.maxvaln && cluster.v_el_betrag <= clusterParam.v_el_betrag.maxvaln)
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_FEL2", isoCode, "Der berechnete Expected Loss liegt " + clusterParam.v_el_prozent.maxvaln + "-" + clusterParam.v_el_prozent.maxvaln, ocheckAntAngDto.STATUS_YELLOW, "FEL2");
                }
                else
                {
                    //Grün
                }
            }
   
            if (KEL1)
            {
                bool hasMA = krBo.hasMA(sysid);
                krBo.performProductValidation(outDto, sysid, kontext, isoCode, hasMA);
            }

            #endregion Cluster

            #endregion ProduktPrüfung Hardcodiert

            return outDto;
        }


      
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public orisikoSimDto risikoSim(irisikoSimDto input)
        {
            return null;
        }


        /// <summary>
        /// Angebot Pruefung
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <param name="sysvart">sysvart</param>
        /// <param name="isoCode">isoCode</param>
        /// <returns></returns>
        public override ocheckAntAngDto checkAngebotById(long sysid, long sysvart, String isoCode)
        {
            prKontextDto kontext = new prKontextDto();
            kontext.sysvart = sysvart;
            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> paramsInput = prismaParameterBo.listAvailableParameter(kontext);
            AngebotDto angebot = getAngebot(sysid);
            ocheckAntAngDto outDto = new ocheckAntAngDto();
            AngAntObSmallDto angAntObDtoSmall = new AngAntObSmallDto();
            if (angebot.angAntObDto != null)
            {
                angAntObDtoSmall.erstzulassung = angebot.angAntObDto.erstzulassung;
                angAntObDtoSmall.ubnahmeKm = angebot.angAntObDto.ubnahmeKm;
            }
            outDto = checkAngebot(angebot.angAntVars[0].kalkulation, kontext, isoCode, angAntObDtoSmall);
            return outDto;
        }
        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
        public override VertragDto getVertrag(long sysid, long sysperole)
        {
            return this.angAntDao.getVertrag(sysid,sysperole);
        }

        #region Produktprüfung My Methods

        /// <summary>
        /// MyCalcAbloesen
        /// </summary>
        /// <param name="angAntAblDto"></param>
        /// <returns></returns>
        double MyCalcAbloesen(List<AngAntAblDto> angAntAblDto)
        {
            double abloesen = 0;
            if (angAntAblDto != null)
            {
                // Ablösen summieren und zum 1. AuszahlungsBetrag addieren
                foreach (var antAbl in angAntAblDto)
                {
                    abloesen += antAbl.betrag;
                }
            }
            return abloesen;
        }

        /// <summary>
        /// MyAddErrorMessage
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="messageCode"></param>
        /// <param name="isoCode"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage(ocheckAntAngDto outDto, String messageCode, String isoCode, String defaultMessage, String newStatus, String code)
        {
            outDto.errortext.Add(translateBo.translateMessage(messageCode, isoCode, defaultMessage));
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }

        /// <summary>
        /// MyAddErrorMessage2
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="message"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage2(ocheckAntAngDto outDto, String message, String newStatus, String code)
        {
            outDto.errortext.Add(message);
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }

        /// <summary>
        /// MyTranslate
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="messageCode"></param>
        /// <param name="defaultMessage"></param>
        /// <returns></returns>
        private string MyTranslate(String messageCode, String isoCode, String defaultMessage)
        {
            return (translateBo.translateMessage(messageCode, isoCode, defaultMessage));
        }

        /// <summary>
        /// domizilPruefung
        /// </summary>
        /// <param name="iso">iso</param>
        /// <returns></returns>
        private bool MyDomizilPruefung(string iso)
        {
            if (iso.Equals(WOHNSITZ_CH) || iso.Equals(WOHNSITZ_LI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// NationalitaetPrüfung
        /// </summary>
        /// <param name="iso">iso</param>
        /// <returns></returns>
        private bool MyNationalitaetPruefung(string iso)
        {
            if (iso.Equals(NATIONALITAET_CH) || iso.Equals(NATIONALITAET_LI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// getEndAlterKunde
        /// </summary>
        /// <param name="endeAm">endeam</param>
        /// <param name="gebDatum">gebDatum</param>
        /// <returns></returns>
        private double MyGetEndAlterKunde(DateTime? endeAm, DateTime? gebDatum)
        {
            if (gebDatum != null && endeAm != null && gebDatum < endeAm)
            {
                return MyGetJahreDiff((DateTime)endeAm, (DateTime)gebDatum);
            }

            return 0;
        }

        /// <summary>
        /// getJahreDiff
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private double MyGetJahreDiff(DateTime date1, DateTime date2)
        {
            return MyGetMonatDiff(date1, date2) / 12;
        }

        /// <summary>
        /// getMonatDiff
        /// </summary>
        /// <param name="ndate1"></param>
        /// <param name="ndate2"></param>
        /// <returns></returns>
        private double MyGetMonatDiff(DateTime? ndate1, DateTime? ndate2)
        {
            if (ndate1 == null && ndate2 == null)
            {
                return 0;
            }

            DateTime date1 = ndate1 == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate1.Value;
            DateTime? dt2temp = DateTimeHelper.ClarionDateToDtoDate(ndate2);
            DateTime date2 = dt2temp == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate2.Value;

            // Beide Daten in einer Liste speichern und sortieren 
            List<DateTime> period = new List<DateTime>() { date1, date2 };
            period.Sort(DateTime.Compare);

            // Ganze Monate zählen
            int monthsFull;
            for (monthsFull = 0; period[0].AddMonths(monthsFull + 1).CompareTo(period[1]) <= 0; monthsFull++)
            {
                ;
            }

            int dayDiff = 0;
            if (date2.Day != date1.Day)
            {
                DateTime startTime = period[0];
                DateTime endTime = period[1];
                dayDiff = endTime.Day - startTime.Day;
                if (dayDiff < 0)
                {
                    dayDiff += 31;
                }
            }

            // nur die Resttage durch 31 teilen
            return monthsFull + dayDiff / 31.0;
        }

        // Die Methode vermeidet, dass der Status von rot auf gelb zurückgesetzt wird.
        private String MySetStatus(String oldstatus, String status)
        {
            if (oldstatus == ocheckAntAngDto.STATUS_RED)
            {
                return oldstatus;
            }
            else
            {
                return status;
            }
        }

        private bool MyIsGreaterThanMaxVal(double valueToCompare, double maxValue)
        {
            // Wenn PRPARAM.MAXVALN gleich 0 ist, dann soll sie nicht berücksichtigt werden.
            if (maxValue > 0)
            {
                // Wenn PRPARAM.MAXVALN > 0 und valueToCompare > maxValue, dann wird ein Fehlertext ausgegeben.
                return (valueToCompare > maxValue);
            }
            // Kein Fehler
            return false;
        }

        private bool MyIsSmallerThanMinVal(double valueToCompare, double minValue)
        {
            // Wenn PRPARAM.MINVALN gleich 0 ist, dann soll sie nicht berücksichtigt werden.
            if (minValue > 0)
            {
                // Wenn PRPARAM.MINVALN > 0 und valueToCompare <= minValue, dann wird ein Fehlertext ausgegeben.
                return (valueToCompare < minValue);
            }
            // Kein Fehler
            return false;
        }

        private double MyCalcPercentToValue(double? baseValueNullable, double percent)
        {
            return (baseValueNullable == null ? 0 : (double)baseValueNullable) * percent / 100;
        }

        private string MyRemoveSpaces(string inputString)
        {
            if (inputString != null)
            {
                return inputString.Trim();
            }
            return String.Empty;
        }


        private double MyCalcBetragToCompare_FB12(AngAntKalkDto kalkDto, bool berechnen)
        {
            double betrag = kalkDto.bginternbrutto;            // Aufruf
            if (berechnen)
            {
                betrag = kalkDto.bginternbrutto - kalkDto.bginternust - kalkDto.szBrutto;
            }
            return betrag;
        }
        private double MyCalcBetragToCompare_FB4567(AngAntKalkDto kalkDto, bool berechnen)
        {
            double betrag = kalkDto.bginternbrutto;            // Aufruf
            if (berechnen)
            {
                if (kalkDto.szBrutto > 0)
                {
                    betrag -= kalkDto.szBrutto;
                }
                else
                {
                    betrag -= kalkDto.rateBrutto;
                }
            }
            return betrag;
        }
        private double MyCalcBetragToCompare_FB89(AngAntKalkDto kalkDto, bool berechnen)
        {
            double betrag = kalkDto.bginternbrutto;            // Aufruf
            if (berechnen)
            {
                betrag = kalkDto.bginternbrutto - kalkDto.szBrutto;
            }
            return betrag;
        }

        private double MyCalcBetragToCompare_FRW3(AngAntKalkDto kalkDto, bool berechnen)
        {
            double betrag = kalkDto.bginternbrutto;            // Aufruf
            if (berechnen)
            {
                betrag = kalkDto.bginternbrutto - kalkDto.bginternust;
            }
            return betrag;
        }

        private bool isAbsvorVtEqualVt(List<long> vertrags, long sysvt)
        {
            foreach (var v in vertrags)
            {
                if (v == sysvt)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// converterB2BNullDate
        /// </summary>
        /// <param name="dtoDate"></param>
        /// <returns></returns>
        public static System.DateTime? converterB2BNullDate(System.DateTime? dtoDate)
        {
            System.Collections.Generic.List<System.DateTime?> nullDates = new System.Collections.Generic.List<System.DateTime?>();
            nullDates.Add(new System.DateTime?(new System.DateTime(001, 1, 1)));

            System.DateTime? returnDate = null;

            //Check if dtoDate is in list of bad dates
            if (nullDates.Contains(dtoDate))
            {
                returnDate = null;
            }
            else
            {
                returnDate = dtoDate;
            }
            return returnDate;
        }


        /// <summary>
        /// getClusterParam
        /// </summary>
        /// <param name="vg"></param>
        /// <param name="scorebezeichnung"></param>
        /// <returns></returns>
        public  VClusterParamDto getClusterParam(VgDto vg, String scorebezeichnung)
        {
            
                if (vg != null && !vg.name.Equals(""))
                {
                    VClusterParamDto clusterParam = new VClusterParamDto();
                    clusterParam.v_cluster = vg.name;
                    Cic.OpenOne.Common.DTO.Prisma.ParamDto betrag = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                    betrag.meta = "V_EL_BETRAG";
                    betrag.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_BETRAG_MAX, 0);
                    betrag.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_BETRAG_MIN, 0);
                    clusterParam.v_el_betrag = betrag;
                    Cic.OpenOne.Common.DTO.Prisma.ParamDto prozent = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                    prozent.meta = "V_EL_PROZENT";
                    prozent.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_PROZENT_MAX, 0);
                    prozent.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_PROZENT_MIN, 0);
                    clusterParam.v_el_prozent = prozent;
                    Cic.OpenOne.Common.DTO.Prisma.ParamDto prof = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                    prof.meta = "V_PROF";
                    prof.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, PROF_MAX, 0);
                    prof.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, PROF_MIN, 0);
                    clusterParam.v_prof = prof;
                    return clusterParam;
                }
                return null;
            }
           

        /// <summary>
        /// MyCalcAuszahlungbetrag
        /// </summary>
        /// <param name="kalkulation"></param>
        /// <param name="sysvm"></param>
        /// <returns></returns>
        private double MyCalcAuszahlungbetrag(KalkulationDto  kalkulation, long? sysvm) 
        {
            double ersteLeasingrate = kalkulation.angAntKalkDto.szBrutto > 0 ? kalkulation.angAntKalkDto.szBrutto : kalkulation.angAntKalkDto.rateBrutto;

            if (kalkulation.angAntKalkDto.verrechnungFlag)
            {
                return kalkulation.angAntKalkDto.bgexternbrutto - ersteLeasingrate - kalkulation.angAntKalkDto.calcRsvmonat - kalkulation.angAntKalkDto.depot - MyCalcZinsubvention(kalkulation.angAntSubvDto, sysvm);
            }
            else
            {
                return kalkulation.angAntKalkDto.bgexternbrutto;
            }
        }

        /// <summary>
        /// MyCalcZinsubventio
        /// </summary>
        /// <param name="angAntSubvDto"></param>
        /// <param name="sysvm"></param>
        /// <returns></returns>
        private double MyCalcZinsubvention(List<AngAntSubvDto> angAntSubvDto, long? sysvm)
        {
            double betrag = 0 ;
            foreach (var v in angAntSubvDto)
            {
                betrag += (v.syssubvg == sysvm) ? v.betrag : 0;
            }

            return betrag;
        }


        #endregion          // Produktprüfung My Methods

        #endregion          // Produktprüfung
    }
}