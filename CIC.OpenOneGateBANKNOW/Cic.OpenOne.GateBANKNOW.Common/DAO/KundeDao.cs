using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OL.EF6.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// KundeDao erstellt und holt neue und vorhandene KundeDtos
    /// </summary>
    public class KundeDao : IKundeDao
    {
        const String RISIKOKLQUERY_B2B = "select risikokl.* from risikokl left outer join person on person.sysrisikokl=risikokl.sysrisikokl where person.sysperson=:sysperson";

        const String RISIKOKLQUERY = "select risikokl.* from deoutexec, dedetail, risikokl where deoutexec.SYSDEOUTEXEC = dedetail.SYSDEOUTEXEC  " +
                                        " and dedetail.antragsteller = 1 and risikokl.sysrisikokl = dedetail.risikoklasseid " +
                                        " and deoutexec.SYSAUSKUNFT = (select max(sysauskunft) from auskunft " +
                                                                        " where sysauskunfttyp = 3 and statusnum = 0 and sysid = :sysantrag) ";

        const String PKZQUERY = "select * from PKZ where sysantrag=:sysantrag and sysperson=:sysperson";
        const String ITPKZQUERY = "select * from ITPKZ where sysantrag=:sysantrag and sysit=:sysit";

        const String ANZVQUERY = "select count(*) from vt where syskd=:sysperson and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0)";
        const String ANZVITQUERY = "select count(*) from vt where sysit=:sysit and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0)";

        const String ANZVEXPRESSQUERY = "select count(*) from vt where syskd=:sysperson and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0) and sysvart in (select sysvart from vart where code='KREDIT_EXPRESS')";
        const String ANZVITEXPRESSQUERY = "select count(*) from vt where sysit=:sysit and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0) and sysvart in (select sysvart from vart where code='KREDIT_EXPRESS')";

        const String VDISPOQUERY = "select sysid from vt where syskd=:sysperson and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0) and sysvart in (select sysvart from vart where code like 'KREDIT_DISPO%')";
        const String VITDISPOQUERY = "select sysid from vt where sysit=:sysit and (aktivkz = 1 or VT.AKTIVKZ+VT.ENDEKZ+VT.FLAGSALDIERT=0) and sysvart in (select sysvart from vart where code like 'KREDIT_DISPO%')";

        // da muss der neueste Datensatz aus Kremo geholt werden
        const String BUDGETQUERY = "select * from Kremo where sysantrag=:sysantrag order by syskremo desc ";

        const String QUERYMERGEPKZ = @"merge into pkz dest
                                  using (select * from itpkz where rownum=1 and sysit=:sysit and sysantrag=:sysantrag order by sysitpkz desc) src
                                  on ( dest.sysantrag=:sysantrag and dest.sysperson=:syskd)
                                  when matched then update set
dest.ABBEWILLIGUNGBIS=src.ABBEWILLIGUNGBIS,
dest.AHBEWILLIGUNGBIS=src.AHBEWILLIGUNGBIS,
dest.ANREDE=src.ANREDE,
dest.ANREDECODE=src.ANREDECODE,
dest.ANREDECODEKORR=src.ANREDECODEKORR,
dest.ANREDEKORR=src.ANREDEKORR,
dest.ANZAHLVOLLSTR=src.ANZAHLVOLLSTR,
dest.ANZEINK=src.ANZEINK,
dest.ANZKINDER=src.ANZKINDER,
dest.ANZKINDER1=src.ANZKINDER1,
dest.ANZKINDER2=src.ANZKINDER2,
dest.ANZKINDER3=src.ANZKINDER3,
dest.ANZNEINK=src.ANZNEINK,
dest.ANZZEINK=src.ANZZEINK,
dest.AUSLAGEN=src.AUSLAGEN,
dest.AUSLAGENCODE=src.AUSLAGENCODE,
dest.AUSLAUSWEIS=src.AUSLAUSWEIS,
dest.AUSLAUSWEISCODE=src.AUSLAUSWEISCODE,
dest.AUSLAUSWEISGUELTIG=src.AUSLAUSWEISGUELTIG,
dest.AUSWEISABLAUF=src.AUSWEISABLAUF,
dest.AUSWEISART=src.AUSWEISART,
dest.AUSWEISBEHOERDE=src.AUSWEISBEHOERDE,
dest.AUSWEISDATUM=src.AUSWEISDATUM,
dest.AUSWEISNR=src.AUSWEISNR,
dest.BANKNAME=src.BANKNAME,
dest.BERUF=src.BERUF,
dest.BERUFCODE=src.BERUFCODE,
dest.BERUFLICHCODE=src.BERUFLICHCODE,
dest.BERUFLICHCODE2=src.BERUFLICHCODE2,
dest.BERUFSAUSLAGEN=src.BERUFSAUSLAGEN,
dest.BERUFSAUSLAGENCODE=src.BERUFSAUSLAGENCODE,
dest.BESCHBISAG=src.BESCHBISAG,
dest.BESCHBISAG1=src.BESCHBISAG1,
dest.BESCHBISAG2=src.BESCHBISAG2,
dest.BESCHSEITAG=src.BESCHSEITAG,
dest.BESCHSEITAG1=src.BESCHSEITAG1,
dest.BESCHSEITAG2=src.BESCHSEITAG2,
dest.BETRAGVOLLSTR=src.BETRAGVOLLSTR,
dest.BIC=src.BIC,
dest.BLZ=src.BLZ,
dest.CREBANUMMER=src.CREBANUMMER,
dest.CREBASYSAUSKUNFT=src.CREBASYSAUSKUNFT,
dest.CREEWKNUMMER=src.CREEWKNUMMER,
dest.CREEWKSYSAUSKUNFT=src.CREEWKSYSAUSKUNFT,
dest.EHEPARTNERFLAG=src.EHEPARTNERFLAG,
dest.EINGEFUEGTAM=src.EINGEFUEGTAM,
dest.EINHEIT=src.EINHEIT,
dest.EINKBRUTTO=src.EINKBRUTTO,
dest.EINKNETTO=src.EINKNETTO,
dest.EINKNETTOFLAG=src.EINKNETTOFLAG,
dest.EINREISEDATUM=src.EINREISEDATUM,
dest.EMAIL=src.EMAIL,
dest.EMAIL2=src.EMAIL2,
dest.FAMILIENSTAND=src.FAMILIENSTAND,
dest.FAX=src.FAX,
dest.GEBDATUM=src.GEBDATUM,
dest.GRUENDUNG=src.GRUENDUNG,
dest.GUETERSTAND=src.GUETERSTAND,
dest.HANDY=src.HANDY,
dest.HSNR=src.HSNR,
dest.HSNRAG=src.HSNRAG,
dest.HSNRAG1=src.HSNRAG1,
dest.HSNRAG2=src.HSNRAG2,
dest.HSNRKORR=src.HSNRKORR,
dest.HSNR2=src.HSNR2,
dest.IBAN=src.IBAN,
dest.IDENTUST=src.IDENTUST,
dest.IDENTEG=src.IDENTEG,
dest.INFOMAILFLAG=src.INFOMAILFLAG,
dest.INFOMAIL2FLAG=src.INFOMAIL2FLAG,
dest.INFOSMSFLAG=src.INFOSMSFLAG,
dest.INFOTELFLAG=src.INFOTELFLAG,
dest.INSTRADIERUNG=src.INSTRADIERUNG,
dest.JBONUSBRUTTO=src.JBONUSBRUTTO,
dest.JBONUSNETTO=src.JBONUSNETTO,
dest.KDIDENTFLAG=src.KDIDENTFLAG,
dest.KINDERIMHAUS=src.KINDERIMHAUS,
dest.KONFESSION=src.KONFESSION,
dest.KONKURSFLAG=src.KONKURSFLAG,
dest.KONTONR=src.KONTONR,
dest.KREDRATE=src.KREDRATE,
dest.KREDRATE1=src.KREDRATE1,
dest.KREDRATE2=src.KREDRATE2,
dest.KREDRATE3=src.KREDRATE3,
dest.KREDRATE4=src.KREDRATE4,
dest.LEASINGRATE=src.LEASINGRATE,
dest.LEASINGRATE1=src.LEASINGRATE1,
dest.LEASINGRATE2=src.LEASINGRATE2,
dest.LEASINGRATE3=src.LEASINGRATE3,
dest.LEASINGRATE4=src.LEASINGRATE4,
dest.LEGITABNEHMER=src.LEGITABNEHMER,
dest.LEGITDATUM=src.LEGITDATUM,
dest.LEGITMETHODCODE=src.LEGITMETHODCODE,
dest.MIETE=src.MIETE,
dest.MIETNEBEN=src.MIETNEBEN,
dest.MITARBEITERFLAG=src.MITARBEITERFLAG,
dest.MONATSLOHNXTDFLAG=src.MONATSLOHNXTDFLAG,
dest.NAME=src.NAME,
dest.NAMEAG=src.NAMEAG,
dest.NAMEAG1=src.NAMEAG1,
dest.NAMEAG2=src.NAMEAG2,
dest.NAMEKORR=src.NAMEKORR,
dest.NEBEINKBRUTTO=src.NEBEINKBRUTTO,
dest.NEBEINKNETTO=src.NEBEINKNETTO,
dest.NEBENINMIETE=src.NEBENINMIETE,
dest.ORT=src.ORT,
dest.ORTAG=src.ORTAG,
dest.ORTAG1=src.ORTAG1,
dest.ORTAG2=src.ORTAG2,
dest.ORTKORR=src.ORTKORR,
dest.ORT2=src.ORT2,
dest.PD=src.PD,
dest.PFAENDUNGSFLAG=src.PFAENDUNGSFLAG,
dest.PID=src.PID,
dest.PLZ=src.PLZ,
dest.PLZAG=src.PLZAG,
dest.PLZAG1=src.PLZAG1,
dest.PLZAG2=src.PLZAG2,
dest.PLZKORR=src.PLZKORR,
dest.PLZ2=src.PLZ2,
dest.PREVNAME=src.PREVNAME,
dest.PTELEFON=src.PTELEFON,
dest.QUELLENSTEUERFLAG=src.QUELLENSTEUERFLAG,
dest.SCHUFAFLAG=src.SCHUFAFLAG,
dest.STRASSE=src.STRASSE,
dest.STRASSEAG=src.STRASSEAG,
dest.STRASSEAG1=src.STRASSEAG1,
dest.STRASSEAG2=src.STRASSEAG2,
dest.STRASSEKORR=src.STRASSEKORR,
dest.STRASSE2=src.STRASSE2,
dest.SYSANGEBOT=src.SYSANGEBOT,
dest.SYSBRANCHE=src.SYSBRANCHE,
dest.SYSCTLANG=src.SYSCTLANG,
dest.SYSCTLANGKORR=src.SYSCTLANGKORR,
dest.SYSKDTYP=src.SYSKDTYP,
dest.SYSLAND=src.SYSLAND,
dest.SYSLANDAG=src.SYSLANDAG,
dest.SYSLANDAG1=src.SYSLANDAG1,
dest.SYSLANDKORR=src.SYSLANDKORR,
dest.SYSLANDNAT=src.SYSLANDNAT,
dest.SYSLAND2=src.SYSLAND2,
dest.SYSSTAAT=src.SYSSTAAT,
dest.SYSSTAATKORR=src.SYSSTAATKORR,
dest.SYSSTAAT2=src.SYSSTAAT2,
dest.SYSVT=src.SYSVT,
dest.TELEFON=src.TELEFON,
dest.TELEFON2=src.TELEFON2,
dest.TITEL=src.TITEL,
dest.TITELCODE=src.TITELCODE,
dest.TITEL2=src.TITEL2,
dest.TITEL2CODE=src.TITEL2CODE,
dest.UIDNUMMER=src.UIDNUMMER,
dest.UNTERHALT=src.UNTERHALT,
dest.UNTERHALTCODE=src.UNTERHALTCODE,
dest.VERSICHERUNG=src.VERSICHERUNG,
dest.VORNAME=src.VORNAME,
dest.VORNAMEKORR=src.VORNAMEKORR,
dest.WEITEREAUSLAGEN=src.WEITEREAUSLAGEN,
dest.WEITEREAUSLAGENCODE=src.WEITEREAUSLAGENCODE,
dest.WERBECODE=src.WERBECODE,
dest.WOHNART=src.WOHNART,
dest.WOHNSEIT=src.WOHNSEIT,
dest.WOHNVERHCODE=src.WOHNVERHCODE,
dest.ZEINKART=src.ZEINKART,
dest.ZEINKBRUTTO=src.ZEINKBRUTTO,
dest.ZEINKCODE=src.ZEINKCODE,
dest.ZEINKNETTO=src.ZEINKNETTO,
dest.ZULAGEAUSBILDUNG=src.ZULAGEAUSBILDUNG,
dest.ZULAGEKIND=src.ZULAGEKIND,
dest.ZULAGESONST=src.ZULAGESONST";

        const String QUERYMERGEUKZ = @"merge into ukz dest
                                  using (select * from itukz where rownum=1 and sysit=:sysit and sysantrag=:sysantrag  order by sysitukz desc) src
                                  on ( dest.sysantrag=:sysantrag and dest.sysperson=:syskd)
                                  when matched then update set
dest.ABSATZRISIKO=src.ABSATZRISIKO,
dest.AM1MON1BIS12=src.AM1MON1BIS12,
dest.AM1MON13BIS24=src.AM1MON13BIS24,
dest.AM1MON25BIS36=src.AM1MON25BIS36,
dest.AM2MON1BIS12=src.AM2MON1BIS12,
dest.AM2MON13BIS24=src.AM2MON13BIS24,
dest.AM2MON25BIS36=src.AM2MON25BIS36,
dest.AM3MON1BIS12=src.AM3MON1BIS12,
dest.AM3MON13BIS24=src.AM3MON13BIS24,
dest.AM3MON25BIS36=src.AM3MON25BIS36,
dest.ANLAGENDECK=src.ANLAGENDECK,
dest.ANREDE=src.ANREDE,
dest.ANREDECODE=src.ANREDECODE,
dest.ANREDECODEKONT=src.ANREDECODEKONT,
dest.ANZMA=src.ANZMA,
dest.ANZVOLLSTR=src.ANZVOLLSTR,
dest.AUSNUTZUNG=src.AUSNUTZUNG,
dest.AZSMON1BIS12=src.AZSMON1BIS12,
dest.AZSMON13BIS24=src.AZSMON13BIS24,
dest.AZSMON25BIS36=src.AZSMON25BIS36,
dest.BANKNAME=src.BANKNAME,
dest.BETRAGVOLLSTR=src.BETRAGVOLLSTR,
dest.BIC=src.BIC,
dest.BILANZ=src.BILANZ,
dest.BILANZPER=src.BILANZPER,
dest.BILANZWERT=src.BILANZWERT,
dest.BLZ=src.BLZ,
dest.BRANCHENENTW=src.BRANCHENENTW,
dest.BWABIS1=src.BWABIS1,
dest.BWABIS2=src.BWABIS2,
dest.BWAVON1=src.BWAVON1,
dest.BWAVON2=src.BWAVON2,
dest.CASHFLOW=src.CASHFLOW,
dest.CREBANUMMER=src.CREBANUMMER,
dest.CREBASYSAUSKUNFT=src.CREBASYSAUSKUNFT,
dest.CREEWKNUMMER=src.CREEWKNUMMER,
dest.CREEWKSYSAUSKUNFT=src.CREEWKSYSAUSKUNFT,
dest.DAVMON1BIS12=src.DAVMON1BIS12,
dest.DAVMON13BIS24=src.DAVMON13BIS24,
dest.DAVMON25BIS36=src.DAVMON25BIS36,
dest.DVTMON1BIS12=src.DVTMON1BIS12,
dest.DVTMON13BIS24=src.DVTMON13BIS24,
dest.DVTMON25BIS36=src.DVTMON25BIS36,
dest.EKAPITAL=src.EKAPITAL,
dest.EKQUOTE=src.EKQUOTE,
dest.EMAIL=src.EMAIL,
dest.EMAIL2=src.EMAIL2,
dest.ERGEBNIS1=src.ERGEBNIS1,
dest.ERGEBNIS2=src.ERGEBNIS2,
dest.ERSTELLTAM=src.ERSTELLTAM,
dest.FAX=src.FAX,
dest.FUNGIBILITAET=src.FUNGIBILITAET,
dest.GESAMTSCORE=src.GESAMTSCORE,
dest.GKRENT=src.GKRENT,
dest.GRUENDUNG=src.GRUENDUNG,
dest.HANDY=src.HANDY,
dest.HREGISTER=src.HREGISTER,
dest.HREGISTERORT=src.HREGISTERORT,
dest.HREGISTERFLAG=src.HREGISTERFLAG,
dest.HSNR=src.HSNR,
dest.HSNRKORR=src.HSNRKORR,
dest.IBAN=src.IBAN,
dest.IDENTEG=src.IDENTEG,
dest.IDENTUST=src.IDENTUST,
dest.INFOMAILFLAG=src.INFOMAILFLAG,
dest.INFOMAIL2FLAG=src.INFOMAIL2FLAG,
dest.INFOSMSFLAG=src.INFOSMSFLAG,
dest.INFOTELFLAG=src.INFOTELFLAG,
dest.JUMSATZ=src.JUMSATZ,
dest.KDIDENTFLAG=src.KDIDENTFLAG,
dest.KONKURSFLAG=src.KONKURSFLAG,
dest.KONTONR=src.KONTONR,
dest.KREDITBIS=src.KREDITBIS,
dest.KREDITRAHMEN=src.KREDITRAHMEN,
dest.LEGITABNEHMER=src.LEGITABNEHMER,
dest.LEGITDATUM=src.LEGITDATUM,
dest.LEGITMETHODCODE=src.LEGITMETHODCODE,
dest.LIQUIDITAET=src.LIQUIDITAET,
dest.LIQUIDITAETBETRAG=src.LIQUIDITAETBETRAG,
dest.LJABSCHL=src.LJABSCHL,
dest.MANKONTINUITAET=src.MANKONTINUITAET,
dest.MANQUALITAET=src.MANQUALITAET,
dest.MARKTSTELLUNG=src.MARKTSTELLUNG,
dest.NAME=src.NAME,
dest.NAMEKONT=src.NAMEKONT,
dest.OBLIGOEIGEN=src.OBLIGOEIGEN,
dest.OBLIGOUEBER=src.OBLIGOUEBER,
dest.OBLIGOVERB=src.OBLIGOVERB,
dest.ORT=src.ORT,
dest.ORTKORR=src.ORTKORR,
dest.PD=src.PD,
dest.PFAENDFLAG=src.PFAENDFLAG,
dest.PLZ=src.PLZ,
dest.PLZKORR=src.PLZKORR,
dest.PRODUKTDIENST=src.PRODUKTDIENST,
dest.PROGUMSATZ=src.PROGUMSATZ,
dest.PTELEFON=src.PTELEFON,
dest.RANG=src.RANG,
dest.RECHPLANWESEN=src.RECHPLANWESEN,
dest.RECHTSFORM=src.RECHTSFORM,
dest.RECHTSFORMCODE=src.RECHTSFORMCODE,
dest.REVFLAG=src.REVFLAG,
dest.SCOREBILANZ=src.SCOREBILANZ,
dest.SCOREBWA=src.SCOREBWA,
dest.SCOREKREDIT=src.SCOREKREDIT,
dest.SCOREOBJEKT=src.SCOREOBJEKT,
dest.SCORERATING=src.SCORERATING,
dest.SCOREZAHLUNG=src.SCOREZAHLUNG,
dest.SPIELRAUM=src.SPIELRAUM,
dest.STRASSE=src.STRASSE,
dest.STRASSEKORR=src.STRASSEKORR,
dest.SYSANGEBOT=src.SYSANGEBOT,
dest.SYSBRANCHE=src.SYSBRANCHE,
dest.SYSCTLANG=src.SYSCTLANG,
dest.SYSCTLANGKORR=src.SYSCTLANGKORR,
dest.SYSKDTYP=src.SYSKDTYP,
dest.SYSLAND=src.SYSLAND,
dest.SYSLANDKORR=src.SYSLANDKORR,
dest.SYSLANDNAT=src.SYSLANDNAT,
dest.SYSSTAATKORR=src.SYSSTAAKORR,
dest.SYSSTAAT=src.SYSSTAAT,
dest.SYSVT=src.SYSVT,
dest.TELEFON=src.TELEFON,
dest.TELEFON2=src.TELEFON2,
dest.TITEL=src.TITEL,
dest.TITELCODE=src.TITELCODE,
dest.TITEL2=src.TITEL2,
dest.TITEL2CODE=src.TITEL2CODE,
dest.UIDNUMMER=src.UIDNUMMER,
dest.UMSATZRENT=src.UMSATZRENT,
dest.UMSATZ1=src.UMSATZ1,
dest.UMSATZ2=src.UMSATZ2,
dest.UNTALTER=src.UNTALTER,
dest.UNTSICHERUNG=src.UNTSICHERUNG,
dest.VORNAMEKONT=src.VORNAMEKONT,
dest.WERBECODE=src.WERBECODE,
dest.WERTABPRO01=src.WERTABPRO01,
dest.WERTABPRO02=src.WERTABPRO02,
dest.WERTABPRO03=src.WERTABPRO03,
dest.WERTABPRO04=src.WERTABPRO04,
dest.WERTABPRO05=src.WERTABPRO05,
dest.WERTABPRO06=src.WERTABPRO06,
dest.WERTABPRO07=src.WERTABPRO07,
dest.WERTABPRO08=src.WERTABPRO08,
dest.WERTABPRO09=src.WERTABPRO09,
dest.WERTABPRO10=src.WERTABPRO10,
dest.WERTAB01=src.WERTAB01,
dest.WERTAB02=src.WERTAB02,
dest.WERTAB03=src.WERTAB03,
dest.WERTAB04=src.WERTAB04,
dest.WERTAB05=src.WERTAB05,
dest.WERTAB06=src.WERTAB06,
dest.WERTAB07=src.WERTAB07,
dest.WERTAB08=src.WERTAB08,
dest.WERTAB09=src.WERTAB09,
dest.WERTAB10=src.WERTAB10,
dest.ZUSATZ=src.ZUSATZ
";

        const String QUERYMERGEPERSONPKZ = @"merge into person dest
                                  using (select * from itpkz where rownum=1 and sysit=:sysit and sysantrag=:sysantrag order by sysitpkz desc) src
                                  on ( dest.sysperson=:syskd)
                                  when matched then update set
dest.ANREDE=src.ANREDE,
dest.ANREDECODE=src.ANREDECODE,
dest.AUSLAUSWEIS=src.AUSLAUSWEIS,
dest.AUSLAUSWEISCODE=src.AUSLAUSWEISCODE,
dest.AUSLAUSWEISGUELTIG=src.AUSLAUSWEISGUELTIG,
dest.AUSWEISABLAUF=src.AUSWEISABLAUF,
dest.AUSWEISART=src.AUSWEISART,
dest.AUSWEISBEHOERDE=src.AUSWEISBEHOERDE,
dest.AUSWEISDATUM=src.AUSWEISDATUM,
dest.AUSWEISNR=src.AUSWEISNR,
dest.EINREISEDATUM=src.EINREISEDATUM,
dest.EMAIL=src.EMAIL,
dest.EMAIL2=src.EMAIL2,
dest.FAX=src.FAX,
dest.GEBDATUM=src.GEBDATUM,
dest.GRUENDUNG=src.GRUENDUNG,
dest.HANDY=src.HANDY,
dest.HSNR=src.HSNR,
dest.HSNR2=src.HSNR2,
dest.IDENTEG=src.IDENTEG,
dest.IDENTUST=src.IDENTUST,
dest.INFOMAILFLAG=src.INFOMAILFLAG,
dest.INFOMAIL2FLAG=src.INFOMAIL2FLAG,
dest.INFOSMSFLAG=src.INFOSMSFLAG,
dest.INFOTELFLAG=src.INFOTELFLAG,
dest.LEGITABNEHMER=src.LEGITABNEHMER,
dest.LEGITDATUM=src.LEGITDATUM,
dest.MITARBEITERFLAG=src.MITARBEITERFLAG,
dest.NAME=src.NAME,
dest.ORT=src.ORT,
dest.ORT2=src.ORT2,
dest.PD=src.PD,
dest.PLZ=src.PLZ,
dest.PLZ2=src.PLZ2,
dest.PREVNAME=src.PREVNAME,
dest.PTELEFON=src.PTELEFON,
dest.SCHUFAFLAG=src.SCHUFAFLAG,
dest.STRASSE=src.STRASSE,
dest.STRASSE2=src.STRASSE2,
dest.SYSBRANCHE=src.SYSBRANCHE,
dest.SYSCTLANG=src.SYSCTLANG,
dest.SYSCTLANGKORR=src.SYSCTLANGKORR,
dest.SYSKDTYP=src.SYSKDTYP,
dest.SYSLAND=src.SYSLAND,
dest.SYSLANDNAT=src.SYSLANDNAT,
dest.SYSLAND2=src.SYSLAND2,
dest.SYSSTAAT=src.SYSSTAAT,
dest.SYSSTAAT2=src.SYSSTAAT2,
dest.TELEFON=src.TELEFON,
dest.TELEFON2=src.TELEFON2,
dest.TITEL=src.TITEL,
dest.TITELCODE=src.TITELCODE,
dest.TITEL2=src.TITEL2,
dest.TITEL2CODE=src.TITEL2CODE,
dest.UIDNUMMER=src.UIDNUMMER,
dest.VORNAME=src.VORNAME,
dest.WERBECODE=src.WERBECODE";

        const String QUERYMERGEPERSONUKZ = @"merge into person dest
                                  using (select * from itukz where rownum=1 and sysit=:sysit and sysantrag=:sysantrag  order by sysitukz desc) src
                                  on ( dest.sysperson=:syskd)
                                  when matched then update set
dest.ANREDE=src.ANREDE,
dest.ANREDECODE=src.ANREDECODE,
dest.ANREDECODEKONT=src.ANREDECODEKONT,
dest.EMAIL=src.EMAIL,
dest.EMAIL2=src.EMAIL2,
dest.FAX=src.FAX,
dest.GRUENDUNG=src.GRUENDUNG,
dest.HANDY=src.HANDY,
dest.HREGISTER=src.HREGISTER,
dest.HREGISTERORT=src.HREGISTERORT,
dest.HREGISTERFLAG=src.HREGISTERFLAG,
dest.HSNR=src.HSNR,
dest.IDENTEG=src.IDENTEG,
dest.IDENTUST=src.IDENTUST,
dest.INFOMAILFLAG=src.INFOMAILFLAG,
dest.INFOMAIL2FLAG=src.INFOMAIL2FLAG,
dest.INFOSMSFLAG=src.INFOSMSFLAG,
dest.INFOTELFLAG=src.INFOTELFLAG,
dest.LEGITABNEHMER=src.LEGITABNEHMER,
dest.LEGITDATUM=src.LEGITDATUM,
dest.NAME=src.NAME,
dest.NAMEKONT=src.NAMEKONT,
dest.ORT=src.ORT,
dest.PD=src.PD,
dest.PLZ=src.PLZ,
dest.PTELEFON=src.PTELEFON,
dest.RECHTSFORM=src.RECHTSFORM,
dest.RECHTSFORMCODE=src.RECHTSFORMCODE,
dest.REVFLAG=src.REVFLAG,
dest.STRASSE=src.STRASSE,
dest.SYSBRANCHE=src.SYSBRANCHE,
dest.SYSCTLANG=src.SYSCTLANG,
dest.SYSCTLANGKORR=src.SYSCTLANGKORR,
dest.SYSKDTYP=src.SYSKDTYP,
dest.SYSLAND=src.SYSLAND,
dest.SYSLANDNAT=src.SYSLANDNAT,
dest.SYSSTAAT=src.SYSSTAAT,
dest.TELEFON=src.TELEFON,
dest.TELEFON2=src.TELEFON2,
dest.TITEL=src.TITEL,
dest.TITELCODE=src.TITELCODE,
dest.TITEL2=src.TITEL2,
dest.TITEL2CODE=src.TITEL2CODE,
dest.UIDNUMMER=src.UIDNUMMER,
dest.VORNAMEKONT=src.VORNAMEKONT,
dest.WERBECODE=src.WERBECODE,
dest.ZUSATZ=src.ZUSATZ";

        const String QUERYMERGEITPERSON = @"merge into person dest
                                  using (select * from it where rownum=1 and sysit=:sysit order by sysit desc) src
                                  on ( dest.sysperson=:syskd)
                                  when matched then update set
dest.CODE=:syskd,
dest.ERREICHBBIS=src.ERREICHBBIS,
dest.ERREICHBTEL=src.ERREICHBTEL,
dest.ERREICHBVON=src.ERREICHBVON,
dest.EXTREFERENZ=src.EXTREFERENZ,
dest.GEBORT=src.GEBORT,
dest.HREGISTERORT=src.HREGISTERORT,
dest.IDENTEGHIST=src.IDENTEGHIST,
dest.MATCHCODE=src.MATCHCODE,
dest.NOTIZ=src.NOTIZ,
dest.PRIVATFLAG=src.PRIVATFLAG,
dest.STRASSEZUSATZ=src.STRASSEZUSATZ,
dest.SUFFIX=src.SUFFIX,
dest.URL=src.URL,
dest.WERBECODEGRUND=src.WERBECODEGRUND,
dest.WERBUNGBIS=src.WERBUNGBIS,
dest.WERBUNGVON=src.WERBUNGVON";

        private const String QUERYSYSKDTYP = @"select * from 
(select syskdtyp from itpkz where sysit = :sysit and sysantrag = :sysantrag )
union all
(select syskdtyp from itukz where sysit = :sysit and sysantrag = :sysantrag )";


        private const String QUERYMITANTRAGSTELLER = @"select antobsich.sysit
                        from antobsich, antrag, sichtyp
                        where sichtyp.syssichtyp = antobsich.syssichtyp and sichtyp.rang in ('130', '800', '120')
                        and antrag.sysid = antobsich.sysantrag
                        and antrag.sysit=:sysit
                        order by antrag.sysid desc";//,antrag.syskd KUNDE,antrag.sysit KUNDEIT

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<long, String> kdtypName = CacheFactory<long, String>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Translation);
        private static CacheDictionary<long, int> kdtypTyp = CacheFactory<long, int>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Translation);

        /// <summary>
        /// PkzUkz Ausnahme
        /// </summary>
        public class PkzUkzException : Exception
        {
            /// <summary>
            /// Standard Konstruktor
            /// </summary>
            public PkzUkzException() : base() { }

            /// <summary>
            /// Konstruktor String
            /// </summary>
            /// <param name="Reason"></param>
            public PkzUkzException(string Reason) : base(Reason) { }

            /// <summary>
            /// Konstruktor String, innerException
            /// </summary>
            /// <param name="Reason"></param>
            /// <param name="Exp"></param>
            public PkzUkzException(string Reason, Exception Exp) : base(Reason, Exp) { }
        }

        /// <summary>
        /// createKunde erstellt ein neuen IT Datensatz mit einer neuen SYSIT
        /// </summary>
        /// <param name="kunde">KundeDto mit einer SYSIT = 0</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto mit einer SYSIT aus der Datenbank</returns>
        public KundeDto createKunde(KundeDto kunde, long sysperole)
        {
            long measure = DateTime.Now.TimeOfDay.Milliseconds;
            
            IT kundeOutput = new IT();
            List<ZusatzdatenDto> zusatzdaten = new List<ZusatzdatenDto>();

            using (DdOlExtended context = new DdOlExtended())
            {
                _log.Debug("Duration createKunde A " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                context.IT.Add(kundeOutput);
                _log.Debug("Duration createKunde B " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                
                kundeOutput.ANREDE = kunde.anrede;
                kundeOutput.ANREDECODE = kunde.anredeCode;
                kundeOutput.AUSLAUSWEIS = kunde.auslausweis;
                kundeOutput.AUSLAUSWEISCODE = kunde.auslausweisCode;
                kundeOutput.BESCHARTAG = kunde.beschartag;
                if ((kundeOutput.AUSLAUSWEIS == null || kundeOutput.AUSLAUSWEIS.Length < 1) && kunde.auslausweisCode != null && kunde.auslausweisCode.Length > 0)
                {
                    object[] pars = { 
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = kunde.auslausweisCode } 
                    };
                    kundeOutput.AUSLAUSWEIS = context.ExecuteStoreQuery<String>("select trim(value) from ddlkppos where code='AUSLAUSWEIS' and id=:id", pars).FirstOrDefault();
                }
                kundeOutput.AUSLAUSWEISGUELTIG = kunde.auslausweisGueltig;
                kundeOutput.EINREISEDATUM = kunde.einreisedatum;
                kundeOutput.EMAIL = kunde.email;
                
                DateTime now = DateTime.Now;
                if (kunde.erreichbVon > 2359)
                    kunde.erreichbVon = 0;
                if (kunde.erreichbBis > 2359)
                    kunde.erreichbBis = 0;

                DateTime ttemp = new DateTime(now.Year,now.Month,now.Day,(int)kunde.erreichbBis/100,(int)(kunde.erreichbBis-kunde.erreichbBis/100*100),0);
                kundeOutput.ERREICHBBIS = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);
                
                ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbVon / 100, (int)(kunde.erreichbVon - kunde.erreichbVon / 100 * 100), 0);
                kundeOutput.ERREICHBVON = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp); 

                kundeOutput.ERREICHBTEL = kunde.erreichbtel;
                kundeOutput.GEBDATUM = kunde.gebdatum;
                kundeOutput.GRUENDUNG = kunde.gruendung;
                kundeOutput.HANDY = kunde.handy;
                kundeOutput.HREGISTER = kunde.hregister;
                kundeOutput.HREGISTERFLAG = Convert.ToInt32(kunde.hregisterFlag);
                kundeOutput.HSNR = kunde.hsnr;
                kundeOutput.HSNR2 = kunde.hsnr2;
                kundeOutput.NAME = kunde.name;
                kundeOutput.ORT = kunde.ort;
                kundeOutput.ORT2 = kunde.ort2;
                kundeOutput.PLZ = kunde.plz;
                kundeOutput.PLZ2 = kunde.plz2;
                try
                {
                    kundeOutput.RECHTSFORM = Convert.ToInt32(kunde.rechtsform);
                }
                catch
                {
                    //Do Nothing;
                };
                kundeOutput.RECHTSFORMCODE = kunde.rechtsformCode;
                kundeOutput.REVFLAG = Convert.ToInt32(kunde.revFlag);
                kundeOutput.STRASSE = kunde.strasse;
                kundeOutput.STRASSE2 = kunde.strasse2;
                kundeOutput.SYSBRANCHE = kunde.sysbranche;
                kundeOutput.SYSCTLANG = kunde.sysctlang;
                if(kunde.syskdtyp>0)
                    kundeOutput.SYSKDTYP = kunde.syskdtyp;
                kundeOutput.SYSLAND = kunde.sysland;
                if(kunde.syskd>0)
                {
                    kundeOutput.SYSPERSON=kunde.syskd;
                }
                kundeOutput.SYSLAND2 = kunde.sysland2;
                kundeOutput.SYSCTLANGKORR = kunde.sysctlangkorr;
                kundeOutput.SYSLANDNAT = kunde.syslandnat;
                kundeOutput.SYSSTAAT = kunde.sysstaat;
                kundeOutput.SYSSTAAT2 = kunde.sysstaat2;
                kundeOutput.PTELEFON = kunde.telefon;
                kundeOutput.TELEFON = kunde.telefon2;
                kundeOutput.TITEL = kunde.titel;
                kundeOutput.TITELCODE = kunde.titelCode;
                kundeOutput.TITEL2 = kunde.titel2;
                kundeOutput.URL = kunde.url;
                kundeOutput.VORNAME = kunde.vorname;
                kundeOutput.WOHNSEIT = kunde.wohnseit;
                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.VORNAMEKONT = kunde.vornameKont;

                // Ticket#2012022910000062 : neue Kont-Felder
                kundeOutput.ANREDEKONT = kunde.anredeKont;
                kundeOutput.ANREDECODEKONT = kunde.anredeCodeKont;
                kundeOutput.TITELKONT = kunde.titelKont;
                kundeOutput.MITARBEITERFLAG = kunde.mitarbeiterflag;
                kundeOutput.PRIVATFLAG = kunde.privatflag;
                
                kundeOutput.CODE = NkBuilderFactory.createItNkBuilder().getNextNumber();

                kundeOutput.ZUSATZ = kunde.zusatz;
                kundeOutput.IBAN = kunde.iban;

                kundeOutput.WERBECODE = kunde.werbeCode;
                kundeOutput.INFOMAILFLAG = kunde.infomailflag;
                kundeOutput.INFOMAIL2FLAG = kunde.infomail2flag;
                if (kunde.handy != null && kunde.handy.Length>0)
                    kundeOutput.INFOSMSFLAG = 1;
                else
                    kundeOutput.INFOSMSFLAG = 0;
                kundeOutput.INFOTELFLAG = kunde.infoTelFlag;

                context.SaveChanges();
                _log.Debug("Duration createKunde C " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
                if (sysperole > 0)
                {
                    PEUNIHelper.ConnectNodes(context, PEUNIArea.IT, kundeOutput.SYSIT, sysperole);
                    context.SaveChanges();
                }
                _log.Debug("Duration createKunde D " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
                measure = DateTime.Now.TimeOfDay.Milliseconds;
               
                
                if (kunde.zusatzdaten != null)
                {
                    kunde.sysit = kundeOutput.SYSIT;
                    for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                    {
                        kunde.zusatzdaten[i].kdtyptyp = MyGetKdTypTyp(context, kunde.zusatzdaten[i].kdtyp);
                    }
                }
                updateItoption(kunde, context, kundeOutput);
            }
            IZusatzdatenBo zBo = BOFactory.getInstance().createZusatzdatenBo();
            if (kunde.zusatzdaten != null)
            {
                kunde.sysit = kundeOutput.SYSIT;
                for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                {
                    
                    zusatzdaten.Add(zBo.createOrUpdateZusatzdaten(kunde.zusatzdaten[i], kunde));
                }
            }

            IAdresseBo aBo = BOFactory.getInstance().createAdresseBo();
            IKontoBo kBo = BOFactory.getInstance().createKontoBo();
            
            List<long> kontoIds = new List<long>();
            List<long> adresseIds = new List<long>();


            _log.Debug("Duration createKunde E " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
            measure = DateTime.Now.TimeOfDay.Milliseconds;

            if(kunde.adressen!=null)
            for (int i = 0; i < kunde.adressen.Length; i++)
            {
                // Verknüpfung zu IT, wichtig bei Neuanlage/Update von Adressen
                kunde.adressen[i].sysperson = kundeOutput.SYSIT;
                AdresseDto adresses = aBo.createOrUpdateAdresse(kunde.adressen[i]);
                adresseIds.Add(adresses.sysadresse);
            }
            
            _log.Debug("Duration createKunde F " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
            measure = DateTime.Now.TimeOfDay.Milliseconds;

            if(kunde.kontos!=null)
            for (int i = 0; i < kunde.kontos.Length; i++)
            {
                // Verknüpfung zu IT, wichtig bei Neuanlage/Update von Konten
                kunde.kontos[i].sysperson = kundeOutput.SYSIT;
                KontoDto kontos = kBo.createOrUpdateKonto(kunde.kontos[i]);
                kontoIds.Add(kontos.syskonto);
            }

           

            _log.Debug("Duration createKunde G " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
            measure = DateTime.Now.TimeOfDay.Milliseconds;
            
            KundeDto rval = getKunde(kundeOutput.SYSIT);
            //upon kunde creation/update return only the ukz/pkz given as input
            rval.zusatzdaten = zusatzdaten.ToArray();
            _log.Debug("Duration createKunde H " + (DateTime.Now.TimeOfDay.Milliseconds - measure));
            measure = DateTime.Now.TimeOfDay.Milliseconds;
            return rval;
        }

        /// <summary>
        /// createKunde erstellt ein neuen Datensatz mit einer neuen sysperson/syskd
        /// </summary>
        /// <param name="kunde">KundeDto mit einer syskd = 0</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto mit einer syskd aus der Datenbank</returns>
        public KundeDto createKundePerson(KundeDto kunde, long sysperole)
        {
            PERSON kundeOutput = new PERSON();
            List<ZusatzdatenDto> zusatzdaten = new List<ZusatzdatenDto>();

            using (DdOlExtended context = new DdOlExtended())
            {
                context.PERSON.Add(kundeOutput);
                
                kundeOutput.AUSWEISBEHOERDE = kunde.ausweisbehoerde;
                kundeOutput.LEGITABNEHMER = kunde.legitabnehmer;
                kundeOutput.AUSWEISDATUM = kunde.ausweisdatum;
                kundeOutput.AUSWEISABLAUF = kunde.ausweisablauf;
                kundeOutput.LEGITDATUM = kunde.legitdatum;
                kundeOutput.HREGISTERORT = kunde.hregisterort;
                kundeOutput.AKTIVKZ = kunde.aktivkz;
                kundeOutput.FLAGKD = kunde.flagkd;
                kundeOutput.AUSWEISART = kunde.ausweisart;
                //kundeOutput.WOHNUNGART = kunde.wohnungart;
                kundeOutput.AUSWEISNR = kunde.ausweisnr;
                
                kundeOutput.ANREDE = kunde.anrede;
                kundeOutput.ANREDECODE = kunde.anredeCode;
                kundeOutput.AUSLAUSWEIS = kunde.auslausweis;
                kundeOutput.AUSLAUSWEISCODE = kunde.auslausweisCode;
                if (
                    //(kundeOutput.AUSLAUSWEIS == null || kundeOutput.AUSLAUSWEIS.Length < 1) && //always update by code otherwise the values will not correspond!
                    kunde.auslausweisCode != null && kunde.auslausweisCode.Length > 0)
                {
                    object[] pars = { 
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = kunde.auslausweisCode } 
                    };
                    kundeOutput.AUSLAUSWEIS = context.ExecuteStoreQuery<String>("select trim(value) from ddlkppos where code='AUSLAUSWEIS' and id=:id", pars).FirstOrDefault();
                }
                kundeOutput.AUSLAUSWEISGUELTIG = kunde.auslausweisGueltig;
                kundeOutput.EINREISEDATUM = kunde.einreisedatum;
                kundeOutput.EMAIL = kunde.email;

                DateTime now = DateTime.Now;
                if (kunde.erreichbVon > 2359)
                    kunde.erreichbVon = 0;
                if (kunde.erreichbBis > 2359)
                    kunde.erreichbBis = 0;

                DateTime ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbBis / 100, (int)(kunde.erreichbBis - kunde.erreichbBis / 100 * 100), 0);
                kundeOutput.ERREICHBBIS = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);

                ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbVon / 100, (int)(kunde.erreichbVon - kunde.erreichbVon / 100 * 100), 0);
                kundeOutput.ERREICHBVON = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);

                kundeOutput.ERREICHBTEL = kunde.erreichbtel;
                kundeOutput.FAX = kunde.fax;
                kundeOutput.GEBDATUM = kunde.gebdatum;
                kundeOutput.GEBORT = kunde.gebort;
                kundeOutput.GRUENDUNG = kunde.gruendung;
                kundeOutput.HANDY = kunde.handy;
                kundeOutput.HREGISTER = kunde.hregister;
                kundeOutput.HREGISTERORT = kunde.hregisterort;
                kundeOutput.HREGISTERFLAG = Convert.ToInt32(kunde.hregisterFlag);
                kundeOutput.HSNR = kunde.hsnr;
                kundeOutput.HSNR2 = kunde.hsnr2;
                kundeOutput.IDENTUST = kunde.identust;
                
                kundeOutput.NAME = kunde.name;
                kundeOutput.ORT = kunde.ort;
                kundeOutput.ORT2 = kunde.ort2;
                kundeOutput.PLZ = kunde.plz;
                kundeOutput.PLZ2 = kunde.plz2;
                kundeOutput.RECHTSFORM =kunde.rechtsform;
                
                kundeOutput.RECHTSFORMCODE = kunde.rechtsformCode;
                kundeOutput.REVFLAG = Convert.ToInt32(kunde.revFlag);
                kundeOutput.STRASSE = kunde.strasse;
                kundeOutput.STRASSE2 = kunde.strasse2;
                kundeOutput.SYSBRANCHE = kunde.sysbranche;
                kundeOutput.SYSCTLANG = kunde.sysctlang;
                kundeOutput.SYSKDTYP= kunde.syskdtyp;
                kundeOutput.SYSLAND= kunde.sysland;
                
                kundeOutput.SYSLAND2 = kunde.sysland2;
                kundeOutput.SYSCTLANGKORR = kunde.sysctlangkorr;
                kundeOutput.SYSLANDNAT = kunde.syslandnat;
                
                kundeOutput.SYSSTAAT=kunde.sysstaat;
                kundeOutput.SYSSTAAT2 = kunde.sysstaat2;
                kundeOutput.PTELEFON = kunde.telefon;
                kundeOutput.TELEFON = kunde.telefon2;
                kundeOutput.TITEL = kunde.titel;
                kundeOutput.TITELCODE = kunde.titelCode;
                kundeOutput.TITEL2CODE = kunde.titel2;
                kundeOutput.URL = kunde.url;
                kundeOutput.UIDNUMMER = kunde.uidnummer;
                kundeOutput.VORNAME = kunde.vorname;
                kundeOutput.WOHNSEIT = kunde.wohnseit;
                kundeOutput.ZUSATZ = kunde.zusatz;

                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.VORNAMEKONT = kunde.vornameKont;

                // Ticket#2012022910000062 : neue Kont-Felder
                //kundeOutput.ANREDEKONT = kunde.anredeKont;
                kundeOutput.ANREDECODEKONT = kunde.anredeCodeKont;
                //kundeOutput.TITELKONT = kunde.titelKont;
                kundeOutput.MITARBEITERFLAG = kunde.mitarbeiterflag;
                kundeOutput.PRIVATFLAG = kunde.privatflag;
                
                kundeOutput.CODE = NkBuilderFactory.createPersonNkBuilder().getNextNumber();

                kundeOutput.ZUSATZ = kunde.zusatz;
                kundeOutput.IBAN = kunde.iban;

                kundeOutput.SCHUFAID = kunde.schufaid;
                kundeOutput.CREFOID = kunde.crefoid;
                kundeOutput.FICOID = kunde.ficoid;

                kundeOutput.WERBECODE = kunde.werbeCode;
                kundeOutput.INFOMAILFLAG = kunde.infomailflag;
                kundeOutput.INFOMAIL2FLAG = kunde.infomail2flag;
                if (kunde.handy != null && kunde.handy.Length > 0)
                    kundeOutput.INFOSMSFLAG = 1;
                else
                    kundeOutput.INFOSMSFLAG = 0;
                kundeOutput.INFOTELFLAG = kunde.infoTelFlag;

                context.SaveChanges();

                if(sysperole>0)
                { 
                    PEUNIHelper.ConnectNodes(context, PEUNIArea.PERSON, kundeOutput.SYSPERSON, sysperole);
                    context.SaveChanges();
                }

                

                if (kunde.zusatzdaten != null)
                {
                    kunde.syskd = kundeOutput.SYSPERSON;
                    for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                    {
                        kunde.zusatzdaten[i].kdtyptyp = MyGetKdTypTyp(context, kunde.zusatzdaten[i].kdtyp);
                       
                    }
                }
            }

            IZusatzdatenBo zBo = BOFactory.getInstance().createZusatzdatenBo();

            if (kunde.zusatzdaten != null)
            {
                kunde.syskd = kundeOutput.SYSPERSON;
                for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                {
                    
                    zusatzdaten.Add(zBo.createOrUpdateZusatzdatenPerson(kunde.zusatzdaten[i], kunde));
                }
            }
            
            KundeDto rval = getKundeBySysKd(kundeOutput.SYSPERSON);
            //upon kunde creation/update return only the ukz/pkz given as input
            rval.zusatzdaten = zusatzdaten.ToArray();
            return rval;
        }

        /// <summary>
        /// getKunde holt zu einer SYSIT den passenden Datensatz und liefert ihn zurück
        /// </summary>
        /// <param name="sysit">SYSIT des gewünschten Datensatzes</param>
        /// <returns>KundeDto mit der gleichen SYSIT</returns>
        public KundeDto getKunde(long sysit)
        {
            return getKunde(sysit, 0);
        }
        /// <summary>
        /// getKunde holt zu einer SYSIT den passenden Datensatz und liefert ihn zurück
        /// </summary>
        /// <param name="sysit">SYSIT des gewünschten Datensatzes</param>
        /// <param name="sysangebot">id des Angebots für pkz</param>
        /// <returns>KundeDto mit der gleichen SYSIT</returns>
        public KundeDto getKunde(long sysit,long sysangebot)
        {
            if (sysit == 0)
                return new KundeDto();

            using (DdOlExtended context = new DdOlExtended())
            {
                DbConnection con = (context.Database.Connection);
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                  
                KundeDto rval = con.Query<KundeDto>("select it.*,sysperson syskd from it where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();
                if (rval.syskdtyp == 0)
                {
                    rval.syskdtyp = 1;
                }
                KundeDto mapKd = con.Query<KundeDto>("select ptelefon telefon, telefon telefon2 from it where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();
                rval.telefon2 = mapKd.telefon2;
                rval.telefon = mapKd.telefon;
               
                if (rval == null)
                {
                    _log.Error("No Kunde found for this sysIT: " + sysit);
                    throw new ApplicationException("No Kunde found in IT.");
                }
                DateTime now = DateTime.Now;


                DateTime? ttemp = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTimeNoException((int)rval.erreichbBis);
                if(ttemp.HasValue)
                    rval.erreichbBis = ttemp.Value.Hour * 100 + ttemp.Value.Minute;
                ttemp = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTimeNoException((int)rval.erreichbVon);
                if (ttemp.HasValue)
                    rval.erreichbVon = ttemp.Value.Hour * 100 + ttemp.Value.Minute;
               

                rval.kdtypBezeichnung = MyGetKdTypBezeichnung(context, rval.syskdtyp);

                // Security Check: Aufruf nur mit long
                //jüngster angebotsbezogener satz
                PkzDto itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysit and sysangebot>0 and sysangebot=:sysangebot order by sysitpkz desc", new { sysit = sysit, sysangebot = sysangebot }).FirstOrDefault();
                if (itpkz == null)//kein pkz für gegebenes angebot
                {
                    itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysit and sysangebot>0 order by sysitpkz desc", new { sysit = sysit }).FirstOrDefault();
                    if (itpkz != null)//hat eine zuordnung zu einem evtl anderen angebot
                    {
                        itpkz.sysangebot = 0;
                        itpkz.syspkz = 0;//neuanlage erzwingen
                    }
                }
                //jüngster satz ohne angebot
                if(itpkz==null)
                    itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysit order by sysitpkz desc", new { sysit = sysit }).FirstOrDefault();
                int pkzSize = itpkz != null ? 1 : 0;
                //Mapper.CreateMap<ITPKZ, PkzDto>();
                rval.zusatzdaten = new ZusatzdatenDto[1];
                int ipkz = 0;
                rval.zusatzdaten[0] = new ZusatzdatenDto();
                rval.zusatzdaten[0].pkz = new PkzDto[pkzSize];
                int kdtyptyp = MyGetKdTypTyp(context, rval.syskdtyp);
                switch (kdtyptyp)
                {
                    case 1: rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_PRIVAT; rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_PRIVAT; break;
                    case 3: rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_FIRMA; rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_FIRMA; break;
                    default: rval.zusatzdaten[0].kdtyp = 0; break;
                }
                PkzDto pkzFromKremo = con.Query<PkzDto>("select angebot.sysit syspkz,kremo.krankenkasse, kremo.extbetrkostentat betreuungskosten, arbeitswegauslagen arbeitswegpauschale1,arbeitswegauslagen2 arbeitswegpauschale2 from kremo,angebot where angebot.sysid=kremo.sysangebot and kremo.sysangebot=:sysangebot order by syskremo desc", new { sysangebot = sysangebot }).FirstOrDefault();

                //foreach (ITPKZ itpkz in pkzlist)
                if (itpkz != null)
                {
                    itpkz.zeinkCode = ZusatzdatenDao.fixStringCode(itpkz.zeinkCode);
                    itpkz.wohnverhCode = ZusatzdatenDao.fixStringCode(itpkz.wohnverhCode);
                    itpkz.beruflichCode = ZusatzdatenDao.fixStringCode(itpkz.beruflichCode);
                    itpkz.auslagenCode = ZusatzdatenDao.fixStringCode(itpkz.auslagenCode);
                    
                    if(pkzFromKremo!=null)//fetch data From KREMO
                    {
                        if (sysit == pkzFromKremo.syspkz)//when hauptantragsteller
                        {
                            itpkz.betreuungskosten = pkzFromKremo.betreuungskosten;
                            itpkz.arbeitswegpauschale1 = pkzFromKremo.arbeitswegpauschale1;
                            itpkz.krankenkasse = pkzFromKremo.krankenkasse;
                        }
                        else
                            itpkz.arbeitswegpauschale1 = pkzFromKremo.arbeitswegpauschale2;

                    }


                    rval.zusatzdaten[0].pkz[ipkz] = itpkz;
                    ipkz++;
                }

                // Security Check: Aufruf nur mit long
                UkzDto itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysit and sysangebot>0 and sysangebot=:sysangebot order by sysitukz desc", new { sysit = sysit, sysangebot = sysangebot }).FirstOrDefault();
                if (itukz == null)
                {
                    itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysit and sysangebot>0 order by sysitukz desc", new { sysit = sysit }).FirstOrDefault();
                    if (itukz != null)//hat eine zuordnung zu einem evtl anderen angebot
                    {
                        itukz.sysangebot = 0;
                        itukz.sysukz = 0;//neuanlage erzwingen
                    }
                }
                if(itukz==null)
                    itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysit order by sysitukz desc", new { sysit = sysit }).FirstOrDefault();
                pkzSize = itukz != null ? 1 : 0;
                int iukz = 0;
                
                rval.zusatzdaten[0].ukz = new UkzDto[pkzSize];

                if (itukz != null)
                {
                    rval.zusatzdaten[0].ukz[iukz] = itukz;

                    iukz++;
                }

                rval.adressen = getAdressen(rval.sysit);

				// Security Check: Aufruf nur mit long
				List<ITKONTO> itKontoList = context.ExecuteStoreQuery<ITKONTO> (
					"select itkonto.* from itkontoref, itkonto where itkontoref.sysitkonto=itkonto.sysitkonto and itkontoref.sysit=" + sysit, null).ToList ();


				rval.kontos = new KontoDto[itKontoList.Count];
				int ikontos = 0;
				foreach (ITKONTO itkonto in itKontoList)
				{
					rval.kontos[ikontos] = Mapper.Map<ITKONTO, KontoDto> (itkonto);
					rval.kontos[ikontos].sysperson = rval.sysit;

					// Ticket#2012090410000131: falsche Blz zurückgegeben
					rval.kontos[ikontos].blz = (from blz in context.BLZ where blz.SYSBLZ == itkonto.SYSBLZ select blz.BLZ1).FirstOrDefault ();

					//««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««
					//// rh WEG, WEIL BLZ1 gibt es in Tabelle BLZ gar nicht!
					//««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««
					//// UMWANDELN IN SQL:
					//if (itkonto.SYSBLZ.HasValue && itkonto.SYSBLZ > 0)
					//{
					//	rval.kontos[ikontos].blz = context.ExecuteStoreQuery<string> ("SELECT BLZ.BLZ FROM BLZ WHERE SYSBLZ=:sysblz", new { sysblz = itkonto.SYSBLZ }).FirstOrDefault ();
					//}
					//««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««

					rval.kontos[ikontos].syskonto = itkonto.SYSITKONTO;

					//——————————————————————————————————————————————————————————————————————————————————————————————
					// rh: TRY OMIT string with SPECIAL CONTROL (here INVALID) CHARS lower ASCII 32 (rh 20181118) 
					// due to tech-error BNRSIZE-458 [Ticket#2017032410000033] 
					//——————————————————————————————————————————————————————————————————————————————————————————————
					if (OpenOne.Common.Util.StringUtil.ContainsSpecialChars (rval.kontos[ikontos].kontonr))
					{
						rval.kontos[ikontos].kontonr = null;
					}

					ikontos++;
				}
				//——————————————————————————————————————————————————————————————————————————————————————————————

				rval.sysitkorradresse = con.Query<long>("select sysitadresse from itadresse where rang=2 and sysit=:sysit", new { sysit = sysit }).FirstOrDefault();
                if (rval.auslausweisCode != null)
                    rval.auslausweisCode = rval.auslausweisCode.Trim();


                KundeDto tmpOptions= con.Query<KundeDto>("select flag06 KNEManuell,FLAG01 feststellungsPflicht from itoption where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();
                if (tmpOptions != null)
                {
                    rval.KNEManuell = tmpOptions.KNEManuell;
                    rval.feststellungsPflicht = tmpOptions.feststellungsPflicht;
                }
                        
                return rval;
            }
        }

        /// <summary>
        /// getKunde holt zu einer SYSIT den passenden Datensatz und liefert ihn zurück
        /// 1. erstes PKZ/UKZ zum Antrg passend.
        /// 2. Ganz am Schluß PKZ/UKZ. Wenn Zustand 'Final' Mappen von PKZ/UKZ auf Kunden DTO.
        /// </summary>
        /// <param name="sysit">SYSIT des gewünschten Datensatzes</param>
        /// <param name="sysantrag">Antrag ID</param>
        /// <returns>KundeDto mit der gleichen SYSIT</returns>
        public KundeDto getKundeViaAntragID(long sysit, long sysantrag)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                DbConnection con = (context.Database.Connection);
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                //KundeDto rval = con.Query<KundeDto>("select it.*,sysperson syskd from it where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();

                AntragDto AntragData = con.Query<AntragDto>("select zustand,attribut from antrag where sysid=:sysid", new { sysid = sysantrag }).FirstOrDefault();
                

                /*ANTRAG AntragData = (from ant in context.ANTRAG
                                     where ant.SYSID == sysantrag
                                     select ant).FirstOrDefault();*/

                bool bAbgeschlossen = (AntragData.zustand.IndexOf("Abgeschlossen") != -1);
                bool bAbgelehnt = (AntragData.attribut.IndexOf("Abgelehnt") != -1);
                bool bVerzichtet = (AntragData.attribut.IndexOf("Verzichtet") != -1);
                bool bGeloescht = (AntragData.attribut.IndexOf("Gelöscht") != -1);
                bool bAktiviert = (AntragData.attribut.IndexOf("Vertrag aktiviert") != -1);

                KundeDto rval = con.Query<KundeDto>("select it.*,sysperson syskd from it where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();

                KundeDto mapKd = con.Query<KundeDto>("select ptelefon telefon, telefon telefon2 from it where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();

                rval.telefon2 = mapKd.telefon2;
                rval.telefon = mapKd.telefon;

                if (rval == null)
                {
                    _log.Error("No Kunde found for this sysIT: " + sysit);
                    throw new ApplicationException("No Kunde found in IT.");
                }
                long sysktyp = rval.syskdtyp;

                List<PkzDto> pkzs = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysit and sysantrag>0 and sysantrag=:sysantrag order by sysitpkz desc", new { sysit = sysit, sysantrag = sysantrag }).ToList();
                if (pkzs == null) pkzs = new List<PkzDto>();

                PkzDto pkzFromKremo = con.Query<PkzDto>("select antrag.sysit syspkz, kremo.extbetrkostentat betreuungskosten, krankenkasse, arbeitswegauslagen arbeitswegpauschale1,arbeitswegauslagen2 arbeitswegpauschale2 from kremo,antrag where antrag.sysid=kremo.sysantrag and kremo.sysantrag=:sysantrag order by syskremo desc", new { sysantrag = sysantrag }).FirstOrDefault();

                rval.zusatzdaten = new ZusatzdatenDto[1];
                int ipkz = 0;
                rval.zusatzdaten[0] = new ZusatzdatenDto();
                rval.zusatzdaten[0].pkz = new PkzDto[pkzs.Count];
                int kdtyptyp = MyGetKdTypTyp(context, sysktyp);
                switch (kdtyptyp)
                {
                    case 1:
                        rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_PRIVAT;
                        rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_PRIVAT;
                        break;

                    case 2:
                        rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_PRIVAT;
                        rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_PRIVAT;
                        break;

                    case 3:
                        rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_FIRMA;
                        rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_FIRMA;
                        break;
                    default: rval.zusatzdaten[0].kdtyp = 0; break;
                }

                PkzDto actPkz = null;
                foreach (PkzDto itpkz in pkzs)
                {
                    itpkz.zeinkCode = ZusatzdatenDao.fixStringCode(itpkz.zeinkCode);
                    itpkz.wohnverhCode = ZusatzdatenDao.fixStringCode(itpkz.wohnverhCode);
                    itpkz.beruflichCode = ZusatzdatenDao.fixStringCode(itpkz.beruflichCode);
                    itpkz.auslagenCode = ZusatzdatenDao.fixStringCode(itpkz.auslagenCode);

                    if (pkzFromKremo != null)//fetch data From KREMO
                    {
                        if (sysit == pkzFromKremo.syspkz)//Hauptantragsteller
                        {
                            itpkz.betreuungskosten = pkzFromKremo.betreuungskosten;
                            itpkz.arbeitswegpauschale1 = pkzFromKremo.arbeitswegpauschale1;
                            itpkz.krankenkasse = pkzFromKremo.krankenkasse;
                        }
                        else
                            itpkz.arbeitswegpauschale1 = pkzFromKremo.arbeitswegpauschale2;
                    }

                    rval.zusatzdaten[0].pkz[ipkz] = itpkz;

                    if (actPkz==null)
                    {
                        actPkz = itpkz;
                    }
                    ipkz++;
                }

                int iukz = 0;

                List<UkzDto> ukzs = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysit and sysantrag>0 and sysantrag=:sysantrag order by sysitukz desc", new { sysit = sysit, sysantrag = sysantrag }).ToList();
                if (ukzs == null)
                    ukzs = new List<UkzDto>();


                rval.zusatzdaten[0].ukz = new UkzDto[ukzs.Count];
                UkzDto actUkz = null;

                foreach (UkzDto itukz in ukzs)
                {
                    rval.zusatzdaten[0].ukz[iukz] = itukz;
                    if (actUkz==null)
                    {
                        actUkz = itukz;
                    }
                    iukz++;
                }
                //KNE Fill upon load
                if(kdtyptyp==3)//for company only
                    rval.zusatzdaten[0].kne = con.Query<KneDto>("select * from itkne where sysunter=:sysit and area='ANTRAG' and sysarea=:sysantrag", new {sysit=sysit, sysantrag = sysantrag }).ToList();

                rval.adressen = getAdressen(rval.sysit);

                rval.kontos = con.Query<KontoDto>("select itkontoref.sysantrag,itkonto.sysit sysperson, itkonto.rang,itkonto.sysblz,itkonto.kontonr,itkonto.iban,itkonto.bezeichnung kontobezeichnung,itkonto.sysitkonto syskonto, (select blz from blz where sysblz=itkonto.sysblz) blz from itkontoref, itkonto where itkontoref.sysitkonto=itkonto.sysitkonto and itkontoref.sysit=:sysit and itkontoref.sysantrag=:sysantrag", new { sysit = sysit, sysantrag = sysantrag }).ToArray();
                

				// Finaler Status?
                if (bAbgeschlossen && (bAbgelehnt || bVerzichtet || bGeloescht || bAktiviert))
                {
                    if (actPkz != null)
                    {
                        ITPKZ itp = context.ExecuteStoreQuery<ITPKZ>("select * from itpkz where sysitpkz=" + actPkz.syspkz).FirstOrDefault();
                        
                        Mapper.Map(itp, rval);
                        rval.telefon = itp.PTELEFON;
                        rval.telefon2 = itp.TELEFON;
                    }
                    else if (actUkz != null)
                    {
                        ITUKZ itu = context.ExecuteStoreQuery<ITUKZ>("select * from itukz where sysitukz=" + actUkz.sysukz).FirstOrDefault();
                       
                        Mapper.Map(itu, rval);
                        rval.telefon = itu.PTELEFON;
                        rval.telefon2 = itu.TELEFON;
                    }
                    else
                    {
                        throw new PkzUkzException("No PKZ/UKZ for finalized application!");
                    }
                    if(rval.syskdtyp==0)
                    {
                       rval.syskdtyp = sysktyp;
                    }
                }
                if (rval.auslausweisCode != null)
                    rval.auslausweisCode = rval.auslausweisCode.Trim();

                KundeDto tmpOptions = con.Query<KundeDto>("select flag06 KNEManuell,FLAG01 feststellungsPflicht from itoption where sysit=:sysit", new { sysit = sysit }).FirstOrDefault();
                if (tmpOptions != null)
                {
                    rval.KNEManuell = tmpOptions.KNEManuell;
                    rval.feststellungsPflicht = tmpOptions.feststellungsPflicht;
                }
                return rval;
            }
        }

        /// <summary>
        /// getKunde holt zu einer SYSKD den passenden Datensatz und liefert ihn zurück
        /// </summary>
        /// <param name="syskd">SYSKD des gewünschten Datensatzes</param>
        /// <returns>KundeDto mit der gleichen SYSKD</returns>
        public KundeDto getKundeBySysKd(long syskd)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                
                
                DbConnection con = (context.Database.Connection);
                KundeDto rval = con.Query<KundeDto>("select person.*, person.ptelefon telefon, person.telefon telefon2, person.sysperson syskd from person where sysperson=:syskd", new { syskd = syskd }).FirstOrDefault();


                if(rval==null||rval.syskd==0)
                {
                    _log.Error("No Kunde found by this syskd: " + syskd);
                    throw new ApplicationException("No Kunde found in PERSON.");
                }
             
                rval.kdtypBezeichnung = MyGetKdTypBezeichnung(context, rval.syskdtyp);
              
                rval.zusatzdaten = new ZusatzdatenDto[1];
                
                rval.zusatzdaten[0] = new ZusatzdatenDto();
               
                int kdtyptyp = MyGetKdTypTyp(context, rval.syskdtyp);
                switch (kdtyptyp)
                {
                    case 1: rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_PRIVAT; rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_PRIVAT; break;
                    case 3: rval.zusatzdaten[0].kdtyp = AngAntBo.KDTYPID_FIRMA; rval.zusatzdaten[0].kdtyptyp = AngAntBo.KDTYPTYP_FIRMA; break;
                    default: rval.zusatzdaten[0].kdtyp = 0; break;
                }
                List<PkzDto> pkz = con.Query<PkzDto>("select pkz.*, pkz.kredrate kredtrate, pkz.anzahlvollstr anzvollstr from pkz where sysperson=:sysperson and rownum<10", new { sysperson = syskd }).ToList();
                rval.zusatzdaten[0].pkz = pkz.ToArray();
               

                List<UkzDto> ukz = con.Query<UkzDto>("select ukz.*, ukz.LIQUIDITAET liquiditaet, ukz.OBLIGOEIGEN obliboeigen from ukz where sysperson=:sysperson and rownum<10", new { sysperson = syskd }).ToList();
                rval.zusatzdaten[0].ukz = ukz.ToArray();

                List<AdresseDto> adr = con.Query<AdresseDto>("select adresse.* from adrref,adresse where adrref.sysadresse=adresse.sysadresse and adrref.sysperson=:sysperson", new { sysperson = syskd }).ToList();
                rval.adressen = adr.ToArray();

                rval.syskorradresse = con.Query<long>("select sysadresse from adresse where rang=2 and sysperson=:sysperson", new { sysperson = syskd }).FirstOrDefault();

                rval.sysit = con.Query<long>("select sysit from it where sysperson=:sysperson", new { sysperson = syskd }).FirstOrDefault();

                List<KontoDto> kto = con.Query<KontoDto>("select konto.* from konto where sysperson=:sysperson", new { sysperson = syskd }).ToList();
                rval.kontos = kto.ToArray();
                if (rval.auslausweisCode != null)
                    rval.auslausweisCode = rval.auslausweisCode.Trim();

                return rval;
            }
        }

        /// <summary>
        /// updateKunde speichert einen IT Datensatz zu einem bestehnden Kunden
        /// </summary>
        /// <param name="kunde">KundeDto mit einer SYSIT > 0</param>
        /// <returns>KundeDto mit der selben SYSIT</returns>
        public KundeDto updateKunde(KundeDto kunde)
        {
            if (kunde == null)
            {
                _log.Error("No Kunde received as inputdata.");
                throw new ApplicationException("No Kunde received as inputdata.");
            }
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            IT kundeOutput = null;
            using (DdOlExtended context = new DdOlExtended())
            {
                
                kundeOutput = (from it in context.IT
                                  where it.SYSIT == kunde.sysit
                                  select it).FirstOrDefault();
                _log.Debug("updateKunde1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                if (kundeOutput == null)
                {
                    _log.Error("No Kunde found for this SYSID: " + kunde.sysit);
                    throw new ApplicationException("No Kunde found for this sysIT: " + kunde.sysit);
                }
                kundeOutput.ANREDE = kunde.anrede;
                kundeOutput.ANREDECODE = kunde.anredeCode;
                kundeOutput.AUSLAUSWEIS = kunde.auslausweis;
                kundeOutput.AUSLAUSWEISCODE = kunde.auslausweisCode;
                kundeOutput.BESCHARTAG = kunde.beschartag;
                if ((kundeOutput.AUSLAUSWEIS == null || kundeOutput.AUSLAUSWEIS.Length < 1) && kunde.auslausweisCode != null && kunde.auslausweisCode.Length > 0)
                {
                    object[] pars = {
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = kunde.auslausweisCode }
                    };
                    kundeOutput.AUSLAUSWEIS = context.ExecuteStoreQuery<String>("select trim(value) from ddlkppos where code='AUSLAUSWEIS' and id=:id", pars).FirstOrDefault();
                }
                kundeOutput.AUSLAUSWEISGUELTIG = kunde.auslausweisGueltig;
                kundeOutput.EINREISEDATUM = kunde.einreisedatum;
                kundeOutput.EMAIL = kunde.email;

                DateTime now = DateTime.Now;

                DateTime ttemp;

                if (kunde.erreichbVon > 2359)
                    kunde.erreichbVon = 0;
                if (kunde.erreichbBis > 2359)
                    kunde.erreichbBis = 0;


                if (kunde.erreichbBis > 0)
                {
                    ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbBis / 100, (int)(kunde.erreichbBis - kunde.erreichbBis / 100 * 100), 0);
                    kundeOutput.ERREICHBBIS = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);
                }
                if (kunde.erreichbVon > 0)
                {

                    ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbVon / 100, (int)(kunde.erreichbVon - kunde.erreichbVon / 100 * 100), 0);
                    kundeOutput.ERREICHBVON = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);
                }

                kundeOutput.BERUF = kunde.beruf;
                kundeOutput.ERREICHBTEL = kunde.erreichbtel;
                kundeOutput.GEBDATUM = kunde.gebdatum;
                kundeOutput.GRUENDUNG = kunde.gruendung;
                kundeOutput.HANDY = kunde.handy;
                kundeOutput.HREGISTER = kunde.hregister;
                kundeOutput.HREGISTERFLAG = Convert.ToInt32(kunde.hregisterFlag);
                kundeOutput.HSNR = kunde.hsnr;
                kundeOutput.HSNR2 = kunde.hsnr2;
                kundeOutput.NAME = kunde.name;
                kundeOutput.ORT = kunde.ort;
                kundeOutput.ORT2 = kunde.ort2;
                kundeOutput.PLZ = kunde.plz;
                kundeOutput.PLZ2 = kunde.plz2;
                try
                {
                    kundeOutput.RECHTSFORM = Convert.ToInt32(kunde.rechtsform);
                }
                catch
                {
                    //Do Nothing;
                };
                kundeOutput.RECHTSFORMCODE = kunde.rechtsformCode;
                kundeOutput.REVFLAG = Convert.ToInt32(kunde.revFlag);
                kundeOutput.STRASSE = kunde.strasse;
                kundeOutput.STRASSE2 = kunde.strasse2;
                kundeOutput.SYSBRANCHE = kunde.sysbranche;
                kundeOutput.SYSCTLANG = kunde.sysctlang;
                if(kunde.syskdtyp>0)
                    kundeOutput.SYSKDTYP = kunde.syskdtyp;
                kundeOutput.SYSLAND = kunde.sysland;
                kundeOutput.SYSLAND2 = kunde.sysland2;
                kundeOutput.SYSCTLANGKORR = kunde.sysctlangkorr;
                kundeOutput.SYSLANDNAT = kunde.syslandnat;
                kundeOutput.SYSSTAAT = kunde.sysstaat;
                kundeOutput.SYSSTAAT2 = kunde.sysstaat2;
                kundeOutput.PTELEFON = kunde.telefon;
                kundeOutput.TELEFON = kunde.telefon2;
                kundeOutput.TITEL = kunde.titel;
                kundeOutput.TITELCODE = kunde.titelCode;
                kundeOutput.TITEL2 = kunde.titel2;
                kundeOutput.URL = kunde.url;
                kundeOutput.VORNAME = kunde.vorname;
                kundeOutput.WOHNSEIT = kunde.wohnseit;
                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.VORNAMEKONT = kunde.vornameKont;
                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.ZUSATZ = kunde.zusatz;
                kundeOutput.MITARBEITERFLAG = kunde.mitarbeiterflag;
                kundeOutput.PRIVATFLAG = kunde.privatflag;

                // Ticket#2012022910000062 : neue Kont-Felder
                kundeOutput.ANREDEKONT = kunde.anredeKont;
                kundeOutput.ANREDECODEKONT = kunde.anredeCodeKont;
                kundeOutput.TITELKONT = kunde.titelKont;

                //HCE
                kundeOutput.IBAN = kunde.iban;
                kundeOutput.WOHNUNGART = kunde.wohnungart;
                kundeOutput.LEGITABNEHMER = kunde.legitabnehmer;
                kundeOutput.AUSWEISBEHOERDE = kunde.ausweisbehoerde;
                kundeOutput.AUSWEISDATUM = kunde.ausweisdatum;
                kundeOutput.AUSWEISABLAUF = kunde.ausweisablauf;
                kundeOutput.AUSWEISNR = kunde.ausweisnr;
                kundeOutput.LEGITDATUM = kunde.legitdatum;
                kundeOutput.HREGISTERORT = kunde.hregisterort;
                kundeOutput.IDENTUST = kunde.identust;
                kundeOutput.UIDNUMMER = kunde.uidnummer;
                kundeOutput.FAX = kunde.fax;

                kundeOutput.WERBECODE = kunde.werbeCode;
                kundeOutput.INFOMAILFLAG = kunde.infomailflag;
                kundeOutput.INFOMAIL2FLAG = kunde.infomail2flag;
                if (kunde.handy != null && kunde.handy.Length > 0)
                    kundeOutput.INFOSMSFLAG = 1;
                else
                    kundeOutput.INFOSMSFLAG = 0;
                kundeOutput.INFOTELFLAG = kunde.infoTelFlag;
                try
                {
                    kundeOutput.AUSWEISART = int.Parse(kunde.ausweisart);
                }
                catch (Exception) { }

                context.SaveChanges();
                _log.Debug("updateKunde2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                
                

                
                for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                {
                    kunde.zusatzdaten[i].kdtyptyp = MyGetKdTypTyp(context, kunde.zusatzdaten[i].kdtyp);
                }

                updateItoption(kunde, context, kundeOutput);
               
            }

            IAdresseBo aBo = BOFactory.getInstance().createAdresseBo();
            IKontoBo kBo = BOFactory.getInstance().createKontoBo();
            List<long> kontoIds = new List<long>();
            List<long> adresseIds = new List<long>();

            //if no address given, remove existing address
            if (kunde.adressen == null || kunde.adressen.Length == 0)
            {
                aBo.deleteAdresse(kundeOutput.SYSIT);
            }
            else
            {
                for (int i = 0; i < kunde.adressen.Length; i++)
                {
                    kunde.adressen[i].sysperson = kundeOutput.SYSIT;
                    AdresseDto adresses = aBo.createOrUpdateAdresse(kunde.adressen[i]);
                    adresseIds.Add(adresses.sysadresse);
                }
            }
            _log.Debug("updateKunde3: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            for (int i = 0; i < kunde.kontos.Length; i++)
            {
                kunde.kontos[i].sysperson = kundeOutput.SYSIT;
                KontoDto kontos = kBo.createOrUpdateKonto(kunde.kontos[i]);
                kontoIds.Add(kontos.syskonto);
            }
            _log.Debug("updateKunde4: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

            IZusatzdatenBo zBo = BOFactory.getInstance().createZusatzdatenBo();
            List<ZusatzdatenDto> zusatzdaten = new List<ZusatzdatenDto>();
            for (int i = 0; i < kunde.zusatzdaten.Length; i++)
            {
                zusatzdaten.Add(zBo.createOrUpdateZusatzdaten(kunde.zusatzdaten[i], kunde));
            }

            _log.Debug("updateKunde5: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            KundeDto rval = getKunde(kundeOutput.SYSIT);
            _log.Debug("updateKunde6: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            //upon kunde creation/update return only the ukz/pkz given as input
            rval.zusatzdaten = zusatzdaten.ToArray();
            return rval;
        }

        private static void updateItoption(KundeDto kunde, DdOlExtended context, IT kundeOutput)
        {
            long optcount = context.ExecuteStoreQuery<long>("select count(*) from itoption where sysit=" + kundeOutput.SYSIT).FirstOrDefault();
            if (optcount == 0)
            {
                context.ExecuteStoreCommand("insert into itoption(sysitoption,sysit,flag06) values(" + kundeOutput.SYSIT + "," + kundeOutput.SYSIT + "," + kunde.KNEManuell.GetValueOrDefault() + ")");
            }
            else
            {
                context.ExecuteStoreCommand("update itoption set flag06=" + kunde.KNEManuell.GetValueOrDefault() + " where sysit=" + kundeOutput.SYSIT);
            }
        }


        /// <summary>
        /// updateKunde speichert einen Datensatz zu einem bestehnden Kunden
        /// </summary>
        /// <param name="kunde">KundeDto mit einer syskd > 0</param>
        /// <returns>KundeDto mit der selben syskd</returns>
        public KundeDto updateKundePerson(KundeDto kunde)
        {
            if (kunde == null)
            {
                _log.Error("No Kunde received as inputdata.");
                throw new ApplicationException("No Kunde received as inputdata.");
            }
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            PERSON kundeOutput = null;
            using (DdOlExtended context = new DdOlExtended())
            {
                
                kundeOutput = (from it in context.PERSON
                                  where it.SYSPERSON== kunde.syskd
                                  select it).FirstOrDefault();
                _log.Debug("updateKunde1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                if (kundeOutput == null)
                {
                    _log.Error("No Kunde found for this SYSID: " + kunde.sysit);
                    throw new ApplicationException("No Kunde found for this sysIT: " + kunde.sysit);
                }
                kundeOutput.ANREDE = kunde.anrede;
                kundeOutput.ANREDECODE = kunde.anredeCode;
                kundeOutput.AUSLAUSWEIS = kunde.auslausweis;
                kundeOutput.AUSLAUSWEISCODE = kunde.auslausweisCode;
                kundeOutput.AUSWEISART = kunde.ausweisart;

                if (
                    //(kundeOutput.AUSLAUSWEIS == null || kundeOutput.AUSLAUSWEIS.Length < 1) && //always update by code otherwise the values will not correspond!
                    kunde.auslausweisCode != null && kunde.auslausweisCode.Length > 0)
                {
                    object[] pars = { 
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = kunde.auslausweisCode } 
                    };
                    kundeOutput.AUSLAUSWEIS = context.ExecuteStoreQuery<String>("select trim(value) from ddlkppos where code='AUSLAUSWEIS' and id=:id", pars).FirstOrDefault();
                }
                kundeOutput.AUSLAUSWEISGUELTIG = kunde.auslausweisGueltig;
                kundeOutput.EINREISEDATUM = kunde.einreisedatum;
                kundeOutput.EMAIL = kunde.email;

                DateTime now = DateTime.Now;

                DateTime ttemp;

                if (kunde.erreichbVon > 2359)
                    kunde.erreichbVon = 0;
                if (kunde.erreichbBis > 2359)
                    kunde.erreichbBis = 0;


                if (kunde.erreichbBis > 0)
                {
                    ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbBis / 100, (int)(kunde.erreichbBis - kunde.erreichbBis / 100 * 100), 0);
                    kundeOutput.ERREICHBBIS = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);
                }
                if (kunde.erreichbVon > 0)
                {

                    ttemp = new DateTime(now.Year, now.Month, now.Day, (int)kunde.erreichbVon / 100, (int)(kunde.erreichbVon - kunde.erreichbVon / 100 * 100), 0);
                    kundeOutput.ERREICHBVON = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ttemp);
                }

                //kundeOutput.BERUF = kunde.beruf;
                kundeOutput.ERREICHBTEL = kunde.erreichbtel;
                kundeOutput.GEBDATUM = kunde.gebdatum;
                kundeOutput.GRUENDUNG = kunde.gruendung;
                kundeOutput.HANDY = kunde.handy;
                kundeOutput.HREGISTER = kunde.hregister;
                kundeOutput.HREGISTERFLAG = Convert.ToInt32(kunde.hregisterFlag);
                kundeOutput.HSNR = kunde.hsnr;
                kundeOutput.HSNR2 = kunde.hsnr2;
                kundeOutput.NAME = kunde.name;
                kundeOutput.ORT = kunde.ort;
                kundeOutput.ORT2 = kunde.ort2;
                kundeOutput.PLZ = kunde.plz;
                kundeOutput.PLZ2 = kunde.plz2;
                kundeOutput.RECHTSFORM = kunde.rechtsform;
                
                kundeOutput.RECHTSFORMCODE = kunde.rechtsformCode;
                kundeOutput.REVFLAG = Convert.ToInt32(kunde.revFlag);
                kundeOutput.STRASSE = kunde.strasse;
                kundeOutput.STRASSE2 = kunde.strasse2;
                kundeOutput.SYSBRANCHE = kunde.sysbranche;
                kundeOutput.SYSCTLANG = kunde.sysctlang;

                kundeOutput.SYSKDTYP=kunde.syskdtyp;
                kundeOutput.SYSLAND= kunde.sysland;
                kundeOutput.SYSSTAAT= kunde.sysstaat;

                
                kundeOutput.SYSLAND2 = kunde.sysland2;
                kundeOutput.SYSCTLANGKORR = kunde.sysctlangkorr;
                kundeOutput.SYSLANDNAT = kunde.syslandnat;
                kundeOutput.SYSSTAAT2 = kunde.sysstaat2;
                kundeOutput.PTELEFON = kunde.telefon;
                kundeOutput.TELEFON = kunde.telefon2;
                kundeOutput.TITEL = kunde.titel;
                kundeOutput.TITELCODE = kunde.titelCode;
                //kundeOutput.TITEL2 = kunde.titel2;
                kundeOutput.URL = kunde.url;
                kundeOutput.VORNAME = kunde.vorname;
                kundeOutput.WOHNSEIT = kunde.wohnseit;
                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.VORNAMEKONT = kunde.vornameKont;
                kundeOutput.NAMEKONT = kunde.nameKont;
                kundeOutput.ZUSATZ = kunde.zusatz;
                kundeOutput.MITARBEITERFLAG = kunde.mitarbeiterflag;
                kundeOutput.PRIVATFLAG = kunde.privatflag;

                // Ticket#2012022910000062 : neue Kont-Felder
                //kundeOutput.ANREDEKONT = kunde.anredeKont;
                kundeOutput.ANREDECODEKONT = kunde.anredeCodeKont;
                //kundeOutput.TITELKONT = kunde.titelKont;
                kundeOutput.IBAN = kunde.iban;

                kundeOutput.SCHUFAID = kunde.schufaid;
                kundeOutput.CREFOID = kunde.crefoid;
                kundeOutput.FICOID = kunde.ficoid;

                kundeOutput.WERBECODE = kunde.werbeCode;
                kundeOutput.INFOMAILFLAG = kunde.infomailflag;
                kundeOutput.INFOMAIL2FLAG = kunde.infomail2flag;
                if (kunde.handy != null && kunde.handy.Length > 0)
                    kundeOutput.INFOSMSFLAG = 1;
                else
                    kundeOutput.INFOSMSFLAG = 0;
                kundeOutput.INFOTELFLAG = kunde.infoTelFlag;

                context.SaveChanges();
                _log.Debug("updateKunde2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                IAdresseBo aBo = BOFactory.getInstance().createAdresseBo();
                IKontoBo kBo = BOFactory.getInstance().createKontoBo();
                
                List<long> kontoIds = new List<long>();
                List<long> adresseIds = new List<long>();

               
                _log.Debug("updateKunde4: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
               
                for (int i = 0; i < kunde.zusatzdaten.Length; i++)
                {
                    kunde.zusatzdaten[i].kdtyptyp = MyGetKdTypTyp(context, kunde.zusatzdaten[i].kdtyp);
                    
                }
                
            }
            IZusatzdatenBo zBo = BOFactory.getInstance().createZusatzdatenBo();
            List<ZusatzdatenDto> zusatzdaten = new List<ZusatzdatenDto>();
            for (int i = 0; i < kunde.zusatzdaten.Length; i++)
            {
                zusatzdaten.Add(zBo.createOrUpdateZusatzdatenPerson(kunde.zusatzdaten[i], kunde));
            }
            _log.Debug("updateKunde5: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            KundeDto rval = getKundeBySysKd(kundeOutput.SYSPERSON);
            _log.Debug("updateKunde6: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            //upon kunde creation/update return only the ukz/pkz given as input
            rval.zusatzdaten = zusatzdaten.ToArray();
            return rval;
        }


        /// <summary>
        /// Adresse via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        public AdresseDto[] getAdressen(long sysit)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Security Check: Aufruf nur mit long
                DbConnection con = (context.Database.Connection);
                List<AdresseDto> rval = con.Query<AdresseDto>("select itadresse.*,itadresse.sysitadresse sysadresse from itadresse where rang=2 and rownum<2 and sysit=:sysit", new { sysit = sysit }).ToList();
              
                //ITADRESSE[] adressen = context.ExecuteStoreQuery<ITADRESSE>("select * from itadresse where rang=2 and sysit=" + sysit, null).ToArray();
                _log.Debug("getAdressen: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));


               /* foreach (AdresseDto adrIn in rval)
                {
                    //AdresseDto adr = Mapper.Map<ITADRESSE, AdresseDto>(adrIn);
                    adr.sysadresse = adrIn.SYSITADRESSE;
                    //rval.Add(adr);
                }*/
                return rval.ToArray();
            }
        }

        /// <summary>
        /// Kontendaten via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        public KontoDto[] getKonten(long sysit)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                KONTO[] Konten = null;

                var query = context.KONTO.Where(par => par.PERSON != null && par.PERSON.SYSPERSON == sysit);
                if (query != null)
                    Konten = query.ToArray();

                
                return Mapper.Map<KONTO[], KontoDto[]>(Konten);
            }
        }

        /// <summary>
        /// getItPlusbySysAntrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public KundePlusDto getItPlusbySysAntrag(long? sysantrag, long? sysit)
        {
            KundePlusDto kundePlusDto = new KundePlusDto();

            if (sysit != null && sysantrag != null)
            {
                
                KundeDto kunde = getKunde((long)sysit);
                kundePlusDto = Mapper.Map<KundeDto, KundePlusDto>(kunde);

                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    List<Devart.Data.Oracle.OracleParameter> parameters1 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters1.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysit });
                    kundePlusDto.risikokl = Mapper.Map<RISIKOKL, RisikoklDto>(context.ExecuteStoreQuery<RISIKOKL>(RISIKOKLQUERY_B2B, parameters1.ToArray()).FirstOrDefault());
                    

                    var landQuery = from land in context.LAND
                                    where land.SYSLAND == kunde.syslandnat
                                    select land;
                    kundePlusDto.landNationalitaet = Mapper.Map<LAND, LandDto>(landQuery.FirstOrDefault());

                    var land2 = from land in context.LAND
                                where land.SYSLAND == kunde.sysland
                                select land;
                    kundePlusDto.landWohnsitz = Mapper.Map<LAND, LandDto>(land2.FirstOrDefault());

                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                    ITPKZ pkz = context.ExecuteStoreQuery<ITPKZ>(ITPKZQUERY, parameters2.ToArray()).FirstOrDefault();
                    kundePlusDto.pkz = Mapper.Map<ITPKZ, PkzDto>(pkz);
                    if (kundePlusDto.pkz != null && pkz.KREDRATE.HasValue)
                        kundePlusDto.pkz.kredtrate = (double?)pkz.KREDRATE.Value;

                    List<Devart.Data.Oracle.OracleParameter> parameters3 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    kundePlusDto.anzV = context.ExecuteStoreQuery<int>(ANZVITQUERY, parameters3.ToArray()).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters4 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters4.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    kundePlusDto.anzVExpress = context.ExecuteStoreQuery<int>(ANZVITEXPRESSQUERY, parameters4.ToArray()).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters5 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters5.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    kundePlusDto.vDispo = context.ExecuteStoreQuery<long>(VITDISPOQUERY, parameters5.ToArray()).ToList();
                    kundePlusDto.anzVDispo = kundePlusDto.vDispo.Count();

                    List<Devart.Data.Oracle.OracleParameter> parameters6 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters6.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                    KREMO kremo = context.ExecuteStoreQuery<KREMO>(BUDGETQUERY, parameters6.ToArray()).FirstOrDefault();

                    MyFillKremoFields(kundePlusDto, kremo);
                }
                return kundePlusDto;
            }
            else
                return null;
        }

        /// <summary>
        /// Kunde nach Antrag ID auslesen
        /// </summary>
        /// <param name="sysantrag">Antrag ID</param>
        /// <param name="syskd">Kunde ID</param>
        /// <returns>Kundendaten</returns>
        public KundePlusDto getKundebySysAntrag(long? sysantrag, long? syskd)
        {
            KundePlusDto kundePlusDto = new KundePlusDto();

            if (syskd != null && sysantrag != null)
            {
                
                KundeDto kunde = getKundeBySysKd((long)syskd);
                kundePlusDto = Mapper.Map<KundeDto, KundePlusDto>(kunde);

                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });

                    kundePlusDto.risikokl = Mapper.Map<RISIKOKL, RisikoklDto>(context.ExecuteStoreQuery<RISIKOKL>(RISIKOKLQUERY, parameters.ToArray()).FirstOrDefault());

                    

                    var landQuery = from land in context.LAND
                                    where land.SYSLAND == kunde.syslandnat
                                    select land;
                    kundePlusDto.landNationalitaet = Mapper.Map<LAND, LandDto>(landQuery.FirstOrDefault());

                    var land2 = from land in context.LAND
                                where land.SYSLAND == kunde.sysland
                                select land;
                    kundePlusDto.landWohnsitz = Mapper.Map<LAND, LandDto>(land2.FirstOrDefault());

                    

                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syskd });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                    PKZ pkzTmp = context.ExecuteStoreQuery<PKZ>(PKZQUERY, parameters2.ToArray()).FirstOrDefault();
                    kundePlusDto.pkz = Mapper.Map<PKZ, PkzDto>(pkzTmp);
                    if (pkzTmp != null && pkzTmp.KREDRATE.HasValue)
                        kundePlusDto.pkz.kredtrate = (double?)pkzTmp.KREDRATE.Value;

                    List<Devart.Data.Oracle.OracleParameter> parameters3 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syskd });
                    kundePlusDto.anzV = context.ExecuteStoreQuery<int>(ANZVQUERY, parameters3.ToArray()).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters4 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters4.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syskd });
                    kundePlusDto.anzVExpress = context.ExecuteStoreQuery<int>(ANZVEXPRESSQUERY, parameters4.ToArray()).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters5 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters5.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syskd });
                    kundePlusDto.vDispo = context.ExecuteStoreQuery<long>(VDISPOQUERY, parameters5.ToArray()).ToList();
                    kundePlusDto.anzVDispo = kundePlusDto.vDispo.Count();

                    List<Devart.Data.Oracle.OracleParameter> parameters6 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters6.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                    KREMO kremo = context.ExecuteStoreQuery<KREMO>(BUDGETQUERY, parameters6.ToArray()).FirstOrDefault();

                    MyFillKremoFields(kundePlusDto, kremo);

                    LAND korrLand = (from land in context.LAND
                                     join adresse in context.ADRESSE on land.SYSLAND equals adresse.SYSLAND
                                     where adresse.PERSON.SYSPERSON == syskd && adresse.RANG == 2
                                     select land).FirstOrDefault();
                    kundePlusDto.korrAdresse = Mapper.Map<LAND, LandDto>(korrLand);
                }
                return kundePlusDto;
            }
            else
                return null;
        }


     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysaktuellIT"></param>
        public void altePKZLeerenBysysAntrag(long sysantrag, long sysaktuellIT)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                var sysit = (from ant in context.ANTRAG
                                    where ant.SYSID == sysantrag
                                    select ant.SYSIT).FirstOrDefault();



                List<long> antobsichList = (from obsich in context.ANTOBSICH
                                       where obsich.SYSANTRAG == sysantrag && obsich.IT != null
                                       select obsich.IT.SYSIT).ToList();
                
               
                

                List<ITPKZ> pkzs = (from p in context.ITPKZ
                                    where p.SYSANTRAG == sysantrag && p.IT.SYSIT != sysaktuellIT && !antobsichList.Contains(p.IT.SYSIT) && p.IT.SYSIT!=sysit
                                    select p).ToList();


                foreach (ITPKZ itpkz in pkzs)
                {
                    itpkz.SYSANTRAG = 0;
                }

                List<ITUKZ> ukzs = (from p in context.ITUKZ
                                    where p.SYSANTRAG == sysantrag && p.IT.SYSIT != sysaktuellIT && p.IT.SYSIT != sysit && !antobsichList.Contains(p.IT.SYSIT) && p.IT.SYSIT != sysit
                                    select p).ToList();

                foreach (ITUKZ itukz in ukzs)
                {

                    itukz.SYSANTRAG = 0;

                }

                context.SaveChanges();
            }

        }

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public void transferITPKZUKZToPERSON(long syskd, long sysit, long sysantrag)
        {
            try
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>
                    {
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysit", Value = sysit},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysantrag", Value = sysantrag},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "syskd", Value = syskd}
                    };
                    List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>
                    {
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysit", Value = sysit},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysantrag", Value = sysantrag},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "syskd", Value = syskd}
                    };
                    List<Devart.Data.Oracle.OracleParameter> pars3 = new List<Devart.Data.Oracle.OracleParameter>
                    {
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysit", Value = sysit},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysantrag", Value = sysantrag},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "syskd", Value = syskd}
                    };
                    List<Devart.Data.Oracle.OracleParameter> pars4 = new List<Devart.Data.Oracle.OracleParameter>
                    {
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysit", Value = sysit},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysantrag", Value = sysantrag},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "syskd", Value = syskd}
                    };
                    List<Devart.Data.Oracle.OracleParameter> pars5 = new List<Devart.Data.Oracle.OracleParameter>
                    {
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "sysit", Value = sysit},
                        new Devart.Data.Oracle.OracleParameter {ParameterName = "syskd", Value = syskd}
                    };


                    ctx.ExecuteStoreCommand(QUERYMERGEPKZ, pars.ToArray());
                    ctx.ExecuteStoreCommand(QUERYMERGEUKZ, pars2.ToArray());
                    ctx.ExecuteStoreCommand(QUERYMERGEITPERSON, pars5.ToArray());
                    ctx.ExecuteStoreCommand(QUERYMERGEPERSONPKZ, pars3.ToArray());
                    ctx.ExecuteStoreCommand(QUERYMERGEPERSONUKZ, pars4.ToArray());

                }
            }
            catch (Exception e)
            {
                _log.Error("Mapping ITPKZ/ITUKZ to PKZ/UKZ failed for syskd=" + syskd + " sysit=" + sysit + " sysantrag=" + sysantrag, e);
            }
        }

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public void transferITPKZUKZToPKZUKZ(long syskd, long sysit, long sysantrag)
        {
            try
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    object[] parameters = { 
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag },
                          new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit } 
                    };

                    var sysKdTyp = ctx.ExecuteStoreQuery<long>(QUERYSYSKDTYP, parameters).FirstOrDefault();
                    int kdtyptyp = MyGetKdTypTyp(ctx, sysKdTyp);

                    switch (kdtyptyp)
                    {
                        case 1:
                        case 2:
                        default:

                            if (!ctx.PKZ.Any(a => a.SYSANTRAG == sysantrag && a.PERSON.SYSPERSON == syskd))
                            {
                                var pkz = new PKZ
                                {
                                    SYSPERSON = syskd,
                                    SYSANTRAG = sysantrag,
                                };
                                ctx.PKZ.Add(pkz);
                                ctx.SaveChanges();
                            }


                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskd", Value = syskd });
                            ctx.ExecuteStoreCommand(QUERYMERGEPKZ, pars.ToArray());
                            break;

                        case 3:
                            if (!ctx.UKZ.Any(a => a.SYSANTRAG == sysantrag && a.PERSON.SYSPERSON == syskd))
                            {
                                var ukz = new UKZ
                                {
                                    SYSPERSON =syskd,
                                    SYSANTRAG = sysantrag
                                };
                                ctx.UKZ.Add(ukz);
                                ctx.SaveChanges();
                            }

                            List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>();
                            pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                            pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                            pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskd", Value = syskd });

                            ctx.ExecuteStoreCommand(QUERYMERGEUKZ, pars2.ToArray());
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Mapping ITPKZ/ITUKZ to PKZ/UKZ failed for syskd=" + syskd + " sysit=" + sysit + " sysantrag=" + sysantrag, e);
            }
        }

        /// <summary>
        /// aktuellsten Mitantragsteller laden
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public long getMitantragsteller(long sysit)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                return ctx.ExecuteStoreQuery<long>(QUERYMITANTRAGSTELLER, pars.ToArray()).FirstOrDefault();
            }
        }
        #region Private Methods

        private String MyGetKdTypBezeichnung(DdOlExtended context, long syskdtyp)
        {
            if (!kdtypName.ContainsKey(syskdtyp))
            {
                // Security Check: Aufruf nur mit long
                kdtypName[syskdtyp] = context.ExecuteStoreQuery<String>("select name from kdtyp where syskdtyp=" + syskdtyp, null).FirstOrDefault();
            }
            return kdtypName[syskdtyp];
        }
        private int MyGetKdTypTyp(DdOlExtended context, long syskdtyp)
        {
            if (!kdtypTyp.ContainsKey(syskdtyp))
            {
                // Security Check: Aufruf nur mit long
                kdtypTyp[syskdtyp] = context.ExecuteStoreQuery<int>("select typ from kdtyp where syskdtyp=" + syskdtyp, null).FirstOrDefault();
            }
            return kdtypTyp[syskdtyp];
        }

        private void MyFillKremoFields(KundePlusDto kundePlusDto, KREMO kremo)
        {
            if (kundePlusDto != null)
            {
                if (kremo != null)
                {
                    kundePlusDto.budget = (double?)kremo.BUDGET1;
                    if (kremo.SALDO > 0)
                    {
                        kundePlusDto.budget = (double?)kremo.SALDO;
                    }
                    kundePlusDto.kredinkkg = (double?)kremo.KREDINKKG;
                    kundePlusDto.kredinkkgMax = (double?)kremo.KREDINKKGMAX;
                    kundePlusDto.kredoutkkg = (double?)kremo.KREDOUTKKG;

                    // Ticket#2012042310000088 : CR 23675 - Korrektur Budgetberechnung:
                    kundePlusDto.rateBerechNeu = (double?)kremo.RATEBERECHNEU;
                    kundePlusDto.rateBerechNeu36 = (double?)kremo.RATEBERECHNEU36;
                }
                else
                {
                    kundePlusDto.budget = null;
                    kundePlusDto.kredinkkg = null;
                    kundePlusDto.kredinkkgMax = null;
                    kundePlusDto.kredoutkkg = null;

                    // Ticket#2012042310000088 : CR 23675 - Korrektur Budgetberechnung:
                    kundePlusDto.rateBerechNeu = null;
                    kundePlusDto.rateBerechNeu36 = null;
                }
            }
        }

        public KundeExternDto[] mapSearchResultIdentifyAdress(CrifSoapService.AddressMatchResult source) {
            if (source.addressMatchResultType == CrifSoapService.AddressMatchResultType.CANDIDATES)
            {
                if (source.candidates != null)
                {
                    KundeExternDto[] rval = new KundeExternDto[source.candidates.Length];
                    int i = 0;
                    foreach (CrifSoapService.Candidate k in source.candidates)
                    {
                        //rval[i] = MyMappCandidateTOKundeExternDto(k);
                        i++;
                    }
                    return rval;
                }
            }

            if (source.addressMatchResultType == CrifSoapService.AddressMatchResultType.MATCH)
            {
                KundeExternDto[] rval = new KundeExternDto[1];
                //rval[1] = MyMappFoundAdresseTOKundeExternDto(source.foundAddress);
                return rval;
            }

            if (source.addressMatchResultType == CrifSoapService.AddressMatchResultType.NO_MATCH)
            {
                KundeExternDto[] rval = null;
                return rval;
            }
            return null;

        }


        #endregion Private Methods
    }
}