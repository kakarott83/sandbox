// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOCONHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.PEINFOCON Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int contactType, string contactDetails, System.DateTime? lastVerifiedDate)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}
			// Check object
			if (peinfo == null)
			{
				// Throw exception
				throw new System.ArgumentException("peinfo");
			}

			try
			{
				// Return
				return MyInsert(olExtendedEntities, peinfo, contactType, contactDetails, lastVerifiedDate);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.PEINFOCON MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int contactType, string contactDetails, System.DateTime? lastVerifiedDate)
		{
			Cic.OpenLease.Model.DdOl.PEINFOCON PEINFOCON = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// DETAILS
			if (contactDetails != null)
			{
				contactDetails = StringUtil.GetNullStartIndexSubstring(contactDetails, 100);
			}

			try
			{
				// Create new object
				PEINFOCON = new Cic.OpenLease.Model.DdOl.PEINFOCON();

				// Set values
				PEINFOCON.PEINFO = peinfo;
				PEINFOCON.CONTACTTYPE = contactType;
				PEINFOCON.DETAILS = contactDetails;
				PEINFOCON.LASTVERDATE = lastVerifiedDate;

				// Save
				olExtendedEntities.AddToPEINFOCON(PEINFOCON);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return PEINFOCON;
		}
		#endregion
	}
}
