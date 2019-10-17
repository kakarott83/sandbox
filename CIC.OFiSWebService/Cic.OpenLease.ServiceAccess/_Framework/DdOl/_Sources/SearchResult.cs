using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Ausgabe Suche Daten Transfer Objekt
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SearchResult<R>
    {
        /// <summary>
        /// Suchergebnismenge
        /// </summary>
        public int searchCountFiltered { get; set; }
        /// <summary>
        /// Maximale Suchergebnisse
        /// </summary>
        public int searchCountMax { get; set; }

        /// <summary>
        /// Suchergebnis Seitenanzahl
        /// </summary>
        public int searchNumPages { get; set; }
      
        /// <summary>
        /// Ergebnisse
        /// </summary>
        public List<R> results { get; set; }
    }
}
