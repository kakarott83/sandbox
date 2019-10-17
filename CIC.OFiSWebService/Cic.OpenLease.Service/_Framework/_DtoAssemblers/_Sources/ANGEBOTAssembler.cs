// OWNER JJ, 15-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.Service.Provision;
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.Service.Versicherung;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.BO;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Reflection;
    #endregion
    public class ANGOBOPTION
    {
        public long sysid { get; set; }
        public decimal? PDEC1501 { get; set; }
        public decimal? PDEC1502 { get; set; }
    }

    class PRODINFO
    {
        public long sysvarttab { get; set; }
        public long sysvttyp { get; set; }
        public long sysvart { get; set; }
        public String tarifcode { get; set; }
        public String vartbez { get; set; }
    }
    /// <summary>
    /// Used for updating the vk and agb info upon saving an extended offer
    /// </summary>
    class VKInfo
    {
        public long haendler { get; set; }
        public long ausliefvk { get; set; }
        public long beratvk { get; set; }
        public long sysagb { get; set; }
    }
    public class ANGEBOTITDto
    {
        public static String QUERY = "SELECT HSNR,NAME,ORT,PLZ,STRASSE,VORNAME from IT where sysit=";
        public String HSNR { get; set; }
        public String NAME { get; set; }
        public String ORT { get; set; }
        public String PLZ { get; set; }
        public String STRASSE { get; set; }
        public String VORNAME { get; set; }
    }

    [System.CLSCompliant(true)]
    public class ANGEBOTAssembler : IDtoAssemblerAngebot<ANGEBOTDto, ANGEBOT, ANGKALK, ANGKALKFS, ANGOB, ANGOBINI, ANGEBOTITDto, ANGOBAUST[],ANGOBOPTION>
    {
        #region Private variables
        //TODO 
        private const int RUECKNAMESICHRANG = 200;//_RUECKNAMESICHRANG;
        private const int RUECKNAMESICHTYP = 10;//_RUECKNAMESICHTYP;

        public const int ZBIISICHRANG = 20;//_RUECKNAMESICHRANG;
        public const int ZBIISICHTYP = 223;//_RUECKNAMESICHTYP;
        

        private const int KONTOINHABERSICHRANG = 300;//TODO SEPA
        private const int KONTOINHABERSICHTYP = 300;//TODO SEPA

        private const String TXT_MEMO = "Zinsen/Finanzierung";
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private long? _SysPEROLE; //ServiceValidator.SysPEROLE, 
        private long? _SysBrand;//ServiceValidator.SysBRAND,
        private long? _VpSysPERSON;//ServiceValidator.VpSysPERSON.GetValueOrDefault()
        private long? _SysPERSON;
        private long? _SysPUSER;
        private long? _SysWfuser;
        private long? _SysLf;
        #endregion

        #region Constructors
        public ANGEBOTAssembler(long? sysPEROLE, long? sysBrand, long? vpSysPERSON, long? sysPERSON, long? sysWfuser, long? sysPUSER)
        {
            _SysPEROLE = sysPEROLE;
            _SysBrand = sysBrand;
            _VpSysPERSON = vpSysPERSON;
            _SysPUSER = sysPUSER;
            _SysPERSON = sysPERSON;
            _SysWfuser = sysWfuser;
            _SysLf = 0;
            //save the business line of the current sysPEROLE in sysLF
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    PEROLE pr = PeroleHelper.FindRootPEROLEObjByRoleType(context, (long)sysPEROLE, PeroleHelper.CnstBusinessLineRoleTypeNumber);
                    if (pr != null)
                        _SysLf = pr.SYSPERSON;
                    else
                        _Log.Warn("User PUSER: " + sysPUSER + " with PEROLE: " + sysPEROLE + " has no assigned BusinessLineRole!");
                }
            }
            catch (Exception e)
            {
                _Log.Warn("User PUSER: " + sysPUSER + " with PEROLE: " + sysPEROLE + " has no assigned BusinessLineRole!", e);
            }

            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region Private constants


        #endregion

        #region IDtoAssembler<ANGEBOTDto,ANGEBOT> Members (Methods)
        public bool IsValid(ANGEBOTDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            // Otymistic
            bool IsValid = true;

            // DdOl
            using (DdOlExtended  Context = new DdOlExtended())
            {
                // Check SYSID
                if (IsValid && dto.SYSID.HasValue)
                {
                    // Check if exists
                    if (!ANGEBOTHelper.Contains(Context, (long)dto.SYSID))
                    {
                        _Errors.Add("SYSID", "Can not be found.");
                        IsValid = false;
                    }

                    // Check if _SysPERSONInPEROLE is not null
                    if (IsValid && !_SysPEROLE.HasValue)
                    {
                        _Errors.Add("SYSID", "Not exists in sight field. SysPERSONInPEROLE is null.");
                        IsValid = false;
                    }

                    // Check sight field
                    if (IsValid && _SysPUSER != null && !Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, _SysPUSER.Value, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, (long)dto.SYSID))
                    {
                        _Errors.Add("SYSID", "Not exists in sight field.");
                        IsValid = false;
                    }
                }
            }
            return IsValid;
        }

        /// <summary>
        /// Updates agb
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ang"></param>
        /// <param name="sysid"></param>
        public void updateAGB(DdOlExtended context, ANGEBOT ang, long sysid)//long sysvart, OBART obart,long? syslf, long sysit)
        {
           /* try
            {
                String query = "select sysagb from bmw_angebot_agb_v where sysangebot=" + sysid;
                List<long> agbs = context.ExecuteStoreQuery<long>(query, null).ToList();
                if (agbs == null || agbs.Count == 0)
                {
                    _Log.Error("Set AGB of Angebot " + sysid + " not possible - no result!");
                    return;
                }
                long sysagb = agbs.FirstOrDefault();
                _Log.Debug("Set AGB of Angebot " + sysid + " to " + sysagb);
                ang.SYSAGB = sysagb;
            }
            catch (Exception ex)
            {
                _Log.Error("Setting AGB failed", ex);
            }*/
        }

        /// <summary>
        /// Returns the angobaust with the given snr
        /// appends the snr to ANGEBOTDto if not yet available
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="snr"></param>
        /// <returns></returns>
        public ANGOBAUSDto DeliverANGOBAUST(ANGEBOTDto dto, String snr)
        {

            ANGOBAUSDto target = null;
            if (dto.ANGOBAUST == null || dto.ANGOBAUST.Length == 0)
            {
                dto.ANGOBAUST = new ANGOBAUSDto[1];
                dto.ANGOBAUST[0] = new ANGOBAUSDto();
                dto.ANGOBAUST[0].SNR = snr;
            }

            foreach (ANGOBAUSDto a in dto.ANGOBAUST)
            {
                if (a.SNR.Equals(snr))
                {
                    target = a;
                    break;
                }
            }
            if (target == null)
            {
                List<ANGOBAUSDto> l = dto.ANGOBAUST.ToList();
                ANGOBAUSDto newangob = new ANGOBAUSDto();
                newangob.SNR = snr;
                target = newangob;
                l.Add(newangob);
                dto.ANGOBAUST = l.ToArray();
            }
            return target;
        }
		/// <summary>
		/// DELETE the ANGABL row with SYSANGABL sysangabl
		/// </summary>
		/// <param name="sysangabl"></param>
		public void DeleteAngabl (long sysangabl)
		{
			using (DdOlExtended Context = new DdOlExtended ())
			{
				if (sysangabl > 0) // DIESEN Eintrag entfernen
				{
					Context.ExecuteStoreCommand ("DELETE FROM ANGABL WHERE SYSANGABL = " + sysangabl);
					Context.SaveChanges ();
				}
			}
		}


		/// <summary>
		/// Update the ANGABL table
		/// </summary>
		/// <param name="abl"></param>
		public void UpdateAngabl (ANGABL abl)
		{
			using (DdOlExtended Context = new DdOlExtended ())
			{
				// Read
				long sysangabl = 0;
				if (abl.SYSANGEBOT.HasValue && abl.SYSANGEBOT.Value > 0)
					sysangabl = Context.ExecuteStoreQuery<long> ("SELECT SYSANGABL FROM ANGABL WHERE (SYSABLTYP IS NULL OR SYSABLTYP <> 43) AND SYSANGEBOT = " + abl.SYSANGEBOT.Value).FirstOrDefault ();

				//if (sysangabl > 0) //bisherigen Eintrag entfernen HCERZWEI-2059
				//{
				//	Context.ExecuteStoreCommand ("delete from ANGABL WHERE (SYSABLTYP IS NULL OR SYSABLTYP <> 43) AND SYSANGEBOT = " + abl.SYSANGEBOT.Value);
				//	return;
				//}

				System.DateTime kalkperDate = new DateTime (111, 1, 1);
				if (abl.DATKALKPER.HasValue)
					kalkperDate = new DateTime (abl.DATKALKPER.Value.Year, abl.DATKALKPER.Value.Month, abl.DATKALKPER.Value.Day);

				if (sysangabl == 0)
				{
					List<Devart.Data.Oracle.OracleParameter> parameters1 = new List<Devart.Data.Oracle.OracleParameter> ();

					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangebot", Value = abl.SYSANGEBOT });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pBETRAG", Value = abl.BETRAG });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pIBAN", Value = abl.IBAN });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pFREMDVERTRAG", Value = abl.FREMDVERTRAG });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pFLAGINTEXT", Value = abl.FLAGINTEXT });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pBANK", Value = abl.BANK });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pAKTUELLERATE", Value = abl.AKTUELLERATE });

					Context.ExecuteStoreCommand (
						"INSERT INTO ANGABL (SYSANGEBOT, BETRAG, IBAN, FREMDVERTRAG, FLAGINTEXT, BANK, AKTUELLERATE) " +
						"VALUES (:pSysangebot, :pBETRAG, :pIBAN, :pFREMDVERTRAG, :pFLAGINTEXT, :pBANK, :pAKTUELLERATE)", parameters1.ToArray ());
					Context.SaveChanges ();
					sysangabl = Context.ExecuteStoreQuery<long> ("SELECT SYSANGABL FROM ANGABL WHERE (SYSABLTYP IS NULL OR SYSABLTYP <> 43) AND SYSANGEBOT = " + abl.SYSANGEBOT.Value).FirstOrDefault ();
				}

				List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangebot", Value = abl.SYSANGEBOT });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pBETRAG", Value = abl.BETRAG });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pIBAN", Value = abl.IBAN });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pFREMDVERTRAG", Value = abl.FREMDVERTRAG });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pFLAGINTEXT", Value = abl.FLAGINTEXT });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pBANK", Value = abl.BANK });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pAKTUELLERATE", Value = abl.AKTUELLERATE });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangabl", Value = sysangabl });

				Context.ExecuteStoreCommand ("UPDATE ANGABL SET SYSANGEBOT = :pSysangebot, BETRAG = :pBETRAG, IBAN = :pIBAN, FREMDVERTRAG = :pFREMDVERTRAG, FLAGINTEXT = :pFLAGINTEXT, BANK = :pBANK, AKTUELLERATE = :pAKTUELLERATE WHERE (SYSABLTYP IS NULL OR SYSABLTYP <> 43) SYSANGABL = :pSysangabl", parameters2.ToArray ());
				Context.SaveChanges ();
			}
		}

		/// <summary>
		/// Update the ANGABL table for HHR handling (Vorvertrag)
		/// </summary>
		/// <param name="abl"></param>
		public void UpdateAngablHHR (ANGABL abl)
		{
			using (DdOlExtended Context = new DdOlExtended ())
			{
				// Read
				long sysangabl = 0;
				if (abl.SYSANGEBOT.HasValue && abl.SYSANGEBOT.Value > 0)
					sysangabl = Context.ExecuteStoreQuery<long> ("SELECT SYSANGABL FROM ANGABL WHERE SYSABLTYP = 43 AND SYSANGEBOT = " + abl.SYSANGEBOT.Value).FirstOrDefault ();

				//if (sysangabl > 0) //bisherigen Eintrag entfernen HCERZWEI-2059
				//{
				//	Context.ExecuteStoreCommand ("delete from ANGABL WHERE SYSABLTYP = 43 AND SYSANGEBOT = " + abl.SYSANGEBOT.Value);
				//	return;
				//}

				System.DateTime kalkperDate = new DateTime (111, 1, 1);
				if (abl.DATKALKPER.HasValue)
					kalkperDate = new DateTime (abl.DATKALKPER.Value.Year, abl.DATKALKPER.Value.Month, abl.DATKALKPER.Value.Day);

				if (sysangabl == 0)
				{
					List<Devart.Data.Oracle.OracleParameter> parameters1 = new List<Devart.Data.Oracle.OracleParameter> ();
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangebot", Value = abl.SYSANGEBOT });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pRate", Value = abl.AKTUELLERATE });
					parameters1.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pDate", Value = kalkperDate.ToString ("yyyy-MM-dd") });
					
					Context.ExecuteStoreCommand (
						"INSERT INTO ANGABL (SYSANGEBOT, AKTUELLERATE, DATKALKPER, SYSABLTYP) " +
						"VALUES (:pSysangebot, :pRate, TO_DATE(:pDate, 'yyyy-MM-dd'), 43)", parameters1.ToArray ());
					Context.SaveChanges ();
					sysangabl = Context.ExecuteStoreQuery<long> ("SELECT SYSANGABL FROM ANGABL WHERE SYSABLTYP = 43 AND SYSANGEBOT = " + abl.SYSANGEBOT.Value).FirstOrDefault ();
				}

				List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangebot", Value = abl.SYSANGEBOT });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pRate", Value = abl.AKTUELLERATE });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pDate", Value = kalkperDate.ToString ("yyyy-MM-dd") });
				parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysangabl", Value = sysangabl });
				Context.ExecuteStoreCommand ("UPDATE ANGABL SET SYSANGEBOT = :pSysangebot, AKTUELLERATE = :pRate, DATKALKPER = TO_DATE(:pDate, 'yyyy-MM-dd'), SYSABLTYP = 43 WHERE SYSANGABL = :pSysangabl", parameters2.ToArray ());
				Context.SaveChanges ();
			}
		}


		public void Create(ANGEBOTDto dto, out ANGEBOT outANGEBOT, out ANGKALK outANGKALK, out ANGKALKFS outANGKALKFS, out ANGOB outANGOB, out ANGOBINI outANGOBINI, out ANGOBAUST[] outANGOBAUST, out ANGOBOPTION outOPTION)
        {
            double logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;

            ANGEBOT NewANGEBOT;
            ANGKALK NewANGKALK;
            ANGOB NewANGOB;
            ANGOBINI NewANGOBINI;
            ANGKALKFS NewANGKALKFS;
            ANGOBAUST[] NewANGOBAUST;
            WFMMEMO NewWFMMEMOvk, NewWFMMEMOi;
            OBART NewObArt;
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }
            ANGOBOPTION newOPTION = new ANGOBOPTION();
            NewANGEBOT = new ANGEBOT();
            NewANGKALK = new ANGKALK();
            NewANGKALKFS = new ANGKALKFS();
            NewANGOB = new ANGOB();
            NewANGOBINI = new ANGOBINI();
            NewANGOBAUST = null;
            
            NewWFMMEMOvk = null;
            NewWFMMEMOi = null;

            NewObArt = null;// new OBART();
            ANGOBAUSDto sonzubFreitext = DeliverANGOBAUST(dto, "Freitext");
            sonzubFreitext.BETRAG = dto.ANGOBSONZUBUSER;
            sonzubFreitext.BETRAG2 = dto.ANGOBSONZUBDEFAULT;
            sonzubFreitext.FREITEXT = dto.ANGOBSONZUBTEXT;
            ANGOBAUSDto angobausPreiskarte = DeliverANGOBAUST(dto, "Preiskarte");
            if (dto.WFMMEMOTEXT != null) angobausPreiskarte.FREITEXT = dto.WFMMEMOTEXT;
            angobausPreiskarte.BETRAG = dto.WFMMEMOSTATTPREIS;
            angobausPreiskarte.BETRAG2 = dto.WFMMEMOANGEBOTSPREIS;

            if ((dto.ANGOBAUST != null) && (dto.ANGOBAUST.Length > 0))
            {
                NewANGOBAUST = new ANGOBAUST[dto.ANGOBAUST.Length];
            }
            
            //Cic.OpenLease.Model.DdOiqueue.CfgSingleton CfgSingleton = Cic.OpenLease.Model.DdOiqueue.CfgSingleton.Instance;

            _Log.Warn("Tracing Save Angebot #0: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
            logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;

            //Get Angebot unique identifier
            // String ConfigurationValue = CfgSingleton.GetEntry("NKK", "ANGEBOT_BER", "AIDA", "AIDA");
            NkBuilder Nk = new NkBuilder("ANGEBOT", "B2B");
            // Is Copytoresubmit ?
            if (ZustandHelper.GetStringFromEnumerator(AngebotZustand.NeuResubmit) != dto.ZUSTAND)
                dto.ANGEBOT1 = Nk.getNextNumber();
            else
                dto.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert);

            dto.ANGOBOBJEKT = dto.ANGEBOT1;
            /*
            //Get Angob unique identifier
            ConfigurationValue = CfgSingleton.GetEntry("NKK", "ANGOB_BER", "ANGOB", "AIDA");
            dto.ANGOBOBJEKT = Nk.Build("ANGEBOT", ConfigurationValue);*/

            //NewANGOB.OBKAT = null; //new OBKAT();

            _Log.Warn("Tracing Save Angebot #1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
            logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
            
            using (DdOlExtended Context = new DdOlExtended())
            {
                PRODINFO pinfo = null;
                if(dto.SYSPRPRODUCT.HasValue)
                { 
                        pinfo = Context.ExecuteStoreQuery<PRODINFO>("select tarifcode, sysvarttab,prproduct.sysvart,sysvttyp,vart.bezeichnung vartbez from prproduct,vart where prproduct.sysvart = vart.sysvart and sysprproduct=" + dto.SYSPRPRODUCT, null).FirstOrDefault();
                        dto.SYSVART = pinfo.sysvart;
                        dto.SYSVTTYP = pinfo.sysvttyp;
                }
                String mandant = Context.ExecuteStoreQuery<String>("select name from brand where sysbrand=" + _SysBrand, null).FirstOrDefault();
                //von der brand den mandanten-namen + Kundenart (privat/unternehmen)
                dto.VERTRIEBSWEG = mandant;
                /* if (dto.SYSIT.HasValue && dto.SYSIT > 0)
                {
                    try
                    {
                        String kgroup = Context.ExecuteStoreQuery<String>("select kundengruppe from it where sysit=" + dto.SYSIT, null).FirstOrDefault();
                        if (kgroup != null)
                            dto.VERTRIEBSWEG = mandant + " " + kgroup;
                    }
                    catch (Exception ite)
                    {
                        _Log.Error("Error fetching Kundengruppe", ite);
                    }
                }*/

                if(dto.SYSOBTYP!=null)
                    dto.KFZBRIEF = Context.ExecuteStoreQuery<String>("select bezeichnung from obtyp where importsource = 2  and importtable like 'ETGMODLEVONE'  connect by prior sysobtypp=sysobtyp start with sysobtyp=" + dto.SYSOBTYP, null).FirstOrDefault();
                if (dto.KFZBRIEF != null && dto.KFZBRIEF.Length > 30)
                    dto.KFZBRIEF = dto.KFZBRIEF.Substring(0, 30);

                NewObArt = (from c in Context.OBART
                            where c.SYSOBART == dto.SYSOBART
                            select c).FirstOrDefault();//Context.SelectById<OBART>(dto.SYSOBART.GetValueOrDefault());

                _Log.Warn("Tracing Save Angebot #2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                using (DdOwExtended ContextDdOw = new DdOwExtended())
                {
                       
                    // Insert (with Save changes)           
                    try
                    {
                        decimal Ust = LsAddHelper.GetTaxRate(Context, dto.SYSVART);

                        if (dto.SYSPRPRODUCT.HasValue)
                        {
                            KALKTYP KalkTyp = PRPRODUCTHelper.DeliverKalkTyp(Context, dto.SYSPRPRODUCT.GetValueOrDefault());
                            if (KalkTyp != null)
                            {
                                NewANGKALK.SYSKALKTYP = KalkTyp.SYSKALKTYP;
                                dto.ANGKALKSYSKALKTYP = KalkTyp.SYSKALKTYP;
                            }
                        }

                        if (NewObArt != null)
                            dto.ANGOBFZART = NewObArt.NAME;

                        try
                        {
                            dto.HIST_ANGKALKFUELSYSFSTYP = FsPreisHelper.GetFsTypByKey((long)dto.ANGKALKFUELSYSFSTYP, Context).BEZEICHNUNG;
                        }
                        catch (Exception) { }
                        dto.HIST_ANGOBINIMOTORTYP1 = dto.ANGOBINIMOTORTYP;
                        // Map
                        //NOVANEU
                        bool isNew = OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 0) || OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 2);
                        _Log.Warn("Tracing Save Angebot #3: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        NewANGEBOT.ERFASSUNG = DateTime.Now;
                        NewANGEBOT.DATANGEBOT = DateTime.Now;
                        NewANGEBOT.GUELTIGBIS = (DateTime.Today).Date.AddDays(30);
                        //force mapping to angebot in mymap to entities, because its new
                        dto.DATANGEBOT = NewANGEBOT.DATANGEBOT;
                        dto.ERFASSUNG = NewANGEBOT.ERFASSUNG;
                        dto.GUELTIGBIS = NewANGEBOT.GUELTIGBIS;

                        MyMapToEntities(dto, NewANGKALK, NewANGKALKFS, NewANGOB, NewANGOBINI, NewANGEBOT, NewANGOBAUST, Ust, isNew, Context, newOPTION);

                        NewANGEBOT.SYSMWST = LsAddHelper.getSYSMWSTByVART(Context, dto.SYSVART);

                        //Calculate - all fields are already delivered
                        // MyCalculate(dto, NewANGKALK, NewANGOB, Context);
                        _Log.Warn("Tracing Save Angebot #4: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        //Get Required Fields
                        MyGetFields(NewANGEBOT, NewANGKALK, NewANGOB, dto.SYSPRPRODUCT.GetValueOrDefault(), Context);
                        int? cond = null;
                        if(dto.SYSPRPRODUCT.HasValue)
                            cond = PRPRODUCTHelper.DeliverConditionType(Context, (long)dto.SYSPRPRODUCT);
                        if (pinfo!=null && cond != null && cond == PRPRODUCTHelper.ConditionTypeAktions)
                        {
                            NewANGEBOT.KONSTELLATION = pinfo.tarifcode;
                        }
                        if(pinfo!=null)
                            NewANGEBOT.SYSVARTTAB = pinfo.sysvarttab;
                        if (cond != null && cond == PRPRODUCTHelper.ConditionTypeStandard)
                        {
                            NewANGEBOT.KONSTELLATION = "standard";
                        }

                        /* if ((NewObArt.NAME).Equals("Barkredit"))
                        {
                            NewANGEBOT.OBJEKTVT = "Barkredit";
                            NewANGOB.BGN = 6;
                            NewANGOB.ABNAHMEKM = (15000 / 12 * NewANGKALK.LZ);
                            NewANGOB.SYSKGRUPPE = 301;
                            NewANGOB.USGAAP = 1;
                        }*/
                        //HCE IMMER Konstant
                        NewANGEBOT.SYSPRCHANNEL = 1;
                        NewANGEBOT.SYSPRHGROUP = 0;
                        dto.SYSPRHGROUP = 0;
                        dto.SYSPRCHANNEL = 1;

                        if(!"Neu".Equals(dto.ZUSTAND))
                        { 
                            NewANGEBOT.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert);
                            dto.ZUSTAND = NewANGEBOT.ZUSTAND;
                        }
                        NewANGEBOT.AKTIVKZ = 1;
                        NewANGEBOT.SYSVK = _VpSysPERSON;
                        NewANGEBOT.SYSVM = _VpSysPERSON;
                        NewANGEBOT.SYSLF = _SysLf;
                        NewANGEBOT.SYSLS = LsAddHelper.getMandantByPEROLE(Context, _SysPEROLE.Value);
                        NewANGEBOT.SYSWAEHRUNG = LsAddHelper.getHauswaehrungByPEROLE(Context, _SysPEROLE.Value);

                        if (_SysBrand.HasValue)
                            NewANGEBOT.SYSBRAND= _SysBrand.Value;

                        //NewANGEBOT.SYSBRAND = _SysBrand;//Assign only on create

                        // Set the status change time
                        NewANGEBOT.ZUSTANDAM = DateTime.Now;

                        // Write an empty cancellation reason
                        NewANGEBOT.ABTRETUNGVON = string.Empty;
                            
                        //In eine Sonderkalkulation Copy sollt die Bearbeitung nicht mehr gesperrt sein.
                        if (NewANGEBOT.SPECIALCALCSTATUS == null || NewANGEBOT.SPECIALCALCSTATUS == 1) NewANGEBOT.SPECIALCALCSTATUS = 0;
                        _Log.Warn("Tracing Save Angebot #5: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        Context.ANGEBOT.Add(NewANGEBOT);
                        //Context.ANGEBOTExtension.Insert(NewANGEBOT);
                        //Context.OBART.Add(NewObArt);
                        //Context.OBARTExtension.Insert(NewObArt);
                        NewANGOB.OBART = NewObArt;
                        if (dto.SYSOBTYP.HasValue)
                            NewANGOB.SYSOBTYP =  (long)dto.SYSOBTYP;
                        NewANGOB.OBKAT = (from c in Context.OBKAT
                                          where c.SYSOBKAT == dto.ANGOBSYSOBKAT
                                          select c).FirstOrDefault();//Context.SelectById<OBKAT>(dto.ANGOBSYSOBKAT);

                        if (!isNew)
                        {
                            NewANGOB.NOVA = 0;
                            NewANGOB.NOVABETRAG = 0;
                            NewANGOB.NOVABRUTTO = 0;
                            NewANGOB.NOVAP = 0;
                            NewANGOB.NOVAUST = 0;
                            NewANGOB.NOVAZUAB = 0;
                            NewANGOB.NOVAZUABBRUTTO = 0;
                            NewANGOB.NOVAZUABUST = 0;
                        }

                        Context.ANGOB.Add(NewANGOB);

                        // Get NewANGEBOT.SYSID
                        Context.SaveChanges();

                        NewANGKALK.SYSANGEBOT = NewANGEBOT.SYSID;
                        NewANGKALK.ANGOB = NewANGOB;
                        NewANGKALK.ANGKALKFS = NewANGKALKFS;
                        NewANGKALK.SYSWAEHRUNG = LsAddHelper.getHauswaehrungByPEROLE(Context, _SysPEROLE.Value);
                        NewANGOB.SYSANGEBOT=NewANGEBOT.SYSID;
                        Context.ANGKALK.Add(NewANGKALK);

                        if (dto.ANGKALKVERRECHNUNG.HasValue && dto.ANGKALKVERRECHNUNG.Value > 0)
                        {
                            ANGABL abl = new ANGABL();
                            // Context.ANGABL.Add(abl);
                            abl.SYSANGEBOT = NewANGEBOT.SYSID;
                            abl.BETRAG = dto.ANGKALKVERRECHNUNG.Value;
                            abl.IBAN = dto.ANGABLIBAN;
                            abl.FREMDVERTRAG = dto.ANGABLFREMDVERTRAG;
                            abl.FLAGINTEXT = dto.ANGABLFLAGINTEXT;
                            abl.BANK = dto.ANGABLBANK;
                            abl.AKTUELLERATE = dto.ANGABLAKTUELLERATE;

							UpdateAngabl (abl);
                        }

						//_Log.Warn ("Tracing Save Angebot #6: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
						//logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
						//Context.SaveChanges ();

						// DATKALKPER / AKTUELLERATE (rh 20180414)
						if (dto.VVTAKTUELLERATE.HasValue && dto.VVTAKTUELLERATE.Value > 0)
						{
							ANGABL abl = new ANGABL ();
							// Context.ANGABL .Add(abl);
							//if (dto.ANGKALKVERRECHNUNG.HasValue && dto.ANGKALKVERRECHNUNG.Value > 0) abl.BETRAG = dto.ANGKALKVERRECHNUNG.Value;
							//if ( ! string.IsNullOrEmpty(dto.ANGABLIBAN)) abl.IBAN = dto.ANGABLIBAN;
							//if ( ! string.IsNullOrEmpty (dto.ANGABLFREMDVERTRAG)) abl.FREMDVERTRAG = dto.ANGABLFREMDVERTRAG;
							//abl.FLAGINTEXT = dto.ANGABLFLAGINTEXT;
							//if ( ! string.IsNullOrEmpty (dto.ANGABLBANK)) abl.BANK = dto.ANGABLBANK;

							// ADD ABLÖSE TYPE und values
							// GEHT HIER NICHT wegen EDMX PRB: abl.SYSABLTYP = 43;     // ToDo: SELECT from Tabel ABLTYP:
							abl.DATKALKPER = dto.VVTDATKALKPER;
							abl.AKTUELLERATE = dto.VVTAKTUELLERATE;
							abl.SYSANGEBOT = NewANGEBOT.SYSID;


							//_Log.Warn ("Tracing Save Angebot #6: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
							//logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
							//Context.SaveChanges ();

							//// NOT-UPDATE SYSABLTYP = 43, weil SYSABLTYP im EDMX aufgrund von Kollisionen mit ABLTYP::SYSABLTYP nicht gemappt werden kann.
							//// TODO remove when edmx ready, GET lastIndex  (// ACHTUNG! findet vllt einen früheren DS!!!)
							//Context.ExecuteStoreCommand ("UPDATE ANGABL SET SYSABLTYP = 43 WHERE SYSANGEBOT=" + NewANGEBOT.SYSID, null);

							UpdateAngablHHR (abl);
						}


						if (NewANGEBOT.EINZUG.HasValue)
                        {
                            LogHelper.logToDatabase("OFISWebService SaveAngebot()", "einzug=" + NewANGEBOT.EINZUG.Value + "/ SYSKI=" + NewANGEBOT.SYSKI, "ANGEBOT", (long)NewANGEBOT.SYSID);
                        }


                        _Log.Warn("Tracing Save Angebot #7: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;

                            
                        NewANGOBINI.SYSOBINI = NewANGOB.SYSOB;

                        Context.ANGOBINI.Add(NewANGOBINI);

                        if (NewANGOBAUST != null && NewANGOBAUST.Length > 0)
                        {
                            foreach (ANGOBAUST AngobaustLoop in NewANGOBAUST)
                            {
                                AngobaustLoop.SYSANGOB = NewANGOB.SYSOB;
                                Context.ANGOBAUST.Add(AngobaustLoop);
                                Context.SaveChanges();
                                //Context.Insert < ANGOBAUST>(AngobaustLoop);
                            }
                        }
                        dto.SYSID = NewANGEBOT.SYSID;

                        String mref = createOrUpdateMandat(dto, NewANGEBOT.SYSLS.Value, Context, _VpSysPERSON);

                            

                        //Update AngebotDto
                            
                        dto.ANGKALKSYSKALK = NewANGKALK.SYSKALK;
                        dto.ANGOBSYSOB = NewANGOB.SYSOB;
                        dto.SYSBERATADDB = NewANGEBOT.SYSBERATADDB;
                        dto.SYSBRAND = NewANGEBOT.SYSBRAND;
                        //dto.SYSBRAND = NewANGEBOT.SYSBRAND;
                        MyUpdateProvisions(dto, NewANGKALK, Context, _VpSysPERSON, false, Ust);
                        MyUpdateSicherheiten(dto, NewANGEBOT, Context);
                        MyUpdateInsurance(dto, NewANGKALKFS, Context);
                            
                        Subvention.SetSubventions(dto, NewANGEBOT, Context, NewANGEBOT.SYSBERATADDB, (long)_SysPERSON, (long)_SysPEROLE);
                        if (NewANGEBOT.SYSVORVT.HasValue && NewANGEBOT.CONTRACTEXT.HasValue && NewANGEBOT.CONTRACTEXT.Value > 0)//Verlängerung, Ticket #6316
                        {
                            VKInfo vki = Context.ExecuteStoreQuery<VKInfo>("select sysagb, VT.SysVPFIL haendler, vt.SYSBERATADDA ausliefvk, vt.SYSBERATADDB beratvk from vt where sysid=" + NewANGEBOT.SYSVORVT.Value, null).FirstOrDefault();
                            if (vki != null)
                            {
                                //check if this persons are still available!
                                String code = Context.ExecuteStoreQuery<String>("select code from person where sysperson=" + vki.beratvk, null).FirstOrDefault();
                                String codehd = Context.ExecuteStoreQuery<String>("select code from person where sysperson=" + vki.haendler, null).FirstOrDefault();
                                if (codehd != null && codehd.Length > 0)
                                {
                                    NewANGEBOT.SYSVP = vki.haendler;//_VpSysPERSON;
                                    NewANGEBOT.SYSVK = vki.haendler;//_VpSysPERSON;
                                    NewANGEBOT.SYSVPFIL = vki.haendler;// _VpSysPERSON;
                                }
                                if (code != null && code.Length > 0)
                                {
                                    NewANGEBOT.SYSBERATADDB = vki.beratvk;// _SysPERSON;
                                }
                                if(NewANGKALK.LZ<6)
                                    NewANGEBOT.SYSAGB = vki.sysagb;
                                else 
                                    updateAGB(Context, NewANGEBOT, NewANGEBOT.SYSID);
                            }
                        }
                        else
                        {
                            updateAGB(Context, NewANGEBOT, NewANGEBOT.SYSID);
                        }

                        Context.SaveChanges();
                        //TODO remove when edmx ready
                        if (mref != null)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = NewANGEBOT.SYSID });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = mref });
                            Context.ExecuteStoreCommand("update angebot set mandatreferenz=:ref where sysid=:sysid", parameters.ToArray());
                        }
                        else if (NewANGEBOT.EINZUG.HasValue && NewANGEBOT.EINZUG < 1)
                        {
                            Context.ExecuteStoreCommand("update angebot set mandatreferenz=null where sysid=" + NewANGEBOT.SYSID,null);
                        }

                        //update ANGVS
                        List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = NewANGEBOT.SYSID });
                        Context.ExecuteStoreCommand(@"update angvs set sysvs=(select sysvs from vstyp where sysvstyp=angvs.sysvstyp) ,mitfinflag=(case when exists(select 1 from prrsvset where sysvstyp=angvs.sysvstyp)
                                                            then
                                                                (select prclrsvset.mitfinflag from angebot,PRCLRSVSET,prrsvset,prrsvcode,prrsvpos where prrsvpos.sysprrsvset=prrsvset.sysprrsvset and prrsvset.sysprrsvset=prclrsvset.sysprrsvset and prrsvcode.sysprrsvset=prrsvset.sysprrsvset 
                                                                and (prrsvset.validfrom is null or prrsvset.validfrom<=sysdate or prrsvset.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and (prrsvset.validuntil is null or prrsvset.validuntil>=sysdate or prrsvset.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and (prrsvcode.validfrom is null or prrsvcode.validfrom<=sysdate or prrsvcode.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and prrsvset.activeflag=1 
                                                                and prclrsvset.sysprproduct=angebot.sysprproduct 
                                                                and angebot.sysid=angvs.sysangebot
                                                                and prrsvpos.sysvstyp=angvs.sysvstyp)
                                                        else
                                                                (select prclobvset.mitfinflag from angebot,prclobvset,probvset,probvcode,probvpos where probvpos.sysprobvset=probvset.sysprobvset and probvset.sysprobvset=prclobvset.sysprobvset and probvcode.sysprobvset=probvset.sysprobvset 
                                                                and (probvset.validfrom is null or probvset.validfrom<=sysdate or probvset.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and (probvset.validuntil is null or probvset.validuntil>=sysdate or probvset.validuntil<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and (probvcode.validfrom is null or probvcode.validfrom<=sysdate or probvcode.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                and probvset.activeflag=1 
                                                                and prclobvset.sysprproduct=angebot.sysprproduct 
                                                                and angebot.sysid=angvs.sysangebot
                                                                and probvpos.sysvstyp=angvs.sysvstyp)
                                                        end
                                                            )
                                                            where sysangebot=:sysid", parameters2.ToArray());
                        _Log.Warn("Tracing Save Angebot #8: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        // DTO.MEMO - MEMO
                        if (dto.WFMMEMOSCALCIDTEXT != null && dto.WFMMEMOSCALCIDTEXT != "")
                        {
                            NewWFMMEMOi = MyUpdateWFMMEMOFields(ContextDdOw, NewANGEBOT.SYSID, NewWFMMEMOi, WFMKATHelper.CnstWfmmkatNameSonderkalkulationInnendiest);
                            NewWFMMEMOi.NOTIZMEMO = (dto.WFMMEMOSCALCIDTEXT);
                            NewWFMMEMOi.STR10 = TXT_MEMO;
                        }
                        if (dto.WFMMEMOSCALCVKTEXT != null && dto.WFMMEMOSCALCVKTEXT != "")
                        {
                            NewWFMMEMOvk = MyUpdateWFMMEMOFields(ContextDdOw, NewANGEBOT.SYSID, NewWFMMEMOvk, WFMKATHelper.CnstWfmmkatNameSonderkalkulationVerkaeufer);
                            NewWFMMEMOvk.NOTIZMEMO =  (dto.WFMMEMOSCALCVKTEXT);
                            NewWFMMEMOvk.STR10 = TXT_MEMO;
                        }

                        ContextDdOw.SaveChanges();
                        _Log.Warn("Tracing Save Angebot #9: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        _Log.Debug(_Log.dumpObject(dto));
                        _Log.Debug(_Log.dumpObject(NewANGKALK));
                        _Log.Debug(_Log.dumpObject(NewANGOB));
                        _Log.Debug(_Log.dumpObject(NewANGEBOT));
                        _Log.Debug(_Log.dumpObject(NewANGOBINI));
                        _Log.Warn("Tracing Save Angebot #10: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - logStart));
                        logStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    }
                    catch (Exception r)
                    {
                        _Log.Error("Error Creating ANGEBOT " + r.Message, r);
                        _Log.Error(_Log.dumpObject(dto));
                        _Log.Error(_Log.dumpObject(NewANGKALK));
                        _Log.Error(_Log.dumpObject(NewANGOB));
                        _Log.Error(_Log.dumpObject(NewANGEBOT));
                        _Log.Error(_Log.dumpObject(NewANGOBINI));
                        throw r;
                    }

                    // Create peunis
                    if (_SysPEROLE.HasValue)
                    {
                        PEUNIHelper.ConnectNodes(Context, PEUNIHelper.Areas.ANGEBOT, NewANGEBOT.SYSID, _SysPEROLE.Value);
                    }

                    // Save changes
                    Context.SaveChanges();
                    ContextDdOw.SaveChanges();
                }
            }
            outANGEBOT = NewANGEBOT;
            outANGKALK = NewANGKALK;
            outANGOBINI = NewANGOBINI;
            outANGKALKFS = NewANGKALKFS;
            outANGOB = NewANGOB;
            outANGOBAUST = NewANGOBAUST;
            outOPTION = newOPTION;
        }

        public void Update(ANGEBOTDto dto, out ANGEBOT outANGEBOT, out ANGKALK outANGKALK, out ANGKALKFS outANGKALKFS,
            out ANGOB outANGOB, out ANGOBINI outANGOBINI, out ANGOBAUST[] outANGOBAUST, out ANGOBOPTION outOption)
        {
            Update(dto, out outANGEBOT, out outANGKALK, out outANGKALKFS, out outANGOB, out outANGOBINI, out outANGOBAUST, null, null,out outOption);
        }

        public void Update(ANGEBOTDto dto, out ANGEBOT outANGEBOT, out ANGKALK outANGKALK, out ANGKALKFS outANGKALKFS,
            out ANGOB outANGOB, out ANGOBINI outANGOBINI, out ANGOBAUST[] outANGOBAUST, int? finKz, long? sysAngebot, out ANGOBOPTION outOPTION)
        {
            ANGEBOT OriginalANGEBOT = null;
            ANGEBOT ModifiedANGEBOT = null;
            ANGKALK OriginalANGKALK = null;
            ANGKALK ModifiedANGKALK;
            ANGKALKFS OriginalANGKALKFS = null;
            ANGOBOPTION OriginalOPTION = null;
            
            ANGKALKFS ModifiedANGKALKFS;
            ANGOB OriginalANGOB = null;
            ANGOB ModifiedANGOB;
            ANGOBINI OriginalANGOBINI = null;
            ANGOBINI ModifiedANGOBINI;

            WFMMEMO OriginalWFMEMOvk, OriginalWFMEMOi;

            ANGOBAUSTAssembler ANGOBAUSTAssembler = new ANGOBAUSTAssembler(_SysPEROLE);
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            // DbTransaction OlTransaction = null;
            //DbTransaction OwTransaction = null;
            string obartname = null;
            string tarifcode = null;
            string vartbez = null;
            decimal Ust = 0;
            bool isNew = false;
            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    OBART ObArt = (from c in Context.OBART
                                   where c.SYSOBART == dto.SYSOBART
                                   select c).FirstOrDefault();//Context.SelectById<OBART>(dto.SYSOBART.GetValueOrDefault());
                    if (ObArt == null)
                    {
                        throw new Exception("ObArt could not be found.");
                    }
                    obartname = ObArt.NAME;
                    dto.ANGOBFZART = obartname;
                   

                    PRODINFO pinfo = Context.ExecuteStoreQuery<PRODINFO>("select tarifcode, sysvarttab,prproduct.sysvart,sysvttyp,vart.bezeichnung vartbez from prproduct,vart where prproduct.sysvart = vart.sysvart and sysprproduct=" + dto.SYSPRPRODUCT, null).FirstOrDefault();
                    dto.SYSVARTTAB = pinfo.sysvarttab;
                    dto.SYSVTTYP = pinfo.sysvttyp;
                    dto.SYSVART = pinfo.sysvart;
                    
                    tarifcode = pinfo.tarifcode;
                    vartbez = pinfo.vartbez;

                    Ust = LsAddHelper.GetTaxRate(Context, dto.SYSVART);
                    BRAND br = BRANDHelper.DeliverBRAND(Context, (long)_SysBrand);
                    //von der brand den mandanten-namen + Kundenart (privat/unternehmen)
                    dto.VERTRIEBSWEG = br.MANDANT + " Einzelkunde";
                    dto.HIST_ANGOBINIMOTORTYP1 = dto.ANGOBINIMOTORTYP;
                    try
                    {
                        dto.HIST_ANGKALKFUELSYSFSTYP = FsPreisHelper.GetFsTypByKey((long)dto.ANGKALKFUELSYSFSTYP, Context).BEZEICHNUNG;
                    }
                    catch (Exception) { }

                    isNew = OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 0) || OBARTHelper.isOfType(Context, dto.SYSOBART.GetValueOrDefault(), 2);

                    if (!isNew)
                    {
                        dto.ANGOBNOVAZUAB = 0;
                        dto.ANGOBNOVABRUTTO = 0;
                        dto.ANGOBNOVABETRAG = 0;
                        dto.ANGOBNOVA = 0;
                        dto.ANGOBNOVABRUTTO = 0;
                        dto.ANGOBNOVAP = 0;
                        dto.ANGOBNOVAUST = 0;
                        dto.ANGOBNOVAZUABBRUTTO = 0;
                        dto.ANGOBNOVAZUABUST = 0;
                    }
                }

                using (DdOlExtended Context = new DdOlExtended())
                {
                    System.Collections.Generic.List<ANGOBAUST> ModifiedANGOBAUSTS = new System.Collections.Generic.List<ANGOBAUST>();
                    ANGOBAUST[] OriginalANGOBAUSTS = ANGOBHelper.GetAngobaustFromAngob(Context, (long)dto.ANGOBSYSOB);

                    System.Collections.Generic.List<ANGOBAUST> ANGOBAUSTFromDto = new System.Collections.Generic.List<ANGOBAUST>();
                    System.Collections.Generic.List<string> SNRList = new System.Collections.Generic.List<string>();//contains all angobaust pkeys currently in dto

                    ANGOBAUSDto sonzub = DeliverANGOBAUST(dto, "Freitext");
                    sonzub.FREITEXT = dto.ANGOBSONZUBTEXT;
                    sonzub.BETRAG = dto.ANGOBSONZUBUSER;
                    sonzub.BETRAG2 = dto.ANGOBSONZUBDEFAULT;

                    ANGOBAUSDto angobausPreiskarte = DeliverANGOBAUST(dto, "Preiskarte");
                    if (dto.WFMMEMOTEXT != null) angobausPreiskarte.FREITEXT = dto.WFMMEMOTEXT;
                    angobausPreiskarte.BETRAG = dto.WFMMEMOSTATTPREIS;
                    angobausPreiskarte.BETRAG2 = dto.WFMMEMOANGEBOTSPREIS;

                    //alle momentanen angobaust merken
                    for (int i = 0; i < dto.ANGOBAUST.Length; i++)
                    {
                        ANGOBAUSTFromDto.Add(ANGOBAUSTAssembler.ConvertToDomain(dto.ANGOBAUST[i]));
                        SNRList.Add(dto.ANGOBAUST[i].SNR);
                    }
                    //alle momentanen angobaust iterieren
                    foreach (ANGOBAUST ANGOBAUSTLoop in ANGOBAUSTFromDto)
                    {
                        ANGOBAUST AngobaustTemp = ANGOBHelper.GetAngobAust(Context, dto.ANGOBSYSOB, ANGOBAUSTLoop.SNR);

                        if (AngobaustTemp == null)//neue sonderausstattung
                        {
                            ANGOBAUSTLoop.SYSANGOB = dto.ANGOBSYSOB;
                            Context.ANGOBAUST.Add(ANGOBAUSTLoop);
                            Context.SaveChanges();
                            //Context.Insert < ANGOBAUST>(ANGOBAUSTLoop);
                            ModifiedANGOBAUSTS.Add(ANGOBAUSTLoop);
                            _Log.Debug("New ANGOBAUST " + ANGOBAUSTLoop.SNR);
                        }
                        else//vorhandene
                        {
                            AngobaustTemp.BESCHREIBUNG = ANGOBAUSTLoop.BESCHREIBUNG;
                            AngobaustTemp.BETRAG = ANGOBAUSTLoop.BETRAG;
                            AngobaustTemp.FLAGPACKET = ANGOBAUSTLoop.FLAGPACKET;
                            AngobaustTemp.FLAGRWREL = ANGOBAUSTLoop.FLAGRWREL;
                            AngobaustTemp.BETRAG2 = ANGOBAUSTLoop.BETRAG2;
                            AngobaustTemp.FREITEXT = ANGOBAUSTLoop.FREITEXT;
                            Context.SaveChanges();
                            ModifiedANGOBAUSTS.Add(AngobaustTemp);// Context.Update < ANGOBAUST>(AngobaustTemp, null));
                            _Log.Debug("Update ANGOBAUST " + ANGOBAUSTLoop.SNR);
                        }
                    }

                    var QueryToDelete = from angobaust in OriginalANGOBAUSTS
                                        where !SNRList.Contains(angobaust.SNR)
                                        select angobaust;

                    ANGOBAUST[] AngobaustToDelete = QueryToDelete.ToArray();
                    foreach (ANGOBAUST ANGOBAUSTLoop in AngobaustToDelete)
                    {
                        Context.DeleteObject(ANGOBAUSTLoop);
                        Context.SaveChanges();
                        //Context.Delete<ANGOBAUST>(ANGOBAUSTLoop);
                        _Log.Debug("Deleting ANGOBAUST " + ANGOBAUSTLoop.SNR);
                    }
                    Context.SaveChanges();
                    outANGOBAUST = ModifiedANGOBAUSTS.ToArray();
                }

                using (DdOlExtended Context = new DdOlExtended())
                {
                    OriginalANGEBOT = (from a in Context.ANGEBOT
                                       where a.SYSID == (long)dto.SYSID
                                       select a).FirstOrDefault();

                    OriginalANGKALK = (from a in Context.ANGKALK
                                       where a.SYSKALK == (long)dto.ANGKALKSYSKALK
                                       select a).FirstOrDefault();

                    OriginalANGKALKFS = (from a in Context.ANGKALKFS
                                         where a.SYSANGKALKFS == (long)dto.ANGKALKSYSKALK
                                         select a).FirstOrDefault();

                    OriginalANGOB = (from a in Context.ANGOB
                                     where a.SYSOB == (long)dto.ANGOBSYSOB
                                     select a).FirstOrDefault();

                    OriginalANGOBINI = (from a in Context.ANGOBINI
                                        where a.SYSOBINI == (long)dto.ANGOBSYSOB
                                        select a).FirstOrDefault();

					// check all but HHR types
                    //ANGABL abl_ALT = (from a in Context.ANGABL where (a.SYSANGEBOT == (long)dto.SYSID && a.SYSABLTYP != 43) select a).FirstOrDefault();
					ANGABL abl = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + dto.SYSID + " AND (SYSABLTYP is null OR SYSABLTYP <> 43)", null).FirstOrDefault ();
					
					if (dto.ANGKALKVERRECHNUNG.HasValue && dto.ANGKALKVERRECHNUNG.Value > 0)
                    {
                        if (abl == null)
                        {
                            abl = new ANGABL();
                            // Context.ANGABL.Add(abl);
                            abl.SYSANGEBOT = dto.SYSID;
                        }
                        
                        abl.BETRAG = dto.ANGKALKVERRECHNUNG.Value;
                        abl.IBAN = dto.ANGABLIBAN;
                        abl.FREMDVERTRAG = dto.ANGABLFREMDVERTRAG;
                        abl.FLAGINTEXT = dto.ANGABLFLAGINTEXT;
                        abl.BANK = dto.ANGABLBANK;
                        abl.AKTUELLERATE = dto.ANGABLAKTUELLERATE;

						UpdateAngabl (abl);
					}
					else if (abl != null && (dto.ANGKALKVERRECHNUNG.HasValue && dto.ANGKALKVERRECHNUNG.Value <= 0))   //no verrechnung but ablöse found --> delete
					{
						// Context.DeleteObject (abl);
						DeleteAngabl (abl.SYSANGABL);
					}

					// VVT-Ablöse (rh 20180414)
					// ANGABL ablhhr_ALT = (from a in Context.ANGABL where (a.SYSANGEBOT == (long) dto.SYSID && a.SYSABLTYP == 43) select a).FirstOrDefault ();
					ANGABL ablhhr = Context.ExecuteStoreQuery<ANGABL> ("SELECT * FROM ANGABL WHERE SYSANGEBOT = " + dto.SYSID + " AND SYSABLTYP = 43", null).FirstOrDefault ();
					if (dto.VVTAKTUELLERATE.HasValue && dto.VVTAKTUELLERATE.Value > 0)  // Vorvertrag-Ablöse FOUND: Rate und Datum  (rh: 20180412)
					{
						if (ablhhr == null)
						{// NO ablhhr FOUND --> ADD NEW
							ablhhr = new ANGABL	{ SYSANGEBOT = dto.SYSID };
							// additional UPDATE ONLY for SYSABLTYP Context.ExecuteStoreCommand ("UPDATE ANGABL SET SYSABLTYP = 43 WHERE SYSANGEBOT = " + dto.SYSID, null);
						}
						if ( ! (ablhhr.SYSANGEBOT == dto.SYSID && ablhhr.DATKALKPER == dto.VVTDATKALKPER && ablhhr.AKTUELLERATE == dto.VVTAKTUELLERATE))
						{// if NOT SAME data ALREADY in DB --> SAVE 
							ablhhr.DATKALKPER = dto.VVTDATKALKPER;
							ablhhr.AKTUELLERATE = dto.VVTAKTUELLERATE;

							UpdateAngablHHR (ablhhr);
						}
					}
					else if (ablhhr != null && (dto.VVTAKTUELLERATE.HasValue && dto.VVTAKTUELLERATE.Value <= 0)	)
					{// no VVTAKTUELLERATE but ablöse found --> DELETE
						// ALT: Context.DeleteObject(ablhhr);
						DeleteAngabl (ablhhr.SYSANGABL);
					}

					KALKTYP KalkTyp = PRPRODUCTHelper.DeliverKalkTyp(Context, (long)dto.SYSPRPRODUCT);

                    if (KalkTyp != null)
                    {
                        dto.ANGKALKSYSKALKTYP = KalkTyp.SYSKALKTYP;
                        OriginalANGKALK.SYSKALKTYP = KalkTyp.SYSKALKTYP;
                    }
                    /*if (OriginalOPTION==null)
                    {
                        OriginalOPTION = new ANGOBOPTION();
                        Context.ExecuteStoreCommand("insert into angoboption(sysid) values(" + OriginalANGOB.SYSOB + ")");
                        OriginalOPTION.sysid = OriginalANGOB.SYSOB;
                    }*/

					OriginalANGOB.SYSOBART=dto.SYSOBART.GetValueOrDefault();
                    dto.KFZBRIEF = Context.ExecuteStoreQuery<String>("select bezeichnung from obtyp where importsource = 2  and importtable like 'ETGMODLEVONE'  connect by prior sysobtypp=sysobtyp start with sysobtyp=" + dto.SYSOBTYP, null).FirstOrDefault();
                    if (dto.KFZBRIEF != null && dto.KFZBRIEF.Length > 30)
                        dto.KFZBRIEF = dto.KFZBRIEF.Substring(0, 30);

                    MyMapToEntities(dto, OriginalANGKALK, OriginalANGKALKFS, OriginalANGOB, OriginalANGOBINI, OriginalANGEBOT, null, Ust, isNew, Context,OriginalOPTION);

                    if (OriginalANGEBOT.EINZUG.HasValue)
                    {
                        LogHelper.logToDatabase("OFISWebService SaveAngebot()", "einzug=" + OriginalANGEBOT.EINZUG.Value+"/ SYSKI="+OriginalANGEBOT.SYSKI, "ANGEBOT", (long)dto.SYSID);
                    }
                    if(dto.ZUSTAND.Equals("Neu")&&dto.SYSPRPRODUCT.HasValue)//VAP changes to Kalkuliert from Neu
                    {
                       dto.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert);
                       OriginalANGEBOT.ZUSTAND = ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert);
                    }
                    
                    OriginalANGEBOT.SYSMWST = LsAddHelper.getSYSMWSTByVART(Context, dto.SYSVART);
                    OriginalANGEBOT.SYSLS = LsAddHelper.getMandantByPEROLE(Context, _SysPEROLE.Value);
                    OriginalANGEBOT.SYSWAEHRUNG = LsAddHelper.getHauswaehrungByPEROLE(Context, _SysPEROLE.Value);
                    OriginalANGKALK.SYSWAEHRUNG = LsAddHelper.getHauswaehrungByPEROLE(Context, _SysPEROLE.Value);

                    // Set FINKZ
                    if (finKz != null)
                    {
                        OriginalANGEBOT.FINKZ = finKz;
                    }

                    // Set SysAngebot
                    if (sysAngebot != null)
                    {
                        OriginalANGEBOT.SYSANGEBOT = sysAngebot;
                    }

                    // Update (with Save changes)
                    _Log.Debug("Updating ANGEBOT: " + _Log.dumpObject(OriginalANGEBOT));

                    int? cond = PRPRODUCTHelper.DeliverConditionType(Context, (long)dto.SYSPRPRODUCT);
                    if (cond != null && cond == PRPRODUCTHelper.ConditionTypeAktions)
                    {
                        OriginalANGEBOT.KONSTELLATION = tarifcode;

                    }
                    if (cond != null && cond == PRPRODUCTHelper.ConditionTypeStandard)
                    {
                        OriginalANGEBOT.KONSTELLATION = "standard";

                    }

                    if ((obartname).Equals("Barkredit"))
                    {
                        OriginalANGEBOT.OBJEKTVT = "Barkredit";
                        OriginalANGOB.BGN = 6;
                        OriginalANGOB.ABNAHMEKM = (15000 / 12 * OriginalANGKALK.LZ);
                        OriginalANGOB.SYSKGRUPPE = 301;
                        OriginalANGOB.USGAAP = 1;
                    }

                    String mref = createOrUpdateMandat(dto,  OriginalANGEBOT.SYSLS.Value, Context, _VpSysPERSON);
                    //if (mref != null)
                    //    OriginalANGEBOT.MANDATREFERENZ = mref;

                    try
                    {
                        // Context.ContextOptions.UseLegacyPreserveChangesBehavior = true;
                        Context.SaveChanges();
                        
                        /*List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PDEC1501", Value = OriginalOPTION.PDEC1501 });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PDEC1502", Value = OriginalOPTION.PDEC1502 });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = OriginalOPTION.sysid });
                        Context.ExecuteStoreCommand("update angoboption set PDEC1501=:PDEC1501,PDEC1502=:PDEC1502 where sysid=:sysid", parameters2.ToArray());*/
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        ObjectStateEntry entry = updateError.StateEntries[0];

                        _Log.Debug("ConcurrencyError for DDOL in " + entry.Entity, updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for DDOL", updateError);
                    }

                    ModifiedANGEBOT = (from c in Context.ANGEBOT
                                       where c.SYSID == dto.SYSID
                                       select c).FirstOrDefault();//Context.SelectById<ANGEBOT>((long)dto.SYSID);
                    ModifiedANGKALK = (from c in Context.ANGKALK
                                       where c.SYSKALK == dto.ANGKALKSYSKALK
                                       select c).FirstOrDefault();//Context.SelectById < ANGKALK>((long)dto.ANGKALKSYSKALK);
                    ModifiedANGKALKFS = (from c in Context.ANGKALKFS
                                         where c.SYSANGKALKFS == dto.ANGKALKSYSKALK
                                         select c).FirstOrDefault();//Context.SelectById < ANGKALKFS>((long)dto.ANGKALKSYSKALK);
                    ModifiedANGOB = (from c in Context.ANGOB
                                     where c.SYSOB == dto.ANGOBSYSOB
                                     select c).FirstOrDefault();//Context.SelectById<ANGOB>((long)dto.ANGOBSYSOB);
                    ModifiedANGOBINI = (from c in Context.ANGOBINI
                                        where c.SYSOBINI == dto.ANGOBSYSOB
                                        select c).FirstOrDefault();//Context.SelectById<ANGOBINI>((long)dto.ANGOBSYSOB);
                                                                   //  ModifiedANGOPTION =   Context.ExecuteStoreQuery < ANGOBOPTION>("select * from angoboption where sysid="+(long)dto.ANGOBSYSOB,null).FirstOrDefault();


                    if (ModifiedANGEBOT.SYSVORVT.HasValue && ModifiedANGEBOT.CONTRACTEXT.HasValue && ModifiedANGEBOT.CONTRACTEXT.Value > 0)
                    {
                        VKInfo vki = Context.ExecuteStoreQuery<VKInfo>("select sysagb, VT.SysVPFIL haendler, vt.SYSBERATADDA ausliefvk, vt.SYSBERATADDB beratvk from vt where sysid=" + ModifiedANGEBOT.SYSVORVT.Value, null).FirstOrDefault();
                        if (vki != null)
                        {
                            //verlängerungen erhalten die agb des vorvertrags wenn kleiner 6 monate
                            if (ModifiedANGKALK.LZ < 6)
                                ModifiedANGEBOT.SYSAGB = vki.sysagb;
                            else
                                updateAGB(Context, ModifiedANGEBOT, ModifiedANGEBOT.SYSID);
                        }
                    }
                    else
                    {
                        updateAGB(Context, ModifiedANGEBOT, ModifiedANGEBOT.SYSID);
                    }
                    MyUpdateSicherheiten(dto, ModifiedANGEBOT, Context);

                    // ModifiedANGEBOT.SYSVP wird nur beim anlegen zugewiesen
                    long vpPerson = _VpSysPERSON.Value;
                    long subSysPerson = _SysPERSON.Value;
                    long subSysPerole = _SysPEROLE.Value;
                    //evtl auch so lösen, falls für 10.2 in Aida1.1 die Subventionen/Provisionen nicht passen
                    /*  if (dto.noProvisionChange)//für Provisionen/Subventionen den Original-Anlegeuser verwenden (weil bei Sonderkalk der interneMitarbeiter speichert und dann diese Infos überschrieben werden)
                      {
                          vpPerson = ModifiedANGEBOT.SYSVP.Value;
                          CreatorInfoDto creator = getCreatorInfo(ModifiedANGEBOT.SYSID, Context);
                          subSysPerson = creator.sysperson;
                          subSysPerole = creator.sysperole;
                      }*/

                    Subvention.SetSubventions(dto, ModifiedANGEBOT, Context, ModifiedANGEBOT.SYSVP, subSysPerson, subSysPerole);

                    try
                    {
                        if (!dto.noProvisionChange)
                        {
                            MyUpdateProvisions(dto, ModifiedANGKALK, Context, vpPerson, false,Ust);
                        }
                        else if (dto.APROVPEROLE > 0)
                        {
                            MyUpdateProvisions(dto, ModifiedANGKALK, Context, vpPerson, true, Ust);
                        }
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        ObjectStateEntry entry = updateError.StateEntries[0];

                        _Log.Debug("ConcurrencyError for ANGPROV in " + entry.Entity, updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for ANGPROV", updateError);
                    }
                    try
                    {
                        MyUpdateInsurance(dto, ModifiedANGKALKFS, Context);
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        ObjectStateEntry entry = updateError.StateEntries[0];

                        _Log.Debug("ConcurrencyError for ANGVS in " + entry.Entity, updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for ANGVS", updateError);
                    }

                    try
                    {
                        Context.SaveChanges();
                        //TODO remove when edmx ready
                        if (mref != null)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = ModifiedANGEBOT.SYSID });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = mref });
                            Context.ExecuteStoreCommand("update angebot set mandatreferenz=:ref where sysid=:sysid", parameters.ToArray());
                        }
                        else if(ModifiedANGEBOT.EINZUG.HasValue && ModifiedANGEBOT.EINZUG<1)
                        {
                            Context.ExecuteStoreCommand("update angebot set mandatreferenz=null where sysid=" + ModifiedANGEBOT.SYSID, null);
                        }
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        ObjectStateEntry entry = updateError.StateEntries[0];

                        _Log.Debug("ConcurrencyError for DDOL in " + entry.Entity, updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for DDOL", updateError);
                    }

                    //update ANGVS
                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = ModifiedANGEBOT.SYSID });
                    //removed the join to the xypos-table, causing multiple results in mitfinflag-subquery. For yet unknown reasons causing multiple results we added a max() 
                    Context.ExecuteStoreCommand(@"update angvs set sysvs=(select sysvs from vstyp where sysvstyp=angvs.sysvstyp) ,mitfinflag=(case when exists(select 1 from prrsvpos where sysvstyp=angvs.sysvstyp)
                                                              then
                                                                  (select max(prclrsvset.mitfinflag) from angebot,PRCLRSVSET,prrsvset where  prrsvset.sysprrsvset=prclrsvset.sysprrsvset "
                                                                  + " and " + SQL.CheckCurrentSysDate ("prrsvset") +  
                                                                  // and (prrsvset.validfrom is null or prrsvset.validfrom<=sysdate or prrsvset.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                  // and (prrsvset.validuntil is null or prrsvset.validuntil>=sysdate or prrsvset.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                  
                                                                  @" and prrsvset.activeflag=1 
                                                                  and prclrsvset.sysprproduct=angebot.sysprproduct 
                                                                  and angebot.sysid=angvs.sysangebot
                                                                  )
                                                            else
                                                                  (select max(prclobvset.mitfinflag) from angebot,prclobvset,probvset where  probvset.sysprobvset=prclobvset.sysprobvset "
                                                                  + " and " + SQL.CheckCurrentSysDate ("probvset") +  
                                                                  // and (probvset.validfrom is null or probvset.validfrom<=sysdate or probvset.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                  // and (probvset.validuntil is null or probvset.validuntil>=sysdate or probvset.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))
                                                                  @" and probvset.activeflag=1 
                                                                  and prclobvset.sysprproduct=angebot.sysprproduct 
                                                                  and angebot.sysid=angvs.sysangebot
                                                                  )
                                                            end
                                                              )
                                                              where sysangebot=:sysid", parameters2.ToArray());

                    
                }
                using (DdOwExtended ContextDdOw = new DdOwExtended())
                {
                    OriginalWFMEMOvk = WFMMEMOHelper.DeliverWfmmemoFromAngebot(ContextDdOw, dto.SYSID.GetValueOrDefault(), WFMKATHelper.CnstWfmmkatNameSonderkalkulationVerkaeufer);
                    OriginalWFMEMOi = WFMMEMOHelper.DeliverWfmmemoFromAngebot(ContextDdOw, dto.SYSID.GetValueOrDefault(), WFMKATHelper.CnstWfmmkatNameSonderkalkulationInnendiest);

                    try
                    {
                        // DTO.MEMO - MEMO
                        if (dto.WFMMEMOSCALCIDTEXT != null && dto.WFMMEMOSCALCIDTEXT != "")
                        {
                            OriginalWFMEMOi = MyUpdateWFMMEMOFields(ContextDdOw, dto.SYSID.GetValueOrDefault(), OriginalWFMEMOi, WFMKATHelper.CnstWfmmkatNameSonderkalkulationInnendiest);
                            OriginalWFMEMOi.NOTIZMEMO =  (dto.WFMMEMOSCALCIDTEXT);
                            OriginalWFMEMOi.STR10 = TXT_MEMO;
                        }
                        if (dto.WFMMEMOSCALCVKTEXT != null && dto.WFMMEMOSCALCVKTEXT != "")
                        {

                            OriginalWFMEMOvk = MyUpdateWFMMEMOFields(ContextDdOw, dto.SYSID.GetValueOrDefault(), OriginalWFMEMOvk, WFMKATHelper.CnstWfmmkatNameSonderkalkulationVerkaeufer);
                            OriginalWFMEMOvk.NOTIZMEMO =  (dto.WFMMEMOSCALCVKTEXT);
                            OriginalWFMEMOvk.STR10 = TXT_MEMO;
                        }
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        _Log.Debug("ConcurrencyError for MEMOFields", updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for MEMOFields", updateError);
                    }
                    //Save the changes
                    try
                    {
                        ContextDdOw.SaveChanges();
                        // Commit the transactions
                        // OwTransaction.Commit();
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException updateError)
                    {
                        _Log.Debug("ConcurrencyError for DDOW", updateError);
                    }
                    catch (Exception updateError)
                    {
                        _Log.Error("Update failed for DDOW", updateError);
                    }

                }
            }
            catch (Exception r)
            {

                _Log.Error("Error updating ANGEBOT " + r.Message, r);
                _Log.Error(_Log.dumpObject(dto));
                _Log.Error(_Log.dumpObject(OriginalANGEBOT));
                _Log.Error(_Log.dumpObject(OriginalANGOB));
                _Log.Error(_Log.dumpObject(OriginalANGKALK));
                _Log.Error(_Log.dumpObject(OriginalANGOBINI));
                throw r;
            }
            outANGEBOT = ModifiedANGEBOT;
            outANGKALK = ModifiedANGKALK;
            outANGKALKFS = ModifiedANGKALKFS;
            outANGOB = ModifiedANGOB;
            outANGOBINI = ModifiedANGOBINI;
            outOPTION = null;// ModifiedANGOPTION;
        }

        /// <summary>
        /// After Save/Update/Deliver, actualize the Dto with the saved values
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="angkalk"></param>
        /// <param name="angkalkfs"></param>
        /// <param name="angob"></param>
        /// <param name="angobini"></param>
        /// <param name="it"></param>
        /// <param name="angobausts"></param>
        /// <returns></returns>
        public ANGEBOTDto ConvertToDto(ANGEBOT domain, ANGKALK angkalk, ANGKALKFS angkalkfs, ANGOB angob, ANGOBINI angobini, ANGEBOTITDto it, ANGOBAUST[] angobausts, ANGOBOPTION option)
        {
            ANGEBOTDto ANGEBOTDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ANGEBOTDto = new ANGEBOTDto();


            using (DdOlExtended Context = new DdOlExtended())
            {
                decimal Ust = LsAddHelper.GetTaxRate(Context, domain.SYSVART);
                MyMapToDto(domain, angkalk, angkalkfs, angob, angobini, ANGEBOTDto, it, angobausts, Ust, option);
                MyGetProvisions(ANGEBOTDto, angkalk, Context,Ust);
                MyGetInsurance(ANGEBOTDto, Context);
                MyGetSubventions(ANGEBOTDto, Context);
                MyGetHistoryData(ANGEBOTDto, Context);
                MyGetSicherheiten(ANGEBOTDto, Context);
                ANGEBOTDto.PRODUCTVALID = 0;
                if(ANGEBOTDto.SYSPRPRODUCT.HasValue)
                    ANGEBOTDto.PRODUCTVALID =getPRODUCTValid(ANGEBOTDto.SYSPRPRODUCT.Value,Context);
            }

            return ANGEBOTDto;
        }
        public static int getPRODUCTValid(long sysprproduct, DdOlExtended Context)
        {
            return Context.ExecuteStoreQuery<int>("select count(1) from prproduct where sysprproduct=" + sysprproduct + " and activeflag=1 and (validuntil is null or validuntil>= TRUNC(SYSDATE) or validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))").FirstOrDefault();
        }
        public static int getPRODUCTValidFromAngebot(long sysangebot, DdOlExtended Context)
        {
            return Context.ExecuteStoreQuery<int>("select count(1) from prproduct,angebot where angebot.sysid="+sysangebot+ " and angebot.sysprproduct=prproduct.sysprproduct and prproduct.activeflag=1 and (prproduct.validuntil is null or prproduct.validuntil>= TRUNC(SYSDATE) or  prproduct.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))").FirstOrDefault();
        }

        public ANGEBOT ConvertToDomain(ANGEBOTDto dto, ANGKALK angkalk, ANGKALKFS angkalkfs, ANGOB angob, ANGOBINI angobini, ANGEBOTITDto it, ANGOBAUST[] angobausts, ANGOBOPTION option)
        {
            ANGEBOT ANGEBOT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANGEBOT = new ANGEBOT();
            //MyMap(dto, ANGEBOT);

            return ANGEBOT;
        }
        #endregion

        #region IDtoAssembler<ANGEBOTDto,ANGEBOT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                return _Errors;
            }
        }
        #endregion

        #region My methods

        private void calcRateGesamt(ANGKALK toAngkalk, ANGEBOTDto fromAngebotDto, DdOlExtended context)
        {
            try
            {
                decimal brutto = 0, ust = 0, netto = 0;
                brutto = fromAngebotDto.ANGKALKRATEBRUTTO.Value;
                netto = fromAngebotDto.ANGKALKRATE.Value;
                ust = fromAngebotDto.ANGKALKRATEUST.Value;
                if (checkFlag(fromAngebotDto.ANGKALKFSVKFLAG))
                {
                    InsuranceDto dto = getInsurance(fromAngebotDto, context, "KASKO");
                    if (dto != null)
                    {
                        brutto += dto.InsuranceResult.Praemie;
                        netto += dto.InsuranceResult.Praemie;
                    }
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSHPFLAG))
                {
                    InsuranceDto dto = getInsurance(fromAngebotDto, context, "HP");
                    if (dto != null)
                    {
                        brutto += dto.InsuranceResult.Praemie;
                        netto += dto.InsuranceResult.Praemie;
                    }
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSINSASSENFLAG))
                {
                    InsuranceDto dto = getInsurance(fromAngebotDto, context, "IUV");
                    if (dto != null)
                    {
                        brutto += dto.InsuranceResult.Praemie;
                        netto += dto.InsuranceResult.Praemie;
                    }
                }
                /* if (checkFlag(fromAngebotDto.ANGKALKFSGAPFLAG))
                 {
                     InsuranceDto dto = getInsurance(fromAngebotDto, context, "GAP");
                     if (dto != null)
                     {
                         brutto += dto.InsuranceResult.Praemie;
                         netto += dto.InsuranceResult.Praemie;
                     }
                 }*/
                /*if (checkFlag(fromAngebotDto.ANGKALKFSRSVFLAG))
                {
                    InsuranceDto dto = getInsurance(fromAngebotDto, context, "RSDV");
                    if (dto != null)
                    {
                        brutto += dto.InsuranceResult.Praemie;
                        netto += dto.InsuranceResult.Netto;
                    }
                }*/
                if (checkFlag(fromAngebotDto.ANGKALKFSRECHTSCHUTZFLAG))
                {
                    InsuranceDto dto = getInsurance(fromAngebotDto, context, "RSV");
                    if (dto != null)
                    {
                        brutto += dto.InsuranceResult.Praemie;
                        netto += dto.InsuranceResult.Praemie;
                    }
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSMAINTENANCEFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSMAINTENANCEBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSMAINTENANCE.Value;
                    ust += fromAngebotDto.ANGKALKFSMAINTENANCEUST.Value;
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSTIRESINCLFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSSTIRESPRICEBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSSTIRESPRICE.Value;
                    ust += fromAngebotDto.ANGKALKFSSTIRESPRICEUST.Value;
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSFUELFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSFUELBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSFUELPRICE.Value;
                    ust += fromAngebotDto.ANGKALKFSFUELUST.Value;
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSREPCARFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSREPCARRATEBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSREPCARRATE.Value;
                    ust += fromAngebotDto.ANGKALKFSREPCARRATEUST.Value;
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSANABMLDFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSANABBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSANABMELDUNG.Value;
                    ust += fromAngebotDto.ANGKALKFSANABUST.Value;
                }
                if (checkFlag(fromAngebotDto.ANGKALKFSEXTRASFLAG))
                {
                    brutto += fromAngebotDto.ANGKALKFSEXTRASBRUTTO.Value;
                    netto += (decimal)fromAngebotDto.ANGKALKFSEXTRASPRICE.Value;
                    ust += fromAngebotDto.ANGKALKFSEXTRASUST.Value;
                }
                fromAngebotDto.ANGKALKRATEGESAMT = netto;
                fromAngebotDto.ANGKALKRATEGESAMTBRUTTO = brutto;
                fromAngebotDto.ANGKALKRATEGESAMTUST = ust;
            }catch(Exception e)
            {
                _Log.Debug("calcRateGesamt: " + e.Message);
            }
        }

        public static bool checkFlag(int? v)
        {
            if (v == null) return false;
            if (!v.HasValue) return false;
            if (v.Value < 1) return false;
            return true;
        }
        /// <summary>
        /// called upon update and create, maps Dto to ENTITIES
        /// </summary>
        /// <param name="fromAngebotDto"></param>
        /// <param name="toAngkalk"></param>
        /// <param name="toAngkalkfs"></param>
        /// <param name="toAngob"></param>
        /// <param name="toAngobini"></param>
        /// <param name="toAngebot"></param>
        /// <param name="toAngobaust"></param>
        /// <param name="Ust"></param>
        /// <param name="isNew"></param>
        /// <param name="context"></param>
        private void MyMapToEntities(ANGEBOTDto fromAngebotDto, ANGKALK toAngkalk, ANGKALKFS toAngkalkfs, ANGOB toAngob, ANGOBINI toAngobini, ANGEBOT toAngebot, ANGOBAUST[] toAngobaust, decimal Ust, bool isNew, DdOlExtended context, ANGOBOPTION toOption)
        {
            try
            {
                if (!(fromAngebotDto.ANGKALKSYSKALKTYP == 49 || fromAngebotDto.ANGKALKSYSKALKTYP == 52 || fromAngebotDto.ANGKALKSYSKALKTYP == 40 || fromAngebotDto.ANGKALKSYSKALKTYP == 42))
                {
                    fromAngebotDto.ANGOBSATZMEHRKMBRUTTO = 0;
                    fromAngebotDto.ANGOBSATZMINDERKMBRUTTO = 0;
                    fromAngebotDto.ANGOBSATZMINDERKM = 0;
                    fromAngebotDto.ANGOBSATZMEHRKM = 0;
                    fromAngebotDto.ANGOBKMTOLERANZ = 0;
                }

                validateInsurances(fromAngebotDto);


                //-----------------------ANGEBOT-------------------------------------
                toAngebot.SYSIT = (fromAngebotDto.SYSIT.HasValue && fromAngebotDto.SYSIT.Value < 1L ? null : fromAngebotDto.SYSIT);
                //TODO SEPA
                toAngebot.SYSKI = (fromAngebotDto.SYSKI.HasValue && fromAngebotDto.SYSKI.Value < 1L ? null : fromAngebotDto.SYSKI);
                toAngebot.EINZUG = fromAngebotDto.EINZUG;
               

                toAngebot.SYSPRPRODUCT = fromAngebotDto.SYSPRPRODUCT;
                if (_SysBrand.HasValue)
                    toAngebot.SYSBRAND= _SysBrand.Value;
                toAngebot.VERTRIEBSWEG = fromAngebotDto.VERTRIEBSWEG;
                toAngebot.SYSANGEBOT = fromAngebotDto.SYSANGEBOT;
                toAngebot.SYSVART = fromAngebotDto.SYSVART;
                toAngebot.SYSVARTTAB = fromAngebotDto.SYSVARTTAB;
                toAngebot.ANGEBOT1 = fromAngebotDto.ANGEBOT1;
                toAngebot.BEGINN = fromAngebotDto.BEGINN;
                toAngebot.ENDE = fromAngebotDto.ENDE;
                toAngebot.ERFASSUNG = fromAngebotDto.ERFASSUNG;

                toAngebot.SYSVORVT = fromAngebotDto.SYSVORVT;
                toAngebot.CONTRACTEXT = fromAngebotDto.CONTRACTEXT;
                toAngebot.CONTRACTTYPE = fromAngebotDto.CONTRACTTYPE;

                if (toAngebot.ZUSTAND == null)
                    toAngebot.ZUSTAND = fromAngebotDto.ZUSTAND;
                // Get the status from ANGEBOT and set Aktivkz depending on the status
                AngebotZustand CurrentStatus = ZustandHelper.GetEnumeratorFromString(toAngebot.ZUSTAND);
                if (Array.Exists<AngebotZustand>(new AngebotZustand[] { AngebotZustand.Abgelaufen, AngebotZustand.Storniert, AngebotZustand.StornoWiedereinreichung }, Status => Status == CurrentStatus))
                {
                    toAngebot.AKTIVKZ = 0;
                }
                else
                {
                    toAngebot.AKTIVKZ = 1;
                }
                toAngebot.ZUSTANDAM = fromAngebotDto.ZUSTANDAM;
                toAngebot.TRADEONOWNACCOUNT = fromAngebotDto.TRADEONOWNACCOUNT;
                toAngebot.OBJEKTVT = fromAngebotDto.OBJEKTVT;
                toAngebot.DATANGEBOT = fromAngebotDto.DATANGEBOT;
                toAngebot.SYSVART = fromAngebotDto.SYSVART;
                toAngebot.SYSVTTYP = fromAngebotDto.SYSVTTYP;
                toAngebot.SYSPRPRODUCT = fromAngebotDto.SYSPRPRODUCT;
                toAngebot.RATE = fromAngebotDto.RATE;
                toAngebot.ZINS = fromAngebotDto.ZINS;
                toAngebot.VART = fromAngebotDto.VART;
                if (fromAngebotDto.SPECIALCALCCOUNT != null)
                    toAngebot.SPECIALCALCCOUNT = fromAngebotDto.SPECIALCALCCOUNT;
                toAngebot.SPECIALCALCSTATUS = fromAngebotDto.SPECIALCALCSTATUS;
                toAngebot.SPECIALCALCDATE = fromAngebotDto.SPECIALCALCDATE;
                toAngebot.SPECIALCALCSYSWFUSER = fromAngebotDto.SPECIALCALCSYSWFUSER;
                //toAngebot.VPVERTRAG = "Extranet";
                //Ticket OSTER-2010-10-22-8265 
                toAngebot.PPY = 12;


                //-----------------------ANGKALK-------------------------------------
                toAngkalk.GEBUEHRNACHLASS = fromAngebotDto.ANGKALKGEBUEHR_NACHLASS;
                toAngkalk.BGEXTERN = fromAngebotDto.ANGKALKBGEXTERN;
                toAngkalk.BGEXTERNBRUTTO = fromAngebotDto.ANGKALKBGEXTERNBRUTTO;
                toAngkalk.BWFEHLER = fromAngebotDto.ANGKALKBWFEHLER;
                toAngkalk.OBJECTMETACALCTARGET = fromAngebotDto.ANGKALKOBJECTMETACALCTARGET;

                toAngkalk.BGINTERNBRUTTO = fromAngebotDto.ANGKALKBGINTERNBRUTTO;
                if (fromAngebotDto.ANGKALKBGINTERNBRUTTO.HasValue)
                    toAngkalk.BGINTERN = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)fromAngebotDto.ANGKALKBGINTERNBRUTTO, Ust);
                toAngkalk.BGEXTERNUST = fromAngebotDto.ANGKALKBGEXTERNUST;
                toAngkalk.DEPOT = fromAngebotDto.ANGKALKDEPOT;
                toAngkalk.DEPOTP = fromAngebotDto.ANGKALKDEPOTP;
                toAngkalk.GEBUEHR = fromAngebotDto.ANGKALKGEBUEHR;
                toAngkalk.GEBUEHRBRUTTO = fromAngebotDto.ANGKALKGEBUEHRBRUTTO;
                toAngkalk.GEBUEHRUST = fromAngebotDto.ANGKALKGEBUEHRUST;
                toAngkalk.LZ = fromAngebotDto.ANGKALKLZ;
                toAngkalk.MITFINBRUTTO = fromAngebotDto.ANGKALKMITFINBRUTTO;
                toAngkalk.MITFINUST = fromAngebotDto.ANGKALKMITFINUST;
                toAngkalk.PAKRABO = fromAngebotDto.ANGKALKPAKRABO;
                //toAngkalk.PPY = fromAngebotDto.ANGKALKPPY;
                //Ticket OSTER-2010-10-22-8265 
                toAngkalk.PPY = 12;
                toAngkalk.RABATTO = fromAngebotDto.ANGKALKRABATTO;
                toAngkalk.RABATTOP = fromAngebotDto.ANGKALKRABATTOP;
                toAngkalk.RATE = fromAngebotDto.ANGKALKRATE;
                toAngkalk.RATEBRUTTO = fromAngebotDto.ANGKALKRATEBRUTTO;
                toAngkalk.RATEUST = fromAngebotDto.ANGKALKRATEUST;

                toAngkalk.RATEGESAMT = fromAngebotDto.ANGKALKRATEGESAMT;
                toAngkalk.RATEGESAMTBRUTTO = fromAngebotDto.ANGKALKRATEGESAMTBRUTTO;
                toAngkalk.RATEGESAMTUST = fromAngebotDto.ANGKALKRATEGESAMTUST;
                calcRateGesamt(toAngkalk, fromAngebotDto, context);
                try
                {
                    if (fromAngebotDto.ANGKALKERSTERATE.HasValue && toAngebot.GUELTIGBIS.Value.CompareTo(fromAngebotDto.ANGKALKERSTERATE) > 0)
                    {
                        DateTime nextMonth = fromAngebotDto.ANGKALKERSTERATE.Value.Date.AddMonths(1);
                        if (toAngebot.GUELTIGBIS.Value.CompareTo(nextMonth) > 0)
                            nextMonth = nextMonth.Date.AddMonths(1);
                        if (toAngebot.GUELTIGBIS.Value.CompareTo(nextMonth) > 0)
                            nextMonth = nextMonth.Date.AddMonths(1);
                        fromAngebotDto.ANGKALKERSTERATE = nextMonth;
                    }
                    toAngkalk.ERSTERATE = fromAngebotDto.ANGKALKERSTERATE;
                    if (toAngkalk.ERSTERATE.HasValue)
                        toAngkalk.LETZTERATE = toAngkalk.ERSTERATE.Value.Date.AddMonths(toAngkalk.LZ.Value);
                }catch(Exception exl)
                {
                    _Log.Error("Error managing the ersterate/letzterate field " + fromAngebotDto.ANGKALKERSTERATE + "/" + toAngebot.GUELTIGBIS + "/" + toAngkalk.LZ, exl);
                }
                toAngkalk.RGGEBUEHR = fromAngebotDto.ANGKALKRGGEBUEHR;
                toAngkalk.RGGFREI = fromAngebotDto.ANGKALKRGGFREI;
                toAngkalk.RGGVERR = fromAngebotDto.ANGKALKRGGVERR;
                // toAngkalk.RWBASE = fromAngebotDto.ANGOBRWBASE;
                toAngkalk.RWBASE = fromAngebotDto.ANGKALKRWBASE;
                toAngkalk.RWBASEBRUTTO = fromAngebotDto.ANGKALKRWBASEBRUTTO;
                toAngkalk.RWBASEBRUTTOP = fromAngebotDto.ANGKALKRWBASEBRUTTOP;
                toAngkalk.RWBASEUST = fromAngebotDto.ANGKALKRWBASEUST;
                toAngkalk.RWCRV = fromAngebotDto.ANGOBRWCRV;
                toAngkalk.GESAMTKREDIT = fromAngebotDto.ANGKALKKREDITBETRAG;
                toAngkalk.RESTKAUFPREIS = fromAngebotDto.RESTKAUFPREIS;
                toAngkalk.RWCRVBRUTTO = fromAngebotDto.ANGKALKRWCRVBRUTTO;
                toAngkalk.RWCRVBRUTTOP = fromAngebotDto.ANGKALKRWCRVBRUTTOP;
                toAngkalk.RWCRVUST = fromAngebotDto.ANGKALKRWCRVUST;
                toAngkalk.RWKALK = fromAngebotDto.ANGKALKRWKALK;
                toAngkalk.RWKALKBRUTTO = fromAngebotDto.ANGKALKRWKALKBRUTTO;
                toAngkalk.RWKALKBRUTTOP = fromAngebotDto.ANGKALKRWKALKBRUTTOP;
                toAngkalk.RWKALKBRUTTODEF = fromAngebotDto.ANGKALKRWKALKBRUTTO_DEFAULT;
                toAngkalk.RWKALKBRUTTOPDEF = fromAngebotDto.ANGKALKRWKALKBRUTTOP_DEFAULT;

                toAngkalk.RWKALKBRUTTOORG = fromAngebotDto.ANGKALKRWKALKBRUTTOORG;
                toAngkalk.RWKALKBRUTTOPORG = fromAngebotDto.ANGKALKRWKALKBRUTTOPORG;

                toAngkalk.RWKALKDEF = fromAngebotDto.ANGKALKRWKALK_DEFAULT;
                toAngkalk.RWKALKUST = fromAngebotDto.ANGKALKRWKALKUST;
                toAngkalk.RWKALKUSTDEF = fromAngebotDto.ANGKALKRWKALKUST_DEFAULT;

                //ab hier
                toAngkalk.SZ = fromAngebotDto.ANGKALKSZ;
                toAngkalk.SZBRUTTO = fromAngebotDto.ANGKALKSZBRUTTO;
                toAngkalk.SZBRUTTOP = fromAngebotDto.ANGKALKSZBRUTTOP;
                toAngkalk.SZP = fromAngebotDto.ANGKALKSZP;
                toAngkalk.SZUST = fromAngebotDto.ANGKALKSZUST;
                toAngkalk.ZUBEHOERBRUTTO = fromAngebotDto.ANGKALKZUBEHOERBRUTTO;
                toAngkalk.ZUBEHOERNETTO = fromAngebotDto.ANGKALKZUBEHOERNETTO;
                toAngkalk.ZUBEHOEROR = fromAngebotDto.ANGKALKZUBEHOEROR;
                toAngkalk.ZUBEHOERORP = fromAngebotDto.ANGKALKZUBEHOERORP;
                toAngkalk.ZUSTAND = fromAngebotDto.KALKULATIONSOURCE.ToString();
                toAngkalk.SYSKALKTYP = fromAngebotDto.ANGKALKSYSKALKTYP;
                toAngkalk.ZINS = fromAngebotDto.ANGKALKZINS;
                toAngkalk.ZINSDEF = fromAngebotDto.ANGKALKZINS_DEFAULT;
                toAngkalk.BASISZINS = fromAngebotDto.ANGKALKZINSBASIS;//Basiszins
                toAngkalk.ZINS3 = fromAngebotDto.ANGKALKZINSAKTION;//Aktionszins

                //May cause ORA-01438 when too big.
                if (fromAngebotDto.ANGKALKZINSEFF < 100)
                {
                    toAngkalk.ZINSEFF = fromAngebotDto.ANGKALKZINSEFF;
                    toAngkalk.ZINSEFFDEF = fromAngebotDto.ANGKALKZINSEFF_DEFAULT;
                }
                else
                {
                    _Log.Warn("Invalid ZINSEFF while saving: " + fromAngebotDto.ANGKALKZINSEFF);
                }

                toAngkalk.BGINTERN = fromAngebotDto.ANGKALKBGINTERN;
                toAngkalk.MITFIN = fromAngebotDto.ANGKALKMITFIN;
                //Telefonat 12.10. HC - ZINSTYP kommt weg
                if (!fromAngebotDto.ANGKALKZINSTYP.HasValue || fromAngebotDto.ANGKALKZINSTYP == 0)
                    fromAngebotDto.ANGKALKZINSTYP = 1;

                toAngkalk.SYSINTTYPE = fromAngebotDto.ANGKALKZINSTYP;
                toAngkalk.REFIZINS1 = fromAngebotDto.ANGKALKREFIZINS1;
                toAngkalk.GEBUEHRINTERNBRUTTO = fromAngebotDto.ANGKALKGEBUEHRINTERNBRUTTO;
                toAngkalk.VERRECHNUNG = fromAngebotDto.ANGKALKVERRECHNUNG;
                toAngkalk.GESAMTBRUTTO = fromAngebotDto.ANGKALKGESAMTBRUTTO;
                toAngkalk.GESAMTNETTO = fromAngebotDto.ANGKALKGESAMTNETTO;
                toAngkalk.GESAMTUST = fromAngebotDto.ANGKALKGESAMTUST;
                toAngkalk.GESAMTKOSTEN = fromAngebotDto.ANGKALKGESAMTKOSTEN;
                toAngkalk.GESAMTKOSTENBRUTTO = fromAngebotDto.ANGKALKGESAMTKOSTENBRUTTO;
                toAngkalk.GESAMTKOSTENUST = fromAngebotDto.ANGKALKGESAMTKOSTENUST;
                fromAngebotDto.ANGKALKGRUNDNETTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(fromAngebotDto.ANGKALKBGEXTERNBRUTTO.GetValueOrDefault());
                toAngkalk.GRUND = fromAngebotDto.ANGKALKGRUNDNETTO;
                toAngkalk.PAKRABOP = fromAngebotDto.ANGOBPAKRABOP;
                toAngkalk.PAKETE = fromAngebotDto.ANGKALKPAKETENETTO;
                toAngkalk.ZINS1 = fromAngebotDto.ANGKALKZINS1;
                toAngkalk.ZINS2 = fromAngebotDto.ANGKALKZINS2;
                toAngkalk.SONZUB = fromAngebotDto.ANGKALKSONZUBNETTO;
                toAngkalk.RWCRV = fromAngebotDto.ANGKALKRWCRV;


                //-----------------------ANGKALKFS-------------------------------------
                toAngkalkfs.SYSITRSVVN = fromAngebotDto.SYSITRSVVN;
                toAngkalkfs.FUELLIEFERANT = fromAngebotDto.HIST_ANGKALKFUELSYSFSTYP;
                toAngkalkfs.FUELBEZEICHNUNG = fromAngebotDto.HIST_ANGOBINIMOTORTYP1;
                toAngkalkfs.EXTRASPRICEUNIT = fromAngebotDto.ANGKALKFSEXTRASPRICEUNIT;
                toAngkalkfs.MAINTPRICEKM = fromAngebotDto.ANGKALKFSKOSTENPERKILOMETER;
                //An/Abmelde-Service
                toAngkalkfs.ANABMLDFLAG = fromAngebotDto.ANGKALKFSANABMLDFLAG;
                toAngkalkfs.ANABBRUTTO = fromAngebotDto.ANGKALKFSANABBRUTTO;
                toAngkalkfs.ANABMELDUNG = fromAngebotDto.ANGKALKFSANABMELDUNG;
                toAngkalkfs.ANABUST = fromAngebotDto.ANGKALKFSANABUST;
                toAngkalkfs.ANABKENNZINKLFLAG = fromAngebotDto.ANGKALKFSANABKENNZINKLFLAG;
                toAngkalkfs.ANABNACHLASS = fromAngebotDto.ANGKALKFSANABNACHLASS;
                if (fromAngebotDto.ANGKALKFSANABGROSSUNITPRICE.HasValue)
                    toAngkalkfs.ANABPRICE = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)fromAngebotDto.ANGKALKFSANABGROSSUNITPRICE, Ust);
                toAngkalkfs.ANABPRICEBRUTTO = fromAngebotDto.ANGKALKFSANABGROSSUNITPRICE;
                toAngkalkfs.EXTRASBRUTTO = fromAngebotDto.ANGKALKFSEXTRASBRUTTO;
                toAngkalkfs.EXTRASFLAG = fromAngebotDto.ANGKALKFSEXTRASFLAG;
                toAngkalkfs.EXTRASPRICE = fromAngebotDto.ANGKALKFSEXTRASPRICE;
                toAngkalkfs.EXTRASUST = fromAngebotDto.ANGKALKFSEXTRASUST;
                toAngkalkfs.FUELBRUTTO = fromAngebotDto.ANGKALKFSFUELBRUTTO;
                toAngkalkfs.FUELFLAG = fromAngebotDto.ANGKALKFSFUELFLAG;

                toAngkalkfs.FUELPRICE = fromAngebotDto.ANGKALKFSFUELPRICE;
                toAngkalkfs.FUELUST = fromAngebotDto.ANGKALKFSFUELUST;
                toAngkalkfs.FUELSYSFSTYP = fromAngebotDto.ANGKALKFUELSYSFSTYP;
                toAngkalkfs.MAINTENANCE = fromAngebotDto.ANGKALKFSMAINTENANCE;
                toAngkalkfs.MAINTENANCEBRUTTO = fromAngebotDto.ANGKALKFSMAINTENANCEBRUTTO;
                toAngkalkfs.MAINTENANCEFLAG = fromAngebotDto.ANGKALKFSMAINTENANCEFLAG;
                toAngkalkfs.MAINTENANCEUST = fromAngebotDto.ANGKALKFSMAINTENANCEUST;
                toAngkalkfs.MAINTFIXFLAG = fromAngebotDto.ANGKALKFSMAINTFIXFLAG;
                toAngkalkfs.REPCARCOUNT = fromAngebotDto.ANGKALKFSREPCARCOUNT;
                toAngkalkfs.REPCARFLAG = fromAngebotDto.ANGKALKFSREPCARFLAG;
                toAngkalkfs.REPCARPRICE = fromAngebotDto.ANGKALKFSREPCARPRICE;
                toAngkalkfs.REPCARRATE = fromAngebotDto.ANGKALKFSREPCARRATE;
                toAngkalkfs.REPCARRATEBRUTTO = fromAngebotDto.ANGKALKFSREPCARRATEBRUTTO;
                toAngkalkfs.REPCARRATEUST = fromAngebotDto.ANGKALKFSREPCARRATEUST;
                toAngkalkfs.RIMSCODEH = fromAngebotDto.ANGKALKFSRIMSCODEH;
                toAngkalkfs.RIMSCODEV = fromAngebotDto.ANGKALKFSRIMSCODEV;
                toAngkalkfs.RIMSCOUNTH = fromAngebotDto.ANGKALKFSRIMSCOUNTH;
                toAngkalkfs.RIMSCOUNTV = fromAngebotDto.ANGKALKFSRIMSCOUNTV;
                toAngkalkfs.RIMSPRICEH = fromAngebotDto.ANGKALKFSRIMSPRICEH;
                toAngkalkfs.RIMSPRICEV = fromAngebotDto.ANGKALKFSRIMSPRICEV;
                toAngkalkfs.STIRESCODEH = fromAngebotDto.ANGKALKFSSTIRESCODEH;
                toAngkalkfs.STIRESCODEV = fromAngebotDto.ANGKALKFSSTIRESCODEV;
                toAngkalkfs.STIRESCOUNTH = fromAngebotDto.ANGKALKFSSTIRESCOUNTH;
                toAngkalkfs.STIRESCOUNTV = fromAngebotDto.ANGKALKFSSTIRESCOUNTV;
                toAngkalkfs.STIRESMODH = fromAngebotDto.ANGKALKFSSTIRESMODH;
                toAngkalkfs.STIRESMODV = fromAngebotDto.ANGKALKFSSTIRESMODV;
                toAngkalkfs.STIRESPRICE = fromAngebotDto.ANGKALKFSSTIRESPRICE;
                toAngkalkfs.STIRESPRICEH = fromAngebotDto.ANGKALKFSSTIRESPRICEH;
                toAngkalkfs.STIRESPRICEHBRUTTO = fromAngebotDto.ANGKALKFSSTIRESPRICEHBRUTTO;
                toAngkalkfs.STIRESPRICEHUST = fromAngebotDto.ANGKALKFSSTIRESPRICEHUST;
                toAngkalkfs.STIRESPRICEV = fromAngebotDto.ANGKALKFSSTIRESPRICEV;
                toAngkalkfs.STIRESPRICEVBRUTTO = fromAngebotDto.ANGKALKFSSTIRESPRICEVBRUTTO;
                toAngkalkfs.STIRESPRICEVUST = fromAngebotDto.ANGKALKFSSTIRESPRICEVUST;
                toAngkalkfs.TIRESADDITION = fromAngebotDto.ANGKALKFSTIRESADDITION;
                toAngkalkfs.TIRESADDITIONBRUTTO = fromAngebotDto.ANGKALKFSTIRESADDITIONBRUTTO;
                toAngkalkfs.TIRESADDITIONUST = fromAngebotDto.ANGKALKFSTIRESADDITIONUST;
                toAngkalkfs.TIRESFIXFLAG = fromAngebotDto.ANGKALKFSTIRESFIXFLAG;
                toAngkalkfs.TIRESFLAG = fromAngebotDto.ANGKALKFSTIRESFLAG;
                toAngkalkfs.TIRESINCLFLAG = fromAngebotDto.ANGKALKFSTIRESINCLFLAG;
                toAngkalkfs.WTIRESCODEH = fromAngebotDto.ANGKALKFSWTIRESCODEH;
                toAngkalkfs.WTIRESCODEV = fromAngebotDto.ANGKALKFSWTIRESCODEV;
                toAngkalkfs.WTIRESCOUNTH = fromAngebotDto.ANGKALKFSWTIRESCOUNTH;
                toAngkalkfs.WTIRESCOUNTV = fromAngebotDto.ANGKALKFSWTIRESCOUNTV;
                toAngkalkfs.WTIRESMODH = fromAngebotDto.ANGKALKFSWTIRESMODH;
                toAngkalkfs.WTIRESMODV = fromAngebotDto.ANGKALKFSWTIRESMODV;
                toAngkalkfs.WTIRESPRICEH = fromAngebotDto.ANGKALKFSWTIRESPRICEH;
                toAngkalkfs.WTIRESPRICEHBRUTTO = fromAngebotDto.ANGKALKFSWTIRESPRICEHBRUTTO;
                toAngkalkfs.WTIRESPRICEHUST = fromAngebotDto.ANGKALKFSWTIRESPRICEHUST;
                toAngkalkfs.WTIRESPRICEV = fromAngebotDto.ANGKALKFSWTIRESPRICEV;
                toAngkalkfs.WTIRESPRICEVBRUTTO = fromAngebotDto.ANGKALKFSWTIRESPRICEVBRUTTO;
                toAngkalkfs.WTIRESPRICEVUST = fromAngebotDto.ANGKALKFSWTIRESPRICEVUST;
                toAngkalkfs.FUELNACHLASS = fromAngebotDto.ANGKALKFSFUELNACHLASS;
                toAngkalkfs.MAINTNACHLASS = fromAngebotDto.ANGKALKFSMAINTNACHLASS;
                toAngkalkfs.TIRESNACHLASS = fromAngebotDto.ANGKALKFSTIRESNACHLASS;
                toAngkalkfs.REPCARNACHLASS = fromAngebotDto.ANGKALKFSREPCARNACHLASS;
                toAngkalkfs.REPCARNACHLASS = fromAngebotDto.ANGKALKFSEXTRASNACHLASS;
                toAngkalkfs.STIRESPRICEBRUTTO = fromAngebotDto.ANGKALKFSSTIRESPRICEBRUTTO;
                toAngkalkfs.STIRESPRICEUST = fromAngebotDto.ANGKALKFSSTIRESPRICEUST;
                toAngkalkfs.TIRESSETS = fromAngebotDto.ANGKALKFSTIRESSETS;
                toAngkalkfs.RIMSMODV = fromAngebotDto.ANGKALKFSRIMSMODV;
                toAngkalkfs.RIMSMODH = fromAngebotDto.ANGKALKFSRIMSMODH;
                toAngkalkfs.RIMSPRICEVUST = fromAngebotDto.ANGKALKFSRIMSPRICEVUST;
                toAngkalkfs.RIMSPRICEVBRUTTO = fromAngebotDto.ANGKALKFSRIMSPRICEVBRUTTO;
                toAngkalkfs.SYSITRSVVN = fromAngebotDto.SYSITRSVVN;
                toAngkalkfs.RIMSPRICEHUST = fromAngebotDto.ANGKALKFSRIMSPRICEHUST;
                toAngkalkfs.RIMSPRICEHBRUTTO = fromAngebotDto.ANGKALKFSRIMSPRICEHBRUTTO;
                toAngkalkfs.FUELPRICEAVGBRUTTO = fromAngebotDto.FUELPRICEAVGBRUTTO;
                toAngkalkfs.STIRESTEXTH = fromAngebotDto.ANGKALKFSSTIRESTEXTH;
                toAngkalkfs.STIRESTEXTV = fromAngebotDto.ANGKALKFSSTIRESTEXTV;
                toAngkalkfs.WTIRESTEXTH = fromAngebotDto.ANGKALKFSWTIRESTEXTH;
                toAngkalkfs.WTIRESTEXTV = fromAngebotDto.ANGKALKFSWTIRESTEXTV;

                if (fromAngebotDto.ANGKALKFSMAINTFIXFLAG.HasValue && fromAngebotDto.ANGKALKFSMAINTFIXFLAG.Value == 1)//#4061
                {
                    toAngkalkfs.MEHRKMBRUTTO = fromAngebotDto.ANGKALKFSMEHRKMBRUTTO;
                    toAngkalkfs.MINDERKMBRUTTO = fromAngebotDto.ANGKALKFSMINDERKMBRUTTO;
                    if (fromAngebotDto.ANGKALKFSMEHRKMBRUTTO.HasValue)
                        toAngkalkfs.MEHRKMUST = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue((decimal)fromAngebotDto.ANGKALKFSMEHRKMBRUTTO, Ust);
                    if (fromAngebotDto.ANGKALKFSMINDERKMBRUTTO.HasValue)
                        toAngkalkfs.MINDERKMUST = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue((decimal)fromAngebotDto.ANGKALKFSMINDERKMBRUTTO, Ust);
                    if (fromAngebotDto.ANGKALKFSMEHRKMBRUTTO.HasValue)
                        toAngkalkfs.MEHRKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)fromAngebotDto.ANGKALKFSMEHRKMBRUTTO, Ust);
                    if (fromAngebotDto.ANGKALKFSMINDERKMBRUTTO.HasValue)
                        toAngkalkfs.MINDERKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue((decimal)fromAngebotDto.ANGKALKFSMINDERKMBRUTTO, Ust);
                }
                toAngkalkfs.REPCARCOUNT = fromAngebotDto.ANGKALKFSREPCARCOUNT;
                toAngkalkfs.RECHTSCHUTZFLAG = fromAngebotDto.ANGKALKFSRECHTSCHUTZFLAG;
                toAngkalkfs.INSASSENFLAG = fromAngebotDto.ANGKALKFSINSASSENFLAG;
                //HCE TODO
                //toAngkalkfs.INSASSENFLAG = fromAngebotDto.ANGKALKFSGAPFLAG;
                toAngkalkfs.HPFLAG = fromAngebotDto.ANGKALKFSHPFLAG;
                toAngkalkfs.VKFLAG = fromAngebotDto.ANGKALKFSVKFLAG;
                toAngkalkfs.RSVFLAG = fromAngebotDto.ANGKALKFSRSVFLAG;


                if (fromAngebotDto.ANGOBKW != null && fromAngebotDto.ANGOBKW.Value > 0)
                    fromAngebotDto.ANGOBINIKW = (long)fromAngebotDto.ANGOBKW.Value;
                //-----------------------ANGOBINI-------------------------------------
                toAngobini.ACTUATION = fromAngebotDto.ANGOBINIACTUATION;
                toAngobini.CCM = fromAngebotDto.ANGOBCCM;
                toAngobini.KW = (fromAngebotDto.ANGOBINIKW != null) ? fromAngebotDto.ANGOBINIKW : 0;
                toAngobini.PS = (long)Math.Round((double)toAngobini.KW * 1.36, 0);
                toAngobini.CO2 = fromAngebotDto.ANGOBINICO2;
                toAngobini.KMSTAND = fromAngebotDto.ANGOBINIKMSTAND;
                toAngobini.NOX = fromAngebotDto.ANGOBININOX;
                toAngobini.PARTICLES = fromAngebotDto.ANGOBINIPARTICLES;
                toAngobini.VERBRAUCH_D = fromAngebotDto.ANGOBINIVERBRAUCH_D;
                toAngobini.VORBESITZER = fromAngebotDto.ANGOBINIVORBESITZER;
                toAngobini.ERSTZUL = fromAngebotDto.ANGOBINIERSTZUL;
                toAngobini.NOVA_P = fromAngebotDto.ANGOBININOVA_P;
                //telefonat 12.10. Harald Cich
                toAngobini.MOTORTYP = "0";//0 == Benzin, 1== Diesel, 2 == Hybrid, 
                toAngobini.SYSPRMART=fromAngebotDto.SYSMART;
                toAngobini.ACTUATION = 1;
                if ((fromAngebotDto.ANGOBINIMOTORTYP != null) && (fromAngebotDto.ANGOBINIMOTORTYP.Length >= 1))
                {
                    switch (fromAngebotDto.ANGOBINIMOTORTYP)
                    {

                        case "UNDEFINED": toAngobini.MOTORFUEL = "0"; toAngobini.MOTORTYP = "0"; break;
                        case "UNLEADED_PETROL_AND_ETHANOL": toAngobini.MOTORFUEL = "14"; toAngobini.MOTORTYP = "0"; break;
                        case "DIESEL": toAngobini.ACTUATION = 2; toAngobini.MOTORFUEL = "3"; toAngobini.MOTORTYP = "1"; break;
                        case "UNLEADED_PETROL": toAngobini.MOTORFUEL = "14"; toAngobini.MOTORTYP = "0"; break;
                        case "PETROL": toAngobini.MOTORFUEL = "8"; toAngobini.MOTORTYP = "0"; break;
                        case "GAS": toAngobini.MOTORFUEL = "10"; toAngobini.MOTORTYP = "0"; break;
                        case "ELECTRICITY": toAngobini.ACTUATION = 3; toAngobini.MOTORFUEL = "4"; toAngobini.MOTORTYP = "0"; break;

                    }
                }
                if (fromAngebotDto.ANGOBINIACTUATION == 1)//Hybrid
                    toAngobini.ACTUATION = 3;
                if (fromAngebotDto.ANGOBFZART.Equals("Barkredit"))//#183550
                    toAngobini.MOTORTYP = "2";
                toAngobini.KFZBRIEF = fromAngebotDto.KFZBRIEF;


                //CO2 Reifen - default values
                toAngob.NOVAPDEF = fromAngebotDto.ANGOBNOVAPDEF;
                toAngobini.CO2DEF = fromAngebotDto.ANGOBINICO2DEF;
                toAngobini.NOXDEF = fromAngebotDto.ANGOBININOXDEF;
                toAngobini.VERBRAUCH_DDEF = fromAngebotDto.ANGOBINIVERBRAUCH_DDEF;
                toAngobini.PARTICLESDEF = fromAngebotDto.ANGOBINIPARTICLESDEF;
                toAngobini.ACTUATIONDEF = fromAngebotDto.ANGOBINIACTUATIONDEF;



                //-----------------------ANGOB-------------------------------------
                //CRITICAL?
                if (fromAngebotDto.SYSOBART.HasValue)
                    toAngob.SYSOBART=(long)fromAngebotDto.SYSOBART;
                toAngob.SYSOBKAT=fromAngebotDto.ANGOBSYSOBKAT;
                if (fromAngebotDto.SYSOBTYP.HasValue)
                    toAngob.SYSOBTYP=  (long)fromAngebotDto.SYSOBTYP;
                toAngob.PAKETBRUTTOEXKLN = fromAngebotDto.ANGOBPAKETEBRUTTOEXKLNOVA;
                toAngob.SONZUBBRUTTOEXKLN = fromAngebotDto.ANGOBSONZUBBRUTTOEXKLNOVA;
                toAngob.SONZUBRV = fromAngebotDto.ANGOBSONZUBRV;
                toAngob.GRUNDBRUTTOEXKLN = fromAngebotDto.ANGOBGRUNDBRUTTOEXKLNOVA;

                toAngob.FZART = fromAngebotDto.ANGOBFZART;
                toAngob.AHK = fromAngebotDto.ANGOBAHK;
                toAngkalk.AHK = toAngob.AHK;
                toAngob.RWCRV = fromAngebotDto.ANGOBRWCRV;
                toAngob.JAHRESKM = fromAngebotDto.ANGOBJAHRESKM;
                toAngob.AUTOMATIK = fromAngebotDto.ANGOBAUTOMATIK;
                toAngob.FABRIKAT = fromAngebotDto.ANGOBFABRIKAT;
                if (toAngob.FABRIKAT != null && toAngob.FABRIKAT.Length > 40)
                    toAngob.FABRIKAT = toAngob.FABRIKAT.Substring(0, 40);
                toAngob.FARBEA = fromAngebotDto.ANGOBFARBEA;
                if (fromAngebotDto.ANGOBFARBEA != null && fromAngebotDto.ANGOBFARBEA.Length > 20)
                    toAngob.FARBEA = fromAngebotDto.ANGOBFARBEA.Substring(0, 20);
                toAngob.FZNR = fromAngebotDto.ANGOBFZNR;
                toAngob.HERSTELLER = fromAngebotDto.ANGOBHERSTELLER;
                toAngob.BAUJAHR = fromAngebotDto.ANGOBBAUJAHR;
                toAngob.CCM = fromAngebotDto.ANGOBCCM;
                toAngob.KW = fromAngebotDto.ANGOBINIKW;
                toAngob.KMTOLERANZ = fromAngebotDto.ANGOBKMTOLERANZ;
                toAngob.CONFIGID = fromAngebotDto.ANGOBCONFIGID;
                toAngob.POLSTERCODE = fromAngebotDto.ANGOBPOLSTERCODE;
                toAngob.POLSTERTEXT = fromAngebotDto.ANGOBPOLSTERTEXT;
                toAngob.CONFIGSOURCE = fromAngebotDto.ANGOBCONFIGSOURCE.ToString();
                toAngob.CONFIGPICTUREURL = fromAngebotDto.ANGOBPICTUREURL;
                if (toAngob.CONFIGPICTUREURL != null && toAngob.CONFIGPICTUREURL.Length > 254)
                    toAngob.CONFIGPICTUREURL = toAngob.CONFIGPICTUREURL.Substring(0, 254);
                toAngob.LIEFERUNG = fromAngebotDto.ANGOBLIEFERUNG;
                toAngob.NOVA = fromAngebotDto.ANGOBNOVA;
                toAngob.NOVAP = fromAngebotDto.ANGOBNOVAP;
                
                toAngob.SERIE = fromAngebotDto.ANGOBSERIE;
                toAngob.FGNR = fromAngebotDto.ANGOBFGNR;
                toAngob.SPECIFICATION = fromAngebotDto.ANGOBSPECIFICATION;
                toAngob.TYP = fromAngebotDto.ANGOBTYP;
                if (fromAngebotDto.ANGOBTYP != null && fromAngebotDto.ANGOBTYP.Length > 60)
                    toAngob.TYP = fromAngebotDto.ANGOBTYP.Substring(0, 60);
                toAngob.SCHWACKE = fromAngebotDto.ANGOBSCHWACKE;
                toAngob.NOVAZUAB = fromAngebotDto.ANGOBNOVAZUAB;
                toAngob.NOVABRUTTO = fromAngebotDto.ANGOBNOVABRUTTO;
                toAngob.NOVABETRAG = fromAngebotDto.ANGOBNOVABETRAG;
                toAngob.SATZMEHRKM = fromAngebotDto.ANGOBSATZMEHRKM;
                toAngob.SATZMINDERKM = fromAngebotDto.ANGOBSATZMINDERKM;
                toAngob.OBJEKT = fromAngebotDto.ANGOBOBJEKT;
                toAngob.SATZMEHRKMBRUTTO = fromAngebotDto.ANGOBSATZMEHRKMBRUTTO;
                toAngob.SATZMINDERKMBRUTTO = fromAngebotDto.ANGOBSATZMINDERKMBRUTTO;
                toAngob.GRUND = fromAngebotDto.ANGOBGRUND;
                toAngob.GRUNDRABATTOP = fromAngebotDto.ANGOBGRUNDRABATTOP;
                toAngob.GRUNDRABATTO = fromAngebotDto.ANGOBGRUNDRABATTO;
                toAngob.GRUNDEXTERNBRUTTO = fromAngebotDto.ANGOBGRUNDEXTERNBRUTTO;
                toAngob.GRUNDEXTERNUST = fromAngebotDto.ANGOBGRUNDEXTERNUST;
                toAngob.GRUNDEXTERN = fromAngebotDto.ANGOBGRUNDEXTERN;
                toAngob.SONZUB = fromAngebotDto.ANGOBSONZUB;
                toAngob.SONZUBRABATTO = fromAngebotDto.ANGOBSONZUBRABATTO;
                toAngob.SONZUBRABATTOP = fromAngebotDto.ANGOBSONZUBRABATTOP;
                toAngob.SONZUBEXTERNBRUTTO = fromAngebotDto.ANGOBSONZUBEXTERNBRUTTO;
                toAngob.SONZUBEXTERNUST = fromAngebotDto.ANGOBSONZUBEXTERNUST;
                toAngob.SONZUBEXTERN = fromAngebotDto.ANGOBSONZUBEXTERN;
                toAngob.PAKETE = fromAngebotDto.ANGOBPAKETE;
                toAngob.PAKETERABATTOP = fromAngebotDto.ANGOBPAKETERABATTOP;
                toAngob.PAKETERABATTO = fromAngebotDto.ANGOBPAKETERABATTO;
                toAngob.PAKETEEXTERNBRUTTO = fromAngebotDto.ANGOBPAKETEEXTERNBRUTTO;
                toAngob.PAKETEEXTERNUST = fromAngebotDto.ANGOBPAKETEEXTERNUST;
                toAngob.PAKETEEXTERN = fromAngebotDto.ANGOBPAKETEEXTERN;
                toAngob.HERZUBRABATTOP = fromAngebotDto.ANGOBHERZUBRABATTOP;
                toAngob.HERZUBRABATTO = fromAngebotDto.ANGOBHERZUBRABATTO;
                toAngob.HERZUBEXTERNBRUTTO = fromAngebotDto.ANGOBHERZUBEXTERNBRUTTO;
                toAngob.HERZUBEXTERNUST = fromAngebotDto.ANGOBHERZUBEXTERNUST;
                toAngob.HERZUBEXTERN = fromAngebotDto.ANGOBHERZUBEXTERN;
                toAngob.ZUBEHOER = fromAngebotDto.ANGOBZUBEHOER;
                toAngob.ZUBEHOERBRUTTO = fromAngebotDto.ANGOBZUBEHOERBRUTTO;
                toAngob.ZUBEHOERRABATTOP = fromAngebotDto.ANGOBZUBEHOERRABATTOP;
                toAngob.ZUBEHOERRABATTO = fromAngebotDto.ANGOBZUBEHOERRABATTO;
                toAngob.ZUBEHOEREXTERNBRUTTO = fromAngebotDto.ANGOBZUBEHOEREXTERNBRUTTO;
                toAngob.ZUBEHOEREXTERNUST = fromAngebotDto.ANGOBZUBEHOEREXTERNUST;
                toAngob.ZUBEHOEREXTERN = fromAngebotDto.ANGOBZUBEHOEREXTERN;
                toAngob.AHKBRUTTO = fromAngebotDto.ANGOBAHKBRUTTO;
                toAngob.ERINKLMWST=fromAngebotDto.ANGOBERINKLMWST;
                toAngob.AHKEXTERNBRUTTO = fromAngebotDto.ANGOBAHKEXTERNBRUTTO;
                toAngob.AHKEXTERNUST = fromAngebotDto.ANGOBAHKEXTERNUST;
                toAngob.AHKEXTERN = fromAngebotDto.ANGOBAHKEXTERN;
                toAngob.AHKRABATTO = fromAngebotDto.ANGOBAHKRABATTO;
                toAngob.AHKRABATTOBRUTTO = fromAngebotDto.ANGOBAHKRABATTOBRUTTO;
                toAngob.AHKRABATTOP = fromAngebotDto.ANGOBAHKRABATTOP;
                toAngob.NOVAUST = fromAngebotDto.ANGOBNOVAUST;
                toAngob.GRUNDBRUTTO = fromAngebotDto.ANGOBGRUNDBRUTTO;
                toAngob.SONZUBBRUTTO = fromAngebotDto.ANGOBSONZUBBRUTTO;
                toAngob.PAKETEBRUTTO = fromAngebotDto.ANGOBPAKETEBRUTTO;
                toAngob.HERZUBBRUTTO = fromAngebotDto.ANGOBHERZUBBRUTTO;
                //HCEDEV new fields
                toAngob.UEBERFUEHRUNGBRUTTO = fromAngebotDto.ANGOBUEBERFUEHRUNGBRUTTO;
                toAngob.ZULASSUNGBRUTTO = fromAngebotDto.ANGOBZULASSUNGBRUTTO;
                
               // toAngob.UEBERFUEHRUNG = toAngob.UEBERFUEHRUNGBRUTTO / (1 + Ust / 100);
               // toAngob.ZULASSUNG = toAngob.ZULASSUNGBRUTTO / (1 + Ust / 100);
                toAngob.UEBERFUEHRUNGUST = toAngob.UEBERFUEHRUNGBRUTTO - toAngob.UEBERFUEHRUNG ;
                toAngob.ZULASSUNGUST = toAngob.ZULASSUNGBRUTTO - toAngob.ZULASSUNG ;

                toAngob.HERZUB = fromAngebotDto.ANGOBHERZUB;
                
                /*toAngob.GRUND = toAngob.GRUNDBRUTTO / (1 + Ust / 100);
                toAngob.SONZUB = toAngob.SONZUBBRUTTO / (1 + Ust / 100);
                toAngob.PAKETE = toAngob.PAKETEBRUTTO / (1 + Ust / 100);
                toAngob.HERZUB = toAngob.HERZUBBRUTTO / (1 + Ust / 100);
                toAngob.ZUBEHOER = toAngob.ZUBEHOERBRUTTO / (1 + Ust / 100);*/

                toAngob.NOVAZUABBRUTTO = fromAngebotDto.ANGOBNOVAZUABBRUTTO;
                toAngob.NOVAZUABUST = fromAngebotDto.ANGOBNOVAZUABUST;
                toAngob.INVENTAR = fromAngebotDto.ANGOBINVENTAR;
                //toAngob.RANG = fromAngebotDto.ANGOBORANG;
                toAngob.RANG = 1;
                toAngob.ABNAHMEKM = fromAngebotDto.ANGOBABNAHMEKM;
                toAngob.BGN = fromAngebotDto.ANGOBBGN;
                toAngob.SYSKGRUPPE = fromAngebotDto.ANGOBSYSKGRUPPE;
                toAngob.USGAAP = fromAngebotDto.ANGOBUSGAAP;
                toAngob.WAGENTYP = fromAngebotDto.ANGOBWAGENTYP;
                toAngob.VORKENNZEICHEN = fromAngebotDto.ANGOBVORKENNZEICHEN;
                toAngob.PAKETEEXTERNUST = fromAngebotDto.ANGOBPAKETEEXTERNUST;
                toAngob.GRUNDBRUTTO = fromAngebotDto.ANGOBGRUNDBRUTTO;
                toAngob.GRUNDEXKLN = fromAngebotDto.ANGOBGRUNDEXKLN;
                toAngob.PAKETEBRUTTO = fromAngebotDto.ANGOBPAKETEBRUTTO;
                toAngob.HERZUBBRUTTO = fromAngebotDto.ANGOBHERZUBBRUTTO;
                toAngob.HERZUBRABATTO = fromAngebotDto.ANGKALKSONZUBNACHLBRUTTO;
                toAngob.HERZUBRABATTO = fromAngebotDto.ANGOBHERZUBRABATTO;
                toAngob.HERZUBRABATTOP = fromAngebotDto.ANGOBHERZUBRABATTOP;
                toAngob.HERZUBEXTERNUST = fromAngebotDto.ANGOBHERZUBEXTERNUST;
                toAngob.RWCRV = fromAngebotDto.ANGKALKRWCRV;
                toAngob.SATZMEHRKMBRUTTO = fromAngebotDto.ANGOBSATZMEHRKMBRUTTO;
                toAngob.SATZMINDERKMBRUTTO = fromAngebotDto.ANGOBSATZMINDERKMBRUTTO;
                bool iscredit=updatePrices(Ust, fromAngebotDto, toAngob, toAngkalk, isNew, context, _SysPEROLE.Value);

                //HCE new fieldmapping
                //AHKBRUTTO contains currently the gui value for kaufpreis
                /*toOption.PDEC1501=fromAngebotDto.ANGOBAHKBRUTTO;
                toOption.PDEC1502 = fromAngebotDto.ANGOBAHK;
                
                
                toAngob.AHKBRUTTO = toOption.PDEC1501 - toAngob.AHKRABATTOBRUTTO;
                toAngob.AHK = toOption.PDEC1502 - toAngob.AHKRABATTO;
              */

                toAngkalk.AHKBRUTTO = toAngob.AHKBRUTTO;
                toAngkalk.AHK = toAngob.AHK;
                if(iscredit)
                {
                    toAngkalk.AHK = toAngkalk.AHKBRUTTO;
                }
                ANGOBAUSTAssembler ANGOBAUSTAssembler = new Service.ANGOBAUSTAssembler(_SysPEROLE);
                if ((fromAngebotDto.ANGOBAUST != null) && (fromAngebotDto.ANGOBAUST.Length > 0) && toAngobaust != null)
                {
                    for (int i = 0; i < toAngobaust.Length; i++)
                    {
                        toAngobaust[i] = ANGOBAUSTAssembler.ConvertToDomain(fromAngebotDto.ANGOBAUST[i]);
                    }
                }


            }
            catch (Exception e)
            {

                throw new Exception("Error in MyMapToEntities",e);
            }
        }


        /// <summary>
        /// called upon deliver and save, fills dto from ENTITIES
        /// </summary>
        /// <param name="fromAngebot"></param>
        /// <param name="fromAngkalk"></param>
        /// <param name="fromAngkalkfs"></param>
        /// <param name="fromAngob"></param>
        /// <param name="fromAngobini"></param>
        /// <param name="toAngebotDto"></param>
        /// <param name="fromIT"></param>
        /// <param name="fromANGOBAUST"></param>
        /// <param name="Ust"></param>
        private void MyMapToDto(ANGEBOT fromAngebot, ANGKALK fromAngkalk, ANGKALKFS fromAngkalkfs, ANGOB fromAngob, ANGOBINI fromAngobini, ANGEBOTDto toAngebotDto, ANGEBOTITDto fromIT, ANGOBAUST[] fromANGOBAUST, decimal Ust, ANGOBOPTION fromOption)
        {
            System.Collections.Generic.List<ANGOBAUSDto> TempAngobaustList = new System.Collections.Generic.List<ANGOBAUSDto>();
            ANGOBAUSTAssembler ANGOBAUSTAssembler = new Service.ANGOBAUSTAssembler(_SysPEROLE);

            //HCE new fieldmapping
            //toAngebotDto.ANGOBAHKBRUTTO = fromAngob.AHKBRUTTO + fromAngob.AHKRABATTOBRUTTO;
            //toAngebotDto.ANGOBAHK = fromAngob.AHK + fromAngob.AHKRABATTO;
           

            //Ids
            toAngebotDto.SYSID = fromAngebot.SYSID;
            toAngebotDto.SYSIT = fromAngebot.SYSIT;
            
            toAngebotDto.SYSKI = fromAngebot.SYSKI;
            toAngebotDto.EINZUG = fromAngebot.EINZUG;
            //HCE konstant
            toAngebotDto.SYSPRHGROUP = 0;
            toAngebotDto.SYSPRCHANNEL = 1;
            
            toAngebotDto.SYSKD = fromAngebot.SYSKD;
            toAngebotDto.SYSOBTYP = fromAngob.SYSOBTYP;
            toAngebotDto.SYSBERATADDB = fromAngebot.SYSBERATADDB;
            toAngebotDto.SYSANGEBOT = fromAngebot.SYSANGEBOT;

            toAngebotDto.SYSVORVT = fromAngebot.SYSVORVT;
            toAngebotDto.CONTRACTEXT = fromAngebot.CONTRACTEXT;
            toAngebotDto.CONTRACTTYPE = fromAngebot.CONTRACTTYPE;
            
            toAngebotDto.ANGKALKSYSKALK = fromAngkalk.SYSKALK;
            toAngebotDto.ANGOBSYSOB = fromAngob.SYSOB;
            toAngebotDto.SYSANGKALKFS = fromAngkalkfs.SYSANGKALKFS;
            toAngebotDto.ANGKALKSYSKALKTYP = fromAngkalk.SYSKALKTYP;
            toAngebotDto.ANGKALKERSTERATE = fromAngkalk.ERSTERATE;
            toAngebotDto.ANGOBPAKETEBRUTTOEXKLNOVA = fromAngob.PAKETBRUTTOEXKLN;
            toAngebotDto.ANGOBSONZUBBRUTTOEXKLNOVA = fromAngob.SONZUBBRUTTOEXKLN;
            toAngebotDto.ANGOBGRUNDBRUTTOEXKLNOVA = fromAngob.GRUNDBRUTTOEXKLN;
            toAngebotDto.ANGKALKBGINTERNBRUTTO = fromAngkalk.BGINTERNBRUTTO;
            toAngebotDto.ANGOBERINKLMWST = fromAngob.ERINKLMWST;
            toAngebotDto.ANGOBSONZUBRV = fromAngob.SONZUBRV;
            toAngebotDto.ANGKALKFSEXTRASPRICEUNIT = fromAngkalkfs.EXTRASPRICEUNIT;
            toAngebotDto.ANGKALKGEBUEHR_NACHLASS = fromAngkalk.GEBUEHRNACHLASS;

            toAngebotDto.HIST_ANGKALKFUELSYSFSTYP = fromAngkalkfs.FUELLIEFERANT;
            toAngebotDto.HIST_ANGOBINIMOTORTYP1 = fromAngkalkfs.FUELBEZEICHNUNG;


            toAngebotDto.ANGKALKFSKOSTENPERKILOMETER = fromAngkalkfs.MAINTPRICEKM;

            toAngebotDto.SYSVTTYP = fromAngebot.SYSVTTYP;
            toAngebotDto.SYSPRPRODUCT = fromAngebot.SYSPRPRODUCT;
            toAngebotDto.ANGOBSYSOBKAT = fromAngob.SYSOBKAT.GetValueOrDefault();
            toAngebotDto.SYSOBART = fromAngob.SYSOBART;
            toAngebotDto.SYSBRAND = _SysBrand;

            toAngebotDto.SYSITRSVVN = fromAngkalkfs.SYSITRSVVN;
            //Properties
            toAngebotDto.ANGEBOT1 = fromAngebot.ANGEBOT1;
            toAngebotDto.BEGINN = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromAngebot.BEGINN);
            toAngebotDto.ENDE = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromAngebot.ENDE);
            toAngebotDto.ERFASSUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromAngebot.ERFASSUNG);
            toAngebotDto.ZUSTAND = fromAngebot.ZUSTAND;
            toAngebotDto.ZUSTANDAM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromAngebot.ZUSTANDAM);
            toAngebotDto.ANGKALKBGEXTERN = fromAngkalk.BGEXTERN;
            toAngebotDto.ANGKALKBGEXTERNBRUTTO = fromAngkalk.BGEXTERNBRUTTO;
            toAngebotDto.ANGKALKBGEXTERNUST = fromAngkalk.BGEXTERNUST;
            toAngebotDto.ANGKALKDEPOT = fromAngkalk.DEPOT;
            toAngebotDto.ANGKALKDEPOTP = fromAngkalk.DEPOTP;

            toAngebotDto.ANGKALKBWFEHLER=fromAngkalk.BWFEHLER;
            toAngebotDto.ANGKALKOBJECTMETACALCTARGET=fromAngkalk.OBJECTMETACALCTARGET;
            //An/Abmelde-Service

            toAngebotDto.ANGKALKFSANABKENNZINKLFLAG = fromAngkalkfs.ANABKENNZINKLFLAG;
            toAngebotDto.ANGKALKFSANABMLDFLAG = fromAngkalkfs.ANABMLDFLAG;
            toAngebotDto.ANGKALKFSANABBRUTTO = fromAngkalkfs.ANABBRUTTO;
            toAngebotDto.ANGKALKFSANABMELDUNG = fromAngkalkfs.ANABMELDUNG;
            toAngebotDto.ANGKALKFSANABUST = fromAngkalkfs.ANABUST;
            toAngebotDto.ANGKALKFSANABBRUTTO = fromAngkalkfs.ANABBRUTTO;
            toAngebotDto.ANGKALKFSANABNACHLASS = fromAngkalkfs.ANABNACHLASS;
            toAngebotDto.ANGKALKFSANABGROSSUNITPRICE = fromAngkalkfs.ANABPRICEBRUTTO;

            toAngebotDto.ANGKALKFSEXTRASBRUTTO = fromAngkalkfs.EXTRASBRUTTO;
            toAngebotDto.ANGKALKFSEXTRASFLAG = fromAngkalkfs.EXTRASFLAG;
            toAngebotDto.ANGKALKFSEXTRASPRICE = fromAngkalkfs.EXTRASPRICE;
            toAngebotDto.ANGKALKFSEXTRASUST = fromAngkalkfs.EXTRASUST;
            toAngebotDto.ANGKALKFSFUELBRUTTO = fromAngkalkfs.FUELBRUTTO;
            toAngebotDto.ANGKALKFSFUELFLAG = fromAngkalkfs.FUELFLAG;
            toAngebotDto.ANGKALKFSFUELPRICE = fromAngkalkfs.FUELPRICE;
            toAngebotDto.ANGKALKFSFUELUST = fromAngkalkfs.FUELUST;
            toAngebotDto.ANGKALKFUELSYSFSTYP = fromAngkalkfs.FUELSYSFSTYP;
            toAngebotDto.FUELPRICEAVGBRUTTO = fromAngkalkfs.FUELPRICEAVGBRUTTO;
            toAngebotDto.ANGKALKFSVKFLAG = fromAngkalkfs.VKFLAG;
            toAngebotDto.ANGKALKFSHPFLAG = fromAngkalkfs.HPFLAG;
            toAngebotDto.ANGKALKFSRECHTSCHUTZFLAG = fromAngkalkfs.RECHTSCHUTZFLAG;
            toAngebotDto.ANGKALKFSINSASSENFLAG = fromAngkalkfs.INSASSENFLAG;
            toAngebotDto.ANGKALKFSRSVFLAG = fromAngkalkfs.RSVFLAG;
            //HCE TODO
            //toAngebotDto.ANGKALKFSGAPFLAG = fromAngkalkfs.INSASSENFLAG;

            toAngebotDto.ANGKALKFSMOTORVS = fromAngkalkfs.MOTORVS;

            toAngebotDto.ANGKALKFSMAINTENANCEBRUTTO = fromAngkalkfs.MAINTENANCEBRUTTO;
            toAngebotDto.ANGKALKFSMAINTENANCEFLAG = fromAngkalkfs.MAINTENANCEFLAG;
            toAngebotDto.ANGKALKFSMAINTENANCEUST = fromAngkalkfs.MAINTENANCEUST;
            toAngebotDto.ANGKALKFSMAINTFIXFLAG = fromAngkalkfs.MAINTFIXFLAG;
            toAngebotDto.ANGKALKFSMAINTENANCE = fromAngkalkfs.MAINTENANCE;
            toAngebotDto.ANGKALKFSMEHRKM = fromAngkalkfs.MEHRKM;
            toAngebotDto.ANGKALKFSMINDERKM = fromAngkalkfs.MINDERKM;
            toAngebotDto.ANGKALKFSREPCARCOUNT = fromAngkalkfs.REPCARCOUNT;
            toAngebotDto.ANGKALKFSREPCARFLAG = fromAngkalkfs.REPCARFLAG;
            toAngebotDto.ANGKALKFSREPCARPRICE = fromAngkalkfs.REPCARPRICE;
            toAngebotDto.ANGKALKFSREPCARRATE = fromAngkalkfs.REPCARRATE;
            toAngebotDto.ANGKALKFSREPCARRATEBRUTTO = fromAngkalkfs.REPCARRATEBRUTTO;
            toAngebotDto.ANGKALKFSREPCARRATEUST = fromAngkalkfs.REPCARRATEUST;
            toAngebotDto.ANGKALKFSRIMSCODEH = fromAngkalkfs.RIMSCODEH;
            toAngebotDto.ANGKALKFSRIMSCODEV = fromAngkalkfs.RIMSCODEV;
            toAngebotDto.ANGKALKFSRIMSCOUNTH = fromAngkalkfs.RIMSCOUNTH;
            toAngebotDto.ANGKALKFSRIMSCOUNTV = fromAngkalkfs.RIMSCOUNTV;
            toAngebotDto.ANGKALKFSRIMSPRICEH = fromAngkalkfs.RIMSPRICEH;
            toAngebotDto.ANGKALKFSRIMSPRICEV = fromAngkalkfs.RIMSPRICEV;
            toAngebotDto.ANGKALKFSSTIRESCODEH = fromAngkalkfs.STIRESCODEH;
            toAngebotDto.ANGKALKFSSTIRESCODEV = fromAngkalkfs.STIRESCODEV;
            toAngebotDto.ANGKALKFSSTIRESCOUNTH = fromAngkalkfs.STIRESCOUNTH;
            toAngebotDto.ANGKALKFSSTIRESCOUNTV = fromAngkalkfs.STIRESCOUNTV;
            toAngebotDto.ANGKALKFSSTIRESMODH = fromAngkalkfs.STIRESMODH;
            toAngebotDto.ANGKALKFSSTIRESMODV = fromAngkalkfs.STIRESMODV;
            toAngebotDto.ANGKALKFSSTIRESPRICE = fromAngkalkfs.STIRESPRICE;
            toAngebotDto.ANGKALKFSSTIRESPRICEH = fromAngkalkfs.STIRESPRICEH;
            toAngebotDto.ANGKALKFSSTIRESPRICEHBRUTTO = fromAngkalkfs.STIRESPRICEHBRUTTO;
            toAngebotDto.ANGKALKFSSTIRESPRICEHUST = fromAngkalkfs.STIRESPRICEHUST;
            toAngebotDto.ANGKALKFSSTIRESPRICEV = fromAngkalkfs.STIRESPRICEV;
            toAngebotDto.ANGKALKFSSTIRESPRICEVBRUTTO = fromAngkalkfs.STIRESPRICEVBRUTTO;
            toAngebotDto.ANGKALKFSSTIRESPRICEVUST = fromAngkalkfs.STIRESPRICEVUST;
            toAngebotDto.ANGKALKFSTIRESADDITION = fromAngkalkfs.TIRESADDITION;
            toAngebotDto.ANGKALKFSTIRESADDITIONBRUTTO = fromAngkalkfs.TIRESADDITIONBRUTTO;
            toAngebotDto.ANGKALKFSTIRESADDITIONUST = fromAngkalkfs.TIRESADDITIONUST;
            toAngebotDto.ANGKALKFSTIRESFIXFLAG = fromAngkalkfs.TIRESFIXFLAG;
            toAngebotDto.ANGKALKFSTIRESFLAG = fromAngkalkfs.TIRESFLAG;
            toAngebotDto.ANGKALKFSTIRESINCLFLAG = fromAngkalkfs.TIRESINCLFLAG;


            toAngebotDto.TRADEONOWNACCOUNT = fromAngebot.TRADEONOWNACCOUNT;

            toAngebotDto.ANGKALKFSWTIRESCODEH = fromAngkalkfs.WTIRESCODEH;
            toAngebotDto.ANGKALKFSWTIRESCODEV = fromAngkalkfs.WTIRESCODEV;
            toAngebotDto.ANGKALKFSWTIRESCOUNTH = fromAngkalkfs.WTIRESCOUNTH;
            toAngebotDto.ANGKALKFSWTIRESCOUNTV = fromAngkalkfs.WTIRESCOUNTV;
            toAngebotDto.ANGKALKFSWTIRESMODH = fromAngkalkfs.WTIRESMODH;
            toAngebotDto.ANGKALKFSWTIRESMODV = fromAngkalkfs.WTIRESMODV;
            toAngebotDto.ANGKALKFSWTIRESPRICEH = fromAngkalkfs.WTIRESPRICEH;
            toAngebotDto.ANGKALKFSWTIRESPRICEHBRUTTO = fromAngkalkfs.WTIRESPRICEHBRUTTO;
            toAngebotDto.ANGKALKFSWTIRESPRICEHUST = fromAngkalkfs.WTIRESPRICEHUST;
            toAngebotDto.ANGKALKFSWTIRESPRICEV = fromAngkalkfs.WTIRESPRICEV;
            toAngebotDto.ANGKALKFSWTIRESPRICEVBRUTTO = fromAngkalkfs.WTIRESPRICEVBRUTTO;
            toAngebotDto.ANGKALKFSWTIRESPRICEVUST = fromAngkalkfs.WTIRESPRICEVUST;
            toAngebotDto.ANGKALKGEBUEHR = fromAngkalk.GEBUEHR;
            toAngebotDto.ANGKALKGEBUEHRBRUTTO = fromAngkalk.GEBUEHRBRUTTO;
            toAngebotDto.ANGOBJAHRESKM = fromAngob.JAHRESKM;
            toAngebotDto.ANGKALKLZ = fromAngkalk.LZ;
            toAngebotDto.ANGKALKMITFINBRUTTO = fromAngkalk.MITFINBRUTTO;
            toAngebotDto.ANGKALKMITFINUST = fromAngkalk.MITFINUST;
            toAngebotDto.ANGKALKPAKRABO = fromAngkalk.PAKRABO;
            toAngebotDto.ANGKALKPPY = fromAngkalk.PPY;
            toAngebotDto.ANGKALKRABATTO = fromAngkalk.RABATTO;
            toAngebotDto.ANGKALKRABATTOP = fromAngkalk.RABATTOP;
            toAngebotDto.ANGKALKRATE = fromAngkalk.RATE;
            toAngebotDto.ANGKALKRATEBRUTTO = fromAngkalk.RATEBRUTTO;
            toAngebotDto.ANGKALKRATEUST = fromAngkalk.RATEUST;

            toAngebotDto.ANGKALKRATEGESAMT = fromAngkalk.RATEGESAMT;
            toAngebotDto.ANGKALKRATEGESAMTBRUTTO = fromAngkalk.RATEGESAMTBRUTTO;
            toAngebotDto.ANGKALKRATEGESAMTUST = fromAngkalk.RATEGESAMTUST;

            toAngebotDto.ANGKALKRGGEBUEHR = fromAngkalk.RGGEBUEHR;
            toAngebotDto.ANGKALKRGGFREI = fromAngkalk.RGGFREI;
            toAngebotDto.ANGKALKRGGVERR = fromAngkalk.RGGVERR;
            toAngebotDto.ANGOBRWBASE = fromAngob.RWBASE;
            toAngebotDto.ANGKALKRWBASE = fromAngkalk.RWBASE;
            toAngebotDto.ANGKALKRWBASEBRUTTO = fromAngkalk.RWBASEBRUTTO;
            toAngebotDto.ANGKALKRWBASEBRUTTOP = fromAngkalk.RWBASEBRUTTOP;
            toAngebotDto.ANGKALKRWBASEUST = fromAngkalk.RWBASEUST;
            toAngebotDto.ANGOBRWCRV = fromAngob.RWCRV;
            toAngebotDto.ANGKALKRWCRV = fromAngob.RWCRV;
            toAngebotDto.ANGKALKKREDITBETRAG = fromAngkalk.GESAMTKREDIT;
            toAngebotDto.ANGKALKRWCRVBRUTTO = fromAngkalk.RWCRVBRUTTO;
            toAngebotDto.ANGKALKRWCRVBRUTTOP = fromAngkalk.RWCRVBRUTTOP;

            toAngebotDto.ANGKALKFSFUELNACHLASS = fromAngkalkfs.FUELNACHLASS;
            toAngebotDto.ANGKALKFSMAINTNACHLASS = fromAngkalkfs.MAINTNACHLASS;
            toAngebotDto.ANGKALKFSTIRESNACHLASS = fromAngkalkfs.TIRESNACHLASS;
            toAngebotDto.ANGKALKFSREPCARNACHLASS = fromAngkalkfs.REPCARNACHLASS;
            toAngebotDto.ANGKALKFSEXTRASNACHLASS = fromAngkalkfs.EXTRASNACHLASS;
            toAngebotDto.RESTKAUFPREIS = fromAngkalk.RESTKAUFPREIS;
            toAngebotDto.ANGKALKRWCRVUST = fromAngkalk.RWCRVUST;
            toAngebotDto.ANGKALKRWKALK = fromAngkalk.RWKALK;
            toAngebotDto.ANGKALKRWKALKBRUTTO = fromAngkalk.RWKALKBRUTTO;
            toAngebotDto.ANGKALKRWKALKBRUTTOP = fromAngkalk.RWKALKBRUTTOP;
            toAngebotDto.ANGKALKRWKALKBRUTTO_DEFAULT = fromAngkalk.RWKALKBRUTTODEF;
            toAngebotDto.ANGKALKRWKALKBRUTTOP_DEFAULT = fromAngkalk.RWKALKBRUTTOPDEF;


            toAngebotDto.ANGKALKRWKALKBRUTTOORG = fromAngkalk.RWKALKBRUTTOORG;
            toAngebotDto.ANGKALKRWKALKBRUTTOPORG = fromAngkalk.RWKALKBRUTTOPORG;

            toAngebotDto.ANGKALKRWKALK_DEFAULT = fromAngkalk.RWKALKDEF;
            toAngebotDto.ANGKALKRWKALKUST_DEFAULT = fromAngkalk.RWKALKUSTDEF;

            toAngebotDto.ANGKALKRWKALKUST = fromAngkalk.RWKALKUST;
            toAngebotDto.ANGKALKSZ = fromAngkalk.SZ;
            toAngebotDto.ANGKALKSZBRUTTO = fromAngkalk.SZBRUTTO;
            toAngebotDto.ANGKALKSZBRUTTOP = fromAngkalk.SZBRUTTOP;
            toAngebotDto.ANGKALKSZP = fromAngkalk.SZP;
            toAngebotDto.ANGKALKSZUST = fromAngkalk.SZUST;
            toAngebotDto.ANGKALKZUBEHOERBRUTTO = fromAngkalk.ZUBEHOERBRUTTO;
            toAngebotDto.ANGKALKZUBEHOERNETTO = fromAngkalk.ZUBEHOERNETTO;
            toAngebotDto.ANGKALKZUBEHOEROR = fromAngkalk.ZUBEHOEROR;
            toAngebotDto.ANGKALKZUBEHOERORP = fromAngkalk.ZUBEHOERORP;
            toAngebotDto.ANGOBAUTOMATIK = fromAngob.AUTOMATIK;
            toAngebotDto.ANGOBFABRIKAT = fromAngob.FABRIKAT;
            toAngebotDto.ANGOBFARBEA = fromAngob.FARBEA;

            toAngebotDto.ANGOBFZART = fromAngob.FZART;
            toAngebotDto.ANGOBFZNR = fromAngob.FZNR;
            toAngebotDto.ANGOBHERSTELLER = fromAngob.HERSTELLER;

            toAngebotDto.ANGOBINIACTUATION = 0;
            if (fromAngobini.ACTUATION == 3)//Hybrid
                toAngebotDto.ANGOBINIACTUATION = 1;

            toAngebotDto.ANGOBCONFIGID = fromAngob.CONFIGID;
            toAngebotDto.ANGOBPOLSTERCODE = fromAngob.POLSTERCODE;
            toAngebotDto.ANGOBPOLSTERTEXT = fromAngob.POLSTERTEXT;
            if (fromAngob.CONFIGSOURCE != null)
            {
                toAngebotDto.ANGOBCONFIGSOURCE =
                    (Cic.OpenLease.ServiceAccess.OfferTypeConstants)
                    Enum.Parse(typeof(OfferTypeConstants), fromAngob.CONFIGSOURCE, true);
            }
            if (fromAngkalk.ZUSTAND != null)
            {
                toAngebotDto.KALKULATIONSOURCE =
                    (ServiceAccess.DdOl.CalculationDto.CalculationSources)
                    Enum.Parse(typeof(ServiceAccess.DdOl.CalculationDto.CalculationSources), fromAngkalk.ZUSTAND, true);
            }

            toAngebotDto.ANGOBPICTUREURL = fromAngob.CONFIGPICTUREURL;

            toAngebotDto.ANGOBBAUJAHR = fromAngob.BAUJAHR;
            //TODO WB 0 WB, Check why is is angob, not in angobini

            toAngebotDto.ANGOBKW = fromAngobini.KW;
            toAngebotDto.ANGOBINIKW = fromAngobini.KW;
            toAngebotDto.ANGOBCCM = fromAngob.CCM;

            toAngebotDto.ANGOBINICO2 = fromAngobini.CO2;
            toAngebotDto.ANGOBINIKMSTAND = fromAngobini.KMSTAND;
            toAngebotDto.ANGOBININOX = fromAngobini.NOX;
            toAngebotDto.ANGOBINIPARTICLES = fromAngobini.PARTICLES;
            toAngebotDto.ANGOBINIVERBRAUCH_D = fromAngobini.VERBRAUCH_D;
            toAngebotDto.ANGOBINIVORBESITZER = fromAngobini.VORBESITZER;
            toAngebotDto.ANGOBKMTOLERANZ = fromAngob.KMTOLERANZ;
            toAngebotDto.ABZUGNOVAAUFSCHLAG = fromAngob.AHKEXTERNNOVAZU;

            //CO2 Reifen - default values
            toAngebotDto.ANGOBNOVAPDEF = fromAngob.NOVAPDEF;
            if (toAngebotDto.ANGOBNOVAPDEF == 0) toAngebotDto.ANGOBNOVAPDEF = fromAngob.NOVAP;//old offers!
            toAngebotDto.ANGOBINICO2DEF = fromAngobini.CO2DEF;
            if (toAngebotDto.ANGOBINICO2DEF == 0) toAngebotDto.ANGOBINICO2DEF = fromAngobini.CO2;//old offers!
            toAngebotDto.ANGOBININOXDEF = fromAngobini.NOXDEF;
            if (toAngebotDto.ANGOBININOXDEF == 0) toAngebotDto.ANGOBININOXDEF = fromAngobini.NOX;//old offers!
            toAngebotDto.ANGOBINIVERBRAUCH_DDEF = fromAngobini.VERBRAUCH_DDEF;
            if (toAngebotDto.ANGOBINIVERBRAUCH_DDEF == 0) toAngebotDto.ANGOBINIVERBRAUCH_DDEF = fromAngobini.VERBRAUCH_D;//old offers!
            toAngebotDto.ANGOBINIPARTICLESDEF = fromAngobini.PARTICLESDEF;
            if (toAngebotDto.ANGOBINIPARTICLESDEF == 0) toAngebotDto.ANGOBINIPARTICLESDEF = fromAngobini.PARTICLES;//old offers!
            toAngebotDto.ANGOBINIACTUATIONDEF = fromAngobini.ACTUATIONDEF;
            if (!fromAngobini.ACTUATIONDEF.HasValue) toAngebotDto.ANGOBINIACTUATIONDEF = fromAngobini.ACTUATION;//old offers!

            toAngebotDto.ANGOBLIEFERUNG = fromAngob.LIEFERUNG;
            toAngebotDto.ANGOBNOVA = fromAngob.NOVA;
            toAngebotDto.ANGOBNOVAP = fromAngob.NOVAP;
            toAngebotDto.ANGOBSERIE = fromAngob.SERIE;
            toAngebotDto.ANGOBFGNR = fromAngob.FGNR;
            toAngebotDto.ANGOBSPECIFICATION = fromAngob.SPECIFICATION;
            toAngebotDto.ANGOBTYP = fromAngob.TYP;
            toAngebotDto.ITHSNR = fromIT.HSNR;
            toAngebotDto.ITNAME = fromIT.NAME;
            toAngebotDto.ITORT = fromIT.ORT;
            toAngebotDto.ITPLZ = fromIT.PLZ;
            toAngebotDto.ITSTRASSE = fromIT.STRASSE;
            toAngebotDto.ITVORNAME = fromIT.VORNAME;

            toAngebotDto.OBJEKTVT = fromAngebot.OBJEKTVT;
            toAngebotDto.ANGKALKZINS = fromAngkalk.ZINS;
            toAngebotDto.ANGKALKZINS_DEFAULT = fromAngkalk.ZINSDEF;
            toAngebotDto.ANGKALKZINSEFF = fromAngkalk.ZINSEFF;
            toAngebotDto.ANGKALKZINSEFF_DEFAULT = fromAngkalk.ZINSEFFDEF;
            toAngebotDto.ANGOBSCHWACKE = fromAngob.SCHWACKE;
            toAngebotDto.ANGOBNOVAZUAB = fromAngob.NOVAZUAB;
            toAngebotDto.ANGOBNOVABRUTTO = fromAngob.NOVABRUTTO;
            toAngebotDto.ANGOBNOVABETRAG = fromAngob.NOVABETRAG;
            toAngebotDto.ANGKALKGEBUEHRUST = fromAngkalk.GEBUEHRUST;
            toAngebotDto.ANGOBSATZMEHRKM = fromAngob.SATZMEHRKM;
            toAngebotDto.ANGOBSATZMINDERKM = fromAngob.SATZMINDERKM;
            toAngebotDto.DATANGEBOT = fromAngebot.DATANGEBOT;
            toAngebotDto.ANGOBOBJEKT = fromAngob.OBJEKT;
            toAngebotDto.SYSVART = fromAngebot.SYSVART;

            toAngebotDto.RATE = fromAngebot.RATE;
            toAngebotDto.ZINS = fromAngebot.ZINS;
            toAngebotDto.ANGKALKBGINTERN = fromAngkalk.BGINTERN;
            toAngebotDto.ANGOBINIERSTZUL = fromAngobini.ERSTZUL;
            toAngebotDto.ANGOBININOVA_P = fromAngobini.NOVA_P;
            toAngebotDto.ANGOBGRUND = fromAngob.GRUND;
            toAngebotDto.ANGOBGRUNDEXKLN = fromAngob.GRUNDEXKLN;
            toAngebotDto.ANGOBGRUNDRABATTOP = fromAngob.GRUNDRABATTOP;
            toAngebotDto.ANGOBGRUNDRABATTO = fromAngob.GRUNDRABATTO;
            toAngebotDto.ANGOBGRUNDEXTERNBRUTTO = fromAngob.GRUNDEXTERNBRUTTO;
            toAngebotDto.ANGOBGRUNDEXTERNUST = fromAngob.GRUNDEXTERNUST;
            toAngebotDto.ANGOBGRUNDEXTERN = fromAngob.GRUNDEXTERN;
            toAngebotDto.ANGOBSONZUB = fromAngob.SONZUB;
            toAngebotDto.ANGOBSONZUBBRUTTO = fromAngob.SONZUBBRUTTO;
            toAngebotDto.ANGOBSONZUBRABATTO = fromAngob.SONZUBRABATTO;
            toAngebotDto.ANGOBSONZUBRABATTOP = fromAngob.SONZUBRABATTOP;
            toAngebotDto.ANGOBSONZUBEXTERNBRUTTO = fromAngob.SONZUBEXTERNBRUTTO;
            toAngebotDto.ANGOBSONZUBEXTERNUST = fromAngob.SONZUBEXTERNUST;
            toAngebotDto.ANGOBSONZUBEXTERN = fromAngob.SONZUBEXTERN;
            toAngebotDto.ANGOBPAKETE = fromAngob.PAKETE;
            toAngebotDto.ANGOBPAKETERABATTOP = fromAngob.PAKETERABATTOP;
            toAngebotDto.ANGOBPAKETERABATTO = fromAngob.PAKETERABATTO;
            toAngebotDto.ANGOBPAKETEEXTERNBRUTTO = fromAngob.PAKETEEXTERNBRUTTO;
            toAngebotDto.ANGOBPAKETEEXTERNUST = fromAngob.PAKETEEXTERNUST;
            toAngebotDto.ANGOBPAKETEEXTERN = fromAngob.PAKETEEXTERN;
            toAngebotDto.ANGOBHERZUBRABATTOP = fromAngob.HERZUBRABATTOP;
            toAngebotDto.ANGKALKHERZUBRABOP = fromAngob.HERZUBRABATTOP;
            toAngebotDto.ANGKALKHERZUBRABO = fromAngob.HERZUBRABATTO;
            toAngebotDto.ANGOBHERZUBRABATTO = fromAngob.HERZUBRABATTO;
            toAngebotDto.ANGOBHERZUBEXTERNBRUTTO = fromAngob.HERZUBEXTERNBRUTTO;
            toAngebotDto.ANGOBHERZUBEXTERNUST = fromAngob.HERZUBEXTERNUST;
            toAngebotDto.ANGKALKHERZUBUST = fromAngob.HERZUBEXTERNUST;
            toAngebotDto.ANGOBHERZUBEXTERN = fromAngob.HERZUBEXTERN;
            toAngebotDto.ANGOBZUBEHOER = fromAngob.ZUBEHOER;
            toAngebotDto.ANGOBZUBEHOERBRUTTO = fromAngob.ZUBEHOERBRUTTO;
            toAngebotDto.ANGOBZUBEHOERRABATTOP = fromAngob.ZUBEHOERRABATTOP;
            toAngebotDto.ANGOBZUBEHOERRABATTO = fromAngob.ZUBEHOERRABATTO;
            toAngebotDto.ANGOBZUBEHOEREXTERNBRUTTO = fromAngob.ZUBEHOEREXTERNBRUTTO;
            toAngebotDto.ANGOBZUBEHOEREXTERNUST = fromAngob.ZUBEHOEREXTERNUST;
            toAngebotDto.ANGOBZUBEHOEREXTERN = fromAngob.ZUBEHOEREXTERN;
            toAngebotDto.ANGOBAHKBRUTTO = fromAngob.AHKBRUTTO;
            toAngebotDto.ANGOBAHK = fromAngob.AHK;

            toAngebotDto.ANGOBAHKEXTERNBRUTTO = fromAngob.AHKEXTERNBRUTTO;
            toAngebotDto.ANGOBAHKEXTERNUST = fromAngob.AHKEXTERNUST;
            toAngebotDto.ANGOBAHKEXTERN = fromAngob.AHKEXTERN;
            toAngebotDto.ANGOBAHKRABATTO = fromAngob.AHKRABATTO;
            toAngebotDto.ANGOBAHKRABATTOBRUTTO = fromAngob.AHKRABATTOBRUTTO;
            toAngebotDto.ANGOBAHKRABATTOP = fromAngob.AHKRABATTOP;

            toAngebotDto.SYSITRSVVN = fromAngkalkfs.SYSITRSVVN;
            toAngebotDto.VART = fromAngebot.VART;
            toAngebotDto.ANGKALKMITFIN = fromAngkalk.MITFIN;
            toAngebotDto.ANGKALKZINSTYP = (int?)fromAngkalk.SYSINTTYPE;
            toAngebotDto.ANGKALKFSSTIRESPRICEBRUTTO = fromAngkalkfs.STIRESPRICEBRUTTO;
            toAngebotDto.ANGKALKFSSTIRESPRICEUST = fromAngkalkfs.STIRESPRICEUST;
            toAngebotDto.ANGKALKREFIZINS1 = fromAngkalk.REFIZINS1;
            toAngebotDto.ANGKALKGEBUEHRINTERNBRUTTO = fromAngkalk.GEBUEHRINTERNBRUTTO;
            toAngebotDto.ANGKALKVERRECHNUNG = fromAngkalk.VERRECHNUNG;
            toAngebotDto.ANGKALKFSTIRESSETS = fromAngkalkfs.TIRESSETS;
            toAngebotDto.ANGKALKFSRIMSMODV = fromAngkalkfs.RIMSMODV;
            toAngebotDto.ANGKALKFSRIMSMODH = fromAngkalkfs.RIMSMODH;
            toAngebotDto.ANGKALKFSRIMSPRICEVUST = fromAngkalkfs.RIMSPRICEVUST;
            toAngebotDto.ANGKALKFSRIMSPRICEVBRUTTO = fromAngkalkfs.RIMSPRICEVBRUTTO;
            toAngebotDto.ANGKALKFSRIMSPRICEHUST = fromAngkalkfs.RIMSPRICEHUST;
            toAngebotDto.ANGKALKFSRIMSPRICEHBRUTTO = fromAngkalkfs.RIMSPRICEHBRUTTO;


            toAngebotDto.ANGOBINIMOTORTYP = "UNDEFINED";
            if (fromAngkalkfs.FUELBEZEICHNUNG != null && fromAngkalkfs.FUELBEZEICHNUNG.Length > 0)
            {
                toAngebotDto.ANGOBINIMOTORTYP = fromAngkalkfs.FUELBEZEICHNUNG;
                if ("UNLEADED_PETROL_AND_ETHANOL".Equals(fromAngkalkfs.FUELBEZEICHNUNG.Trim()))
                {
                    if (fromAngobini.ACTUATIONDEF.HasValue && fromAngobini.ACTUATIONDEF.Value == 0)
                    {
                        toAngebotDto.ANGOBINIMOTORTYP = "UNLEADED_PETROL";
                    }
                }
            }
            else
            {
                //Attention! unleaded_petrol and unleaded_petrol_and_ethanol will both be saved as 14 for motorfuel! so reading is not correctly possible!
                if ("14".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "UNLEADED_PETROL_AND_ETHANOL";
                else if ("3".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "DIESEL";
                else if ("14".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "UNLEADED_PETROL";
                else if ("8".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "PETROL";
                else if ("10".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "GAS";
                else if ("4".Equals(fromAngobini.MOTORFUEL))
                    toAngebotDto.ANGOBINIMOTORTYP = "ELECTRICITY";
            }
            toAngebotDto.SYSMART = fromAngobini.SYSPRMART.GetValueOrDefault();
            toAngebotDto.ANGOBNOVAUST = fromAngob.NOVAUST;
            toAngebotDto.ANGOBGRUNDBRUTTO = fromAngob.GRUNDBRUTTO;
            toAngebotDto.ANGOBPAKETEBRUTTO = fromAngob.PAKETEBRUTTO;
            toAngebotDto.ANGOBHERZUBBRUTTO = fromAngob.HERZUBBRUTTO;
            toAngebotDto.ANGOBHERZUB = fromAngob.HERZUB;

            //HCEDEV new fields
            toAngebotDto.ANGOBUEBERFUEHRUNGBRUTTO = fromAngob.UEBERFUEHRUNGBRUTTO;
            toAngebotDto.ANGOBZULASSUNGBRUTTO = fromAngob.ZULASSUNGBRUTTO;

            toAngebotDto.ANGOBNOVAZUABBRUTTO = fromAngob.NOVAZUABBRUTTO;
            toAngebotDto.ANGOBNOVAZUABUST = fromAngob.NOVAZUABUST;
            toAngebotDto.ANGOBINVENTAR = fromAngob.INVENTAR;
            toAngebotDto.ANGOBORANG = fromAngob.RANG;
            toAngebotDto.ANGOBABNAHMEKM = fromAngob.ABNAHMEKM;
            toAngebotDto.ANGOBBGN = fromAngob.BGN;
            toAngebotDto.ANGOBSYSKGRUPPE = fromAngob.SYSKGRUPPE;
            toAngebotDto.ANGOBUSGAAP = fromAngob.USGAAP;

            //Extended properties
            toAngebotDto.ExtTitle = "";// fromAngebot.ExtTitle;

            if ((fromANGOBAUST != null) && (fromANGOBAUST.Length > 0))
            {
                foreach (ANGOBAUST ANGOBAUSTLoop in fromANGOBAUST)
                {
                    ANGOBAUSDto dto = ANGOBAUSTAssembler.ConvertToDto(ANGOBAUSTLoop);

                    TempAngobaustList.Add(dto);
                    if (dto.SNR.Equals("Freitext"))
                    {
                        toAngebotDto.ANGOBSONZUBTEXT = dto.FREITEXT;
                        toAngebotDto.ANGOBSONZUBUSER = dto.BETRAG;
                        toAngebotDto.ANGOBSONZUBDEFAULT = dto.BETRAG2;
                    }
                    if (dto.SNR.Equals("Preiskarte"))
                    {
                        toAngebotDto.WFMMEMOANGEBOTSPREIS = dto.BETRAG2;
                        toAngebotDto.WFMMEMOSTATTPREIS = dto.BETRAG;
                        toAngebotDto.WFMMEMOTEXT = dto.FREITEXT;
                    }

                }
            }

            toAngebotDto.ANGOBAUST = TempAngobaustList.ToArray();


            toAngebotDto.ANGKALKFSSTIRESTEXTH = fromAngkalkfs.STIRESTEXTH;
            toAngebotDto.ANGKALKFSSTIRESTEXTV = fromAngkalkfs.STIRESTEXTV;
            toAngebotDto.ANGKALKFSWTIRESTEXTH = fromAngkalkfs.WTIRESTEXTH;
            toAngebotDto.ANGKALKFSWTIRESTEXTV = fromAngkalkfs.WTIRESTEXTV;

            toAngebotDto.ANGKALKFSMEHRKMBRUTTO = fromAngkalkfs.MEHRKMBRUTTO;
            toAngebotDto.ANGKALKFSMEHRKMUST = fromAngkalkfs.MEHRKMUST;
            toAngebotDto.ANGKALKFSMINDERKMBRUTTO = fromAngkalkfs.MINDERKMBRUTTO;
            toAngebotDto.ANGKALKFSMINDERKMUST = fromAngkalkfs.MINDERKMUST;

            toAngebotDto.ANGKALKGESAMTBRUTTO = fromAngkalk.GESAMTBRUTTO;
            toAngebotDto.ANGKALKGESAMTNETTO = fromAngkalk.GESAMTNETTO;
            toAngebotDto.ANGKALKGESAMTUST = fromAngkalk.GESAMTUST;
            toAngebotDto.ANGKALKGESAMTKOSTEN = fromAngkalk.GESAMTKOSTEN;
            toAngebotDto.ANGKALKGESAMTKOSTENBRUTTO = fromAngkalk.GESAMTKOSTENBRUTTO;
            toAngebotDto.ANGKALKGESAMTKOSTENUST = fromAngkalk.GESAMTKOSTENUST;

            toAngebotDto.ANGOBWAGENTYP = fromAngob.WAGENTYP;
            toAngebotDto.ANGOBVORKENNZEICHEN = fromAngob.VORKENNZEICHEN;
            toAngebotDto.SPECIALCALCSTATUS = fromAngebot.SPECIALCALCSTATUS == null ? 0 : fromAngebot.SPECIALCALCSTATUS;


            if (fromAngebot.SPECIALCALCCOUNT != null)
                toAngebotDto.SPECIALCALCCOUNT = fromAngebot.SPECIALCALCCOUNT;
            else
                toAngebotDto.SPECIALCALCCOUNT = 0;

            toAngebotDto.SPECIALCALCDATE = fromAngebot.SPECIALCALCDATE;

            if (fromAngebot.SPECIALCALCSYSWFUSER.HasValue)
                toAngebotDto.SPECIALCALCSYSWFUSER = (long)fromAngebot.SPECIALCALCSYSWFUSER;

            AngebotDtoHelper.UpdateAngebotSpecialCalcStatus(toAngebotDto);

            
            toAngebotDto.ANGKALKPAKETEUST = fromAngob.PAKETEEXTERNUST;
            toAngebotDto.ANGOBPAKETEEXTERNUST = fromAngob.PAKETEEXTERNUST;
            toAngebotDto.ANGKALKSONZUBNACHLBRUTTO = fromAngob.HERZUBRABATTO;
            toAngebotDto.ANGKALKGRUNDNACHLBRUTTO = fromAngob.GRUNDBRUTTO;
            toAngebotDto.ANGKALKGRUNDNETTO = fromAngkalk.GRUND;
            toAngebotDto.ANGKALKPAKETENACHLBRUTTO = fromAngob.PAKETEBRUTTO;
            toAngebotDto.ANGOBPAKRABOP = fromAngkalk.PAKRABOP;
            toAngebotDto.ANGKALKHERZUBNACHLBRUTTO = fromAngob.HERZUBBRUTTO;
            toAngebotDto.ANGKALKHERZUBNETTO = fromAngob.HERZUB;
            toAngebotDto.ANGKALKHERZUBRABO = fromAngob.HERZUBRABATTO;
            toAngebotDto.ANGKALKHERZUBRABOP = fromAngob.HERZUBRABATTOP;
            toAngebotDto.ANGKALKHERZUBUST = fromAngob.HERZUBEXTERNUST;
            toAngebotDto.ANGKALKPAKETENETTO = fromAngkalk.PAKETE;
            toAngebotDto.ANGKALKZINS1 = fromAngkalk.ZINS1;
            toAngebotDto.ANGKALKZINS2 = fromAngkalk.ZINS2;

            toAngebotDto.ANGKALKZINSBASIS = fromAngkalk.BASISZINS;
            toAngebotDto.ANGKALKZINSAKTION = fromAngkalk.ZINS3;

            toAngebotDto.ANGKALKSONZUBNETTO = fromAngkalk.SONZUB;

            toAngebotDto.RWCRV = fromAngob.RWCRV;

            toAngebotDto.ANGOBSATZMEHRKMBRUTTO = fromAngob.SATZMEHRKMBRUTTO;
            toAngebotDto.ANGOBSATZMINDERKMBRUTTO = fromAngob.SATZMINDERKMBRUTTO;
            toAngebotDto.ANGOBKW = fromAngob.KW;
            toAngebotDto.GUELTIGBIS = fromAngebot.GUELTIGBIS;

            if (!(toAngebotDto.ANGKALKSYSKALKTYP == 49 || toAngebotDto.ANGKALKSYSKALKTYP == 52 || toAngebotDto.ANGKALKSYSKALKTYP == 40 || toAngebotDto.ANGKALKSYSKALKTYP == 42))
            {
                toAngebotDto.ANGOBSATZMEHRKMBRUTTO = 0;
                toAngebotDto.ANGOBSATZMINDERKMBRUTTO = 0;
                toAngebotDto.ANGOBSATZMINDERKM = 0;
                toAngebotDto.ANGOBSATZMEHRKM = 0;
                toAngebotDto.ANGOBKMTOLERANZ = 0;
            }



           

        }

        /*private void MyCalculate(ANGEBOTDto angebotDto, ANGKALK angkalk, ANGOB angob, DdOlExtended context)
        {
            PRHGROUP PrHGroup;
            // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
            PrHGroup = PRHGROUPHelper.DeliverPrHGroupList(context, (long)_SysPEROLE, (long)_SysBrand).FirstOrDefault<PRHGROUP>();

            //Calculate ZINS
            angkalk.ZINS = ZinsHelper.DeliverZins(angebotDto.SYSPRPRODUCT.GetValueOrDefault(), (long)angebotDto.ANGKALKLZ.GetValueOrDefault(), angebotDto.ANGOBGRUNDBRUTTO.GetValueOrDefault(), angebotDto.SYSOBTYP.GetValueOrDefault(), angebotDto.SYSOBART.GetValueOrDefault(), angebotDto.SYSPRKGROUP.GetValueOrDefault(), PrHGroup.SYSPRHGROUP, _SysBrand.GetValueOrDefault(), _SysPEROLE.GetValueOrDefault(), angebotDto.ANGKALKZINSTYP.GetValueOrDefault());
            angkalk.BASISZINS = ZinsHelper.DeliverZinsBasis(angebotDto.SYSPRPRODUCT.GetValueOrDefault(), (long)angebotDto.ANGKALKLZ.GetValueOrDefault(), (long)angebotDto.ANGOBGRUNDBRUTTO.GetValueOrDefault());

            //Calculate Gesamt
            angkalk.GESAMTBRUTTO = (angebotDto.ANGKALKLZ.GetValueOrDefault() * angebotDto.ANGKALKRATEBRUTTO.GetValueOrDefault()) + angebotDto.ANGKALKSZBRUTTO.GetValueOrDefault() + angebotDto.ANGKALKRWKALKBRUTTO.GetValueOrDefault() + angebotDto.ANGKALKGEBUEHRBRUTTO.GetValueOrDefault();
            angkalk.GESAMT = (angebotDto.ANGKALKLZ.GetValueOrDefault() * angebotDto.ANGKALKRATE.GetValueOrDefault()) + angebotDto.ANGKALKSZ.GetValueOrDefault() + angebotDto.ANGKALKRWKALK.GetValueOrDefault() + angebotDto.ANGKALKGEBUEHR.GetValueOrDefault();
            angkalk.GESAMTUST = (angebotDto.ANGKALKLZ.GetValueOrDefault() * angebotDto.ANGKALKRATEUST.GetValueOrDefault()) + angebotDto.ANGKALKSZUST.GetValueOrDefault() + angebotDto.ANGKALKRWKALKUST.GetValueOrDefault() + angebotDto.ANGKALKGEBUEHRUST.GetValueOrDefault();

            //Calculate BGINTERN
            angkalk.BGEXTERN = angob.AHKEXTERNBRUTTO - angkalk.MITFIN;
            angkalk.BGINTERN = angkalk.BGEXTERN - angkalk.SZ;

            //Round
            angkalk.GESAMTBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(angkalk.GESAMTBRUTTO.GetValueOrDefault());
            angkalk.GESAMT = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(angkalk.GESAMT.GetValueOrDefault());
            angkalk.GESAMTUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(angkalk.GESAMTUST.GetValueOrDefault());

        }*/

        /// <summary>
        /// Diese Felder werden nur beim Anlegen dem Angebot zugewiesen, nicht beim Update
        /// </summary>
        /// <param name="angebot"></param>
        /// <param name="angkalk"></param>
        /// <param name="angob"></param>
        /// <param name="sysPrProduct"></param>
        /// <param name="context"></param>
        private void MyGetFields(ANGEBOT angebot, ANGKALK angkalk, ANGOB angob, long sysPrProduct, DdOlExtended context)
        {
            //From Service Validator
            angebot.SYSVP = _VpSysPERSON;
            angebot.SYSVPFIL = _VpSysPERSON;
            angebot.SYSBERATADDB = _SysPERSON;
            angebot.SYSWFUSER = _SysWfuser;
            angebot.SYSLF = _SysLf;


        }

        private void MySetWFMMEMOFields(DdOwExtended context, ANGEBOT angebot, WFMMEMO wfmmemo, bool create)
        {
            if (wfmmemo != null)
            {
                if (create)
                {
                    wfmmemo.CREATEDATE = DateTime.Now;
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    wfmmemo.CREATEUSER = _SysPERSON;
                    wfmmemo.SYSLEASE = angebot.SYSID;
                    wfmmemo.SYSWFMTABLE = WFTABLEHelper.DeliverSyswftableForAngebot(context);
                    wfmmemo.WFMMKAT = WFMKATHelper.DeliverWfmkatForAngebot(context);
                }
                else
                {
                    wfmmemo.EDITDATE = DateTime.Now;
                    wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    wfmmemo.EDITUSER = _SysPERSON;
                }
                
            }
        }

        /// <summary>
        /// Creates or updates a WFMMEMO
        /// </summary>
        /// <param name="context"></param>
        /// <param name="syslease"></param>
        /// <param name="wfmmemo"></param>
        /// <param name="kategorieBezeichnung"></param>
        /// <returns></returns>
        private WFMMEMO MyUpdateWFMMEMOFields(DdOwExtended context, long syslease, WFMMEMO wfmmemo, String kategorieBezeichnung)
        {

            if (wfmmemo == null)
            {
                wfmmemo = new WFMMEMO();
                wfmmemo.CREATEDATE = DateTime.Now;
                wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                wfmmemo.CREATEUSER = _SysPERSON;
                wfmmemo.SYSLEASE = syslease;
                wfmmemo.SYSWFMTABLE = WFTABLEHelper.DeliverSyswftableForAngebot(context);
                wfmmemo.WFMMKAT = WFMKATHelper.DeliverWfmkat(context, kategorieBezeichnung);
                if (_SysPERSON.HasValue)
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        PERSON vkper = PERSONHelper.SelectBySysPERSONWithoutException(Context, (long)_SysPERSON);
                        if (vkper != null)
                        {
                            wfmmemo.STR02 = vkper.NAME + " " + vkper.VORNAME;
                            if (wfmmemo.STR02.Length > 40)
                                wfmmemo.STR02 = wfmmemo.STR02.Substring(0, 40);
                        }
                    }
                context.WFMMEMO.Add(wfmmemo);
            }
            else
            {
                wfmmemo.EDITDATE = DateTime.Now;
                wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                wfmmemo.EDITUSER = _SysPERSON;
                if (_SysPERSON.HasValue && (wfmmemo.STR02 == null || wfmmemo.STR02.Length < 2))//#4467, keep the original createuser in Str02
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        PERSON vkper = PERSONHelper.SelectBySysPERSONWithoutException(Context, (long)_SysPERSON);
                        if (vkper != null)
                        {
                            wfmmemo.STR02 = vkper.NAME + " " + vkper.VORNAME;
                            if (wfmmemo.STR02.Length > 40)
                                wfmmemo.STR02 = wfmmemo.STR02.Substring(0, 40);
                        }
                    }
            }
            return wfmmemo;
        }

        private void MyUpdateSicherheiten(ANGEBOTDto angebotDto, ANGEBOT ang, DdOlExtended context)
        {
            SICHTYP sich = SICHTYPHelper.GetSichTyp(context, RUECKNAMESICHTYP);
            ANGOBSICH sicherheit = ANGOBSICHHelper.GetAngobsischByRang(context, ang.SYSID, RUECKNAMESICHRANG);
            if (sicherheit == null && angebotDto.RESTWERTGARANTIE)
            {
                sicherheit = new ANGOBSICH();
                context.ANGOBSICH.Add(sicherheit);
            }
            if (sicherheit != null)
            {
                sicherheit.RANG = RUECKNAMESICHRANG;
                sicherheit.BEZEICHNUNG = "Restwertgarantie";
                sicherheit.SYSPERSON = _VpSysPERSON;
                sicherheit.SYSSICHTYP=sich.SYSSICHTYP;

                // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
                // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
                sicherheit.SYSVT = ang.SYSID;
                sicherheit.SYSANGEBOT = ang.SYSID;

                sicherheit.AKTIVFLAG = angebotDto.RESTWERTGARANTIE ? 1 : 0;
            }


            sich = SICHTYPHelper.GetSichTyp(context, ZBIISICHRANG);
            sicherheit = ANGOBSICHHelper.GetAngobsischByRang(context, ang.SYSID, ZBIISICHRANG);
            if (sicherheit == null)
            {
                sicherheit = new ANGOBSICH();
                context.ANGOBSICH.Add(sicherheit);
            }
            if (sicherheit != null)
            {
                sicherheit.RANG = ZBIISICHRANG;
                sicherheit.BEZEICHNUNG = "Zulassungsbescheinigung";
                sicherheit.SYSIT= angebotDto.SYSIT.Value;
                sicherheit.SYSSICHTYP= sich.SYSSICHTYP;

                // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
                sicherheit.SYSVT = ang.SYSID;
                sicherheit.SYSANGEBOT = ang.SYSID;

                sicherheit.AKTIVFLAG =1;
            }

        }
        /// <summary>
        /// Update Provisions in db from dto
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="angKalk"></param>
        /// <param name="context"></param>
        /// <param name="vpsysperson"></param>
        /// <param name="assignabprov">when true only this value will be considered for update</param>
        private void MyUpdateProvisions(ANGEBOTDto angebotDto, ANGKALK angKalk, DdOlExtended context, long? vpsysperson, bool assignabprov, decimal Ust)
        {


            bool iscredit = false;

            CalculationDao calcDao = new CalculationDao(context);
            if (angebotDto.SYSPRPRODUCT.HasValue)
            {
                VartDTO va = calcDao.getVART(angebotDto.SYSPRPRODUCT.Value);
                if (va.CODE.IndexOf("KREDIT") > -1)
                    iscredit = true;
            }

            foreach (ProvisionTypeConstants LoopProvision in Enum.GetValues(typeof(ProvisionTypeConstants)))
            {
                try
                {
                    decimal ProvisionValue = 0;
                    decimal Wunschprovision = 0;
                    long? SYSPROVTARIF = 0;
                    long? provReceiver = vpsysperson;
                    if (assignabprov && angebotDto.APROVPEROLE == 0)
                        return;
                    if (assignabprov && LoopProvision != ProvisionTypeConstants.Abschluss)
                        continue;
                    decimal? provisionpro = null;
                    switch (LoopProvision)
                    {
                        case ProvisionTypeConstants.Abschluss:
                            ProvisionValue = angebotDto.ABSCHLUSSPROVISION.GetValueOrDefault();
                            Wunschprovision = angebotDto.WUNSCHPROVISION.GetValueOrDefault();
                            SYSPROVTARIF = angebotDto.SYSPROVTARIF;
                            if (angebotDto.APROVPEROLE > 0)
                            {
                                provReceiver =  (from perole in context.PEROLE
                                                 where perole.SYSPEROLE == angebotDto.APROVPEROLE 
                                             select perole.SYSPERSON).FirstOrDefault();
                            }
                            break;
                        case ProvisionTypeConstants.GAP:
                            ProvisionValue = getProvisionFromInsurance(context, angebotDto, "GAP");
                            break;
                        case ProvisionTypeConstants.Bearbeitungsgebuehr:
                            //ProvisionValue = angebotDto.GEBUEHRPROVISION.GetValueOrDefault();
                            continue;
                        case ProvisionTypeConstants.WartungReparatur:
                            //ProvisionValue = angebotDto.WARTUNGSPROVISION.GetValueOrDefault();
                            continue;
                        case ProvisionTypeConstants.Haftpflicht:
                            ProvisionValue = getProvisionFromInsurance(context, angebotDto, "HP");
                            break;
                        case ProvisionTypeConstants.Kasko:
                            ProvisionValue = getProvisionFromInsurance(context, angebotDto, "KASKO");
                            break;
                        case ProvisionTypeConstants.Restschuld:
                            ProvisionValue = getProvisionFromInsurance(context, angebotDto, "RSDV");
                            break;
                        case ProvisionTypeConstants.Zugang:
                            ProvisionValue = angebotDto.ZUGANGSPROVISION.GetValueOrDefault();
                            provisionpro = angebotDto.ZUGANGSPROVISIONPRO.GetValueOrDefault();

                            if (!iscredit)
                            {
                                ProvisionValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(ProvisionValue, Ust));
                            }
                            break;
                    }
                    

                    int Type = (int)LoopProvision;

                    ANGPROV CurrentAngProv = (from AngProv in context.ANGPROV
                                              where AngProv.ANGKALK.SYSKALK == angKalk.SYSKALK
                                              && AngProv.ART == Type
                                              select AngProv).FirstOrDefault();
                    if (CurrentAngProv == null && ProvisionValue == 0) continue;//not yet saved and zero, dont create one
                    if (CurrentAngProv != null && (ProvisionValue < 0 || ProvisionValue == decimal.MinValue))
                    {
                            context.DeleteObject(CurrentAngProv);//remove negative provisions (should be only gap! / Zugangsprovision)
                    }
                    if (ProvisionValue == decimal.MinValue) continue;
                    if (ProvisionValue < 0) continue;//must reside in subvention!

                    // Check if anything was found
                    if (CurrentAngProv == null)
                    {
                        // Create new
                        CurrentAngProv = new ANGPROV();

                        // Add
                        context.ANGPROV.Add(CurrentAngProv);
                    }

                    // Map ANGPROV
                    CurrentAngProv.PROVISION = ProvisionValue;
                    if(!iscredit)
                        CurrentAngProv.PROVISIONBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(ProvisionValue, Ust));
                    else 
                        CurrentAngProv.PROVISIONBRUTTO = CurrentAngProv.PROVISION;

                   


                    CurrentAngProv.PROVISIONUST = CurrentAngProv.PROVISIONBRUTTO - CurrentAngProv.PROVISION;

                    CurrentAngProv.ART = (int)LoopProvision;
                    CurrentAngProv.ANGKALK = angKalk;
                    CurrentAngProv.SYSPROVTARIF = SYSPROVTARIF;

                    CurrentAngProv.PERSON = (from Person in context.PERSON
                                             where Person.SYSPERSON == provReceiver
                                             select Person).FirstOrDefault();
                    CurrentAngProv.VALUTA = DateTime.Today;
                    CurrentAngProv.TEXT = LoopProvision.ToString();
                    CurrentAngProv.WUNSCHPROVISION = Wunschprovision;
                    if (provisionpro!=null && provisionpro.HasValue)
                        CurrentAngProv.PROVISIONPRO = provisionpro;
                }
                catch (Exception ex)
                {
                    _Log.Error("Update of provision failed: " + LoopProvision, ex);
                }
            }
        }

        public static decimal getProvisionFromInsurance(DdOlExtended context, ANGEBOTDto angebotDto, String codeMethod)
        {
            if (angebotDto.ANGVSPARAM != null && angebotDto.ANGVSPARAM.Length > 0)
            {
                VSTYPDao vs = new VSTYPDao(context);
                foreach (InsuranceDto insurance in angebotDto.ANGVSPARAM)
                {

                    VSTYP vtemp = vs.getVsTyp(insurance.InsuranceParameter.SysVSTYP);

                    if (codeMethod.Equals(vtemp.CODEMETHOD))
                    {
                        return insurance.InsuranceResult.Provision;
                    }

                }
            }
            return decimal.MinValue;
        }
        /// <summary>
        /// Returns the insurance value for the given insurance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="angebotDto"></param>
        /// <param name="codeMethod"></param>
        /// <returns></returns>
        public static decimal getValueFromInsurance(DdOlExtended context, ANGEBOTDto angebotDto, String codeMethod)
        {
            if (angebotDto.ANGVSPARAM != null && angebotDto.ANGVSPARAM.Length > 0)
            {
                VSTYPDao vs = new VSTYPDao(context);
                foreach (InsuranceDto insurance in angebotDto.ANGVSPARAM)
                {

                    VSTYP vtemp = vs.getVsTyp(insurance.InsuranceParameter.SysVSTYP);

                    if (codeMethod.Equals(vtemp.CODEMETHOD))
                    {
                        return insurance.InsuranceResult.Praemie;
                    }

                }
            }
            return 0;
        }


        //assign the history-values to the dto, use after MyGetInsurances!
        private void MyGetHistoryData(ANGEBOTDto angebotDto, DdOlExtended context)
        {
            try
            {
                
                if (angebotDto.SYSPRPRODUCT.HasValue)
                {
                    angebotDto.HIST_SYSPRPRODUCT  = context.ExecuteStoreQuery<string>("select name from prproduct where sysprproduct=" + angebotDto.SYSPRPRODUCT.Value, null).FirstOrDefault();
                }
                else
                    angebotDto.HIST_SYSPRPRODUCT = "Noch kein Produkt";

                if (angebotDto.ANGKALKZINSTYP.HasValue)
                    angebotDto.HIST_ANGKALKZINSTYP = context.ExecuteStoreQuery<string>("select name from inttype where sysinttype=" + angebotDto.ANGKALKZINSTYP, null).FirstOrDefault();
                if (angebotDto.SYSPROVTARIF.HasValue)
                    angebotDto.HIST_SYSPROVTARIF = context.ExecuteStoreQuery<string>("select name from provtarif where sysprovtarif=" + angebotDto.SYSPROVTARIF, null).FirstOrDefault();

                angebotDto.HIST_ANGOBSYSOBKAT = context.ExecuteStoreQuery<string>("select name from obkat where sysobkat=" + angebotDto.ANGOBSYSOBKAT, null).FirstOrDefault();
                if (angebotDto.SYSOBART.HasValue)
                    angebotDto.HIST_SYSOBART = context.ExecuteStoreQuery<string>("select name from obart where sysobart=" + angebotDto.SYSOBART, null).FirstOrDefault();

                VSTYPDao vsdao = new VSTYPDao(context);

                if (angebotDto.ANGKALKFUELSYSFSTYP.HasValue)
                {

                    try
                    {
                        angebotDto.HIST_ANGKALKFUELSYSFSTYP = FsPreisHelper.GetFsTypByKey((long)angebotDto.ANGKALKFUELSYSFSTYP, context).BEZEICHNUNG;
                    }
                    catch (Exception) { }
                }

                foreach (var LoopInsuance in angebotDto.ANGVSPARAM)
                {



                    VSTYP vstyp = vsdao.getVsTyp(LoopInsuance.InsuranceParameter.SysVSTYP);

                    if (vstyp == null) continue;

                    LoopInsuance.InsuranceParameter.CODEMETHOD = vstyp.CODEMETHOD;



                    PersonName p = vsdao.getVSPERSON(LoopInsuance.InsuranceParameter.SysVSTYP);

                    String vsname = "";
                    if (p != null)
                        vsname = p.NAME;

                    if ("HP".Equals(vstyp.CODEMETHOD))
                    {
                        angebotDto.HIST_HP_VSPERSON = vsname;
                        angebotDto.HIST_HP_VSTYP = vstyp.BESCHREIBUNG;//Deckungssumme

                    }
                    else if ("IUV".Equals(vstyp.CODEMETHOD))
                    {
                        angebotDto.HIST_IUV_VSPERSON = vsname;

                    }
                    else if ("KASKO".Equals(vstyp.CODEMETHOD))
                    {
                        angebotDto.HIST_KASKO_VSPERSON = vsname;
                        angebotDto.HIST_KASKO_VSTYP = vstyp.BESCHREIBUNG;
                        angebotDto.HIST_KASKO_DESC = vstyp.BESCHREIBUNG;

                    }
                    else if ("GAP".Equals(vstyp.CODEMETHOD))
                    {
                        angebotDto.HIST_GAP_VSPERSON = vsname;
                        angebotDto.HIST_GAP_VSTYP = vstyp.BESCHREIBUNG;
                        angebotDto.HIST_GAP_LZ = "" + LoopInsuance.InsuranceParameter.Laufzeit;

                    }
                    else if ("RSV".Equals(vstyp.CODEMETHOD))
                    {
                        angebotDto.HIST_RSV_VSPERSON = vsname;
                        
                    }
                    else if ("RSDV".Equals(vstyp.CODEMETHOD) && angebotDto.SYSITRSVVN.HasValue)
                    {
                        angebotDto.HIST_RSDV_VSPERSON = vsname;
                        angebotDto.HIST_RSDV_VSTYP = vstyp.BESCHREIBUNG;
                        
                        try
                        {
                            ITName nameinfo = context.ExecuteStoreQuery<ITName>("select name,vorname from it where sysit=" + angebotDto.SYSITRSVVN, null).FirstOrDefault();

                            if (nameinfo != null)
                                angebotDto.HIST_SYSITRSVVN = nameinfo.NAME + " " + nameinfo.VORNAME;
                        }
                        catch (Exception e)
                        {
                            _Log.Error("Angebot contains a non-existent person: " + angebotDto.SYSITRSVVN, e);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                _Log.Error("Retrieving History Data failed", ex);
            }

        }

        private class ITName
        {
            public String NAME { get; set; }
            public String VORNAME { get; set; }
        }
        //assign the _default-values to the dto
        private void MyGetSubventions(ANGEBOTDto angebotDto, DdOlExtended context)
        {
            // Query ANGSUBV
            var subventions = from subv in context.ANGSUBV
                              where subv.ANGEBOT.SYSID == angebotDto.SYSID
                              select subv;

            foreach (ANGSUBV subv in subventions)
            {
                if (((int)SubventionTypeConstants.Gebuehr).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKGEBUEHR_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.RgGebuehr).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKRGGEBUEHR_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.Rate).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKRATE_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.MitFin).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKMITFIN_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.FuelPrice).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKFSFUELPRICE_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.Anabmeldung).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKFSANABMELDUNG_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.RepCarRate).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKFSREPCARRATE_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.TiresPrice).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKFSSTIRESPRICE_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.Maintenance).ToString().Equals(subv.CODE))
                {
                    angebotDto.ANGKALKFSMAINTENANCE_DEFAULT = subv.BETRAGDEF;
                }
                else if (((int)SubventionTypeConstants.RateKredit).ToString().Equals(subv.CODE))
                {
                    angebotDto.ZUGANGSPROVISION = -1 * subv.BETRAGBRUTTO;
                    angebotDto.ZUGANGSPROVISIONNETTO = -1 * subv.BETRAG;
                    //angebotDto.TOTALPROVISION += angebotDto.ZUGANGSPROVISION;
                }
                else if (((int)SubventionTypeConstants.GAP).ToString().Equals(subv.CODE))
                {
                    angebotDto.GAPPROVISION = -1*subv.BETRAGBRUTTO;
                    //angebotDto.TOTALPROVISION += angebotDto.ZUGANGSPROVISION;
                }
            }

        }

        /// <summary>
        /// returns the subvention value given for an insurance
        /// </summary>
        /// <param name="insurancecode"></param>
        /// <param name="subventions"></param>
        /// <param name="laufzeit"></param>
        /// <returns></returns>
        private decimal getSubvention(String insurancecode, List<ANGSUBV> subventions, int laufzeit)
        {
            SubventionTypeConstants fldid;
            if (insurancecode.Equals(VSCalcFactory.Cnst_CALC_HAFTPFLICHT))
                fldid = SubventionTypeConstants.Haftpflicht;
            else if (insurancecode.Equals(VSCalcFactory.Cnst_CALC_INSASSEN))
                fldid = SubventionTypeConstants.IUV;
            else if (insurancecode.Equals(VSCalcFactory.Cnst_CALC_KASKO))
                fldid = SubventionTypeConstants.Kasko;
            else if (insurancecode.Equals(VSCalcFactory.Cnst_CALC_RECHTSSCHUTZ))
                fldid = SubventionTypeConstants.Rechtsschutz;
            else if (insurancecode.Equals(VSCalcFactory.Cnst_CALC_RESTSCHULD))
                fldid = SubventionTypeConstants.Restschuld;
            else return 0;

            foreach (ANGSUBV subv in subventions)
            {

                if (((int)fldid).ToString().Equals(subv.CODE))
                {
                    return (decimal)subv.BETRAG / laufzeit;
                }

            }
            return 0;
        }
        private void MyGetSicherheiten(ANGEBOTDto angebotDto, DdOlExtended context)
        {
            ANGOBSICH sicherheit = ANGOBSICHHelper.GetAngobsischByRang(context, angebotDto.SYSID.Value, RUECKNAMESICHRANG);
            angebotDto.RESTWERTGARANTIE = false;
            if (sicherheit != null)
            {
                if (sicherheit.AKTIVFLAG.HasValue && sicherheit.AKTIVFLAG.Value > 0)
                    angebotDto.RESTWERTGARANTIE = true;

            }
        }
        /// <summary>
        /// Update Dto Provision from DB
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="angKalk"></param>
        /// <param name="context"></param>
        private void MyGetProvisions(ANGEBOTDto angebotDto, ANGKALK angKalk, DdOlExtended context,decimal Ust)
        {
            /*bool iscredit = false;

            CalculationDao calcDao = new CalculationDao(context);
            if (angebotDto.SYSPRPRODUCT.HasValue)
            {
                VART va = calcDao.getVART(angebotDto.SYSPRPRODUCT.Value);
                if (va.CODE.IndexOf("KREDIT") > -1)
                    iscredit = true;
            }*/

            // Query ANGPROV
            var Provisions = from Provision in context.ANGPROV
                             where Provision.ANGKALK.SYSKALK == angKalk.SYSKALK
                             select Provision;
            angebotDto.ABSCHLUSSPROVISION = 0;
            angebotDto.WUNSCHPROVISION = 0;
            angebotDto.SYSPROVTARIF = 0;
            angebotDto.GEBUEHRPROVISION = 0;
            angebotDto.HAFTPFLICHTPROVISION = 0;
            angebotDto.KASKOPROVISION = 0;
            angebotDto.RESTSCHULDPROVISION = 0;
            angebotDto.WARTUNGSPROVISION = 0;
            angebotDto.ZUGANGSPROVISION = 0;
            angebotDto.GAPPROVISION = 0;
            angebotDto.TOTALPROVISION = 0;
            angebotDto.APROVPEROLE = 0;
            // Iterate through all results
            foreach (var LoopProvision in Provisions)
            {
                // Get the type
                ProvisionTypeConstants Type = (ProvisionTypeConstants)LoopProvision.ART;

                switch (Type)
                {
                    case ProvisionTypeConstants.Abschluss:
                        angebotDto.ABSCHLUSSPROVISION = LoopProvision.PROVISION;
                        angebotDto.TOTALPROVISION += angebotDto.ABSCHLUSSPROVISION;
                        angebotDto.WUNSCHPROVISION = LoopProvision.WUNSCHPROVISION;
                        angebotDto.SYSPROVTARIF = LoopProvision.SYSPROVTARIF;

                        long syspersonprov = LoopProvision.SYSVM.GetValueOrDefault();// sysper DdOlExtended.getKey(LoopProvision.PERSONReference.EntityKey);
                        angebotDto.APROVPEROLE = context.ExecuteStoreQuery<long>("select perole.sysperole from perole, roletype where perole.sysroletype=roletype.sysroletype and roletype.typ=7 and perole.sysperson=" + syspersonprov, null).FirstOrDefault();
                        
                        break;
                    case ProvisionTypeConstants.GAP:
                        angebotDto.GAPPROVISION = LoopProvision.PROVISION;
                        if (checkFlag(angebotDto.ANGKALKFSGAPFLAG))
                            angebotDto.TOTALPROVISION += angebotDto.GAPPROVISION;
                        break;
                    case ProvisionTypeConstants.Bearbeitungsgebuehr:
                        angebotDto.GEBUEHRPROVISION = LoopProvision.PROVISION;
                        angebotDto.TOTALPROVISION += angebotDto.GEBUEHRPROVISION;
                        break;
                    case ProvisionTypeConstants.Haftpflicht:
                        angebotDto.HAFTPFLICHTPROVISION = LoopProvision.PROVISION;
                        if (checkFlag(angebotDto.ANGKALKFSHPFLAG))
                            angebotDto.TOTALPROVISION += angebotDto.HAFTPFLICHTPROVISION;
                        break;
                    case ProvisionTypeConstants.Kasko:
                        angebotDto.KASKOPROVISION = LoopProvision.PROVISION;
                        if (checkFlag(angebotDto.ANGKALKFSVKFLAG))
                            angebotDto.TOTALPROVISION += angebotDto.KASKOPROVISION;
                        break;
                    case ProvisionTypeConstants.Restschuld:
                        angebotDto.RESTSCHULDPROVISION = LoopProvision.PROVISION;
                        if (checkFlag(angebotDto.ANGKALKFSRSVFLAG))
                            angebotDto.TOTALPROVISION += angebotDto.RESTSCHULDPROVISION;
                        break;
                    case ProvisionTypeConstants.WartungReparatur:
                        angebotDto.WARTUNGSPROVISION = LoopProvision.PROVISION;
                        if (checkFlag(angebotDto.ANGKALKFSMAINTENANCEFLAG))
                            angebotDto.TOTALPROVISION += angebotDto.WARTUNGSPROVISION;
                        break;
                    case ProvisionTypeConstants.Zugang:
                        angebotDto.ZUGANGSPROVISION = LoopProvision.PROVISIONBRUTTO;
                        angebotDto.ZUGANGSPROVISIONPRO = LoopProvision.PROVISIONPRO;
                        angebotDto.ZUGANGSPROVISIONNETTO = LoopProvision.PROVISION;
                        //if (LoopProvision.PROVISION.HasValue)
                          //  angebotDto.ZUGANGSPROVISIONNETTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(LoopProvision.PROVISION.Value, Ust));
                        angebotDto.TOTALPROVISION += angebotDto.ZUGANGSPROVISION;
                        break;
                }
            }
        }

        /// <summary>
        /// Reads the given insurance code data
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="context"></param>
        /// <param name="vscode"></param>
        /// <returns></returns>
        private InsuranceDto getInsurance(ANGEBOTDto angebotDto, DdOlExtended context, String vscode)
        {
            try
            {

                //Update/Insert Insurances from AngebotDto to DB
                if (angebotDto.ANGVSPARAM != null)
                {
                    VSTYPDao vsdao = new VSTYPDao(context);
                    foreach (var LoopInsuance in angebotDto.ANGVSPARAM)
                    {
                        try
                        {

                            VSTYP vt = vsdao.getVsTyp(LoopInsuance.InsuranceParameter.SysVSTYP);

                            if (vt == null)
                            {
                                continue;
                            }
                            if (vscode.Equals(vt.CODEMETHOD))
                            {
                                return LoopInsuance;
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error("Reading Insurance failed for code " + vscode, ex);
            }
            return null;

        }

        /// <summary>
        /// Validates the integrity of an insurance FLAG with the given ANGVSPARAM-Contents
        /// angebotDto will be changed
        /// </summary>
        /// <param name="angebotDto"></param>
        public static void validateInsurances(ANGEBOTDto angebotDto)
        {
            if (angebotDto.ANGVSPARAM == null)
                angebotDto.ANGVSPARAM = new InsuranceDto[0];

            angebotDto.ANGVSPARAM = (from v in angebotDto.ANGVSPARAM
                                     where v.InsuranceParameter.CODEMETHOD != null
                                     select v).ToArray();

            List<InsuranceDto> result = new List<InsuranceDto>();

            //wenn flag 1 und keine in angebotDto.ANGVSPARAM, dann FLAG 0 setzen
            int inscount = (from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("RSDV")
                            select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSRSVFLAG) && inscount == 0)
                angebotDto.ANGKALKFSRSVFLAG = 0;
            else if(inscount>0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("RSDV")
                            select v).FirstOrDefault());
            }
          

            inscount = (from v in angebotDto.ANGVSPARAM
                        where v.InsuranceParameter.CODEMETHOD.Equals("KASKO")
                        select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSVKFLAG) && inscount == 0)
                angebotDto.ANGKALKFSVKFLAG = 0;
            else if (inscount > 0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("KASKO")
                            select v).FirstOrDefault());
            }

            inscount = (from v in angebotDto.ANGVSPARAM
                        where v.InsuranceParameter.CODEMETHOD.Equals("HP")
                        select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSHPFLAG) && inscount == 0)
                angebotDto.ANGKALKFSHPFLAG = 0;
            else if (inscount > 0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("HP")
                            select v).FirstOrDefault());
            }

            inscount = (from v in angebotDto.ANGVSPARAM
                        where v.InsuranceParameter.CODEMETHOD.Equals("IUV")
                        select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSINSASSENFLAG) && inscount == 0)
                angebotDto.ANGKALKFSINSASSENFLAG = 0;
            else if (inscount > 0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("IUV")
                            select v).FirstOrDefault());
            }


            inscount = (from v in angebotDto.ANGVSPARAM
                        where v.InsuranceParameter.CODEMETHOD.Equals("GAP")
                        select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSGAPFLAG) && inscount == 0)
                angebotDto.ANGKALKFSGAPFLAG = 0;
            else if (inscount > 0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("GAP")
                            select v).FirstOrDefault());
            }

            inscount = (from v in angebotDto.ANGVSPARAM
                        where v.InsuranceParameter.CODEMETHOD.Equals("RSV")
                        select v).Count();
            if (checkFlag(angebotDto.ANGKALKFSRECHTSCHUTZFLAG) && inscount == 0)
                angebotDto.ANGKALKFSRECHTSCHUTZFLAG = 0;
            else if (inscount > 0)
            {
                result.Add((from v in angebotDto.ANGVSPARAM
                            where v.InsuranceParameter.CODEMETHOD.Equals("RSV")
                            select v).FirstOrDefault());
            }

            angebotDto.ANGVSPARAM = result.ToArray();


            //alle aus ANGVSPARAM entfernen, die nicht angeflagt sind
            List<InsuranceDto> resultInsurances = new List<InsuranceDto>();
            foreach (InsuranceDto ins in angebotDto.ANGVSPARAM)
            {

                if (checkFlag(angebotDto.ANGKALKFSRSVFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("RSDV"))
                    resultInsurances.Add(ins);
                else if (ins.InsuranceParameter.CODEMETHOD.Equals("GAP"))
                {
                    if (checkFlag(angebotDto.ANGKALKFSGAPFLAG)) resultInsurances.Add(ins);//nur wenn gap angehakt überhaupt speichern
                }
                else if (!ins.InsuranceParameter.CODEMETHOD.Equals("RSDV") && !ins.InsuranceParameter.CODEMETHOD.Equals("GAP"))
                {
                    resultInsurances.Add(ins);
                }
                /*if (checkFlag(angebotDto.ANGKALKFSVKFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("KASKO"))
                    resultInsurances.Add(ins);
                if (checkFlag(angebotDto.ANGKALKFSHPFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("HP"))
                    resultInsurances.Add(ins);
                if (checkFlag(angebotDto.ANGKALKFSINSASSENFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("IUV"))
                    resultInsurances.Add(ins);
                if (checkFlag(angebotDto.ANGKALKFSRECHTSCHUTZFLAG) && ins.InsuranceParameter.CODEMETHOD.Equals("RSV"))
                    resultInsurances.Add(ins);*/
            }
            angebotDto.ANGVSPARAM = resultInsurances.ToArray();

        }
        private decimal getSlpos(long sysangvs, DdOlExtended context)
        {
            String slpquery = "select slpos.betrag from slpos,sllink,sl where sl.syssl=slpos.syssl and slpos.syssl=sllink.syssl and sllink.sysid=:sysangvs and sllink.gebiet='ANGVS'";
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvs", Value = sysangvs });
            return context.ExecuteStoreQuery<decimal>(slpquery, parameters.ToArray()).FirstOrDefault();
        }
        private void createOrUpdateSlpos(long sysangvs, decimal? value)
        {
            using (OpenOne.Common.Model.DdOl.DdOlExtended context = new OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                String slpquery = "select slpos.syssl from slpos,sllink,sl where sl.syssl=slpos.syssl and slpos.syssl=sllink.syssl and sllink.sysid=:sysangvs and sllink.gebiet='ANGVS'";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvs", Value = sysangvs });
                long syssl = context.ExecuteStoreQuery<long>(slpquery, parameters.ToArray()).FirstOrDefault();

                if (syssl == 0)
                {
                    long syssltyp = context.ExecuteStoreQuery<long>("select syssltyp from sltyp where bemerkung='CODE:ANGVSALL'", null).FirstOrDefault();
                    CIC.Database.OL.EF6.Model.SL sl = new CIC.Database.OL.EF6.Model.SL();
                    sl.SYSSLTYP = syssltyp;// SLTYPReference.EntityKey = context.getEntityKey(typeof(CIC.Database.OL.EF6.Model.SLTYP), syssltyp);//must be set by gui
                    sl.STAND = DateTime.Now;
                    context.SL.Add(sl);
                    context.SaveChanges();

                    CIC.Database.OL.EF6.Model.SLLINK sllink = new CIC.Database.OL.EF6.Model.SLLINK();
                    sllink.GEBIET = "ANGVS";
                    sllink.SYSID = sysangvs;
                    sllink.SYSSL = sl.SYSSL;// SLReference.EntityKey = context.getEntityKey(typeof(CIC.Database.OL.EF6.Model.SL), sl.SYSSL);
                    context.SLLINK.Add(sllink);
                    context.SaveChanges();
                    syssl = sl.SYSSL;
                }
                long sysslpos = context.ExecuteStoreQuery<long>("select sysslpos from slpos where syssl=" + syssl, null).FirstOrDefault();
                CIC.Database.OL.EF6.Model.SLPOS rval = null;
                if (sysslpos == 0)
                {
                    rval = new CIC.Database.OL.EF6.Model.SLPOS();                   
                    rval.SYSSLPOS = 0;
                    context.SLPOS.Add(rval);
                }
                else
                {
                    rval = (from f in context.SLPOS
                            where f.SYSSLPOS == sysslpos
                            select f).FirstOrDefault();

                }
                rval.SYSSL = syssl;// SLReference.EntityKey = context.getEntityKey(typeof(CIC.Database.OL.EF6.Model.SL), syssl);
                if (rval != null)
                    rval.BETRAG = value;
                context.SaveChanges();
            }

        }
        private void deleteSlpos(long sysangvs, DdOlExtended context)
        {
            String delquery = "select slpos.syssl from slpos,sllink,sl where sl.syssl=slpos.syssl and slpos.syssl=sllink.syssl and sllink.sysid=:sysangvs and sllink.gebiet='ANGVS'";
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvs", Value = sysangvs });
            long[] delIds = context.ExecuteStoreQuery<long>(delquery, parameters.ToArray()).ToArray();

            if (delIds != null && delIds.Count() > 0)
            {
                context.ExecuteStoreCommand("delete from slpos where syssl in (" + String.Join(",", delIds) + ")", null);
                context.ExecuteStoreCommand("delete from sl where syssl in (" + String.Join(",", delIds) + ")", null);
                context.ExecuteStoreCommand("delete from sllink where syssl in (" + String.Join(",", delIds) + ")", null);
            }
            //context.ExecuteStoreCommand("update sl set inaktiv=1, inaktivab=sysdate where syssl in (select syssl from sllink where gebiet='ANGVS' and sysid=" + sysangvs+")", null);
        }
        /// <summary>
        /// Update Insurance from DTO into DB
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="angKalkfs"></param>
        /// <param name="context"></param>
        private void MyUpdateInsurance(ANGEBOTDto angebotDto, ANGKALKFS angKalkfs, DdOlExtended context)
        {
            try
            {


                // Get the insurance list from database
                var ExistingInsurance = from Insurance in context.ANGVS
                                        where Insurance.SYSANGEBOT == angebotDto.SYSID
                                        select Insurance;

                // Remove all Insurances that are no more on the AngebotDto
                foreach (var LoopInsurance in ExistingInsurance)
                {
                    // Get the dto which is connected to the row
                    var InsuranceDto = angebotDto.ANGVSPARAM.Where(Insurance => Insurance.SysAngVs == LoopInsurance.SYSANGVS).FirstOrDefault();

                    // Check if dto exists
                    if (InsuranceDto == null)
                    {
                        // Remove the object
                        deleteSlpos(LoopInsurance.SYSANGVS,context);
                        //context.DeleteObject(LoopInsurance);
                        context.ExecuteStoreCommand("delete from angvspos where sysangvs="+LoopInsurance.SYSANGVS);
                        context.ExecuteStoreCommand("delete from angvs where sysangvs=" + LoopInsurance.SYSANGVS);
                    }
                }
                context.SaveChanges();
                VSTYPDao vsdao = new VSTYPDao(context);
                //Update/Insert Insurances from AngebotDto to DB
                if (angebotDto.ANGVSPARAM != null)
                {
                    foreach (var LoopInsuance in angebotDto.ANGVSPARAM)
                    {
                        try
                        {
                            var CurrentInsurance = (from Insurance in context.ANGVS
                                                    where Insurance.SYSANGVS == LoopInsuance.SysAngVs
                                                    select Insurance).FirstOrDefault();

                            if (CurrentInsurance == null)//neuanlage
                            {
                                CurrentInsurance = new ANGVS();
                                context.ANGVS.Add(CurrentInsurance);
                            }
                           

                            CurrentInsurance.SYSANGEBOT = angebotDto.SYSID.Value;
                            CurrentInsurance.PRAEMIE = LoopInsuance.InsuranceResult.Praemie;
                            CurrentInsurance.PRAEMIEDEFAULT = LoopInsuance.InsuranceResult.Praemie_Default;
                            CurrentInsurance.PRAEMIENETTO = LoopInsuance.InsuranceResult.Netto;
                            CurrentInsurance.VERSICHERUNGSSTEUER = LoopInsuance.InsuranceResult.Versicherungssteuer;
                            CurrentInsurance.VORVERSICHERUNG = LoopInsuance.InsuranceParameter.Vorversicherung;
                            CurrentInsurance.PRAEMIENSTUFE = LoopInsuance.InsuranceParameter.Praemienstufe;

                            CurrentInsurance.LZ = (int)LoopInsuance.InsuranceParameter.Laufzeit;
                            CurrentInsurance.DECKUNGSSUMME = LoopInsuance.InsuranceParameter.Deckungssumme;
                            CurrentInsurance.FREMDVERSICHERUNG = LoopInsuance.InsuranceParameter.Fremdversicherung;
                            CurrentInsurance.POLKENNZEICHEN = LoopInsuance.InsuranceParameter.PolKennzeichen;
                            CurrentInsurance.ZUBEHOERFINANZIERT = LoopInsuance.InsuranceParameter.ZubehoerFinanziert;
                            CurrentInsurance.NACHLASS = LoopInsuance.InsuranceParameter.Nachlass;
                            //XXX TODO Fehlt in edmx:
                            //CurrentInsurance.ZAHLMODUS = LoopInsuance.InsuranceParameter.Zahlmodus;

                            VSTYP vstyp = vsdao.getVsTyp(LoopInsuance.InsuranceParameter.SysVSTYP);


                            /*
                            ANGVS:SB1     Selbstbeteiligung1 bei der Versicherung 
                                                 - bei Kasko aus VSTYP:SBVK ziehen
                                                 - bei Haftpflicht aus VSTYP:SBHP ziehen.
                            ANGVS:SB2   Selbstbeteiligung2 bei der Versicherung 
                                                 - bei Kasko aus VSTYP:SBHP ziehen
                             * */
                            if (vstyp == null)
                            {
                                _Log.Error("Received incomplete Insurance Data: " + _Log.dumpObject(LoopInsuance));
                                continue;
                            }

                            CurrentInsurance.SYSVSTYP=vstyp.SYSVSTYP;
                            if ("HP".Equals(vstyp.CODEMETHOD))
                            {
                                angKalkfs.MOTORVS = LoopInsuance.InsuranceResult.Motorsteuer;
                                CurrentInsurance.SB1 = vstyp.SBHP;
                            }
                            else if ("KASKO".Equals(vstyp.CODEMETHOD))
                            {

                                CurrentInsurance.SB1 = vstyp.SBVK;
                                CurrentInsurance.SB2 = vstyp.SBHP;
                            }
                            else if ("RSDV".Equals(vstyp.CODEMETHOD))
                            {
                                // angebotDto.RESTSCHULDPRAEMIE = CurrentInsurance.PRAEMIENETTO;
                            }
                            if(CurrentInsurance.SYSANGVS==0)
                                context.SaveChanges();
                            
                            createOrUpdateSlpos(CurrentInsurance.SYSANGVS,LoopInsuance.InsuranceResult.Gesamtfinanzierungsrate);

                            List<Devart.Data.Oracle.OracleParameter> parametersupd = new List<Devart.Data.Oracle.OracleParameter>();
                            parametersupd.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvs", Value = CurrentInsurance.SYSANGVS });

                           

                            

                            if (LoopInsuance.InsuranceResult.positions != null && LoopInsuance.InsuranceResult.positions.Count > 0)
                            {
                                List<InsuranceResultDto> positions = context.ExecuteStoreQuery<InsuranceResultDto>("select sysangvspos, sysvstyppos, PraemieNetto Netto,Praemie,Versicherungssteuer,Praemie Praemie_Default,KOSTENABSCHL,KOSTENVERW,KOSTENSONST,PRAEMIELS,PRAEMIEVS from angvspos where sysangvs=" + CurrentInsurance.SYSANGVS, null).ToList();
                                foreach (InsuranceResultDto pos in LoopInsuance.InsuranceResult.positions)
                                {
                                    InsuranceResultDto dbpos = (from p in positions
                                                                where p.sysvstyppos == pos.sysvstyppos
                                                                select p).FirstOrDefault();
                                    if (dbpos != null && dbpos.sysangvspos > 0)
                                    {
                                        //update
                                        List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "brutto", Value = pos.Praemie });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "steuer", Value = pos.Versicherungssteuer });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "netto", Value = pos.Netto });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = dbpos.sysangvspos });
                                        context.ExecuteStoreCommand("update angvspos set PraemieNetto=:netto,Praemie=:brutto,Versicherungssteuer=:steuer where sysangvspos=:sysid", parameters2.ToArray());

                                    }
                                    else
                                    {
                                        //insert
                                        List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "brutto", Value = pos.Praemie });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "steuer", Value = pos.Versicherungssteuer });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "netto", Value = pos.Netto });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvs", Value = CurrentInsurance.SYSANGVS });
                                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvstyppos", Value = pos.sysvstyppos });
                                        context.ExecuteStoreCommand("insert into angvspos(sysangvs,sysvstyppos,praemienetto,praemie,versicherungssteuer) values(:sysangvs,:sysvstyppos,:netto,:brutto,:steuer)", parameters2.ToArray());
                                    }
                                }

                                context.ExecuteStoreCommand(@"merge into angvspos angvspos
                                                                    using (select * from vstyppos) vstyppos
                                                                    on (vstyppos.sysvstyppos=angvspos.sysvstyppos and angvspos.sysangvs=:sysangvs)
                                                                    when matched then update set 
                                                                    angvspos.KOSTENABSCHL=vstyppos.KOSTENABSCHLP*angvspos.praemie/100,
                                                                    angvspos.KOSTENVERW=vstyppos.KOSTENVERWP*angvspos.praemie/100,
                                                                    angvspos.KOSTENSONST=vstyppos.KOSTENSONSTP*angvspos.praemie/100,
                                                                    angvspos.PRAEMIELS=vstyppos.ANTEILLS*angvspos.praemie/100,
                                                                    angvspos.PRAEMIEVS=vstyppos.ANTEILVS*angvspos.praemie/100
                                                                    ", parametersupd.ToArray());

                                //HCERDREI-41 - Anpassung Kostenausweis Versicherungen Einzelrisiken (ANGVSPOS)
                                //alle angvspos für sysangvs=CurrentInsurance.SYSANGVS durchlaufen 
                                //falls sysvstyppos ein cfg enthält, schauen ob für abschl oder verw +der sonst in der cfg dazu ein Wert mit
                                //"Monatswert auf Jahr" == (*12)  oder "Gesamtwert auf Jahr" == (/ LZ * 12) 
                                //konfiguriert ist, falls ja, den aktuellen Feldwert entsprechend multiplizeren und updaten
                                List<InsuranceResultDto> sysvstypposes = context.ExecuteStoreQuery<InsuranceResultDto>("select sysvstyppos,sysangvspos from angvspos where sysangvs=" + CurrentInsurance.SYSANGVS).ToList();
                                foreach (InsuranceResultDto irupd in sysvstypposes)
                                {
                                    String abschl = AppConfig.Instance.GetEntry("" + irupd.sysvstyppos, "KOSTENABSCHL", "", "VSTYPPOS");
                                    if ("Monatswert auf Jahr".Equals(abschl))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set kostenabschl=kostenabschl*12 where sysangvspos=" + irupd.sysangvspos);
                                    }
                                    else if ("Gesamtwert auf Jahr".Equals(abschl))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set kostenabschl=kostenabschl*12/" + CurrentInsurance.LZ + " where sysangvspos=" + irupd.sysangvspos);
                                    }
                                    String verw = AppConfig.Instance.GetEntry("" + irupd.sysvstyppos, "KOSTENVERW", "", "VSTYPPOS");
                                    if ("Monatswert auf Jahr".Equals(verw))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set KOSTENVERW=KOSTENVERW*12 where sysangvspos=" + irupd.sysangvspos);
                                    }
                                    else if ("Gesamtwert auf Jahr".Equals(verw))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set KOSTENVERW=KOSTENVERW*12/" + CurrentInsurance.LZ + " where sysangvspos=" + irupd.sysangvspos);
                                    }
                                    String sonst = AppConfig.Instance.GetEntry("" + irupd.sysvstyppos, "KOSTENSONST", "", "VSTYPPOS");
                                    if ("Monatswert auf Jahr".Equals(sonst))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set KOSTENSONST=KOSTENSONST*12 where sysangvspos=" + irupd.sysangvspos);
                                    }
                                    else if ("Gesamtwert auf Jahr".Equals(sonst))
                                    {
                                        context.ExecuteStoreCommand("update angvspos set KOSTENSONST=KOSTENSONST*12/" + CurrentInsurance.LZ + " where sysangvspos=" + irupd.sysangvspos);
                                    }
                                }
                                 
                                context.SaveChanges();


                            }
                            List<InsuranceResultDto> aktpositions = context.ExecuteStoreQuery<InsuranceResultDto>("select sysangvspos, sysvstyppos, PraemieNetto Netto,Praemie,Versicherungssteuer,Praemie Praemie_Default,KOSTENABSCHL,KOSTENVERW,KOSTENSONST,PRAEMIELS,PRAEMIEVS from angvspos where sysangvs=" + CurrentInsurance.SYSANGVS, null).ToList();
                            double KOSTENABSCHL = (from f in aktpositions
                                                    select (f.KOSTENABSCHL)).Sum();
                            double KOSTENVERW = (from f in aktpositions
                                                 select (f.KOSTENVERW)).Sum();
                            double KOSTENSONST = (from f in aktpositions
                                                  select (f.KOSTENSONST)).Sum();
                            double PRAEMIELS = (from f in aktpositions
                                                   select (f.PRAEMIELS)).Sum();
                            double PRAEMIEVS = (from f in aktpositions
                                                select (f.PRAEMIEVS)).Sum();
                          


                            List<Devart.Data.Oracle.OracleParameter> parameters3 = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "KOSTENABSCHL", Value = KOSTENABSCHL });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "KOSTENVERW", Value = KOSTENVERW });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "KOSTENSONST", Value = KOSTENSONST });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PRAEMIELS", Value = PRAEMIELS });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PRAEMIEVS", Value = PRAEMIEVS });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = CurrentInsurance.SYSANGVS });
                            context.ExecuteStoreCommand("update angvs set KOSTENABSCHL=:KOSTENABSCHL,KOSTENVERW=:KOSTENVERW,KOSTENSONST=:KOSTENSONST,PRAEMIELS=:PRAEMIELS,PRAEMIEVS=:PRAEMIEVS where sysangvs=:sysid", parameters3.ToArray());


                        }
                        catch (Exception ex)
                        {
                            _Log.Error("Update of Insurance failed " + _Log.dumpObject(LoopInsuance), ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error("Update Insurance failed: ", ex);
            }

        }
        /// <summary>
        /// Deliver Insurance to Dto from DB, call after MyGetProvisions, because angebotDto will be filled with neeed provisions there
        /// sets the angkalkfsgapflag when insurance found
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="context"></param>
        private void MyGetInsurance(ANGEBOTDto angebotDto, DdOlExtended context)
        {
            var InsuranceSet = from Insurance in context.ANGVS
                               where Insurance.SYSANGEBOT == angebotDto.SYSID
                               select Insurance;
            // Query ANGSUBV
            var subventionQuery = from subv in context.ANGSUBV
                                  where subv.ANGEBOT.SYSID == angebotDto.SYSID
                                  select subv;

            List<ANGSUBV> subventions = subventionQuery.ToList();

            List<InsuranceDto> InsuranceList = new List<InsuranceDto>();
            VSTYPDao vs = new VSTYPDao(context);
            angebotDto.ANGKALKFSGAPFLAG = 0;
            

            foreach (var LoopInsurance in InsuranceSet)
            {
                InsuranceDto NewInsurance = new InsuranceDto();
                NewInsurance.InsuranceParameter = new InsuranceParameterDto();
                NewInsurance.InsuranceResult = new InsuranceResultDto();


                NewInsurance.SysAngVs = LoopInsurance.SYSANGVS;
                NewInsurance.InsuranceResult.Praemie = LoopInsurance.PRAEMIE.GetValueOrDefault();
                NewInsurance.InsuranceResult.Praemie_Default = LoopInsurance.PRAEMIEDEFAULT.GetValueOrDefault();
                NewInsurance.InsuranceResult.Netto = LoopInsurance.PRAEMIENETTO.GetValueOrDefault();
                NewInsurance.InsuranceResult.Versicherungssteuer = LoopInsurance.VERSICHERUNGSSTEUER.GetValueOrDefault();

                NewInsurance.InsuranceParameter.Praemienstufe = LoopInsurance.PRAEMIENSTUFE.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Praemienstufe = LoopInsurance.PRAEMIENSTUFE.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Deckungssumme = LoopInsurance.DECKUNGSSUMME.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Fremdversicherung = LoopInsurance.FREMDVERSICHERUNG;

                NewInsurance.InsuranceParameter.Laufzeit = LoopInsurance.LZ.GetValueOrDefault();

                NewInsurance.InsuranceParameter.Vorversicherung = LoopInsurance.VORVERSICHERUNG;
                NewInsurance.InsuranceParameter.PolKennzeichen = LoopInsurance.POLKENNZEICHEN;
                NewInsurance.InsuranceParameter.ZubehoerFinanziert = LoopInsurance.ZUBEHOERFINANZIERT.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Nachlass = LoopInsurance.NACHLASS.GetValueOrDefault();
                NewInsurance.InsuranceParameter.sysObArt = angebotDto.SYSOBART.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Zahlmodus = 12;
                NewInsurance.InsuranceParameter.sysObTyp = angebotDto.SYSOBTYP.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Finanzierungssumme = angebotDto.FINANZIERUNGSSUMME.GetValueOrDefault();

                NewInsurance.InsuranceParameter.KW = angebotDto.ANGOBKW.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Nova = angebotDto.ANGOBNOVA.GetValueOrDefault();
                NewInsurance.InsuranceParameter.sysPrProduct = angebotDto.SYSPRPRODUCT.GetValueOrDefault();
                NewInsurance.InsuranceParameter.Sonderausstattung = angebotDto.ANGOBAUST.Sum(Option => Option.BETRAG.GetValueOrDefault());
                

                NewInsurance.InsuranceParameter.SysVSTYP =LoopInsurance.SYSVSTYP.GetValueOrDefault();
                 

                if (NewInsurance.InsuranceParameter.SysVSTYP > 0)
                {

                    VSTYP vstemp = vs.getVsTyp(NewInsurance.InsuranceParameter.SysVSTYP);
                    NewInsurance.InsuranceParameter.CODEMETHOD = vstemp.CODEMETHOD;
                    NewInsurance.InsuranceResult.Praemie_Subvention = getSubvention(vstemp.CODEMETHOD, subventions, (int)angebotDto.ANGKALKLZ);
                    NewInsurance.InsuranceResult.positions = context.ExecuteStoreQuery<InsuranceResultDto>("select sysvstyppos, PraemieNetto Netto,Praemie,Versicherungssteuer,Praemie Praemie_Default from angvspos where sysangvs=" + NewInsurance.SysAngVs, null).ToList();
                    if (VSCalcFactory.Cnst_CALC_HAFTPFLICHT.Equals(vstemp.CODEMETHOD) && angebotDto.HAFTPFLICHTPROVISION.HasValue)
                    {
                        NewInsurance.InsuranceResult.Provision = (decimal)angebotDto.HAFTPFLICHTPROVISION;
                        if (angebotDto.ANGKALKFSMOTORVS.HasValue)
                            NewInsurance.InsuranceResult.Motorsteuer = (decimal)angebotDto.ANGKALKFSMOTORVS;
                    }
                    else if (VSCalcFactory.Cnst_CALC_KASKO.Equals(vstemp.CODEMETHOD) && angebotDto.KASKOPROVISION.HasValue)
                    {
                        NewInsurance.InsuranceResult.Provision = (decimal)angebotDto.KASKOPROVISION;
                    }
                    else if (VSCalcFactory.Cnst_CALC_RESTSCHULD.Equals(vstemp.CODEMETHOD) && angebotDto.RESTSCHULDPROVISION.HasValue)
                    {
                        NewInsurance.InsuranceResult.Provision = (decimal)angebotDto.RESTSCHULDPROVISION;
                    }
                    else if (VSCalcFactory.Cnst_CALC_GAP.Equals(vstemp.CODEMETHOD))
                    {
                        angebotDto.ANGKALKFSGAPFLAG = 1;
                        NewInsurance.InsuranceResult.Provision = (decimal)angebotDto.GAPPROVISION;
                    }
                    NewInsurance.InsuranceResult.Gesamtfinanzierungsrate = getSlpos(LoopInsurance.SYSANGVS, context);



                    InsuranceList.Add(NewInsurance);
                }
                else _Log.Error("Error Loading Insurance SYSANGVS " + LoopInsurance.SYSANGVS + " for Angebot " + angebotDto.SYSID + " - no VSTYP in DB");

            }


            angebotDto.ANGVSPARAM = InsuranceList.ToArray();
        }

        /// <summary>
        /// NOVANEU
        /// Write new nova fields
        /// Annahme: Wir gehen von den bisherigen brutto inkl. nova inkl. ust aus und rechnen zurück
        /// </summary>
        /// <param name="ust"></param>
        /// <param name="dto"></param>
        /// <param name="angob"></param>
        /// <param name="angkalk"></param>
        /// <param name="isNew"></param>
        /// <param name="context"></param>
        private bool updatePrices(decimal ust, ANGEBOTDto dto, ANGOB angob, ANGKALK angkalk, bool isNew, DdOlExtended context, long sysperole)
        {
            try
            {
                ust = LsAddHelper.getGlobalUst(sysperole);
                decimal ust2 = ust;
                if (dto.ANGOBERINKLMWST.HasValue && dto.ANGOBERINKLMWST == 0)
                    ust2 = 0;
                
                
                
                decimal useval = 0;
                
                angob.NOVAZUABUST = 0;
                angob.NOVAZUABNOVAZU = 0;
                angob.NOVAZUAB = 0;
                
                

                DateTime smdate = DateTime.Now;
                if (angob.LIEFERUNG.HasValue)
                    smdate = angob.LIEFERUNG.Value;

                //Listenpreis vor Rabatt
                NovaType nt = new NovaType(ust2, 0, 0, 0);
                useval = 0;
                if (!angob.GRUNDBRUTTO.HasValue)
                    _Log.Error("GRUNDBRUTTO is empty!");
                else useval = angob.GRUNDBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.GRUND = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                angob.GRUNDNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.GRUNDNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);//#4164 - reverted in salzburg 29.6.
                if (nt.netto != angob.GRUNDEXKLN)
                {
                    _Log.Warn("nt.netto!=angob.GRUNDEXKLN: "+nt.netto+"!="+angob.GRUNDEXKLN);
                }
                angob.GRUNDEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);//#4170

                useval = 0;
                //Listenpreis nach rabatt
                if (!angob.GRUNDEXTERNBRUTTO.HasValue)
                    _Log.Error("GRUNDEXTERNBRUTTO is empty!");
                else useval = angob.GRUNDEXTERNBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.GRUNDEXTERNNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.GRUNDEXTERNNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.GRUNDEXTERNEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);
                angob.GRUNDEXTERNUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.ust);//#4165
                //angob.GRUNDEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                

                //Sonderzubehör
                useval = 0;
                if (!angob.SONZUBBRUTTO.HasValue)
                    ;//_Log.Error("SONZUBBRUTTO is empty!");
                else useval = angob.SONZUBBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.SONZUB = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                angob.SONZUBNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.SONZUBNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.SONZUBEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);

                //Pakete
                useval = 0;
                if (!angob.PAKETEBRUTTO.HasValue)
                    ;//_Log.Error("PAKETEBRUTTO is empty!");
                else useval = angob.PAKETEBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.PAKETE = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                angob.PAKETENOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.PAKETENOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.PAKETEEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);

                //Herstellerzubehör
                useval = 0;
                if (!angob.HERZUBBRUTTO.HasValue)
                    ;//_Log.Error("HERZUBBRUTTO is empty!");
                else useval = angob.HERZUBBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.HERZUB = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);

                useval = 0;
                if (!angob.SONZUBEXTERNBRUTTO.HasValue)
                    ;//_Log.Error("SONZUBEXTERNBRUTTO is empty!");
                else 
                    useval = angob.SONZUBEXTERNBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.SONZUBEXTERNNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.SONZUBEXTERNNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.SONZUBEXTERNEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);
                angob.SONZUBEXTERNUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.ust);
                angob.SONZUBEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                

                useval = 0;
                if (!angob.PAKETEEXTERNBRUTTO.HasValue)
                    ;//_Log.Error("PAKETEEXTERNBRUTTO is empty!");
                else useval = angob.PAKETEEXTERNBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.PAKETEEXTERNNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.PAKETEEXTERNNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.PAKETEEXTERNEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);
                angob.PAKETEEXTERNUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.ust);
                angob.PAKETEEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);

                //Rabatt netto
               /* useval = 0;
                if (!angob.AHKRABATTOBRUTTO.HasValue)
                    _Log.Error("AHKRABATTOBRUTTO is empty!");
                else useval = angob.AHKRABATTOBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.AHKRABATTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);
                */
               

                //Händlerzubehör mit akt. Steuersatz
                nt = new NovaType(ust, 0, 0, 0);
                useval = 0;
                if (!angob.ZUBEHOERBRUTTO.HasValue)
                    ;//_Log.Error("ZUBEHOERBRUTTO is empty!");
                else useval = angob.ZUBEHOERBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.ZUBEHOER = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);

                //Überführungskosten mit akt. Steuersatz
                useval = 0;
                if (!angob.UEBERFUEHRUNGBRUTTO.HasValue)
                    ;//_Log.Error("UEBERFUEHRUNGBRUTTO is empty!");
                else useval = angob.UEBERFUEHRUNGBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.UEBERFUEHRUNG = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);

                //Zulassungskosten - mit Vorsteuer
                nt = new NovaType(ust, 0, 0, 0);
                useval = 0;
                if (!angob.ZULASSUNGBRUTTO.HasValue)
                    ;//_Log.Error("ZULASSUNGBRUTTO is empty!");
                else useval = angob.ZULASSUNGBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.ZULASSUNG = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova);


                //Kaufpreis
                angob.AHKNOVAZU = angob.PAKETENOVAZU + angob.SONZUBNOVAZU + angob.GRUNDNOVAZU + angob.NOVAZUABNOVAZU;
                angob.AHKNOVA = angob.PAKETENOVA + angob.SONZUBNOVA + angob.GRUNDNOVA;
                angob.AHKEXKLN = angob.PAKETEEXKLN + angob.SONZUBEXKLN + angob.GRUNDEXKLN + angob.HERZUB + angob.ZUBEHOER;

                

                

                bool iscredit = false;

                CalculationDao calcDao = new CalculationDao(context);
                if(dto.SYSPRPRODUCT.HasValue)
                {
                    VartDTO va = calcDao.getVART(dto.SYSPRPRODUCT.Value);
                    if (va.CODE.IndexOf("KREDIT") > -1)
                        iscredit= true;
                }
                //Sonderfälle:
                angob.AHKEXTERNNOVAZU = 0;

                /*
                useval = 0;
                if (!angob.AHKEXTERNBRUTTO.HasValue)
                    _Log.Error("AHKEXTERNBRUTTO is empty!");
                else useval = angob.AHKEXTERNBRUTTO.Value;
                nt.setBruttoInklNova(useval);
                angob.AHKEXTERNNOVAZU = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.novaZuschlag);
                angob.AHKEXTERNNOVA = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nova);
                angob.AHKEXTERNEXKLN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.netto);
                angob.AHKEXTERNUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.ust);
                angob.AHKEXTERN = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.nettoInklNova-nt.getSteuervorteil());
                */
                
                //BGEXTERN
                angkalk.BGEXTERNNOVA = 0;
                //decimal mitfininsurance = getMitfinInsurance(dto, context, iscredit);
                //angkalk.BGEXTERNBRUTTO = dto.ANGOBAHKBRUTTO + mitfininsurance;
                //angkalk.BGEXTERN = dto.ANGOBAHK + mitfininsurance;

                angkalk.BGEXTERNBRUTTO = dto.ANGKALKBGEXTERNBRUTTO;
                angkalk.BGEXTERN = dto.ANGKALKBGEXTERN;
                if (angkalk.BGEXTERNBRUTTO.HasValue && angkalk.BGEXTERN.HasValue)
                    angkalk.BGEXTERNUST = angkalk.BGEXTERNBRUTTO-angkalk.BGEXTERN;

                if (dto.ANGKALKBGINTERN.HasValue)
                    angkalk.BGINTERN = dto.ANGKALKBGINTERN.Value;
                angkalk.BGEXTERNEXKLN = angkalk.BGEXTERN;
                if (iscredit)
                {
                    angkalk.BGEXTERNUST = 0;
                    angkalk.BGEXTERN = angkalk.BGEXTERNBRUTTO;
                }
                return iscredit;
            }
            catch (Exception e)
            {
                _Log.Error("Updating Price-Components failed", e);
            }
            return false;
        }

        /// <summary>
        /// Calculates the mitfin insurance, that is only for credit and only for gap/rsdv if checked
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="context"></param>
        /// <param name="iscredit"></param>
        /// <returns></returns>
        private decimal getMitfinInsurance(ANGEBOTDto dto, DdOlExtended context,bool iscredit)
        {
            if (!iscredit) return 0;
            decimal rval = 0;
             if (checkFlag(dto.ANGKALKFSGAPFLAG))
                 rval += getValueFromInsurance(context, dto, "GAP");
             if (checkFlag(dto.ANGKALKFSRSVFLAG))
                 rval += getValueFromInsurance(context, dto, "RSDV");
             return rval;
        }

        private CreatorInfoDto getCreatorInfo(long sysid, DdOlExtended context)
        {
            return context.ExecuteStoreQuery<CreatorInfoDto>("select perole.sysperole, sysperson from peuni, perole where perole.sysperole=peuni.sysperole and area='ANGEBOT' and sysid=" + sysid + " order by syspeuni", null).FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// updates/creates Mandat
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="sysid">id of angebot</param>
        /// <param name="sysls"></param>
        /// <param name="ctx"></param>
        /// <param name="_VpSysPERSON"></param>
        public static String createOrUpdateMandat(ANGEBOTDto angebotDto, long sysls, DdOlExtended ctx, long? _VpSysPERSON)
        {
            try
            {
                //nur in diesen zuständen überhaupt änderbar/speicherbar
                if (!(angebotDto.ZUSTAND == ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert) || angebotDto.ZUSTAND == ZustandHelper.GetStringFromEnumerator(AngebotZustand.Neu) || angebotDto.ZUSTAND == ZustandHelper.GetStringFromEnumerator(AngebotZustand.NeuResubmit)))
                    return null;

                //Falls es sich um eine Wiedereinreichung handelt ggf. das Wandermandat umhängen
                if (angebotDto.ANGEBOT1.IndexOf(".")>-1 && angebotDto.SYSANGEBOT.HasValue)
                {
                         
                     long sysmandat = ctx.ExecuteStoreQuery<long>("select sysmandat from mandat,vt where status in (1,2,3) and "
                          // + "(mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) "
                          + SQL.CheckCurrentSysDate ("mandat")
                          + " and area='VT' and mandat.sysid=vt.sysid and vt.sysangebot=" + angebotDto.SYSANGEBOT.Value, null).FirstOrDefault ();
                    if(sysmandat==0)
                    {
                        sysmandat = ctx.ExecuteStoreQuery<long> ("select sysmandat from mandat,antrag where status in (1,2,3) and "
                            //+ " (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) "
                            + SQL.CheckCurrentSysDate ("mandat")
                            + " and area='ANTRAG' and mandat.sysid=antrag.sysid and antrag.sysangebot=" + angebotDto.SYSANGEBOT.Value, null).FirstOrDefault ();
                    }

                    if (sysmandat > 0)//vt/antrag wandermandat umhängen
                    {
                        MANDAT wm = (from p in ctx.MANDAT
                                         where p.SYSMANDAT == sysmandat
                                         select p).FirstOrDefault();
                        wm.SYSID = angebotDto.SYSID.Value;
                        wm.AREA = "ANGEBOT";

                        ctx.SaveChanges();
                    }
                }
                //WANDERMANDAT
                MANDAT mandat = (from p in ctx.MANDAT
                                 where p.AREA.Equals("ANGEBOT") && p.SYSID == angebotDto.SYSID.Value
                                 select p).FirstOrDefault();

                bool isEinzug = angebotDto.EINZUG.HasValue &&angebotDto.EINZUG.Value==1;

                if (!angebotDto.SYSKI.HasValue || !isEinzug)//kein Mandat, da selbstzahler
                {
                    if (mandat != null)
                    {
                        mandat.STATUS = 5;//Wandermandat wird nicht mehr verwendet, da Bankverbindung geändert wurde
                        ctx.DeleteObject(mandat);
                    }
                    
                    return null;
                }

                //umstellung auf globalmandat, falls im status kalkuliert
                //für den fall einer wiedereinreichung keine umstellung auf globalmandat!
                if (angebotDto.ANGEBOT1.IndexOf(".")<0 && mandat != null && (angebotDto.ZUSTAND == ZustandHelper.GetStringFromEnumerator(AngebotZustand.Kalkuliert)))
                {
                    mandat.STATUS = 5;
                    ctx.DeleteObject(mandat);//remove old wandermandat
                    mandat = null;
                }


                BankdatenDao bdao = new BankdatenDao();
                BankdatenDto bankdaten = BankdatenDao.getBankDatenFromAngebot(angebotDto, sysls, ctx, true,true);               
                MANDAT m = bdao.createOrUpdateMandat(ctx, bankdaten, _VpSysPERSON.HasValue?_VpSysPERSON.Value:0);
                if (m != null)
                {
                    return m.REFERENZ;
                }
                /*
              

                //Verlängerungen erlauben KEINE Änderung des Mandats mehr (ausser IT-Referenz-Update)!
                mandat.LSADDReference.EntityKey = ctx.getEntityKey(typeof(LSADD), sysls);
               
                mandat.PAYART = (int)ctx.ExecuteStoreQuery<long>("select payart from it where sysit=" + angebotDto.SYSKI.Value, null).FirstOrDefault();
                mandat.PAYTYPE = 2;
                mandat.STATUS = 1;
            
                if (angebotDto.SYSIT.Value != angebotDto.SYSKI.Value)
                    mandat.SYSITDEBITOR = angebotDto.SYSIT;
                if (angebotDto.SYSKI.HasValue)
                    mandat.ITReference.EntityKey = ctx.getEntityKey(typeof(IT), angebotDto.SYSKI.Value);

                //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
                if (!mandat.PAYART.HasValue || mandat.PAYART == 0)
                {
                    mandat.PAYART = 1;
                }
                


                SICHTYP sich = SICHTYPHelper.GetSichtTyp(ctx, KONTOINHABERSICHTYP);
                ANGOBSICH sicherheit = ANGOBSICHHelper.GetAngobsischByRang(ctx, angebotDto.SYSID.Value, KONTOINHABERSICHRANG);
                bool isThirdPerson = false;
                if (angebotDto.SYSIT.Value != angebotDto.SYSKI.Value)//Schuldner ist nicht Kunde, also evtl 3. Person, dann ist eine Sicherheit nötig
                {
                    String query = "select angobsich.sysit from angobsich, sichtyp,angebot where angobsich.bezeichnung='Mitantragsteller' and angobsich.sysvt=angebot.sysid and angobsich.syssichtyp=sichtyp.syssichtyp and sichtyp.rang=10 and angebot.sysid=:sysid";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = angebotDto.SYSID });

                    List<long> mas = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).ToList();

                    if (!mas.Contains(angebotDto.SYSKI.Value))//sicherheitengeber ist 3. Person da nicht mitantragsteller
                    {
                        isThirdPerson = true;
                        //Create Sicherheit for Mandat

                        if (sicherheit == null)
                        {
                            sicherheit = new ANGOBSICH();
                            ctx.ANGOBSICH.Add(sicherheit);
                            sicherheit.BEGINN = DateTime.Now;
                        }
                        if (sicherheit != null)
                        {
                            sicherheit.RANG = KONTOINHABERSICHRANG;
                            sicherheit.BEZEICHNUNG = "Kontoinhaber";
                            sicherheit.SYSPERSON = _VpSysPERSON;
                            sicherheit.SICHTYPReference.EntityKey = ctx.getEntityKey(typeof(SICHTYP), sich.SYSSICHTYP);
                            sicherheit.SYSVT = angebotDto.SYSID.Value;
                            sicherheit.STATUS = "erhalten";
                            sicherheit.AKTIVFLAG = 1;
                            sicherheit.ITReference.EntityKey = ctx.getEntityKey(typeof(IT), angebotDto.SYSKI.Value);

                        }
                    }
                }
                if (sicherheit != null && !isThirdPerson)
                    sicherheit.AKTIVFLAG = 0;//falls schon eine Sicherheit da ist, diese deaktivieren, da nicht mehr 3. Person
                */
            }
            catch (Exception e)
            {
                _Log.Error("Error updating Mandat", e);
            }
            return null;
        }

       

        public static bool kontoChanged(long syski, long sysvorvt, MANDAT vvtMandat, DdOlExtended ctx)
        {
            //prüfung ob sich kontodaten geändert haben!
            KtoInfo ktoInfo = ctx.ExecuteStoreQuery<KtoInfo>("select trim(konto.iban) iban,konto.kontonr,blz.name bankname, blz.blz blz, blz.bic bic from konto,blz,vt where  blz.sysblz=konto.sysblz and konto.rang=vt.rangki and sysperson=vt.syskd and vt.sysid=" + sysvorvt + " order by konto.rang desc", null).FirstOrDefault();
            if (ktoInfo == null)
                ktoInfo = ctx.ExecuteStoreQuery<KtoInfo>("select trim(konto.iban) iban,konto.kontonr,blz.name bankname, blz.blz blz, blz.bic bic from konto,blz,vt where blz.sysblz=konto.sysblz and sysperson=vt.syskd and vt.sysid=" + sysvorvt + "  order by konto.rang desc", null).FirstOrDefault();
            if (ktoInfo == null)
                ktoInfo = new KtoInfo();
            KtoInfo aktKonto = ctx.ExecuteStoreQuery<KtoInfo>("select payart,bic,trim(iban) iban from it where sysit=" + syski, null).FirstOrDefault();
            if (aktKonto == null)
                aktKonto = new KtoInfo();
            int oldpayart = vvtMandat.PAYART.HasValue ? vvtMandat.PAYART.Value : 0;
            int aktpayart = aktKonto.PAYART.HasValue ? aktKonto.PAYART.Value : 0;

            if (oldpayart != aktpayart || (aktKonto.BIC != null && !aktKonto.BIC.Equals(ktoInfo.BIC)) || (aktKonto.IBAN != null && !aktKonto.IBAN.Equals(ktoInfo.IBAN)))
            {
                _Log.Debug("Not using VVT Mandate for VT:" + sysvorvt + ": IBAN/BIC/PAYART changed to original Mandate");
                return true;//neues mandat!
            }
            return false;
        }

    }
    class CreatorInfoDto
    {
        public long sysperson { get; set; }
        public long sysperole { get; set; }
    }
}
