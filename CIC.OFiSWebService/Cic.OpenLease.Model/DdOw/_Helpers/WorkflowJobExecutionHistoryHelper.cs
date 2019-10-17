// OWNER BK, 20-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class WorkflowJobExecutionHistoryHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFJEXEC Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.WFUSER workflowUser, long sysCICLOG, string workflowTableCode, System.DateTime jobDate, string jobText)
		{
			try
			{
				// Internal
				return MyInsert(owExtendedEntities, workflowUser, sysCICLOG, workflowTableCode, jobDate, jobText);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFJEXEC Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysWFUSER, long sysCICLOG, string workflowTableCode, System.DateTime jobDate, string jobText)
		{
			Cic.OpenLease.Model.DdOw.WFUSER WorkflowUser;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			try
			{
				// Get workflow user
                WorkflowUser = Cic.OpenLease.Model.DdOw.WorkflowUserHelper.SelectBySysWFUSER(owExtendedEntities, sysWFUSER);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			try
			{
				// Internal
				return MyInsert(owExtendedEntities, WorkflowUser, sysCICLOG, workflowTableCode, jobDate, jobText);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFJEXEC SelectBySysWFJEXEC(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysWFJEXEC)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.WFJEXEC> Query;
			Cic.OpenLease.Model.DdOw.WFJEXEC WorkflowJobExecutionHistory = null;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.WFJEXEC;
			// Search for primary key
			Query = Query.Where<Cic.OpenLease.Model.DdOw.WFJEXEC>(par => par.SYSWFJEXEC == sysWFJEXEC);

			try
			{
				// Select
				WorkflowJobExecutionHistory = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.WFJEXEC>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (WorkflowJobExecutionHistory == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.WFJEXEC).ToString() + "." + Cic.OpenLease.Model.DdOw.WFJEXEC.FieldNames.SYSWFJEXEC.ToString() + " = " + sysWFJEXEC.ToString());
			}

			// Return
			return WorkflowJobExecutionHistory;
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOw.WFJEXEC MyInsert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.WFUSER workflowUser, long sysCICLOG, string workflowTableCode, System.DateTime jobDate, string jobText)
		{
			Cic.OpenLease.Model.DdOw.WFJEXEC WorkflowJobExecutionHistory = null;
			System.DateTime CurrentDateTime;
			int ClarionTime;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (workflowUser == null)
			{
				// Throw exception
				throw new System.ArgumentException("workflowUser");
			}

			// Check workflow table code
			if (StringUtil.IsTrimedNullOrEmpty(workflowTableCode))
			{
				// Throw exception
				throw new System.ArgumentException("workflowTableCode");
			}

			// Trim
			workflowTableCode = workflowTableCode.Trim();
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			workflowTableCode = StringUtil.GetNullStartIndexSubstring(workflowTableCode, 15);

			// Check job text
			if (StringUtil.IsTrimedNullOrEmpty(jobText))
			{
				// Throw exception
				throw new System.ArgumentException("jobText");
			}

			// Trim
			jobText = jobText.Trim();
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			jobText = StringUtil.GetNullStartIndexSubstring(jobText, 80);

			// Get current date time
			CurrentDateTime = System.DateTime.Now;
			// Get clarion time
			ClarionTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(CurrentDateTime);
			// Only date
			CurrentDateTime = CurrentDateTime.Date;

			try
			{
				// Create new workflow job execution history
				WorkflowJobExecutionHistory = new Cic.OpenLease.Model.DdOw.WFJEXEC();

				// Set values
				// TODO MK 0 BK, Change name to WorkflowTableCode in DD
				WorkflowJobExecutionHistory.SYSCICLOG = sysCICLOG;
				WorkflowJobExecutionHistory.TABLESYSCODE = workflowTableCode;
				WorkflowJobExecutionHistory.JOBDATE = jobDate;
				WorkflowJobExecutionHistory.JOBTEXT = jobText;
				WorkflowJobExecutionHistory.DATUM = CurrentDateTime;
				WorkflowJobExecutionHistory.UHRZEIT = ClarionTime;
				WorkflowJobExecutionHistory.WFUSER = workflowUser;

				// Save workflow job execution history
				owExtendedEntities.AddToWFJEXEC(WorkflowJobExecutionHistory);
				owExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return WorkflowJobExecutionHistory;
		}
		#endregion
	}
}
