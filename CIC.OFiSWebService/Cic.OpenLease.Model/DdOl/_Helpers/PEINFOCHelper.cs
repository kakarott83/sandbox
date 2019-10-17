// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOCHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
        public static Cic.OpenLease.Model.DdOl.PEINFOC Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, System.DateTime? entryDate, System.DateTime? lastVerifiedDate, System.DateTime? knownSinceDate, string chNumber, System.DateTime? foundingDate, string nogaCode, string nogaDescription, int numberOfShares, int numberOfEmployees, string auditingCompany, string lastShabPublication, System.DateTime? LastShabDate, int companyStatus, decimal capital, decimal capitalPayed, string purpose, int leaderShipSize, int leaderShipSizeNeg, int managementSize)
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
				return MyInsert(olExtendedEntities, peinfo, entryDate, lastVerifiedDate, knownSinceDate, chNumber, foundingDate, nogaCode, nogaDescription, numberOfShares, numberOfEmployees, auditingCompany, lastShabPublication, LastShabDate, companyStatus, capital, capitalPayed, purpose, leaderShipSize, leaderShipSizeNeg, managementSize);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.PEINFOC MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, System.DateTime? entryDate, System.DateTime? lastVerifiedDate, System.DateTime? knownSinceDate, string chNumber, System.DateTime? foundingDate, string nogaCode, string nogaDescription, int numberOfShares, int numberOfEmployees, string auditingCompany, string lastShabPublication, System.DateTime? LastShabDate, int companyStatus, decimal capital, decimal capitalPayed, string purpose, int leaderShipSize, int leaderShipSizeNeg, int managementSize)
		{
			Cic.OpenLease.Model.DdOl.PEINFOC PEINFOC = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// TRADEREGNUMBER
			if (chNumber != null)
			{
				chNumber = StringUtil.GetNullStartIndexSubstring(chNumber, 20);
			}
			// BRANCHCODE
			if (nogaCode != null)
			{
				nogaCode = StringUtil.GetNullStartIndexSubstring(nogaCode, 5);
			}
			// BRANCHDESC
			if (nogaDescription != null)
			{
				nogaDescription = StringUtil.GetNullStartIndexSubstring(nogaDescription, 100);
			}
			// AUDITINGCOMP
			if (auditingCompany != null)
			{
				auditingCompany = StringUtil.GetNullStartIndexSubstring(auditingCompany, 255);
			}
			// LASTSHABPUBTEXT
			if (lastShabPublication != null)
			{
				lastShabPublication = StringUtil.GetNullStartIndexSubstring(lastShabPublication, 1000);
			}
			// PURPOSE
			if (purpose != null)
			{
				purpose = StringUtil.GetNullStartIndexSubstring(purpose, 1000);
			}

			try
			{
				// Create new object
				PEINFOC = new Cic.OpenLease.Model.DdOl.PEINFOC();

				// Set values
				PEINFOC.PEINFO = peinfo;
				PEINFOC.ENTRYDATE = entryDate;
				PEINFOC.LASTVERDATE = lastVerifiedDate;
				PEINFOC.KNOWNSINCEDATE = knownSinceDate;
				PEINFOC.TRADEREGNUMBER = chNumber;
				PEINFOC.FOUNDINGDATE = foundingDate;
				PEINFOC.BRANCHCODE = nogaCode;
				PEINFOC.BRANCHDESC = nogaDescription;
				PEINFOC.NUMBOFSHARES = numberOfShares;
				PEINFOC.NUMBOFEMP = numberOfEmployees;
				PEINFOC.AUDITINGCOMP = auditingCompany;
				PEINFOC.LASTSHABPUBTEXT = lastShabPublication;
				PEINFOC.LASTSHABPUBDATE = LastShabDate;
				PEINFOC.COMPSTATE = companyStatus;
				PEINFOC.CAPITAL = capital;
				PEINFOC.CAPITALPAYED = capitalPayed;
				PEINFOC.PURPOSE = purpose;
				PEINFOC.LEADSHIPSIZE = leaderShipSize;
				PEINFOC.LEADSHIPSIZENEG = leaderShipSizeNeg;
				PEINFOC.MANAGEMENTSIZE = managementSize;

				// Save
				olExtendedEntities.AddToPEINFOC(PEINFOC);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return PEINFOC;
		}
		#endregion
	}
}
