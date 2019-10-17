using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Search;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Eingangs DTO Suche
    /// </summary>
    public class iSearchDto
    {
        /// <summary>
        /// Überspringen
        /// </summary>
        public int skip { get; set; }
        /// <summary>
        /// Anzahl
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// Seitengröße
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// Suchtyp
        /// </summary>
        public SearchType searchType { get; set; }

        /// <summary>
        /// Sortierfelder Array
        /// </summary>
        public Sorting[] sortFields { get; set; }
        /// <summary>
        /// Gruppierfelder Array
        /// </summary>
        public Grouping[] groupFields { get; set; }

        /// <summary>
        /// Filterbedingung und SuchFilter
        /// </summary>
        public Filter[] filters { get; set; }
    }
}
