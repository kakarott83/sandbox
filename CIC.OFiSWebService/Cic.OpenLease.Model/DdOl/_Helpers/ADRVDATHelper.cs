// OWNER BK, 05-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class ADRVDATHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.ADRVDAT Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, int legalForm, int sex, string firstName, string name, string maidenName, string street, string housenumber, string zip, string city, string country, System.DateTime? birthdate, string telephone, string fax, string mobile)
		{
			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}
			try
			{
				// Return
				return MyInsert(olExtendedEntities, legalForm, sex, firstName, name, maidenName, street, housenumber, zip, city, country, birthdate, telephone, fax, mobile);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.ADRVDAT MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, int legalForm, int sex, string firstName, string name, string maidenName, string street, string housenumber, string zip, string city, string country, System.DateTime? birthdate, string telephone, string fax, string mobile)
		{
			Cic.OpenLease.Model.DdOl.ADRVDAT ADRVDAT = null;

			// Check context
			if (olExtendedEntities == null)
			{
				// Throw exception
				throw new System.ArgumentException("owExtendedEntities");
			}

			// VORNAME
			if (firstName != null)
			{
				firstName = StringUtil.GetNullStartIndexSubstring(firstName, 60);
			}
			// NAME
			if (name != null)
			{
				name = StringUtil.GetNullStartIndexSubstring(name, 128);
			}
			// MAEDCHENNAME
			if (maidenName != null)
			{
				maidenName = StringUtil.GetNullStartIndexSubstring(maidenName, 128);
			}
			// STRASSE
			if (street != null)
			{
				street = StringUtil.GetNullStartIndexSubstring(street, 40);
			}
			// HSNR
			if (housenumber != null)
			{
				housenumber = StringUtil.GetNullStartIndexSubstring(housenumber, 20);
			}
			// PLZ
			if (zip != null)
			{
				zip = StringUtil.GetNullStartIndexSubstring(zip, 15);
			}
			// ORT
			if (city != null)
			{
				city = StringUtil.GetNullStartIndexSubstring(city, 40);
			}
			// LANDISO3166A3
			if (country != null)
			{
				country = StringUtil.GetNullStartIndexSubstring(country, 3);
			}
			// TELEFON
			if (telephone != null)
			{
				telephone = StringUtil.GetNullStartIndexSubstring(telephone, 40);
			}
			// FAX
			if (fax != null)
			{
                fax = Cic.OpenOne.Common.Util.StringUtil.GetNullStartIndexSubstring(fax, 40);
			}
			// HANDY
			if (mobile != null)
			{
				mobile = StringUtil.GetNullStartIndexSubstring(mobile, 40);
			}

			try
			{
				// Create new object
				ADRVDAT = new Cic.OpenLease.Model.DdOl.ADRVDAT();

				// Set values
				ADRVDAT.RECHTSFORM = legalForm;
				ADRVDAT.GESCHLECHT = sex;
				ADRVDAT.VORNAME = firstName;
				ADRVDAT.NAME = name;
				ADRVDAT.MAEDCHENNAME = maidenName;
				ADRVDAT.STRASSE = street;
				ADRVDAT.HSNR = housenumber;
				ADRVDAT.PLZ = zip;
				ADRVDAT.ORT = city;
				ADRVDAT.LANDISO3166A3 = country;
				ADRVDAT.GEBDATUM = birthdate;
				ADRVDAT.TELEFON = telephone;
				ADRVDAT.FAX = fax;
				ADRVDAT.HANDY = mobile;

				// Save
				olExtendedEntities.AddToADRVDAT(ADRVDAT);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return ADRVDAT;
		}
		#endregion
	}
}
