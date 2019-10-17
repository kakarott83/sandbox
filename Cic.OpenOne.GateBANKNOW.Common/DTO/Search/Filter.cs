using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Search
{
    /// <summary>
    /// Filter Klasee
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Filter Feldname
        /// </summary>
        public String fieldname { get; set; }
        /// <summary>
        /// Filterwert
        /// </summary>
        public String value { get; set; }
        /// <summary>
        /// Filterart
        /// </summary>
        public FilterType filterType { get; set; }
    }
}
