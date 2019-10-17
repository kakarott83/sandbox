namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// DecisionEngineGuardeanSqlStatements
    /// select 'public const string ' || UPPER(code) || ' = ' || CHR(10) || '@"' || TRIM(paramfile) || '";' from eaipar;
    /// HCER1
    /// </summary>
    internal class DEGSql
    {

        public const string APPLICANTAPPLICATIONS =
@"select nvl(sum(bginternbrutto),0) AS applicationContingencies from antkalk, antrag where antkalk.sysantrag=antrag.sysid and antrag.sysid in ( select sysid from antrag where zustand <> 'Abgeschlossen' and syskd in  ( :pSysKd))";

        public const string APPLICANTCREDITLINES =
@"SELECT
  /*+ index(nkk nkk_rvt) index(rvt rvt_pk_id) */
  klinie.limitextern AS credit
  ,CASE WHEN cic.cic_sys.to_cladate(klinie.beginn) = 0 THEN NULL else klinie.beginn END  AS BEGIN
  ,CASE WHEN cic.cic_sys.to_cladate(klinie.ende) = 0 THEN NULL else klinie.ende END  AS END
  ,klinie.bezeichnung  AS name
  ,klinie.gesperrt     AS locked
  ,rvt.rahmen          AS rvtnumber
  ,klinart.sysklinart  AS kind
  ,nvl(SUM(NKKSALDO.SALDO),0) AS creditused
FROM hdklinie,
  klinie,
  rvt,
  nkk,
  prproduct,
  klinart,
  (SELECT
    CASE
      WHEN trim(nkk.zustand) = 'aktiv'
      THEN AUSZAHLUNG
      ELSE RESTSCHULD
    END saldo,
    sysnkk
  FROM nkk
  WHERE trim(NKK.ZUSTAND) IN ('gebucht','aktiv','abgerechnet')
  ) nkksaldo
WHERE hdklinie.sysrvt  = rvt.sysrvt
AND rvt.sysrvt         = nkk.sysrvt(+)
AND nkk.sysnkk         = nkksaldo.sysnkk(+)
AND nkk.sysprproduct   = prproduct.sysprproduct(+)
AND hdklinie.sysklinie = klinie.sysklinie
AND klinie.sysklinart  = nkk.sysklinie(+)
AND klinart.sysklinart(+) = klinie.sysklinart
AND hdklinie.syshd     = (SELECT max(sysperson)
FROM perole
WHERE sysroletype in (2)
  CONNECT BY PRIOR perole.sysparent = perole.sysperole
  START WITH SYSPERSON              = :pSysKD)
--AND klinie.beginn                                                                    <= TRUNC(sysdate)
AND (klinie.ende                                                                     IS NULL
OR (DECODE(cic.cic_sys.to_cladate(klinie.ende), 0, TRUNC(sysdate,'DD'), klinie.ende) >= TRUNC(sysdate)))
GROUP BY klinie.limitextern ,
  klinie.beginn ,
  klinie.ende ,
  klinie.bezeichnung ,
  klinie.gesperrt ,
  rvt.rahmen ,
  klinart.sysklinart,
  klinie.sysklinie";

        public const string APPLICANTCONTRACTS =
        @"with
      ANT as
        (select antrag, syskd, sysid from antrag where syskd = :pSysKd)
    , Tage As
        (select trunc (to_date('01.01.1990','dd.mm.yyyy'),'Month') +(rownum - 1) As Datum from dual
        connect by trunc (to_date('01.01.1990','dd.mm.yyyy'),'month') + (rownum - 1) <= last_day(sysdate))
    , Monate As
        (select 1, trunc(datum,'month') as Monat from Tage group by  trunc(datum,'month') order by  trunc(datum,'month'))
    , VorVerträge as
        (select vt.syskd, vt.sysid, vt.beginn, 1, endeam
                from vt, ANT
                where vt.syskd in ( :pSysKd )
                and vt.beginn > to_date('01.01.1990','DD.MM.YY')
                order by sysid)
    , Aktiv as
        (select Monate.Monat
        , (case when VorVerträge.beginn <= Monate.monat and decode(endeam,to_date('01.01.0111','DD.MM.YY'),sysdate,endeam) >= Monate.Monat Then 1 else 0 end) VT_aktiv
        from Monate, VorVerträge
        where 1 = 1
        order by monat desc)
    , EffBez AS
        (select Aktiv.Monat, max(Aktiv.VT_aktiv) laufend from Aktiv group by Aktiv.Monat order by Monat)
    , KdBez AS
        (select Aktiv.Monat, sum(Aktiv.VT_aktiv) laufend from Aktiv group by Aktiv.Monat order by Monat)
    , Mahn AS
        ( SELECT case when sum(vtmahn.MZAEHLER3) > 0 then '3'  when sum(vtmahn.MZAEHLER3) = 0 and sum(vtmahn.MZAEHLER2) > 0 then '2'  when sum(vtmahn.MZAEHLER3) = 0 and sum(vtmahn.MZAEHLER2) = 0 and sum(vtmahn.MZAEHLER1) > 0 then '1'  else '0' end AS contractMaxiDunLevel  FROM vt, vtmahn WHERE vtmahn.SYSVTMAHN = vt.SYSID and vt.syskd in (:pSysKd))
select sum(EffBez.laufend) AS contractEffekCustomRelation, sum(KdBez.laufend) AS contractDurCustomRelation, max(contractMaxiDunLevel) AS contractMaxiDunLevel from EffBez, KdBez, MAHN where EffBez.Monat = KdBez.Monat";

        public const string APPLICANTDATA =
        @"select
   nvl(pkz.zeinknetto,0) as lastAdditionalIncome
  ,nvl(pkz.JBONUSNETTO,0) as lastBonus
  ,nvl(pkz.familienstand,0) as lastCivilStatus
  ,nvl(pkz.BERUFLICHCODE,0) as lastEmployment
  ,nvl(pkz.NEBEINKNETTO,0) as lastExtraIncome
  ,nvl(pkz.EINKNETTO,0) as lastIncome
  ,trim(land.iso) as lastNationality
  ,nvl(pkz.MIETE,0) as lastRent
  ,nvl(null,0) as lastRiskClass
  ,nvl(pkz.wohnart,0) as lastTypeOfLiving
  ,(select nvl(max(sysrisikoklneu),0) from rklhist where sysperson = pkz.sysperson) as maxCurrentRiskClass
  ,(select nvl(sysrisikoklneu,0) from rklhist where sysrklhist = (select max(sysrklhist) from rklhist where sysperson = pkz.sysperson)) as maxRiskClass
  ,case when (select Max(case when rating.status = 3 then 0 else 1 end) from antrag,rating where rating.sysid=antrag.sysid and antrag.syskd = :pSysKd)=1 then 1 else 0 end as manuCreditDecisHist
from pkz,person,land
where syspkz = (
select max(pkz.syspkz) from pkz
where pkz.sysperson = :pSysKd)
and pkz.sysperson = person.sysperson
and land.sysland = person.syslandnat";

        public const string APPLICANTEMPLOYMENT =
        @"select
 ITPKZ.NAMEAG as employerName
,CASE WHEN itpkz.syskdtyp = 2 THEN ITPKZ.GRUENDUNG ELSE ITPKZ.BESCHSEITAG END as employmentStartDate
,ITPKZ.BESCHBISAG as employmentEndDate
,ITPKZ.STRASSEAG as employerStreet
,ITPKZ.HSNRAG as employerHousenumber
,ITPKZ.PLZAG as employerPostalCode
,ITPKZ.ORTAG as employerCity
,land.iso as employerCountry
,ITPKZ.BERUF as employmentType
,CASE WHEN itpkz.syskdtyp = 2 then 1 else 0 END as isSelfEmployed
from itpkz, antrag, land
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and itpkz.syslandag = land.sysland (+)
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTEMPLOYMENT2 =
        @"select
 ITPKZ.NAMEAG as employerName
,CASE WHEN ITPKZ.syskdtyp = 2 THEN ITPKZ.GRUENDUNG ELSE ITPKZ.BESCHSEITAG END as employmentStartDate
,ITPKZ.BESCHBISAG as employmentEndDate
,ITPKZ.STRASSEAG as employerStreet
,ITPKZ.HSNRAG as employerHousenumber
,ITPKZ.NAMEAG as employerName
,CASE WHEN ITPKZ.syskdtyp = 2 THEN ITPKZ.GRUENDUNG ELSE ITPKZ.BESCHSEITAG END as employmentStartDate
,ITPKZ.BESCHBISAG as employmentEndDate
,ITPKZ.STRASSEAG as employerStreet
,ITPKZ.HSNRAG as employerHousenumber
,ITPKZ.PLZAG as employerPostalCode
,ITPKZ.ORTAG as employerCity
,land.iso as employerCountry
,ITPKZ.BERUF as employmentType
,CASE WHEN ITPKZ.syskdtyp = 2 then 1 else 0 END as isSelfEmployed
from
  antrag,
  antobsich,
  land,
  sichtyp,
  itpkz
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and ITPKZ.sysit = antobsich.sysit
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and land.sysland = itpkz.syslandag (+)
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTFINANCIALSEXPEN =
        @"select
  itpkz.KREDRATE1 as expenseAmount
  ,'AE' as expenseType
from
  itpkz,
  antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = :pANTRAGID
union
select
  itpkz.MIETE as expenseAmount
  ,'MR' as expenseType
from
  itpkz,
  antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and itpkz.sysit = antrag.sysit
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTFINANCIALSEXPEN2 =
        @"select
  itpkz.KREDRATE1 as expenseAmount
  ,'AE' as expenseType
from
  itpkz,
  antrag,
  antobsich,
  sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and itpkz.sysit = antobsich.sysit
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID
union
select
  itpkz.MIETE as expenseAmount
  ,'MR' as expenseType
from
  itpkz,
  antrag,
  antobsich,
  sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTFINANCIALSINC =
        @"select                                        
itpkz.EINKNETTO as netIncome
,'HE' as incomeType
from itpkz, antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = :pANTRAGID
union
select
itpkz.NEBEINKNETTO as netIncome
,'NE' as incomeType
from itpkz, antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = :pANTRAGID
union
select
itpkz.ZEINKNETTO as netIncome
,'KE' as incomeType
from itpkz, antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTFINANCIALSINC2 =
        @"select
  itpkz.EINKNETTO as netIncome
  ,'HE' as incomeType
from
  itpkz,
  antrag,
  antobsich,
  sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID
union
select
  itpkz.NEBEINKNETTO as netIncome
  ,'NE' as incomeType
from
  itpkz,
  antrag,
  antobsich,
  sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID
union
select
  itpkz.ZEINKNETTO as netIncome
  ,'KE' as incomeType
from
  itpkz,
  antrag,
  antobsich,
  sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTIDENTITY =
        @"select
   ITPKZ.AHBEWILLIGUNGBIS as residencePermitionUntil
  ,ITPKZ.AUSWEISART as identityType
  ,ITPKZ.AUSWEISNR as identityNumber
  ,ITPKZ.AUSWEISBEHOERDE as issuingAuthority
  ,ITPKZ.AUSWEISDATUM as documentIssueDate
  ,ITPKZ.AUSWEISABLAUF as documentValidTill
  ,ITPKZ.legitdatum as validatedAt
  ,ITPKZ.legitabnehmer as validatedFrom
  ,case when ITPKZ.Legitdatum is null then 0 else 1 end as isVerified
  ,case when ITPKZ.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencePermition
  ,case when ITPKZ.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencyStatus
from itpkz, antrag
where itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = :pANTRAGID";

        public const string APPLICANTIDENTITY2 =
        @"select
   ITPKZ.AHBEWILLIGUNGBIS as residencePermitionUntil
  ,ITPKZ.AUSWEISART as identityType
  ,ITPKZ.AUSWEISNR as identityNumber
  ,ITPKZ.AUSWEISBEHOERDE as issuingAuthority
  ,ITPKZ.AUSWEISDATUM as documentIssueDate
  ,ITPKZ.AUSWEISABLAUF as documentValidTill
  ,ITPKZ.legitdatum as validatedAt
  ,ITPKZ.legitabnehmer as validatedFrom
  ,case when ITPKZ.Legitdatum is null then 0 else 1 end as isVerified
  ,case when ITPKZ.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencePermition
  ,case when ITPKZ.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencyStatus
from itpkz, antrag, antobsich, sichtyp
where itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and antrag.sysid = :pANTRAGID";

        public const string APPLICATION =
        @"SELECT
     CASE
         WHEN instr(antrag,'.') > '0' THEN nvl(concat(substr(antrag,1,instr(antrag,'.') - 1),CASE
             WHEN instr(antrag,'_') > '0' THEN substr(antrag,instr(antrag,'_'),length(antrag) )
             ELSE ''
         END),antrag)
         ELSE antrag
     END AS applicationid,
     antrag.erfassung
     || ' '
     || TO_CHAR(cic.mdbs_tooratime(erfassungzeit,erfassung),'hh24:mi:ss') AS applicationdate,
     antrag.sysprchannel    AS applicationtype,
     auskunft.sysauskunft   AS externalreference,
     CASE
         WHEN instr(antrag,'.') > '0' THEN regexp_substr(substr(antrag,instr(antrag,'.') ),'[[:digit:]]')
         ELSE '1'
     END AS applicationversion
FROM
     antrag,
     auskunft
WHERE
     auskunft.sysid = antrag.sysid
     AND auskunft.statusnum = 0
     AND auskunft.sysauskunfttyp = 101
     AND antrag.sysid =:pANTRAGID";

        public const string BANKACCOUNTPRIVATE =
        @"select
  1 as paymentMethod
  ,kz.name as bankAccountOwner
  ,kz.BANKNAME as bankName
  ,kz.bic as BIC
  ,kz.iban AS IBAN
  ,nvl(to_num(SUBSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),1,INSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),'.',1)-1)),1) as directDebitDay
