using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Dto containing information about a service
    /// </summary>
    public class AvailableServiceDto : DropListDto
    {
        /// <summary>
        /// Flag to determine if the service is necessary (aka checked or needed)
        /// </summary>
        public bool selected { get; set; }

        /// <summary>
        /// Flag to determine if the service is editable (aka greyed out - not disabled)
        /// </summary>
        public bool editable { get; set; }

        /// <summary>
        /// Type of Service
        /// </summary>
        public ServiceType serviceType { get; set; }

        /// <summary>
        /// Flag to determine if the service is included in the interest rates
        /// </summary>
        public int mitfin { get; set; }

        /// <summary>
        /// Optional PaketCode
        /// </summary>
        public string paketCode { get; set; }
    }
}