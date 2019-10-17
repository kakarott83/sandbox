// OWNER BK, 05-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
	#endregion

	[System.CLSCompliant(true)]
	public static class ADRVTRFHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.ADRVTRF Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, Cic.OpenLease.Model.DdOl.ADRVDAT foundADRVDAT, long addressID, int status, int confidence, int difference, int similarity, int character)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (adrv == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrv");
			}

			// Check object
			if (adrv.ADRVDAT == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrv.ADRVDAT");
			}

			try
			{
				// Return
				return MyInsert(olExtendedEntities, adrv, foundADRVDAT, addressID, status, confidence, difference, similarity, character);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.ADRVTRF MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRV adrv, Cic.OpenLease.Model.DdOl.ADRVDAT foundADRVDAT, long addressID, int status, int confidence, int difference, int similarity, int character)
		{
			Cic.OpenLease.Model.DdOl.ADRVTRF ADRVTRF;

			long? FoundSysADRVDAT = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (foundADRVDAT != null)
			{
				// Set value
				FoundSysADRVDAT = foundADRVDAT.SYSADRVDAT;
			}

			try
			{
				// Create new object
				ADRVTRF = new Cic.OpenLease.Model.DdOl.ADRVTRF();

				// Set values
				ADRVTRF.ADRV = adrv;
				ADRVTRF.ADRVDAT = adrv.ADRVDAT;
				ADRVTRF.SYSADRVDATT = FoundSysADRVDAT;
				ADRVTRF.EXTERNEID = addressID;
				ADRVTRF.STATUS = status;
				ADRVTRF.WERTUNG = confidence;
				ADRVTRF.UNTERSCHIEDE = difference;
				ADRVTRF.QUALITAET = similarity;
				// TODO BK 9 BK, Add if it is available
				//ADRVTRF.CHARAKTER = character;

				// Save
				olExtendedEntities.AddToADRVTRF(ADRVTRF);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return ADRVTRF;
		}
		#endregion
	}
}