from
  itpkz kz,
  antrag,
  antkalk,
  (select syski from angebot where sysantrag = :pANTRAGID) ki
where kz.sysit = ki.syski
and kz.sysantrag = antrag.sysid
AND antrag.sysit = ki.syski
AND antrag.sysid = antkalk.sysantrag
AND antrag.sysid = :pANTRAGID";

        public const string BANKACCOUNTPRIVATE2 =
            @"select
  1 as paymentMethod
  ,kz.name as bankAccountOwner
  ,kz.BANKNAME as bankName
  ,kz.bic as BIC
  ,kz.iban AS IBAN
  ,nvl(to_num(SUBSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),1,INSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),'.',1)-1)),1) as directDebitDay
from
  itpkz kz,
  antrag,
  antkalk,
  (select syski from angebot where sysantrag = :pANTRAGID) ki,
  antobsich
where kz.sysit = ki.syski
and kz.sysantrag = antrag.sysid
AND antrag.sysid = antkalk.sysantrag
AND antobsich.sysit = ki.syski
AND antobsich.syssichtyp in (265,232,229)
AND antrag.sysid = :pANTRAGID  ";

        public const string BANKACCOUNTCOMP =
        @"select
  1 as paymentMethod
  ,kz.name as bankAccountOwner
  ,kz.BANKNAME as bankName
  ,kz.bic as BIC
  ,kz.iban AS IBAN
  ,nvl(to_num(SUBSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),1,INSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),'.',1)-1)),1) as directDebitDay
from
  itukz kz,
  antrag,
  antkalk,
  (select syski from angebot where sysantrag = :pANTRAGID) ki
where kz.sysit = ki.syski
and kz.sysantrag = antrag.sysid
AND antrag.sysit = ki.syski
AND antrag.sysid = antkalk.sysantrag
AND antrag.sysid = :pANTRAGID";


        public const string BANKACCOUNTCOMP2 = 
            @"select
  1 as paymentMethod
  ,kz.name as bankAccountOwner
  ,kz.BANKNAME as bankName
  ,kz.bic as BIC
  ,kz.iban AS IBAN
  ,nvl(to_num(SUBSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),1,INSTR(to_char(antkalk.ERSTERATE,'dd.MM.yyyy'),'.',1)-1)),1) as directDebitDay
from
  itukz kz,
  antrag,
  antkalk,
  (select syski from angebot where sysantrag = :pANTRAGID) ki,
  antobsich
where kz.sysit = ki.syski
and kz.sysantrag = antrag.sysid
AND antrag.sysid = antkalk.sysantrag
AND antobsich.sysit = ki.syski
AND antobsich.syssichtyp in (265,232,229)
AND antrag.sysid = :pANTRAGID      ";

        public const string BUSINESSINFORMATION =
@"SELECT TO_CHAR(createdate,'dd.MM.yyyy')
  ||' '
  ||TO_CHAR(cic.MDBS_TOORATIME(WFMMEMO.CREATETIME,WFMMEMO.CREATEDATE),'hh24:mi:ss') AS statusUpdateDateTime --Datum und Uhrzeit der Statusänderung
  ,
  DDLKPPOS.VALUE
  ||' : '
  ||TRIM(WFMMEMO.NOTIZMEMO) AS statusUpdateDescription --Kommentarfeld
  ,
  WFUSER.CODE AS statusUpdateEditor --USER CODE
  ,
  WFMMEMO.STR02 AS statusUpdateSystem --VLM Name
  ,
  NULL AS statusUpdateText --nicht relevant
FROM WFMMEMO,
  WFUSER,
  DDLKPPOS
