using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für getDocumentDto-Methode
    /// </summary>
    public class igetDocumentDto
    {
        /// <summary>
        /// Id of the document to load
        /// </summary>
        public long sysdmsdoc { get; set; }
        /// <summary>
        /// Template code of document to load
        /// </summary>
        public String wftxCode { get; set; }
    }
}
