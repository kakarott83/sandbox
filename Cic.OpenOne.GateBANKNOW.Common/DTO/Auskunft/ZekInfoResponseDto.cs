using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Info Response Data Transfer Object
    /// </summary>
    public class ZekInfoResponseDto
    {
        /// <summary>
        /// Getter/Setter ZEK found Contracts
        /// </summary>
        public ZekFoundContractsDto FoundContracts { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Found Person
        /// </summary>
        public ZekAddressDescriptionDto FoundPerson { get; set; }

        /// <summary>
        /// Getter/Setter Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }

        /// <summary>
        /// Getter/Setter Synonyms
        /// </summary>
        public List<ZekAddressDescriptionDto> Synonyms { get; set; }

        /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }
    }
}