WHERE WFMMEMO.CREATEUSER = WFUSER.SYSWFUSER
AND WFMMEMO.STR03        = DDLKPPOS.ID(+)
AND DDLKPPOS.CODE = 'ANTRAGSZUSTAND'
AND WFMMEMO.SYSWFMMEMO   =
  (SELECT MAX(syswfmmemo)
  FROM wfmmemo
  WHERE syslease = :pANTRAGID
  AND str01     = 'CHGSTATE'
  ) 
";

        public const string TECHNICALINFORMATION =
@"SELECT 
nvl(substr(ANTRAG.ANTRAG,1,instr(ANTRAG.ANTRAG,'.')-1),ANTRAG.ANTRAG) as applicationID --Antragsnummer
,AUSKUNFT.SYSAUSKUNFT AS externalReference --SYSAUSKUNFT aus der INT1 
,DDLKPPOS.ID AS status --Code
FROM AUSKUNFT,
  ANTRAG,
  DDLKPPOS
WHERE AUSKUNFT.SYSID      = ANTRAG.SYSID
AND trim(ANTRAG.ATTRIBUT) = DDLKPPOS.VALUE
AND DDLKPPOS.CODE = 'GUARDEAN_STATUS'
AND ANTRAG.SYSID = :pANTRAGID
AND AUSKUNFT.SYSAUSKUNFT = (SELECT MAX(SYSAUSKUNFT) FROM AUSKUNFT WHERE AUSKUNFT.SYSAUSKUNFTTYP = 101 AND AUSKUNFT.SYSID = ANTRAG.SYSID)
";

        public const string COLLATERALINFO =
        @"select
vtobsich.STATUSDAT as collateralDateOfValue,
vtobsich.SICHERHEITENWERT as collateralEffectiveValue,
vtobsich.wert as collateralNominalValue,
CASE WHEN sichtyp.BEZEICHNUNG=dedefcon.name THEN DEDEFCON.EXTERNCODE ELSE '' END AS collateralName,
vtobsich.BEMERKUNG as collateralAdditionalText,
case when vtobsich.sysvt > 0 then 1 else 0 end as collateralContractFlag,
vt.vertrag AS collateralContractId,
case when vtobsich.sysmh > 0 then 1 else 0 end as collateralCustomerFlag,
vt.syskd as collateralCustomerId,
vtobsich.status as collateralStatus,
vtobsich.ende as collateralTimeLimit
from vt, ob, vtobsich, sichtyp, dedefcon
where ob.sysvt = vt.sysid and vtobsich.AKTIVFLAG = 1 and vtobsich.SYSOB = ob.sysob and vtobsich.SYSSICHTYP = sichtyp.SYSSICHTYP and vt.syskd = :pSysKd
and vt.AKTIVKZ = 1
union
select
vtobsich.STATUSDAT as collateralDateOfValue,
vtobsich.SICHERHEITENWERT as collateralEffectiveValue,
vtobsich.wert as collateralNominalValue,
CASE WHEN sichtyp.BEZEICHNUNG=dedefcon.name THEN DEDEFCON.EXTERNCODE ELSE '' END AS collateralName,
vtobsich.BEMERKUNG as collateralAdditionalText,
case when vtobsich.sysvt > 0 then 1 else 0 end as collateralContractFlag,
vt.vertrag AS collateralContractId,
case when vtobsich.sysmh > 0 then 1 else 0 end as collateralCustomerFlag,
vt.syskd as collateralCustomerId,
vtobsich.status as collateralStatus,
vtobsich.ende as collateralTimeLimit
from vt, vtobsich, sichtyp,dedefcon 
where sichtyp.BEZEICHNUNG=dedefcon.name 
and vtobsich.SYSVT = vt.sysid 
and vtobsich.SYSSICHTYP = sichtyp.SYSSICHTYP 
and vtobsich.AKTIVFLAG = 1
and vt.AKTIVKZ = 1
and vt.syskd = :pSysKd
union
select
vtobsich.STATUSDAT as collateralDateOfValue,
vtobsich.SICHERHEITENWERT as collateralEffectiveValue,
vtobsich.wert as collateralNominalValue,
CASE WHEN sichtyp.BEZEICHNUNG=dedefcon.name THEN DEDEFCON.EXTERNCODE ELSE '' END AS collateralName,
vtobsich.BEMERKUNG as collateralAdditionalText,
case when vtobsich.sysvt > 0 then 1 else 0 end as collateralContractFlag,
vt.vertrag AS collateralContractId,
case when vtobsich.sysmh > 0 then 1 else 0 end as collateralCustomerFlag,
vt.syskd as collateralCustomerId,
vtobsich.status as collateralStatus,
vtobsich.ende as collateralTimeLimit
from vtobsich, sichtyp, vt, dedefcon
where sichtyp.BEZEICHNUNG=dedefcon.name 
and vtobsich.SYSVT = vt.sysid 
and vtobsich.AKTIVFLAG = 1 
and vt.AKTIVKZ = 1
and vtobsich.SYSSICHTYP = sichtyp.SYSSICHTYP 
and vtobsich.sysmh = :pSysKd";

        public const string COLLATERALVALUE =
        @"select vt.sysid,vtobsich.STATUSDAT as collateralDateOfValue, vtobsich.SICHERHEITENWERT as collateralEffectiveValue, vtobsich.wert as collateralNominalValue from vt, ob, vtobsich, sichtyp where ob.sysvt = vt.sysid and vtobsich.SYSOB = ob.sysob and vtobsich.AKTIVFLAG = 1 and vtobsich.rang = sichtyp.rang and  vt.syskd = :pSysKd
union
select vt.sysid,vtobsich.STATUSDAT as collateralDateOfValue, vtobsich.SICHERHEITENWERT as collateralEffectiveValue, vtobsich.wert as collateralNominalValue from vt, vtobsich, sichtyp where vtobsich.SYSVT = vt.sysid and vtobsich.AKTIVFLAG = 1 and vtobsich.rang = sichtyp.rang and  vt.syskd = :pSysKd";


        public const string COMPAPPLILIST = @"SELECT 
  DECODE( itlist.applicantrole, 'GESELLS','Participants', 'KOMMAN','LimitedPartner', 'KOMPL','Complementary', 'WB','UBO', 'AKT','Shareholder', 'INH','Owner', 'VEREINVORST','AssociationCEO', 'STIFTUNGSVORST','FoundationCEO', 'PARTNER','Partner', 'GESCHF','CEO', 'VERTRETUNGSBERECHTIGTE','authorizedRepresentative' ) applicantRole,
  0 as amountForeclosure,
  it.sysit AS applicantID,
  it.IDENTEG AS taxId,
  it.IDENTUST AS businessIdentificationNumber,
  decode(it.sysKDTYP,1,0,2,2,3,1) as applicantType,
  null as bankEnquiryStatus,
  null as customerId,
  (select nvl(trunc(koopvon),to_date('01.01.1800','dd.mm.yyyy')) from person where it.sysperson = person.sysperson) as customerSince,
  0 as foreclosure,
  (select languagename from ctlang c where c.sysctlang = decode(it.sysctlang,0,1,it.sysctlang)) as language,
  null as securityType,
  null as state
FROM
  it,
  (SELECT it.sysit,
    upper(sichtyp.bezeichnung) AS applicantRole
  FROM antrag,
    antobsich,
    sichtyp,
    itpkz kz,
    land,
    it
  WHERE antobsich.sysantrag = antrag.sysid
  AND antobsich.syssichtyp  = sichtyp.syssichtyp
  AND kz.sysland            = land.SYSLAND (+)
  AND it.sysit              = kz.sysit
  AND kz.sysantrag          = antrag.sysid
  AND antobsich.rang       IN (101,102)
  AND kz.sysit              = antobsich.sysit
  AND antrag.sysid          = :pANTRAGID
  UNION
  SELECT itkne.sysober,
    relatetypecode AS applicantRole
  FROM itkne,
    angebot
  WHERE itkne.area      = 'ANGEBOT'
  AND itkne.sysarea     = angebot.sysid
  AND angebot.sysantrag = :pANTRAGID
  ) itlist
  where it.sysit = itlist.sysit";


        public const string COMPAPPLIADDRESS =
        @"SELECT kz.WOHNUNGART AS typeOfLiving,
  kz.STRASSE         AS street ,
  kz.HSNR            AS housenumber ,
  kz.PLZ             AS postalCode ,
  kz.ORT             AS city ,
  land.ISO           AS country ,
  kz.wohnseit        AS addressSince ,
  '1'                AS adresstype
FROM it kz,
  land
