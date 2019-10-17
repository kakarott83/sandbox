using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Deltavista Out Data transfer Object
    /// </summary>
    public class DeltavistaOutDto
    {
        /// <summary>
        /// Output for Addressvalidierung
        /// </summary>
        public List<DVAddressMatchDto> CandidateList { get; set; } 
        /// <summary>
        /// Output for Addressvalidierung
        /// </summary>
        public DVAddressMatchDto FoundAddress { get; set; } 
        /// <summary>
        /// Output for all Deltavista methods
        /// </summary>
        public DVTransactionErrorDto TransactionError { get; set; }
        /// <summary>
        /// Output for Bonitätsdaten
        /// </summary>
        public List<DVDebtEntryDto> DebtList { get; set; }      
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public List<DVManagementMemberDto> ManagementList { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public List<DVContactDescriptionDto> ContactList { get; set; }  
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public List<DVAddressDescriptionDto> SamePhoneList { get; set; }  
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public List<DVKeyValuePairDto> KeyValueList { get; set; }  
        /// <summary>
        /// Output for Addressvalidierung (kein Treffer, Trefferliste, eindeutiger Treffer)
        /// </summary>
        public int VerificationDecision { get; set; } 
        /// <summary>
        /// Output for Handelsregister-, Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        public int ReferenceNumber { get; set; }
        /// <summary>
        /// Output for Bonitätsdaten (gefunden/nicht gefunden, 0=keine Debts, 1=Debts vorhanden)
        /// </summary>
        public int? ReturnCode { get; set; }
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public DVAddressDescriptionDto HqAddress { get; set; }
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string DateEntry { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string DateLastVerified { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string DateKnownSince { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string ChNumber { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string FoundingDate { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string NogaCode02 { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string NogaCode02Description { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string NogaCode08 { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string NogaCode08Description { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int NumberOfShares { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int NumberOfEmployees { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string AuditingCompany { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string LastShabPublication { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string LastShabDate { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int CompanyStatus { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int Capital { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int CapitalPayed { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public string Purpose { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int LeaderShipSize { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int LeaderShipSizeNeg { get; set; } 
        /// <summary>
        /// Output for CompanyDetails
        /// </summary>
        public int ManagementSize { get; set; }
        /// <summary>
        /// Output for Handelsregisterauskunft
        /// </summary>
        public string Report { get; set; }
        /// <summary>
        /// Output for Handelsregisterauskunft
        /// </summary>
        public byte[] ReportBlob { get; set; }
        /// <summary>
        /// Output for Handelsregisterauskunft
        /// </summary>
        public string ReportBlobFormat { get; set; }
    }
}
