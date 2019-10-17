// OWNER BK, 05-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class ADRVKORHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.ADRVKOR Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRVTRF adrvtrf, int streetCorr, string street, int housenumberCorr, string housenumber, int zipCorr, string zip, int cityCorr, string city)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// Check object
			if (adrvtrf == null)
			{
				// Throw exception
				throw new System.ArgumentException("adrvtrf");
			}

			try
			{
				// Return
				return MyInsert(olExtendedEntities, adrvtrf, streetCorr, street, housenumberCorr, housenumber, zipCorr, zip, cityCorr, city);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.ADRVKOR MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.ADRVTRF adrvtrf, int streetCorr, string street, int housenumberCorr, string housenumber, int zipCorr, string zip, int cityCorr, string city)
		{
			Cic.OpenLease.Model.DdOl.ADRVKOR ADRVKOR;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// STRASSEWERT
			if (street != null)
			{
				street = StringUtil.GetNullStartIndexSubstring(street, 255);
			}
			// HSNRWERT
			if (housenumber != null)
			{
				housenumber = StringUtil.GetNullStartIndexSubstring(housenumber, 50);
			}
			// PLZWERT
			if (zip != null)
			{
				zip = StringUtil.GetNullStartIndexSubstring(zip, 12);
			}
			// ORTWERT
			if (city != null)
			{
				city = StringUtil.GetNullStartIndexSubstring(city, 100);
			}

			try
			{
				// Create new object
				ADRVKOR = new Cic.OpenLease.Model.DdOl.ADRVKOR();

				// Set values
				ADRVKOR.ADRVTRF = adrvtrf;
				ADRVKOR.STRASSEKOR = streetCorr;
				ADRVKOR.STRASSEWERT = street;
				ADRVKOR.HSNRKOR = housenumberCorr;
				ADRVKOR.HSNRWERT = housenumber;
				ADRVKOR.PLZKOR = zipCorr;
				ADRVKOR.PLZWERT = zip;
				ADRVKOR.ORTKOR = cityCorr;
				ADRVKOR.ORTWERT = city;

				// Save
				olExtendedEntities.AddToADRVKOR(ADRVKOR);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return ADRVKOR;
		}
		#endregion
	}
}
