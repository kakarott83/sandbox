using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter for DMS -> OL Updates
    /// </summary>
    public class iDMSUploadDto
    {
        /// <summary>
        /// DMR-Vorgangsnummer
        /// Pflichtfeld
        /// </summary>
        public String caseId { get; set; }
        /// <summary>
        /// Dokument-Typ
        /// Pflichtfeld
        /// </summary>
        public String documentType { get; set; }
        /// <summary>
        /// DMS Dokumenten id
        /// Pflichtfeld
        /// </summary>
        public long documentId { get; set; }
        /// <summary>
        /// DMS Interface-Version String
        /// Pflichtfeld
        /// </summary>
        public String interfaceVersion { get; set; }
        /// <summary>
        /// Kundennummer
        /// Pflichtfeld sobald bekannt
        /// </summary>
        public long sysReferenz { get; set; }
        /// <summary>
        /// angebot/antrag/vt-Nummer
        /// Pflichtfeld sobald bekannt
        /// </summary>
        public String vtNumber { get; set; }

        /// <summary>
        /// List of dmr specific metadata
        /// </summary>
        public List<DMSField> values { get; set; }

    }
}
