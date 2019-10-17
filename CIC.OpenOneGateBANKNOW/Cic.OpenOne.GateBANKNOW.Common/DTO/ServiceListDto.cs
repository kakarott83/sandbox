using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Dto containing information about a service
    /// </summary>
    public class ServiceListDto : DropListDto
    {
        /// <summary>
        /// Flag to determine if the service is necessary (aka checked)
        /// </summary>
        public bool needed { get; set; }

        /// <summary>
        /// Flag to determine if the service is disabled (aka greyed out)
        /// </summary>
        public bool disabled { get; set; }
    }
}
