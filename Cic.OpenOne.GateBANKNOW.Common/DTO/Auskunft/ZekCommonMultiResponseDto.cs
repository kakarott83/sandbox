using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Multi Response Data Transfer Object ZEK
    /// </summary>
    public class ZekCommonMultiResponseDto
    {
        /// <summary>
        /// Getter/Setter Kreditgesuch ID
        /// </summary>
        public string KreditGesuchID { get; set; }

        /// <summary>
        /// Getter/Setter Vertrag ID
        /// </summary>
        public string KreditVertragID { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Responses
        /// </summary>
        public List<ZekResponseDescriptionDto> Responses { get; set; }

        /// <summary>
        /// Getter/Setter Transaktion Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }
    }
}
