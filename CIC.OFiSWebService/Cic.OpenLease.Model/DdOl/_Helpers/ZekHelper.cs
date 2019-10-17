// OWNER BK, 18-11-2008
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class ZekHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static void UpdateResponse(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek, int responseClarionDate, int responseClarionTime)
		{
			System.DateTime ResponseDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (zek == null)
			{
				// Throw exception
				throw new System.ArgumentException("zek");
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
				if (zek.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(zek);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, zek);
				// Set values
				zek.ANTWORTDATUM = ResponseDate.Date;
				zek.ANTWORTZEIT = responseClarionTime;

				// POSCHEN232009493349
				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, zek);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateRequest(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek, int requestClarionDate, int requestClarionTime, bool first)
		{
			System.DateTime RequestDate;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if (zek == null)
			{
				// Throw exception
				throw new System.ArgumentException("zek");
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
				if (zek.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(zek);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, zek);
				zek.ACTIONFLAG = (int)Cic.OpenLease.Model.DdOl.ZekProcessConstants.InProcess;
				if (first)
				{
					zek.ERSTEANFRAGEDATUM = RequestDate.Date;
					zek.ERSTEANFRAGEZEIT = requestClarionTime;
				}
				zek.LETZTEANFRAGEDATUM = RequestDate.Date;
				zek.LETZTEANFRAGEZEIT = requestClarionTime;

				// POSCHEN232009493349
				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, zek);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void UpdateSysWFEXEC(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek, long sysWFEXEC)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}
			// Check object
			if (zek == null)
			{
				// Throw exception
				throw new System.ArgumentException("zek");
			}

			try
			{
				// Check state
				if (zek.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(zek);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, zek);
				// Set values
				zek.SYSWFEXEC = sysWFEXEC;

				// POSCHEN232009493349
				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, zek);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetListProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.ZEK> zekList, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Return
				MySetListProcessCode(olExtendedEntities, zekList, zekProcessConstant, errorCode, errorNumber, errorMessage);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetInProcess(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, zek, Cic.OpenLease.Model.DdOl.ZekProcessConstants.InProcess, 0, null, null, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			try
			{
				// Internale
				MySetProcessCode(true, olExtendedEntities, zek, zekProcessConstant, errorCode, errorNumber, errorMessage, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.ZEK> Select(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, System.DateTime referenceSendAtDateTime, Cic.OpenLease.Model.BooleanSelectConstants sendAsBulkConstants)
		{
			try
			{
				// Internale
				return MySelect(olExtendedEntities, zekProcessConstant, referenceSendAtDateTime, sendAsBulkConstants, 0, 0);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.ZEK> Select(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, System.DateTime referenceSendAtDateTime, Cic.OpenLease.Model.BooleanSelectConstants sendAsBulkConstants, int pageIndex, int pageSize)
		{
			try
			{
				// Internale
				return MySelect(olExtendedEntities, zekProcessConstant, referenceSendAtDateTime, sendAsBulkConstants, pageIndex, pageSize);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.ZEK SelectBySysZEK(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, long sysZEK)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.ZEK> Query;
			Cic.OpenLease.Model.DdOl.ZEK Zek = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Set Query
			Query = olExtendedEntities.ZEK;
			// Search for primary key
			Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.SYSZEK == sysZEK);

			try
			{
				// Select
				Zek = Query.FirstOrDefault<Cic.OpenLease.Model.DdOl.ZEK>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (Zek == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOl.ZEK).ToString() + "." + Cic.OpenLease.Model.DdOl.ZEK.FieldNames.SYSZEK.ToString() + " = " + sysZEK.ToString());
			}

			// Return
			return Zek;
		}
		#endregion

		#region My methods
		private static System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.ZEK> MySelect(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZekProcessConstants? zekProcessConstant, System.DateTime? referenceSendAtDateTime, Cic.OpenLease.Model.BooleanSelectConstants sendAsBulkConstants, int pageIndex, int pageSize)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOl.ZEK> Query;
			int Skip;
			int Top;
			int ActionFlag;
			System.DateTime ReferenceSendAtDateTime;
			System.DateTime? MinDateForClarion;
			System.DateTime? MaxDateForClarion;
			long ClarionMinTime;
			long ClarionMaxTime;
			long ClarionReferenceSendAtTime;
			System.DateTime? ReferenceSendAtDay;
			System.DateTime? ReferenceSendAtNextDay;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
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
			Query = olExtendedEntities.ZEK;
			// Parmeter name
			Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => !string.IsNullOrEmpty(par.PARANAME.Trim()));
			// Check process code
			if ((zekProcessConstant != null) && (System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.ZekProcessConstants), zekProcessConstant)))
			{
				// Search for foreign key
				ActionFlag = (int)zekProcessConstant;
				Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.ACTIONFLAG == ActionFlag);
			}
			// Check state
			if (sendAsBulkConstants != Cic.OpenLease.Model.BooleanSelectConstants.CanBeBoth)
			{
				// Check state
				if (sendAsBulkConstants == Cic.OpenLease.Model.BooleanSelectConstants.MustBeTrue)
				{
					// Send as bulk
					Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.MASSENMELDUNG == 1);
				}
				else
				{
					// Send as bulk
					Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.MASSENMELDUNG != 1);
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
					MinDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMinDateForClarion();
					MaxDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMaxDateForClarion();
					ClarionMinTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinTime();
					ClarionMaxTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxTime();

					// Get clarion time
					ClarionReferenceSendAtTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(ReferenceSendAtDateTime);

					// Get today
					ReferenceSendAtDay = ReferenceSendAtDateTime.Date;
					// Get tomorrow
					ReferenceSendAtNextDay = ReferenceSendAtDay.Value.AddDays(1);

					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTIL.HasValue);
					//// Check submit date
                    Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTIL >= MinDateForClarion);
                    // Check submit date
                    Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTIL <= MaxDateForClarion);

					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTILTIME.HasValue);
					//// Check submit time
                    Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTILTIME >= ClarionMinTime);
                    // Check submit time
                    Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => par.WAITUNTILTIME <= ClarionMaxTime);

					// Search submit date and time
					Query = Query.Where<Cic.OpenLease.Model.DdOl.ZEK>(par => ((par.WAITUNTIL < ReferenceSendAtDay) || ((par.WAITUNTIL >= ReferenceSendAtDay) && (par.WAITUNTIL < ReferenceSendAtNextDay) && (par.WAITUNTILTIME <= ClarionReferenceSendAtTime))));
				}
			}
			// Check skip
			if (Skip > 0)
			{
				// Search for transaction
                // TODO MK 10 BK, Set skip ALWAYS if you set Top - otherwise you will get wrong sql
				Query = Query.Skip<Cic.OpenLease.Model.DdOl.ZEK>(Skip);
			}
			// Check top
			if (Top > 0)
			{
				// TODO MK 0 BK, Check exceptions (Maybe change Dynamics.cs)
				Query = Query.Take<Cic.OpenLease.Model.DdOl.ZEK>(Top);
			}

			//string Test = ((System.Data.Objects.ObjectQuery)Query).ToTraceString();

			try
			{
				// Return
				return Query.ToList<Cic.OpenLease.Model.DdOl.ZEK>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ZEK zek, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, int errorCode, string errorNumber, string errorMessage, bool saveChanges)
		{
			int ActionFlag;

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
				if (zek == null)
				{
					// Throw exception
					throw new System.ArgumentException("zek");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.ZekProcessConstants), zekProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("zekProcessConstant");
				}
			}

			// Set process code
			ActionFlag = (int)zekProcessConstant;

			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			errorNumber = StringUtil.GetNullStartIndexSubstring(errorNumber, 20);
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			// KRIEG2820091162155 120 > 1024
			errorMessage = StringUtil.GetNullStartIndexSubstring(errorMessage, 1024);

			try
			{
				// Check state
				if (zek.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					olExtendedEntities.Attach(zek);
				}
				// POSCHEN232009493349
				// Refresh
				olExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, zek);
				// Set values
				zek.ACTIONFLAG = ActionFlag;
				zek.ACTIONERROR = errorCode;
				zek.RUECKMELDECODE = errorNumber;
				zek.RUECKMELDUNG = errorMessage;

				// Check state
				if (saveChanges)
				{
					// POSCHEN232009493349
					// Save changes
					//olExtendedEntities.SaveChanges();
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, zek);
				}
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		private static void MySetListProcessCode(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.ZEK> zekList, Cic.OpenLease.Model.DdOl.ZekProcessConstants zekProcessConstant, int errorCode, string errorNumber, string errorMessage)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("olExtendedEntities");
			}

			// Check object
			if ((zekList == null) || (zekList.Count <= 0))
			{
				// Throw exception
				throw new System.ArgumentException("zekList");
			}

			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOl.ZekProcessConstants), zekProcessConstant))
			{
				// Throw exception
				throw new System.ArgumentException("zekProcessConstant");
			}

			try
			{
				// Loop through eaijobs
				foreach (Cic.OpenLease.Model.DdOl.ZEK LoopZek in zekList)
				{
					// Set process code
					MySetProcessCode(false, olExtendedEntities, LoopZek, zekProcessConstant, errorCode, errorNumber, errorMessage, false);
				}
				// POSCHEN232009493349
				// Save changes
				//olExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(olExtendedEntities, zekList.ToArray());
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
