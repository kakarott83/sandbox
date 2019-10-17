using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input für DocumentLoad
    /// </summary>
    public class iDocumentLoadDto
    {
        /// <summary>
        /// ITA-Dokumenten-ID (nicht optional)
        /// </summary>
        public string Docid { get; set; }

        /// <summary>
        /// (Optional) Dateiendung (Dos-Format, z.B. „pdf“)
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// (Optional) Name des zu verwendenden Benutzerprofils
        /// </summary>
        public string ProfileName { get; set; }
    }
}