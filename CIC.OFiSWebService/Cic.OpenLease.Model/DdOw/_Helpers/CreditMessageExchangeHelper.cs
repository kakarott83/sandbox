// OWNER BK, 27-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class CreditMessageExchangeHelper
	{
		#region Private constants
		private const int CnstErrorCodeNoError = 0;
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, int responseClarionDate, int responseClarionTime)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageExchange == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageExchange");
			}

			// Check clarion response date
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(responseClarionDate))
			{
				// Throw exception
				throw new System.ArgumentException("responseClarionDate");
			}

			// Check clarion response time
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(responseClarionTime))
			{
				// Throw exception
				throw new System.ArgumentException("responseClarionTime");
			}

			try
			{
				// Check state
				if (creditMessageExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageExchange);
				// Set values
				creditMessageExchange.RESPONSEDATE = responseClarionDate;
				creditMessageExchange.RESPONSETIME = responseClarionTime;

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageExchange);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, int requestClarionDate, int requestClarionTime, bool first)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageExchange == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageExchange");
			}

			// Check clarion request date
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(requestClarionDate))
			{
				// Throw exception
				throw new System.ArgumentException("requestClarionDate");
			}

			// Check clarion request time
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(requestClarionTime))
			{
				// Throw exception
				throw new System.ArgumentException("requestClarionTime");
			}

			try
			{
				// Check state
				if (creditMessageExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageExchange);
				// Set values
				//if (!_DebugDoNotChangeCreditMessageExchangeProcessCode)
				//{
				creditMessageExchange.PROCESSCODE = (int)Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants.InProcess;
				//}
				if (first)
				{
					creditMessageExchange.REQUESTDATE = requestClarionDate;
					creditMessageExchange.REQUESTTIME = requestClarionTime;
				}

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageExchange);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX SelectBySysCMEX(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysCMEX)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.CMEX> Query;
			Cic.OpenLease.Model.DdOw.CMEX CreditMessageExchange;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.CMEX;
			// Search for primary key
			Query = Query.Where<Cic.OpenLease.Model.DdOw.CMEX>(par => par.SYSCMEX == sysCMEX);

			try
			{
				// Select
				CreditMessageExchange = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.CMEX>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (CreditMessageExchange == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.CMEX).ToString() + "." + Cic.OpenLease.Model.DdOw.CMEX.FieldNames.SYSCMEX.ToString() + " = " + sysCMEX.ToString());
			}

			// Return
			return CreditMessageExchange;
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, string responseData, int responseClarionDate, int responseClarionTime)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageExchange == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageExchange");
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
				if (creditMessageExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageExchange);
				// Set values
				creditMessageExchange.RESPONSEDATA = responseData;
				creditMessageExchange.RESPONSEDATE = responseClarionDate;
				creditMessageExchange.RESPONSETIME = responseClarionTime;

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageExchange);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, int requestClarionDate, int requestClarionTime)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (creditMessageExchange == null)
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageExchange");
			}

			// Check clarion request date
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(requestClarionDate))
			{
				// Throw exception
				throw new System.ArgumentException("requestClarionDate");
			}

			// Check clarion request time
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(requestClarionTime))
			{
				// Throw exception
				throw new System.ArgumentException("requestClarionTime");
			}

			try
			{
				// Check state
				if (creditMessageExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageExchange);
				// Set values
				creditMessageExchange.PROCESSCODE = (int)Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants.InProcess;
				creditMessageExchange.REQUESTDATE = requestClarionDate;
				creditMessageExchange.REQUESTTIME = requestClarionTime;

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageExchange);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, owExtendedEntities, creditMessageExchange, creditMessageExchangeProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant)
		{
			try
			{
				// Internale
				MySetProcessCode(true, owExtendedEntities, creditMessageExchange, creditMessageExchangeProcessConstant, null, null, null, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysZEK, long sysCMBULK, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant)
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
				CreditMessageBulk =  owExtendedEntities.CMBULK.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SYSCMBULK == sysCMBULK).FirstOrDefault<Cic.OpenLease.Model.DdOw.CMBULK>();
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
				// Return
				return MyInsert(owExtendedEntities, sysZEK, CreditMessageBulk, creditMessageExchangeProcessConstant, null, null, null, null, null, null, null, null, null, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.ZEK);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysZEK, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant)
		{
			try
			{
				// Return
				return MyInsert(owExtendedEntities, sysZEK, creditMessageBulk, creditMessageExchangeProcessConstant, null, null, null, null, null, null, null, null, null, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.ZEK);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX InsertForKREMO(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysKREMO, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl, string requestData, int requestClarionDate, int requestClarionTime)
		{
			try
			{
				// Return
				return MyInsert(owExtendedEntities, sysKREMO, null, creditMessageExchangeProcessConstant, adapterName, adapterVersion, adapterIdentifier, serviceName, serviceVersion, serviceUrl, requestData, requestClarionDate, requestClarionTime, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.KREMO);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX InsertForZEK(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysZEK, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl, string requestData, int requestClarionDate, int requestClarionTime)
		{
			try
			{
				// Return
				return MyInsert(owExtendedEntities, sysZEK, null, creditMessageExchangeProcessConstant, adapterName, adapterVersion, adapterIdentifier, serviceName, serviceVersion, serviceUrl, requestData, requestClarionDate, requestClarionTime, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.ZEK);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX InsertForADRV(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysADRV, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl, string requestData, int requestClarionDate, int requestClarionTime)
		{
			try
			{
				// Return
				return MyInsert(owExtendedEntities, sysADRV, null, creditMessageExchangeProcessConstant, adapterName, adapterVersion, adapterIdentifier, serviceName, serviceVersion, serviceUrl, requestData, requestClarionDate, requestClarionTime, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.ADRV);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMEX InsertForPEINFO(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysPEINFO, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl, string requestData, int requestClarionDate, int requestClarionTime)
		{
			try
			{
				// Return
				return MyInsert(owExtendedEntities, sysPEINFO, null, creditMessageExchangeProcessConstant, adapterName, adapterVersion, adapterIdentifier, serviceName, serviceVersion, serviceUrl, requestData, requestClarionDate, requestClarionTime, Cic.OpenLease.Model.DdOw.CMEXTypeConstants.PEINFO);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOw.CMEX MyInsert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysZEKorKREMOorADRVorPEINFO, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl, string requestData, int? requestClarionDate, int? requestClarionTime, Cic.OpenLease.Model.DdOw.CMEXTypeConstants cmexTypeConstant)
		{
			Cic.OpenLease.Model.DdOw.CMEX CreditMessageExchange = null;
			int ProcessCode;
			int? Rank = null;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants), creditMessageExchangeProcessConstant))
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageExchangeProcessConstant");
			}
			// Set process code
			ProcessCode = (int)creditMessageExchangeProcessConstant;

			try
			{
				// Create new credit message exchange
				CreditMessageExchange = new Cic.OpenLease.Model.DdOw.CMEX();

				// Check credit message bulk
				if (creditMessageBulk != null)
				{
					CreditMessageExchange.CMBULK = creditMessageBulk;
				}

				// Set values
				switch (cmexTypeConstant)
				{
					case CMEXTypeConstants.ZEK:
						CreditMessageExchange.SYSCM = sysZEKorKREMOorADRVorPEINFO;
						break;
					case CMEXTypeConstants.KREMO:
						CreditMessageExchange.SYSKREMO = sysZEKorKREMOorADRVorPEINFO;
						break;
					case CMEXTypeConstants.ADRV:
						CreditMessageExchange.SYSADRV = sysZEKorKREMOorADRVorPEINFO;
						break;
					case CMEXTypeConstants.PEINFO:
						CreditMessageExchange.SYSPEINFO = sysZEKorKREMOorADRVorPEINFO;
						break;
				}
				CreditMessageExchange.ADAPTERNAME = adapterName;
				CreditMessageExchange.ADAPTERVERSION = adapterVersion;
				CreditMessageExchange.ADAPTERIDENTIFIER = adapterIdentifier;
				CreditMessageExchange.SERVICENAME = serviceName;
				CreditMessageExchange.SERVICEVERSION = serviceVersion;
				CreditMessageExchange.SERVICEURL = serviceUrl;
				CreditMessageExchange.REQUESTDATA = requestData;
				CreditMessageExchange.REQUESTDATE = requestClarionDate;
				CreditMessageExchange.REQUESTTIME = requestClarionTime;
				CreditMessageExchange.PROCESSCODE = ProcessCode;
				CreditMessageExchange.ERRORCODE = CnstErrorCodeNoError;
				// Get rank
				switch (cmexTypeConstant)
				{
					case CMEXTypeConstants.ZEK:
						Rank = owExtendedEntities.CMEX.Where<Cic.OpenLease.Model.DdOw.CMEX>(par => par.SYSCM == sysZEKorKREMOorADRVorPEINFO).Select(par => par.RANK).Max();
						break;
					case CMEXTypeConstants.KREMO:
						Rank = owExtendedEntities.CMEX.Where<Cic.OpenLease.Model.DdOw.CMEX>(par => par.SYSKREMO == sysZEKorKREMOorADRVorPEINFO).Select(par => par.RANK).Max();
						break;
					case CMEXTypeConstants.ADRV:
						Rank = owExtendedEntities.CMEX.Where<Cic.OpenLease.Model.DdOw.CMEX>(par => par.SYSADRV == sysZEKorKREMOorADRVorPEINFO).Select(par => par.RANK).Max();
						break;
					case CMEXTypeConstants.PEINFO:
						Rank = owExtendedEntities.CMEX.Where<Cic.OpenLease.Model.DdOw.CMEX>(par => par.SYSPEINFO == sysZEKorKREMOorADRVorPEINFO).Select(par => par.RANK).Max();
						break;
				}
				// Set values
				CreditMessageExchange.RANK = ((Rank == null) || (Rank <= 0)) ? 1 : (Rank + 1);

				// Save workflow job execution history
				owExtendedEntities.AddToCMEX(CreditMessageExchange);
				owExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return CreditMessageExchange;
		}

		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMEX creditMessageExchange, Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants creditMessageExchangeProcessConstant, int? errorCode, string errorNumber, string errorMessage, bool saveChanges)
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
				if (creditMessageExchange == null)
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageExchange");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageExchangeProcessConstants), creditMessageExchangeProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageExchangeProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)creditMessageExchangeProcessConstant;

			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			if (errorNumber != null)
			{
				errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			}
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			if (errorMessage != null)
			{
				errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);
			}

			try
			{
				// Check state
				if (creditMessageExchange.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageExchange);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageExchange);
				// Set values
				creditMessageExchange.PROCESSCODE = ProcessCode;
				if (errorCode.HasValue)
				{
					creditMessageExchange.ERRORCODE = errorCode;
				}
				if (errorNumber != null)
				{
					creditMessageExchange.ERRORNUMBER = errorNumber;
				}
				if (errorMessage != null)
				{
					creditMessageExchange.ERRORMESSAGE = errorMessage;
				}

				// Check state
				if (saveChanges)
				{
					// POSCHEN232009493349
					// Save changes
					//owExtendedEntities.SaveChanges();
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageExchange);
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
