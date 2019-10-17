// OWNER BK, 05-13-2009
using Cic.OpenOne.Common.Util;
namespace Cic.OpenLease.Model
{
	[System.CLSCompliant(true)]
	public static class ExtendedPropertyHelper
	{
		#region Private constants
		private const string CnstUnknown = "-"; // "[?]";
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static string DeliverPersonCompleteName(bool isPrivate, string title, string firstName, string name, string nameSuffix)
		{
			// Return
			return MyDeliverPersonCompleteName(isPrivate, title, firstName, name, nameSuffix);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverPersonTitle(bool isPrivate, string code, string title, string firstName, string name, string nameSuffix)
		{
			// Return
			return MyDeliverPersonTitle(isPrivate, code, title, firstName, name, nameSuffix);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverContractTitle(bool personIsPrivate, string personCode, string personTitle, string personFirstName, string personName, string personNameSuffix, string contractCode)
		{
			// Return
			return MyDeliverContractTitle(MyDeliverPersonTitle(personIsPrivate, personCode, personTitle, personFirstName, personName, personNameSuffix), contractCode);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverContractTitle(string personExtTitle, string contractCode)
		{
			// Return
			return MyDeliverContractTitle(personExtTitle, contractCode);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverZipCodeCity(string zipCode, string city)
		{
			// Return
			return MyDeliverZipCodeCity(zipCode, city);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverZipCodeCityCountryName(string zipCode, string city, string countryName)
		{
			// Return
			return MyDeliverZipCodeCityCountryName(DeliverZipCodeCity(zipCode, city), countryName);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverZipCodeCityCountryName(string extZipCodeCity, string countryName)
		{
			// Return
			return MyDeliverZipCodeCityCountryName(extZipCodeCity, countryName);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverBankAccountCompleteName(string number, string iban)
		{
			// Return
			return MyDeliverBankAccountCompleteName(number, iban);
		}

		// TEST BK 0 BK, Not tested
		public static string DeliverBankCodeCompleteName(string code, string name)
		{
			// Return
			return MyDeliverBankCodeCompleteName(code, name);
		}
		#endregion

		#region My methods
		private static string MyDeliverPersonCompleteName(bool isPrivate, string title, string firstName, string name, string nameSuffix)
		{
			string Text = string.Empty;

			// Check state
			if (isPrivate)
			{
				// Set text
				Text = MyDeliverPrivatePersonCompleteName(title, firstName, name, nameSuffix);
			}
			else
			{
				// Set text
				Text = MyDeliverCompanyPersonCompleteName(name, nameSuffix);
			}

			// Return
			return Text;
		}

		private static string MyDeliverCompanyPersonCompleteName(string name, string nameSuffix)
		{
			string Text = string.Empty;

			// Check values
			name = StringUtil.IsTrimedNullOrEmpty(name) ? string.Empty : name.Trim();
			nameSuffix = StringUtil.IsTrimedNullOrEmpty(nameSuffix) ? string.Empty : nameSuffix.Trim();

			// Add name
			Text = (Text + name);

			// Check text and name suffix
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(nameSuffix)))
			{
				// Add separator
				Text = (Text + " ");
			}
			// Add name suffix
			Text = (Text + nameSuffix);

			// Return
			return Text;
		}

		private static string MyDeliverPrivatePersonCompleteName(string title, string firstName, string name, string nameSuffix)
		{
			string Text = string.Empty;

			// Check values
			title = StringUtil.IsTrimedNullOrEmpty(title) ? string.Empty : title.Trim();
			firstName = StringUtil.IsTrimedNullOrEmpty(firstName) ? string.Empty : firstName.Trim();
			name = StringUtil.IsTrimedNullOrEmpty(name) ? string.Empty : name.Trim();
			nameSuffix = StringUtil.IsTrimedNullOrEmpty(nameSuffix) ? string.Empty : nameSuffix.Trim();

			// Add title
			Text = (Text + title);

			// Check text and first name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(firstName)))
			{
				// Add separator
				Text = (Text + " ");
			}
			// Add first name
			Text = (Text + firstName);

			// Check text and name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(name)))
			{
				// Add separator
				Text = (Text + " ");
			}
			// Add name
			Text = (Text + name);

			// Check text and name suffix
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(nameSuffix)))
			{
				// Add separator
				Text = (Text + " ");
			}
			// Add name suffix
			Text = (Text + nameSuffix);

			// Return
			return Text;
		}

		private static string MyDeliverPersonTitle(bool isPrivate, string code, string title, string firstName, string name, string nameSuffix)
		{
			string CompleteName;
			string Text = string.Empty;

			// Get values
			CompleteName = MyDeliverPersonCompleteName(isPrivate, title, firstName, name, nameSuffix);

			// Check values
			code = StringUtil.IsTrimedNullOrEmpty(code) ? string.Empty : code.Trim();
			CompleteName = StringUtil.IsTrimedNullOrEmpty(CompleteName) ? string.Empty : CompleteName.Trim();

			// Add code
			Text = (Text + code);

			// Check text and first name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(CompleteName)))
			{
				// Add separator
				Text = (Text + " | ");
			}

			// Add complete name
			Text = (Text + CompleteName);

			// Check value
			if (string.IsNullOrEmpty(Text))
			{
				// Set unknown
				Text = CnstUnknown;
			}

			// Return
			return Text;
		}

		private static string MyDeliverContractTitle(string personExtTitle, string contractCode)
		{
			string Text = string.Empty;

			// Check values
			personExtTitle = StringUtil.IsTrimedNullOrEmpty(personExtTitle) ? string.Empty : personExtTitle.Trim();
			contractCode = StringUtil.IsTrimedNullOrEmpty(contractCode) ? string.Empty : contractCode.Trim();

			// Add code
			Text = (Text + personExtTitle);

			// Check text and first name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(contractCode)))
			{
				// Add separator
				Text = (Text + " | ");
			}

			// Add complete name
			Text = (Text + contractCode);

			//// Check value
			//if (string.IsNullOrEmpty(Text))
			//{
			//    // Set unknown
			//    Text = CnstUnknown;
			//}

			// Return
			return Text;
		}

		private static string MyDeliverZipCodeCity(string zipCode, string city)
		{
			string Text = string.Empty;

			// Check values
			zipCode = (StringUtil.IsTrimedNullOrEmpty(zipCode) ? string.Empty : zipCode.Trim());
			city = (StringUtil.IsTrimedNullOrEmpty(city) ? string.Empty : city.Trim());

			// Add zip code
			Text = (Text + zipCode);

			// Check text and first name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(city)))
			{
				// Add separator
				Text = (Text + " ");
			}

			// Add city
			Text = (Text + city);

			// Return
			return Text;
		}

		private static string MyDeliverZipCodeCityCountryName(string extZipCodeCity, string countryName)
		{
			string Text = string.Empty;

			// Check values
			extZipCodeCity = (StringUtil.IsTrimedNullOrEmpty(extZipCodeCity) ? string.Empty : extZipCodeCity.Trim());
			countryName = (StringUtil.IsTrimedNullOrEmpty(countryName) ? string.Empty : countryName.Trim());

			// Add zip code
			Text = (Text + extZipCodeCity);

			// Check text and first name
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(countryName)))
			{
				// Add separator
				Text = (Text + " - ");
			}

			// Add city
			Text = (Text + countryName);

			// Return
			return Text;
		}

		private static string MyDeliverBankAccountCompleteName(string number, string iban)
		{
			string Text = string.Empty;

			// Check values
			number = (StringUtil.IsTrimedNullOrEmpty(number) ? string.Empty : number.Trim());
			iban = (StringUtil.IsTrimedNullOrEmpty(iban) ? string.Empty : iban.Trim());

			// Add number
			Text = (Text + number);

			// Check text and iban
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(iban)))
			{
				// Add separator
				Text = (Text + " ");
			}

            // Check iban
            if (!string.IsNullOrEmpty(iban))
            {
                // Add iban
                Text = (Text + "[" + iban + "]");
            }

			// Return
			return Text;
		}

		private static string MyDeliverBankCodeCompleteName(string code, string name)
		{
			string Text = string.Empty;

			// Check values
			code = (StringUtil.IsTrimedNullOrEmpty(code) ? string.Empty : code.Trim());
			name = (StringUtil.IsTrimedNullOrEmpty(name) ? string.Empty : name.Trim());

			// Add code
			Text = (Text + code);

			// Check text and iban
			if ((!string.IsNullOrEmpty(Text)) && (!string.IsNullOrEmpty(name)))
			{
				// Add separator
				Text = Text + " ";
			}

			// Add name
			Text = (Text + name);

			// Return
			return Text;
		}
		#endregion
	}
}
