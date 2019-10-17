using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Out Data Transfer Object
    /// </summary>
    public class ZekOutDto
    {
        /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }

        /// <summary>
        /// Getter/Setter Kreditvertrag ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter Kreditgesuch ID
        /// </summary>
        public string KreditgesuchID { get; set; }

        /// <summary>
        /// Getter/Setter Responses
        /// </summary>
        public List<ZekResponseDescriptionDto> Responses { get; set; }

        /// <summary>
        /// Getter/Setter Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }

        /// <summary>
        /// Getter/Setter Found Person
        /// </summary>
        public ZekAddressDescriptionDto FoundPerson { get; set; }

        /// <summary>
        /// Getter/Setter Synonyms
        /// </summary>
        public List<ZekAddressDescriptionDto> Synonyms { get; set; }

        /// <summary>
        /// Getter/Setter SynonymsNew
        /// </summary>
        public List<ZekAddressDescriptionDto> SynonymsNew { get; set; }

        /// <summary>
        /// Getter/Setter Found Contracts
        /// </summary>
        public ZekFoundContractsDto FoundContracts { get; set; }

        /// <summary>
        /// Getter/Setter eCodeId
        /// </summary>
        public string eCodeId { get; set; }

        /// <summary>
        /// armResponse
        /// </summary>
        public ZekArmResponseDto armResponse { get; set; }
    }
}