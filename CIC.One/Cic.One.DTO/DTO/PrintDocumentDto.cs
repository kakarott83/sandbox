using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class PrintDocumentDto
    {
        /// <summary>
        /// Internal Document code
        /// </summary>
        public String code { get; set; }
        /// <summary>
        /// Title of the document
        /// </summary>
        public String title { get; set; }
        /// <summary>
        /// Target-Area of the document
        /// </summary>
        public String area { get; set; }
    }
}
