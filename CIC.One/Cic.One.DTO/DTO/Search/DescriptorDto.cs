using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Descriptor eines Dokuments
    /// </summary>
    public class DescriptorDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Anzahl an Values
        /// </summary>
        public int CountValues { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public List<DescriptorValueDto> Values { get; set; }
    }
}