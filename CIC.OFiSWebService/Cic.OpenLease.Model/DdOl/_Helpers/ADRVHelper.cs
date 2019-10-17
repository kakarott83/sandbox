// OWNER BK, 15-09-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util.Extension;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class ADRVHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, int responseClarionDate, int responseClarionTime)
		{
			System.DateTime ResponseDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (adrv == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrv");
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

			// Get response date time
			ResponseDate = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateAndTimeToDateTime(responseClarionDate, responseClarionTime);

			try
			{
				// Check state
				if (adrv.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(adrv);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, adrv);
				// Set values
				adrv.RESPONSEDATE = ResponseDate.Date;
				adrv.RESPONSETIME = responseClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, adrv);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, int requestClarionDate, int requestClarionTime)
		{
			System.DateTime RequestDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (adrv == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrv");
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

			// Get request date time
			RequestDate = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateAndTimeToDateTime(requestClarionDate, requestClarionTime);

			try
			{
				// Check state
				if (adrv.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(adrv);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, adrv);
				// Set values
				adrv.REQUESTDATE = RequestDate.Date;
				adrv.REQUESTTIME = requestClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, adrv);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		/*public static Cic.OpenLease.Model.DdOl.ADRV SelectById(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long id)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			try
			{
				// Select portal user
                return olExtendedEntities.SelectById < ADRV>(id);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}*/

		// TEST BK 0 BK, Not tested
		public static void UpdateSysWFEXEC(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, long sysWFEXEC)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}
			// Check object
			if (adrv == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrv");
			}

			try
			{
				// Check state
				if (adrv.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(adrv);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, adrv);
				// Set values
				// TODO BK 10 BK, remove comments if field is available
				//adrv.SYSWFEXEC = sysWFEXEC;

				// Save changes
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, adrv);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetInProcess(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, adrv, Cic.OpenLease.Model.DdOl.ADRVProcessConstants.InProcess, 0, null, null, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, Cic.OpenLease.Model.DdOl.ADRVProcessConstants adrvProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, adrv, adrvProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region MyMethods
		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, Cic.OpenLease.Model.DdOl.ADRVProcessConstants adrvProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
		{
			int ProcessCode;

			// Check state
			if (checkParameters)
			{
				// Check context
				if (olExtendedEntities == null)
				{
					// Throw exception
					throw new System.ArgumentException("olExtendedEntities");
				}

				// Check object
				if (adrv == null)
				{
					// Throw exception
					throw new System.ArgumentException("adrv");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.ADRVProcessConstants), adrvProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("adrvProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)adrvProcessConstant;

			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (adrv.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(adrv);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, adrv);
				adrv.PROCESSCODE = ProcessCode;
				adrv.FEHLERCODE = errorCode;
				adrv.FEHLERNUMMER = errorNumber;
				adrv.FEHLERMELDUNG = errorMessage;

				// Check state
				if (saveChanges)
				{
					// Save changes
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, adrv);
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