// OWNER BK, 27-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class CreditMessageBulkHelper
	{
		#region Private constants
		private const int CnstErrorCodeNoError = 0;
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMBULK SelectBySysCMBULK(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysCMBULK)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.CMBULK> Query;
			Cic.OpenLease.Model.DdOw.CMBULK CreditMessageBulk;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.CMBULK;
			// Search for primary key
			Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SYSCMBULK == sysCMBULK);

			try
			{
				// Select
				CreditMessageBulk = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.CMBULK>();
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

			// Return
			return CreditMessageBulk;
		}

		// TEST BK 0 BK, Not tested
		//public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.CMBULK> Select(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants?[] creditMessageBulkProcessConstants, System.DateTime? referenceSendAtDateTime, int pageIndex, int pageSize)
		public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.CMBULK> Select(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants? creditMessageBulkProcessConstant, System.DateTime? referenceSendAtDateTime, int pageIndex, int pageSize)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.CMBULK> Query;
			int Skip;
			int Top;
			int ProcessCode;
			System.DateTime ReferenceSendAtDateTime;
			int ClarionDateMultiplier;
			System.DateTime MinDateForClarion;
			System.DateTime MaxDateForClarion;
			int ClarionMinDate;
			int ClarionMaxDate;
			int ClarionMinTime;
			int ClarionMaxTime;
			int ClarionReferenceSendAtDate;
			int ClarionReferenceSendAtTime;
			long ClarionReferenceSendAtDateAndTime;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check page index
			if (pageIndex < 0)
			{
				// Throw exception
				throw new System.ArgumentException("pageIndex");
			}

			// Check page index
			if (pageSize < 0)
			{
				// Throw exception
				throw new System.ArgumentException("pageSize");
			}

			// Set default values
			Skip = 0;
			Top = 0;

			// Check page size
			if (pageSize > 0)
			{
				// Set values
				Skip = (pageIndex * pageSize);
				Top = pageSize;
			}

			// Set query
			Query = owExtendedEntities.CMBULK;
			// Parmeter name
			Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => !string.IsNullOrEmpty(par.PARNAME.Trim()));
			//// Check process codes
			//if ((creditMessageBulkProcessConstants != null) && (creditMessageBulkProcessConstants.GetLength(0) > 0))
			//{
			//    // Loop through array
			//    foreach (Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants? LoopCreditMessageBulkProcessConstant in creditMessageBulkProcessConstants)
			//    {
			//        // Check process code
			//        if ((LoopCreditMessageBulkProcessConstant != null) && (System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants), LoopCreditMessageBulkProcessConstant)))
			//        {
			//            // Search for foreign key
			//            ProcessCode = (int)LoopCreditMessageBulkProcessConstant;
			//            Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.ProcessCode == ProcessCode);
			//        }
			//    }
			//}
			if (creditMessageBulkProcessConstant != null)
			{
				// Check process code
				if ((creditMessageBulkProcessConstant != null) && (System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants), creditMessageBulkProcessConstant)))
				{
					// Search for foreign key
					ProcessCode = (int)creditMessageBulkProcessConstant;
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.PROCESSCODE == ProcessCode);
				}
			}
			// Check reference date time
			if (referenceSendAtDateTime != null)
			{
				// Convert to date time
				ReferenceSendAtDateTime = (System.DateTime)referenceSendAtDateTime;
				// Get clarion values
				MinDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMinDateForClarion();
				MaxDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMaxDateForClarion();
				// Check value
				if ((ReferenceSendAtDateTime >= MinDateForClarion) && (ReferenceSendAtDateTime <= MaxDateForClarion))
				{
					// Get clarion values
					ClarionDateMultiplier = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionDateMultiplier();
					MinDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMinDateForClarion();
					MaxDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMaxDateForClarion();
					ClarionMinDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinDate();
					ClarionMaxDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxDate();
					ClarionMinTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinTime();
					ClarionMaxTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxTime();
					ClarionReferenceSendAtDateAndTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateTime(ReferenceSendAtDateTime);

					// Get clarion date and clation time
					Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(ReferenceSendAtDateTime, out ClarionReferenceSendAtDate, out ClarionReferenceSendAtTime);

					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATDATE.HasValue);
					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATDATE >= ClarionMinDate);
					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATDATE <= ClarionMaxDate);


					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATTIME.HasValue);
					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATTIME >= ClarionMinTime);
					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => par.SENDATTIME <= ClarionMaxTime);

					// Search submit date and time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.CMBULK>(par => (((par.SENDATDATE * ClarionDateMultiplier) + par.SENDATTIME) <= ClarionReferenceSendAtDateAndTime));
				}
			}
			// Check skip
			if (Skip > 0)
			{
				// Search for transaction
                // TODO MK 10 BK, Set skip ALWAYS if you set Top - otherwise you will get wrong sql
				Query = Query.Skip<Cic.OpenLease.Model.DdOw.CMBULK>(Skip);
			}
			// Check top
			if (Top > 0)
			{
				// TODO MK 0 BK, Check exceptions (Maybe change Dynamics.cs)
				Query = Query.Take<Cic.OpenLease.Model.DdOw.CMBULK>(Top);
			}

			//string Test = ((System.Data.Objects.ObjectQuery)Query).ToTraceString();

			try
			{
				// Return
				return Query.ToList<Cic.OpenLease.Model.DdOw.CMBULK>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, int responseClarionDate, int responseClarionTime)
		{
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

			try
			{
				// Check state
				if (creditMessageBulk.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageBulk);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageBulk);
				// Set values
				creditMessageBulk.LASTRESPONSEDATE = responseClarionDate;
				creditMessageBulk.LASTRESPONSETIME = responseClarionTime;

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageBulk);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, string requestIdentifier, int requestClarionDate, int requestClarionTime, int minIntervalSeconds, int maxDurationSeconds, bool first)
		{
			System.DateTime SendAtDateTime;
			System.DateTime TimeoutAtDateTime;
			int SendAtClarionDate;
			int SendAtClarionTime;
			int TimeoutAtClarionDate;
			int TimeoutAtClarionTime;

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

			// Check min interval
			if (minIntervalSeconds < 0)
			{
				// Set value
				minIntervalSeconds = 0;
			}
			// Check max duration
			if (maxDurationSeconds < minIntervalSeconds)
			{
				// Set value
				maxDurationSeconds = minIntervalSeconds;
			}

			try
			{
				// Check state
				if (creditMessageBulk.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageBulk);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageBulk);
				// Set values
				creditMessageBulk.PROCESSCODE = (int)Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants.InProcess;
				// Check state
				if (first)
				{
					creditMessageBulk.REQUESTIDENTIFIER = requestIdentifier;
					creditMessageBulk.FIRSTREQUESTDATE = requestClarionDate;
					creditMessageBulk.FIRSTREQUESTTIME = requestClarionTime;
				}
				creditMessageBulk.LASTREQUESTDATE = requestClarionDate;
				creditMessageBulk.LASTREQUESTTIME = requestClarionTime;
				// Check min interval
				if (minIntervalSeconds == 0)
				{
					// Set values
					SendAtClarionDate = requestClarionDate;
					SendAtClarionTime = requestClarionTime;
				}
				else
				{
					// Get date time
					SendAtDateTime = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateAndTimeToDateTime(requestClarionDate, requestClarionTime).AddSeconds((double)minIntervalSeconds);
					// Get clarion values
					Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(SendAtDateTime, out SendAtClarionDate, out SendAtClarionTime);
				}
				// Set values
				creditMessageBulk.SENDATDATE = SendAtClarionDate;
				creditMessageBulk.SENDATTIME = SendAtClarionTime;
				// Check state
				if (first)
				{
					// Check min interval
					if (maxDurationSeconds == 0)
					{
						// Set values
						TimeoutAtClarionDate = requestClarionDate;
						TimeoutAtClarionTime = requestClarionTime;
					}
					else
					{
						// Get date time
						TimeoutAtDateTime = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateAndTimeToDateTime(requestClarionDate, requestClarionTime).AddSeconds((double)maxDurationSeconds);
						// Get clarion values
						Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(TimeoutAtDateTime, out TimeoutAtClarionDate, out TimeoutAtClarionTime);
					}
					// Set values
					creditMessageBulk.TIMEOUTDATE = TimeoutAtClarionDate;
					creditMessageBulk.TIMEOUTTIME = TimeoutAtClarionTime;
				}

				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageBulk);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants creditMessageBulkProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, owExtendedEntities, creditMessageBulk, creditMessageBulkProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.CMBULK Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants creditMessageBulkProcessConstant, string parameterName, int messageCode, int providerCode, int serviceCode, string adapterName, string adapterVersion, string adapterIdentifier, string serviceName, string serviceVersion, string serviceUrl)
		{
			Cic.OpenLease.Model.DdOw.CMBULK CreditMessageBulk = null;
			int ProcessCode;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants), creditMessageBulkProcessConstant))
			{
				// Throw exception
				throw new System.ArgumentException("creditMessageBulkProcessConstant");
			}
			// Set process code
			ProcessCode = (int)creditMessageBulkProcessConstant;

			// Check parameter name
			if (StringUtil.IsTrimedNullOrEmpty(parameterName))
			{
				// Throw exception
				throw new System.ArgumentException("parameterName");
			}
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			parameterName = StringUtil.GetNullStartIndexSubstring(parameterName, 128);

			// Check adapter name
			if (StringUtil.IsTrimedNullOrEmpty(adapterName))
			{
				// Throw exception
				throw new System.ArgumentException("adapterName");
			}
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			adapterName = StringUtil.GetNullStartIndexSubstring(adapterName, 1024);

			// Check adapter version
			if (StringUtil.IsTrimedNullOrEmpty(adapterVersion))
			{
				// Throw exception
				throw new System.ArgumentException("adapterVersion");
			}
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			adapterVersion = StringUtil.GetNullStartIndexSubstring(adapterVersion, 1024);

			// Check adapter identifier
			if (StringUtil.IsTrimedNullOrEmpty(adapterIdentifier))
			{
				// Throw exception
				throw new System.ArgumentException("adapterIdentifier");
			}
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			adapterIdentifier = StringUtil.GetNullStartIndexSubstring(adapterIdentifier, 1024);

			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			serviceName = StringUtil.GetNullStartIndexSubstring(serviceName, 1024);

			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			serviceVersion = StringUtil.GetNullStartIndexSubstring(serviceVersion, 1024);

			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			serviceUrl = StringUtil.GetNullStartIndexSubstring(serviceUrl, 1024);

			try
			{
				// Create new credit message bulk
				CreditMessageBulk = new Cic.OpenLease.Model.DdOw.CMBULK();

				// Set values
				CreditMessageBulk.PROCESSCODE = ProcessCode;
				CreditMessageBulk.PARNAME = parameterName;
				CreditMessageBulk.MESSAGECODE = messageCode;
				CreditMessageBulk.PROVIDERCODE = providerCode;
				CreditMessageBulk.SERVICECODE = serviceCode;
				CreditMessageBulk.ADAPTERNAME = adapterName;
				CreditMessageBulk.ADAPTERVERSION = adapterVersion;
				CreditMessageBulk.ADAPTERIDENTIFIER = adapterIdentifier;
				CreditMessageBulk.SERVICENAME = serviceName;
				CreditMessageBulk.SERVICEVERSION = serviceVersion;
				CreditMessageBulk.SERVICEURL = serviceUrl;
				CreditMessageBulk.LASTERRORCODE = CnstErrorCodeNoError;

				// Save workflow job execution history
				owExtendedEntities.AddToCMBULK(CreditMessageBulk);
				owExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return CreditMessageBulk;
		}

		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.CMBULK creditMessageBulk, Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants creditMessageBulkProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
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
				if (creditMessageBulk == null)
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageBulk");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.CreditMessageBulkProcessConstants), creditMessageBulkProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("creditMessageBulkProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)creditMessageBulkProcessConstant;

			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (creditMessageBulk.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(creditMessageBulk);
				}

				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, creditMessageBulk);
                // Set values
                creditMessageBulk.PROCESSCODE = ProcessCode;
                creditMessageBulk.LASTERRORCODE = errorCode;
				creditMessageBulk.LASTERRORNUMBER = errorNumber;
                creditMessageBulk.LASTERRORMESSAGE = errorMessage;

                // Check state
                if (saveChanges)
                {
					// POSCHEN232009493349
					// Save changes
					//owExtendedEntities.SaveChanges();
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, creditMessageBulk);
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
