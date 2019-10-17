// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOPHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.PEINFOP Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, System.DateTime? entryDate, System.DateTime? lastVerifiedDate, System.DateTime? knownSinceDate, int legalGuardShip, int numberOfAddresses, int language, string profession, int numberOfRelations, int numberOfNegativeRelations)
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
				return MyInsert(olExtendedEntities, peinfo, entryDate, lastVerifiedDate, knownSinceDate, legalGuardShip, numberOfAddresses, language, profession, numberOfRelations, numberOfNegativeRelations);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.PEINFOP MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, System.DateTime? entryDate, System.DateTime? lastVerifiedDate, System.DateTime? knownSinceDate, int legalGuardShip, int numberOfAddresses, int language, string profession, int numberOfRelations, int numberOfNegativeRelations)
		{
			Cic.OpenLease.Model.DdOl.PEINFOP PEINFOP = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// PROFESSION
			if (profession != null)
			{
				profession = StringUtil.GetNullStartIndexSubstring(profession, 100);
			}

			try
			{
				// Create new object
				PEINFOP = new Cic.OpenLease.Model.DdOl.PEINFOP();

				// Set values
				PEINFOP.PEINFO = peinfo;
				PEINFOP.ENTRYDATE = entryDate;
				PEINFOP.LASTVERDATE = lastVerifiedDate;
				PEINFOP.KNOWNSINCEDATE = knownSinceDate;
				PEINFOP.LEGALGUARDSHIP = legalGuardShip;
				PEINFOP.NUMBOFADDR = numberOfAddresses;
				PEINFOP.LANGUAGE = language;
				PEINFOP.PROFESSION = profession;
				PEINFOP.NUMBOFREL = numberOfRelations;
				PEINFOP.NUMBOFRELNEG = numberOfNegativeRelations;

				// Save
				olExtendedEntities.AddToPEINFOP(PEINFOP);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return PEINFOP;
		}
		#endregion
	}
}