WHERE kz.sysland = land.SYSLAND (+)
AND kz.sysit     = :pSYSIT
UNION
SELECT NULL,
  kz.STRASSE2 AS street ,
  kz.HSNR2    AS housenumber ,
  kz.PLZ2     AS postalCode ,
  kz.ORT2     AS city ,
  CASE
    WHEN kz.STRASSE2 IS NOT NULL
    OR kz.HSNR2      IS NOT NULL
    OR kz.PLZ2       IS NOT NULL
    OR kz.ORT2       IS NOT NULL
    THEN land.iso
  END AS country ,
  NULL ,
  CASE
    WHEN kz.STRASSE2 IS NOT NULL
    OR kz.HSNR2      IS NOT NULL
    OR kz.PLZ2       IS NOT NULL
    OR kz.ORT2       IS NOT NULL
    THEN '2'
  END AS adresstype
FROM it kz ,
  land
WHERE kz.sysit    = :pSYSIT
AND kz.sysland    = land.SYSLAND (+)
AND (kz.STRASSE2 IS NOT NULL
OR kz.HSNR2      IS NOT NULL
OR kz.PLZ2       IS NOT NULL
OR kz.ORT2       IS NOT NULL)";


        public const string COMPAPPLIBANKACCINFO =
        @"SELECT 1 from dual where 1=0";


        public const string COMPAPPLICONTACT =
        @"select
   d.TELEFON as phoneBusiness
  ,d.PTELEFON as phonePrivate
  ,d.HANDY as mobilePrivate
  ,d.FAX as faxPrivate
  ,d.EMAIL as emailPrivate
FROM     
    it d
  WHERE 
  d.sysit = :pSYSIT";


        public const string COMPAPPLIPRIVATE =
        @"select
   null as  birthName
  ,null as countryOfBirth
  ,case when kz.BESCHSEITAG is not null then months_between(sysdate,kz.BESCHSEITAG) else 0 end as curEmploymentMonth
  ,null as educationType
  ,CASE WHEN kz.beruf = 60 then 0 else 1 end as employee
  ,case when kz.ABBEWILLIGUNG is not null then months_between(sysdate,kz.ABBEWILLIGUNG) else 0 end as residencyMonthsSince
  ,kz.ABBEWILLIGUNG as residencySince
  ,case when kz.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencyStatus
  ,null as schufaID
  ,kz.Familienstand as maritalStatus
  ,kz.KINDERIMHAUS as numberOfChildUndEighteen
  ,decode(kz.anredecode,'1','Frau','2','Mann','3','Firma','Herr','Herr','Frau','Frau','Firma','Firma')  as salutation
  ,kz.TITELCODE as title
  ,kz.VORNAME as firstName
  ,kz.NAME as lastName
  ,kz.GEBDATUM as dateOfBirth
  ,kz.GEBORT as placeOfBirth
  ,land.iso as citizenship
  ,nvl(to_number(kz.WOHNVERH),0) as jointHousehold
  ,DECODE(kz.anredecode,'1','F','2','M','3',NULL,'Herr','M','Frau','F','Firma',NULL) AS gender
  ,DECODE(kz.schufaflag,1,'True','False') as schufaRequired
FROM it kz
  ,land
WHERE kz.sysit                       = :pSYSIT
and land.sysland (+) = kz.syslandnat
";


        public const string COMPAPPLIPRIVATEEMPL =@"SELECT 1 from dual where 1=0";


        public const string COMPAPPLIPRIVATEFINEXP = @"SELECT 1 from dual where 1=0";


        public const string COMPAPPLIPRIVATEFININC =@"SELECT 1 from dual where 1=0";

        public const string COMPAPPLIPRIVATEIDENTIT =
        @"select
   kz.AHBEWILLIGUNGBIS as residencePermitionUntil
  ,kz.AUSWEISART as identityType
  ,kz.AUSWEISNR as identityNumber
  ,kz.AUSWEISBEHOERDE as issuingAuthority
  ,kz.AUSWEISDATUM as documentIssueDate
  ,kz.AUSWEISABLAUF as documentValidTill
  ,kz.legitdatum as validatedAt
  ,kz.legitabnehmer as validatedFrom
  ,case when kz.Legitdatum is null then 0 else 1 end as isVerified
  ,case when kz.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencePermition
  ,case when kz.AHBEWILLIGUNGBIS is null then 1 else 0 end as residencyStatus
FROM 
    it kz
WHERE
  kz.sysit = :pSYSIT";


        public const string COMPPEPINFORMATION = @"SELECT 
  COMPLIANCE.BEZEICHNUNG as employmentTitle,
  compliance.beginn as since,
  compliance.ende as till,
  land.ISO as country
FROM 
  land,
  itpkz,
  COMPLIANCE
WHERE COMPLIANCE.area    = 'ITPKZ'
AND COMPLIANCE.flagaktiv = 1
AND itpkz.sysitpkz       = COMPLIANCE.sysid
AND compliance.sysland   = land.sysland (+)
AND itpkz.sysit = :pSYSIT
AND itpkz.SYSANTRAG = :pANTRAGID
";
        public const string COMPENSATION =
        @"select
   0 as numberOfAbloesen
  ,0 as numberOfEigenabloesen
  ,0 as sumAbloesen
  ,0 as sumEigenabloesen
from it, antrag
where it.sysit = antrag.sysit
and antrag.sysid = :pANTRAGID";

        public const string CONTACTPRIVATE =
        @"select
  kz.name as contactPerson
  ,kz.TELEFON as phoneBusiness
  ,kz.PTELEFON as phonePrivate
  ,kz.HANDY as mobilePrivate
  ,kz.FAX as faxPrivate
  ,kz.EMAIL as emailPrivate
  ,kz.url as website
from antrag, itpkz kz
where
antrag.sysid = kz.sysantrag
and antrag.sysit = kz.sysit
and antrag.sysid = :pANTRAGID";

        public const string CONTACTCOMP =
        @"select
kz.name as contactPerson
,kz.TELEFON as phoneBusiness
,kz.PTELEFON as phonePrivate
,kz.HANDY as mobilePrivate
,kz.FAX as faxPrivate
,kz.EMAIL as emailPrivate
,kz.url as website
from antrag, itukz kz
where 
antrag.sysid = kz.sysantrag
and antrag.sysit = kz.sysit
and antrag.sysid = :pANTRAGID";

        public const string CONTACTPRIVATE2 =
        @"select
kz.name as contactPerson
,kz.TELEFON as phoneBusiness
,kz.PTELEFON as phonePrivate
,kz.HANDY as mobilePrivate
,kz.FAX as faxPrivate
,kz.EMAIL as emailPrivate
,kz.url as website
from antrag, itpkz kz,antobsich ao,sichtyp si
where 
antrag.sysid = kz.sysantrag
and ao.sysantrag = antrag.sysid
and ao.sysit = kz.sysit
and ao.syssichtyp = si.syssichtyp
and si.rang in (10,80,110)
and antrag.sysid = :pANTRAGID";

        public const string CONTACTCOMP2 =
        @"select
  kz.name as contactPerson
  ,kz.TELEFON as phoneBusiness
  ,kz.PTELEFON as phonePrivate
  ,kz.HANDY as mobilePrivate
  ,kz.FAX as faxPrivate
  ,kz.EMAIL as emailPrivate
  ,null as website
from antrag, itukz kz,antobsich ao,sichtyp si
where 
antrag.sysid = kz.sysantrag
and ao.sysantrag = antrag.sysid
and ao.sysit = kz.sysit
and ao.syssichtyp = si.syssichtyp
and si.rang in (10,80,110)
and antrag.sysid = :pANTRAGID";

        public const string CONTRACT =
        @"select  
  prproduct.name  as productType
  ,vart.code  as contractType
  ,ak.Zins as baseInterestRate
  ,mwst.prozent as VATpercentage
  ,nvl(ak.BGINTERN,0) as financedAmountNet
  ,nvl(ak.BGINTERN,0) as totalLoanAmountNet
  ,nvl(ak.BGINTERNBRUTTO,0) as totalLoanAmountGross
  ,nvl(ak.BGINTERNBRUTTO,0) as financedAmountGross
  ,CASE WHEN nvl(ak.gesamtkostenbrutto,0) = 0 THEN 0 ELSE  nvl(ak.gesamtkostenbrutto/(1+mwst.prozent/100),0) END as overallCreditCostsNet
  ,nvl(ak.gesamtkostenbrutto,0) as  overallCreditCostsGross
  ,nvl(ak.LZ,0) as term
  ,nvl(ak.rate,0) as totalMonthlyPaymentNet
  ,nvl(ak.RwKalkBrutto,0) as residualValueExternalGross
  ,ao.WERKABGABEPREISBRUTTO as discountAmountGross
  ,ao.AHKRABATTOBRUTTO as discountGross
  ,CASE WHEN nvl(ak.RwKalkBrutto,0) = 0 OR nvl(ao.WERKABGABEPREISBRUTTO,0) = 0 THEN 0 ELSE nvl((ak.RwKalkBrutto / ao.WERKABGABEPREISBRUTTO)*100,0) END as residualValueExternalGrossPer
  ,nvl(ak.szbrutto,0) as downpaymentNetGross
  ,nvl(ak.szbruttop,0) as downpaymentGrossPercentage
  ,nvl(ak.sz,0) as downpaymentNet
  ,nvl(ak.Zins,0) as interestRate
  ,nvl(ak.Zinseff,0) as effInterestRate
  ,CASE WHEN nvl(ak.bginternbrutto,0) = 0 THEN 0 ELSE nvl(ak.bginternbrutto / 100 * ak.zinseff,0) END as interestIncome
  ,null as bankGrants
  ,CASE WHEN instr(vart.code,'KREDIT')>0 THEN nvl(ak.ratebrutto,0) ELSE nvl(ak.ratebrutto,0) + (select nvl(sum(praemie),0) from antvs where sysantrag = an.sysid) END as totalMonthlyPaymentGross
