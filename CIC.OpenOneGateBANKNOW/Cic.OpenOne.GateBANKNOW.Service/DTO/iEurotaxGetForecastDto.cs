using Cic.OpenOne.Common.DTO.Prisma;
using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetForecast"/> Methode
    /// </summary>
    public class iEurotaxGetForecastDto
    {
        /// <summary>
        /// Natcode (GetForecast)
        /// </summary>
        public long sysobtyp { get; set; }

        /// <summary>
        /// Natcode (GetForecast)
        /// </summary>
        public long NationalVehicleCode { get; set; }

        /// <summary>
        /// Laufzeit von (GetForecast)
        /// </summary>
        public string ForecastPeriodFrom { get; set; }

        /// <summary>
        /// Laufzeit bis (GetForecast)
        /// </summary>
        public string ForecastPeriodUntil { get; set; }

        /// <summary>
        /// jährliche Laufleistung in Kilometer (GetForecast)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        [CLSCompliant(false)]
        public uint EstimatedAnnualMileageValue { get; set; }

        /// <summary>
        /// Aktueller Kilometerstand (GetForecast)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        [CLSCompliant(false)]
        public uint? CurrentMileageValue { get; set; }

        /// <summary>
        /// Erstzulassung (GetForecast)
        /// </summary>        
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// TotalListPriceOfEquipment (GetForecast)
        /// </summary>
        public double TotalListPriceOfEquipment { get; set; }

        /// <summary>
        /// enum von countryCodes (GetForecast)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOcountryType ISOCountryCode { get; set; }

        /// <summary>
        /// enum von currencyCodes (GetForecast)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOcurrencyType ISOCurrencyCode { get; set; }

        /// <summary>
        /// enum von languageCodes (GetForecast)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOlanguageType ISOLanguageCode { get; set; }

        /// <summary>
        /// Produktkontext zur Restwertermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }
    }
}
