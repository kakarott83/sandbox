using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Model.DdOd;
using System.Net;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.Prisma;
using CIC.Database.OD.EF6.Model;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public class StateStruct
    {
        public String id { get;set;}
        public String zustand { get; set; }
        public String attribut { get; set; }
    }

    /// <summary>
    /// Decision Engine DB Data Access Object
    /// </summary>
    public class DecisionEngineGuardeanDBDao : IDecisionEngineGuardeanDBDao
    {
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const String AUSKUNFTCFGBEZ = "GUARDIAN";
        //private static String DEFQUERY = "select it.ort as city, trim(land.COUNTRYNAME) as country, it.HSNR as housenumber, it.HSNR2 as housenumberaffix, it.PLZ as postalcode, it.STRASSE as street, wa.code as currency, null as employerContactPerson, itpkz.NAMEAG1 as employerName, itpkz.BESCHBISAG1 as employmentEndDate, trunc(months_between(sysdate,to_date(itpkz.beschseitag1,'dd.mm.yy')),1) as employmentMonthsSince, null as employmentSector, itpkz.BESCHSEITAG1 as employmentsStartDate, null as employmentType, null as experienceMonthsSince, null as isCurrentEmployment, null as isSelfEmployment, it.BERUF as profession, itpkz.BERUFLICHCODE as typeOfJobContract, null as debtIncomeRatio, itpkz.auslagen as expence, itpkz.EINKNETTO as income, itpkz.WEITEREAUSLAGEN + itpkz.AUSLAGEN +itpkz.BERUFSAUSLAGEN as totalExpenseAmount, itpkz.EINKBRUTTO + itpkz.NEBEINKBRUTTO + itpkz.ZEINKBRUTTO as totalGrossIncome, itpkz.EINKNETTO + itpkz.NEBEINKNETTO + itpkz.ZEINKNETTO as totalNetIncome, null as documentIssueDate, null as documentValidTill, null as isVeryfied, null as issuingAuthority, itpkz.AUSLAUSWEISCODE as residencePermition, itpkz.EINREISEDATUM as residencePermitionArrivalDate, itpkz.AUSLAUSWEISGUELTIG as residencePermitionUntil, an.ERFASSUNG as applicationDate, an.sysid as applicationID, blz.BIC as BIC, ko.IBAN as IBAN, ko.KONTONR as bankAccountNumber, ko.KONTOINHABER as bankAccountOwner, null as bankAccountSince, null as bankAccountType, blz.ORT as bankCity, null as bankCode, blz.NAME as bankName, null as paymentsMethod, null as nichtGedeckterFehlbetrag, null as nichtGedeckterFehlbetragVorh, null as nichtGedeckterFehlbetragVorz, it.EIGENKAPITAL as sharedCapitel, it.JAHRESUMSATZ as turnover, null as contactType, it.EMAIL as eMail, it.FAX as faxNumber, it.FAXKONT as faxNumberBusiness, it.HANDY as mobileNumber, it.HANDYKONT as mobileNumberBusiness, it.PTELEFON as phoneNumber, it.PTELEFONKONT as phoneNumberBusiness, (select count(1) from vstyp,vsart where vstyp.sysvsart = vsart.sysvsart and vstyp.sysvstyp = av.sysvstyp and vsart.sysvsart = 1) as PPI_FLAG_PAKET1, (select count(1) from vstyp,vsart where vstyp.sysvsart = vsart.sysvsart and vstyp.sysvstyp = av.sysvstyp and vsart.sysvsart = 2) as PPI_FLAG_PAKET2, ak.ZINS as baseInterestRate, ak.ANZAHLUNG as downPayment, ak.BGINTERNBRUTTO as financedAmount, null as term, ak.rate as totalMonthlyPayment, null as dealerCompany, null as dealerGroup, null as dealerRating, null as dealerType, (select count(1) from itadresse where itadresse.sysit = it.sysit and itadresse.rang = 2) as addAdressInfo, (select count(1) from itadresse where itadresse.sysit = it.sysit and itadresse.rang = 2) as addressExists, ia.GUELTIGAB as addressSince, ia.type as addressType, ia.ok as addressValidated, ia.ort as city_2, trim(land_2.countryname) as country_2, ia.hsnr as housenumber_2, null as housenumberaffix_2, null as isCurrentAddress, ia.strasse as street_2, ia.plz as postalCode_2, null as typeOfLiving, it.identust as VATNumber, itpkz.nameag2 as companyName, null as crefoID, it.rechtsform as legalForm, itukz.ANZMA as numberOfEmployees, null as registerNumber, null as birthName, trim(land_nat.countryname) as citizenship, null as countryOfBirth, null as educationType, null as employee, it.vorname as firstName, null as gender, it.name as lastname, (select ddlkppos.value from ddlkppos where ddlkppos.code = 'ZIVILSTAENDE' and id = it.FAMILIENSTAND) as maritalStatus, itpkz.ANZKINDER as numberOfChildrenType1, itpkz.ANZKINDER1 as numberOfChildrenType2, itpkz.ANZKINDER2 as numberOfChildrenType3, itpkz.ANZKINDER3 as numberOfChildrenType4, null as Dependents, null as placeOfBirth, months_between(sysdate,to_date(it.wohnseit,'dd.mm.yyyy')) as residencyMonthSince, it.WOHNSEIT as residencySince, null as residencyStatus, (select ddlkppos.value from ddlkppos where ddlkppos.code = 'ANREDE' and ddlkppos.domainid = 'KURZ' and id = it.anredecode) as salutation, null as schufaID, it.titel as title, null as amountForeclosure, it.sysperson as applicantID, null as applicantRole, case when it.syskdtyp = '1' then 'Privat' else 'Firma' end, pe.KOOPVON as customerSince, (select ctlang.languagename from ctlang where sysctlang = it.SYSCTLANG) as language, (select bezeichnung from sichtyp where syssichtyp = sec.syssichtyp) as securityType, null as state, null as accountNumber, null as date_f, null as description, null as featureCode, null as featureWithoutDateOfBirth, null as installmentNumber, null as installmentType, null as ownFeature, null as text, null as type, itpkz.EINKBRUTTO as grossIncome, null as incomeSince, null as incomeTerm, null as incomeType, itpkz.EINKNETTO as netIncome, itukz.ERGEBNIS1 as annualProfit, itukz.liquiditaet as liquidAssets, itukz.obligoeigen as shortTermLiabilities, itukz.bilanzwert as totalAssets, itukz.jumsatz as turnover, it.name as companyName, null as crefoID, (select ddlkppos.value from ddlkppos where ddlkppos.code = 'RECHTSFORMCODE' and id = itukz.rechtsformcode) as legalForm, it.gebdatum as dateOfBirth from  antrag an, antobsich aos, antobsich sec, antkalk ak, person pe, it, itkonto ko, blz, itpkz, pkz, antob ao, land, land land_2, land land_nat, WAEHRUNG wa, antvs av, itadresse ia, itukz where an.sysid = ak.sysantrag and an.syskd = pe.sysperson (+) and an.sysid = sec.sysantrag (+) and an.sysit = it.sysit and an.sysit = itpkz.sysit and an.syskd = pkz.sysperson (+) and an.sysid = aos.sysantrag (+) and an.sysid = pkz.sysantrag (+) and it.sysit= ko.sysit (+) and ko.sysblz = blz.sysblz (+) and an.sysid = ao.sysantrag and it.sysland = land.sysland and ak.SYSWAEHRUNG = wa.SYSWAEHRUNG (+) and an.sysid = av.sysantrag (+) and it.sysit = ia.sysit (+) and ia.sysland = land_2.sysland (+) and it.sysit = itukz.sysit (+) and it.syslandnat = land_nat.sysland and an.sysid = :pANTRAGID";
        private static String  STATEQUERY = "SELECT DDLKPPOS.ID,Statedef.ZUSTAND, attributdef.ATTRIBUT FROM State, Statedef, attribut, attributdef, DDLKPPOS WHERE State.SYSSTATEDEF=Statedef.SYSSTATEDEF AND attribut.SYSATTRIBUTDEF=attributdef.SYSATTRIBUTDEF AND State.SYSSTATE=attribut.SYSSTATE AND DDLKPPOS.VALUE (+) =attributdef.ATTRIBUT";
        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        /// <param name="cred"></param>
        public NetworkCredential getCredentials()
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                CIC.Database.OW.EF6.Model.AUSKUNFTCFG auskunft = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToUpper() == AUSKUNFTCFGBEZ).Single();
                NetworkCredential rval = new NetworkCredential
                {
                    Password = auskunft.KEYVALUE,
                    UserName = auskunft.USERNAME
                };
                return rval;
            }
        }


        /// <summary>
        /// returns the currently configured states map mapping from shs state to zustand.attribut
        /// </summary>
        /// <returns></returns>
        public List<StateStruct> getStatesMap()
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                List<StateStruct> states = context.ExecuteStoreQuery<StateStruct>(STATEQUERY, null).ToList();
                return states ?? new List<StateStruct>();
            }
        }

        /// <summary>
        /// Map DecisionEngineInDto to entities and save to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveDecisionEngineInput(long sysAuskunft, DecisionEngineGuardeanInDto inDto)
        {
            long sysdeinpexec = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                DEINPEXEC deInputExec = new DEINPEXEC {AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single()};
                context.DEINPEXEC.Add(deInputExec);
                context.SaveChanges();
                sysdeinpexec = deInputExec.SYSDEINPEXEC;
            }
            using (DdOdExtended context = new DdOdExtended())
            {


                //String value = XMLSerializer.Serialize(inDto, "UTF-8");
                DDLKPSPOS output = new DDLKPSPOS
                {
                    AREA = "DEINPEXEC",
                    SYSID = sysdeinpexec,
                    CONTENT = inDto.XMLREQUEST,
                    ACTIVEFLAG = 1
                };
                context.DDLKPSPOS.Add(output);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Map DecisionEngineOutDto to entities and save to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveDecisionEngineOutput(long sysAuskunft, DecisionEngineGuardeanOutDto outDto)
        {
            long sysdeoutexec = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                DEOUTEXEC deOutExec;

                // check if DEOUTEXEC already exists
                AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                if (Auskunft != null && !context.Entry(Auskunft).Collection(f => f.DEOUTEXECList).IsLoaded)
                    context.Entry(Auskunft).Collection(f => f.DEOUTEXECList).Load();
                
                // Update if DEOUTEXEC already exists / Create new objects if not
                /* if (Auskunft.DEOUTEXECList.Count() > 0)
                 {
                     // OUTPUTSÄTZE WURDEN BEREITS ANGELEGT, ES MUSS NICHTS MEHR ANGELEGT / UPGEDATET WERDEN!!!
                 }
                 else*/
                {
                    // NEW
                    deOutExec = new DEOUTEXEC {AUSKUNFT = Auskunft};
                    context.DEOUTEXEC.Add(deOutExec);

                    DEDECISION deDecision = new DEDECISION
                    {
                        DEOUTEXEC = deOutExec,
                        STATUS = outDto.executionId
                    };
                    context.DEDECISION.Add(deDecision);
                    context.SaveChanges();
                    sysdeoutexec = deOutExec.SYSDEOUTEXEC;
                }
            }
            using (DdOdExtended context = new DdOdExtended())
            {
                //String value = XMLSerializer.Serialize(inDto, "UTF-8");
                DDLKPSPOS output = new DDLKPSPOS
                {
                    AREA = "DEOUTEXEC",
                    SYSID = sysdeoutexec,
                    CONTENT = outDto.XMLRESPONSE,
                    ACTIVEFLAG = 1
                };
                context.DDLKPSPOS.Add(output);
                context.SaveChanges();
            }
        }




        /// <summary>
        /// Get entities from database by SysAuskunft and map entities to DecisionEngineInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>DecisionEngineInDto, filled with values from database</returns>
        public DecisionEngineGuardeanInDto FindBySysId(long sysAuskunft)
        {
            DecisionEngineGuardeanInDto inDto = null;
            using (DdIcExtended context = new DdIcExtended())
            {
                inDto = context.ExecuteStoreQuery<DecisionEngineGuardeanInDto>("select sysauskunft, sysid from auskunft where sysauskunft=" + sysAuskunft, null).FirstOrDefault();
            }

            return inDto;
        }

        /// <summary>
        /// Fills the InputDto from Antrag
        /// </summary>
        /// <param name="sysAntrag"></param>
        /// <param name="ma"></param>
        /// <returns></returns>
        public DecisionEngineGuardeanInDto fillFromAntrag(long sysAntrag, bool ma)
        {

            using (PrismaExtended ctx = new PrismaExtended())
            {
                // Get DecisionEngineInDto
                EaiparDao eaiParDao = new EaiparDao();

                String query = @"select antrag.sysid sysid, 0 isMa, it.syskdtyp
                        from it, antrag
                        where it.sysit = antrag.sysit
                        and antrag.sysid = :pANTRAGID";
                if (ma) query = @"select antrag.sysid sysid, 1 isMa, it.syskdtyp
                            from it, antrag, antobsich, sichtyp
                            where it.sysit = antobsich.sysit
                            and antobsich.sysantrag = antrag.sysid
                            and antobsich.syssichtyp = sichtyp.syssichtyp
                            and sichtyp.rang in (10,80,110) 
                            and antrag.sysid = :pANTRAGID";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pANTRAGID", Value = sysAntrag });
                return ctx.ExecuteStoreQuery<DecisionEngineGuardeanInDto>(query, parameters.ToArray()).FirstOrDefault();

            }
        }

    }
}