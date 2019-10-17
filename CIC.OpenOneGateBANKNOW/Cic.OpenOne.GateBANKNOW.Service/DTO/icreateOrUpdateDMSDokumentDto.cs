using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für createOrUpdateDMSDokmentDto Methode
    /// data to update a document (the file) in the DMS via the DMS Documentimport Interface
    /// </summary>
    public class icreateOrUpdateDMSDokumentDto
    {
        /// <summary>
        /// area tech id
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// area
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// document id internal
        /// </summary>
        public long sysdmsdoc { get; set; }
       
    }
}
