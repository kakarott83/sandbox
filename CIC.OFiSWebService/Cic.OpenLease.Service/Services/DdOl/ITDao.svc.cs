// OWNER JJ, 06-06-2009
namespace Cic.OpenLease.Service.DdOl
{
    #region Using
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public sealed partial class ITDao : Cic.OpenLease.ServiceAccess.DdOl.IITDao
    {
        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        #endregion
        private static CacheDictionary<String, int> searchCache = CacheFactory<String, int>.getInstance().createCache(1000 * 60 * 2);

        #region IITDao Members
        public SearchResult<ITShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ITSortData[] itSortDatas)
        {

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {


                if (itSearchData == null)
                {
                    throw new ArgumentException("itSearchData");
                }

                List<ITShortDto> ITList = null;
                long resCount = 0;
                SearchResult<ITShortDto> result = new SearchResult<ITShortDto>();

                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Get raw list
                    ITList = MyDeliverITList(Context, itSearchData, searchParameters, itSortDatas, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, out resCount);
                    result.results = ITList;
                    result.searchCountMax = (int)resCount;
                    result.searchCountFiltered = ITList.Count;

                    // Check list
                    if ((ITList == null) || (ITList.Count == 0))
                        return result;


                    //Get filter
                    string Filter=null;
                    Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "VT", string.Empty);
                    

                    if (Filter != null && Filter.Length > 0)
                    {
                        Filter = " AND " + Filter;
                    }
                    if (Filter == null)
                        Filter = "";
                    string Filtera = null;

                    Filtera = AppConfig.getValueFromDb("B2B", "FILTERS", "ANTRAG", string.Empty);

                    if (Filtera != null && Filtera.Length > 0)
                    {
                        Filtera = " AND " + Filtera;
                    }
                    if (Filtera == null)
                        Filtera = "";
                    foreach (ITShortDto ITLoop in ITList)
                    {


                        ITLoop.GEBDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(ITLoop.GEBDATUM);
                        ITLoop.GRUENDUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(ITLoop.GRUENDUNG);

                        // Get angebot count
                        string query3 = "SELECT count(*) FROM(";
                        query3 += "select angebot.SYSID from angebot left outer join antrag on antrag.SYSANGEBOT=angebot.SYSID where angebot.SYSIT=" + ITLoop.SYSIT + " and antrag.sysid is null";
                        query3 += " and angebot.SYSID in (SELECT sysid FROM peuni, perolecache WHERE area = 'ANGEBOT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") ";
                        query3 += " ORDER BY ANGEBOT.AKTIVKZ, ANGEBOT.DATAKTIV, ANGEBOT.ANGEBOT";
                        query3 += ") WHERE rownum <=100";

                        ITLoop.ANGEBOTCount = Context.ExecuteStoreQuery<int>(query3, null).FirstOrDefault<int>();
                        
                        

                        if (ITLoop.SYSPERSON.HasValue && ITLoop.SYSPERSON.Value > 0)
                        {
                            // Get antrag count
                            ITLoop.ANTRAGCount = Context.ExecuteStoreQuery<int>("select count(*) from antrag where syskd = " + ITLoop.SYSPERSON + " and CIC.CIC_PEROLE_UTILS.ChkObjInPEUNI( " + ServiceValidator.SYSPUSER + @", 'ANTRAG', antrag.sysid,sysdate) = 1 " + Filtera, null).FirstOrDefault<int>();

                            // Get vt count
                           // ITLoop.VTCount = Context.ExecuteStoreQuery<int>("select count(*) from vt where syskd = " + ITLoop.SYSPERSON + " and CIC.CIC_PEROLE_UTILS.ChkObjInPEUNI( " + ServiceValidator.SYSPUSER + @", 'VT', vt.sysid,sysdate) = 1", null).FirstOrDefault<int>();

                            if (ITLoop.SYSPERSON.HasValue)
                            {
                                ITLoop.VTCount = Context.ExecuteStoreQuery<int>("select count(*) from vt where SYSKD = " + ITLoop.SYSPERSON.Value + " and sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") "+Filter, null).FirstOrDefault<int>();

                                ITLoop.EOTCount = 0;// Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='EOT' and vt.syskd=" + ITLoop.SYSPERSON.Value, null).FirstOrDefault<int>();
                                //ITLoop.CAMPCount = Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='CAMP' and camp.status>1 and 3>camp.status and vt.syskd=" + ITLoop.SYSPERSON.Value, null).FirstOrDefault<int>();
                            }
                        }

                    }
                }


                return result;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.ItSearchFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ItSearchFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }

        }


        public Cic.OpenLease.ServiceAccess.DdOl.ITDto Deliver(long sysIT, long sysANGEBOT)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                Cic.OpenLease.Service.ITAssembler ITAssembler;
                Cic.OpenLease.ServiceAccess.DdOl.ITDto ITDto = null;
                //IT IT = null;
                double start1 = DateTime.Now.TimeOfDay.TotalMilliseconds;

                using (DdOlExtended Context = new DdOlExtended())
                {
                    DbConnection con = (Context.Database.Connection);
                    //IT = Context.ExecuteStoreQuery<IT>("select SYSIT,SYSPERSON,SYSLAND,SYSSTAAT,SYSLANDNAT,SYSCTLANG,SYSBRANCHE,SYSLANDAG,SYSKDTYP,PRIVATFLAG,ANREDE,TITEL,SUFFIX,VORNAME,NAME,ZUSATZ,GESCHLECHT,GEBDATUM,RECHTSFORM,GRUENDUNG,UIDNUMMER,HREGISTER,STRASSE,HSNR,PLZ,ORT,AUSWEISART,AUSWEISNR,AUSWEISBEHOERDE,AUSWEISORT,AUSWEISDATUM,AUSWEISABLAUF,LEGITDATUM,LEGITABNEHMER,SVNR,TELEFON,PTELEFON,HANDY,FAX,EMAIL,ERREICHBTREL,KONTOINHABER,BLZ,KONTONR,BANKNAME,IBAN,BIC,ANREDEKONT,TITELKONT,VORNAMEKONT,NAMEKONT,TELEFONKONT,EMAILKONT,FAMILIENSTAND,KINDERIMHAUS,WOHNUNGART,WEHRDIENST,EINKNETTO,NEBENEINKNETTO,ZEINKNETTO,SONSTVERM,ARTSONSTVERM,MIETE,AUSLAGEN,KREDRATE1,UNTERHALT,MIETNEBEN,BERUF,BESCHSEITAG,BESCHBISAG,NAMEAG,STRASSEAG,PLZAG,ORTAG,NAMEAG1,BESCHSEITAG1,BESCHBISAG1,NAMEAG2,BESCHSEITAG2,BESCHBISAG2,NAMEAG3,BESCHSEITAG3,BESCHBISAG3,MELDEDATUM,AHBEWILLIGUNG,AHBEWILLIGUNGBIS,AHGUELTIG,AHBEWILLDURCH,ABBEWILLIGUNG,ABBEWILLIGUNGBIS,ABGUELTIG,ABBEWILLDURCH,URL,SUFFIXKONT,WBEGUENST,USTABZUG,KUNDENGRUPPE from it where sysit=" + sysIT, null).FirstOrDefault();
                    //IT = Context.SelectById<IT>(sysIT);

                    IT IT = con.Query<IT>("select * from it where sysit=:sysIT", new { sysIT = sysIT }).FirstOrDefault(); 
                    _Log.Debug("Tracing IT Load: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start1));
                    start1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    // Check sight field
                    if (IT != null)
                    {
                        if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, sysIT))
                        {
                            // Not exists on the list
                            IT = null;
                        }
                    }
                    //now done by call to updateItBankdaten upon assigning kunde to angebot in GUI!
                    /* BLZInfo ibanInfo = Context.ExecuteStoreQuery<BLZInfo>("select konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,it where blz.sysblz=konto.sysblz and konto.sysperson=it.sysperson and  it.sysit=" + sysIT + " and  (konto.rang<90 or konto.rang>100) and konto.rang<900  order by konto.rang desc", null).FirstOrDefault();
                     if (ibanInfo != null && ibanInfo.IBAN != null && ibanInfo.IBAN.Length > 4 && (IT.IBAN==null||IT.IBAN.Length<4))
                     {
                         IT.BLZ = ibanInfo.BLZ;
                         IT.BIC = ibanInfo.BIC;
                         IT.IBAN = ibanInfo.IBAN;
                         IT.BANKNAME = ibanInfo.NAME;
                         IT.KONTONR = ibanInfo.KONTONR;
                     }*/


                    _Log.Debug("Tracing IT PEUNI: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start1));

                    start1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    if (IT != null)
                    {
                        // New assembler
                        ITAssembler = new Cic.OpenLease.Service.ITAssembler(ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER);

                        // Create dto
                        ITDto = ITAssembler.ConvertToDto(IT);


                        //Read compliance-Data
                        ITDto itcomp = null;
                        if (ITDto.SYSKDTYP.HasValue && ITDto.SYSKDTYP.Value == 3)
                        {
                           // itcomp = Context.ExecuteStoreQuery<ITDto>("select compliance.bezeichnung, compliance.flagaktiv,compliance.beginn,compliance.ende,compliance.sysland from compliance,ITUKZ where compliance.area='ITUKZ' and compliance.sysid=ITUKZ.SYSITUKZ and ITUKZ.SYSIT=" + sysIT + " order by sysitukz desc").FirstOrDefault();
                            if (itcomp == null)
                            {
                                itcomp = con.Query < ITDto >("select compliance.bezeichnung AMTBEZ, compliance.flagaktiv,compliance.beginn AMTSEIT,compliance.ende AMTBIS,compliance.sysland AMTLAND from compliance where compliance.area='IT' and compliance.sysid=" + sysIT).FirstOrDefault();
                            }
                        }
                        else
                        {
                            //itcomp = Context.ExecuteStoreQuery<ITDto>("select compliance.bezeichnung, compliance.flagaktiv,compliance.beginn,compliance.ende,compliance.sysland from compliance,ITPKZ where compliance.area='ITPKZ' and compliance.sysid=ITPKZ.SYSITPKZ and ITPKZ.SYSIT=" + sysIT + " order by sysitukz desc").FirstOrDefault();
                            if (itcomp == null)
                            {
                                itcomp = con.Query<ITDto>("select compliance.bezeichnung AMTBEZ, compliance.flagaktiv,compliance.beginn AMTSEIT,compliance.ende AMTBIS,compliance.sysland AMTLAND from compliance where compliance.area='IT' and compliance.sysid=" + sysIT).FirstOrDefault();
                            }
                        }
                        if (itcomp != null)
                        {
                            ITDto.FLAGAKTIV = itcomp.FLAGAKTIV;
                            ITDto.AMTBEZ = itcomp.AMTBEZ;
                            ITDto.AMTBIS = itcomp.AMTBIS;
                            ITDto.AMTLAND = itcomp.AMTLAND;
                            ITDto.AMTSEIT = itcomp.AMTSEIT;
                        }


                        ITDto = ITAssembler.ValidateLegit(ITDto);
                        /*ITOPTION ito = Context.SelectById<ITOPTION>(sysIT);
                        if(ito!=null)
                        ITDto.VERTRETUNGSBERECHTIGUNG = ito.OPTION1;*/

                        //set default to Österreich when 0
                        if (!ITDto.SYSLANDNAT.HasValue || ITDto.SYSLANDNAT.Value == 0)
                            ITDto.SYSLANDNAT = 127;
                        // Get angebot count
                        long kneAngebot = sysANGEBOT;
                        if (kneAngebot > 0)//when one is given, but with no kne yet (after angebot-copy)
                        {
                            ITDto.KNELIST = Context.ExecuteStoreQuery<ITKNEDto>("select sysober,sysunter,relatetypecode,quote from itkne where area='ANGEBOT' and sysarea=" + kneAngebot + " and sysunter = " + ITDto.SYSIT).ToList();
                            if (ITDto.KNELIST == null || ITDto.KNELIST.Count == 0)
                                kneAngebot = 0;//force the last offer WITH kne to be used for finding kne
                        }

                        if (kneAngebot == 0)//when none given search latest offer kne data
                        {
                            kneAngebot = Context.ExecuteStoreQuery<long>("select max(sysarea) from itkne where area='ANGEBOT' and sysunter = " + ITDto.SYSIT).FirstOrDefault();
                        }


                        if (kneAngebot > 0)
                        {
                            ITDto.KNELIST = Context.ExecuteStoreQuery<ITKNEDto>("select sysober,sysunter,relatetypecode,quote from itkne where area='ANGEBOT' and sysarea=" + kneAngebot + " and sysunter = " + ITDto.SYSIT).ToList();
                            if(ITDto.KNELIST!=null && ITDto.KNELIST.Count>0)
                            {
                                String[] knes = {"GESELLS","KOMPL","PARTNER","VORSTAND","STIFTUNGSV","STIFTB" };
                                foreach(ITKNEDto kne in ITDto.KNELIST)
                                {
                                    if (knes.Contains(kne.relateTypeCode))
                                        kne.relateTypeCode = "INH";
                                }
                            }
                        }
                        if(sysANGEBOT>0)
                        { 
                            //if this it is registered as WB for the offer, load the quote from itkne
                            ITDto.WINTUMFANG = Context.ExecuteStoreQuery<decimal>("select quote from itkne where area='ANGEBOT' and sysarea=" + sysANGEBOT + " and relatetypecode='WB' and sysober = " + ITDto.SYSIT).FirstOrDefault();
                            ITDto.WINTART = Context.ExecuteStoreQuery<string>("select CODERELATEKIND from itkne where area='ANGEBOT' and sysarea=" + sysANGEBOT + " and relatetypecode='WB' and sysober = " + ITDto.SYSIT).FirstOrDefault();
                        }
                        

                        string query3 = "SELECT count(*) FROM(";
                        query3 += "select angebot.SYSID from angebot left outer join antrag on antrag.SYSANGEBOT=angebot.SYSID where angebot.SYSIT=" + ITDto.SYSIT + " and antrag.sysid is null";
                        query3 += " and angebot.SYSID in (SELECT sysid FROM peuni, perolecache WHERE area = 'ANGEBOT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") ";
                        query3 += " ORDER BY ANGEBOT.AKTIVKZ, ANGEBOT.DATAKTIV, ANGEBOT.ANGEBOT";
                        query3 += ") WHERE rownum <=100";

                        ITDto.ANGEBOTCount = Context.ExecuteStoreQuery<int>(query3, null).FirstOrDefault<int>();
                            //"select count(*) from angebot left outer join antrag on antrag.SYSANGEBOT=angebot.SYSID where angebot.SYSIT=" + ITDto.SYSIT + " and antrag.sysid is null and CIC.CIC_PEROLE_UTILS.ChkObjInPEUNI( " + ServiceValidator.SYSPUSER + @", 'ANGEBOT', angebot.sysid,sysdate) = 1", null).FirstOrDefault<int>();


                        //FILL EOTCOUNT, CAMPCOUNT, ISPERSON
                        /*if (!ITDto.SYSPERSON.HasValue || ITDto.SYSPERSON.Value == 0)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "name", Value = ITDto.NAME.Trim() });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vorname", Value = ITDto.VORNAME != null ? ITDto.VORNAME.Trim() : "" });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "plz", Value = ITDto.PLZ != null ? ITDto.PLZ.Trim() : "" });
                            ITDto.SYSPERSON = Context.ExecuteStoreQuery<long>("select sysperson from person where trim(name)=:name and trim(vorname)=:vorname and trim(plz)=:plz", parameters.ToArray()).FirstOrDefault();
                        }*/

                        if (ITDto.SYSPERSON.HasValue && ITDto.SYSPERSON.Value > 0)
                        {
                            
                            // Get vt count
                            // ITLoop.VTCount = Context.ExecuteStoreQuery<int>("select count(*) from vt where syskd = " + ITLoop.SYSPERSON + " and CIC.CIC_PEROLE_UTILS.ChkObjInPEUNI( " + ServiceValidator.SYSPUSER + @", 'VT', vt.sysid,sysdate) = 1", null).FirstOrDefault<int>();

                            //Get filter
                            string Filter = null;
                            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "VT", string.Empty);
                            

                            if (Filter != null && Filter.Length > 0)
                            {
                                Filter = " AND " + Filter;
                            }
                            if (Filter == null)
                                Filter = "";

                            if (ITDto.SYSPERSON.HasValue)
                            {
                                ITDto.VTCount = Context.ExecuteStoreQuery<int>("select count(*) from vt where SYSKD = " + ITDto.SYSPERSON.Value + " and sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") " + Filter, null).FirstOrDefault<int>();

                                //ITDto.EOTCount = Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='EOT' and vt.syskd=" + ITDto.SYSPERSON.Value, null).FirstOrDefault<int>();
                                //ITDto.CAMPCount = Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='CAMP' and camp.status>1 and 3>camp.status and vt.syskd=" + ITDto.SYSPERSON.Value, null).FirstOrDefault<int>();
                            }
                        }

                      

                    }
                }
                _Log.Debug("Tracing IT Assembling: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start1));
               
                return ITDto;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeliverFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeliverFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// determines if konto may be changed
        /// only true when no active contract
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public bool changeBankdatenAllowed(long sysAngebot)
        {
            return new BankdatenDao().changeBankdatenAllowed(sysAngebot);
            
        }

        /// <summary>
        /// updates the konto info in IT from available global mandate or person konto
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="sysvsart"></param>
        public void updateITBankdaten(long sysit, long sysvsart)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            using (DdOlExtended Context = new DdOlExtended())
            {
                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, sysit))
                    return;
            }
            new BankdatenDao().updateITBankdaten(sysit, sysvsart, ServiceValidator.SysPEROLE);
        }

        /// <summary>
        /// returns the default signcity for the current salesperson
        /// </summary>
        public String getDefaultSigncity()
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            long vpsysperson = ServiceValidator.VpSysPERSON.GetValueOrDefault();
            return BankdatenDao.getDefaultSigncity(vpsysperson);
        }
        
        /// <summary>
        /// delivers the current active konto used for the offer in status genehmigt
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto DeliverBankdaten(long sysAngebot)
        {
            return BankdatenDao.DeliverBankdaten(sysAngebot,true,true);
        }

        /// <summary>
        /// saves the currently used konto for a already submitted offer
        /// may create a konto and global-mandate
        /// </summary>
        /// <param name="bankdaten"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto SaveBankdaten(Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto bankdaten)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            using (DdOlExtended Context = new DdOlExtended())
            {
                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, bankdaten.SYSIT))
                    throw new Exception("No Permission to IT");
            }
            return new BankdatenDao().SaveBankdaten(bankdaten, ServiceValidator.VpSysPERSON.GetValueOrDefault(),ServiceValidator.SysPEROLE);
        }

        /// <summary>
        /// Saves the IT and the Offer Mandat reference (MANDAT, ANGEBOT.EINZUG)
        /// </summary>
        /// <param name="itDto"></param>
        /// <param name="angebotDto"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.ITDto SaveKonto(Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto,Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            bool mandatchange = false;//true when we have to change the active mandat because of iban-change
            using (DdOlExtended Context = new DdOlExtended())
            {
                if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, itDto.SYSIT.Value))
                    throw new Exception("No Permission to IT");

                String ibanOld = Context.ExecuteStoreQuery<String>("select iban from it where sysit=" + itDto.SYSIT, null).FirstOrDefault();
                if (ibanOld == null) ibanOld = "";
                ibanOld = ibanOld.Trim();
                String ibanNew = itDto.IBAN;
                if (ibanNew == null) ibanNew = "";
                ibanNew = ibanNew.Trim();
                if (!ibanOld.Equals(ibanNew))
                {
                    mandatchange = true;
                }
            }
            //wenn iban/bic-änderung UND mandat.syskonto befüllt, dann bisheriges mandat nicht verwenden sondern neues anlegen
            Cic.OpenLease.ServiceAccess.DdOl.ITDto rval = Save(itDto);
            if (angebotDto != null && angebotDto.SYSVART.HasValue && angebotDto.SYSID.HasValue)
            {
                angebotDto.SYSIT = rval.SYSIT;
                using (DdOlExtended Context = new DdOlExtended())
                {
                    long sysls = LsAddHelper.getMandantByPEROLE(Context, ServiceValidator.SysPEROLE);
                    

                    //SAVE ANGEBOT Settings
                    ANGEBOT ang = (from a in Context.ANGEBOT
                                       where a.SYSID == (long)angebotDto.SYSID
                                       select a).FirstOrDefault();

                    ang.SYSKI = (angebotDto.SYSKI.HasValue && angebotDto.SYSKI.Value < 1L ? null : angebotDto.SYSKI);
                    ang.EINZUG = angebotDto.EINZUG;
                    angebotDto.ZUSTAND = ang.ZUSTAND;
                    angebotDto.ANGEBOT1 = ang.ANGEBOT1;
                    angebotDto.SYSVART = angebotDto.SYSVART.Value;
                    Context.SaveChanges();


                    if (mandatchange && ang.ANGEBOT1.IndexOf(".")>-1)//umgehängte wiedereinreichungswandermandate auf bei ibanänderung ungültig machen damit ein globalmandat entsteht
                    {
                        BankdatenDto bankdaten = BankdatenDao.DeliverBankdaten(angebotDto.SYSID.Value,false,true);
                        long sysmandat = 0;
                        List<Devart.Data.Oracle.OracleParameter> parameters;
                        //falls wiedereinreichungsmandat, dann vom antrag und oder angebotsmandat suchen
                        if (sysmandat == 0)//wandermandat am vt suchen der wiedereingereicht wurde
                        {
                            parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                            sysmandat = Context.ExecuteStoreQuery<long>(BankdatenDao.QUERYITWANDERMANDATVT, parameters.ToArray()).FirstOrDefault();
                        }
                        if (sysmandat == 0)//wandermandat am antrag suchen der wiedereingereicht wurde
                        {
                            parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                            sysmandat = Context.ExecuteStoreQuery<long>(BankdatenDao.QUERYITWANDERMANDATANTRAG, parameters.ToArray()).FirstOrDefault();
                        }
                        if (sysmandat == 0)//wandermandat hängt noch am alten angebot
                        {
                            parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                            sysmandat = Context.ExecuteStoreQuery<long>(BankdatenDao.QUERYITWANDERMANDATANGEBOT, parameters.ToArray()).FirstOrDefault();
                        }

                        if(sysmandat>0)
                        {
                            Context.ExecuteStoreCommand("update mandat set validuntil = SYSDATE-1 where sysmandat=" + sysmandat, null);
                        }
                    }

                    //Create or Update the Mandate
                    String mref = ANGEBOTAssembler.createOrUpdateMandat(angebotDto,  sysls, Context, ServiceValidator.VpSysPERSON.GetValueOrDefault());

                  
                    

                    Context.SaveChanges();
                    //TODO remove when edmx ready
                    if (mref != null)
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = angebotDto.SYSID });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ref", Value = mref });
                        Context.ExecuteStoreCommand("update angebot set mandatreferenz=:ref where sysid=:sysid", parameters.ToArray());
                    }
                    else if(angebotDto.EINZUG.HasValue && angebotDto.EINZUG.Value<1)
                    {
                        Context.ExecuteStoreCommand("update angebot set mandatreferenz=null where sysid=" + angebotDto.SYSID, null);
                    }
                }
            }
            return rval;
        }


        public Cic.OpenLease.ServiceAccess.DdOl.ITDto Save(Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            ServiceValidator.ValidateInsert();

            try
            {
                Cic.OpenLease.Service.ITAssembler ITAssembler;
                IT ModifiedIT = null;
                Cic.OpenLease.ServiceAccess.DdOl.ITDto ModifiedITDto = null;
				
                if (itDto.SYSIT.HasValue && itDto.SYSIT.Value > 0)
                {
                    using (DdOlExtended Context = new DdOlExtended())
                    {
                        if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, itDto.SYSIT.Value))
                            throw new Exception("No Permission to IT");
                    }
                }

                // New assembler
                ITAssembler = new Cic.OpenLease.Service.ITAssembler(ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER);

                // Check dto
                if (!ITAssembler.IsValid(itDto))
                {
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralNotValid, Cic.OpenLease.Service.DtoAssemblerHelper.DeliverErrorMessage(ITAssembler.Errors));
                }

                if (itDto == null)
                {
                    throw new ArgumentException("itDto");
                }

                if (itDto.IBAN != null)
                {
                    IBANValidator ib = new IBANValidator();
                    //change to correct BIC
                    IBANValidationError ibanerror = ib.checkIBANandBIC(itDto.IBAN,itDto.BIC);
                    if (ibanerror.newBIC != null)
                        itDto.BIC = ibanerror.newBIC;
                }

                if (!itDto.SYSIT.HasValue)
                {
                    // Create
                    ModifiedIT = ITAssembler.Create(itDto);
                }
                else
                {
                    // Update
                    if (!ITDao.hasSubmittedOffer(itDto.SYSIT.Value))
                    {
                        ModifiedIT = ITAssembler.Update(itDto);
                    }
                    else//do not update, but update  compliance and kne, so read db version
                    {
                        using (DdOlExtended Context = new DdOlExtended())
                        {
                            ModifiedIT = (from c in Context.IT
                                          where c.SYSIT == itDto.SYSIT
                                          select c).FirstOrDefault();//Context.SelectById<IT>((long)itDto.SYSIT.Value);
                        }
                    }
                }
                itDto.SYSIT = ModifiedIT.SYSIT;
                ITAssembler.UpdateCompliance(itDto);
                ITAssembler.UpdateKNE(itDto);

                // Create dto
                ModifiedITDto = ITAssembler.ConvertToDto(ModifiedIT);
                // ModifiedITDto.VERTRETUNGSBERECHTIGUNG = itDto.VERTRETUNGSBERECHTIGUNG;
                ModifiedITDto.AMTBEZ = itDto.AMTBEZ;
                ModifiedITDto.AMTSEIT = itDto.AMTSEIT;
                ModifiedITDto.AMTBIS = itDto.AMTBIS;
                ModifiedITDto.AMTLAND = itDto.AMTLAND;
                ModifiedITDto.FLAGAKTIV = itDto.FLAGAKTIV;
                ModifiedITDto.WINTUMFANG = itDto.WINTUMFANG;
                ModifiedITDto.WINTART = itDto.WINTART;
                ModifiedITDto.KNE = itDto.KNE;


                resultIdCache.Clear();
                countCache.Clear();
                return ModifiedITDto;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.ItSaveFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ItSaveFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Deletes the IT
        /// </summary>
        /// <param name="sysIT"></param>
        public void Delete(long sysIT)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateDelete();

            try
            {
                IT IT = null;

                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Transaction
                    //TRANSACTIONS
                    // using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
                    {
                        // Select IT
                        IT = (from c in Context.IT
                              where c.SYSIT == sysIT
                              select c).FirstOrDefault();//Context.SelectById<IT>(sysIT);

                        // Check sight field
                        if (IT != null)
                        {
                            if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, sysIT))
                            {
                                // Not exists on the list
                                // TODO JJ 0 JJ, Localize text
                                throw new System.ApplicationException("No Permission to IT.");
                            }
                        }
                        else
                        {
                            // Not exists
                            // TODO JJ 0 JJ, Localize text
                            throw new System.ApplicationException("IT not exists.");
                        }

                        // Check if IT has no offer
                        // TODO JJ 0 JJ, Add: - or if all offers are submitted
                        int ANGEBOTCount = (from angebot in Context.ANGEBOT
                                            where (angebot.SYSIT == IT.SYSIT)
                                            select angebot).Count();

                        if (ANGEBOTCount > 0)
                        {
                            // IT has one or more offers
                            // TODO JJ 0 JJ, Localize text
                            throw new System.ApplicationException("One or more ANGEBOT objects has references to the IT object.");
                        }

                        // Delete PEUNI
                        string AreaName = PEUNIArea.IT.ToString();

                        // Select PEUNI list                 
                        var PEUNIQuery = from peuni in Context.PEUNI
                                         where (peuni.SYSID == IT.SYSIT && peuni.AREA == AreaName)
                                         select peuni;

                        // Delete each PEUNI (in context)                    
                        foreach (PEUNI LoopPEUNI in PEUNIQuery)
                        {
                            Context.DeleteObject(LoopPEUNI);
                        }

                        // DeleteObject IT and SaveChanges
                        //Context.Delete<IT>(IT);
                        Context.DeleteObject(IT);
                        // Set transaction complete
                        // TransactionScope.Complete();
                        Context.SaveChanges();
                    }
                }
                resultIdCache.Clear();
                countCache.Clear();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeleteFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeleteFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Deletes the ITKNE
        /// </summary>
        /// <param name="kne"></param>
        public void DeleteKNE(ITKNEDto kne)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateDelete();

            try
            {
                IT IT = null;

                using (DdOlExtended Context = new DdOlExtended())
                {


                    /*  if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, kne.SYSUNTER))
                      {
                          // Not exists on the list
                          // TODO JJ 0 JJ, Localize text
                          throw new System.ApplicationException("No Permission to IT.");
                      }*/
                    if (kne.SYSOBER == 0 && kne.SYSUNTER == 0)//delete all kne for this offer
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = kne.SYSANGEBOT });
                        Context.ExecuteStoreCommand("delete from itkne where area='ANGEBOT' and sysarea=:p4", parameters.ToArray());

                    }
                    else
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = kne.SYSOBER });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = kne.SYSUNTER });
                        
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = kne.SYSANGEBOT });

                        String[] knes = { "GESELLS", "KOMPL", "PARTNER", "VORSTAND", "STIFTUNGSV", "STIFTB" };
                        String query = "delete from itkne where sysober=:p1 and sysunter=:p2 and relatetypecode=:p3 and area='ANGEBOT' and sysarea=:p4";
                        if (kne.relateTypeCode.Equals("INH"))
                        {
                            query = "delete from itkne where sysober=:p1 and sysunter=:p2 and relatetypecode in ('INH', 'GESELLS', 'KOMPL', 'PARTNER', 'VORSTAND', 'STIFTUNGSV', 'STIFTB') and area='ANGEBOT' and sysarea=:p4";
                        }
                        else
                        {
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = kne.relateTypeCode });
                        }
                        Context.ExecuteStoreCommand(query, parameters.ToArray());
                    }
                    Context.SaveChanges();
                    
                }
               
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeleteFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.ItDeleteFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }



        public ITDto DeliverItDtoFromSa3(string xmlData)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Get the offer
                ITConfiguration ITConfiguration = Sa3Helper.GetSa3ITConfiguration(xmlData);

                // Create and return angebot
                return ITAssembler.CreateITDto(ITConfiguration);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverITDtoFromSa3Failed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverITDtoFromSa3Failed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverITDtoFromSa3Failed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }




        #region search Vorgang
        public VorgaengeDto searchVorgaenge(long sysIT, int anzahlMax)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            VorgaengeDto vs = new VorgaengeDto();

            // Create search data
            Cic.OpenLease.Service.DdOl.VTDao vt = new Cic.OpenLease.Service.DdOl.VTDao();
            Cic.OpenLease.Service.DdOl.ANTRAGDao antrag = new Cic.OpenLease.Service.DdOl.ANTRAGDao();
            Cic.OpenLease.Service.DdOl.ANGEBOTDao angebot = new Cic.OpenLease.Service.DdOl.ANGEBOTDao();


            List<long> vertraegeIds = null;
            List<long> antraegeIds = null;
            List<long> angeboteIds = null;

            string vtfilter = "";
            string atfilter = "";
            string angfilter = "";
            //Get filter
            string Filter;
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "ANTRAG", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                atfilter = " AND " + Filter;
            }
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "VERTRAG", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                vtfilter = " AND " + Filter;
            }
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "ANGEBOT", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                angfilter = " AND " + Filter;
            }

            string query1 = "SELECT * FROM(";
            query1 += "SELECT SYSID FROM CIC.VT WHERE SYSKD IN (SELECT DISTINCT SYSPERSON FROM IT WHERE SYSIT=" + sysIT + ")";
            query1 += vtfilter;
            query1 += " and SYSID in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") ";
            //query1 += " AND SYSID IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + ServiceValidator.SYSPUSER + ", 'VT',sysdate)))";
            query1 += " ORDER BY AKTIVKZ, DATAKTIV, VERTRAG";
            query1 += ") WHERE rownum <= " + anzahlMax;

            string query2 = "SELECT * FROM(";
            query2 += "select antrag.SYSID from antrag left outer join vt on vt.SYSANTRAG=antrag.SYSID where antrag.SYSKD IN (SELECT DISTINCT IT.SYSPERSON FROM IT WHERE IT.SYSIT=" + sysIT + ") and vt.sysid is null";
            query2 += atfilter;
            query2 += " and antrag.SYSID in (SELECT sysid FROM peuni, perolecache WHERE area = 'ANTRAG' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") ";
            //query2 += " AND antrag.SYSID  IN (SELECT * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + ServiceValidator.SYSPUSER + ", 'ANTRAG',sysdate)))";
            query2 += " ORDER BY ANTRAG.AKTIVKZ, ANTRAG.DATAKTIV, ANTRAG.ANTRAG";
            query2 += ") WHERE rownum <=" + anzahlMax;

            string query3 = "SELECT * FROM(";
            query3 += "select angebot.SYSID from angebot left outer join antrag on antrag.SYSANGEBOT=angebot.SYSID where angebot.SYSIT=" + sysIT + " and antrag.sysid is null";
            query3 += angfilter;
            query3 += " and angebot.SYSID in (SELECT sysid FROM peuni, perolecache WHERE area = 'ANGEBOT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") ";
            //query3 += " AND angebot.SYSID IN (SELECT * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + ServiceValidator.SYSPUSER + ", 'ANGEBOT',sysdate)))";
            query3 += " ORDER BY ANGEBOT.AKTIVKZ, ANGEBOT.DATAKTIV, ANGEBOT.ANGEBOT";
            query3 += ") WHERE rownum <=" + anzahlMax;

            List<VorgangDto> vg = new List<VorgangDto>();
            // Create search data
            using (DdOlExtended context = new DdOlExtended())
            {

                try
                {
                    vertraegeIds = context.ExecuteStoreQuery<long>(query1, null).ToList<long>();

                    if (vertraegeIds != null)
                    {
                        foreach (var vertraegeIdsLoop in vertraegeIds)
                        {
                            VTDto orgDto = vt.Deliver(vertraegeIdsLoop);
                            GenericAssembler<VorgangDto, VTDto> assembler = new GenericAssembler<VorgangDto, VTDto>(new VTVorgangMapper());

                            VorgangDto vdto = assembler.ConvertToDto(orgDto);
                            vdto.PRODUCTNAME = "";

                            try
                            {
                                string PRPRODUCTQuery = "select prproduct.* from prproduct,angebot,vt where angebot.sysid=vt.sysangebot and angebot.sysprproduct=prproduct.sysprproduct and vt.sysid=" + vertraegeIdsLoop;
                                PRPRODUCT p = context.ExecuteStoreQuery< PRPRODUCT>(PRPRODUCTQuery, null).FirstOrDefault<PRPRODUCT>();
                                vdto.PRODUCTNAME = p.NAME;
                            }
                            catch (Exception)
                            {
                                //ignore when not mapped;
                            }
                            vg.Add(vdto);


                        }

                    }

                }
                catch (Exception)
                {
                }

                try
                {
                    antraegeIds = context.ExecuteStoreQuery<long>(query2, null).ToList<long>();
                    if (antraegeIds != null)
                    {
                        foreach (var antraegeIdsLoop in antraegeIds)
                        {
                            ANTRAGDto orgDto = antrag.Deliver(antraegeIdsLoop);
                            GenericAssembler<VorgangDto, ANTRAGDto> assembler = new GenericAssembler<VorgangDto, ANTRAGDto>(new ANTRAGVorgangMapper());
                            VorgangDto vdto = assembler.ConvertToDto(orgDto);
                            vdto.PRODUCTNAME = "";

                            try
                            {
                                string PRPRODUCTQuery = "select prproduct.* from prproduct,angebot,antrag where angebot.sysid=antrag.sysangebot and angebot.sysprproduct=prproduct.sysprproduct and antrag.sysid=" + antraegeIdsLoop;
                                PRPRODUCT p = context.ExecuteStoreQuery<PRPRODUCT>(PRPRODUCTQuery, null).FirstOrDefault<PRPRODUCT>();
                                vdto.PRODUCTNAME = p.NAME;
                            }
                            catch (Exception)
                            {
                                //ignore when not mapped;
                            }
                            vg.Add(vdto);

                        }


                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    angeboteIds = context.ExecuteStoreQuery<long>(query3, null).ToList<long>();
                    if (angeboteIds != null)
                    {
                        foreach (var angeboteIdsLoop in angeboteIds)
                        {
                            ANGEBOTDto orgDto = context.ExecuteStoreQuery<ANGEBOTDto>("select angebot.sysprproduct,angkalk.ratebrutto angkalkratebrutto, angebot.objektvt, angebot angebot1, angob.jahreskm angobjahreskm, angkalk.lz angkalklz, angkalk.rwkalkbrutto angkalkrwkalkbrutto, angebot.sysid, angebot.zustand, angebot.vart from angebot,angob, angkalk where angkalk.SYSANGEBOT=angebot.sysid and angob.sysob=angkalk.sysob and angebot.sysid=" + angeboteIdsLoop, null).FirstOrDefault();
                                //angebot.Deliver(angeboteIdsLoop);
                            GenericAssembler<VorgangDto, ANGEBOTDto> assembler = new GenericAssembler<VorgangDto, ANGEBOTDto>(new ANGEBOTVorgangMapper());
                            VorgangDto vdto = assembler.ConvertToDto(orgDto);

                            string PRPRODUCTQuery = "SELECT * FROM PRPRODUCT WHERE SYSPRPRODUCT = " + orgDto.SYSPRPRODUCT;
                            PRPRODUCT p = context.ExecuteStoreQuery<PRPRODUCT>(PRPRODUCTQuery, null).FirstOrDefault<PRPRODUCT>();
                            vdto.PRODUCTNAME = p.NAME;
                            vg.Add(vdto);

                        }


                    }
                }
                catch (Exception)
                {
                }
                vs.Vorgaenge = vg.ToArray();
                return vs;
            }

        }


        #endregion
        #endregion

        #region My methods

        private static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(1000 * 60 * 3);
        private static CacheDictionary<String, List<long>> resultIdCache = CacheFactory<String, List<long>>.getInstance().createCache(1000 * 60 * 3);
        private System.Collections.Generic.List<ITShortDto> MyDeliverITList(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ITSortData[] itSortDatas, long? sysPEROLE, long sysPUSER, out long rescount)
        {
            string Query;
            System.Collections.Generic.List<ITShortDto> ITList;
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();
            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (itSearchData == null)
            {
                throw new ArgumentException("itSearchData");
            }

            // Create new list

            object[] Parameters = MyDeliverQueryParameters(itSearchData);

            try
            {
                // Create query
                Query = MyCreateQuery(context, itSearchData, sysPEROLE, sysPUSER, false);
            }
            catch
            {
                throw;
            }

            QueryBuilder.Append(Query);

            StringBuilder sortBuilder = new StringBuilder();
            if (itSortDatas != null && itSortDatas.Length > 0)
            {
                int i = 0;
                sortBuilder.Append(" ORDER BY ");
                // Order
                foreach (Cic.OpenLease.ServiceAccess.DdOl.ITSortData itSortDataLoop in itSortDatas)
                {
                    sortBuilder.Append(itSortDataLoop.SortDataConstant.ToString() + " " + itSortDataLoop.SortOrderConstant.ToString());
                    if (i != itSortDatas.Length - 1)
                    {
                        sortBuilder.Append(", ");
                    }
                    i++;
                }
            }
            else
            {
                // Default Order
                sortBuilder.Append(" ORDER BY NAME, VORNAME, PLZ, ORT, STRASSE, HSNR, CODE ");
            }
            QueryBuilder.Append(sortBuilder.ToString());

            String itQuery = QueryBuilder.ToString();



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
                String resultIdCacheKey = getParamKey(Parameters, "IT_" + sysPUSER + sortBuilder.ToString()+"_PF"+itSearchData.ISPERSON);

                if (!resultIdCache.ContainsKey(resultIdCacheKey))
                {
                    _Log.Debug("IT-Search-SQL: " + itQuery);
                    resultIdCache[resultIdCacheKey] = context.ExecuteStoreQuery<long>(itQuery, Parameters).ToList();
                }
                List<long> ids = resultIdCache[resultIdCacheKey];


                rescount = ids.Count;

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

                itQuery = "SELECT SYSPERSON,SYSIT, SYSKDTYP, NAME, VORNAME, STRASSE, HSNR, PLZ, ORT, PTELEFON,TELEFON,HANDY,FAX,EMAIL,BESCHARTAG1,GEBDATUM,GRUENDUNG FROM CIC.IT WHERE sysit in (" + string.Join(",", ids) + ") " + sortBuilder.ToString();
                _Log.Debug("IT-Search-SQL: " + itQuery);
                ITList = context.ExecuteStoreQuery<ITShortDto>(itQuery, null).ToList();
                _Log.Debug("IT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                return ITList;
            }


            Query = "SELECT * FROM (SELECT rownum rnum, a.* FROM(" + QueryBuilder.ToString() + ") a WHERE rownum <= " + (searchParameters.Skip + searchParameters.Top) + ") WHERE rnum > " + searchParameters.Skip;

            try
            {
                // Create list

                double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;

                int resultCount = 0;
                String countKey = getParamKey(Parameters, Query);
                // count when query changed in the last 2 minutes
                if (!searchCache.ContainsKey(countKey))
                {
                    String countQuery = MyCreateQuery(context, itSearchData, sysPEROLE, sysPUSER, true);
                    _Log.Debug("IT-Search-Count-SQL: " + countQuery);
                    resultCount = context.ExecuteStoreQuery<int>(countQuery, Parameters).FirstOrDefault();
                    Parameters = MyDeliverQueryParameters(itSearchData);
                }
                else
                    resultCount = searchCache[countKey];
                _Log.Debug("IT-Search-SQL: " + Query);
                ITList = context.ExecuteStoreQuery<ITShortDto>(Query, Parameters).ToList();
                _Log.Debug("IT-Search Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                searchCache[countKey] = resultCount; //touch the cache, another cache lifetime duration for no fetching the count
                rescount = resultCount;
            }
            catch
            {
                throw;
            }

            return ITList;
        }

        public static bool hasSubmittedOffer(long sysit)
        {
            //Sicherheiten
            String queryA = "select count(*) from angobsich,sichtyp,angebot where sichtyp.syssichtyp=angobsich.syssichtyp and sichtyp.rang in (10,80,140) and angobsich.sysit=:sysit and angebot.sysid=angobsich.sysvt and angebot.zustand='Eingereicht'";
            //Angebote
            String queryB = "select count(*) from angebot where angebot.sysit=:sysit and  angebot.zustand='Eingereicht'";


            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });

                long hasSubmittedOffer = ctx.ExecuteStoreQuery<long>(queryA, parameters.ToArray()).FirstOrDefault();
                if (hasSubmittedOffer > 0) return true;


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });

                hasSubmittedOffer = ctx.ExecuteStoreQuery<long>(queryB, parameters.ToArray()).FirstOrDefault();
                if (hasSubmittedOffer > 0) return true;

            }
            return false;
        }

        private static String getParamKey(object[] par, String prefix)
        {
            StringBuilder sb = new StringBuilder(prefix);
            sb.Append(": ");
            foreach (object o in par)
                sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
            return sb.ToString();
        }
        private string MyCreateQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, long? sysPEROLE, long sysPUSER, bool count)
        {
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (itSearchData == null)
            {
                throw new ArgumentException("itSearchData");
            }

            if (count)
            {
                QueryBuilder.Append("SELECT COUNT(*) FROM CIC.IT WHERE 1 = 1 ");
            }
            else
            {
                //QueryBuilder.Append("SELECT SYSPERSON,SYSIT, SYSKDTYP, NAME, VORNAME, STRASSE, HSNR, PLZ, ORT, PTELEFON,TELEFON,HANDY,FAX,EMAIL,BESCHARTAG1,GEBDATUM,GRUENDUNG FROM CIC.IT WHERE 1 = 1 ");
                QueryBuilder.Append("SELECT SYSIT FROM CIC.IT WHERE 1 = 1 ");
            }
            // SYSIT
            if (itSearchData.SYSIT.HasValue)
            {
                // Where
                QueryBuilder.Append("AND SYSIT = :pSYSIT ");
            }

            //TODO  AIDA2 ISPERSON FIlter

            // IsPrivate
            if (itSearchData.PRIVATFLAG.HasValue)
            {
                // Where
                if (itSearchData.PRIVATFLAG.Value)
                {
                    // Where
                    QueryBuilder.Append("AND (PRIVATFLAG IS NOT NULL AND PRIVATFLAG <> 0) ");
                }
                else
                {
                    // Where
                    QueryBuilder.Append("AND (PRIVATFLAG IS NULL OR PRIVATFLAG = 0) ");
                }
            }

            // CODE
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.CODE))
            {
                // Where
                QueryBuilder.Append("AND UPPER(CODE) LIKE UPPER(:pCODE) ");
            }

            // NAME
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.NAME))
            {
                // Where
                QueryBuilder.Append("AND UPPER(NAME) LIKE UPPER(:pNAME) ");
            }

            // VORNAME
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.VORNAME))
            {
                // Where
                QueryBuilder.Append("AND UPPER(VORNAME) LIKE UPPER(:pVORNAME) ");
            }

            // ZUSATZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.ZUSATZ))
            {
                // Where
                QueryBuilder.Append("AND UPPER(ZUSATZ) LIKE UPPER(:pZUSATZ) ");
            }


            // NAMEVORNAMEZUSATZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.NAMEVORNAMEZUSATZ))
            {
                // Where
                QueryBuilder.Append("AND (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:pNAMEVORNAMEZUSATZ))");
            }
            // STRASSE
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.STRASSE))
            {
                // Where
                QueryBuilder.Append("AND UPPER(STRASSE) LIKE UPPER(:pSTRASSE) ");
            }

            // HSNR
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.HSNR))
            {
                // Where
                QueryBuilder.Append("AND UPPER(HSNR) LIKE UPPER(:pHSNR) ");
            }


            // PLZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.PLZ))
            {
                // Where
                QueryBuilder.Append("AND UPPER(PLZ) LIKE UPPER(:pPLZ) ");
            }

            // ORT
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.ORT))
            {
                // Where
                QueryBuilder.Append("AND UPPER(ORT) LIKE UPPER(:pORT) ");
            }
            if (itSearchData.SYSKDTYP!=null&& itSearchData.SYSKDTYP.Length>0)
                QueryBuilder.Append("AND SYSKDTYP in ("+itSearchData.SYSKDTYP+") ");

            System.Text.StringBuilder QueryFinanzierungsart = new System.Text.StringBuilder();
            QueryFinanzierungsart.Append("SELECT DISTINCT SYSIT FROM CIC.ANGEBOT WHERE SYSIT IS NOT NULL AND SYSIT <> 0 AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            QueryFinanzierungsart.Append("UNION SELECT DISTINCT SYSIT FROM CIC.ANTRAG  WHERE SYSIT IS NOT NULL AND SYSIT <> 0 AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            QueryFinanzierungsart.Append("UNION SELECT DISTINCT SYSIT FROM CIC.VT WHERE SYSIT IS NOT NULL AND SYSIT <> 0 AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            bool filterByNumber = false;
            if (itSearchData.AngAntVtBis != null || !Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.AngAntVtNummer) || itSearchData.AngAntVtVon != null)


                // Deliver filtered Ids query by AngAntVt if there are search values
                if (itSearchData.AngAntVtBis != null || !Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.AngAntVtNummer) || itSearchData.AngAntVtVon != null)
                {
                    //Create query of id's
                    string QueryFilteredIdsByAngAntVt = MyDeliverFilteredSYSITsByAngAntVtQuery(context, itSearchData);
                    //filterByNumber = true;
                    //Add values to query
                    QueryBuilder.Append("AND SYSIT IN (" + QueryFilteredIdsByAngAntVt + ") ");

                    QueryFinanzierungsart = new System.Text.StringBuilder();
                }

            // Deliver filtered Ids query by Fz if there are search values
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzBEZEICHNUNG) || !Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzKENNZEICHEN))
            {
                //Create list of id's
                string QueryFilteredIdsByFz = MyDeliverFilteredSYSITsByFzQuery(context, itSearchData);

                //Add subquery
                QueryBuilder.Append("AND SYSIT IN (" + QueryFilteredIdsByFz + ") ");
                QueryFinanzierungsart = new System.Text.StringBuilder();

            }



            // FINANZIERUNGSART
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.Finanzierungsart) && QueryFinanzierungsart.Length != 0)
            {
                // Where
                QueryBuilder.Append("AND SYSIT IN (" + QueryFinanzierungsart + ") ");
            }
            //Nur Kunden
            if (itSearchData.ISPERSON)
            {
                QueryBuilder.Append("AND IT.SYSPERSON>0 ");
            }


            // Sight fields narrowing
            if (sysPEROLE.HasValue && !filterByNumber)
            {

                QueryBuilder.Append(" and SYSIT in (SELECT sysid FROM peuni, perolecache WHERE area = 'IT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + sysPEROLE + ") ");

                //QueryBuilder.Append("AND SYSIT IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'IT',sysdate))) ");
            }

            //Get filter
            string Filter;
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "IT", string.Empty);
            

            if (Filter != null && Filter.Length > 0)
            {
                QueryBuilder.Append("AND " + Filter);
            }

            return QueryBuilder.ToString();
        }

        private string MyDeliverFilteredSYSITsByAngAntVtQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData)
        {
            System.Text.StringBuilder QueryAngebotBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryAntragBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryVtBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder FinalQueryBuilder = new System.Text.StringBuilder();

            QueryAngebotBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.ANGEBOT WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");
            QueryAntragBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.ANTRAG  WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");
            QueryVtBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.VT  WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");

            System.Collections.Generic.List<long> SysITList = new System.Collections.Generic.List<long>();

            string Date;

            // Finanzierungsart
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.Finanzierungsart))
            {
                // Angebot
                QueryAngebotBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
                QueryAntragBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
                QueryVtBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            }


            // AngAntVtNummer
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.AngAntVtNummer))
            {
                // Angebot
                QueryAngebotBuilder.Append("AND UPPER(ANGEBOT) LIKE UPPER(:pAngAntVtNummer) ");

                // Antrag
                QueryAntragBuilder.Append("AND UPPER(ANTRAG) LIKE UPPER(:pAngAntVtNummer) ");

                // Vt
                QueryVtBuilder.Append("AND UPPER(VERTRAG) LIKE UPPER(:pAngAntVtNummer) ");
            }

            // AngAntVtVon
            if (itSearchData.AngAntVtVon.HasValue)
            {

                // Angebot
                QueryAngebotBuilder.Append("AND DATANGEBOT >= to_date(:pAngAntVtVon, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");


                // Antrag
                QueryAntragBuilder.Append("AND BEGINN >= to_date(:pAngAntVtVon, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");

                // Vt
                QueryVtBuilder.Append("AND BEGINN >= to_date(:pAngAntVtVon, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");
            }

            // AngAntVtBis
            if (itSearchData.AngAntVtBis.HasValue)
            {
                Date = itSearchData.AngAntVtBis.Value.Date.ToString("yyyy-MM-dd");
                // Angebot
                QueryAngebotBuilder.Append("AND DATANGEBOT <= to_date(:pAngAntVtBis, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");


                // Antrag
                QueryAntragBuilder.Append("AND BEGINN <= to_date(:pAngAntVtBis, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");

                // Vt
                QueryVtBuilder.Append("AND BEGINN <= to_date(:pAngAntVtBis, 'yyyy-mm-dd') AND BEGINN IS NOT NULL ");
            }

            //Union Query
            FinalQueryBuilder.Append(QueryAngebotBuilder.ToString() + " UNION ");
            FinalQueryBuilder.Append(QueryAntragBuilder.ToString() + " UNION ");
            FinalQueryBuilder.Append(QueryVtBuilder.ToString());

            return FinalQueryBuilder.ToString();
        }

        private string MyDeliverFilteredSYSITsByFzQuery(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData)
        {
            System.Text.StringBuilder QueryANGOBBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryANTOBBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryOBBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryAngebotBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryAntragBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryVtBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder QueryAgebotSysItBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder FinalQueryBuilder = new System.Text.StringBuilder();
            System.Collections.Generic.List<long> SysITList = new System.Collections.Generic.List<long>();

            QueryANGOBBuilder.Append("SELECT DISTINCT SYSOB FROM CIC.ANGOB WHERE SYSOB IS NOT NULL AND SYSOB <> 0 ");
            QueryANTOBBuilder.Append("SELECT DISTINCT SYSVT FROM CIC.ANTOB WHERE SYSVT IS NOT NULL AND SYSVT <> 0 ");
            QueryOBBuilder.Append("SELECT DISTINCT SYSVT FROM CIC.OB WHERE SYSVT IS NOT NULL AND SYSVT <> 0 ");
            QueryAngebotBuilder.Append("SELECT DISTINCT SYSANGEBOT FROM CIC.ANGKALK WHERE SYSANGEBOT IS NOT NULL AND SYSANGEBOT <> 0 ");
            QueryAntragBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.ANTRAG WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");
            QueryVtBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.VT WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");
            QueryAgebotSysItBuilder.Append("SELECT DISTINCT SYSIT FROM CIC.ANGEBOT WHERE SYSIT IS NOT NULL AND SYSIT <> 0 ");


            // Finanzierungsart
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.Finanzierungsart))
            {
                // Angebot
                QueryAgebotSysItBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
                QueryAntragBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
                QueryVtBuilder.Append("AND SYSPRPRODUCT IN (SELECT SYSPRPRODUCT FROM PRPRODUCT WHERE UPPER(NAME) LIKE UPPER(:pFinanzierungsart)) ");
            }




            // Angebot
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzBEZEICHNUNG))
            {
                // Deliver filtered by FzBEZEICHNUNG ANGOB objects query
                QueryANGOBBuilder.Append("AND UPPER(BEZEICHNUNG) LIKE UPPER(:pFzBEZEICHNUNG) ");
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzKENNZEICHEN))
            {
                QueryANGOBBuilder.Append("AND UPPER(KENNZEICHEN) LIKE UPPER(:pFzKENNZEICHEN) ");
            }

            if (QueryANGOBBuilder.ToString().Trim() != "SELECT DISTINCT SYSOB FROM CIC.ANGOB WHERE SYSOB IS NOT NULL AND SYSOB <> 0")
            {
                QueryAngebotBuilder.Append("AND SYSOB IN(" + QueryANGOBBuilder.ToString() + ")");
            }

            // Antrag
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzBEZEICHNUNG))
            {
                // Antrag
                QueryANTOBBuilder.Append("AND UPPER(BEZEICHNUNG) LIKE UPPER(:pFzBEZEICHNUNG) ");
                //QueryANTOB = context.ANTOB.Where(antob => antob.BEZEICHNUNG.ToUpper().StartsWith(itSearchData.FzBEZEICHNUNG.ToUpper()));
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzKENNZEICHEN))
            {
                QueryANTOBBuilder.Append("AND UPPER(KENNZEICHEN) LIKE UPPER(:pFzKENNZEICHEN) ");
            }

            //if (QueryANTOBBuilder != null)
            //{

            QueryAntragBuilder.Append("AND SYSID IN(" + QueryANTOBBuilder.ToString() + ")");
            //QueryAntrag = QueryANTOB.Select(antob => antob.ANTRAG);
            //}

            // Vertrag
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzBEZEICHNUNG))
            {
                // OB
                QueryOBBuilder.Append("AND UPPER(BEZEICHNUNG) LIKE UPPER(:pFzBEZEICHNUNG) ");
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzKENNZEICHEN))
            {
                QueryOBBuilder.Append("AND UPPER(KENNZEICHEN) LIKE UPPER(:pFzKENNZEICHEN) ");
            }

            QueryVtBuilder.Append("AND SYSVT IN(" + QueryOBBuilder.ToString() + ")");
            QueryAgebotSysItBuilder.Append("AND SYSID IN (" + QueryAngebotBuilder.ToString() + ")");

            FinalQueryBuilder.Append(QueryAgebotSysItBuilder.ToString() + "UNION ");
            FinalQueryBuilder.Append(QueryAntragBuilder.ToString() + "UNION ");
            FinalQueryBuilder.Append(QueryVtBuilder.ToString());
            return FinalQueryBuilder.ToString();
        }

        private object[] MyDeliverQueryParameters(Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData)
        {

            System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter> ParametersList = new List<Devart.Data.Oracle.OracleParameter>();

            //TODO  AIDA2 ISPERSON FIlter
            if (itSearchData.SYSIT.HasValue)
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSIT", Value = itSearchData.SYSIT });
            }
            // CODE
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.CODE))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pCODE", Value = itSearchData.CODE + "%" });
            }

            // NAME
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.NAME))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pNAME", Value = itSearchData.NAME.Trim() + "%" });
            }

            // VORNAME
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.VORNAME))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pVORNAME", Value = itSearchData.VORNAME.Trim() + "%" });
            }

            // ZUSATZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.ZUSATZ))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pZUSATZ", Value = itSearchData.ZUSATZ.Trim() + "%" });
            }

            // NAMEVORNAMEZUSATZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.NAMEVORNAMEZUSATZ))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pNAMEVORNAMEZUSATZ", Value = "%" + (itSearchData.NAMEVORNAMEZUSATZ.Trim()).Replace(" ", "%") + "%" });
            }
            // STRASSE
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.STRASSE))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSTRASSE", Value = itSearchData.STRASSE.Trim() + "%" });
            }

            // HSNR
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.HSNR))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pHSNR", Value = itSearchData.HSNR.Trim() + "%" });
            }


            // PLZ
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.PLZ))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPLZ", Value = itSearchData.PLZ.Trim() + "%" });
            }

            // ORT
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.ORT))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pORT", Value = (itSearchData.ORT.Trim()).Replace(" ", "%") + "%" });
            }
            
            //AngAntVtNummer
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.AngAntVtNummer))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pAngAntVtNummer", Value = "%" + itSearchData.AngAntVtNummer.Trim() + "%" });
            }

            //AngAntVtVon
            if (itSearchData.AngAntVtVon.HasValue)
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pAngAntVtVon", Value = itSearchData.AngAntVtVon.Value.Date.ToString("yyyy-MM-dd") });
            }

            if (itSearchData.AngAntVtBis.HasValue)
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pAngAntVtBis", Value = itSearchData.AngAntVtBis.Value.Date.ToString("yyyy-MM-dd") });
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzBEZEICHNUNG))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFzBEZEICHNUNG", Value = "%" + itSearchData.FzBEZEICHNUNG.Trim() + "%" });
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.FzKENNZEICHEN))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFzKENNZEICHEN", Value = "%" + itSearchData.FzKENNZEICHEN.Trim() + "%" });
            }

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(itSearchData.Finanzierungsart))
            {
                ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pFinanzierungsart", Value = "%" + itSearchData.Finanzierungsart.Trim() + "%" });
            }
            return ParametersList.ToArray();
        }

        private int MyDeliverITVTCount(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, long sysIt)
        {
            // TODO JJ 0 JJ, Move to the hepler
            IQueryable<VT> Query;

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (itSearchData == null)
            {
                throw new ArgumentException("itSearchData");
            }

            try
            {
                // Create query
                //  Query = context.CreateQuery<VT>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(VT)));
            }
            catch
            {
                throw;
            }

            // Check query
            //if (Query != null)
            {
                // SysIt
                Query = (from o in context.VT
                         where o.SYSIT == sysIt
                         select o);
            }

            return Query.Count();
        }

        private int MyDeliverITANTRAGCount(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, long sysIt)
        {
            // TODO JJ 0 JJ, Move to the hepler
            IQueryable<ANTRAG> Query;

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (itSearchData == null)
            {
                throw new ArgumentException("itSearchData");
            }

            try
            {
                // Create query
                //Query = context.CreateQuery<ANTRAG>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(ANTRAG)));
            }
            catch
            {
                throw;
            }

            // Check query
            // if (Query != null)
            {
                // SysIt
                //Query = Query.Where(o => o.SYSIT == sysIt);
                Query = (from o in context.ANTRAG
                         where o.SYSIT == sysIt
                         select o);
            }

            return Query.Count();
        }

        private int MyDeliverITANGEBOTCount(DdOlExtended context, Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, long sysIt)
        {
            // TODO JJ 0 JJ, Move to the hepler
            IQueryable<ANGEBOT> Query;

            if (context == null)
            {
                throw new ArgumentException("context");
            }

            if (itSearchData == null)
            {
                throw new ArgumentException("itSearchData");
            }

            try
            {
                // Create query
                //    Query = context.CreateQuery<ANGEBOT>(Cic.Basic.Data.Objects.ContextHelper.GetQualifiedEntitySetName(context, typeof(ANGEBOT)));
            }
            catch
            {
                throw;
            }

            // Check query
            // if (Query != null)
            {
                // SysIt
                // Query = Query.Where(o => o.SYSIT == sysIt);
                Query = (from o in context.ANGEBOT
                         where o.SYSIT == sysIt
                         select o);
            }

            return Query.Count();
        }
        #endregion
    }
}