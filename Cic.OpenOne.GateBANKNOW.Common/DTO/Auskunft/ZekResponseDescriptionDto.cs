using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Response description Data Access Object
    /// </summary>
    public class ZekResponseDescriptionDto
    {
        /// <summary>
        ///  Getter/Setter  Synonyms
        /// </summary>
        public List<ZekAddressDescriptionDto> Synonyms { get; set; }

        /// <summary>
        ///  Getter/Setter Found Person
        /// </summary>
        public ZekAddressDescriptionDto FoundPerson { get; set; }

        /// <summary>
        ///  Getter/Setter Found Contracts
        /// </summary>
        public ZekFoundContractsDto FoundContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Return Code
        /// </summary>
        public ZekReturnCodeDto ReturnCode { get; set; }

        /// <summary>
        ///  Getter/Setter 
        /// </summary>
        public int RefNo { get; set; }
    }
}