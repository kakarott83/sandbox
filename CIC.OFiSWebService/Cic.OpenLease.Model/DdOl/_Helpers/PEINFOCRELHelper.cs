// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOCRELHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.PEINFOCREL Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, Cic.OpenLease.Model.DdOl.PEINFOADR peinfoadr, int addressId, int functionCode, int signatureCode, System.DateTime? startDate, System.DateTime? endDate, bool isNegative)
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
			// Check object
			if (peinfoadr == null)
			{
				// Throw exception
				throw new System.ArgumentException("peinfoadr");
			}

			// TODO BK 8 BK, Check if peinfoadr has the same syspeinfo like the peinfo

			try
			{
				// Return
				return MyInsert(olExtendedEntities, peinfo, peinfoadr, addressId, functionCode, signatureCode, startDate, endDate, isNegative);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.PEINFOCREL MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, Cic.OpenLease.Model.DdOl.PEINFOADR peinfoadr, int addressId, int functionCode, int signatureCode, System.DateTime? startDate, System.DateTime? endDate, bool isNegative)
		{
			Cic.OpenLease.Model.DdOl.PEINFOCREL PEINFOCREL = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			try
			{
				// Create new object
				PEINFOCREL = new Cic.OpenLease.Model.DdOl.PEINFOCREL();

				// Set values
				PEINFOCREL.PEINFO = peinfo;
				PEINFOCREL.PEINFOADR = peinfoadr;
				PEINFOCREL.EXTERNEID = addressId;
				PEINFOCREL.FUNCCODE = functionCode;
				PEINFOCREL.SIGCODE = signatureCode;
				PEINFOCREL.STARTDATE = startDate;
				PEINFOCREL.ENDDATE = endDate;
				PEINFOCREL.NEGATIVEFLAG = (isNegative? 1 : 0);

				// Save
				olExtendedEntities.AddToPEINFOCREL(PEINFOCREL);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return PEINFOCREL;
		}
		#endregion
	}
}
