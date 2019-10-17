using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Deltavista Ingoing Data Transfer Object
    /// </summary>
    public class DeltavistaInDto
    {
        /// <summary>
        /// Input for Addressvalidierung, Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVAddressIdentification,DVorderCresuraReport")]
        public DVAddressDescriptionDto AddressDescription { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public DVOrderDescriptionDto OrderDescription { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string RefNo { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string Reason { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string ContactEmail { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string ContactFaxNr { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string ContactName { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string ContactTelDirect { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string BinaryPOI { get; set; }
        /// <summary>
        /// Input for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        [AttributeFilter("DVorderCresuraReport")]
        public string BinaryPOItype { get; set; }
        /// <summary>
        /// Input for Bonitätsdaten, CompanyDetails, Handelsregisterauskunft
        /// </summary>
        [AttributeFilter("DVgetCompanyDetails,DVgetDebtDetails,DVgetReport")]
        public int AddressId { get; set; }
        /// <summary>
        /// Input for Handelsregisterauskunft
        /// </summary>
        [AttributeFilter("DVgetReport")]
        public int ReportId { get; set; }
        /// <summary>
        /// Input for Handelsregisterauskunft
        /// </summary>
        [AttributeFilter("DVgetReport")]
        public string TargetFormat { get; set; }
    }
}