from 
  antrag an
  ,antkalk ak
  ,antob ao
  ,vart
  ,mwst
  ,angsubv
  ,prproduct
where
ak.sysantrag = an.SYSID
and an.sysvart = vart.sysvart
and ao.sysantrag = an.sysid
and mwst.sysmwst (+) = an.sysmwst
and an.sysid = angsubv.sysangebot(+)
and an.sysprproduct = prproduct.sysprproduct
and an.sysid = :pANTRAGID";

        public const string DEALER =
        @"select
   hd.name as dealerName
  ,hd.sysperson as dealerNumber
  ,hd.strasse||' '||hd.hsnr||' '||hd.PLZ||' '||hd.ort as dealerAddress
  ,vk.name||' '||vk.vorname as salesmanName
  ,Vk.Sysperson As Salesmannumber
  ,(Select Name From Perole where Sysroletype = 13 Connect By Prior Sysparent = Sysperole Start With Perole.Sysperson = hd.sysperson)
  ,null as dealerLimit
  ,Null As Dealerrating
  ,Roletype.Name As Dealertype
From
  antrag,
  Person Hd,
  Person Vk,
  perole,
  roletype
Where Perole.Sysperson = Hd.Sysperson
and perole.sysroletype = roletype.sysroletype
and hd.sysperson = antrag.sysvm
and vk.sysperson = antrag.sysvk
and antrag.sysid = :pANTRAGID";

        public const string DEALERCONTACT =
        @"select
   vk.NAME as contactPerson
  ,hd.TELEFON as phoneBusiness
  ,hd.PTELEFON as phonePrivate
  ,hd.HANDY as mobilePrivate
  ,hd.FAX as faxPrivate
  ,hd.EMAIL as emailPrivate
  ,hd.URL as website
from antrag, person hd , person vk
where
  hd.sysperson = antrag.sysvm
  and vk.sysperson = antrag.sysvk
and antrag.sysid = :pANTRAGID";

        public const string DRIVERLICENSE =
        @"select
 null as documentID
,null as driversLicenseBusiness
from it, antrag
where it.sysit = antrag.sysit
and antrag.sysid = :pANTRAGID";

        public const string EXISTINGAPPLICATIONS =
        @"select DISTINCT( applicationId), applicationDateTime,applicationDownpayAmo,applicationInitialFinAmo,applicationStatus,applicationTotalExposureFlag,applicationVersion 
from
(select
   antobsich.sysmh as customerID
  ,antrag.erfassung as applicationDateTime
  ,antkalk.szbrutto as applicationDownpayAmo
  ,case when INSTR(antrag.antrag,'W') = 0 then antrag.antrag else substr(antrag.antrag,1,INSTR(antrag.antrag,'W')-2) end as applicationId
  ,antkalk.bginternbrutto as applicationInitialFinAmo
  ,(select id from ddlkppos where code = 'GUARDEAN_STATUS' and activeflag = 1 and domainid = 'ANTRAG' and upper(value)= upper(antrag.attribut)) as applicationStatus
  ,'0' as applicationTotalExposureFlag
  ,case when INSTR(antrag.antrag,'W') = 0 then 1 else length(antrag.antrag)+2-INSTR(antrag.antrag,'W') end as applicationVersion
  ,to_char(antrag.zustandam,'dd.MM.yyyy hh24:mi:ss')
from antrag,antkalk,antobsich,ITPKZ
where antrag.sysid = antkalk.sysantrag 
and antobsich.rang not in (101,102)
and antobsich.sysantrag(+) = antrag.sysid
and itpkz.sysperson = :pSysKD
and (antrag.sysit = itpkz.sysit or antobsich.sysit = itpkz.sysit))";

        public const string EXISTINGCONTRACTS =
        @"select * from (
WITH 
V_MWST as
(select
CASE WHEN MWSTDATE.PROZENT is null THEN mwst.prozentneu ELSE MWSTDATE.PROZENT END PROZENT
,MWST.SYSMWST SYSID
,DENSE_RANK() OVER (PARTITION BY MWST.SYSMWST ORDER BY MWSTDATE.GUELTIGAB DESC) RANG
from MWST,MWSTDATE where MWSTDATE.SYSMWST (+) = MWST.SYSMWST)
select 
   nvl(bonitaet.rating,0) as contractApplicantRating
  ,vt.KAUTIONVVT as contractCautionDesposit
  ,0 as contractDelinquant
  ,vt.SZ as contractDownpayment
  ,vt.SZ/vt.BGEXTERN * 100 as contractDownpayPercentage
  ,vt.ende as contractEndDate
  ,vt.BGEXTERN as contractFinancedAmount
  ,(select prproduct.name from prproduct where sysprproduct = vt.sysprproduct)  as contractFinancialProduct
  ,vt.vertrag as contractID
  ,(select count (vtobslpos.sysid) from vtobsl, vtobslpos where vtobsl.sysid = vtobslpos.sysvtobsl and vtobsl.sysvt = vt.sysid) as contractInstallments
  ,vt.ZINS as contractInterestRate
  ,vt.EWBBETRAG as contractLossGivenDefault
  ,vt.RATE as contractMonthlyInstallGross
  ,CASE WHEN V_MWST.PROZENT > 0 THEN ROUND(vt.rate-(vt.RATE / CASE WHEN V_MWST.PROZENT > 0 THEN V_MWST.PROZENT ELSE 1 END),2) ELSE vt.rate END as contractMonthlyInstallNet
  ,vt.vertrag as contractPrevApplicationId
  ,(select sum(FIVKZNJ.SBSALDOS - FIVKZNJ.SBSALDOH) as BETRAG
    from VT v,PENKONTO,FIVKZNJ,LSADD
    where v.SYSID = PENKONTO.SYSVT
    and PENKONTO.SYSNKONTO = FIVKZNJ.SYSSKONTO
    and PENKONTO.RANG in (10000,20000)
    and FIVKZNJ.SYSPERIOD = LSADD.AKTPERIOD
    and v.sysid = vt.sysid) as contractOutstanding
  ,vt.attribut as contractPaymentBehaviour
  ,vt.einzug as contractPaymentMethod
  ,ob.bezeichnung as contractProductType
  ,vt.RW as contractResidual
  ,vt.beginn as contractStartDate
  ,(select ID from ddlkppos where DOMAINID = 'VT' and upper(VALUE) = upper(vt.zustand))  as contractStatus 
  ,trunc(months_between(vt.ende,vt.beginn)+1) as contractTerm
  ,vart.bezeichnung as contractType
  ,ob.hersteller as contractVehicleBrand
  ,nvl((select decode(rating.status,1,2,2,2,3,1,4,1,5,2,0) from rating where area = 'ANTRAG' and sysid = vt.sysantrag),0) /* 0 unklar / 1=automatisch 2=manuell*/ as contractStatusHistDecisions
  ,0 as contractDeferralsCount
  ,nvl((select mzaehler1 from vtmahn where sysvtmahn = vt.sysid),0) as contractDunLevelOneCount
  ,nvl((select mzaehler2 from vtmahn where sysvtmahn = vt.sysid),0) as contractDunLevelTwoCount
  ,nvl((select mzaehler3 from vtmahn where sysvtmahn = vt.sysid),0) as contractDunLevelThreeCount
  ,nvl((select sum(r.RLSKZ) from vt v,rn r where r.sysvt = v.sysid and v.sysid = vt.sysid),0) as directDebitChargeBack
  ,(select case when count(*) > 0 then 1 else 0 end from vt v,rn r where r.sysvt = v.sysid and  v.sysid = vt.sysid and trunc(r.belegdatum) <= trunc(sysdate) - 360) as directDebitChargeBack12m
  ,vt.sysantrag
  ,(
  SELECT NVL(SUM(RN.GBetrag + RN.GSteuer + RN.Mahnbetrag + RN.Zinsen + RN.Gebuehr + RN.FremdGebuehr - (RN.Teilzahlung + RN.Anzahlung + RN.Storno + RN.WAusgleich)),0) BETRAG
  FROM rn
  WHERE rn.bezahlt  = 0
  AND rn.art        = 0
  AND nvl(rn.SYSPERSON,0) != 0
  AND rn.sysvt = vt.sysid) as ContractOutstandingInvoice
  ,vt.sysid
