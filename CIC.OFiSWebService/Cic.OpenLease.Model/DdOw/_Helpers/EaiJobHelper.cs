// OWNER BK, 11-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
	#endregion

	[System.CLSCompliant(true)]
	public static class EaiJobHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static void SetListInProcess(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> eaiJobList)
		{
			int ClarionDate;
			int ClarionTime;

			// Get clarion values
			Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(System.DateTime.Now, out ClarionDate, out ClarionTime);

			try
			{
				// Return
				MySetListProcessCode(owExtendedEntities, eaiJobList, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants.InProcess, ClarionDate, ClarionTime);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetListProcessed(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> eaiJobList)
		{
			int ClarionDate;
			int ClarionTime;

			// Get clarion values
			Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(System.DateTime.Now, out ClarionDate, out ClarionTime);

			try
			{
				// Return
				MySetListProcessCode(owExtendedEntities, eaiJobList, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants.Processed, ClarionDate, ClarionTime);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetListProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> eaiJobList, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, System.DateTime dateTime)
		{
			int ClarionDate;
			int ClarionTime;

			// Validate datetime
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateDateTimeForClarion(dateTime))
			{
				// Throw exception
				throw new System.ArgumentException("requestDateTime");
			}
			// Get clarion value
			Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(dateTime, out ClarionDate, out ClarionTime);

			try
			{
				// Return
				MySetListProcessCode(owExtendedEntities, eaiJobList, eaiJobProcessConstant, ClarionDate, ClarionTime);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.EAIJOB eaiJob, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, int clarionDate, int clarionTime, bool start)
		{
			try
			{
				// Return
				MySetProcessCode(true, owExtendedEntities, eaiJob, eaiJobProcessConstant, clarionDate, clarionTime, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static void SetProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.EAIJOB eaiJob, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, System.DateTime dateTime)
		{
			int ClarionDate;
			int ClarionTime;

			// Validate datetime
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateDateTimeForClarion(dateTime))
			{
				// Throw exception
				throw new System.ArgumentException("requestDateTime");
			}
			// Get clarion value
			Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateAndTime(dateTime, out ClarionDate, out ClarionTime);

			try
			{
				// Return
				MySetProcessCode(true, owExtendedEntities, eaiJob, eaiJobProcessConstant, ClarionDate, ClarionTime, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> Select(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysEAIART, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, System.DateTime referenceDateTime, Cic.OpenLease.Model.BooleanSelectConstants PartOfTransactionConstants)
		{

			try
			{
				// Return
				return MySelect(owExtendedEntities, sysEAIART, eaiJobProcessConstant, referenceDateTime, PartOfTransactionConstants, 0, 0);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> Select(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysEAIART, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, System.DateTime referenceDateTime, Cic.OpenLease.Model.BooleanSelectConstants PartOfTransactionConstants, int pageIndex, int pageSize)
		{

			try
			{
				// Return
				return MySelect(owExtendedEntities, sysEAIART, eaiJobProcessConstant, referenceDateTime, PartOfTransactionConstants, pageIndex, pageSize);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.EAIJOB SelectBySysEAIJOB(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysEAIJOB)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.EAIJOB> Query;
			Cic.OpenLease.Model.DdOw.EAIJOB EaiJob;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.EAIJOB;
			// Search for primary key
			Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.SYSEAIJOB == sysEAIJOB);

			try
			{
				// Select
				EaiJob = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.EAIJOB>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (EaiJob == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.EAIJOB).ToString() + "." + Cic.OpenLease.Model.DdOw.EAIJOB.FieldNames.SYSEAIJOB.ToString() + " = " + sysEAIJOB.ToString());
			}

			// Return
			return EaiJob;
		}

        public static void Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, EAIJOB Eaijob)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Save
            owExtendedEntities.AddToEAIJOB(Eaijob);
            owExtendedEntities.SaveChanges();
        }

		#endregion

		#region My methods
		private static void MySetListProcessCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities,  System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> eaiJobList, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, int clarionDate, int clarionTime)
		{
			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if ((eaiJobList == null) || (eaiJobList.Count <= 0))
			{
				// Throw exception
				throw new System.ArgumentException("eaiJobList");
			}

			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.EaiJobProcessConstants), eaiJobProcessConstant))
			{
				// Throw exception
				throw new System.ArgumentException("eaiJobProcessConstant");
			}

			// Validate clarion date
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(clarionDate))
			{
				// Throw exception
				throw new System.ArgumentException("clarionDate");
			}

			// Validate clarion time
			if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(clarionTime))
			{
				// Throw exception
				throw new System.ArgumentException("clarionTime");
			}

			try
			{
				// Loop through eaijobs
				foreach (Cic.OpenLease.Model.DdOw.EAIJOB LoopEaiJob in eaiJobList)
				{
					// Set process code
					MySetProcessCode(false, owExtendedEntities, LoopEaiJob, eaiJobProcessConstant, clarionDate, clarionTime, false);
				}
				// POSCHEN232009493349
				// Save changes
				//owExtendedEntities.SaveChanges();
				Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, eaiJobList.ToArray());
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		private static void MySetProcessCode(bool checkParameters, Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.EAIJOB eaiJob, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants eaiJobProcessConstant, int clarionDate, int clarionTime, bool saveChanges)
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
				if (eaiJob == null)
				{
					// Throw exception
					throw new System.ArgumentException("eaiJob");
				}

				// Check type constant
				if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.EaiJobProcessConstants), eaiJobProcessConstant))
				{
					// Throw exception
					throw new System.ArgumentException("eaiJobProcessConstant");
				}

				// Validate clarion date
				if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionDate(clarionDate))
				{
					// Throw exception
					throw new System.ArgumentException("clarionDate");
				}

				// Validate clarion time
				if (!Cic.OpenOne.Common.Util.DateTimeHelper.ValidateClarionTime(clarionTime))
				{
					// Throw exception
					throw new System.ArgumentException("clarionTime");
				}
			}

			// Set process code
			ProcessCode = (int)eaiJobProcessConstant;

			try
			{
				// Check state
				if (eaiJob.EntityState == System.Data.EntityState.Detached)
				{
					// Attach
					owExtendedEntities.Attach(eaiJob);
				}
				// POSCHEN232009493349
				// Refresh
				owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.StoreWins, eaiJob);
				// Set values
				eaiJob.PROZESSSTATUS = ProcessCode;
				// Check state
				if (eaiJobProcessConstant == Cic.OpenLease.Model.DdOw.EaiJobProcessConstants.InProcess)
				{
					eaiJob.STARTDATE = clarionDate;
					eaiJob.STARTTIME = clarionTime;
					eaiJob.FINISHDATE = null;
					eaiJob.FINISHTIME = null;
				}
				else if (eaiJobProcessConstant == Cic.OpenLease.Model.DdOw.EaiJobProcessConstants.Processed)
				{
					eaiJob.FINISHDATE = clarionDate;
					eaiJob.FINISHTIME = clarionTime;
				}
				// Check state
				if (saveChanges)
				{
					// POSCHEN232009493349
					// Save changes
					//owExtendedEntities.SaveChanges();
					Cic.OpenLease.Model.ObjectContextHelper.SaveChanges(owExtendedEntities, eaiJob);
				}
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		private static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.EAIJOB> MySelect(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long? sysEAIART, Cic.OpenLease.Model.DdOw.EaiJobProcessConstants? eaiJobProcessConstant, System.DateTime? referenceDateTime, Cic.OpenLease.Model.BooleanSelectConstants PartOfTransactionConstants, int pageIndex, int pageSize)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.EAIJOB> Query;
			int Skip;
			int Top;
			long ProcessCode;
			System.DateTime DateTime;
			int ClarionDateMultiplier;
			int ClarionMinDate;
			int ClarionMaxDate;
			int ClarionMinTime;
			int ClarionMaxTime;
			long ClarionDateTime;

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

			// Set Query
			Query = owExtendedEntities.EAIJOB;
			// Search for foreign key
			Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.EAIART.SYSEAIART == sysEAIART);
			// Check process code
			if ((eaiJobProcessConstant != null) && (System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.EaiJobProcessConstants), eaiJobProcessConstant)))
			{
				// Set proccess code
				ProcessCode = (int)eaiJobProcessConstant;
				// Search for foreign key
				Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.PROZESSSTATUS == ProcessCode);
			}
			// Check state
			if (PartOfTransactionConstants != Cic.OpenLease.Model.BooleanSelectConstants.CanBeBoth)
			{
				// Check part of transaction flag
				if (PartOfTransactionConstants == Cic.OpenLease.Model.BooleanSelectConstants.MustBeTrue)
				{
					// Search for transaction
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.TRANSACTIONFLAG == 1);
				}
				else
				{
					// Search for transaction
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.TRANSACTIONFLAG != 1);
				}
			}
			// Check reference date time
			if (referenceDateTime != null)
			{
				// Convert to date time
				DateTime = (System.DateTime)referenceDateTime;
				// Check value
				if (DateTime >= Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMinDateForClarion())
				{
					// Get clarion values
					ClarionDateMultiplier = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionDateMultiplier();
					ClarionMinDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinDate();
					ClarionMaxDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxDate();
					ClarionMinTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinTime();
					ClarionMaxTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxTime();

					// Get clarion date time
					ClarionDateTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateTime(DateTime);

					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.SUBMITDATE >= ClarionMinDate);
					// Check submit date
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.SUBMITDATE <= ClarionMaxDate);
					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.SUBMITTIME >= ClarionMinTime);
					// Check submit time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => par.SUBMITTIME <= ClarionMaxTime);

					// Search submit date and time
					Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIJOB>(par => ((par.SUBMITDATE * ClarionDateMultiplier) + par.SUBMITTIME) <= ClarionDateTime);
				}
			}
			// Order by submit date
			Query = Query.OrderBy(par => par.SUBMITDATE).ThenBy(par => par.SUBMITTIME);

			// Check top
			if (Top > 0)
			{
				// TODO MK 0 BK, Check exceptions (Maybe change Dynamics.cs)
                Query = Query.Skip<Cic.OpenLease.Model.DdOw.EAIJOB>(Skip);

				Query = Query.Take<Cic.OpenLease.Model.DdOw.EAIJOB>(Top);
			}

			try
			{
				// Return
				return Query.ToList<Cic.OpenLease.Model.DdOw.EAIJOB>();
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
