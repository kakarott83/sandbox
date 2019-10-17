// OWNER: BK, 14-07-2008
namespace Cic.OpenOne.Util
{
	[System.CLSCompliant(true)]
	public static class CultureHelper
	{
		#region Private variables
		private static System.Collections.Generic.Dictionary<string, System.Globalization.CultureInfo> _ISOCurrencySymbolCultureInfoDictionary;
		#endregion

		#region Methods
		// NOTE BK, You always should use "RFC" instead of "Rfc"
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RFC")]
		// TEST BK 0 BK, Not tested
		public static bool ValidateRFC4646CultureName(string name)
		{
			System.Globalization.CultureInfo CultureInfo = null;

			// Check name
			if (StringHelper.IsTrimedNullOrEmpty(name))
			{
				// Throw exception
				throw new System.ArgumentException("name");
			}

			// TODO BK 0 BK, Add to CultureInfoHelper
			try
			{
				// Get culture info
				CultureInfo = new System.Globalization.CultureInfo(name);
			}
			catch
			{
				// Ignore exception
			}

			// Return, check culture info
			return ((CultureInfo != null) && ((CultureInfo.CultureTypes & System.Globalization.CultureTypes.SpecificCultures) == System.Globalization.CultureTypes.SpecificCultures));
		}

		// NOTE BK, You always should use "ISO" instead of "Iso"
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ISO")]
		// TEST BK 0 BK, Not tested
		public static bool ValidateISO4217CurrencyName(string name)
		{
			bool Valid = false;

			// Check name
			if (StringHelper.IsTrimedNullOrEmpty(name))
			{
				// Throw exception
                throw new System.ArgumentException("name");
			}

			// Create dictionary
			MyCreateISOCurrencySymbolCultureInfoDictionary();
			// Check containing
			Valid = _ISOCurrencySymbolCultureInfoDictionary.ContainsKey(name);

			// Return
			return Valid;
		}
		#endregion

		#region My methods
		private static void MyCreateISOCurrencySymbolCultureInfoDictionary()
		{
			System.Globalization.CultureInfo[] CultureInfos;
			System.Globalization.RegionInfo RegionInfo;

			// Check dictionary
			if (_ISOCurrencySymbolCultureInfoDictionary == null)
			{
				// Create dictionay
				_ISOCurrencySymbolCultureInfoDictionary = new System.Collections.Generic.Dictionary<string, System.Globalization.CultureInfo>();
				// Get the list of cultures. We are not interested in neutral cultures, since
				// currency and RegionInfo is only applicable to specific cultures
				CultureInfos = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
				// Loop through culture infos
				foreach (System.Globalization.CultureInfo CultureInfo in CultureInfos)
				{
					// Reset region info
					RegionInfo = null;
					// Create a RegionInfo from culture id. 
					// RegionInfo holds the currency ISO code
					RegionInfo = new System.Globalization.RegionInfo(CultureInfo.LCID);
					// Check region info
					if (RegionInfo != null)
					{
						// multiple cultures can have the same currency code
						if (!_ISOCurrencySymbolCultureInfoDictionary.ContainsKey(RegionInfo.ISOCurrencySymbol))
						{
							// Add
							_ISOCurrencySymbolCultureInfoDictionary.Add(RegionInfo.ISOCurrencySymbol, CultureInfo);
						}
					}
				}
			}
		}
		#endregion
	}
}
