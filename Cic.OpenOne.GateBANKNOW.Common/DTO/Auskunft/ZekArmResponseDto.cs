using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZekArmResponseDto
    /// </summary>
    public class ZekArmResponseDto
    {
        /// <summary>
        /// armList
        /// </summary>
        public List<ZekArmItemDto> armList { get; set; }

        /// <summary>
        /// requestDate
        /// </summary>
        public string requestDate { get; set; }

    }
}
