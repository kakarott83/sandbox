using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Score
{
    public class ScoreEntityDao
	{
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);

		/// <summary>
		/// save string to long converter
		/// </summary>
		/// <param name="strVar"></param>
		/// <param name="VarName"></param>
		/// <returns></returns>
		private long getLongID (string strVar, string VarName)
		{
			long lVar = 0;
			if (!long.TryParse (strVar, out lVar))
			{
				_log.Info ("Could NOT determine long ID for " + VarName + " from " + strVar);
			}
			return lVar;
		}

		/// <summary>
		/// date converter helper
		/// </summary>
		/// <param name="nullable"></param>
		/// <returns>the date as string</returns>
		private string getDateString (DateTime? nullable)
		{
			string tmpVar = string.Empty;

			if (nullable.HasValue)
				tmpVar = String.Format ("{0:yyyy-MM-dd}", nullable.Value);
			return tmpVar;
		}

		/// <summary>
		/// converter string to Double with default-return on exception
		/// </summary>
		/// <param name="strInput"></param>
		/// <param name="dDefault"></param>
		/// <returns></returns>
		public static double ToDouble (string strInput, double dDefault)
		{
			double rval;

			if (string.IsNullOrEmpty (strInput))
				return dDefault;				// fallback

			try
			{
				strInput = strInput.Replace (",", ".");
				rval = double.Parse (strInput, System.Globalization.CultureInfo.InvariantCulture);
			}
			catch
			{
				rval = dDefault;
			}

			return rval;
		}

		private EAIQIN createEaiQin (long syseaihot, string value, string data)
		{
			EAIQIN eaiqinInput = new EAIQIN ();
			eaiqinInput.SYSEAIHOT= syseaihot;
			eaiqinInput.F01 = value;
			eaiqinInput.F02 = data;
			return eaiqinInput;
		}

		/// <summary>
		/// Bestehendes Eaihot holen
		/// </summary>
		/// <param name="syseaihot">Primary Key</param>
		/// <returns>Eaihot Ausgang</returns>
		public EaihotDto getEaihot (long syseaihot)
		{
			EaihotDto rval = null;
			using (DdOwExtended owCtx = new DdOwExtended ())
			{
				EAIHOT eaihotOutput = (from e in owCtx.EAIHOT
									   where e.SYSEAIHOT == syseaihot
									   select e).FirstOrDefault ();

				if (eaihotOutput != null)
				{

					rval = Mapper.Map<EAIHOT, EaihotDto> (eaihotOutput);
					
				}
			}
			return rval;
		}


		/// <summary>
		/// GET Contract Precalcuölated Fields
		/// </summary>
		/// <param name="owCtx"></param>
		/// <param name="contractReference"></param>
		/// <returns>Eaihot Ausgang</returns>
		public EaihotDto getContractPrecalcFields (DdOwExtended owCtx, string contractReference)
		{

			// 1. INPUTPARAMETER1 = VT-REFERENCE   
			// 2. OLTABLE = SYSTEM (NOT VT!)
			// 3. GET LATEST SCORE_CREATE_COL_CASE !!!>>>OR<<<!!! SCORE_UPDATE_COL_CASE
			//		LATEST = MAX (syseaihot
			//			IF NOT FOUND --> RETURN 0  (==no PRE-CALCULATED FIELDS available)

			object[] pars2 = { new Devart.Data.Oracle.OracleParameter { ParameterName = "contractReference", Value = contractReference } };

			string sqlQuery = @"SELECT * FROM (SELECT * FROM eaihot
				 WHERE inputparameter1 = :contractReference
				 AND CODE IN ('SCORE_CREATE_COL_CASE', 'SCORE_UPDATE_COL_CASE')
				 ORDER BY syseaihot DESC)
			WHERE ROWNUM = 1";

			EaihotDto rval = null;
			EAIHOT eaihotOutput = owCtx.ExecuteStoreQuery<EAIHOT>(sqlQuery, pars2).FirstOrDefault ();

			if (eaihotOutput != null)
			{
				rval = Mapper.Map<EAIHOT, EaihotDto> (eaihotOutput);
			}
			return rval;
		}




		/// <summary>
		/// Prüfung auf Timeout
		/// </summary>
		/// <param name="oldDate">Alter Zeitstempel</param>
		/// <param name="timeOut">NeuerZeitstempel</param>
		/// <returns>Timeout true= Timeout/false = kein Timeout</returns>
		public static bool isTimeOut (DateTime oldDate, TimeSpan timeOut)
		{
			TimeSpan ts = DateTime.Now - oldDate;

			if (ts > timeOut)
				return true;

			return false;
		}

		/// <summary>
		/// ASSERT Person exists in DB
		/// </summary>
		/// <param name="owCtx"></param>
		/// <param name="customerID"></param>
		/// <returns></returns>
		private bool PersonNotFound (DdOwExtended owCtx, long customerID)
		{
			// ASK customerID OK: SQL QUERY ...
			return 0 >= owCtx.ExecuteStoreQuery<int> ("SELECT Count(*) FROM PERSON WHERE Person.SYSPerson = " + customerID).FirstOrDefault ();
		}
		/// <summary>
		/// ASSERT Contract exists in DB
		/// </summary>
		/// <param name="owCtx"></param>
		/// <param name="ContractReference"></param>
		/// <returns></returns>
		private bool ContractNotFound (DdOwExtended owCtx, string ContractReference)
		{
			// ASK contractID OK: SQL QUERY ...
			object[] pars2 = { new Devart.Data.Oracle.OracleParameter { ParameterName = "contractReference", Value = ContractReference } };
			return 0 >= owCtx.ExecuteStoreQuery<int> ("SELECT Count(*) FROM VT WHERE VT.VERTRAG = :contractReference", pars2).FirstOrDefault ();
		}


		/// <summary>
		/// GET Contract 
		/// </summary>
		/// <param name="contractDto"></param>
		/// <returns>DTO.ScoreContractDto</returns>
		public void getContract (ref DTO.ogetContractDto contractDto)
		{
			bool bOLCalcError = false;
			string RETURNVALUE_DUMMY = string.Empty;

			try
			{

				// Cic.OpenOne.Common.Model.DdOw
				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					
					if (contractDto.Contract == null)
					{
						throw new ArgumentException ("Dto.Contract EMPTY!");
					}

					if (string.IsNullOrEmpty (contractDto.Contract.ContractReference))
					{
						throw new ArgumentException ("ContractReference EMPTY!");
					}

					if (ContractNotFound (owCtx, contractDto.Contract.ContractReference))
						throw new ArgumentException ("Contract with id " + contractDto.Contract.ContractReference + " not found!");
					

					// —————————————————————————————————————————————————————————————————————
					// FAST INTERFACE WITHOUT WAIT FOR RESPONSE
					// —————————————————————————————————————————————————————————————————————
					// just ASK the LATEST PRE-CALCULATED output params 
					// 1. INPUTPARAMETER1 = VT-REFERENCE   
					// 2. OLTABLE = SYSTEM (NOT VT!)
					// 3. GET LATEST SCORE_CREATE_COL_CASE !!!>>>OR<<<!!! SCORE_UPDATE_COL_CASE
					//			IF NOT FOUND --> RETURN 0  (==no PRE-CALCULATED FIELDS available)
					// 4. 
					// —————————————————————————————————————————————————————————————————————

					EaihotDto eaihotOutput = getContractPrecalcFields (owCtx, contractDto.Contract.ContractReference);
					
					if (eaihotOutput != null)		
					{
					
						// Fehler-Handling: ON String "FEHLER" in Output-Para 1 indicates OL-calculation error 
						if (eaihotOutput.OUTPUTPARAMETER1 != null)
						{
							if (eaihotOutput.OUTPUTPARAMETER1 == "FEHLER")
							{
								bOLCalcError = true;
								RETURNVALUE_DUMMY = eaihotOutput.OUTPUTPARAMETER1;
								contractDto.Contract.ArrearsBalance = 0.0;		// fallback
							}
							else
							{
								contractDto.Contract.ArrearsBalance = ToDouble (eaihotOutput.OUTPUTPARAMETER1, 0.0);
							}
						}
						else
						{
							contractDto.Contract.ArrearsBalance = 0.0;		// fallback
						}

						if (!bOLCalcError)
						{
							contractDto.Contract.RemainingBalance = ToDouble (eaihotOutput.OUTPUTPARAMETER2, 0.0);
							contractDto.Contract.LateInterest = ToDouble (eaihotOutput.OUTPUTPARAMETER3, 0.0);
							contractDto.Contract.SettlementAmountForTermination = ToDouble (eaihotOutput.OUTPUTPARAMETER4, 0.0);
						}
					}
					else
					{
						// return 0.0 for all precalculated fields as there are no precalculated fields
						contractDto.Contract.ArrearsBalance = 0.0;
						contractDto.Contract.RemainingBalance =  0.0;
						contractDto.Contract.LateInterest =  0.0;
						contractDto.Contract.SettlementAmountForTermination = 0.0;
					}


					if (RETURNVALUE_DUMMY != null && RETURNVALUE_DUMMY.Length > 0)
					{
						throw new Exception (RETURNVALUE_DUMMY);
					}
					//————————————————————————————————————————————————————————————
				}

			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::getContract data delivered " + e.Message, e);
				RETURNVALUE_DUMMY = e.Message;
				throw e;

			}
 
		}




		/// <summary>
		/// updates/creates DunningLevel (nach createOrUpdateHEKOb)
		/// </summary>
		/// <param name="dunningLevelDto"></param>
		/// <returns></returns>
		public void createOrUpdateDunningLevel (DTO.ScoreDunningLevelDto dunningLevelDto)
		{
			string ANY_dunningLevelDto_RETURNVALUE_DUMMY = string.Empty;

			try
			{
				 
				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					 
					long customerID = getLongID (dunningLevelDto.CustomerReference, "CustomerReference");
					
					if (PersonNotFound (owCtx, customerID))
						throw new ArgumentException ("Person with id " + customerID + " not found!");

					
					if (dunningLevelDto == null)
					{
						throw new ArgumentException ("dunningLevelDto EMPTY!");
					}

					if (string.IsNullOrEmpty (dunningLevelDto.ContractReference))
					{
						throw new ArgumentException ("ContractReference EMPTY!");
					}
					

					if (ContractNotFound (owCtx, dunningLevelDto.ContractReference))
						throw new ArgumentException ("Contract with id " + dunningLevelDto.ContractReference + " not found!");

					//——————————————————————————————————————————————————————————————
					// SET eaihotInput
					//——————————————————————————————————————————————————————————————
					EAIHOT eaihotInput = new EAIHOT ();
					eaihotInput.CODE = "SCORE_UPDATE_DUN_LEVEL";					
					eaihotInput.OLTABLE = "VT";					
					eaihotInput.SYSOLTABLE = 0;
					eaihotInput.PROZESSSTATUS = 0;
					eaihotInput.HOSTCOMPUTER = "";
					eaihotInput.CLIENTART = 0;
					eaihotInput.EVE = 0;					// FLAG: do NOT process YET!

					eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));

					
					owCtx.EAIHOT.Add(eaihotInput);
					eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
										  where EaiArt.CODE == eaihotInput.CODE
										  select EaiArt).FirstOrDefault ();
					

					owCtx.SaveChanges ();	

					long syseaihot = eaihotInput.SYSEAIHOT;

					// lt. ScoreService-Name Spalte
					List<EAIQIN> eaiqins = new List<EAIQIN> ();
					eaiqins.Add (createEaiQin ( syseaihot, "CustomerReference", dunningLevelDto.CustomerReference));
					eaiqins.Add (createEaiQin ( syseaihot, "ContractReference", dunningLevelDto.ContractReference));
					eaiqins.Add (createEaiQin ( syseaihot, "InvoiceReference", dunningLevelDto.InvoiceReference));
					eaiqins.Add (createEaiQin ( syseaihot, "DunningLevel", "" + dunningLevelDto.DunningLevel));
					
					// double
					eaiqins.Add (createEaiQin ( syseaihot, "DunningFee", dunningLevelDto.DunningFee.ToString (CultureInfo.InvariantCulture)));

					eaiqins.Add (createEaiQin ( syseaihot, "DunningActivityDate", getDateString (dunningLevelDto.DunningActivityDate)));	// DateTime?
					eaiqins.Add (createEaiQin ( syseaihot, "DunningHoldFlag", "" + (dunningLevelDto.DunningHoldFlag?1:0)));						// bool
					eaiqins.Add (createEaiQin ( syseaihot, "DunningHoldEndDate", getDateString (dunningLevelDto.DunningHoldEndDate)));		// DateTime?
					eaiqins.Add (createEaiQin ( syseaihot, "DunningEpisodeEnd", getDateString (dunningLevelDto.DunningEpisodeEnd)));		// DateTime?

					foreach (EAIQIN eaiqinInput in eaiqins)
					{
						owCtx.EAIQIN.Add (eaiqinInput);
					}

					eaihotInput.EVE = 1;			// FLAG: process NOW!
					owCtx.SaveChanges ();

					 
				}

			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::DunningLevel data delivered " + e.Message, e);
				ANY_dunningLevelDto_RETURNVALUE_DUMMY = e.Message;
				throw e;

			}
			 
		}

		/// <summary>
		/// updates/creates Arrangement (wie createOrUpdateHEKOb)
		/// </summary>
		/// <param name="arrangementDto"></param>
		/// <returns></returns>
		public void createOrUpdateArrangement (DTO.ScoreArrangementDto arrangementDto)
		{
			string ANY_dunningLevelDto_RETURNVALUE_DUMMY = string.Empty;

			try
			{

				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					//————————————————————————————————————————————————————————————————————————
					// VORAB-PRÜFUNGEN
					//————————————————————————————————————————————————————————————————————————
					long customerID = getLongID (arrangementDto.CustomerReference, "CustomerReference");
					
					if (PersonNotFound (owCtx, customerID))
						throw new ArgumentException ("Person with id " + customerID + " not found!");

					
					if (ContractNotFound (owCtx, arrangementDto.ContractReference))
						throw new ArgumentException ("Contract with id " + arrangementDto.ContractReference + " not found!");

					//——————————————————————————————————————————————————————————————
					// SET eaihotInput
					//——————————————————————————————————————————————————————————————
					EAIHOT eaihotInput = new EAIHOT ();
					
					// ALT mit "A": bis 20181115: eaihotInput.CODE = "SCORE_UPDATE_ARRANGEMANT";
					eaihotInput.CODE = "SCORE_UPDATE_ARRANGEMENT";		// NEU mit "E" (ab 2018115)
					eaihotInput.OLTABLE = "VT";
					eaihotInput.SYSOLTABLE = 0;
					eaihotInput.PROZESSSTATUS = 0;
					eaihotInput.HOSTCOMPUTER = "";
					eaihotInput.CLIENTART = 0;
					eaihotInput.EVE = 0;					// FLAG: do NOT process YET!

					eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					owCtx.EAIHOT.Add(eaihotInput);
					eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
										  where EaiArt.CODE == eaihotInput.CODE
										  select EaiArt).FirstOrDefault ();

					owCtx.SaveChanges ();			// SAVE here to GET a new sysEaiHot

					long syseaihot = eaihotInput.SYSEAIHOT;

					// lt. ScoreService-Name Spalte
					List<EAIQIN> eaiqins = new List<EAIQIN> ();
					eaiqins.Add (createEaiQin ( syseaihot, "CustomerReference", arrangementDto.CustomerReference));
					eaiqins.Add (createEaiQin ( syseaihot, "ContractReference", arrangementDto.ContractReference));
					eaiqins.Add (createEaiQin ( syseaihot, "InvoiceReference", arrangementDto.InvoiceReference));
					eaiqins.Add (createEaiQin ( syseaihot, "ArrangementStatusFlag", arrangementDto.ArrangementStatusFlag));
					eaiqins.Add (createEaiQin ( syseaihot, "ArrangementCancellationReason", arrangementDto.ArrangementCancellationReason));

					eaiqins.Add (createEaiQin ( syseaihot, "ArrangementStartDate", getDateString (arrangementDto.ArrangementStartDate)));	// DateTime?
					eaiqins.Add (createEaiQin ( syseaihot, "ArrangementEndDate", getDateString (arrangementDto.ArrangementEndDate)));	// DateTime?
					eaiqins.Add (createEaiQin ( syseaihot, "ArrangementDetails", arrangementDto.ArrangementDetails));				// string
					eaiqins.Add (createEaiQin ( syseaihot, "PtPStatusFlag", arrangementDto.PtPStatusFlag));						// Flag as String
					
					eaiqins.Add (createEaiQin ( syseaihot, "PtPCancellationReason", arrangementDto.PtPCancellationReason));		// string
					eaiqins.Add (createEaiQin ( syseaihot, "PtPStartDate", getDateString (arrangementDto.PtPStartDate)));			// DateTime?
					eaiqins.Add (createEaiQin ( syseaihot, "PtPEndDate", getDateString (arrangementDto.PtPEndDate)));				// DateTime?

					foreach (EAIQIN eaiqinInput in eaiqins)
					{
						owCtx.EAIQIN.Add(eaiqinInput);
					}

					eaihotInput.EVE = 1;			// FLAG: process NOW!
					owCtx.SaveChanges ();
				}
			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::Arrangement data delivered " + e.Message, e);
				ANY_dunningLevelDto_RETURNVALUE_DUMMY = e.Message;
				throw e;
			}
			
		}

		/// <summary>
		/// updates/creates DDebit (wie createOrUpdateHEKOb)
		/// </summary>
		/// <param name="dDebitDto"></param>
		/// <returns></returns>
		public void createOrUpdateDDebit (DTO.ScoreDDebitDto dDebitDto)
		{

			string ANY_dunningLevelDto_RETURNVALUE_DUMMY = string.Empty;

			try
			{

				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					
					if (ContractNotFound (owCtx, dDebitDto.ContractReference))
						throw new ArgumentException ("Contract with id " + dDebitDto.ContractReference + " not found!");

					//——————————————————————————————————————————————————————————————
					// SET eaihotInput
					//——————————————————————————————————————————————————————————————
					EAIHOT eaihotInput = new EAIHOT ();
					eaihotInput.CODE = "SCORE_2ND_DD";					
					eaihotInput.OLTABLE = "VT";					
					eaihotInput.SYSOLTABLE = 0;
					eaihotInput.PROZESSSTATUS = 0;
					eaihotInput.HOSTCOMPUTER = "";
					eaihotInput.CLIENTART = 0;
					eaihotInput.EVE = 0;					// FLAG: do NOT process YET!

					eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));

					owCtx.EAIHOT.Add(eaihotInput);
					eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
										  where EaiArt.CODE == eaihotInput.CODE
										  select EaiArt).FirstOrDefault ();

					owCtx.SaveChanges ();			// SAVE here to GET a new sysEaiHot

					long syseaihot = eaihotInput.SYSEAIHOT;

					// lt. ScoreService-Name Spalte
					List<EAIQIN> eaiqins = new List<EAIQIN> ();
					eaiqins.Add (createEaiQin ( syseaihot, "ContractReference", "" + dDebitDto.ContractReference));
					eaiqins.Add (createEaiQin ( syseaihot, "DirectDebitID", "" + dDebitDto.DirectDebitID));

					foreach (EAIQIN eaiqinInput in eaiqins)
					{
						owCtx.EAIQIN.Add(eaiqinInput);
					}

					eaihotInput.EVE = 1;			// FLAG: process NOW!
					owCtx.SaveChanges ();
				}
			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::2ndDirectDebit data delivered " + e.Message, e);
				ANY_dunningLevelDto_RETURNVALUE_DUMMY = e.Message;
				throw e;
			}
			// no return-value here: return XY;
		}

		/// <summary>
		/// updates/creates Posting (wie createOrUpdateHEKOb)
		/// </summary>
		/// <param name="postingDto"></param>
		/// <returns></returns>
		public void createOrUpdatePosting (DTO.ScorePostingDto postingDto)
		{

			string ANY_dunningLevelDto_RETURNVALUE_DUMMY = string.Empty;

			try
			{

				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					//————————————————————————————————————————————————————————————————————————
					// VORAB-PRÜFUNGEN
					//————————————————————————————————————————————————————————————————————————
					
					if (ContractNotFound (owCtx, postingDto.ContractReference))
						throw new ArgumentException ("Contract with id " + postingDto.ContractReference + " not found!");

					//——————————————————————————————————————————————————————————————
					// SET eaihotInput
					//——————————————————————————————————————————————————————————————
					EAIHOT eaihotInput = new EAIHOT ();
					eaihotInput.CODE = "SCORE_POSTING";					
					eaihotInput.OLTABLE = "VT";					
					eaihotInput.SYSOLTABLE = 0;

					eaihotInput.PROZESSSTATUS = 0;
					eaihotInput.HOSTCOMPUTER = "";
					eaihotInput.CLIENTART = 0;
					eaihotInput.EVE = 0;					// FLAG: do NOT process YET!

					eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));

					owCtx.EAIHOT.Add(eaihotInput);
					eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
										  where EaiArt.CODE == eaihotInput.CODE
										  select EaiArt).FirstOrDefault ();

					owCtx.SaveChanges ();				// SAVE here to GET a new sysEaiHot

					long syseaihot = eaihotInput.SYSEAIHOT;

					// lt. ScoreService-Name Spalte
					List<EAIQIN> eaiqins = new List<EAIQIN> ();
					eaiqins.Add (createEaiQin ( syseaihot, "ContractReference", "" + postingDto.ContractReference));
					eaiqins.Add (createEaiQin ( syseaihot, "InvoiceID", postingDto.InvoiceID.ToString (CultureInfo.InvariantCulture)));
					eaiqins.Add (createEaiQin ( syseaihot, "Amount", postingDto.Amount.ToString (CultureInfo.InvariantCulture)));
					eaiqins.Add (createEaiQin ( syseaihot, "Currency", postingDto.Currency));
					eaiqins.Add (createEaiQin ( syseaihot, "PostingDate", getDateString (postingDto.PostingDate)));
					eaiqins.Add (createEaiQin ( syseaihot, "PostingCode", postingDto.PostingCode));

					foreach (EAIQIN eaiqinInput in eaiqins)
					{
						owCtx.EAIQIN.Add(eaiqinInput);
					}

					eaihotInput.EVE = 1;				// FLAG: process NOW!
					owCtx.SaveChanges ();
				}
			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::Posting data delivered " + e.Message, e);
				ANY_dunningLevelDto_RETURNVALUE_DUMMY = e.Message;
				throw e;
			}
			
		}

		/// <summary>
		/// updates/creates LateInterest (wie createOrUpdateHEKOb)
		/// </summary>
		/// <param name="lateInterestDto"></param>
		/// <returns></returns>
		public void createOrUpdateLateInterest (DTO.ScoreLateInterestDto lateInterestDto)
		{

			string ANY_dunningLevelDto_RETURNVALUE_DUMMY = string.Empty;

			try
			{

				using (DdOwExtended owCtx = new DdOwExtended ())
				{
					//————————————————————————————————————————————————————————————————————————
					// VORAB-PRÜFUNGEN
					//————————————————————————————————————————————————————————————————————————
					
					if (ContractNotFound (owCtx, lateInterestDto.ContractReference))
						throw new ArgumentException ("Contract with id " + lateInterestDto.ContractReference + " not found!");

					//——————————————————————————————————————————————————————————————
					// SET eaihotInput
					//——————————————————————————————————————————————————————————————
					EAIHOT eaihotInput = new EAIHOT ();
					eaihotInput.CODE = "SCORE_LATE_INTEREST";					
					eaihotInput.OLTABLE = "VT";					
					eaihotInput.SYSOLTABLE = 0;

					eaihotInput.PROZESSSTATUS = 0;
					eaihotInput.HOSTCOMPUTER = "";
					eaihotInput.CLIENTART = 0;
					eaihotInput.EVE = 0;					// FLAG: do NOT process YET!

					eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));
					eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime (Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate (null));

					owCtx.EAIHOT.Add(eaihotInput);
					eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
										  where EaiArt.CODE == eaihotInput.CODE
										  select EaiArt).FirstOrDefault ();

					owCtx.SaveChanges ();			// SAVE here to GET a new sysEaiHot

					long syseaihot = eaihotInput.SYSEAIHOT;

					// lt. ScoreService-Name Spalte
					List<EAIQIN> eaiqins = new List<EAIQIN> ();
					eaiqins.Add (createEaiQin ( syseaihot, "ContractReference", "" + lateInterestDto.ContractReference));

					foreach (EAIQIN eaiqinInput in eaiqins)
					{
						owCtx.EAIQIN.Add(eaiqinInput);
					}

					eaihotInput.EVE = 1;			// FLAG: process NOW!
					owCtx.SaveChanges ();
				}
			}
			catch (Exception e)
			{
				_log.Info ("Processing SCORE::LateInterest data delivered " + e.Message, e);
				ANY_dunningLevelDto_RETURNVALUE_DUMMY = e.Message;
				throw e;
			}
			 
		}



		 
		/// <summary>
		/// updates/creates Person (DUMMY)
		/// </summary>
		/// <param name="personDto"></param>
		/// <returns></returns>
		public void createOrUpdatePerson (DTO.ScoreCustomerDto personDto)
		{
            throw new NotImplementedException();
        }

		/// <summary>
		/// updates/creates Vehicle (DUMMY)
		/// </summary>
		/// <param name="vehicleDto"></param>
		/// <returns></returns>
		public void createOrUpdateVehicle (DTO.ScoreVehicleDto vehicleDto)
		{ 
			throw new NotImplementedException ();
		}
		/// <summary>
		/// updates/creates Invoice (DUMMY)
		/// </summary>
		/// <param name="invoiceDto"></param>
		/// <returns></returns>
		public void createOrUpdateInvoice (DTO.ScoreInvoiceDto invoiceDto)
		{ 
			throw new NotImplementedException ();
		}

	}
}



