using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cic.OpenLease.Service.Services.DdOl
{
    class BankAngebotInfo
    {
        public long sysls { get; set; }
        public long sysit { get; set; }
    }
    class VorgaengeIds
    {
        public long sysvt { get; set; }
        public long sysangebot { get; set; }
        public long sysantrag { get; set; }
    }
    public class BankdatenDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int KONTOINHABERSICHRANG = 300;
        private const int KONTOINHABERSICHTYP = 300;

        private static String QUERYCHANGEBANKDATENALLOWED = "select  vt.zustand from angebot, vt,angob where angob.sysob = angebot.sysid and angebot.sysid=vt.sysangebot and angebot.sysid=:sysID";
        private static String QUERYCHANGEBANKDATENALLOWED1 = "select  zustand from angebot where angebot.sysid=:sysID";
        private static String QUERYAVAILGLOBMANDAT = "select mandat.sysls, mandat.sysmandat, konto.syskonto, konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,it,mandat where mandat.syskonto=konto.syskonto and blz.sysblz=konto.sysblz and konto.sysperson=it.sysperson and  it.sysit=:sysit and konto.rang<900 and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) and replace(konto.iban,' ','')=replace(:iban,' ','') and mandat.status in (1,2,3)  and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and mandat.area is null and mandat.sysls=:sysls and mandat.payart=:payart order by status desc,konto.rang desc, mandat.sysmandat desc";
        //querys to find a konto
        private static String QUERYAREAKONTONOITMATCH = "select konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,mandat,it where  blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and (mandat.sysperson=it.sysperson or mandat.sysit=it.sysit) and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and mandat.sysls=:sysls and mandat.area=:area and mandat.sysid=:sysid and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) order by status desc, mandat.sysmandat desc";
        private static String QUERYAREAKONTO = "select konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,mandat,it where  blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and (mandat.sysperson=it.sysperson or mandat.sysit=it.sysit) and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and mandat.sysls=:sysls and mandat.area=:area and mandat.sysid=:sysid and replace(konto.iban,' ','')=replace(it.iban,' ','') and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) order by status desc, mandat.sysmandat desc";
        //find mandate with matching it's iban with the mandat iban and matching the mandant
        private static String QUERYGLOBALMANDATE = "select mandat.signcity,konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz, konto,mandat,it where mandat.area is null and  blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and (mandat.sysperson=it.sysperson or mandat.sysit=it.sysit) and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and replace(konto.iban,' ','')=replace(it.iban,' ','') and mandat.sysls=:sysls and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) order by status desc, mandat.sysmandat desc";
        //find mandate without matching it-iban to mandat iban
        private static String QUERYGLOBALMANDATENOITMATCH = "select mandat.signcity,konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz, konto,mandat,it where mandat.area is null and  blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and  (mandat.sysperson=it.sysperson or mandat.sysit=it.sysit) and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and mandat.sysls=:sysls and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) order by status desc, mandat.sysmandat desc";
        //find mandate with matching it's iban with the mandat iban and NOT matching the mandant
        private static String QUERYGLOBALMANDATENOMANDANT = "select mandat.signcity,konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz, konto,mandat,it where  mandat.area is null and blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and  (mandat.sysperson=it.sysperson or mandat.sysit=it.sysit) and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) order by status desc, mandat.sysmandat desc";

        private static String QUERYITKONTOMANDAT = "select konto.syskonto, mandat.sysmandat,konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,mandat where  blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and status in (1,2,3) and mandat.sysls=:sysls and mandat.area is null and konto.syskonto=:syskonto and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) and mandat.payart=:payart order by status desc, mandat.sysmandat desc";

        private static String QUERYITGLOBALMANDAT = "select sysmandat from mandat,it where mandat.syskonto is null and mandat.sysit=it.sysit and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and area is null and mandat.sysls=:sysls and mandat.payart=:payart order by status desc, mandat.sysmandat desc";
        private static String QUERYITGLOBALMANDATKONTO = "select sysmandat from mandat,it,konto where mandat.syskonto=konto.syskonto and mandat.sysit=it.sysit and it.sysit=:sysit and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and area is null and replace(konto.iban,' ','')=replace(it.iban,' ','')  and mandat.sysls=:sysls and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) and mandat.payart=:payart order by status desc, mandat.sysmandat desc";


        public static String QUERYITWANDERMANDATANGEBOT = "select sysmandat from mandat,it where mandat.sysit=it.sysit and it.sysit=:sysit  and mandat.sysls=:sysls and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and area='ANGEBOT' and sysid=:sysid";
        public static String QUERYITWANDERMANDATANTRAG = "select sysmandat from mandat,it,antrag where mandat.sysit=it.sysit and it.sysit=:sysit  and mandat.sysls=:sysls and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and area='ANTRAG' and mandat.sysid=antrag.sysid and antrag.sysangebot=:sysid";
        public static String QUERYITWANDERMANDATVT = "select sysmandat from mandat,it,vt where mandat.sysit=it.sysit and it.sysit=:sysit  and mandat.sysls=:sysls and status in (1,2,3) and (mandat.validfrom is null or mandat.validfrom<=sysdate) and  (mandat.validuntil is null or mandat.validuntil>=sysdate) and area='VT' and mandat.sysid=vt.sysid and vt.sysangebot=:sysid";

        private static String QUERYPERSONKONTO = "select konto.syskonto, konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,it where blz.sysblz=konto.sysblz and konto.sysperson=it.sysperson and  it.sysit=:sysit and  konto.rang<900 and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null)   order by konto.rang desc";

        private static String QUERYPERSONKONTOIBAN = "select konto.syskonto, konto.kontonr,trim(blz.name) name ,replace(konto.iban,' ','') iban,trim(blz.bic) bic,trim(blz.blz) blz from blz,konto,it where blz.sysblz=konto.sysblz and konto.sysperson=it.sysperson and  it.sysit=:sysit and konto.rang<900 and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null) and replace(konto.iban,' ','')=replace(:iban,' ','') order by konto.rang desc";

        private static String QUERYITBANKDATEN = "select BIC,IBAN,BANKNAME,KONTONR,BLZ from it where sysit=:sysit";
        // private static String QUERYPERSONKONTONEXTRANG = "select max(konto.rang)+10 rang from blz,konto,it where blz.sysblz=konto.sysblz and konto.sysperson=it.sysperson and  it.sysit=:sysit and  (konto.rang<90) and (konto.bezeichnung not like '%inaktiv%' or konto.bezeichnung is null)  order by konto.rang desc";
        /// <summary>
        /// determines if konto may be changed
        /// only true when no active contract
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public bool changeBankdatenAllowed(long sysAngebot)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysID", Value = sysAngebot });

                String angebotzustand = ctx.ExecuteStoreQuery<String>(QUERYCHANGEBANKDATENALLOWED1, parameters.ToArray()).FirstOrDefault();

                if (!angebotzustand.ToUpper().Equals("GENEHMIGT"))//dont allow bankdatenchanges when there is no antrag/vt - avoid changing while guardean is running
                    return false;

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysID", Value = sysAngebot });

                String vtzustand = ctx.ExecuteStoreQuery<String>(QUERYCHANGEBANKDATENALLOWED, parameters.ToArray()).FirstOrDefault();
                if (vtzustand == null) return true;//no contract

                String[] activeStatus = new String[] { "GEPRUEFT / NICHT AKTIV", "AKTIV", "VERLAENGERT / NICHT AKTIV" };
                if (activeStatus.Contains(vtzustand.ToUpper()))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// updates the konto info in IT from available global mandate or person konto
        /// </summary>
        /// <param name="sysit"></param>
        public String updateITBankdaten(long sysit, long sysvsart, long sysperole)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {

                //Globalmandat
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                BLZInfo blzinfo1 = ctx.ExecuteStoreQuery<BLZInfo>(QUERYGLOBALMANDATENOMANDANT, parameters.ToArray()).FirstOrDefault();
                BLZInfo blzinfo = null;
                if (sysvsart > 0)
                {
                    long sysls = LsAddHelper.getMandantByPEROLE(ctx, sysperole);
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = sysls });
                    blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYGLOBALMANDATENOITMATCH, parameters.ToArray()).FirstOrDefault();
                }

                if (blzinfo == null || blzinfo.IBAN == null)
                    blzinfo = blzinfo1;//try the one without mandant-condition    


                if (blzinfo == null)//IT_PERSON-Konto - suche passende personen-konto-verbindung zur Vorbelegung
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYPERSONKONTO, parameters.ToArray()).FirstOrDefault();
                }
                if (blzinfo == null) return "";

                if (blzinfo.IBAN != null)
                {
                    IBANValidator ib = new IBANValidator();
                    //change to correct BIC
                    IBANValidationError ibanerror = ib.checkIBANandBIC(blzinfo.IBAN, blzinfo.BIC);
                    if (ibanerror.newBIC != null)
                        blzinfo.BIC = ibanerror.newBIC;
                }

                IT i = (from c in ctx.IT
                        where c.SYSIT == sysit
                        select c).FirstOrDefault();
                int payart = 1;
                if (blzinfo.SYSMANDAT > 0)
                {
                    payart = ctx.ExecuteStoreQuery<int>("select payart from mandat where sysmandat=" + blzinfo.SYSMANDAT, null).FirstOrDefault();
                }
                //no change in it when same iban/bic! -> wont log, wont update kontonr!
                //if (i.IBAN != null && i.IBAN.Equals(blzinfo.IBAN) && i.BIC != null && i.BIC.Equals(blzinfo.BIC))
                //     return blzinfo.SIGNCITY;

                StringBuilder sb = new StringBuilder();
                sb.Append("OLD: ");
                sb.Append("IBAN=");
                sb.Append(i.IBAN);
                sb.Append(" BIC=");
                sb.Append(i.BIC);
                sb.Append(" KONTONR=");
                sb.Append(i.KONTONR);
                sb.Append(" BLZ=");
                sb.Append(i.BLZ);
                sb.Append(" NEW: ");
                sb.Append("IBAN=");
                sb.Append(blzinfo.IBAN);
                sb.Append(" BIC=");
                sb.Append(blzinfo.BIC);
                sb.Append(" KONTONR=");
                sb.Append(blzinfo.KONTONR);
                sb.Append(" BLZ=");
                sb.Append(blzinfo.BLZ);
                sb.Append(" PAYART=");
                sb.Append(payart);
                LogHelper.logToDatabase("OFISWebService updateITBankdaten()", sb.ToString(), "IT", sysit);

                i.BANKNAME = blzinfo.NAME;
                i.BIC = blzinfo.BIC;
                i.IBAN = blzinfo.IBAN;

                i.BLZ = blzinfo.BLZ;
                i.KONTONR = blzinfo.KONTONR;
                i.PAYART = payart;
                ctx.SaveChanges();
                return blzinfo.SIGNCITY;
            }
        }

        /// <summary>
        /// returns the default signcity for the current salesperson
        /// </summary>
        /// <param name="vpsysperson"></param>
        /// <returns></returns>
        public static String getDefaultSigncity(long vpsysperson)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                return ctx.ExecuteStoreQuery<String>("select ort from person where sysperson=" + vpsysperson, null).FirstOrDefault();
            }
        }

        /// <summary>
        /// delivers the current active konto or a suitable preselection used for the offer when the offer has been saved already
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <param name="checkRangki">when true, returns the currently assigned konto of the angebot from its mandatreferenz</param>
        /// <returns></returns>
        public static Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto DeliverBankdaten(long sysAngebot, bool checkRangki, bool noitibanMatch)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (checkRangki)
                {
                    BankdatenDto curBankdaten = ctx.ExecuteStoreQuery<BankdatenDto>("SELECT angebot.SYSIT, angebot.SYSLS, angebot.sysvart,angebot.einzug,angebot.syski, angebot.sysid sysangebot,mandat.payart,konto.iban,blz.name, blz.bic,mandat.signcity mandatsort from angebot,mandat,konto,blz where blz.sysblz=konto.sysblz and konto.syskonto=mandat.syskonto and mandat.referenz=angebot.mandatreferenz and angebot.rangki is not null and angebot.rangki>0 and angebot.sysid=" + sysAngebot, null).FirstOrDefault();
                    if (curBankdaten != null && curBankdaten.SYSIT > 0)
                    {
                        if (curBankdaten.SYSKI == 0)//wenn ohne einzug eingereicht wurde ist syski leer!
                            curBankdaten.SYSKI = curBankdaten.SYSIT;
                        if (curBankdaten.MANDATSORT == null || curBankdaten.MANDATSORT.Length == 0)
                        { //preselected signcity if not yet in mandate
                            curBankdaten.MANDATSORT = ctx.ExecuteStoreQuery<String>("select ort from person,angebot where sysperson=angebot.sysvk and angebot.sysid=" + sysAngebot, null).FirstOrDefault();
                        }
                        return curBankdaten;
                    }
                }
                //Prüfung, ob ein Vertrag für das Angebot vorhanden ist: area=VT
                //Sonst, prüfen, ob ein Antrag für das Angebot vorhanden ist: area:ANTRAG
                //Sonst area:ANGEBOT
                BankdatenDto rval = ctx.ExecuteStoreQuery<BankdatenDto>("SELECT SYSIT, SYSLS, sysvart,einzug,syski, sysid sysangebot from angebot where sysid=" + sysAngebot, null).FirstOrDefault();


                if (rval.SYSKI == 0)//wenn ohne einzug eingereicht wurde ist syski leer!
                    rval.SYSKI = rval.SYSIT;

                BLZInfo blzinfo = getBankinfos(ctx, rval, noitibanMatch);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                if (blzinfo == null)//IT_PERSON-Konto
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = rval.SYSKI });
                    blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYPERSONKONTO, parameters.ToArray()).FirstOrDefault();
                }
                if (blzinfo == null && rval.EINZUG == 0)//angebot wurde ohne einzug eingereicht, daher keine konto/mandatsdaten vorhanden
                //BMWASEPFU-137 - wenn EINZUG=1 und Bankdaten mitlerweile ungültig, nichts mehr darstellen
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = rval.SYSKI });
                    blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYITBANKDATEN, parameters.ToArray()).FirstOrDefault();
                }
                if (blzinfo != null)
                {
                    if (blzinfo.SYSMANDAT > 0)
                    {
                        MANDAT m = (from f in ctx.MANDAT
                                    where f.SYSMANDAT == blzinfo.SYSMANDAT
                                    select f).FirstOrDefault();
                        if (m != null)
                        {
                            rval.MANDATSORT = m.SIGNCITY;
                            rval.PAYART = m.PAYART.HasValue ? m.PAYART.Value : 1;
                        }
                    }
                    else
                    {

                        if (rval.MANDATSORT == null || rval.MANDATSORT.Length == 0)
                        { //preselected signcity if not yet in mandate
                            rval.MANDATSORT = ctx.ExecuteStoreQuery<String>("select ort from person,angebot where sysperson=angebot.sysvk and angebot.sysid=" + sysAngebot, null).FirstOrDefault();
                        }

                    }
                    rval.BANKNAME = blzinfo.NAME;
                    rval.BIC = blzinfo.BIC;
                    rval.IBAN = blzinfo.IBAN;
                }
                else
                {
                    rval.PAYART = 1;
                }

                return rval;
            }

        }

        /// <summary>
        /// finds the konto/mandat-info for the given angebot id
        /// first searches vt, then antrag then angebot then global mandate
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="bankdaten"></param>
        /// <param name="hasAntrag"></param>
        /// <returns></returns>
        public static BLZInfo getBankinfos(DdOlExtended ctx, BankdatenDto bankdaten, bool noitibanMatch)
        {
            VorgaengeIds vorgang = ctx.ExecuteStoreQuery<VorgaengeIds>("select  vt.sysid sysvt, antrag.sysid sysantrag, angebot.sysid sysangebot from angebot left outer join vt on angebot.sysid=vt.sysangebot left outer join antrag on (antrag.sysid=vt.sysantrag or antrag.sysangebot=angebot.sysid) where angebot.sysid=" + bankdaten.SYSANGEBOT, null).FirstOrDefault();
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = vorgang.sysvt });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = "VT" });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });

            BLZInfo blzinfo = null;
            String areaQuery = QUERYAREAKONTO;
            if (noitibanMatch)
            {
                areaQuery = QUERYAREAKONTONOITMATCH;
            }
            if (vorgang.sysvt > 0)
                blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(areaQuery, parameters.ToArray()).FirstOrDefault();
            if (blzinfo == null && vorgang.sysantrag > 0)
            {
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = vorgang.sysantrag });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = "ANTRAG" });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(areaQuery, parameters.ToArray()).FirstOrDefault();
            }
            if (blzinfo == null && vorgang.sysangebot > 0)
            {
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = vorgang.sysangebot });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = "ANGEBOT" });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                if (bankdaten.SYSANGEBOT > 0)
                    blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(areaQuery, parameters.ToArray()).FirstOrDefault();
            }
            if (blzinfo == null)//Globalmandat
            {
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                String query = QUERYGLOBALMANDATE;
                if (noitibanMatch)//ein eingereichtes angebot darf NICHT mehr über it iban matchen!
                    query = QUERYGLOBALMANDATENOITMATCH;
                blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(query, parameters.ToArray()).FirstOrDefault();
            }
            return blzinfo;
        }

        /// <summary>
        /// creates a konto and changes the einzug/rangki for antrag/vt correspondingly, also updates the sich for antrag/vt
        /// </summary>
        /// <param name="bankdaten"></param>
        /// <param name="sysblz"></param>
        /// <param name="sysperson"></param>
        /// <param name="syskonto"></param>
        /// <param name="sysangobsich"></param>
        /// <returns>the newly created konto syskonto</returns>
        private long createOrUpdateKontodaten(Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto bankdaten, long sysblz, long sysperson, long syskonto, long sysangobsich, long sysmandat)
        {
            if (true) return 0; //disabled for hce

            /*EAIHOT eaihotInput = new Cic.OpenLease.Model.DdOw.EAIHOT();
            eaihotInput.CODE = "AIDA_KONTO";
            eaihotInput.OLTABLE = "ANGEBOT";
            eaihotInput.SYSOLTABLE = bankdaten.SYSANGEBOT;
            eaihotInput.PROZESSSTATUS = 0;
            //eaihotInput.HOSTCOMPUTER = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_HOSTCOMPUTER, "*", CFG);
            //eaihotInput.CLIENTART = int.Parse(Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_CLIENTART, "0", CFG));
            eaihotInput.EVE = 0;// int.Parse(Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFG_SEC, CFG_VAR_EVE, "1", CFG));
            eaihotInput.INPUTPARAMETER1 = "" + bankdaten.SYSANGEBOT;

            using (OwExtendedEntities owCtx = new OwExtendedEntities())
            {
                owCtx.EAIHOT.Add(eaihotInput);
                eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                      where EaiArt.CODE == eaihotInput.CODE
                                      select EaiArt).FirstOrDefault();

                owCtx.SaveChanges();

                EAIQIN param = createEaiQin(eaihotInput.EntityKey, "IBAN", bankdaten.IBAN);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysBLZ", "" + sysblz);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysPERSON", "" + sysperson);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "Einzug", "" + bankdaten.EINZUG);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysKonto", "" + syskonto);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysAngobsich", "" + sysangobsich);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysMandat", "" + sysmandat);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "payart", "" + bankdaten.PAYART);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "mandatort", "" + bankdaten.MANDATSORT);
                owCtx.EAIQIN.Add(param);
                param = createEaiQin(eaihotInput.EntityKey, "sysKi", "" + bankdaten.SYSKI);
                owCtx.EAIQIN.Add(param);
                owCtx.SaveChanges();

                eaihotInput.EVE = 1;
                owCtx.SaveChanges();
                //return 0;
                DateTime oldDate = DateTime.Now;
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 45);
                int pstatus = 0;
                while (pstatus != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    pstatus = owCtx.ExecuteStoreQuery<int>("select prozessstatus from eaihot where syseaihot=" + eaihotInput.SYSEAIHOT, null).FirstOrDefault();
                    System.Threading.Thread.Sleep(500);

                }
                if (pstatus == (int)EaiHotStatusConstants.Ready)
                {
                    return owCtx.ExecuteStoreQuery<int>("select OUTPUTPARAMETER1 from eaihot where syseaihot=" + eaihotInput.SYSEAIHOT, null).FirstOrDefault();
                }
                LogHelper.logToDatabase("AIDA-WS createOrUpdateKontodaten()", "TIMEOUT while creating konto for Angebot, syseaihot: " + eaihotInput.SYSEAIHOT + " status: " + pstatus, "ANGEBOT", bankdaten.SYSANGEBOT);
                throw new Exception("KONTO konnte nicht angelegt werden (Timeout nach 45sec): " + eaihotInput.PROZESSSTATUS);

            }*/
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

            if (ts > timeOut) return true;

            return false;
        }
     /*   private EAIQIN createEaiQin(System.Data.EntityKey syseaihot, string value, string data)
        {
            EAIQIN eaiqinInput = new EAIQIN();
            eaiqinInput.EAIHOTReference.EntityKey = syseaihot;
            eaiqinInput.F01 = value;
            eaiqinInput.F02 = data;
            return eaiqinInput;
        }*/

        /// <summary>
        /// saves the currently used konto for a already submitted offer
        /// a konto must be already there
        /// may create a konto and global-mandate
        /// 
        /// a) Save entered data to IT
        /// b) match it.iban over mandat/konto to find a possibly valid mandate
        /// </summary>
        /// <param name="bankdaten"></param>
        /// <returns></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto SaveBankdaten(Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto bankdaten, long vpsysperson, long sysperole)
        {
            double measure = DateTime.Now.TimeOfDay.Milliseconds;

            using (DdOlExtended ctx = new DdOlExtended())
            {

                IBANValidator ib = new IBANValidator();
                //gui has already validated that!
                IBANValidationError ibanerror = ib.checkIBANandBIC(bankdaten.IBAN, bankdaten.BIC);
                if (ibanerror.newBIC != null)
                    bankdaten.BIC = ibanerror.newBIC;
                if (ibanerror.error != IBANValidationErrorType.NoError || ibanerror.bicwarning)
                {
                    if (bankdaten.EINZUG > 0)
                    {
                        LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "IBAN=" + bankdaten.IBAN + " or BIC=" + bankdaten.BIC + " not valid", "ANGEBOT", bankdaten.SYSANGEBOT);
                        throw new Exception("SaveBankdaten failed: IBAN or BIC not valid");
                    }
                }

                if (bankdaten.SYSLS == 0 && bankdaten.SYSVART > 0)
                {
                    bankdaten.SYSLS = LsAddHelper.getMandantByPEROLE(ctx, sysperole);
                }

                if (bankdaten.SYSKI == 0)//for offer with einzug 0 originally
                    bankdaten.SYSKI = bankdaten.SYSIT;
                _Log.Debug("Duration A " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                //update it bankdaten
                IT i = (from c in ctx.IT
                        where c.SYSIT == bankdaten.SYSKI
                        select c).FirstOrDefault();
                i.IBAN = bankdaten.IBAN;
                i.BIC = bankdaten.BIC;
                i.PAYART = bankdaten.PAYART;
                ctx.SaveChanges();
                _Log.Debug("Duration B " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "IT.IBAN=" + bankdaten.IBAN + " IT.BIC=" + bankdaten.BIC + " IT.PAYART=" + bankdaten.PAYART, "IT", bankdaten.SYSKI);

                VorgaengeIds vorgang = ctx.ExecuteStoreQuery<VorgaengeIds>("select  vt.sysid sysvt, antrag.sysid sysantrag, angebot.sysid sysangebot from angebot left outer join vt on vt.sysangebot=angebot.sysid left outer join antrag on  (antrag.sysid=vt.sysantrag or antrag.sysangebot=angebot.sysid)  where angebot.sysid=" + bankdaten.SYSANGEBOT, null).FirstOrDefault();

                long sysperson = ctx.ExecuteStoreQuery<long>("select sysperson from it where sysit=" + bankdaten.SYSKI).FirstOrDefault();
                long syskonto = 0;
                long sysblz = ibanerror.sysblz;
                long sysmandat = 0;
                //•	suche in allen Konten des Kunden nach Übereinstimmung von IBAN, Bezeichnung nicht wie „%inaktiv%“
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "iban", Value = bankdaten.IBAN });
                BLZInfo blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYPERSONKONTOIBAN, parameters.ToArray()).FirstOrDefault();
                bool mandatfound = false;
                String mandatreferenz = null;
                if (blzinfo != null)//konto gefunden, deshalb mandat suchen
                {
                    syskonto = blzinfo.SYSKONTO;
                    sysblz = blzinfo.SYSBLZ;



                    BLZInfo mandatInfo = getBankinfos(ctx, bankdaten, false);//Hier it iban matchen da ja eben die iban gerade geändert wurde!
                    if (mandatInfo != null && mandatInfo.SYSMANDAT > 0 && false)//disabled for now
                    {
                        MANDAT m = (from s in ctx.MANDAT
                                    where s.SYSMANDAT == mandatInfo.SYSMANDAT
                                    select s).FirstOrDefault();
                        //Weiters muss bei der Mandatsanlage geprüft werden, ob zu dieser Bankverbindung be-reits ein vertragsbezogenes Mandat vorhanden ist. Ist eines vorhanden, so ist der Status des bestehenden Mandates relevant. Ist der Status „Aktiv“, „Versendet“ oder „Neu“, so muss kein neues Mandat angelegt werden.
                        if (m.AREA != null)//angebots/antrags/vt-mandat, darf storniert werden wenn kein bankeinzug mehr gewünscht
                        {
                            if (bankdaten.EINZUG == 0 || bankdaten.SYSKI == 0)//kein einzug oder selbstzahler
                            {
                                m.STATUS = 5;//Storno
                                LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "Mandat " + m.SYSMANDAT + " set to storno, einzug:" + bankdaten.EINZUG + " SYSKI: " + bankdaten.SYSKI, "ANGEBOT", bankdaten.SYSANGEBOT);
                            }
                        }

                        mandatfound = true;
                        sysmandat = m.SYSMANDAT;
                        mandatreferenz = m.REFERENZ;
                        if (m.STATUS <= 1)//falls im status neu einige daten aktualisieren lassen
                        {
                            m.PAYART = bankdaten.PAYART;
                            //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
                            if (!m.PAYART.HasValue || m.PAYART == 0)
                            {
                                m.PAYART = 1;
                            }
                            if (bankdaten.SYSIT != bankdaten.SYSKI)
                            {
                                m.SYSITDEBITOR = bankdaten.SYSIT;
                            }

                            if (!m.SIGNDATE.HasValue && bankdaten.MANDATSORT != null && bankdaten.MANDATSORT.Length > 0)
                            {
                                String newOrt = bankdaten.MANDATSORT;
                                if (newOrt.Length > 40)
                                    newOrt = newOrt.Substring(0, 40);
                                if (!newOrt.Equals(m.SIGNCITY))
                                {
                                    m.SIGNCITY = newOrt;
                                    m.SIGNDATE = DateTime.Now;
                                    m.VALIDFROM = m.SIGNDATE;
                                }
                            }
                        }


                        if (m.STATUS != 5)
                        {
                            LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "Mandat " + m.SYSMANDAT + " found, wont create new Mandat", "ANGEBOT", bankdaten.SYSANGEBOT);
                        }
                    }
                }
                _Log.Debug("Duration C " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                ANGOBSICH sich = createSicherheit(ctx, bankdaten, vpsysperson);
                ctx.SaveChanges();
                _Log.Debug("Duration D " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                if (bankdaten.EINZUG == 0)
                {
                    LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "rangki/einzug=0 for Angebot " + bankdaten.SYSANGEBOT + " Antrag " + vorgang.sysantrag + " VT " + vorgang.sysvt, "ANGEBOT", bankdaten.SYSANGEBOT);



                    //this call is just for the rangki/einzug-update on antrag/vt and to update the sicherheit to inactive, if any
                    createOrUpdateKontodaten(bankdaten, sysblz, sysperson, syskonto, sich != null ? sich.SYSID : 0, sysmandat);

                    ctx.ExecuteStoreCommand("update angebot set rangki=0,einzug=0 where sysid=" + bankdaten.SYSANGEBOT, null);

                    return bankdaten;
                }
                _Log.Debug("Duration E " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                //kein Konto/kein Mandat •	falls nicht gefunden, neuen Eintrag in Konto anlegen, Mandatsuche dann nicht notwendig, da neues konto und damit kein mandat vorhanden sein kann
                syskonto = createOrUpdateKontodaten(bankdaten, sysblz, sysperson, syskonto, sich != null ? sich.SYSID : 0, sysmandat);
                _Log.Debug("Duration F " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;
                long rang = ctx.ExecuteStoreQuery<long>("select rang from konto where syskonto=" + syskonto, null).FirstOrDefault();
                ctx.ExecuteStoreCommand("update angebot set rangki=" + rang + ",einzug=1, syski=" + bankdaten.SYSKI + " where sysid=" + bankdaten.SYSANGEBOT, null);
                LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "rangki=" + rang + "einzug=1 for Angebot " + vorgang.sysangebot + " Antrag " + vorgang.sysantrag + " VT " + vorgang.sysvt, "ANGEBOT", bankdaten.SYSANGEBOT);




                MANDAT mandat = null;
                if (!mandatfound && false)//disabled for now
                {
                    mandat = new MANDAT();
                    //Cic.OpenLease.Model.DdOiqueue.CfgSingleton CfgSingleton = Cic.OpenLease.Model.DdOiqueue.CfgSingleton.Instance;

                    //Get Angebot unique identifier
                    //String ConfigurationValue = CfgSingleton.GetEntry("NKK", "MANDAT_BER", "AIDA", "AIDA");
                    NkBuilder nk = new NkBuilder("MANDAT", "B2B");
                    nk.addBind("sysvart", bankdaten.SYSVART);
                    mandat.REFERENZ = nk.getNextNumber();
                    mandat.VERSION = 0;
                    mandat.SYSKONTO= syskonto;
                    mandat.PAYART = bankdaten.PAYART;
                    mandat.SYSLS= bankdaten.SYSLS;
                    mandat.PAYTYPE = 2;
                    mandat.STATUS = 1;
                    if (bankdaten.SYSIT != bankdaten.SYSKI)
                        mandat.SYSITDEBITOR = bankdaten.SYSIT;
                    if (bankdaten.SYSKI > 0)
                        mandat.SYSIT=bankdaten.SYSKI;
                    if (sysperson > 0)
                        mandat.SYSPERSON = sysperson;

                    //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
                    if (!mandat.PAYART.HasValue || mandat.PAYART == 0)
                    {
                        mandat.PAYART = 1;
                    }
                    //mandat.SIGNCITY = bankdaten.MANDATSORT;
                    mandat.SIGNDATE = DateTime.Now;
                    mandat.VALIDFROM = mandat.SIGNDATE;

                    ctx.MANDAT.Add(mandat);
                    ctx.ExecuteStoreCommand("update angebot set mandatreferenz='" + mandat.REFERENZ + "' where sysid=" + bankdaten.SYSANGEBOT, null);
                }
                else
                {
                    ctx.ExecuteStoreCommand("update angebot set mandatreferenz='" + mandatreferenz + "' where sysid=" + bankdaten.SYSANGEBOT, null);
                    LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "Update Mandatreferenz to " + mandatreferenz, "ANGEBOT", bankdaten.SYSANGEBOT);
                }
                ctx.SaveChanges();
                if (mandat != null)
                {
                    LogHelper.logToDatabase("OFISWebService SaveBankdaten()", "Created new Mandat " + mandat.SYSMANDAT, "ANGEBOT", bankdaten.SYSANGEBOT);
                }
                _Log.Debug("Duration G " + (DateTime.Now.TimeOfDay.Milliseconds - measure)); measure = DateTime.Now.TimeOfDay.Milliseconds;

            }
            return bankdaten;
        }

        /// <summary>
        /// cancels/creates a global mandate
        /// first searches available it/person kontos, if found search for valid mandate of this konto
        /// else create global mandate with NO konto reference!
        /// </summary>
        /// <param name="bankdaten"></param>
        /// <returns></returns>
        public MANDAT createOrUpdateMandat(DdOlExtended ctx, BankdatenDto bankdaten, long vpsysperson)
        {
            if (true) return null; //disabled for now
            

            //long sysperson = ctx.ExecuteStoreQuery<long>("select sysperson from it where sysit=" + bankdaten.SYSKI).FirstOrDefault();
            //long syskonto = 0;
            //String mandatreferenz = "";
            //long sysmandat = findMandat(ctx, bankdaten, ref syskonto);

            //MANDAT mandat = null;
            //if (sysmandat > 0)
            //{
            //    mandat = (from s in ctx.MANDAT
            //              where s.SYSMANDAT == sysmandat
            //              select s).FirstOrDefault();
            //    if (mandat.STATUS == 1 && mandat.AREA != null)//angebots/antrags/vt-mandat, darf storniert werden
            //    {
            //        if (bankdaten.EINZUG == 0 || bankdaten.SYSKI == 0)//kein einzug oder selbstzahler
            //        {
            //            mandat.STATUS = 5;//Storno
            //            LogHelper.logToDatabase("AIDA-WS createOrUpdateMandat()", "Mandat " + mandat.SYSMANDAT + " set to storno, einzug:" + bankdaten.EINZUG + " SYSKI: " + bankdaten.SYSKI, "ANGEBOT", bankdaten.SYSANGEBOT);
            //        }
            //    }
            //    if (mandat.STATUS != 5)
            //    {
            //        LogHelper.logToDatabase("AIDA-WS createOrUpdateMandat()", "Mandat " + mandat.SYSMANDAT + " found, wont create new Mandat", "ANGEBOT", bankdaten.SYSANGEBOT);
            //        mandatreferenz = mandat.REFERENZ;
            //    }
            //    if (DdOlExtended.getKey(mandat.ITReference.EntityKey) == 0)
            //    {
            //        if (bankdaten.SYSKI > 0)
            //        {
            //            mandat.ITReference.EntityKey = ctx.getEntityKey(typeof(IT), bankdaten.SYSKI);
            //        }
            //        LogHelper.logToDatabase("AIDA-WS createOrUpdateMandat()", "Mandat " + mandat.SYSMANDAT + " updated with missing sysit " + bankdaten.SYSKI, "ANGEBOT", bankdaten.SYSANGEBOT);
            //    }
            //    if (mandat.STATUS == 1)//falls im status neu einige daten aktualisieren lassen
            //    {
            //        mandat.PAYART = bankdaten.PAYART;
            //        //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
            //        if (!mandat.PAYART.HasValue || mandat.PAYART == 0)
            //        {
            //            mandat.PAYART = 1;
            //        }
            //        if (bankdaten.SYSIT != bankdaten.SYSKI)
            //        {
            //            mandat.SYSITDEBITOR = bankdaten.SYSIT;
            //        }

            //        if ((!mandat.SIGNDATE.HasValue || (bankdaten.MANDATSORT != null && bankdaten.MANDATSORT.Length > 0 && !bankdaten.MANDATSORT.Equals(mandat.SIGNCITY))))
            //        {
            //            //if (!String.IsNullOrEmpty(bankdaten.MANDATSORT) && (!mandat.SIGNDATE.HasValue||(!bankdaten.MANDATSORT.Equals(mandat.SIGNCITY) ) ))

            //            String newOrt = bankdaten.MANDATSORT;
            //            if (newOrt.Length > 40)
            //                newOrt = newOrt.Substring(0, 40);
            //            if (!newOrt.Equals(mandat.SIGNCITY))
            //            {
            //                mandat.SIGNCITY = newOrt;
            //                mandat.SIGNDATE = DateTime.Now;
            //                mandat.VALIDFROM = mandat.SIGNDATE;
            //            }
            //        }

            //    }
            //}



            //if (sysmandat == 0 && bankdaten.EINZUG > 0 && bankdaten.SYSKI > 0)//neues globalmandat anlegen
            //{
            //    mandat = new MANDAT();
            //    Cic.OpenLease.Model.DdOiqueue.CfgSingleton CfgSingleton = Cic.OpenLease.Model.DdOiqueue.CfgSingleton.Instance;

            //    //Get Angebot unique identifier
            //    //String ConfigurationValue = CfgSingleton.GetEntry("NKK", "MANDAT_BER", "AIDA", "AIDA");
            //    NkBuilder nk = new NkBuilder("MANDAT", "B2B");
            //    nk.addBind("sysvart", bankdaten.SYSVART);
            //    mandat.REFERENZ = nk.getNextNumber();
            //    mandat.VERSION = 0;
            //    if (syskonto > 0)
            //    {
            //        mandat.KONTOReference.EntityKey = ctx.getEntityKey(typeof(KONTO), syskonto);
            //    }
            //    /*if (sysperson > 0)
            //    {
            //        mandat.PERSONReference.EntityKey = ctx.getEntityKey(typeof(PERSON), sysperson);
            //    }*/
            //    mandat.PAYART = bankdaten.PAYART;
            //    mandat.LSADDReference.EntityKey = ctx.getEntityKey(typeof(LSADD), bankdaten.SYSLS);

            //    mandat.PAYTYPE = 2;
            //    mandat.STATUS = 1;//NEU
            //    if (bankdaten.SYSIT != bankdaten.SYSKI)
            //        mandat.SYSITDEBITOR = bankdaten.SYSIT;
            //    if (bankdaten.SYSKI > 0)
            //        mandat.ITReference.EntityKey = ctx.getEntityKey(typeof(IT), bankdaten.SYSKI);

            //    //default immer CORE (BMWSEPA-2031) falls nichts gesetzt
            //    if (!mandat.PAYART.HasValue || mandat.PAYART == 0)
            //    {
            //        mandat.PAYART = 1;
            //    }

            //    if (bankdaten.MANDATSORT != null && bankdaten.MANDATSORT.Length > 0)
            //    {
            //        String newOrt = bankdaten.MANDATSORT;
            //        if (newOrt.Length > 40)
            //            newOrt = newOrt.Substring(0, 40);
            //        mandat.SIGNCITY = newOrt;
            //        mandat.SIGNDATE = DateTime.Now;
            //        mandat.VALIDFROM = mandat.SIGNDATE;
            //    }

            //    mandatreferenz = mandat.REFERENZ;
            //    ctx.MANDAT.Add(mandat);
            //}
            //createSicherheit(ctx, bankdaten, vpsysperson);
            //ctx.SaveChanges();
            //ctx.ExecuteStoreCommand("update angebot set mandatreferenz='" + mandatreferenz + "' where sysid=" + bankdaten.SYSANGEBOT, null);
            //LogHelper.logToDatabase("AIDA-WS createOrUpdateMandat()", "Update Mandatreferenz to " + mandatreferenz, "ANGEBOT", bankdaten.SYSANGEBOT);
            //if (sysmandat == 0)
            //{
            //    LogHelper.logToDatabase("AIDA-WS createOrUpdateMandat()", "Created new Mandat " + mandat.SYSMANDAT, "ANGEBOT", bankdaten.SYSANGEBOT);
            //}

            //return mandat;
        }

        /// <summary>
        /// creates a bankdaten dto used for many methods 
        /// filled from a given angebot
        /// </summary>
        /// <param name="angebotDto"></param>
        /// <param name="sysls"></param>
        /// <param name="ctx"></param>
        /// <param name="useIt">when true uses in any case the it-data as leading bank data, not an already saved mandate</param>
        /// <returns></returns>
        public static BankdatenDto getBankDatenFromAngebot(ANGEBOTDto angebotDto, long sysls, DdOlExtended ctx, bool useIt, bool noItIbanMatch)
        {

            BankdatenDto bankdaten = new BankdatenDto();
            bankdaten.SYSKI = angebotDto.SYSKI.HasValue ? angebotDto.SYSKI.Value : 0;
            bankdaten.EINZUG = angebotDto.EINZUG.HasValue ? angebotDto.EINZUG.Value : 0;
            bankdaten.SYSIT = angebotDto.SYSIT.HasValue ? angebotDto.SYSIT.Value : 0;
            bankdaten.SYSANGEBOT = angebotDto.SYSID.Value;
            bankdaten.SYSVART = angebotDto.SYSVART.Value;
            bankdaten.SYSLS = sysls;

            if (bankdaten.SYSKI > 0)
            {
                //when angebot was already saved we can find it this way
                BankdatenDto bankdaten2 = DeliverBankdaten(bankdaten.SYSANGEBOT, false, noItIbanMatch);
                if (!useIt && bankdaten2.IBAN != null && bankdaten2.IBAN.Length > 0 && bankdaten2.PAYART > 0)
                {
                    bankdaten.IBAN = bankdaten2.IBAN;
                    bankdaten.BIC = bankdaten2.BIC;
                    bankdaten.BANKNAME = bankdaten2.BANKNAME;
                    bankdaten.PAYART = bankdaten2.PAYART;
                }
                else
                {
                    //if angebot was not yet saved we have to look in current IT
                    BLZInfo itbank = ctx.ExecuteStoreQuery<BLZInfo>("select kontonr,iban,bic,bankname from it where sysit=" + bankdaten.SYSKI, null).FirstOrDefault();
                    bankdaten.IBAN = itbank.IBAN;
                    bankdaten.BIC = itbank.BIC;
                    bankdaten.BANKNAME = itbank.NAME;
                    bankdaten.PAYART = (int)ctx.ExecuteStoreQuery<long>("select payart from it where sysit=" + bankdaten.SYSKI, null).FirstOrDefault();
                }
            }
            //always use input
            bankdaten.SYSKI = angebotDto.SYSKI.HasValue ? angebotDto.SYSKI.Value : 0;

            bankdaten.EINZUG = angebotDto.EINZUG.HasValue ? angebotDto.EINZUG.Value : 0;
            bankdaten.SYSIT = angebotDto.SYSIT.HasValue ? angebotDto.SYSIT.Value : 0;
            bankdaten.SYSANGEBOT = angebotDto.SYSID.Value;
            bankdaten.SYSVART = angebotDto.SYSVART.Value;
            bankdaten.SYSLS = sysls;

            //always use intput mandatsort!
            //bankdaten.MANDATSORT = angebotDto.UNTERSCHRIFTORT;

            return bankdaten;
        }

        /// <summary>
        /// finds the VALID mandat for a given angebot, 
        /// searches by current it iban for a konto, then its mandates, then globalmandate, then angebot-mandate
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="bankdaten"></param>
        /// <param name="syskonto">konto found for iban, if any</param>
        /// <returns></returns>
        public static long findMandat(DdOlExtended ctx, BankdatenDto bankdaten, ref long syskonto)
        {

            long sysmandat = 0;
            long payart = bankdaten.PAYART;
            if (payart == 0)
                payart = 1;
            //Prüfe "gültiges Globalmandat bei einer Bankverbindung zu diesem Kunden...."
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "iban", Value = bankdaten.IBAN });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "payart", Value = payart });
            BLZInfo blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYAVAILGLOBMANDAT, parameters.ToArray()).FirstOrDefault();
            if (blzinfo != null && blzinfo.SYSMANDAT > 0)
                return blzinfo.SYSMANDAT;

            //suche in allen Konten des Kunden nach Übereinstimmung von IBAN, Bezeichnung nicht wie „%inaktiv%“
            parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "iban", Value = bankdaten.IBAN });
            //konto schon vorhanden für it.iban?
            blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYPERSONKONTOIBAN, parameters.ToArray()).FirstOrDefault();

            if (blzinfo != null && blzinfo.SYSKONTO > 0)//Konto vorhanden, mandat für konto suchen
            {
                syskonto = blzinfo.SYSKONTO;

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskonto", Value = blzinfo.SYSKONTO });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "payart", Value = payart });
                blzinfo = ctx.ExecuteStoreQuery<BLZInfo>(QUERYITKONTOMANDAT, parameters.ToArray()).FirstOrDefault();
                if (blzinfo != null && blzinfo.SYSMANDAT > 0)//mandat gefunden
                {
                    sysmandat = blzinfo.SYSMANDAT;
                }
                //falls wiedereinreichungsmandat, dann vom antrag und oder angebotsmandat suchen
                if (sysmandat == 0)//wandermandat am vt suchen der wiedereingereicht wurde
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITWANDERMANDATVT, parameters.ToArray()).FirstOrDefault();
                }
                if (sysmandat == 0)//wandermandat am antrag suchen der wiedereingereicht wurde
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITWANDERMANDATANTRAG, parameters.ToArray()).FirstOrDefault();
                }
                if (sysmandat == 0)//wandermandat hängt noch am alten angebot
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITWANDERMANDATANGEBOT, parameters.ToArray()).FirstOrDefault();
                }
            }
            else
            {//kein konto vorhanden
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "payart", Value = payart });
                sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITGLOBALMANDATKONTO, parameters.ToArray()).FirstOrDefault();

                if (sysmandat == 0)//globalmandat ohne konto suchen
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "payart", Value = payart });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITGLOBALMANDAT, parameters.ToArray()).FirstOrDefault();
                }
                if (sysmandat == 0)//wandermandat am antrag suchen
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITWANDERMANDATANTRAG, parameters.ToArray()).FirstOrDefault();
                }
                if (sysmandat == 0)//wandermandat hängt noch am alten angebot
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = bankdaten.SYSKI });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = bankdaten.SYSLS });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });
                    sysmandat = ctx.ExecuteStoreQuery<long>(QUERYITWANDERMANDATANGEBOT, parameters.ToArray()).FirstOrDefault();
                }
            }
            return sysmandat;
        }

        /// <summary>
        /// Creates or Disables the ANGOBSICH for payments
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="bankdaten"></param>
        /// <param name="vpsysperson"></param>
        private ANGOBSICH createSicherheit(DdOlExtended ctx, Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto bankdaten, long vpsysperson)
        {
            SICHTYP sich = SICHTYPHelper.GetSichTyp(ctx, KONTOINHABERSICHTYP);
            ANGOBSICH sicherheit = ANGOBSICHHelper.GetAngobsischByRang(ctx, bankdaten.SYSANGEBOT, KONTOINHABERSICHRANG);
            bool isThirdPerson = false;
            if (bankdaten.SYSIT != bankdaten.SYSKI && bankdaten.EINZUG == 1)//Schuldner ist nicht Kunde, also evtl 3. Person, dann ist eine Sicherheit nötig
            {
                String query = "select angobsich.sysit from angobsich, sichtyp,angebot where angobsich.bezeichnung='Mitantragsteller' and angobsich.sysvt=angebot.sysid and angobsich.syssichtyp=sichtyp.syssichtyp and sichtyp.rang=10 and angebot.sysid=:sysid";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = bankdaten.SYSANGEBOT });

                List<long> mas = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).ToList();

                if (!mas.Contains(bankdaten.SYSKI))//sicherheitengeber ist 3. Person da nicht mitantragsteller
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
                        sicherheit.SYSPERSON = vpsysperson;
                        sicherheit.SYSSICHTYP =sich.SYSSICHTYP;
                        sicherheit.SYSVT = bankdaten.SYSANGEBOT;
                        sicherheit.STATUS = "erhalten";
                        sicherheit.AKTIVFLAG = 1;
                        sicherheit.SYSIT= bankdaten.SYSKI;
                        LogHelper.logToDatabase("OFISWebService createSicherheit()", "Kontoinhaber=" + bankdaten.SYSKI + " for SICHERHEIT " + sicherheit.SYSID, "ANGEBOT", bankdaten.SYSANGEBOT);
                    }
                }
            }
            if (sicherheit != null && !isThirdPerson)
            {
                sicherheit.AKTIVFLAG = 0;//falls schon eine Sicherheit da ist, diese deaktivieren, da nicht mehr 3. Person
                LogHelper.logToDatabase("OFISWebService createSicherheit()", "AKTIVFLAG=0 for SICHERHEIT " + sicherheit.SYSID, "ANGEBOT", bankdaten.SYSANGEBOT);
            }
            return sicherheit;
        }

    }
}