using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class FileDto
    { /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysFile { get; set; }

        /// <summary>
        /// Id zur Area
        /// </summary>
        public long? sysId { get; set; }

        /// <summary>
        /// Area an dem das File hängt
        /// </summary>
        public string area { get; set; }

        /// <summary>
        ///  Name der angehängten Datei 
        /// </summary>
        public String fileName { get; set; }

        /// <summary>
        /// Ort der angehängten Datei 
        /// </summary>
        public String fileLocation { get; set; }

        /// <summary>
        /// Grösse der angehängten Datei 
        /// </summary>
        public long fileSize { get; set; }

        /// <summary>
        /// Inhalt 
        /// </summary>
        public byte[] content { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public String description { get; set; }

        /// <summary>
        /// format 
        /// </summary>
        public String format { get; set; }

        /// <summary>
        /// syscrtuser
        /// </summary>
        public long syscrtuser { get; set; }

        /// <summary>
        /// sysCrtDate
        /// </summary>
        public DateTime? sysCrtDate { get; set; }

        /// <summary>
        /// sysCrtTime
        /// </summary>
        public long sysCrtTime { get; set; }

        /// <summary>
        /// activFlag
        /// </summary>
        public bool activFlag { get; set; }

        /// <summary>
        /// syswfuser
        /// </summary>
        public long syswfuser { get; set; }
    }
}
