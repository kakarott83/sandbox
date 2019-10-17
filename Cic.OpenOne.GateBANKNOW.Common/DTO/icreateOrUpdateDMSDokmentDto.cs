using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für createOrUpdateDMSDokmentDto Methode
    /// data to update a document (the file) in the DMS via the DMS Documentimport Interface
    /// </summary>
    public class icreateOrUpdateDMSDokmentDto
    {
      
        /// <summary>
        /// dms akte id
        /// </summary>
        public long sysdmsakte { get; set; }

        /// <summary>
        /// dms document id which shall be uploaded
        /// </summary>
        public long sysdmsdoc { get; set; }
    }
}
