// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class PEINFOADRHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
		public static Cic.OpenLease.Model.DdOl.PEINFOADR Insert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int addresType, int legalForm, int sex, string firstName, string name, string maidenName, string street, string housenumber, string zip, string city, string country, System.DateTime? birthdate, string telephone, string fax, string mobile)
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
				return MyInsert(olExtendedEntities, peinfo, addresType, legalForm, sex, firstName, name, maidenName, street, housenumber, zip, city, country, birthdate, telephone, fax, mobile);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOl.PEINFOADR MyInsert(Cic.OpenLease.Model.DdOl.OlExtendedEntities olExtendedEntities, Cic.OpenLease.Model.DdOl.PEINFO peinfo, int addresType, int legalForm, int sex, string firstName, string name, string maidenName, string street, string housenumber, string zip, string city, string country, System.DateTime? birthdate, string telephone, string fax, string mobile)
		{
			Cic.OpenLease.Model.DdOl.PEINFOADR PEINFOADR = null;

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
				fax = StringUtil.GetNullStartIndexSubstring(fax, 40);
			}
			// HANDY
			if (mobile != null)
			{
				mobile = StringUtil.GetNullStartIndexSubstring(mobile, 40);
			}

			try
			{
				// Create new object
				PEINFOADR = new Cic.OpenLease.Model.DdOl.PEINFOADR();

				// Set values
				PEINFOADR.PEINFO = peinfo;
				PEINFOADR.ADRTYPE = addresType;
				PEINFOADR.RECHTSFORM = legalForm;
				PEINFOADR.GESCHLECHT = sex;
				PEINFOADR.VORNAME = firstName;
				PEINFOADR.NAME = name;
				PEINFOADR.MAEDCHENNAME = maidenName;
				PEINFOADR.STRASSE = street;
				PEINFOADR.HSNR = housenumber;
				PEINFOADR.PLZ = zip;
				PEINFOADR.ORT = city;
				PEINFOADR.LANDISO3166A3 = country;
				PEINFOADR.GEBDATUM = birthdate;
				PEINFOADR.TELEFON = telephone;
				PEINFOADR.FAX = fax;
				PEINFOADR.HANDY = mobile;

				// Save
				olExtendedEntities.AddToPEINFOADR(PEINFOADR);
				olExtendedEntities.SaveChanges();
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return PEINFOADR;
		}
		#endregion
	}
}
