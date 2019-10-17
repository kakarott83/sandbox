using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetValuation"/> Methode
    /// </summary>
    public class iEurotaxGetValuationDto
    {
        /// <summary>
        /// sysobtyp
        /// </summary>
        public long sysobtyp { get; set; }

        /// <summary>
        /// Natcode (GetValuation)
        /// </summary>
        public long NationalVehicleCode { get; set; }

        /// <summary>
        /// Erstzulassung (GetValuation)
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// enum von countryCodes (GetValuation)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType ISOCountryCode { get; set; }

        /// <summary>
        /// enum von currencyCodes (GetValuation)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType ISOCurrencyCode { get; set; }

        /// <summary>
        /// enum von languageCodes (GetValuation)
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType ISOLanguageCode { get; set; }
        
        /// <summary>
        /// Mileage (GetValuation)
        /// </summary>
        public string Mileage { get; set; }

        /// <summary>
        /// Produktkontext zur Restwertermittlung
        /// </summary>
        public prKontextDto prodKontext { get; set; }

    }
}
