using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Search;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Ausgabe Suche Daten Transfer Objekt
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class oSearchDto<R> 
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
        /// Suchergebnisarray
        /// </summary>
        public GroupResult<R>[] groupResults { get; set; }
        /// <summary>
        /// Ergebnisse
        /// </summary>
        public R[] results { get; set; }
    }
}
