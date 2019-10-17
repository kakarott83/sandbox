using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider
{
    class EurotaxGlobalization
    {

        #region Methods

        // Converts Eurotax language code to .NET language code
        public static String GetNativeLanguageCode(String EurotaxLanguageCode)
        {
            String NativeCode = "";

            switch (EurotaxLanguageCode)
            {
                case "ATDE": NativeCode = "de-AT"; break;
                case "BEFR": NativeCode = "fr-BE"; break;
                case "BENL": NativeCode = "nl-BE"; break;
                case "CHDE": NativeCode = "de-CH"; break;
                case "CHFR": NativeCode = "fr-CH"; break;
                case "CHIT": NativeCode = "it-CH"; break;
                case "CZCZ": NativeCode = "cs-CZ"; break;
                case "DEDE": NativeCode = "de-DE"; break;
                case "ESES": NativeCode = "es-ES"; break;
                case "FRFR": NativeCode = "fr-FR"; break;
                case "GBBR": NativeCode = "en-GB"; break;
                case "HUHU": NativeCode = "hu-HU"; break;
                case "ITIT": NativeCode = "it-IT"; break;
                case "NLNL": NativeCode = "nl-NL"; break;
                case "PLPL": NativeCode = "pl-PL"; break;
                case "PTPT": NativeCode = "pt-PT"; break;
                case "SISI": NativeCode = "sl-SI"; break;
                case "SKSK": NativeCode = "sk-SK"; break;
                case "RORO": NativeCode = "ro-RO"; break;
                case "EUBR": NativeCode = "en"; break;
                case "EUDE": NativeCode = "de"; break;
            }

            return NativeCode;
        }

        // Converts .NET language code to Eurotax language code
        public static String GetEurotaxLanguageCode(String NativeLanguageCode)
        {
            String EurotaxCode = "";

            switch (NativeLanguageCode)
            {
                case "de-AT": EurotaxCode = "ATDE"; break;
                case "fr-BE": EurotaxCode = "BEFR"; break;
                case "nl-BE": EurotaxCode = "BENL"; break;
                case "de-CH": EurotaxCode = "CHDE"; break;
                case "fr-CH": EurotaxCode = "CHFR"; break;
                case "it-CH": EurotaxCode = "CHIT"; break;
                case "cs-CZ": EurotaxCode = "CZCZ"; break;
                case "de-DE": EurotaxCode = "DEDE"; break;
                case "es-ES": EurotaxCode = "ESES"; break;
                case "fr-FR": EurotaxCode = "FRFR"; break;
                case "en-GB": EurotaxCode = "GBBR"; break;
                case "hu-HU": EurotaxCode = "HUHU"; break;
                case "it-IT": EurotaxCode = "ITIT"; break;
                case "nl-NL": EurotaxCode = "NLNL"; break;
                case "pl-PL": EurotaxCode = "PLPL"; break;
                case "pt-PT": EurotaxCode = "PTPT"; break;
                case "sl-SI": EurotaxCode = "SISI"; break;
                case "sk-SK": EurotaxCode = "SKSK"; break;
                case "ro-RO": EurotaxCode = "RORO"; break;
                case "en": EurotaxCode = "EUBR"; break;
                case "de": EurotaxCode = "EUDE"; break;
            }

            return EurotaxCode;
        }

        // Converts .NET language code to Eurotax market code
        public static String GetEurotaxMarketCode(String NativeLanguageCode)
        {
            String MarketCode = "";

            switch (NativeLanguageCode)
            {
                case "de-AT": MarketCode = "AT"; break;
                case "fr-BE": MarketCode = "BE"; break;
                case "nl-BE": MarketCode = "BE"; break;
                case "de-CH": MarketCode = "CH"; break;
                case "fr-CH": MarketCode = "CH"; break;
                case "it-CH": MarketCode = "CH"; break;
                case "cs-CZ": MarketCode = "CZ"; break;
                case "de-DE": MarketCode = "DE"; break;
                case "es-ES": MarketCode = "ES"; break;
                case "fr-FR": MarketCode = "FR"; break;
                case "en-GB": MarketCode = "GB"; break;
                case "hu-HU": MarketCode = "HU"; break;
                case "it-IT": MarketCode = "IT"; break;
                case "nl-NL": MarketCode = "NL"; break;
                case "pl-PL": MarketCode = "PL"; break;
                case "pt-PT": MarketCode = "PT"; break;
                case "sl-SI": MarketCode = "SI"; break;
                case "sk-SK": MarketCode = "SK"; break;
                case "ro-RO": MarketCode = "RO"; break;
                case "en": MarketCode = "EU"; break;
                case "de": MarketCode = "EU"; break;
            }

            return MarketCode;
        }

        // Converts .NET currency code to Eurotax currency code
        public static String GetEurotaxCurrencyCode(String NativeCurrencyCode)
        {
            String EurotaxCode = "";

            switch (NativeCurrencyCode)
            {
                case "EUR": EurotaxCode = "EUR"; break;
                case "CHF": EurotaxCode = "CHF"; break;
                case "CZK": EurotaxCode = "CZK"; break;
                case "GBP": EurotaxCode = "GBP"; break;
                case "HUF": EurotaxCode = "HUF"; break;
                case "PLN": EurotaxCode = "PLZ"; break;
                case "SIT": EurotaxCode = "SIT"; break;
                case "SKK": EurotaxCode = "SKK"; break;
            }

            return EurotaxCode;
        }

        // Converts Eurotax currency code to .NET currency code
        public static String GetNativeCurrencyCode(String EurotaxCurrencyCode)
        {
            String NativeCode = "";

            switch (EurotaxCurrencyCode)
            {
                case "EUR": NativeCode = "EUR"; break;
                case "CHF": NativeCode = "CHF"; break;
                case "CZK": NativeCode = "CZK"; break;
                case "GBP": NativeCode = "GBP"; break;
                case "HUF": NativeCode = "HUF"; break;
                case "PLZ": NativeCode = "PLN"; break;
                case "SIT": NativeCode = "SIT"; break;
                case "SKK": NativeCode = "SKK"; break;
            }

            return NativeCode;
        }

        #endregion

    }
}
