// OWNER BK, 18-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class WorkflowUserHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFUSER SelectByCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string code)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.WFUSER> Query;
			Cic.OpenLease.Model.DdOw.WFUSER WorkflowUser;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check code
			if (StringUtil.IsTrimedNullOrEmpty(code))
			{
				// Throw exception
				throw new System.ArgumentException("code");
			}

			// Set Query
			Query = owExtendedEntities.WFUSER;
			// Search for code
			Query = Query.Where<Cic.OpenLease.Model.DdOw.WFUSER>(par => par.CODE.ToUpper() == code.ToUpper());

			try
			{
				// Select
				WorkflowUser = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.WFUSER>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (WorkflowUser == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.WFUSER).ToString() + "." + Cic.OpenLease.Model.DdOw.WFUSER.FieldNames.CODE.ToString() + " = " + code.ToUpper());
			}

			// Return
			return WorkflowUser;
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.WFUSER SelectBySysWFUSER(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysWFUSER)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.WFUSER> Query;
			Cic.OpenLease.Model.DdOw.WFUSER WorkflowUser;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.WFUSER;
			// Search for code
			Query = Query.Where<Cic.OpenLease.Model.DdOw.WFUSER>(par => par.SYSWFUSER == sysWFUSER);

			try
			{
				// Select
				WorkflowUser = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.WFUSER>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (WorkflowUser == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.WFUSER).ToString() + "." + Cic.OpenLease.Model.DdOw.WFUSER.FieldNames.SYSWFUSER.ToString() + " = " + sysWFUSER.ToString());
			}

			// Return
			return WorkflowUser;
		}
		#endregion
	}
}