from 
  vt
  ,bonitaet
  ,ob
  ,vart
  ,V_MWST
  ,(select vo.sysmh,vo.sysvt from vtobsich vo,sichtyp si where vo.syssichtyp = si.syssichtyp and si.rang in (10,80,110)) VO
where vt.syskd = bonitaet.sysperson (+)
and vt.sysid = bonitaet.sysvt (+)
and vt.sysid = ob.sysvt
and VO.sysvt(+) = vt.sysid
and vart.sysvart(+) = vt.sysvart
and V_MWST.SYSID (+) = vt.SYSMWST
and V_MWST.RANG = 1
and (vt.syskd = :pSysKD or VO.sysmh = :pSysKD)
)";

        public const string EXTENDEDADDRESS =
        @"select
   KZ.WOHNART as typeOfLiving
  ,KZ.STRASSE as street
  ,KZ.HSNR as housenumber
  ,KZ.PLZ as postalCode
  ,KZ.ORT as city
  ,land.ISO as country
  ,KZ.wohnseit as addressSince
  ,'1' as adresstype
from
  itpkz kz
  ,antrag
  ,land
where kz.sysit = antrag.sysit
and kz.sysland (+) = land.SYSLAND
and antrag.sysid = :pANTRAGID
union
select
   null
  ,kz.STRASSE2 as street
  ,kz.HSNR2 as housenumber
  ,kz.PLZ2 as postalCode
  ,kz.ORT2 as city
  ,case when kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null then land.iso end as country
  ,null
  ,case when kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null then '2' end  as adresstype
from
  itpkz kz
  ,antrag
  ,land
where kz.sysit = antrag.sysit
and kz.sysland2 = land.SYSLAND (+)
and (kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null)
and antrag.sysid = :pANTRAGID
and kz.sysantrag = antrag.sysid
union
select
   null as typeOfLiving
  ,KZ.STRASSE as street
  ,KZ.HSNR as housenumber
  ,KZ.PLZ as postalCode
  ,KZ.ORT as city
  ,land.ISO as country
  ,null as addressSince
  ,'1' as adresstype
from
  itukz kz
  ,antrag
  ,land
where kz.sysit = antrag.sysit
and kz.sysland (+) = land.SYSLAND
and kz.sysantrag = antrag.sysid
and antrag.sysid = :pANTRAGID";

        public const string EXTENDEDADDRESS2 =
        @"select
   kz.WOHNART as typeOfLiving
  ,kz.STRASSE as street
  ,kz.HSNR as housenumber
  ,kz.PLZ as postalCode
  ,kz.ORT as city
  ,land.ISO as country
  ,kz.wohnseit as addressSince
  ,'1' as adresstype
from itpkz kz, antrag, antobsich, land, sichtyp
where kz.sysit = antobsich.sysit
and antrag.sysid = antobsich.SYSANTRAG
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and land.sysland (+) = kz.syslandnat
and antrag.sysid = :pANTRAGID
and kz.sysantrag = antrag.sysid
union
select
   null
  ,kz.STRASSE2 as street
  ,kz.HSNR2 as housenumber
  ,kz.PLZ2 as postalCode
  ,kz.ORT2 as city
  ,case when kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null then land.iso end as country
  ,null
  ,case when kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null then '2' end  as adresstype
from itpkz kz, antrag, antobsich, land, sichtyp
where kz.sysit = antobsich.sysit
and (kz.STRASSE2 is not null or kz.HSNR2 is not null or kz.PLZ2 is not null or kz.ORT2 is not null)
and antrag.sysid = antobsich.SYSANTRAG
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and land.sysland (+) = kz.syslandnat
and antrag.sysid = :pANTRAGID
and kz.sysantrag = antrag.sysid";

        public const string EXTENDEDAPPLICANT =
        @"select
  kz.sysit as applicantID,
  null as customerId,
  decode(kz.sysKDTYP,1,0,2,2,3,1) as applicantType,
  0 as applicantRole,
  (select nvl(trunc(koopvon),null) from person where kz.sysperson = person.sysperson) as  customerSince
from
  itpkz kz,
  antrag,
  antkalk
where kz.sysit = antrag.sysit
and antkalk.SYSANTRAG = antrag.SYSID
and kz.sysantrag = antrag.sysid
and antrag.sysid = :pANTRAGID
union all
select
  kz.sysit as applicantID,
  null as customerId,
  decode(kz.sysKDTYP,1,0,2,2,3,1) as applicantType,
  0 as applicantRole,
  (select nvl(trunc(koopvon),null) from person where kz.sysperson = person.sysperson) as  customerSince
from
  itukz kz,
  antrag,
  antkalk
where kz.sysit = antrag.sysit
and antkalk.SYSANTRAG = antrag.SYSID
and kz.sysantrag = antrag.sysid
and antrag.sysid = :pANTRAGID";

        public const string EXTENDEDAPPLICANT2 =
        @"select
  kz.sysit as applicantID,null as customerId,		
  decode(kz.sysKDTYP,1,0,2,2,3,1) as applicantType,
  decode(sichtyp.rang, 10, 1, 80, 2, 110, 2, 120, 2) as applicantRole,
  (select nvl(trunc(koopvon),null) from person where kz.sysperson = person.sysperson) as  customerSince
from
  itpkz kz,
  antrag,
  antobsich,
  sichtyp
where kz.sysit = antobsich.sysit			
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110,120)
and antrag.sysid = kz.sysantrag
and antrag.sysid = :pANTRAGID";

        public const string EXTENDEDAPPLICANTCOMP =
        @"SELECT 
  itukz.name as companyName,
  it.rechtsformcode as legalForm,		
  branchea.schluessel as industrialSector,
  itukz.gruendung as foundationDate,
  itukz.hregister as registerNumber,
  itukz.identust as businessIdentificationNumber,
  it.hregisterort as legalCourt
FROM it,
  itukz,
  branche,
  antrag,
  branchea
Where It.Sysit                     = antrag.Sysit
and itukz.sysit = antrag.sysit
and itukz.SYSANGEBOT = antrag.sysangebot
And Itukz.Sysbranche                  = Branche.Sysbranche (+)
And Substr(Branche.Schluessel,1,2) = Branchea.Schluessel (+)
and antrag.sysid = :pANTRAGID";

        public const string EXTENDEDAPPLICANTCOMP2 =
        @"SELECT
   it.name as companyName,
    it.rechtsformcode as legalForm,
    branchea.schluessel as industrialSector,
    it.gruendung as foundationDate,
    it.hregister as registerNumber,
    itukz.identust as businessIdentificationNumber,
    it.hregisterort as legalCourt
FROM antobsich
  ,it
  ,itukz
  ,bonitaet
  ,branchea
  ,antrag
  ,sichtyp
  ,branche
WHERE it.sysit                     = antobsich.sysit
AND itukz.sysit                    = antobsich.sysit
and itukz.SYSANGEBOT               = antrag.sysangebot
AND it.sysperson                   = bonitaet.sysperson (+)
AND It.Sysbranche                  = Branche.Sysbranche (+)
AND SUBSTR(Branche.Schluessel,1,2) = Branchea.Schluessel (+)
AND antobsich.sysantrag            = antrag.sysid
AND antobsich.syssichtyp           = sichtyp.syssichtyp
AND sichtyp.rang                 IN (10,80,110)
AND antrag.sysid              = :pANTRAGID";

        public const string EXTENDEDAPPLICANTPRIVATE =
        @"SELECT
  ITPKZ.Familienstand                                                                     AS maritalStatus ,
  ITPKZ.KINDERIMHAUS                                                                      AS numberOfChildUndEighteen,
  DECODE(ITPKZ.anredecode,'1','Frau','2','Mann','3','Firma','Herr','Herr','Frau','Frau','Firma','Firma') AS salutation,
  ITPKZ.TITELCODE                                                                         AS title ,
  ITPKZ.VORNAME                                                                           AS firstName,
  ITPKZ.NAME                                                                              AS lastName,
  ITPKZ.IDENTEG                                                                           AS taxId,
  ITPKZ.GEBDATUM                                                                          AS dateOfBirth, 
  ITPKZ.GEBORT                                                                            AS placeOfBirth,
  replace(ITPKZ.PREVNAME,'-',' ')                                                         AS birthName,
  land.iso                                                                                AS citizenship,
  DECODE(ITPKZ.anredecode,'1','1','2','0','3',NULL,'Herr','0','Frau','1','Firma',NULL)    AS gender
FROM it,
  itpkz,
  antrag,
  land
