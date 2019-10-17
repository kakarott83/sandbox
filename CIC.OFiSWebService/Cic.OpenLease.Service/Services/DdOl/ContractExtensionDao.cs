using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service.Services.DdOl
{
    /// <summary>
    /// Stores Information about previous contract validation
    /// </summary>
    class ExtensionContractValidationInfo
    {
        public long rangsl { get; set; }
        public long syskalktyp { get; set; }
        public DateTime ende { get; set; }
        public decimal bgextern { get; set; }
        public decimal sz { get; set; }
        public long sysvorvt { get; set; }
        public String schwacke { get; set; }

        public int mvzcounter { get; set; }
        public decimal mvzgesamt { get; set; }
        public decimal urahk { get; set; }
    }
    class ContractInfo
    {
        public long SYSIT { get; set; }
        public long SYSPERSON { get; set; }
        public long SYSVART { get; set; }
    }
    class SubmitInfo
    {
        public long SYSVORVT { get; set; }
        public decimal sz { get; set; }
        public long rangsl { get; set; }
        public long lz { get; set; }
        public int CONTRACTEXT { get; set; }
    }
    public class ContractExtensionDao
    {

        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Validates the extended offer before submitting
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public bool validateSubmit(long sysangebot)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                try
                {
                    SubmitInfo sInfo = Context.ExecuteStoreQuery<SubmitInfo>("select kalktyp.rangsl,angebot.sysvorvt, angkalk.sz from angebot, angkalk, angob, kalktyp where kalktyp.syskalktyp=angkalk.syskalktyp and angob.sysob = angkalk.sysob and angebot.sysid=angkalk.sysangebot and angebot.sysid=" + sysangebot, null).FirstOrDefault();
                    long sysvt = sInfo.SYSVORVT;
                    ExtensionContractValidationInfo vtInfo = Context.ExecuteStoreQuery<ExtensionContractValidationInfo>("select kalktyp.rangsl, kalktyp.syskalktyp, vt.ende,kalk.bgextern,kalk.sz,sysvorvt from vt,ob,kalk,kalktyp where kalktyp.syskalktyp=kalk.syskalktyp and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysvt, null).FirstOrDefault();

                    int mvzcounter;
                    decimal mvzgesamt = sInfo.sz;
                    decimal urahk;

                    //Nur Leasing
                    if (sInfo.rangsl == 100 && sInfo.sz>0.01M)
                    {
                        vtInfo = calculateContractHistory(sysvt, Context, vtInfo, out mvzcounter, ref mvzgesamt, out urahk);

                        decimal maxQuoteVV = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_MAX_MVZ_VORVERTRAG);
                        if (mvzgesamt > Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(maxQuoteVV / 100 * urahk))
                        {
                            return false;
                        }
                    }



                }
                catch (System.Exception exception)
                {
                    // Log the exception
                    _Log.Error("Vertragsverlängerung konnte nicht geprüft werden: ", exception);
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Validates the extended offer for submitting max 1 month before end
        /// TPA #150515 - Verlängerung w 6 Monate nur im Ablaufmonat durchführen
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public bool validateExtensionMonth(long sysangebot)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                try
                {
                    SubmitInfo sInfo = Context.ExecuteStoreQuery<SubmitInfo>("select sysvorvt, contractext, angkalk.lz from angebot, angkalk, angob, kalktyp where kalktyp.syskalktyp=angkalk.syskalktyp and angob.sysob = angkalk.sysob and angebot.sysid=angkalk.sysangebot and angebot.sysid=" + sysangebot, null).FirstOrDefault();
                    if ((sInfo.CONTRACTEXT == 2 || sInfo.CONTRACTEXT == 1 )&& sInfo.lz < 6)//nur für folgevertrag wenn kleiner 6 Monate
                    {
                        DateTime ende = Context.ExecuteStoreQuery<DateTime>("select vt.ende from vt where vt.sysid=" + sInfo.SYSVORVT, null).FirstOrDefault();
                        if (ende.Year == DateTime.Now.Year)
                        {
                            if (ende.Month > DateTime.Now.Month)//Nur Im Ablaufmonat!
                                return false;
                            if (ende.Month <= DateTime.Now.Month)//Wenn ein Vertragsabläufer mit Ablaufdatum < aktuelles Monat, dann darf die Plausiprüfung nicht greifen
                                return true;
                        }
                        if (ende.Year < DateTime.Now.Year)
                        {
                            return true;//Wenn ein Vertragsabläufer mit Ablaufdatum < aktuelles Monat, dann darf die Plausiprüfung nicht greifen
                        }
                        if (ende.Year > DateTime.Now.Year)
                        {
                            return false;//Nur Im Ablaufmonat!
                        }

                    }
                }
                catch (System.Exception exception)
                {
                    // Log the exception
                    _Log.Error("Vertragsverlängerung konnte nicht geprüft werden: ", exception);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates a contract before extension
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public List<ValidationResultDto> validateVTExtension(long sysvt, int contractext)
        {
            List<ValidationResultDto> rval = new List<ValidationResultDto>();

            using (DdOlExtended Context = new DdOlExtended())
            {
                try
                {
                    ExtensionContractValidationInfo vtInfo = Context.ExecuteStoreQuery<ExtensionContractValidationInfo>("select ob.schwacke, kalktyp.rangsl, kalktyp.syskalktyp, vt.ende,kalk.bgextern,kalk.sz,bmwhist.subjectsysid sysvorvt from vt left outer join bmwhist on bmwhist.sysvt=vt.sysid and bmwhist.aktionid = 'VLG',ob,kalk,kalktyp where kalktyp.syskalktyp=kalk.syskalktyp and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysvt, null).FirstOrDefault();
                    vtInfo.schwacke = trim(vtInfo.schwacke);
                    long rangsl = vtInfo.rangsl;

                    long sysobtyp = Context.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke='" + vtInfo.schwacke + "'", null).FirstOrDefault();
                    if (sysobtyp == 0)
                    {
                        rval.Add(new ValidationResultDto() { valid = false, Message = "Fahrzeug für Schwacke " + vtInfo.schwacke + " konnte nicht aufgelöst werden." });
                    }

                    if (contractext == 2)//Verlängerung
                    {
                        //VT.ENDE 
                        if (rangsl == 100)
                        {
                            if (DateTime.Compare(DateTime.Now, vtInfo.ende.AddMonths(-4)) < 0 || DateTime.Compare(DateTime.Now, vtInfo.ende.AddMonths(6)) > 0)
                                rval.Add(new ValidationResultDto() { valid = false, Message = "Verlängerung nur 4 Monate vor bzw. bis 6 Monate nach Vertragsende möglich. Bitte Verlängerung mittels Neuvertrag durchführen." });
                        }
                        //Bei Kreditvertrag nur im Ablaufmonat
                        if (vtInfo.rangsl == 200)
                        {
                            if (!(vtInfo.ende.Month == DateTime.Now.Month && vtInfo.ende.Year == DateTime.Now.Year))
                                rval.Add(new ValidationResultDto() { valid = false, Message = "Verlängerung nur im Ablaufmonat möglich. Bitte Verlängerung mittels Neuvertrag durchführen." });
                        }

                    }
                    else//Für Neuvertrag:
                    {
                    }

                }
                catch (System.Exception exception)
                {
                    // Log the exception
                    _Log.Error("Vertragsverlängerung konnte nicht geprüft werden: ", exception);
                    rval.Add(new ValidationResultDto() { valid = false, Message = "Vertragsverlängerung konnte nicht geprüft werden: " + exception.Message });
                }

            }

            return rval;
        }

        /// <summary>
        /// Validates a contract before extension
        /// </summary>
        /// <param name="calculationDto"></param>
        /// <param name="isCredit"></param>
        /// <param name="Ust"></param>
        public void validateVTExtension(Cic.OpenLease.ServiceAccess.DdOl.CalculationDto calculationDto, bool isCredit, decimal Ust)
        {
            if (!calculationDto.SYSVORVT.HasValue) return;
            ExtensionContractValidationInfo vtInfo = getExtensionInfo(calculationDto.SYSVORVT.Value);
            if (vtInfo == null) return;
            if (vtInfo.mvzcounter >= 3)//bereits dreimal MVZ für diese vertragshistorie
            {
                calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Lz002;
                calculationDto.Message = "Eine MVZ wurde bereits dreimal eingebracht.";
                calculationDto.MietvorauszahlungBrutto = 0;
                calculationDto.MietvorauszahlungBruttoP = 0;
                return;
            }

            //Nur Leasing
            if (!isCredit)
            {
                QUOTEDao qd = new QUOTEDao();

                decimal maxQuoteVV = (decimal)qd.getQuote(QUOTEDao.QUOTE_MAX_MVZ_VORVERTRAG);
                if (vtInfo.mvzgesamt + Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(calculationDto.MietvorauszahlungBrutto, Ust) > Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(maxQuoteVV / 100 * vtInfo.urahk))
                {
                    calculationDto.MessageCode = ServiceAccess.DdOl.CalculationDto.MessageCodes.Lz002;
                    calculationDto.Message = "MVZ Gesamt darf nicht mehr als " + maxQuoteVV + "% des ursprünglichen Anschaffungswertes betragen.";

                    calculationDto.MietvorauszahlungBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(maxQuoteVV / 100 * vtInfo.urahk) - vtInfo.mvzgesamt;
                    if ((double)calculationDto.MietvorauszahlungBrutto < 0.01) calculationDto.MietvorauszahlungBrutto = 0;
                    calculationDto.MietvorauszahlungBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(calculationDto.MietvorauszahlungBrutto, Ust);
                    if (calculationDto.MietvorauszahlungBrutto < 0) calculationDto.MietvorauszahlungBrutto = 0;
                    calculationDto.MietvorauszahlungBruttoP = KalkulationHelper.CalculateMietvorauszahlungP(calculationDto.MietvorauszahlungBrutto, calculationDto.AnschaffungswertBrutto);

                    return;
                }
            }

        }
        private static CacheDictionary<long, ExtensionContractValidationInfo> vtextCache = CacheFactory<long, ExtensionContractValidationInfo>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private ExtensionContractValidationInfo getExtensionInfo(long sysvt)
        {
            if (!vtextCache.ContainsKey(sysvt))
            {
                using (DdOlExtended Context = new DdOlExtended())
                {


                    try
                    {
                        ExtensionContractValidationInfo vtInfo = Context.ExecuteStoreQuery<ExtensionContractValidationInfo>("select ob.schwacke, kalktyp.rangsl, kalktyp.syskalktyp, vt.ende,kalk.bgextern,kalk.sz,bmwhist.subjectsysid sysvorvt from vt left outer join bmwhist on bmwhist.sysvt=vt.sysid and bmwhist.aktionid = 'VLG',ob,kalk,kalktyp where kalktyp.syskalktyp=kalk.syskalktyp and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysvt, null).FirstOrDefault();
                        vtInfo.schwacke = trim(vtInfo.schwacke);
                        long rangsl = vtInfo.rangsl;
                        int mvzcounter;
                        decimal mvzgesamt = 0;
                        decimal urahk = 0;
                        calculateContractHistory(sysvt, Context, vtInfo, out mvzcounter, ref mvzgesamt, out urahk);
                        vtInfo.urahk = urahk;
                        vtInfo.mvzgesamt = mvzgesamt;
                        vtInfo.mvzcounter = mvzcounter;

                        vtextCache[sysvt] = vtInfo;
                    }
                    catch (System.Exception exception)
                    {
                        // Log the exception
                        _Log.Error("MVZ-Prüfung fehlgeschlagen", exception);
                        return null;
                    }
                }
            }
            return vtextCache[sysvt];
        }

        /// <summary>
        /// Returns the id of previous contracts for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="syspuser"></param>
        /// <returns></returns>
        public String[] getVorvertraege(long sysangebot, long syspuser)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                long sysperson = ctx.ExecuteStoreQuery<long>("select it.sysperson from angebot,it where it.sysit=angebot.sysit and angebot.sysid=" + sysangebot, null).FirstOrDefault();

                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syspuser", Value = syspuser });
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });


                String[] rval = ctx.ExecuteStoreQuery<String>("select vertrag from vt where SYSID IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(:syspuser, 'VT',sysdate)))  and syskd=:sysperson order by vertrag", par.ToArray()).ToArray();
                return rval;
            }
        }

        /// <summary>
        /// Calculates the sum of MVZ and count of previous contracts with MVZ and also returns the original AHK
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="Context"></param>
        /// <param name="vtInfo"></param>
        /// <param name="mvzcounter"></param>
        /// <param name="mvzgesamt"></param>
        /// <param name="urahk"></param>
        /// <returns></returns>
        private static ExtensionContractValidationInfo calculateContractHistory(long sysvt, DdOlExtended Context, ExtensionContractValidationInfo vtInfo, out int mvzcounter, ref decimal mvzgesamt, out decimal urahk)
        {
            //für verlängerung und neuvertrag-----------------------------------------------------
            long sysvv = sysvt;
            int vorvertraege = 0;//anzahl aller vorverträge inkl zu verlängerndem vertrag
            mvzcounter = 0;
            urahk = vtInfo.bgextern;
            do
            {
                vorvertraege++;
                urahk = vtInfo.bgextern;

                //if mvz>0 inc mvzcounter
                if (vtInfo.sz > 0)
                    mvzcounter++;
                mvzgesamt += vtInfo.sz;

                if (vtInfo.sysvorvt > 0)
                {
                    sysvv = vtInfo.sysvorvt;
                    vtInfo = Context.ExecuteStoreQuery<ExtensionContractValidationInfo>("select kalktyp.rangsl, kalktyp.syskalktyp, vt.ende,kalk.bgextern,kalk.sz,sysvorvt from vt,ob,kalk,kalktyp where kalktyp.syskalktyp=kalk.syskalktyp and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + sysvv, null).FirstOrDefault();
                }
                else vtInfo = null;
            } while (vtInfo != null);
            return vtInfo;
        }

        /// <summary>
        /// Fills a new Offer from a Contract
        /// </summary>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        public ANGEBOTDto getExtensionInformation(long sysvt)
        {
            ANGEBOTDto rval = new ANGEBOTDto();
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {


                    String vtDataQuery = "select ob.sysobtyp as SYSOBTYP,kalk.syskalktyp as ANGKALKSYSKALKTYP,  obini.erstzul as ANGOBINIERSTZUL,ob.liefer as ANGOBLIEFERUNG, ob.schwacke as ANGOBSCHWACKE, ob.polster as ANGOBPOLSTERCODE, vt.syskd as SYSFSTYPPETROL, vt.sysvart, vt.vertrag vorvertrag, obini.farbe_a angobfarbea, obini.vorbesitzer angobinivorbesitzer, ob.serie angobserie, obini.erstzul angobinierstzul, ob.hersteller angobhersteller, ob.bezeichnung angobtyp, ob.objektvt angobfabrikat, obini.kmstand as angobinikmstand, 0 as angobgrundrabattop, 0 as angobgrundrabatto, 0 as angobsonzubrabattop, 0 as angobsonzubrabatto, 0 as angobpaketerabattop, 0 as angobpaketerabatto, 0 as angobherzubrabattop, 0 as angobherzubrabatto, 0 as angobzubehoerrabattop, 0 as angobzubehoerrabatto, kalk.grund as angobgrund, kalk.sonder as angobsonzub, kalk.spaket as angobpakete, kalk.zubehoer as angobherzub, 0 as angobzubehoerbrutto, 0 as angobnovahzuabbrutto, kalk.gesamt as angobahk, kalk.rwbase as angobahkextern, ob.jahreskm as angobjahreskm, ob.kennzeichen as angobvorkennzeichen, 0 as angkalksz, 0 as angkalkszbrutto, 0 as angkalkszbruttop, 0 as angkalkdepotp, 12 as angkalklz, kalk.depot as ANGKALKDEPOT from vt,kalk,ob left outer join obini on obini.sysobini=ob.sysob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=";


                    //IT am Vertrag prüfen, falls ja, nur sysit kopieren
                    //falls nein, neuen it aus person anlegen, diese sysit zuweisen
                    rval = context.ExecuteStoreQuery<ANGEBOTDto>(vtDataQuery + sysvt, null).FirstOrDefault();
                    rval.ANGOBSCHWACKE = trim(rval.ANGOBSCHWACKE);
                    rval.SYSOBTYP = context.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke='" + rval.ANGOBSCHWACKE + "'", null).FirstOrDefault();

                    decimal Ust = LsAddHelper.GetTaxRate(context, rval.SYSVART);
                    rval.ANGOBGRUNDBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBGRUND.Value, Ust);
                    rval.ANGOBSONZUBBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBSONZUB.Value, Ust);
                    rval.ANGOBPAKETEBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBPAKETE.Value, Ust);
                    rval.ANGOBHERZUBBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBHERZUB.Value, Ust);
                    rval.ANGOBAHKBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBAHK.Value, Ust);
                    rval.ANGOBAHKEXTERNBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBAHKEXTERN.Value, Ust);

                }


                return rval;
            }
            catch (System.Exception exception)
            {
                // Log the exception
                _Log.Error("Fehler beim Lesen der Folgevertragsinformation aus dem Angebot", exception);
                throw exception;
            }
        }

        /// <summary>
        /// Creates a new Extension Offer from a Contract
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <param name="sysperole"></param>
        /// <param name="syspuser"></param>
        /// <returns></returns>
        public ANGEBOTDto getAngebotFromVertrag(long sysvt, int contractext, long sysperole, long syspuser)
        {
            ANGEBOTDto rval = new ANGEBOTDto();
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {

                    String vtDataQuery = "select vt.SYSBERATADDB, vt.einzug EINZUG,vertrag VORVERTRAGSNUMMER,obini.nova_p as ANGOBININOVA_P, vt.vertriebsweg as HIST_SYSPRPRODUCT, vt.ende as ANGOBLIEFERUNG, ob.sysobtyp as SYSOBTYP,kalk.syskalktyp as ANGKALKSYSKALKTYP, ob.schwacke as ANGOBSCHWACKE, ob.polster as ANGOBPOLSTERCODE, vt.syskd as SYSFSTYPPETROL, vt.sysvart, vt.vertrag vorvertrag, obini.farbe_a angobfarbea, obini.vorbesitzer angobinivorbesitzer, ob.serie angobserie, obini.erstzul angobinierstzul, ob.hersteller angobhersteller, ob.bezeichnung angobtyp, ob.objektvt angobfabrikat, 0 as angobinikmstand, 0 as angobgrundrabattop, 0 as angobgrundrabatto, 0 as angobsonzubrabattop, 0 as angobsonzubrabatto, 0 as angobpaketerabattop, 0 as angobpaketerabatto, 0 as angobherzubrabattop, 0 as angobherzubrabatto, 0 as angobzubehoerrabattop, 0 as angobzubehoerrabatto, kalk.grund as angobgrund, kalk.sonder as angobsonzub, kalk.spaket as angobpakete, kalk.zubehoer as angobherzub, 0 as angobzubehoerbrutto, 0 as angobnovahzuabbrutto, kalk.gesamt as angobahk, kalk.rw as angobahkextern, ob.jahreskm as angobjahreskm, ob.kennzeichen as angobvorkennzeichen, 0 as angkalksz, 0 as angkalkszbrutto, 0 as angkalkszbruttop, 0 as angkalkdepotp, 12 as angkalklz, kalk.depot as ANGKALKDEPOT from vt,kalk,ob left outer join obini on obini.sysobini=ob.sysob where kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=";
                    // kalk.syskalk=vtobsl.syskalk and
                    //vtobsl.variabel as angkalkzinstyp
                    //HIST_SYSPRPRODUCT for temp storage of vertriebsweg


                    //ob.lieferung für lieferdatum

                   
                    rval = context.ExecuteStoreQuery<ANGEBOTDto>(vtDataQuery + sysvt, null).FirstOrDefault();
                    rval.SYSKALKTYPVORVT = rval.ANGKALKSYSKALKTYP;

                    String mandant = rval.HIST_SYSPRPRODUCT.Trim();
                    mandant = mandant.Substring(0, mandant.IndexOf(' ')).Trim();
                    rval.SYSBRAND = context.ExecuteStoreQuery<long>("select sysbrand from brand where mandant='" + mandant + "'", null).FirstOrDefault();
                    rval.ANGKALKZINSTYP = context.ExecuteStoreQuery<int>("select vtobsl.variabel from vtobsl where rang=1000 and syskalk=" + sysvt, null).FirstOrDefault();
                    if (!rval.ANGKALKZINSTYP.HasValue || rval.ANGKALKZINSTYP == 0)
                        rval.ANGKALKZINSTYP = 1;

                    rval.SYSVORVT = sysvt;
                    rval.ANGOBSCHWACKE = trim(rval.ANGOBSCHWACKE);
                    rval.ANGOBINIVORBESITZER = trim(rval.ANGOBINIVORBESITZER);

                    
                    rval.SYSOBTYP = context.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke='" + rval.ANGOBSCHWACKE + "'", null).FirstOrDefault();
                    if (!rval.SYSOBTYP.HasValue || rval.SYSOBTYP.Value == 0)
                    {
                        _Log.Error("No Contract Extension possible for " + sysvt + ": no SCHWACKE-Code found: " + rval.ANGOBSCHWACKE);
                        return null;
                    }

                    //IT am Vertrag prüfen, falls ja, nur sysit kopieren
                    rval.SYSIT = context.ExecuteStoreQuery<long>("select vt.sysit from vt,person,it where vt.syskd=person.sysperson and person.sysperson=it.sysperson and it.sysit=vt.sysit and vt.sysid=" + sysvt, null).FirstOrDefault();
                    
                    //falls nein, neuen it aus person anlegen, diese sysit zuweisen
                    if (!rval.SYSIT.HasValue || rval.SYSIT.Value == 0)
                    {
                        rval.SYSIT = createItFromPerson(rval.SYSFSTYPPETROL.Value, context, sysperole, syspuser, sysvt);
                    }
                    else if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(context, (long)syspuser, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, (long)rval.SYSIT.Value))
                    {
                        PEUNIHelper.ConnectNodes(context,  PEUNIHelper.Areas.IT, rval.SYSIT.Value, sysperole);
                        context.SaveChanges();
                    }
                    //zwingend SYSKI auf SYSIT stellen, falls ein Kontoinhaber zugewiesen ist, die GUI kann nur diesen bearbeiten!
                    if (rval.EINZUG.HasValue && rval.EINZUG.Value == 1)
                    {
                        rval.SYSKI = rval.SYSIT;
                    }
                    else
                    {
                        rval.SYSKI = null;
                    }
                    //update bankdaten from mandates:
                    new BankdatenDao().updateITBankdaten(rval.SYSIT.Value, rval.SYSVART.HasValue?rval.SYSVART.Value:0, sysperole);

                    rval.SYSFSTYPPETROL = 0; //Used to transfer the sysperson-value!
                    rval.ANGEBOT1 = ""; //erst beim erstmaligen speichern zugewiesen, von WEM???? Trigger über contractext?
                    //Vt-Daten in AngebotDto kopieren
                    rval.CONTRACTEXT = contractext;
                    rval.SYSOBART = OBARTHelper.getObartOfType(context, OBARTHelper.CnstObArtUsed);

                    NoVA nv = new NoVA(context);
                    TechnicalDataDto techData = new TechnicalDataDto();
                    try
                    {
                        // Fix for SA3 Bugs fetch in Netvision BmwTechnicalDataDto.fetchListenpreis = true;//Fix #4103
                        techData.sysobtyp = rval.SYSOBTYP.Value;
                        nv.fetchTechnicalDataFromEurotax(rval.ANGOBSCHWACKE, rval.SYSOBART.Value, techData,sysperole);
                    }
                    catch (Exception e)
                    {
                        _Log.Warn("Schwacke-Values for obtyp not found, using FS-Tables: OBtyp:" + rval.SYSOBTYP, e);
                        OBTYP CurrentObTyp = (from ObTyp in context.OBTYP
                                              where ObTyp.SCHWACKE == rval.ANGOBSCHWACKE
                                              select ObTyp).FirstOrDefault();
                        nv.fetchTechnicalDataFromFzTyp(techData, CurrentObTyp,sysperole,context);
                    }
                    rval.ANGOBINICO2 = (long)techData.CO2Emission;
                    rval.ANGOBINIPARTICLES = (double)techData.Particles;
                    rval.ANGOBININOX = (double)techData.NOXEmission;
                    rval.ANGOBINIVERBRAUCH_D = techData.Verbrauch;
                    rval.ANGOBINIKW = techData.Kw;

                    rval.ANGOBNOVA = 0;// not yet known if customer has not to pay nova (value = 1)
                    rval.ANGOBCCM = techData.Ccm;
                    rval.ANGOBKW = techData.Kw;

                    rval.ANGOBININOVA_P = techData.NovaSatz;
                    rval.ANGOBNOVAP = rval.ANGOBININOVA_P;
                    rval.ANGOBNOVAPDEF = rval.ANGOBININOVA_P;

                    //CO2 Reifen
                    //Defaults, they wont be changed ever,                    
                    rval.ANGOBINICO2DEF = rval.ANGOBINICO2;
                    rval.ANGOBININOXDEF = rval.ANGOBININOX;
                    rval.ANGOBINIVERBRAUCH_DDEF = rval.ANGOBINIVERBRAUCH_D;
                    rval.ANGOBINIPARTICLESDEF = rval.ANGOBINIPARTICLES;

                    if (contractext == 2)//Verlängerung
                        rval.CONTRACTTYPE = 2; //Folgevertrag
                    if (contractext == 1)//Neuvertrag
                        rval.CONTRACTTYPE = 0;
                    rval.ERFASSUNG = DateTime.Today;

                    String angebotDataQuery = "select  angob.configpictureurl as ANGOBPICTUREURL , angob.configsource as HIST_SYSPRPRODUCT, angob.configid as ANGOBCONFIGID, angob.polstercode as ANGOBPOLSTERCODE, angob.polstertext as ANGOBPOLSTERTEXT from angebot, vt,angob where angob.sysob = angebot.sysid and angebot.sysid=vt.sysangebot and vt.sysid=";
                    ANGEBOTDto angebotInfo = context.ExecuteStoreQuery<ANGEBOTDto>(angebotDataQuery + sysvt, null).FirstOrDefault();

                    if (angebotInfo != null)
                    {
                        rval.ANGOBPICTUREURL = angebotInfo.ANGOBPICTUREURL;
                        if (angebotInfo.HIST_SYSPRPRODUCT != null)
                            rval.ANGOBCONFIGSOURCE = (Cic.OpenLease.ServiceAccess.OfferTypeConstants)Enum.Parse(typeof(OfferTypeConstants), angebotInfo.HIST_SYSPRPRODUCT, true);
                        rval.ANGOBCONFIGID = angebotInfo.ANGOBCONFIGID;
                        rval.ANGOBPOLSTERCODE = angebotInfo.ANGOBPOLSTERCODE;
                        rval.ANGOBPOLSTERTEXT = angebotInfo.ANGOBPOLSTERTEXT;
                    }


                    decimal Ust = LsAddHelper.GetTaxRate(context, rval.SYSVART);
                    rval.ANGOBGRUNDBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBGRUND.Value, Ust);
                    rval.ANGOBSONZUBBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBSONZUB.Value, Ust);
                    rval.ANGOBPAKETEBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBPAKETE.Value, Ust);
                    rval.ANGOBHERZUBBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBHERZUB.Value, Ust);
                    rval.ANGOBAHKBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBAHK.Value, Ust);
                    rval.ANGOBAHKEXTERNBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(rval.ANGOBAHKEXTERN.Value, Ust);
                    rval.HIST_SYSPRPRODUCT = null;

                    //Restwert resetten
                    rval.ANGKALKRWKALK = 0;
                    rval.ANGKALKRWKALK_DEFAULT = 0;
                    rval.ANGKALKRWKALK_SUBVENTION = 0;
                    rval.ANGKALKRWKALKBRUTTO = 0;
                    rval.ANGKALKRWKALKBRUTTO_DEFAULT = 0;
                    rval.ANGKALKRWKALKBRUTTO_SUBVENTION = 0;
                    rval.ANGKALKRWKALKUST = 0;
                    rval.ANGKALKRWKALKUST_SUBVENTION = 0;
                    rval.ANGKALKRWKALKUST_DEFAULT = 0;
                    rval.ANGKALKRWKALKBRUTTOP = 0;
                    rval.ANGKALKRWKALKBRUTTOP_DEFAULT = 0;
                    rval.ANGKALKRWKALKBRUTTOP_SUBVENTION = 0;
                    rval.ANGKALKRWBASE = 0;
                    rval.ANGKALKRWBASEBRUTTO = 0;
                    rval.ANGKALKRWBASEBRUTTOP = 0;
                    rval.ANGKALKRWBASEUST = 0;
                    rval.ANGKALKRWCRV = 0;
                    rval.ANGKALKRWCRVBRUTTO = 0;
                    rval.ANGKALKRWCRVBRUTTOP = 0;
                    rval.ANGKALKRWCRVUST = 0;

                    rval.ANGKALKRWKALKBRUTTOPORG = 0;
                    rval.ANGKALKRWKALKBRUTTOORG = 0;
                    rval.VVDEPOT = rval.ANGKALKDEPOT > 0;//Für GUI zur Anzeige falls vv ein depot hatte
                    rval.ANGKALKDEPOT = 0;
                    rval.ANGKALKDEPOTP = 0;
                    //rval.KALKULATIONSOURCE = BmwCalculationDto.CalculationSources.
                    rval.SYSPRPRODUCT = 0;
                    DateTime ende = rval.ANGOBLIEFERUNG.HasValue ? rval.ANGOBLIEFERUNG.Value : DateTime.Now;
                    rval.ANGOBLIEFERUNG = DateTimeHelper.getUltimo(ende);
                }


                return rval;
            }
            catch (System.Exception exception)
            {
                // Log the exception
                _Log.Error("Fehler beim Erzeugen des Folgevertrags-Angebots", exception);
                throw exception;
            }
        }

        /// <summary>
        /// creates an it from a contracts person
        /// </summary>
        /// <param name="sysperson"></param>
        /// <param name="context"></param>
        /// <param name="sysperole"></param>
        /// <param name="syspuser"></param>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        public long createItFromPerson(long sysperson, DdOlExtended context, long sysperole, long syspuser, long sysvt)
        {
            try
            {
                ITAssembler asm = new Cic.OpenLease.Service.ITAssembler(sysperole, syspuser);//for peuni 
                String personItQuery = "select sysperson,privatflag, anrede,geschlecht,syskdtyp,titel,name,vorname, uidnummer, strasse,plz,ort,telefon,fax,email,ptelefon,handy,gebdatum,telefon,hsnr, 0 as beschartag1, gruendung, suffix, syslandnat, sysland,  hobby.ausweisnr, kontonr, hregister, sysbranche, null as wbeguenst, url, 0 as auslagen, 0 as kredrate1, 0 as mietneben, fuehrerschein as ausweisbehoerde, ausbehoerde as ausweisort, rpausstellung as ausweisdatum, 0 as ausweisgueltig, rpgueltigbis as ausweisablauf, legitdatum, legitabnehmer, regon as svnr, null as erreichbtrel, anzkinder as kinderimhaus, wohnungart, wehrersatz as wehrdienst, nettomtl as einknetto, sonstigmtl as nebeneinknetto, zulagen as zeinknetto, sonstigverm as sonstverm, artsonstigverm1 as artsonstverm, miete, 0 as auslagen, 0 as kredrate1, alimente as unterhalt, 0 as mietneben, bart as beruf, arbeitgeber as nameag, agstrasse as strasseag, agplz as plzag, agort as ortag, agsysland as syslandag, beschaeftigtseit as beschseitag, agbefristet as beschbisag, arbeitgeber1 as nameag1, ag1von as beschseitag1, ag1bis as beschbisag1, arbeitgeber2 as nameag2, ag2von as beschseitag2, ag2bis as beschbisag2, arbeitgeber3 as nameag3, ag3von as beschseitag3, ag3bis as beschbisag3 from person, hobby where hobby.syshobby=person.sysperson and person.sysperson=";

                ITDto itDto = context.ExecuteStoreQuery<ITDto>(personItQuery + sysperson, null).FirstOrDefault();

                ItInfo itInfo = context.ExecuteStoreQuery<ItInfo>("select nationalitaet,GESCHLECHT,titel2 as titel, rechtsform, hobby.fluchtausweis,hobby.familienstand from person, hobby where hobby.syshobby=person.sysperson and sysperson=" + sysperson, null).FirstOrDefault();
                itDto.PRIVATFLAG = false;
                itDto.ANREDE = "";
                if (itDto.TITEL != null && itDto.TITEL.Trim().ToLower().Equals("frau"))
                    itDto.ANREDE = "Frau";
                if (itDto.TITEL != null && itDto.TITEL.Trim().ToLower().IndexOf("herr") > -1)
                    itDto.ANREDE = "Herrn";
                itDto.TITEL = "";
                if (itInfo.TITEL != null)
                    itDto.TITEL = itInfo.TITEL.Trim();
                if (itInfo.GESCHLECHT == 0)
                    itDto.GESCHLECHT = ITDto.Sex.Unknown;
                if (itInfo.GESCHLECHT == 1)
                    itDto.GESCHLECHT = ITDto.Sex.Male;
                if (itInfo.GESCHLECHT == 2)
                    itDto.GESCHLECHT = ITDto.Sex.Female;

                if (itInfo.RECHTSFORM != null)
                {
                    itInfo.RECHTSFORM = itInfo.RECHTSFORM.Trim();
                    itDto.SYSKDTYP = 2;
                    String rfname = null;
                    if (itInfo.RECHTSFORM.ToLower().IndexOf("einzel") > -1)
                        rfname = "Einzel";
                    else if (itInfo.RECHTSFORM.ToLower().IndexOf("privat") > -1)
                    {
                        rfname = "Privat";
                        itDto.PRIVATFLAG = true;
                    }

                    if (rfname != null)
                        itDto.SYSKDTYP = context.ExecuteStoreQuery<long>("select syskdtyp from kdtyp where name like '%" + rfname + "%'", null).FirstOrDefault();

                }
                if (!itDto.PRIVATFLAG)
                    itDto.ANREDE = "Firma";

                itInfo.RECHTSFORM = trim(itInfo.RECHTSFORM);
                DictionaryDto[] rformen = DictionaryDao.getDictionaryValues(DictionaryDao.CnstDictionaryRechtsform);
                DictionaryDto dict = (from c in rformen
                                      where c.Result1.Equals(itInfo.RECHTSFORM)
                                      select c).FirstOrDefault();
                if (dict != null)
                    itDto.RECHTSFORM = int.Parse(dict.Result2);
                else
                {
                    DictionaryDto[] rformen2 = DictionaryDao.getDictionaryValues(DictionaryDao.CnstDictionaryRechtsformEinzel);
                    dict = (from c in rformen2
                            where c.Result1.Equals(itInfo.RECHTSFORM)
                            select c).FirstOrDefault();
                    if (dict != null)
                        itDto.RECHTSFORM = int.Parse(dict.Result2);
                }

                itInfo.FLUCHTAUSWEIS = trim(itInfo.FLUCHTAUSWEIS);
                DictionaryDto[] ausweisarten = DictionaryDao.getDictionaryValues(DictionaryDao.CnstDictionaryAusweisart);
                dict = (from c in ausweisarten
                        where c.Result1.Equals(itInfo.FLUCHTAUSWEIS)
                        select c).FirstOrDefault();
                if (dict != null)
                    itDto.AUSWEISART = int.Parse(dict.Result2);

                itInfo.FAMILIENSTAND = trim(itInfo.FAMILIENSTAND);
                DictionaryDto[] familienstand = DictionaryDao.getDictionaryValues(DictionaryDao.CnstDictionaryFamilienstand);
                dict = null;
                if (itInfo.FAMILIENSTAND != null)
                    dict = (from c in familienstand
                            where c.Result1.Contains(itInfo.FAMILIENSTAND)
                            select c).FirstOrDefault();
                if (dict != null)
                    itDto.FAMILIENSTAND = int.Parse(dict.Result2);

                itDto.SYSLANDNAT = getSYSLANDNAT(itInfo.NATIONALITAET);

                itDto.WEHRDIENST = trim(itDto.WEHRDIENST);
                if (itDto.WEHRDIENST == null) itDto.WEHRDIENST = "";
                if (itDto.WEHRDIENST.Equals("J"))
                    itDto.WEHRDIENST = "Ja";
                else if (itDto.WEHRDIENST.Equals("N"))
                    itDto.WEHRDIENST = "Nein";
                else if (itDto.WEHRDIENST.Equals("F"))
                    itDto.WEHRDIENST = "Freigestellt";
                itDto.BERUF = trim(itDto.BERUF);
                itDto.EMAIL = trim(itDto.EMAIL);
                itDto.SVNR = trim(itDto.SVNR);
                itDto.ORT = trim(itDto.ORT);
                itDto.NAME = trim(itDto.NAME);
                itDto.HANDY = trim(itDto.HANDY);
                itDto.STRASSE = trim(itDto.STRASSE);
                itDto.VORNAME = trim(itDto.VORNAME);
                itDto.PLZ = trim(itDto.PLZ);
                itDto.FAX = trim(itDto.FAX);
                itDto.ANREDE = trim(itDto.ANREDE);
                itDto.TITEL = trim(itDto.TITEL);
                itDto.NAMEAG1 = trim(itDto.NAMEAG1);
                itDto.URL = trim(itDto.URL);
                itDto.WOHNUNGART = trim(itDto.WOHNUNGART);
                itDto.ARTSONSTVERM = trim(itDto.ARTSONSTVERM);
                itDto.NAMEAG = trim(itDto.NAMEAG);
                itDto.NAMEAG2 = trim(itDto.NAMEAG2);
                itDto.NAMEAG3 = trim(itDto.NAMEAG3);
                itDto.STRASSEAG = trim(itDto.STRASSEAG);
                itDto.PLZAG = trim(itDto.PLZAG);
                itDto.ORTAG = trim(itDto.ORTAG);
                itDto.SUFFIX = trim(itDto.SUFFIX);
                itDto.TELEFON = trim(itDto.TELEFON);
                itDto.TELEFONKONT = trim(itDto.TELEFONKONT);
                itDto.PTELEFON = trim(itDto.PTELEFON);

                itDto.AUSWEISBEHOERDE = trim(itDto.AUSWEISBEHOERDE);
                itDto.AUSWEISORT = trim(itDto.AUSWEISORT);
                //set default to Österreich when 0
                if (!itDto.SYSLANDNAT.HasValue || itDto.SYSLANDNAT.Value == 0)
                    itDto.SYSLANDNAT = 127;

                KtoInfo ktoInfo = context.ExecuteStoreQuery<KtoInfo>("select replace(konto.iban,' ','') iban,trim(konto.kontonr) kontonr,trim(blz.name) bankname, trim(blz.blz) blz, trim(blz.bic) bic from konto,blz,vt where  blz.sysblz=konto.sysblz and konto.rang=vt.rangki and sysperson=vt.syskd and vt.sysid=" + sysvt + " and (konto.rang<90 or konto.rang>100)  and konto.rang<900 order by konto.rang desc", null).FirstOrDefault();
                if (ktoInfo == null)
                    ktoInfo = context.ExecuteStoreQuery<KtoInfo>("select replace(konto.iban,' ','') iban,trim(konto.kontonr) kontonr,trim(blz.name) bankname, trim(blz.blz) blz, trim(blz.bic) bic from konto,blz,vt where blz.sysblz=konto.sysblz and sysperson=vt.syskd and vt.sysid=" + sysvt + " and  (konto.rang<90 or konto.rang>100) and konto.rang<900 order by konto.rang desc", null).FirstOrDefault();
                if (ktoInfo != null)
                {

                    itDto.BANKNAME = ktoInfo.BANKNAME;
                    itDto.BLZ = ktoInfo.BLZ;
                    itDto.IBAN = ktoInfo.IBAN;
                    itDto.BIC = ktoInfo.BIC;
                    itDto.KONTONR = ktoInfo.KONTONR;
                    // try avoid output of 'null' as Vorname: itDto.KONTOINHABER = itDto.VORNAME + " " + itDto.NAME;
					itDto.KONTOINHABER = itDto.VORNAME != null ? (itDto.VORNAME + " ") : "" + itDto.NAME;

					if (itDto.IBAN != null)
                    {
                        IBANValidator ib = new IBANValidator();
                        //change to correct BIC
                        IBANValidationError ibanerror = ib.checkIBANandBIC(itDto.IBAN, itDto.BIC);
                        if (ibanerror.newBIC != null)
                            itDto.BIC = ibanerror.newBIC;
                    }
                }

                //verlängerungsverträge: zahlart übernehmen
                MANDAT vvtMandat = (from p in context.MANDAT
                                    where p.AREA.Equals("VT") && p.SYSID == sysvt && p.VERSION == 0
                             select p).FirstOrDefault();
                if(vvtMandat!=null)
                    itDto.PAYART = vvtMandat.PAYART.HasValue?vvtMandat.PAYART.Value:0;

                IT newIt = asm.Create(itDto);
                
                return newIt.SYSIT;
            }
            catch (System.Exception exception)
            {
                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.getItFromVTPersonFailed, exception);
                throw exception;
            }
        }

        /// <summary>
        /// returns the id of the nationality
        /// </summary>
        /// <param name="nationalitaet"></param>
        /// <returns></returns>
        public static long getSYSLANDNAT(String nationalitaet)
        {
            String nat = trim(nationalitaet);
            if (nat != null)
            {

                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> LANDDtoList = DictionaryDao.getLaender();
                Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto l = (from c in LANDDtoList
                                                                          where c.COUNTRYNAME.Contains(nat)
                                                                          select c).FirstOrDefault();
                if (l != null)
                    return l.SYSLAND;
            }
            return 0;

        }

        /// <summary>
        /// Trims a string and tests for null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static String trim(String str)
        {
            if (str == null) return null;
            String rval = str.Trim();
            if (rval.Length == 0)
                return null;
            return rval;
        }
    }

    /// <summary>
    /// Internal IT value holder
    /// </summary>
    class ItInfo
    {
        public long GESCHLECHT { get; set; }
        public String TITEL { get; set; }
        public String RECHTSFORM { get; set; }
        public String FLUCHTAUSWEIS { get; set; }
        public String FAMILIENSTAND { get; set; }
        public String NATIONALITAET { get; set; }
    }
    /// <summary>
    /// Internal Konto value holder
    /// </summary>
    public class KtoInfo
    {
        public String IBAN { get; set; }
        public String BLZ { get; set; }
        public String BIC { get; set; }
        public String BANKNAME { get; set; }
        public String KONTONR { get; set; }
        public int? PAYART { get; set; }
    }
}