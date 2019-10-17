// OWNER BK, 11-11-2008
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class EaiArtHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.EAIART SelectByCode(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string code)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.EAIART> Query;
			Cic.OpenLease.Model.DdOw.EAIART EaiArt = null;

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
			Query = owExtendedEntities.EAIART;
			// Search for code
			Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIART>(par => par.CODE.ToUpper() == code.ToUpper());

			try
			{
				// Select
				EaiArt = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.EAIART>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (EaiArt == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.EAIART).ToString() + "." + Cic.OpenLease.Model.DdOw.EAIART.FieldNames.CODE.ToString() + " = " + code.ToUpper());
			}

			// Return
			return EaiArt;
		}

		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOw.EAIART SelectBySysEAIART(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long sysEAIART)
		{
			System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.EAIART> Query;
			Cic.OpenLease.Model.DdOw.EAIART EaiArt = null;

			// Check context
			if (owExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Set Query
			Query = owExtendedEntities.EAIART;
			// Search for code
			Query = Query.Where<Cic.OpenLease.Model.DdOw.EAIART>(par => par.SYSEAIART == sysEAIART);

			try
			{
				// Select
				EaiArt = Query.FirstOrDefault<Cic.OpenLease.Model.DdOw.EAIART>();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Check object
			if (EaiArt == null)
			{
				// Throw exception
				throw new System.Exception(typeof(Cic.OpenLease.Model.DdOw.EAIART).ToString() + "." + Cic.OpenLease.Model.DdOw.EAIART.FieldNames.SYSEAIART.ToString() + " = " + sysEAIART.ToString());
			}

			// Return
			return EaiArt;
		}
		#endregion
	}
}
