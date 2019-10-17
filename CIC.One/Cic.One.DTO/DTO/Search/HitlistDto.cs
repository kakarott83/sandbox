using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Enthält alle gefundenen Dokumenten-Infos
    /// </summary>
    public class HitlistDto
    {
        /// <summary>
        /// Anzahl der gesamt gefundenen Elemente
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Anzahl der zurückgelieferten Infos
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Falls ein Fehler existiert wird er hier rein geschrieben
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Alle zurückgelieferten Dokumenten-Infos
        /// </summary>
        public List<DocumentDto> Documents { get; set; }
    }
}