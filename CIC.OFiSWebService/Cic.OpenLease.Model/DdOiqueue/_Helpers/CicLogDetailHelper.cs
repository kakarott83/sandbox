// OWNER BK, 18-11-2008
namespace Cic.OpenLease.Model.DdOiqueue
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class CicLogDetailHelper
	{
		#region Private constants
		private const string CnstConnectionStringUserIdKey = "User ID";
		private const string CnstActionUndefined = "UNDEFINED";
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOiqueue.CICLOGD Insert(Cic.OpenLease.Model.DdOiqueue.OiqueueExtendedEntities oiqueueExtendedEntities, long sysCICLOG, string action, string description)
		{
			Cic.OpenLease.Model.DdOiqueue.CICLOG CicLog = null;

			// Check context
			if (oiqueueExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("oiqueueExtendedEntities");
			}

			try
			{
				// Get cic log
				CicLog = oiqueueExtendedEntities.CICLOG.Where(par => par.SYSCICLOG == sysCICLOG).FirstOrDefault<Cic.OpenLease.Model.DdOiqueue.CICLOG>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (CicLog == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOiqueue.CICLOG).ToString() + "." + Cic.OpenLease.Model.DdOiqueue.CICLOG.FieldNames.SYSCICLOG.ToString() + " = " + sysCICLOG.ToString());
			}

			try
			{
				// Internal
				return MyInsert(oiqueueExtendedEntities, CicLog, action, description);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOiqueue.CICLOGD Insert(Cic.OpenLease.Model.DdOiqueue.OiqueueExtendedEntities oiqueueExtendedEntities, Cic.OpenLease.Model.DdOiqueue.CICLOG cicLog, string action, string description)
		{
			try
			{
				// Internal
				return MyInsert(oiqueueExtendedEntities, cicLog, action, description);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static string MyGetConnectionStringUserIdValue(string openLeaseConnectionString)
		{
			System.Data.Common.DbConnectionStringBuilder DbConnectionStringBuilder;
			object Value;
			string UserId = null;

			// Check openlease connection string
			if (!StringUtil.IsTrimedNullOrEmpty(openLeaseConnectionString))
			{
				// New connection string builder
				DbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder();
				try
				{
					// Check user id
					if (DbConnectionStringBuilder.TryGetValue(CnstConnectionStringUserIdKey, out Value))
					{
						// Cast
						UserId = (string)Value;
					}
				}
				catch
				{
					// Ignore exception
				}
			}

			// Return
			return UserId;
		}

		// TEST BK 0 BK, Not tested
		private static Cic.OpenLease.Model.DdOiqueue.CICLOGD MyInsert(Cic.OpenLease.Model.DdOiqueue.OiqueueExtendedEntities oiqueueExtendedEntities, Cic.OpenLease.Model.DdOiqueue.CICLOG cicLog, string action, string description)
		{
			Cic.OpenLease.Model.DdOiqueue.CICLOGD CicLogDetail;
			string OpenLeaseConnectionString;

			// Check context
			if (oiqueueExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("oiqueueExtendedEntities");
			}

			// Check object
			if (cicLog == null)
			{
				// Throw exception
				throw new System.ArgumentException("cicLog");
			}

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
			if (StringUtil.IsTrimedNullOrEmpty(action))
			{
				// Set action
				// TODO BK 0 BK, Localize text
				action = CnstActionUndefined;
			}

			// Trim
			action = action.Trim();
			// Substring
			// TODO BK 1 BK, Replace with value from InfoPackageDictionary
			action = StringUtil.GetNullStartIndexSubstring(action, 30);

			try
			{
				// Create new ciclog detail
				CicLogDetail = new Cic.OpenLease.Model.DdOiqueue.CICLOGD();

				// Set action
				CicLogDetail.AKTION = action;
				// Check description
				if (!StringUtil.IsTrimedNullOrEmpty(description))
				{
					// Substring
                    // TODO BK 1 BK, Replace with value from InfoPackageDictionary
                    description = StringUtil.GetNullStartIndexSubstring(description, 3999);
                    // Set MessageText
                    CicLogDetail.MSGTEXT = description;

					// TODO BK 1 BK, Replace with value from InfoPackageDictionary
					description = StringUtil.GetNullStartIndexSubstring(description, 128);
					// Set description
					CicLogDetail.DESCRIPTION = description;
				}
				// Set date
				CicLogDetail.AKTIONDATE = System.DateTime.Now;
				CicLogDetail.CICLOG = cicLog;
				// Set primary key by sequence helper
				// TODO BK 1 BK, Remove table name
				CicLogDetail.SYSCICLOGD = Cic.OpenLease.Model.SequenceHelper.DeliverNextSequenceValue(OpenLeaseConnectionString, "CICLOGD");


				// Save ciclog detail
				oiqueueExtendedEntities.AddToCICLOGD(CicLogDetail);
				oiqueueExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return CicLogDetail;
		}
		#endregion
	}
}
