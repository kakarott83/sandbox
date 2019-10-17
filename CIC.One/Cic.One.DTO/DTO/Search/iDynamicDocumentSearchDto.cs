using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input für die Dynamic Document Search
    /// </summary>
    public class iDynamicDocumentSearchDto
    {
        /// <summary>
        /// Datenbestand (nicht optional)
        /// </summary>
        public string Dbas { get; set; }

        /// <summary>
        /// (Optional) Start des Suchzeitraumes
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        /// (Optional) Ende des Suchzeitraumes
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// (Optional) Deskriptoren
        /// </summary>
        public List<DescriptorInputDto> Descriptors { get; set; }
        
        /// <summary>
        /// (Optional) Zeitlimit in Sekunden (0: keine Beschränkung)
        /// </summary>
        public string Limit { get; set; }

        /// <summary>
        /// (Optional) Trefferbeschränkung (0: keine Beschränkung)
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// (Optional) Dateiendung (Dos-Format, z.B. „pdf“)
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// (Optional) Name des zu verwendenden Benutzerprofils
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        /// (Optional) 
        /// Sortierreihenfolge wird durch eine Komma separierte Liste
        /// von DeskriptorIDs festgelegt. 
        /// Ein Minuszeichen vor der ID kennzeichnet ein Abwärtssortierung.
        /// </summary>
        public string OrderBy { get; set; }
    }
}