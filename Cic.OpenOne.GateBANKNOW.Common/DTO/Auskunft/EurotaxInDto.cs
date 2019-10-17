using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{

    /// <summary>
    /// Eurotax Input Data Transfer Objects
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxInDto
    {
        /// <summary>
        /// sysobtyp
        /// </summary>
        [AttributeLabel("Forecast Period From")]
        [AttributeFilter("GetForecast")]
        public long sysobtyp { get; set; }

        /// <summary>
        /// Natcode (GetForecast / GetValuation)
        /// </summary>
        [AttributeLabel("National Vehicle Code")]
        [AttributeFilter("GetValuation,GetForecast")]
        public long NationalVehicleCode { get; set; }

        /// <summary>
        /// Laufzeit Von (GetForecast)
        /// </summary>
        [AttributeLabel("Forecast Period From")]
        [AttributeFilter("GetForecast")]
        public string ForecastPeriodFrom { get; set; }

        /// <summary>
        /// Laufzeit Bis (GetForecast)
        /// </summary>
        [AttributeLabel("Forecast Period To")]
        [AttributeFilter("GetForecast")]
        public string ForecastPeriodUntil { get; set; }

        /// <summary>
        /// jährliche Laufleistung in Kilometer (GetForecast)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        [AttributeLabel("Estimated Annual Mileage Value")]
        [AttributeFilter("GetForecast")]
        public uint EstimatedAnnualMileageValue { get; set; }

        /// <summary>
        /// Aktueller Kilometerstand (GetForecast)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        [AttributeLabel("Current Mileage Value")]
        [AttributeFilter("GetForecast")]
        public uint? CurrentMileageValue { get; set; }

        /// <summary>
        /// Erstzulassung (GetForecast / GetValuation)
        /// </summary>        
        [AttributeLabel("Registration Date")]
        [AttributeFilter("GetValuation,GetForecast")]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// TotalListPriceOfEquipment (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        public double TotalListPriceOfEquipment { get; set; }

        /// <summary>
        /// enum von countryCodes (GetForecast)
        /// </summary>
        [AttributeLabel("ISOcountry Code")]
        [AttributeFilter("GetForecast")]
        public DAO.Auskunft.EurotaxRef.ISOcountryType ISOCountryCode { get; set; }

        /// <summary>
        /// enum von currencyCodes (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        [AttributeLabel("ISOcurrency Code")]
        public DAO.Auskunft.EurotaxRef.ISOcurrencyType ISOCurrencyCode { get; set; }

        /// <summary>
        /// enum von languageCodes (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        [AttributeLabel("ISOlanguage Code")]
        public DAO.Auskunft.EurotaxRef.ISOlanguageType ISOLanguageCode { get; set; }

        /// <summary>
        /// enum von countryCodes (GetValuation)
        /// </summary>
         [AttributeFilter("GetValuation")]
        public DAO.Auskunft.EurotaxValuationRef.ISOcountryType ISOCountryCodeValuation { get; set; }

        /// <summary>
        /// enum von currencyCodes (GetValuation)
        /// </summary>
         [AttributeFilter("GetValuation")]
        public DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType ISOCurrencyCodeValuation { get; set; }

        /// <summary>
        /// enum von languageCodes (GetValuation)
        /// </summary>
         [AttributeFilter("GetValuation")]
        public DAO.Auskunft.EurotaxValuationRef.ISOlanguageType ISOLanguageCodeValuation { get; set; }
        
        /// <summary>
        /// Mileage (GetValuation)
        /// </summary>
         [AttributeFilter("GetValuation")]
        public string Mileage { get; set; }


        /// <summary>
        /// Produktkontext zur Restwertermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }
    }
}
