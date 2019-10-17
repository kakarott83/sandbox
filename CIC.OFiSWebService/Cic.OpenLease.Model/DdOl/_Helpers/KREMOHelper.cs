// OWNER BK, 27-08-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
    using Cic.OpenOne.Common.Util.Extension;
	#endregion

	[System.CLSCompliant(true)]
	public static class KREMOHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo, int responseClarionDate, int responseClarionTime)
		{
			System.DateTime ResponseDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (kremo == null)
			{
				// Throw exception
				throw new System.ArgumentException("kremo");
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
				if (kremo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(kremo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, kremo);
				// Set values
				kremo.RESPONSEDATE = ResponseDate.Date;
				kremo.RESPONSETIME = responseClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, kremo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo, int requestClarionDate, int requestClarionTime)
		{
			System.DateTime RequestDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (kremo == null)
			{
				// Throw exception
				throw new System.ArgumentException("kremo");
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
				if (kremo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(kremo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, kremo);
				// Set values
				kremo.REQUESTDATE = RequestDate.Date;
				kremo.REQUESTTIME = requestClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, kremo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		/*public static Cic.OpenLease.Model.DdOl.KREMO SelectById(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long id)
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
				return olExtendedEntities.SelectById<KREMO>(id);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}*/

		// TEST BK 0 BK, Not tested
		public static void UpdateSysWFEXEC(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo, long sysWFEXEC)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}
			// Check object
			if (kremo == null)
			{
				// Throw exception
				throw new System.ArgumentException("kremo");
			}

			try
			{
				// Check state
				if (kremo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(kremo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, kremo);
				// Set values
				kremo.SYSWFEXEC = sysWFEXEC;

				// Save changes
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, kremo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetInProcess(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, kremo, Cic.OpenLease.Model.DdOl.KREMOProcessConstants.InProcess, 0, null, null, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo, Cic.OpenLease.Model.DdOl.KREMOProcessConstants kremoProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, kremo, kremoProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region MyMethods
		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.KREMO kremo, Cic.OpenLease.Model.DdOl.KREMOProcessConstants kremoProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
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
				if (kremo == null)
				{
					// Throw exception
					throw new System.ArgumentException("kremo");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.KREMOProcessConstants), kremoProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("kremoProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)kremoProcessConstant;

			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (kremo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(kremo);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, kremo);
				kremo.PROCESSCODE = ProcessCode;
				kremo.ERRCODE = errorCode;
				kremo.ERRNUMBER = errorNumber;
				kremo.ERRMESSAGE = errorMessage;

				// Check state
				if (saveChanges)
				{
					// Save changes
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, kremo);
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