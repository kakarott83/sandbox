using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;

using Devart.Data.Oracle;
using System.Data.Common;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
	public class ScorePersonBO
	{
		/// <summary>
        /// SQL
        /// </summary>
        const String QUERY_ANY1 = "SELECT * FROM ANY";
        const String QUERY_ANY2 = "SELECT * FROM ANY";
        const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// Personen-/Customer-information auslesen
		/// (SQL-DEV-Statement in "BMWATDV1_SCORE_PERSON_CONTRACT.sql")
		/// </summary>
		/// <param name="personID">customer data</param>
		/// <returns></returns>
		public ScoreCustomerDto getPerson (long personID)
        {
			ScoreCustomerDto person = null;
			using (DdOlExtended ctx = new DdOlExtended ())
			{
				person = new ScoreCustomerDto ();

				// GET ALL Person Info 
				try
				{// Get the instance information 

					// CustomerType:
					//		Einzelunternehmer:	Person.privat=0 AND + Person.Gebdatum gefüllt AND person.rechtsform 
					//		Unternehmer:        Person.privat=0 AND + Person.Gebdatum nicht gefüllt AND person.rechtsform
					//		Privatperson:       Person.privat=1 AND + Person.Gebdatum gefüllt AND person.rechtsform
					// PhoneType:
					//		Person.telefon ='office', 
					//		Person.handy='mobile', 
					//		Person.petelfon='home'
					// PhoneNumber:
					//		if Person.telefon > '', 
					//			sonst if Person.handy > '', 
					//				sonst Person.ptelefon

					if (personID <= 0)
						throw new ArgumentException ("No User!");

					int counter = ctx.ExecuteStoreQuery<int> ("SELECT Count(*) FROM PERSON WHERE Person.sysperson = " + personID).FirstOrDefault ();
					if (counter <= 0)
						throw new ArgumentException ("Person with id " + personID + " not found!");


					const String QUERY_PERSON =
						@"SELECT Person.SYSPerson AS CustomerID
							, Person.SYSPerson AS CustomerReference
							, SUBSTR (TRIM (Person.Name), 0, 129) AS Surname
							, SUBSTR (TRIM (Person.Vorname), 0, 61) AS Forename
							, SUBSTR (TRIM (Person.Titel2), 0, 51) AS Title
							, SUBSTR (TRIM (Person.Anrede), 0, 51) AS Salutation
							, SUBSTR (TRIM (Person.Zusatz), 0, 61) AS MiddleName
							, decode(Person.Gebdatum, to_date('0111.01.01','YYYY.MM.DD'), NULL, Person.Gebdatum)  AS Birthday
                            , person.privatflag
							, SUBSTR (TRIM (Hobby.Nationalitaet), 0, 31) AS Nationality
							,  (SELECT MIN (dataktiv) FROM vt INNER JOIN person kd ON vt.syskd = kd.sysperson WHERE vt.dataktiv > to_date('01.01.1900' , 'dd.MM.yyyy') AND kd.SYSPerson = :sysperson GROUP BY kd.sysperson) AS Since
							,  0 AS BMWEmployeeAsInt
							,  CASE WHEN (Person.privatflag = 0 AND person.gebdatum > to_date('01.01.1900' , 'dd.MM.yyyy')) THEN 1 ELSE 0 END AS SelfEmployedAsInt
							, decode(Person.gruendung, to_date('0111.01.01','YYYY.MM.DD'), NULL, Person.gruendung) AS CompanyFoundingDate
							, SUBSTR (TRIM (Person.rechtsform), 0, 61) AS CompanyLegalForm
							, SUBSTR (TRIM (Person.identeg), 0, 41) AS CompanyTaxID
							, SUBSTR (TRIM (Hobby.arbeitgeber), 0, 81) AS Employer
				            , (SELECT Count (*) FROM VT WHERE vt.syskd = :sysperson AND VT.Dataktiv > to_date('01.01.1900' , 'dd.MM.yyyy') AND VT.Endeam > to_date('01.01.1900' , 'dd.MM.yyyy')) AS NumberFormerContracts
							, Person.sclearingflag AS InvoicesSummaryAsInt
							, trim (Person.plz) AS PostCode
							, trim (Person.ort) AS City
							, Person.sysland AS Country
							, Person.SYSPerson AS PhoneReference
							, Person.identeg AS PhoneType
					        , CASE WHEN LENGTH(Person.telefon) > 0 THEN Person.telefon ELSE (CASE WHEN LENGTH(Person.handy) > 0 THEN Person.handy ELSE Person.ptelefon END) END AS PhoneNumber

							, (SELECT CASE WHEN count(*) > 0 THEN 1 ELSE 0 END FROM
								(SELECT ausfall.sysausfall
									FROM person, ausfall, VT WHERE person.sysperson = :sysperson 
									  AND ausfall.sysausfalltyp = 4
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid 
                                      AND Person.SYSPerson = vt.syskd
									  AND (ausfall.bis <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.bis > SYSDATE OR Ausfall.bis IS NULL)
									ORDER BY ausfall.sysausfall DESC
								)WHERE ROWNUM = 1) InsolventAsInt

							, (SELECT CASE WHEN InsolvencyStartDate > to_date('1900.01.01','YYYY.MM.DD') THEN InsolvencyStartDate ELSE NULL END FROM 
								  (SELECT ausfall.beginnam AS InsolvencyStartDate
									FROM person, ausfall, VT WHERE person.sysperson = :sysperson 
									  AND ausfall.sysausfalltyp = 4
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid 
									  AND Person.SYSPerson = vt.syskd
									  AND (ausfall.bis <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.bis > SYSDATE OR Ausfall.bis IS NULL)
									ORDER BY ausfall.sysausfall DESC
									)WHERE ROWNUM = 1) InsolvencyStartDate

							, (SELECT CASE WHEN count(*) > 0 THEN 1 ELSE 0 END FROM
								(SELECT ausfall.sysausfall
									FROM person, ausfall, VT WHERE person.sysperson = :sysperson 
									  AND ausfall.sysausfalltyp = 7
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid 
                                        AND Person.SYSPerson = vt.syskd
									  AND (ausfall.endeam <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.endeam > SYSDATE OR Ausfall.endeam IS NULL)
									ORDER BY ausfall.sysausfall DESC
								)WHERE ROWNUM = 1) DeceasedAsInt

							, (SELECT CASE WHEN DeceasedCaseStarted > to_date('1900.01.01','YYYY.MM.DD') THEN DeceasedCaseStarted ELSE NULL END FROM 
								  (SELECT ausfall.beginnam AS DeceasedCaseStarted
									FROM person, ausfall, VT
									WHERE person.sysperson = :sysperson 
									  AND ausfall.sysausfalltyp = 7
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid AND Person.SYSPerson = vt.syskd
									  AND (ausfall.endeam <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.endeam > SYSDATE OR Ausfall.endeam IS NULL)
									ORDER BY ausfall.sysausfall DESC
									)WHERE ROWNUM = 1) DeceasedCaseStarted

							, (SELECT CASE WHEN InsolventFollowupDate > to_date('1900.01.01','YYYY.MM.DD') THEN InsolventFollowupDate ELSE NULL END FROM 
								  (SELECT wfmmemo.dat01 AS InsolventFollowupDate
									FROM person, ausfall, VT, wfmmemo
									WHERE person.sysperson = :sysperson 
  									  AND wfmmemo.syslease = Ausfall.sysausfall
									  AND wfmmemo.syswfmtable = 220
									  AND ausfall.sysausfalltyp = 4
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid AND Person.SYSPerson = vt.syskd
									  AND (ausfall.bis <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.bis > SYSDATE OR Ausfall.bis IS NULL)
									ORDER BY ausfall.sysausfall DESC
									)WHERE ROWNUM = 1) InsolventFollowupDate

							, (SELECT CASE WHEN DeceasedFollowupDate > to_date('1900.01.01','YYYY.MM.DD') THEN DeceasedFollowupDate ELSE NULL END FROM 
								  (SELECT wfmmemo.dat01 AS DeceasedFollowupDate
									FROM person, ausfall, VT, wfmmemo
									WHERE person.sysperson = :sysperson 
  									  AND wfmmemo.syslease = Ausfall.sysausfall
									  AND wfmmemo.syswfmtable = 220
									  AND ausfall.sysausfalltyp = 7
									  AND ausfall.area = 'VT' AND vt.zustand NOT IN ('UEBERNAHME / NICHT AKTIV','FEHLER','STORNO','IRRTÜMLICH ANGELEGT', 'ÜBERNOMMEN','VERLAENGERT / NICHT AKTIV','GEPRUEFT / NICHT AKTIV')	
									  AND ausfall.sysid = vt.sysid AND Person.SYSPerson = vt.syskd
									  AND (ausfall.endeam <= to_date('01.01.0111','dd.mm.yyyy') OR ausfall.endeam > SYSDATE OR Ausfall.endeam IS NULL)
									ORDER BY ausfall.sysausfall DESC
									)WHERE ROWNUM = 1) DeceasedFollowupDate

							, person.FLAGLU19 AS IrregularitiesSuspectedAsInt
							, trim (person.mahngruppe) AS SpecialAccount

							, (SELECT * FROM (SELECT NVL (ROUND (klinie.limitextern, 2), 0.0) FROM hdklinie, klinie 
								WHERE hdklinie.SYSHD = :sysperson AND hdklinie.SYSKLINIE = klinie.SYSKLINIE AND klinie.GESPERRT = 0 
								ORDER BY klinie.sysklinie DESC) WHERE ROWNUM = 1) AS CreditLimit
							, (SELECT * FROM (SELECT NVL (ROUND (klinie.auslastung, 2), 0.0) FROM hdklinie, klinie 
								WHERE hdklinie.SYSHD = :sysperson AND hdklinie.SYSKLINIE = klinie.SYSKLINIE AND klinie.GESPERRT = 0 
								ORDER BY klinie.sysklinie DESC) WHERE ROWNUM = 1) AS ClaimedCreditLimit
 							, ROUND (peoption.PDEC901, 2) AS Rating
							, SUBSTR (TRIM (peoption.option11), 0, 257) AS EmailAddress
						FROM PERSON, Hobby, ausfall, peoption  
						WHERE Person.SYSPerson = :sysperson 
						  AND Hobby.syshobby (+) = Person.SYSPerson 
						  AND ausfall.sysid (+) = Person.SYSPerson
						  AND peoption.SYSID (+) = Person.SYSPerson
					";


					//————————————————————————————————————————————————— 
					// ATTENTION!  
					//————————————————————————————————————————————————— 
					// The following query returns multiple datasets due to different wfmmemo.dat01-dates in different VTs to a specific personID

					// object[] pars = { new OracleParameter { ParameterName = "SYSPerson", Value = personID } };
					List<OracleParameter> pars = new List<OracleParameter> ();
					pars.Add (new OracleParameter { ParameterName = "sysperson", Value = personID });
					person = ctx.ExecuteStoreQuery <ScoreCustomerDto> (QUERY_PERSON, pars.ToArray ()).FirstOrDefault ();

					List<OracleParameter> parsf = new List<OracleParameter>();
                    parsf.Add(new OracleParameter { ParameterName = "sysperson", Value = personID });
                    int privatflag = ctx.ExecuteStoreQuery<int>("select privatflag from person where sysperson=:sysperson", parsf.ToArray()).FirstOrDefault();
                    
                    person.CompanyName = null;
                    if (privatflag == 0 && person.Birthday == null)
                    {
                        person.CompanyName = person.Surname;
                        if (person.Forename != null)
                            person.CompanyName += " " + person.Forename;
                        if (person.MiddleName != null)
                            person.CompanyName += " " + person.MiddleName;
                    }

					//——————————————————————————————————————————————
					// the Address 
					//——————————————————————————————————————————————
					const String QUERY_PERSON_ADDRESS =
						@"SELECT SUBSTR (TRIM (person.strasse), 0, 51) AS Street
						  , SUBSTR (TRIM (person.plz), 0, 16) AS PostCode
						  , SUBSTR (TRIM (person.ort), 0, 41) AS City
						  , person.sysland As Country
						  FROM PERSON, LAND
						  WHERE SYSPERSON = :sysperson
							AND PERSON.SYSLAND = LAND.SYSLAND(+) ";

					List<OracleParameter> pars2 = new List<OracleParameter> ();
					pars2.Add (new OracleParameter { ParameterName = "sysperson", Value = personID });
					ScoreAddressDto addressTmp = ctx.ExecuteStoreQuery<ScoreAddressDto> (QUERY_PERSON_ADDRESS, pars2.ToArray ()).FirstOrDefault ();

					person.Address = new ScoreAddressDto ();
					person.Address.Street = addressTmp.Street;
					person.Address.PostCode = addressTmp.PostCode;
					person.Address.City = addressTmp.City;
					person.Address.Country = addressTmp.Country;
					// string CountryName = addressTmp.CountryName; 

					//———————————————————————————————————————————————————————————————————
					// rh 20170906: 
					// sending FIXED Values in variable fields does NOT make ANY sense,
					// BUT due to Tallyman PRBs AND "special wishes" from Florian Zeitler 
					// we will send what they desire:
					//———————————————————————————————————————————————————————————————————
                    person.Address.AddressType = "Home";        // MaxLen = 100
					//———————————————————————————————————————————————————————————————————

					pars2 = new List<OracleParameter>();
                    pars2.Add(new OracleParameter { ParameterName = "sysperson", Value = personID });

					// single hardcoded phone nr HOME:
					//const String QUERY_PHONE = @"SELECT 
					//		Person.SYSPerson AS PhoneReference
					//		, 'Home' AS PhoneType
					//		, CASE WHEN LENGTH(TRIM (Person.telefon)) > 0 THEN trim (Person.telefon) ELSE (CASE WHEN LENGTH(TRIM (Person.handy)) > 0 THEN trim (Person.handy) ELSE trim (Person.ptelefon) END) END AS PhoneNumber
					//		, TO_DATE('01012017', 'DDMMYYYY') phone_valid_from
					//        , TO_DATE('31122099', 'DDMMYYYY') phone_valid_to
					//	FROM PERSON
					//	WHERE Person.SYSPerson = :sysperson";
					// person.Phone = ctx.ExecuteStoreQuery<ScorePhoneDto> (QUERY_PHONE, pars2.ToArray ()).FirstOrDefault ();

					// phone nr - array:
					ScorePhoneDto[] personPhoneTESTER = new ScorePhoneDto[3];
					personPhoneTESTER[0] = new ScorePhoneDto ();

					pars2 = new List<OracleParameter> ();
					pars2.Add (new OracleParameter { ParameterName = "sysperson", Value = personID });

					personPhoneTESTER[0] = ctx.ExecuteStoreQuery<ScorePhoneDto> (@"SELECT 
							Person.SYSPerson AS PhoneReference
							, 'Work' AS PhoneType
					        , TRIM (Person.telefon) AS PhoneNumber
					        , TO_DATE('01012017', 'DDMMYYYY') phone_valid_from
                            , TO_DATE('31122099', 'DDMMYYYY') phone_valid_to
						FROM PERSON
						WHERE Person.SYSPerson = :sysperson", pars2.ToArray ()).FirstOrDefault ();

					personPhoneTESTER[1] = new ScorePhoneDto ();

					pars2 = new List<OracleParameter> ();
					pars2.Add (new OracleParameter { ParameterName = "sysperson", Value = personID });

					personPhoneTESTER[1] = ctx.ExecuteStoreQuery<ScorePhoneDto> (@"SELECT 
							Person.SYSPerson AS PhoneReference
							, 'Mobile' AS PhoneType
					        , TRIM (Person.handy) AS PhoneNumber
					        , TO_DATE('01012017', 'DDMMYYYY') phone_valid_from
                            , TO_DATE('31122099', 'DDMMYYYY') phone_valid_to
						FROM PERSON
						WHERE Person.SYSPerson = :sysperson", pars2.ToArray ()).FirstOrDefault ();

					personPhoneTESTER[2] = new ScorePhoneDto ();

					pars2 = new List<OracleParameter> ();
					pars2.Add (new OracleParameter { ParameterName = "sysperson", Value = personID });

					personPhoneTESTER[2] = ctx.ExecuteStoreQuery<ScorePhoneDto> (@"SELECT 
							Person.SYSPerson AS PhoneReference
							, 'Home' AS PhoneType
					        , TRIM (Person.ptelefon) AS PhoneNumber
					        , TO_DATE('01012017', 'DDMMYYYY') phone_valid_from
                            , TO_DATE('31122099', 'DDMMYYYY') phone_valid_to
						FROM PERSON
						WHERE Person.SYSPerson = :sysperson", pars2.ToArray ()).FirstOrDefault ();


					int cnt = 0;
					for (int i = 0; i < 3; i++)
					{
						if (personPhoneTESTER[i].PhoneNumber != null)
							cnt++;
					}

					person.Phone = new ScorePhoneDto[cnt];

					int idx = 0;
					for (int i = 0; i < 3; i++)
					{
						if (personPhoneTESTER[i].PhoneNumber != null)
						{
							person.Phone[idx] = new ScorePhoneDto
							{
								PhoneReference = personPhoneTESTER[i].PhoneReference,
								PhoneType = personPhoneTESTER[i].PhoneType,
								PhoneNumber = personPhoneTESTER[i].PhoneNumber,
								Phone_Valid_From = personPhoneTESTER[i].Phone_Valid_From,
								Phone_Valid_To = personPhoneTESTER[i].Phone_Valid_To
							};
							idx++;
						}
					}

					// do *NOT* re-encode as we do perfectly right encoding already (deferred! rh 20171123)
					//		otherwise ß, ö, ä, ÿ and ofc ü would not work anymore!
					// AsserUtf8Encoding (ref person);         // assert encoding from iso to UTF-8 in all relevant fields in table person (rh 20171122)

				}
				catch (Exception e)
				{// person information is NOT available

					string x = e.Message;
					throw e;					// continue routing for error code handling
					// _log.Info ("Processing SCORE::Posting data delivered " + e.Message, e);
				}		
			}

			return person;

		}

		/// <summary>
		/// Re-Encode Helper due to UTF-8 / UTF-16 PRBs on BMW-DB migrations
		/// Recodes concerning fields: Forename, Surname, Street, City
		/// mb+rh 20171122
		/// </summary>
		/// <param name="person"></param>
		private void AsserUtf8Encoding (ref ScoreCustomerDto person)
		{
			Encoding enc = Encoding.GetEncoding ("ISO-8859-1");

			person.Surname = RecodeAccent (person, person.Surname, enc);
			person.Forename = RecodeAccent (person, person.Forename, enc);
			person.Address.Street = RecodeAccent (person, person.Address.Street, enc);
			person.Address.City = RecodeAccent (person, person.Address.City, enc);
		}

		/// <summary>
		/// Re-Encode Helper due to UTF-8 / UTF-16 PRBs with ACCENTS 
		/// SWAPS ACCENT "´" to "'" ONLY
		/// mb+rh 20171122
		/// </summary>
		/// <param name="scDto"></param>
		/// <param name="dbStringField"></param>
		/// <param name="currentEncoding"></param>
		/// <returns>converted string</returns>
		private string RecodeAccent (ScoreCustomerDto scDto, string dbStringField, Encoding currentEncoding)
		{
			if (scDto != null && dbStringField != null)
			{
				// Straight REPLACE ON Accent ONLY
				dbStringField = dbStringField.Replace ("´", "'");


				// rh HINT: disabling upper line we could log any changes down here
				int foundAt = dbStringField.IndexOf ('´');
				if (foundAt >= 0)
				{
					// rh HINT: thread will never come here!
					StringBuilder sb = new StringBuilder (dbStringField);
					sb[foundAt] = '\'';
					dbStringField = sb.ToString ();
				}

			}
			return dbStringField;
		}


		/// <summary>
		/// Re-Encode Helper due to UTF-8 / UTF-16 PRBs with accents 
		/// 
		/// ATTENTION! Destroys all other "special" characters as ß, ä, ö, ...
		/// 
		/// mb+rh 20171122
		/// </summary>
		/// <param name="scDto"></param>
		/// <param name="dbStringField"></param>
		/// <param name="currentEncoding"></param>
		/// <returns>converted string</returns>
		private string RecodeDEFECT (ScoreCustomerDto scDto, string dbStringField, Encoding currentEncoding)
		{
			if (scDto != null && dbStringField != null)
				dbStringField = Encoding.Default.GetString (
					Encoding.Convert (currentEncoding, Encoding.UTF8, Encoding.Default.GetBytes (dbStringField))).ToString ();
			return dbStringField;
		}
	}
}