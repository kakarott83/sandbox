// OWNER BK, 18-11-2008
namespace Cic.OpenLease.Model.DdOiqueue
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class CicLogHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOiqueue.CICLOG SelectById(Cic.OpenLease.Model.DdOiqueue.OiqueueExtendedEntities oiqueueExtendedEntities, long sysCICLOG)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOiqueue.CICLOG> Query;
			Cic.OpenLease.Model.DdOiqueue.CICLOG[] CicLogs = null;
			Cic.OpenLease.Model.DdOiqueue.CICLOG CicLog = null;

			// Check context
			if (oiqueueExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("oiqueueExtendedEntities");
			}

			// Set Query
			Query = oiqueueExtendedEntities.CICLOG;
			// Search for code
			Query = Query.Where<Cic.OpenLease.Model.DdOiqueue.CICLOG>(par => par.SYSCICLOG == sysCICLOG);

			try
			{
				// Return
				CicLogs = Query.ToArray<Cic.OpenLease.Model.DdOiqueue.CICLOG>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check count
			if (CicLogs.Length == 1)
			{
				// Get first
				CicLog = CicLogs[0];
			}

			// Return
			return CicLog;
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOiqueue.CICLOG Insert(Cic.OpenLease.Model.DdOiqueue.OiqueueExtendedEntities oiqueueExtendedEntities, string machine, string workflowUserCode)
		{
			Cic.OpenLease.Model.DdOiqueue.CICLOG CicLog;
			string OpenLeaseConnectionString;
			string OracleUser;

			// Check context
			if (oiqueueExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("oiqueueExtendedEntities");
			}

			// Check cic user
			if (StringUtil.IsTrimedNullOrEmpty(workflowUserCode))
			{
				// Throw exception
				throw new System.ArgumentException("oiqueueExtendedEntities");
			}

			// Trim
			workflowUserCode = workflowUserCode.Trim();
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			workflowUserCode = StringUtil.GetNullStartIndexSubstring(workflowUserCode, 20);

			try
			{
				// Get openlease connection string
				OpenLeaseConnectionString = (oiqueueExtendedEntities.Connection as System.Data.EntityClient.EntityConnection).StoreConnection.ConnectionString;
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check machine
			if (StringUtil.IsTrimedNullOrEmpty(machine))
			{
				// Get machine name
				machine = System.Net.Dns.GetHostName();
			}

			// Trim
			machine = machine.Trim();
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			machine = StringUtil.GetNullStartIndexSubstring(machine, 20);

			// Get oracle user
			OracleUser = Cic.Basic.Data.Common.ConnectionStringHelper.DeliverUserIdValue(OpenLeaseConnectionString);
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			OracleUser = StringUtil.GetNullStartIndexSubstring(OracleUser, 20);

			try
			{
				// Create new log
				CicLog = new Cic.OpenLease.Model.DdOiqueue.CICLOG();
				// Set values
				CicLog.MASCHINE = machine;
				CicLog.ORABENUTZER = OracleUser;
				CicLog.CICBENUTZER = workflowUserCode;
				CicLog.LOGINDATE = System.DateTime.Now;
                
                // Set Oracle session ID
                CicLog.ID = Cic.OpenLease.Model.OracleSessionIdHelper.DeliverOracleSessionIdValue(OpenLeaseConnectionString);

				// Set primary key by sequence helper
				// TODO BK 1 BK, Remove table name
				CicLog.SYSCICLOG = Cic.OpenLease.Model.SequenceHelper.DeliverNextSequenceValue(OpenLeaseConnectionString, "CICLOG");

				// Save log
				oiqueueExtendedEntities.AddToCICLOG(CicLog);
				oiqueueExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}
			
			// Return
			return CicLog;
		}
		#endregion
	}
}
