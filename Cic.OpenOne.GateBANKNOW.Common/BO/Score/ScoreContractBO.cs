using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
	public class ScoreContractBO
	{
		private static String QUERY_CONTRACT_STRING = @"SELECT vt.syskd AS CustomerReference
        ,vt.sysid AS ContractNumber
        ,vt.Vertrag AS ContractReference
        ,VT.SysLS AS MandatorID
        ,DECODE (VT.Sysvart, 100, 'lease', 200, 'loan', 500, 'service', 'No VArt') AS ContractType
        ,SUBSTR (TRIM (VT.Zustand), 0, 26) AS CMSStatus
        ,VT.einzug AS DDPossible
        ,VT.SysLF AS BusinessLine
        ,CASE WHEN (vt.VART LIKE '%gewerblich%') THEN 1 ELSE 0 END AS PrivateBusinessIndicator
		,CASE WHEN (VT.Dateinreichung > to_date('1900.01.01','YYYY.MM.DD')) THEN VT.Dateinreichung ELSE null END AS ContractApplicationDate
		,CASE WHEN (VT.Beginn > to_date('1900.01.01','YYYY.MM.DD')) THEN VT.Beginn ELSE null END AS ContractStartDate
		,CASE WHEN (VT.Ende > to_date('1900.01.01','YYYY.MM.DD')) THEN VT.Ende ELSE null END AS ContractEndDate
		,SUBSTR (TRIM (VT.VART), 0, 100) AS ProductType
		,CASE WHEN (vt.sysvttyp IN (38, 535, 32, 529, 33, 530)) THEN '1' ELSE '0' END AS LoanContractType
        ,CASE WHEN ((SELECT count(*) FROM vtobsl WHERE sysvt = VT.sysid AND rang >= 4000 AND rang  < 5000 AND inaktiv <> 1) > 0) THEN 1 ELSE 0 END AS Insurance
        ,vt.bgextern AS ContractOriginalValue
        ,vt.LZ AS ContractTerm
        ,vt.syswaehrung AS Currency
        ,ROUND (vt.rate * (mwst.prozent/100 + 1.0), 2) AS RegularInstalmentAmountGross
        ,vt.RW AS BalloonRateAmount
        ,CASE WHEN (VT.Ende > to_date('1900.01.01','YYYY.MM.DD')) THEN VT.Ende ELSE null END AS BalloonRateDate
		,CASE WHEN (VT.endeam > to_date('1900.01.01','YYYY.MM.DD')) THEN VT.endeam ELSE null END AS FinalInvoiceDate
        ,OB.jahreskm AS ContractMileage
        -- , (SELECT CASE WHEN (vt.syskd = obvwrt.syskaeufer) THEN 1 ELSE 0 END AS PurchaseFlagCustomer FROM vt, obvwrt WHERE vt.sysid = obvwrt.sysobvwrt AND VT.VERTRAG = :contractReference) AS PurchaseFlagCustomer
        , vt.rw AS AgreedResidualValue
        , SUBSTR (Konto.IBAN, 0, 35) AS IBAN
        -- , VTMAHN.Mahndatum AS DunningActivityDate
		, ROUND (TO_NUMBER (REPLACE (VTOPTION.option10, '.', (SELECT SUBSTR (VALUE, 1, 1) FROM nls_session_parameters WHERE parameter = 'NLS_NUMERIC_CHARACTERS')))) AS ApplicationRating
        -- , VTOPTION.Flag03 AS SpecialCaseTermination
        -- , OBVWRT.Gutachten AS DateOfSale
        , SUBSTR (TRIM (VTKUEND.Grund), 0, 100) AS FinalInvoiceReason
        -- , vtoption.pdec1501 AS WriteOffAmount
		, SUBSTR (peoption.str01, 0, 21) AS PrefComChannel  
        , CASE WHEN (vtobsich.Rang >= 200) THEN 1 ELSE 0 END AS DealerCommitmentToBuyback
		-- , Person.Code AS DealerNumber
		, VT.SYSVPFIL AS DealerNumber
        , person.zahlmodus AS PaymentTargetInvoice
        -- , SUBSTR (TRIM (person.vornamegen), 0, 41) AS PaymentTargetRegularInvoice
        , SUBSTR (TRIM (person.vornamegen), -2, 2) AS PaymentTargetRegularInvoice
      FROM VT, vtobsl, VTMAHN, Konto, vtoption, OB, obvwrt, VTKUEND, peoption, vtobsich, PERSON, vttyp, mwst
      WHERE VT.VERTRAG = :contractReference 
        AND mwst.sysmwst = vt.sysmwst
        AND vtobsl.sysvt (+) = VT.sysid 
		AND VTOBSL.Endfaellig (+) > to_date('1900.01.01','YYYY.MM.DD')
        AND VTMAHN.sysvtmahn (+) = vt.sysid
        AND Konto.Sysperson (+) = vt.syskd
		AND konto.rang (+) = VT.RangKI
        AND vtoption.sysid (+) = vt.sysid
        AND ob.sysvt (+) = VT.sysid 
        AND obvwrt.sysobvwrt (+) = vt.sysid 
        AND VTKUEND.SYSVTKUEND (+) = VT.sysid
        AND peoption.SYSID (+) = VT.syskd
        AND vtobsich.SYSVT (+) = VT.sysid
        AND VT.SYSKD (+) = PERSON.SYSPERSON
		AND vttyp.sysvttyp (+) = vt.sysvttyp";
		/// <summary>
		/// falls contractReference wirklich ein String ist:
		/// Umwandlung
		/// </summary>
		/// <param name="contractReference"></param>
		/// <returns></returns>
		public long getContractID (String contractReference)
		{
			long id = 0;
			bool bOK = long.TryParse (contractReference, out id);       // ID: from string to long
			return id;
		}

		/// <summary>
		/// read contract-information auslesen
		/// ATTENTION: lis reading vehicle data, too!
		/// </summary>
		/// <param name="contractReference">contract reference</param>
		/// <returns></returns>
		// public ScoreContractDto getContract (long customerReference, long contractReference)
		public ScoreContractDto getContractFromDB (string contractReference)
		{
			////————————————————————————————————————————————————————————————————————————————————————————
			//// RH 20170913: 
			//// TEST - Interface for determining NullReferences and/or Errors in SQL - statements 
			////————————————————————————————————————————————————————————————————————————————————————————
			//try
			//{
			//	bool bTEST = true;
			//	if (bTEST)
			//	{
			//		string theFile = @"c:\temp\SqlTestText.txt";
			//		if (System.IO.File.Exists (theFile))
			//			QUERY_CONTRACT_STRING = System.IO.File.ReadAllText (theFile);
			//	}
			//}
			//catch { }
			////————————————————————————————————————————————————————————————————————————————————————————

			ScoreContractDto contract = null;

			using (DdOlExtended ctx = new DdOlExtended ())
			{
				contract = new ScoreContractDto ();
				
				try
				{// Get the instance information 

					// ASSERT: check existing
					if (string.IsNullOrEmpty (contractReference))
					{
						throw new ArgumentException ("An EMPTY Contract number is NOT valid!");
					}

					object[] parsv = { new Devart.Data.Oracle.OracleParameter { ParameterName = "contractReference", Value = contractReference } };
					int counter = ctx.ExecuteStoreQuery<int> ("SELECT Count(*) FROM VT WHERE VT.VERTRAG = :contractReference", parsv).FirstOrDefault ();
					if (counter <= 0)
					{
						throw new ArgumentException ("Contract with number " + contractReference + " not found!");
					}
					
					// GET ALL Person Info 
					object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "contractReference", Value = contractReference } };
					contract = ctx.ExecuteStoreQuery<ScoreContractDto> (QUERY_CONTRACT_STRING, pars.ToArray ()).FirstOrDefault ();

					if (contract != null)
					{
						contract.CustomerRelationshipType = "MAIN_APPLICANT";
					}
				}
				catch (Exception e)
				{// person information is NOT available
					string x = e.Message;
					// _log.Info ("Processing SCORE::Posting data delivered " + e.Message, e);
					throw e;                    // continue routing for error code handling
				}
			}
			return contract;
		}
	}
}