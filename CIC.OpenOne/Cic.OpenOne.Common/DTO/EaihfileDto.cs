using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Eaihfile Dto
    /// </summary>
    public class EaihfileDto
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long SYSEAIHFILE { get; set; }

        public long SYSEAIHOT { get; set; }

        /// <summary>
        /// CODE
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public byte[] EAIHFILE { get; set; }

        public String TARGETFILENAME { get; set; }
        public String TARGETPATHSPEC { get; set; }
        

    }
}
