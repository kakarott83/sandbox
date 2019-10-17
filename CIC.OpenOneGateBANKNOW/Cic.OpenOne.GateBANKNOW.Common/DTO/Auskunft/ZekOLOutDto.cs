using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class ZekOLOutDto : oBaseDto
    {
         /// <summary>
        /// Getter/Setter Transaction Error
        /// </summary>
        public ZekTransactionErrorDto TransactionError { get; set; }

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
        /// Getter/Setter Found Contracts
        /// </summary>
        public ZekOLFoundContractsDto FoundContracts { get; set; }


    }
}