WHERE it.sysit        = antrag.sysit
and itpkz.sysit = antrag.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and land.sysland      = it.syslandnat
and antrag.sysid = :pANTRAGID";

        public const string EXTENDEDAPPLICANTPRIVATE2 =
        @"select
   ITPKZ.Familienstand as maritalStatus
  ,ITPKZ.KINDERIMHAUS as numberOfChildUndEighteen
  ,decode(ITPKZ.anredecode,'1','Frau','2','Mann','3','Firma','Herr','Herr','Frau','Frau','Firma','Firma')  as salutation
  ,ITPKZ.TITELCODE as title
  ,ITPKZ.VORNAME as firstName
  ,ITPKZ.NAME as lastName
  ,ITPKZ.IDENTEG as taxId
  ,ITPKZ.GEBDATUM as dateOfBirth	
  ,ITPKZ.GEBORT as placeOfBirth
  ,replace(ITPKZ.PREVNAME,'-',' ') as birthName
  ,land.iso as citizenship
  ,cast(nvl(itpkz.WOHNVERHCODE,0) as Number) as jointHousehold
  ,DECODE(ITPKZ.anredecode,'1','1','2','0','3',NULL,'Herr','0','Frau','1','Firma',NULL) AS gender
from it, itpkz, antrag, antobsich, land, sichtyp
where it.sysit = antobsich.sysit
and itpkz.sysit = antobsich.sysit
and itpkz.SYSANGEBOT = antrag.sysangebot
and antrag.sysid = CASE WHEN nvl(antobsich.SYSANTRAG,0) = 0 then antobsich.SYSVT else antobsich.SYSANTRAG END
and antobsich.syssichtyp (+) = sichtyp.syssichtyp
and sichtyp.rang in (10,80,110)
and land.sysland = itpkz.syslandnat (+)
and antrag.sysid = :pANTRAGID";

        public const string INSURANCE =
        @"select
 null as gap
,null as legalInsurance
,null as legalInsurancePlus
,null as riskInsurance
from antvs,antrag
where antvs.sysantrag = antrag.sysid
and antrag.sysid = :pANTRAGID";

        public const string RELATIONMEMBERS =
            //            @"select 
            //knetyp.code as typeCode,
            //kne.relatetypecode as roleCode,
            //kne.bezeichnung as groupName,
            //kne.sysober as dominationCustomerId,
            //kne.sysunter as dominatedCustomerId,
            //kne.quote as shareRel,
            //kne.quoteabs as shareAbs,
            //kne.gueltigvon as validFrom,
            //kne.gueltigbis as validTill,
            //wfuser.code as editor,
            //kne.begruendung as description,
            //kne.bestehtseit as creationDate,
            //kne.syskne as relationId
            //from kne, wfuser, knetyp
            //where kne.bezeichnung in 
            //  (select distinct k.bezeichnung from kne k where 
            //    (k.sysober = :pSysKD or k.sysunter = :pSysKD) 
            //    and k.bezeichnung is not null
            //    and k.activeflag = 1
            //    and k.geprueftflag = 1
            //    and (trunc(k.gueltigvon) <= sysdate OR cic.cic_sys.to_cladate(k.gueltigvon) = 0 OR k.gueltigvon is NULL) 
            //    and (trunc(k.gueltigbis) >= sysdate OR cic.cic_sys.to_cladate(k.gueltigbis) = 0 OR k.gueltigbis is NULL)
            //  )
            //and kne.sysknetyp = knetyp.sysknetyp
            //and wfuser.syswfuser (+) = kne.geprueftvon
            //and kne.activeflag = 1
            //and kne.geprueftflag = 1
            //and (trunc(kne.gueltigvon) <= sysdate OR cic.cic_sys.to_cladate(kne.gueltigvon) = 0 OR kne.gueltigvon is NULL) 
            //and (trunc(kne.gueltigbis) >= sysdate OR cic.cic_sys.to_cladate(kne.gueltigbis) = 0 OR kne.gueltigbis is NULL)";

            @"with liabilities as (
    SELECT kne.*
    FROM kne, knetyp where 
      kne.sysknetyp = knetyp.sysknetyp
      and (knetyp.code in ('KNE','GVKA','GVKB') or (knetyp.code = 'OHNE' and kne.relatetypecode = 'WB'))
      -- sysknetyp in (select sysknetyp from knetyp where code in ('KNE','GVKA','GVKB','OHNE')) -- KNE GVKA GVKB
      and kne.activeflag = 1
      and kne.geprueftflag = 1
      and (trunc(kne.gueltigvon) <= sysdate OR cic.cic_sys.to_cladate(kne.gueltigvon) = 0 OR kne.gueltigvon is NULL) 
      and (trunc(kne.gueltigbis) >= sysdate OR cic.cic_sys.to_cladate(kne.gueltigbis) = 0 OR kne.gueltigbis is NULL)
    START WITH sysunter = :pSysKD
    CONNECT BY NOCYCLE 
     (prior sysober = sysunter or prior sysunter = sysober or prior sysunter = sysunter or prior sysober = sysober)
      -- and sysknetyp in (select sysknetyp from knetyp where code in ('KNE','GVKA','GVKB','OHNE')) -- KNE GVKA GVKB
      and kne.sysknetyp = knetyp.sysknetyp
      and (knetyp.code in ('KNE','GVKA','GVKB') or (knetyp.code = 'OHNE' and kne.relatetypecode = 'WB'))
      and prior kne.activeflag = 1
      and prior kne.geprueftflag = 1
      and (trunc(prior kne.gueltigvon) <= sysdate OR cic.cic_sys.to_cladate(prior kne.gueltigvon) = 0 OR prior kne.gueltigvon is NULL) 
      and (trunc(prior kne.gueltigbis) >= sysdate OR cic.cic_sys.to_cladate(prior kne.gueltigbis) = 0 OR prior kne.gueltigbis is NULL))
    
select 
knetyp.code as   typeCode ,
kne.relatetypecode as     roleCode ,
kne.bezeichnung as        groupName        ,
kne.sysober as   dominationCustomerId      ,
kne.sysunter as  dominatedCustomerId       ,
kne.quote as     shareRel ,
kne.quoteabs as  shareAbs ,
kne.gueltigvon as validFrom        ,
kne.gueltigbis as validTill        ,
wfuser.code as   editor   ,
kne.begruendung as        description      ,
kne.bestehtseit as        creationDate     ,
kne.syskne as    relationId
from 
  knetyp,
  kne,
  -- roletype,
  wfuser,
  ( select distinct syskne 
    from (
      SELECT * FROM kne k
      where 
        sysknetyp in (select sysknetyp from knetyp where code in ('RE','WE')) -- nur pfade vom typ RE gehen
        and activeflag = 1
        and geprueftflag = 1
        and (trunc(k.gueltigvon) < = sysdate OR cic.cic_sys.to_cladate(k.gueltigvon) = 0 OR k.gueltigvon is NULL) 
        and (trunc(k.gueltigbis) > = sysdate OR cic.cic_sys.to_cladate(k.gueltigbis) = 0 OR k.gueltigbis is NULL)
      START WITH sysunter in (select TO_NUMBER(:pSysKD) as syskd from dual union select sysober from liabilities) or sysober in (select TO_NUMBER(:pSysKD) as syskd from dual union select sysober from liabilities)
      CONNECT BY NOCYCLE 
        (prior sysober = sysunter or prior sysunter = sysober or prior sysunter = sysunter or prior sysober = sysober) -- darf hoch/runter gehen oder umdrehen
        and prior sysknetyp in (select sysknetyp from knetyp where code in ('RE')) -- nur pfade vom typ RE gehen
        and prior activeflag = 1
        and geprueftflag = 1
        and (trunc(prior k.gueltigvon) < = sysdate OR cic.cic_sys.to_cladate(prior k.gueltigvon) = 0 OR prior k.gueltigvon is NULL) 
        and (trunc(prior k.gueltigbis) > = sysdate OR cic.cic_sys.to_cladate(prior k.gueltigbis) = 0 OR prior k.gueltigbis is NULL)
      UNION 
      select * from liabilities)
  ) found
where 
  kne.syskne = found.syskne and
  knetyp.sysknetyp = kne.sysknetyp and
  -- roletype.sysroletype (+) = kne.sysroletype and
  wfuser.syswfuser (+) = kne.geprueftvon 
order by kne.syskne desc";

        public const string RESIDUALVALUES =
        @"select * from
