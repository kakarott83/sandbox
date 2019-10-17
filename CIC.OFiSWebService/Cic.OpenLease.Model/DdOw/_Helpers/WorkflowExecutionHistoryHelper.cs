// OWNER BK, 20-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class WorkflowExecutionHistoryHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
        public static Cic.OpenLease.Model.DdOw.WFEXEC Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.WFJEXEC workflowJobExecutionHistory, string workflowTableCode, long openLeaseId)
		{
			try
			{
				// Internal
				return MyInsert(owExtendedEntities, workflowJobExecutionHistory, workflowTableCode, openLeaseId);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFEXEC Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysWFJEXEC, string workflowTableCode, long openLeaseId)
		{
			Cic.OpenLease.Model.DdOw.WFJEXEC WorkflowJobExecutionHistory = null;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			try
			{
				// Get workflow job execution history
				WorkflowJobExecutionHistory = owExtendedEntities.WFJEXEC.Where(par => par.SYSWFJEXEC == sysWFJEXEC).FirstOrDefault<Cic.OpenLease.Model.DdOw.WFJEXEC>();
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

			try
			{
				// Internal
				return MyInsert(owExtendedEntities, WorkflowJobExecutionHistory, workflowTableCode, openLeaseId);
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
		private static Cic.OpenLease.Model.DdOw.WFEXEC MyInsert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.WFJEXEC workflowJobExecutionHistory, string workflowTableCode, long openLeaseId)
		{
			Cic.OpenLease.Model.DdOw.WFEXEC WorkflowExecutionHistory;
			System.DateTime CurrentDateTime;
			int ClarionTime;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (workflowJobExecutionHistory == null)
			{
				// Throw exception
				throw new System.ArgumentException("workflowJobExecutionHistory");
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
			workflowTableCode = StringUtil.GetNullStartIndexSubstring(workflowTableCode, 25);

			// Get current date time
			CurrentDateTime = System.DateTime.Now;
			// Get clarion time
			ClarionTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(CurrentDateTime);
			// Only date
			CurrentDateTime = CurrentDateTime.Date;

			try
			{
				// Create new workflow execution history
				WorkflowExecutionHistory = new Cic.OpenLease.Model.DdOw.WFEXEC();

				// Set values
				WorkflowExecutionHistory.TABLESYSCODE = workflowTableCode;
				WorkflowExecutionHistory.LEASESYSID = openLeaseId;
				WorkflowExecutionHistory.JOB = 0;
				WorkflowExecutionHistory.DATUM = CurrentDateTime;
				WorkflowExecutionHistory.UHRZEIT = ClarionTime;
				WorkflowExecutionHistory.WFJEXEC = workflowJobExecutionHistory;

				// Save workflow execution history
				owExtendedEntities.AddToWFEXEC(WorkflowExecutionHistory);
				owExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return WorkflowExecutionHistory;
		}
		#endregion
}
}
