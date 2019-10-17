using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Info zu einem Dokument
    /// </summary>
    public class DocumentDto
    {
        /// <summary>
        /// Id des Dokuments
        /// </summary>
        public string DocId { get; set; }

        /// <summary>
        /// Datum des Dokuments
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// Anzahl der Descriptoren
        /// </summary>
        public int CountDescriptors { get; set; }

        /// <summary>
        /// Descriptoren
        /// </summary>
        public List<DescriptorDto> Descriptors { get; set; }
    }
}