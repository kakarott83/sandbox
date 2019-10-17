using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
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

        /// <summary>
        /// When set, the data will be exported to csv
        /// </summary>
        public ExportCSVDto csvExport { get; set; }

        /// <summary>
        /// Used for choosing a certain server side registered sql by code or a wfvid
        /// </summary>
        public String queryId { get; set; }

        /// <summary>
        /// When set to true, cache wont be used
        /// </summary>
        public bool flushCache { get; set; }

        /// <summary>
        /// Context the search runs in
        /// </summary>
        public WorkflowContext ctx { get; set; }
    }
}