(
with INPUTDATA as
(SELECT CASE WHEN nvl(obtyp.SYSVGRW,0) = 0 THEN (select VGVALID.SYSVG from VGVALID where upper(name) = 'FALLBACK') ELSE nvl(obtyp.SYSVGRW,0) END SYSRW,
  CASE
    WHEN cic.CIC_SYS.TO_CLADATE(antob. erstzul) = 0
    THEN 1
    ELSE greatest (0,ROUND(months_between(trunc(sysdate),DECODE(NVL(cic.cic_sys.to_cladate(antob.erstzul),0),0,trunc(sysdate),antob.erstzul))))
  END FHZALTER,
  antob.JAHRESKM LL,
  antob.UBNAHMEKM AS STARTKM
FROM antrag,
  antob,
  obtyp ,
  vgrw,
  vgvalid
WHERE antrag.sysid = antob.sysantrag
AND antob.SYSOBTYP = obtyp.sysobtyp
AND vgrw.SYSVG     = CASE WHEN nvl(obtyp.SYSVGRW,0) = 0 THEN (select VGVALID.SYSVG from VGVALID where upper(name) = 'FALLBACK') ELSE nvl(obtyp.SYSVGRW,0) END
AND vgvalid.SYSVG  = vgrw.SYSVG
AND antrag.sysid  = :pANTRAGID
AND (trunc(vgvalid.validfrom) < = trunc(sysdate) OR cic.cic_sys.to_cladate(vgvalid.validfrom) = 0 OR vgvalid.validfrom is NULL)
AND (trunc(vgvalid.validuntil) > = trunc(sysdate) OR cic.cic_sys.to_cladate(vgvalid.validuntil) = 0 OR vgvalid.validuntil is NULL)
)
SELECT vgline.scalevalue as milageKey, vgclmn.scalevalue as monthKey, vgcell.value as value
FROM vgvalid,
  vgclmn,
  vgcell,
  vgline,
  INPUTDATA
WHERE vgvalid.SYSVGVALID = vgclmn.SYSVGVALID
AND vgvalid.SYSVG        = INPUTDATA.SYSRW
AND vgline.sysvgvalid = vgvalid.sysvgvalid
AND vgcell.sysvgclmn = vgclmn.sysvgclmn
AND vgcell.sysvgline = vgline.sysvgline
AND (trunc(vgvalid.validfrom) < = trunc(sysdate) OR cic.cic_sys.to_cladate(validfrom) = 0 OR validfrom is NULL)
AND (trunc(vgvalid.validuntil) > = trunc(sysdate) OR cic.cic_sys.to_cladate(validuntil) = 0 OR validuntil is NULL)
ORDER BY to_number(vgclmn.SCALEVALUE) ASC
)";

        public const string REPLACEMENT =
@"select  
antabl.BETRAG as amount
,case when antkalk.ahk <= 0 then 0 else antabl.betrag/antkalk.ahk *100 end as amountper
,antabl.DATKALK as replacementDate
,antabl.SYSABLTYP as type
,antabl.BANK as bankname
,antabl.IBAN as iban
,case when antabl.SYSABLTYP = 1 then vt.vertrag else fremdvertrag end as precontract
,antabl.FREMDVERTRAG
,antabl.AKTUELLERATE as rate
from antabl,vt, antkalk where antabl.sysvorvt = vt.sysid(+) and antabl.SYSANTRAG = antkalk.sysantrag and antabl.sysantrag = :pANTRAGID";

        public const string SCHWACKE =
        @"select
ak.RWBASE as residualValueInternal
,ao.SCHWACKERW as  fairMarketValue
,CASE WHEN INSTR(upper(ao.quellelp),'VALUATION') > 0 THEN 1 ELSE 0 END as  fairMarketValueSpecified
from 
antob ao
,antrag an
,antkalk ak
where an.sysid = ao.sysantrag
and an.sysid = ak.sysantrag 
and an.sysid = :pANTRAGID";

        public const string SHARES =
@"select 
distinct kne.sysober as parentDealerId, 
person.name as parentDealerName, 
decode(knetyp.code, 'GVKW', 'CBU', 'WIBE', 'UBO') as applicantRole, 
1 as shareholderPercentage 
from kne, person, knetyp
where kne.sysober = person.sysperson
and kne.sysknetyp = knetyp.sysknetyp
and kne.sysunter = :pSysKd";

        public const string VEHICLE =
        @"select
ao.BEZEICHNUNG as model
,ao.RWCRV as residualValueInternal
,ao.AHKBRUTTO as buyPriceAfterDiscountGross
,ao.AHK as buyPriceAfterDiscountNet
,ao.GRUNDBRUTTO as buyPriceBeforeDiscountGross
,ao.GRUND buyPriceBeforeDiscountNet
,ao.Automatik as carTransmission
,ao.Fabrikat as modelLine
,ao.FarbeA as carcolour
,ao.Grund as listPriceNet
,ao.GrundBrutto as listPriceGross
,ao.GRUNDBRUTTO-ao.GRUNDUST as listPriceAfterDiscountNet
,ao.GRUND as listPriceAfterDiscountGross
,ao.Hersteller as brand
,ao.HERZUBBRUTTO as manuEquipGross
,ao.HERZUBEXTERNBRUTTO as manufEquiAfterDiscGross
,ao.JahresKM as mileagePerYear
,ao.PaketeBrutto as vehiclePackagesGross
,ao.PaketeExternBrutto as vehiPackAfterDiscountGross
,ao.Schwacke as eurotaxCode
,ao.Serie as vehicleChassisNo
,ao.fabrikat as model
,ao.Typ as modelType
,ao.SONZUBBRUTTO as specialEquipmentGross 
,ao.AHKRABATTOBRUTTO as discountGross 
,ao.ZubehoerBrutto as dealerEquipmentGross
,ao.ZubehoerExternBrutto as dealerEquiAfterDiscountGross
,ao.RWBrutto as residualValueExternal
,ao.UeberfuehrungBrutto as transportionCostsGross
,ao.ZulassungBrutto as carRegistrationsCostsGross
,ab.PS as powerPS
,CASE WHEN (ao.SYSOBART = 12 OR ao.SYSOBART = 15) AND nvl(cic.CIC_SYS.TO_CLADATE(ao.ERSTZUL),0) = 0 THEN null ELSE CASE WHEN nvl(cic.CIC_SYS.TO_CLADATE(ao.ERSTZUL),0) = 0 THEN to_char(sysdate,'dd.mm.yyyy')||' '||to_char(sysdate,'hh24:mi:ss') ELSE to_char(nvl(ao.ERSTZUL,sysdate),'dd.mm.yyyy')||' '||to_char(nvl(ao.ERSTZUL,sysdate),'hh24:mi:ss') END END as registrationDate
,ao.UBNAHMEKM as mileage
,ak.bgextern as buyPriceNet
,ak.bgexternbrutto as buyPriceGross
,ao.ZUBEHOERBRUTTO as  assetPriceGross
,ao.ZUBEHOER as  assetPriceNet
,nvl(ao.SCHWACKERW,0) as  fairMarketValue
,ao.SYSOBART as vehicleStatus
from
antob ao
,antrag an
,antkalk ak
,antobbrief ab
,OBART
,ANTOBOPTION aopt
where an.sysid = ao.sysantrag
and ak.sysantrag = an.sysid
and ab.SYSANTOBBRIEF = ao.sysob
and ao.sysobart = obart.sysobart (+)
and ao.sysob = aopt.SYSID (+)
and an.sysid = :pANTRAGID
";

        public const string PEPINFORMATION = @"SELECT
  COMPLIANCE.BEZEICHNUNG as employmentTitle,
  compliance.beginn as since,
  compliance.ende as till,
  land.ISO as country
FROM antrag,
  itpkz,
  land,
  COMPLIANCE
WHERE COMPLIANCE.area    = 'ITPKZ'
AND COMPLIANCE.flagaktiv = 1
AND itpkz.sysitpkz       = COMPLIANCE.sysid
AND antrag.sysit         = itpkz.sysit
AND compliance.sysland   = land.sysland (+)
AND antrag.sysid         = itpkz.sysantrag
AND antrag.sysid         = :pANTRAGID
";

        public const string PEPINFORMATION2 = @"SELECT 
  COMPLIANCE.BEZEICHNUNG as employmentTitle,
  compliance.beginn as since,
  compliance.ende as till,
  land.ISO as country
FROM antobsich,
  sichtyp,
  itpkz,
  land,
  COMPLIANCE
WHERE COMPLIANCE.area    = 'ITPKZ'
AND COMPLIANCE.flagaktiv = 1
AND itpkz.sysitpkz       = COMPLIANCE.sysid
AND antobsich.sysantrag  = itpkz.sysantrag
AND antobsich.sysit      = itpkz.sysit
AND compliance.sysland = land.sysland (+)
AND antobsich.syssichtyp = sichtyp.syssichtyp
AND sichtyp.rang        IN (10,80)
AND antobsich.sysantrag  = :pANTRAGID";



        public const string PREMIUMS =
            @"SELECT vstyp.code AS code
,antvs.PRAEMIE   AS value
,vstyp.sysvstyp  AS type
FROM antvs,
vstyp
WHERE antvs.sysvstyp = vstyp .sysvstyp
AND antvs.sysantrag  = :pANTRAGID";

    }
}