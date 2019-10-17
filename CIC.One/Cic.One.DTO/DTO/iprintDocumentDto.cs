using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Defines the input for printing a document with EAI
    /// </summary>
    public class iprintDocumentDto
    {
        /// <summary>
        /// Internal Document code
        /// </summary>
        public String code { get; set; }
        /// <summary>
        /// area id
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// Target-Area of the document
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// Internal Document code
        /// </summary>
        public String eaiartCode { get; set; }
        /// <summary>
        /// EAIHOT-Inputparameters, PARAM1 will always be syswfuser
        /// </summary>
        public String INPUTPARAMETER2 { get; set; }
        public String INPUTPARAMETER3 { get; set; }
        public String INPUTPARAMETER4 { get; set; }
        public String INPUTPARAMETER5 { get; set; }
    }
}
