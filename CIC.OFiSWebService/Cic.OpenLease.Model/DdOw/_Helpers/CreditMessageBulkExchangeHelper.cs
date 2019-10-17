// OWNER BK, 27-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class CreditMessageBulkExchangeHelper
	{
		#region Private constants
		private const int CnstErrorCodeNoError = 0;
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULKEX creditMessageBulkExchange, string responseData, int responseClarionDate, int responseClarionTime)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageBulkExchange == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageBulkExchange");
			}

			// Check clarion request date
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(responseClarionDate))
			{
				// Throw exception
				throw new System.ArgumentException("responseClarionDate");
			}

			// Check clarion request time
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(responseClarionTime))
			{
				// Throw exception
				throw new System.ArgumentException("responseClarionTime");
			}

			//// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			//responseData = StringUtil.GetNullStartIndexSubstring(responseData, 4000);

			try
			{
				// Check state
				if (creditMessageBulkExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageBulkExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageBulkExchange);
				// Set values
				creditMessageBulkExchange.RESPONSEDATA = responseData;
				creditMessageBulkExchange.RESPONSEDATE = responseClarionDate;
				creditMessageBulkExchange.RESPONSETIME = responseClarionTime;

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageBulkExchange);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULKEX creditMessageBulkExchange, Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants creditMessageBulkExchangeProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, owExtendedEntities, creditMessageBulkExchange, creditMessageBulkExchangeProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMBULKEX Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysCMBULK, Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants creditMessageBulkExchangeProcessConstants, System.DateTime requestDate, string requestData)
		{
			Cic.OpenLease.Model.DdOw.CMBULK CreditMessageBulk = null;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			try
			{
				// Get credit message bulk
				CreditMessageBulk = owExtendedEntities.CMBULK.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SYSCMBULK == sysCMBULK).FirstOrDefault<Cic.OpenLease.Model.DdOw.CMBULK>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (CreditMessageBulk == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.CMBULK).ToString() + "." + Cic.OpenLease.Model.DdOw.CMBULK.FieldNames.SYSCMBULK.ToString() + " = " + sysCMBULK.ToString());
			}

			try
			{
				// Internal
				return MyInsert(owExtendedEntities, CreditMessageBulk, creditMessageBulkExchangeProcessConstants, requestDate, requestData);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMBULKEX Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants creditMessageBulkExchangeProcessConstants, System.DateTime requestDate, string requestData)
		{
			try
			{
				// Internal
				return MyInsert(owExtendedEntities, creditMessageBulk, creditMessageBulkExchangeProcessConstants, requestDate, requestData);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		// TEST BK 0 BK, Not tested
		private static Cic.OpenLease.Model.DdOw.CMBULKEX MyInsert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants creditMessageBulkExchangeProcessConstant, System.DateTime requestDateTime, string requestData)
		{
			Cic.OpenLease.Model.DdOw.CMBULKEX CreditMessageBulkExchange = null;
			int ProcessCode;
			int RequestClarionDate;
			int RequestClarionTime;
			int? Rank;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageBulk == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageBulk");
			}

			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants), creditMessageBulkExchangeProcessConstant))
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageBulkExchangeProcessConstant");
			}
			// Set process code
			ProcessCode = (int)creditMessageBulkExchangeProcessConstant;

			// Check value
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateDateTimeForClarion(requestDateTime))
			{
				// Throw exception
				throw new System.ArgumentException("requestDateTime");
			}
			// Get clarion value
			Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(requestDateTime, out RequestClarionDate, out RequestClarionTime);

			//// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			//requestData = StringUtil.GetNullStartIndexSubstring(requestData, 4000);

			try
			{
				// Create new credit message bulk
				CreditMessageBulkExchange = new Cic.OpenLease.Model.DdOw.CMBULKEX();

				// Set values
				CreditMessageBulkExchange.CMBULK = creditMessageBulk;
				CreditMessageBulkExchange.PROCESSCODE = ProcessCode;
				CreditMessageBulkExchange.REQUESTDATE = RequestClarionDate;
				CreditMessageBulkExchange.REQUESTTIME = RequestClarionTime;
				CreditMessageBulkExchange.REQUESTDATA = requestData;
				CreditMessageBulkExchange.ERRORCODE = CnstErrorCodeNoError;
				
				// Get rank
				Rank = owExtendedEntities.CMBULKEX.Where<Cic.OpenLease.Model.DdOw.CMBULKEX>(par => par.CMBULK.SYSCMBULK == creditMessageBulk.SYSCMBULK).Select(par => par.RANK).Max();
				// Set values
				CreditMessageBulkExchange.RANK = ((Rank == null) || (Rank <= 0)) ? 1 : (Rank + 1);

				// Save workflow job execution history
				owExtendedEntities.AddToCMBULKEX(CreditMessageBulkExchange);
				owExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return CreditMessageBulkExchange;
		}

		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULKEX creditMessageBulkExchange, Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants creditMessageBulkExchangeProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
		{
			int ProcessCode;

			// Check state
			if (checkParameters)
			{
				// Check context
				if (owExtendedEntities == null)
				{
					// Throw exception
					throw new System.ArgumentException("owExtendedEntities");
				}

				// Check object
				if (creditMessageBulkExchange == null)
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageBulkExchange");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkExchangeProcessConstants), creditMessageBulkExchangeProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageBulkExchangeProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)creditMessageBulkExchangeProcessConstant;

			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (creditMessageBulkExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageBulkExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageBulkExchange);
				// Set values
				creditMessageBulkExchange.PROCESSCODE = ProcessCode;
				creditMessageBulkExchange.ERRORCODE = errorCode;
				creditMessageBulkExchange.ERRORNUMBER = errorNumber;
				creditMessageBulkExchange.ERRORMESSAGE = errorMessage;

				// Check state
				if (saveChanges)
				{
					// POSCHEN232009493349
					// Save changes
					//owExtendedEntities.SaveChanges();
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageBulkExchange);
				}
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion
	}
}
