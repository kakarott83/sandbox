// OWNER MK, 04-02-2009
namespace Cic.OpenLease.Service.DdOl
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    #endregion

    /// <summary>
    /// Datenzugriffsobjekt für Anträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public sealed partial class ANTRAGDao : Cic.OpenLease.ServiceAccess.DdOl.IANTRAGDao
    {

        #region IANTRAGDao methods
        private static CacheDictionary<String, int> searchCache = CacheFactory<String, int>.getInstance().createCache(1000*60 * 2);

      

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public ANTRAGDto Deliver(long sysId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Resolve the problem with includes
                //Devart.Data.Oracle.Entity.OracleEntityProviderServices.TypedNulls = true;

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {

                    
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANTRAG, sysId))
                        throw new Exception("No Permission to ANTRAG");
                    
                    // Query ANTRAG
                    var CurrentAntrag = (from Antrag in Context.ANTRAG
                                             //.Include("ANTOBList").Include("AntObSichList")
                                         where Antrag.SYSID == sysId
                                         select Antrag).FirstOrDefault();

                    // Check if antrag was found
                    if (CurrentAntrag == null)
                    {
                        // Throw an exception
                        throw new Exception("Could not get data from Antrag.");
                    }

                    // Check if AntObSl list was found
                    if (!Context.Entry(CurrentAntrag).Collection(f => f.ANTOBSLList).IsLoaded)
                        Context.Entry(CurrentAntrag).Collection(f => f.ANTOBSLList).Load();
                    

                    // Get antob

                    ANTOB CurrentAntOb = (from ob in Context.ANTOB
                                             where 
                                         ob.SYSANTRAG == CurrentAntrag.SYSID
                                         select ob).FirstOrDefault();
                                             
                              

                    // Get antob
                    ANTKALK CurrentAntKalk = Context.ExecuteStoreQuery<ANTKALK>("select * from antkalk where sysantrag="+CurrentAntrag.SYSID,null).FirstOrDefault();
                        
                        //CurrentAntOb.ANTKALKList;

                   
                    // Query IT
                    var CurrentPerson = (from Person in Context.PERSON
                                         where Person.SYSPERSON == CurrentAntrag.SYSKD
                                         select Person).FirstOrDefault();

                    // Create an assembler
                    ANTRAGAssembler Assembler = new ANTRAGAssembler(ServiceValidator.SysPEROLE);

                    // Conert and return the entities
                    ANTRAGDto rval = Assembler.ConvertToDto(CurrentAntrag, CurrentAntKalk, CurrentAntOb, CurrentPerson, null, Context);
                    if (CurrentAntrag.SYSVART.HasValue)
                        rval.VART = Context.ExecuteStoreQuery<String>("select bezeichnung from vart where sysvart=" + CurrentAntrag.SYSVART.Value, null).FirstOrDefault();
                    
                    rval.ZUSTAND = Context.ExecuteStoreQuery<String>(@"SELECT (SELECT extstate.zustand 
FROM attribut,attributdef,state,statedef extstate,statedef intstate,wftable
WHERE attribut.sysstate = state.sysstate
AND attribut.sysattributdef= attributdef.sysattributdef
AND attribut.sysstatedef = extstate.sysstatedef
AND state.sysstatedef =intstate.sysstatedef
AND state.syswftable = wftable.syswftable
AND wftable.syscode = 'ANTRAG' and UPPER(attributdef.attribut) = upper(antrag.attribut) and upper(intstate.zustand)=upper(antrag.zustand)) zustand from antrag where sysid="+CurrentAntrag.SYSID, null).FirstOrDefault();

                    return rval;
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AntragDeliverFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        private class ITAntragSearch
        {
            public String NAME { get; set; }
            public String VORNAME { get; set; }
            public String ORT { get; set; }
            public String PLZ { get; set; }
            public String ZUSATZ { get; set; }
        }
        private class ANTOBAntragSearch
        {
            public long SYSOB { get; set; }
            public String FABRIKAT { get; set; }
            public String HERSTELLER { get; set; }
            public long? JAHRESKM { get; set; }
        }
        private class ANTKALKAntragSearch
        {
            public decimal? BGEXTERN { get; set; }
            public decimal? DEPOT { get; set; }
            public int? LZ { get; set; }
            public decimal? RATE { get; set; }
            public decimal? RW { get; set; }
            public decimal? SZ { get; set; }
            public decimal? ANZAHLUNG { get; set; }
        }
        // TEST MK 0 BK, Not tested
        /// <summary>
        /// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// Das Gesichtsfeld kann berücksichtigt werden.
        /// Mit Benutzer Validierung (RFG.SEHEN erforderlich).
        /// MK
        /// </summary>
        /// <param name="antragSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="antragSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSortData" />.</param>
        /// <returns></returns>	
        public SearchResult<ANTRAGShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSortData[] antragSortData)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {

                // Check search data
                if (antragSearchData == null)
                {
                    // Throw exception
                    throw new ArgumentException("antragSearchData");
                }

                try
                {
                    // Check parameters
                    Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckTopParameter(searchParameters.Top);
                    Cic.OpenLease.Service.Helpers.ServiceParametersHelper.CheckSkipParameter(searchParameters.Skip);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

                try
                {
                    List<ANTRAGShortDto> ANTRAGList = null;
                    long resCount = 0;
                    SearchResult<ANTRAGShortDto> result = new SearchResult<ANTRAGShortDto>();
                    using (DdOlExtended context = new DdOlExtended())
                    {
                        FlowDao flowdao = new FlowDao(context);
                        try
                        {
                            // Get raw list
                            ANTRAGList = MyDeliverANTRAGList(context, antragSearchData, searchParameters, antragSortData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, out resCount);
                            result.results = ANTRAGList;
                            result.searchCountMax = (int)resCount;
                            result.searchCountFiltered = ANTRAGList.Count;
                        }
                        catch
                        {
                            // Throw caught exception
                            throw;
                        }

                        // Check list
                        if ((ANTRAGList == null) || (ANTRAGList.Count == 0))
                            return result;


                        // Loop through list
                        foreach (Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto LoopANTRAG in ANTRAGList)
                        {

                            LoopANTRAG.AUFLAGEN = "NEIN";
                            
                            if(LoopANTRAG.SYSANGEBOT.HasValue)
                            {
                                List<FlowDto> auflagen = flowdao.getMessages("ANG", LoopANTRAG.SYSANGEBOT.Value);
                                if(auflagen!=null&& auflagen.Count>0)
                                    LoopANTRAG.AUFLAGEN = "JA";
                            }

                            // Check if Antrag is linked to Bonitaet and BoniposList is not null
                          /*  string query = "select count(*) from bonitaet,bonipos where bonipos.sysbonitaet=bonitaet.sysbonitaet and sysantrag=" + LoopANTRAG.SYSID;
                            long hasAuflage = context.ExecuteStoreQuery<long>(query, null).FirstOrDefault();
                            if (hasAuflage > 0)
                                LoopANTRAG.AUFLAGEN = "JA";
                            else
                                LoopANTRAG.AUFLAGEN = "NEIN";*/

                            if (LoopANTRAG.SYSKD>0)
                            {
                                ITAntragSearch itinfo = context.ExecuteStoreQuery<ITAntragSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.person where sysperson=" + LoopANTRAG.SYSKD, null).FirstOrDefault();
                                if (itinfo != null)
                                {
                                    LoopANTRAG.PERSONNAME = itinfo.NAME;
                                    LoopANTRAG.PERSONVORNAME = itinfo.VORNAME;
                                    LoopANTRAG.PERSONORT = itinfo.ORT;
                                    LoopANTRAG.PERSONPLZ = itinfo.PLZ;
                                    LoopANTRAG.PERSONZUSATZ = itinfo.ZUSATZ;
                                }
                            }else
                            {
                                ITAntragSearch itinfo = context.ExecuteStoreQuery<ITAntragSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.it where sysit=" + LoopANTRAG.SYSIT, null).FirstOrDefault();
                                if (itinfo != null)
                                {
                                    LoopANTRAG.PERSONNAME = itinfo.NAME;
                                    LoopANTRAG.PERSONVORNAME = itinfo.VORNAME;
                                    LoopANTRAG.PERSONORT = itinfo.ORT;
                                    LoopANTRAG.PERSONPLZ = itinfo.PLZ;
                                    LoopANTRAG.PERSONZUSATZ = itinfo.ZUSATZ;
                                }
                            }
                            if (LoopANTRAG.SYSVK != null)
                            {
                                ITAntragSearch itinfo = context.ExecuteStoreQuery<ITAntragSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.person where sysperson=" + LoopANTRAG.SYSVK, null).FirstOrDefault();
                                if (itinfo != null)
                                {
                                    LoopANTRAG.VERKAEUFERNAME = itinfo.NAME;
                                    LoopANTRAG.VERKAEUFERVORNAME = itinfo.VORNAME;

                                }
                            }
                            else if (LoopANTRAG.SYSBERATADDB != null)
                            {
                                ITAntragSearch itinfo = context.ExecuteStoreQuery<ITAntragSearch>("Select NAME, VORNAME, ORT, PLZ, ZUSATZ from cic.person where sysperson=" + LoopANTRAG.SYSBERATADDB, null).FirstOrDefault();
                                if (itinfo != null)
                                {
                                    LoopANTRAG.VERKAEUFERNAME = itinfo.NAME;
                                    LoopANTRAG.VERKAEUFERVORNAME = itinfo.VORNAME;

                                }
                            }

                            //-----------------------------
                            //SYSID, VERTRAG, HAUPTVERTRAG, ANTRAG AS ANTRAG1, ANGEBOT, SYSAGB, SYSLS, SYSVT, SYSANTRAG, SYSANGEBOT, SYSFANK, SYSDS, ZUSTAND, ZUSTANDAM, ERFASSUNG, BEARBEITUNG, BENUTZER, OK, AKTIVKZ, ENDEKZ, ENDEAM, FINKZ, AKTIV, SYSADM, SYSBERATADDA, SYSBERATADDB, SYSKD, SYSVK, SYSBN, RANGBN, SYSKI, RANGKI, SYSGA, SYSLF, SYSRA, SYSUN, SYSVS, STANDORT, INVENTAR, FABRIKAT, OBJEKTVT, OBJEKTKZ, SERIE, LFORM, NUTZUNG, VART, VTYP, KONSTELLATION, FFORM, FSATZ, BRUTTOKREDIT, GRUND, GESAMT, BGEXTERN, AHK, BGINTERN, DISAGIO, SZ, SZ2, PROVISION, GEBUEHR, RSV, RSV2, RW, RWHIST, RWKALK, DB, ZINS, ZINSEFF, BASISZINS, LZ, RLZ, GLZ, ALZ, MLZ, PPY, MODUS, RATE, EXTNOM, EXTPANGV, EXTEFF, EXTNOM2, INTZINS1, INTZINS2, INTZINS3, INTZINS4, REFIZINS1, REFIZINS2, BWMARGE1, BWMARGEP1, BWMARGE2, BWMARGEP2, BEGINN, ENDE, ERSTERATE, LETZTERATE, LTSOLL, STAND, UEBERNAHME, RUECKGABE, VALUTATAG, FINSATZ, ZAHLBONITAET, LOCKED, VALUTATAGE, SYSVART, STRUKTUR, ABTRETUNG, ABTRETUNGVON, VERWDRITTER, VERWDRITTERBIND, VERWLIEF, VERWLIEFWERT, VERWRCKNAHME, VERTRIEBSWEG, ZUSENDUNG, STOPFAKT, DATANGEBOT, DATABSCHLUSS, DATEINREICHUNG, DATAKTIV, SYSVPFIL, VPANGEBOT, VPVERTRAG, VPERTRERW, VPEINVERG, SYSBERATER, SYSRVT, DATKUENDIGUNG, REDUZIERUNG, ABLOESE, SCHLUSSZAHL, SYSVTTYP, SYSMWST, SYSWAEHRUNG, SYSRNWAEHRUNG, BONUSFLAG, NETTINGFLAG, SPERRE, SYSKOSTSTEL, SYSKOSTTRAE, SYSKGRUPPE, EINZUG, ZAHLSPERRE, AUSZAHLSPERRE, AUSZAHLSPERREBIS, MAHNSPERRE, MSPERREBIS, ERINKLMWST, FAKTURIERUNG, BLOCKRATE, ESRFLAG, IBORSATZ, IBORAUFSCHLAG, IBORFRIST, IBORBEZUG, SYSIBOR, AKTOBLIGO, AKTOPOS, SYSPARTNER, RWREDUZ, LZVERL, SYSEVL, ZINSCUST, ZINSCUST2, SYSVARTTAB, SYSGST, ZINSCUST3, MANFEE, ANZAHLOB, SYSFBN, DIFFERENZ, DIFFERENZP, EXTRATINGCODE, BESCHRDEUTSCH, BESCHRENGLISCH, ZEKGESUCHSID, ATTRIBUT, ATTRIBUTAM, SYSIT, PTYP, SYSWFUSER, SYSPEROLCONT, SYSPEROLSIGN, SYSPEROLSIGN2, SYSADRESSEP, SYSADRESSEL, SYSLANGOFFER, CHECKEDON, LOCKEDON, RWDUEON, RWBASE, RWCRV, SYSBRAND, SYSPRPRODUCT 
                            //SYSID, DATANGEBOT, ANTRAG AS ANTRAG1, SYSKD, VART, ZUSTAND, SYSBERATADDB, SYSIT
                            LoopANTRAG.KDTYPNAME = context.ExecuteStoreQuery<String>("select kdtyp.name from kdtyp,it where kdtyp.syskdtyp=it.syskdtyp and it.sysit = " + LoopANTRAG.SYSIT, null).FirstOrDefault();


                            ANTOBAntragSearch antob = context.ExecuteStoreQuery<ANTOBAntragSearch>("Select SYSOB, BEZEICHNUNG FABRIKAT, HERSTELLER, JAHRESKM FROM ANTOB where sysantrag=" + LoopANTRAG.SYSID, null).FirstOrDefault();
                            if (antob != null)
                            {
                                LoopANTRAG.ANTOBFABRIKAT = antob.FABRIKAT;
                                LoopANTRAG.ANTOBHERSTELLER = antob.HERSTELLER;
                                LoopANTRAG.ANTOBJAHRESKM = antob.JAHRESKM;
                                ANTKALKAntragSearch antkalk = context.ExecuteStoreQuery<ANTKALKAntragSearch>("Select BGEXTERN,DEPOT,LZ,RATEBRUTTO RATE,RW,SZBRUTTO SZ,ANZAHLUNG FROM ANTKALK where sysob=" + antob.SYSOB, null).FirstOrDefault();
                                if (antkalk != null)
                                {
                                    LoopANTRAG.ANTKALKBGEXTERN = antkalk.BGEXTERN;
                                    LoopANTRAG.ANTKALKDEPOT = antkalk.DEPOT;
                                    LoopANTRAG.ANTKALKLZ = antkalk.LZ;
                                    LoopANTRAG.ANTKALKRATE = antkalk.RATE;
                                    LoopANTRAG.ANTKALKRW = antkalk.RW;
                                    LoopANTRAG.ANTKALKSZ = antkalk.SZ.HasValue && antkalk.SZ.Value > 0 ? antkalk.SZ : antkalk.ANZAHLUNG;
                                    if (!LoopANTRAG.ANTKALKSZ.HasValue)
                                        LoopANTRAG.ANTKALKSZ = 0;
                                }
                            }

                        }

                    }
                    return result;
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }

            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.AntragSearchFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }



        public ApprovalDto DeliverApproval(long sysAntrag)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Query APPROVAL
                    var CurrentApproval = (from Approval in Context.APPROVAL
                                           where Approval.ANTRAG.SYSID == sysAntrag
                                           select Approval).FirstOrDefault();

                    // Check if the approval was found
                    if (CurrentApproval == null)
                    {
                        // Throw an exception
                        throw new Exception("Sepcified approval could not be found");
                    }

                    // Return the entity converted to DTO
                    return null;// ApprovalAssembler.ConvertToDto(CurrentApproval);
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverApprovalFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }
        public BonitaetDto[] DeliverBonitaet(long sysAntrag)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANTRAG, sysAntrag))
                        throw new Exception("No Permission to ANTRAG");

                    // Query BONITAET
                    var BoniTaets = from BoniTaet in Context.BONITAET.Include("BONIPOSList")
                                    where BoniTaet.ANTRAG.SYSID == sysAntrag
                                    select BoniTaet;

                    // Create BoniTaet list
                    List<BonitaetDto> BoniTaetList = new List<BonitaetDto>();

                    // Create an assembler
                    /*BoniTaetAssembler Assembler = new BoniTaetAssembler();

                    // Loop through all rows
                    foreach (var LoopBoniTaet in BoniTaets)
                    {
                        // Add BoniTaet to the list
                        BoniTaetList.Add(Assembler.ConvertToDto(LoopBoniTaet));
                    }
                    */
                    // Return the list
                    return BoniTaetList.ToArray();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBonitaetFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }
        private class UnterlagenResult
        {
            public String BEZEICHNUNG { get; set; }
            public String BESCHREIBUNG { get; set; }
            public long RANG { get; set; }
        }

        #endregion

        #region My methods

        private static string MyCreateQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData, long? sysPERSONInPEROLE, long? sysPUSER, bool count)
        {
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();

            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check search data
            if (antragSearchData == null)
            {
                // Throw exception
                throw new ArgumentException("antragSearchData");
            }
            if (count)
            {
                QueryBuilder.Append("SELECT /*+ RULE */ COUNT(*)  FROM CIC.ANTRAG WHERE 1 = 1 ");
            }
            else
            {
               // QueryBuilder.Append("SELECT /*+ RULE */ SYSID, VERTRAG, HAUPTVERTRAG, ANTRAG AS ANTRAG1, ANGEBOT, SYSAGB, SYSLS, SYSVT, SYSANTRAG, SYSANGEBOT, SYSFANK, SYSDS, ZUSTAND, ZUSTANDAM, ERFASSUNG, BEARBEITUNG, BENUTZER, OK, AKTIVKZ, ENDEKZ, ENDEAM, FINKZ, AKTIV, SYSADM, SYSBERATADDA, SYSBERATADDB, SYSKD, SYSVK, SYSBN, RANGBN, SYSKI, RANGKI, SYSGA, SYSLF, SYSRA, SYSUN, SYSVS, STANDORT, INVENTAR, FABRIKAT, OBJEKTVT, OBJEKTKZ, SERIE, LFORM, NUTZUNG, VART, VTYP, KONSTELLATION, FFORM, FSATZ, BRUTTOKREDIT, GRUND, GESAMT, BGEXTERN, AHK, BGINTERN, DISAGIO, SZ, SZ2, PROVISION, GEBUEHR, RSV, RSV2, RW, RWHIST, RWKALK, DB, ZINS, ZINSEFF, BASISZINS, LZ, RLZ, GLZ, ALZ, MLZ, PPY, MODUS, RATE, EXTNOM, EXTPANGV, EXTEFF, EXTNOM2, INTZINS1, INTZINS2, INTZINS3, INTZINS4, REFIZINS1, REFIZINS2, BWMARGE1, BWMARGEP1, BWMARGE2, BWMARGEP2, BEGINN, ENDE, ERSTERATE, LETZTERATE, LTSOLL, STAND, UEBERNAHME, RUECKGABE, VALUTATAG, FINSATZ, ZAHLBONITAET, LOCKED, VALUTATAGE, SYSVART, STRUKTUR, ABTRETUNG, ABTRETUNGVON, VERWDRITTER, VERWDRITTERBIND, VERWLIEF, VERWLIEFWERT, VERWRCKNAHME, VERTRIEBSWEG, ZUSENDUNG, STOPFAKT, DATANGEBOT, DATABSCHLUSS, DATEINREICHUNG, DATAKTIV, SYSVPFIL, VPANGEBOT, VPVERTRAG, VPERTRERW, VPEINVERG, SYSBERATER, SYSRVT, DATKUENDIGUNG, REDUZIERUNG, ABLOESE, SCHLUSSZAHL, SYSVTTYP, SYSMWST, SYSWAEHRUNG, SYSRNWAEHRUNG, BONUSFLAG, NETTINGFLAG, SPERRE, SYSKOSTSTEL, SYSKOSTTRAE, SYSKGRUPPE, EINZUG, ZAHLSPERRE, AUSZAHLSPERRE, AUSZAHLSPERREBIS, MAHNSPERRE, MSPERREBIS, ERINKLMWST, FAKTURIERUNG, BLOCKRATE, ESRFLAG, IBORSATZ, IBORAUFSCHLAG, IBORFRIST, IBORBEZUG, SYSIBOR, AKTOBLIGO, AKTOPOS, SYSPARTNER, RWREDUZ, LZVERL, SYSEVL, ZINSCUST, ZINSCUST2, SYSVARTTAB, SYSGST, ZINSCUST3, MANFEE, ANZAHLOB, SYSFBN, DIFFERENZ, DIFFERENZP, EXTRATINGCODE, BESCHRDEUTSCH, BESCHRENGLISCH, ZEKGESUCHSID, ATTRIBUT, ATTRIBUTAM, SYSIT, PTYP, SYSWFUSER, SYSPEROLCONT, SYSPEROLSIGN, SYSPEROLSIGN2, SYSADRESSEP, SYSADRESSEL, SYSLANGOFFER, CHECKEDON, LOCKEDON, RWDUEON, RWBASE, RWCRV, SYSBRAND, SYSPRPRODUCT  FROM CIC.ANTRAG WHERE 1 = 1 ");
                //QueryBuilder.Append("SELECT /*+ RULE */SYSID, DATANGEBOT, ANTRAG AS ANTRAG1, SYSKD, VART, ZUSTAND, SYSBERATADDB, SYSIT  FROM CIC.ANTRAG WHERE 1 = 1 ");
                QueryBuilder.Append("SELECT SYSID FROM CIC.ANTRAG WHERE 1 = 1 ");
                
            }
            // SysKD ! Always
            if ((antragSearchData.SYSKD.HasValue) && (antragSearchData.SYSKD.Value > 0))
            {
                // Where
                QueryBuilder.Append("AND SYSKD = :pSYSKD ");
                //Query = Query.Where(antrag => antrag.PERSON.SYSPERSON == antragSearchData.SYSKD.Value);
            }

            // ANTRAGCode
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.ANTRAG1))
            {
                // Where
                QueryBuilder.Append("AND UPPER(ANTRAG) LIKE UPPER(:pANTRAG1) ");
            }


            // SYSIT
            if ((antragSearchData.SYSIT.HasValue) && (antragSearchData.SYSIT.Value > 0))
            {
                // Where

                QueryBuilder.Append("AND SYSKD IN (SELECT DISTINCT SYSPERSON FROM IT WHERE SYSIT=:pSYSIT) ");
            }

            // CustomerName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.Person))
            {
                // Where
                QueryBuilder.Append("AND (SYSKD IN(SELECT SYSPERSON FROM CIC.PERSON WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pPERSON))) ");
                QueryBuilder.Append("or SYSIT IN(SELECT sysit FROM CIC.IT WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pPERSON2))) )");
            }

            // VerkauferName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.Verkaufer))
            {
                // Where
                QueryBuilder.Append("AND SYSVK IN(SELECT SYSPERSON FROM CIC.PERSON WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pVERKAUFER))) ");
            }

            if (sysPERSONInPEROLE.HasValue)
            {
                // Narrow
                QueryBuilder.Append("AND SYSID IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'ANTRAG',sysdate))) ");
                //QueryBuilder.Append("AND exists (select cic.CIC_PEROLE_UTILS.ChkObjInPEUNI(" + sysPUSER + ", 'ANTRAG', SYSID) from dual) ");
                
            }

            //Get filter
            string Filter;
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "ANTRAG", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                QueryBuilder.Append("AND " + Filter);
            }

            // Return
            return QueryBuilder.ToString();
        }
        private static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(1000*30);
        private static CacheDictionary<String, List<long>> resultIdCache = CacheFactory<String, List<long>>.getInstance().createCache(1000*30);
        private System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto> MyDeliverANTRAGList(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSortData[] antragSortData, long? sysPERSONInPEROLE, long? sysPUSER, out long count)
        {
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto> ANTRAGList;
            string Query;
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();
            // Check context
            if (context == null)
            {
                // Throw exception
                throw new ArgumentException("context");
            }

            // Check search data
            if (antragSearchData == null)
            {
                throw new ArgumentException("antragSearchData");
            }

            // Create new list
            ANTRAGList = new System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto>();
            object[] Parameters = MyDeliverQueryParameters(antragSearchData);

            //GET Created Query
            Query = MyCreateQuery(context, antragSearchData, sysPERSONInPEROLE, sysPUSER, false);

            QueryBuilder.Append(Query);

            StringBuilder sortBuilder = new StringBuilder();
            if (antragSortData != null && antragSortData.Length > 0)
            {
                // Order
                int i = 0;
                sortBuilder.Append(" ORDER BY ");

                foreach (Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSortData antragSortDataLoop in antragSortData)
                {
                    String sortField = antragSortDataLoop.SortDataConstant.ToString();
                   
                    sortBuilder.Append(sortField + " " + antragSortDataLoop.SortOrderConstant.ToString());
                    if (i != antragSortData.Length - 1)
                    {
                        sortBuilder.Append(", ");
                    }
                    i++;
                }
            }
            else
            {
                // Default Order
                sortBuilder.Append(" ORDER BY AKTIVKZ, DATAKTIV, ANTRAG1");
            }
            QueryBuilder.Append(sortBuilder.ToString());
            String atQuery = QueryBuilder.ToString();

           


            bool superOptimizedSearchEnabled = true;
            if (superOptimizedSearchEnabled)
            {
                //optimization-idea
                //when unknown query:
                //  now outer rows-query/conditions
                //  fetch only primary key
                //  generate a cached id-list for this query
                //always:
                //  fetch the section to use from the id-list by given parameters skip and pageSize
                //  use inner query with joins and just one search-condition: primary-key in in(<section-ids>) for the detail-fetching of all fields
                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                String resultIdCacheKey = getParamKey(Parameters, "IT_" + sysPUSER + sortBuilder.ToString());

                if (!resultIdCache.ContainsKey(resultIdCacheKey))
                {
                    _Log.Debug("ANTRAG-Search-SQL: " + atQuery);
                    resultIdCache[resultIdCacheKey] = context.ExecuteStoreQuery<long>(atQuery, Parameters).ToList();
                }
                List<long> ids = resultIdCache[resultIdCacheKey];


                count = ids.Count;

                //avoid range-fetching errors:
                int useSize = searchParameters.Top;
                if (searchParameters.Skip > ids.Count)
                {
                    searchParameters.Skip = ids.Count - 1 - useSize;

                }
                if (searchParameters.Skip < 0) searchParameters.Skip = 0;
                if (searchParameters.Skip + useSize >= ids.Count)
                {
                    useSize = ids.Count - searchParameters.Skip;
                }
                ids = ids.GetRange(searchParameters.Skip, useSize);
                //avoid error on empty list
                if (ids.Count == 0)
                    ids.Add(-1);
                //build the simple id fech query

                atQuery = @"SELECT SYSID, SYSANGEBOT, DATANGEBOT, ANTRAG AS ANTRAG1, SYSKD, VART.BEZEICHNUNG VART, (SELECT extstate.zustand 
FROM attribut,attributdef,state,statedef extstate,statedef intstate,wftable
WHERE attribut.sysstate = state.sysstate
AND attribut.sysattributdef= attributdef.sysattributdef
AND attribut.sysstatedef = extstate.sysstatedef
AND state.sysstatedef =intstate.sysstatedef
AND state.syswftable = wftable.syswftable
AND wftable.syscode = 'ANTRAG' and UPPER(attributdef.attribut) = upper(antrag.attribut) and upper(intstate.zustand)=upper(antrag.zustand))  ZUSTAND, SYSBERATADDB, SYSVK, SYSIT FROM CIC.ANTRAG,CIC.VART WHERE antrag.sysvart=vart.sysvart and sysid in (" + string.Join(",", ids) + ") " + sortBuilder.ToString();
                _Log.Debug("AT-Search-SQL: " + atQuery);
                ANTRAGList = context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto>(atQuery, null).ToList();
                _Log.Debug("AT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                return ANTRAGList;
            }
            // Check top
            if ((searchParameters != null) && (searchParameters.Top > 0))
            {
                Query = "SELECT /*+ RULE */ * FROM (SELECT rownum rnum, a.* FROM(" + QueryBuilder.ToString() + ") a WHERE rownum <= " + (searchParameters.Skip + searchParameters.Top) + ") WHERE rnum > " + searchParameters.Skip;
            }
            _Log.Debug("Antrag-Search Query: " + Query);

            try
            {
                // Create list

                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;

                int resultCount = 0;
                String countKey = getParamKey(Parameters, Query);
                // count when query changed in the last 2 minutes
                if (!searchCache.ContainsKey(countKey))
                {
                    String countQuery = MyCreateQuery(context, antragSearchData, sysPERSONInPEROLE, sysPUSER, true);
                    resultCount = context.ExecuteStoreQuery<int>(countQuery, Parameters).FirstOrDefault();
                    Parameters = MyDeliverQueryParameters(antragSearchData);
                }
                else
                    resultCount = searchCache[countKey];

                ANTRAGList = context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto>(Query, Parameters).ToList<Cic.OpenLease.ServiceAccess.DdOl.ANTRAGShortDto>();
                _Log.Debug("Antrag-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                searchCache[countKey] = resultCount; //touch the cache, another cache lifetime duration for no fetching the count
                count = resultCount;
            }
            catch
            {
                throw;
            }

            // Return
            return ANTRAGList;
        }
        private static String getParamKey(object[] par, String prefix)
        {
            StringBuilder sb = new StringBuilder(prefix);
            sb.Append(": ");
            foreach (object o in par)
                sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
            return sb.ToString();
        }

        private long? MyDeliverSYSKDFromSYSID(DdOlExtended context, long SYSID)
        {
            //GET SYSKD FOR ANTRAG
            string Query = "SELECT SYSKD FROM CIC.ANTRAG WHERE SYSID = " + SYSID;
            return context.ExecuteStoreQuery<long>(Query, null).FirstOrDefault<long>();
        }

        private long? MyDeliverSYSITFromSYSID(DdOlExtended context, long SYSID)
        {
            //GET SYSKD FOR ANTRAG
            string Query = "SELECT SYSIT FROM CIC.ANTRAG WHERE SYSID = " + SYSID;
            return context.ExecuteStoreQuery<long>(Query, null).FirstOrDefault<long>();
        }

        private object[] MyDeliverQueryParameters(Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData)
        {

            System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter> ParametersList = new System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter>();
            // SysKD ! Always
            if ((antragSearchData.SYSKD.HasValue) && (antragSearchData.SYSKD.Value > 0))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSKD", Value = antragSearchData.SYSKD + "%" });
            }

            // ANTRAGCode
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.ANTRAG1))
            {
                // Where
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pANTRAG1", Value = "%" + antragSearchData.ANTRAG1.Trim() + "%" });
            }

            // CustomerName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.Person))
            {
                // Where
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPERSON", Value = "%" + (antragSearchData.Person.Trim()).Replace(" ", "%") + "%" });
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPERSON2", Value = "%" + (antragSearchData.Person.Trim()).Replace(" ", "%") + "%" });
            }

            // VERKAUFERName
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(antragSearchData.Verkaufer))
            {
                // Where
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pVERKAUFER", Value = "%" + (antragSearchData.Verkaufer.Trim()).Replace(" ", "%") + "%" });
            }

            // SYSIT
            if ((antragSearchData.SYSIT.HasValue) && (antragSearchData.SYSIT.Value > 0))
            {
                // Where
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSIT", Value = antragSearchData.SYSIT });
            }

            return ParametersList.ToArray();
        }


        private KDTYP MyDeliverSYSKDTYP(DdOlExtended context, long? SYSID)
        {
            //Get SYSKD FROM SYSID

            KDTYP result = null;
            var query =
                from kdtyp in context.KDTYP
                where kdtyp.SYSKDTYP ==
                (
                 from j in context.IT
                 where j.SYSIT == SYSID
                 select j.SYSKDTYP
                 ).FirstOrDefault()
                select kdtyp;

            try
            {
                result = query.FirstOrDefault<KDTYP>();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not get KDTYP in Antrag", e); ;
            }

            return result;
        }


        #endregion
    }
}
