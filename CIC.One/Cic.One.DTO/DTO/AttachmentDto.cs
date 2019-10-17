using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Datei-Anhang
    /// </summary>
    public class AttachmentDto 
    {

        /// <summary>
        /// Daten
        /// </summary>
        public byte[] attachment
        {
            get;
            set;
        }

        /// <summary>
        /// Typ des Attachments
        /// </summary>
        public String mimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Typ des Bezeichnung
        /// </summary>
        public String bezeichnung
        {
            get;
            set;
        }
        /// <summary>
        /// Typ des Beschreibung
        /// </summary>
        public String beschreibung
        {
            get;
            set;
        }

        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysid
        {
            get;
            set;
        }
    }
}
