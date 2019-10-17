// OWNER BK, 15-09-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util.Extension;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int responseClarionDate, int responseClarionTime)
		{
			System.DateTime ResponseDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (peinfo == null)
			{
				// Throw exception
				throw new System.ArgumentException("peinfo");
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
				if (peinfo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(peinfo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, peinfo);
				// Set values
				peinfo.RESPONSEDATE = ResponseDate.Date;
				peinfo.RESPONSETIME = responseClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, peinfo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int requestClarionDate, int requestClarionTime)
		{
			System.DateTime RequestDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (peinfo == null)
			{
				// Throw exception
				throw new System.ArgumentException("peinfo");
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
				if (peinfo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(peinfo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, peinfo);
				// Set values
				peinfo.REQUESTDATE = RequestDate.Date;
				peinfo.REQUESTTIME = requestClarionTime;

				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, peinfo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		/*public static Cic.OpenLease.Model.DdOl.PEINFO SelectById(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long id)
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
                return olExtendedEntities.SelectById < PEINFO>(id);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}*/

		// TEST BK 0 BK, Not tested
		public static void UpdateSysWFEXEC(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, long sysWFEXEC)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}
			// Check object
			if (peinfo == null)
			{
				// Throw exception
				throw new System.ArgumentException("peinfo");
			}

			try
			{
				// Check state
				if (peinfo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(peinfo);
				}
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, peinfo);
				// Set values
				// TODO BK 10 BK, remove comments if field is available
				//peinfo.SYSWFEXEC = sysWFEXEC;

				// Save changes
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, peinfo);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetInProcess(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, peinfo, Cic.OpenLease.Model.DdOl.PEINFOProcessConstants.InProcess, 0, null, null, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, Cic.OpenLease.Model.DdOl.PEINFOProcessConstants peinfoProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, peinfo, peinfoProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region MyMethods
		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, Cic.OpenLease.Model.DdOl.PEINFOProcessConstants peinfoProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
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
				if (peinfo == null)
				{
					// Throw exception
					throw new System.ArgumentException("peinfo");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.PEINFOProcessConstants), peinfoProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("peinfoProcessConstant");
				}
			}

			// Set process code
			ProcessCode = (int)peinfoProcessConstant;

			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 128);
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (peinfo.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(peinfo);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, peinfo);
				peinfo.PROCESSCODE = ProcessCode;
				peinfo.PERRORCODE = errorCode;
				peinfo.PERRORNUMBER = errorNumber;
				peinfo.PERRORMESSAGE = errorMessage;

				// Check state
				if (saveChanges)
				{
					// Save changes
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, peinfo);
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