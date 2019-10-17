using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cic.One.DTO
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
        /// <summary>
        /// Export-File Data
        /// </summary>
        public byte[] export { get; set; }

        /// <summary>
        /// Result Field Descriptions
        /// </summary>
        public List<Viewfield> fields { get; set; }
    }
}
