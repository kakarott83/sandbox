using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Search
{
    /// <summary>
    /// Sorting Class
    /// </summary>
    public class Sorting
    {
        /// <summary>
        /// Fieldname
        /// </summary>
        public string fieldname { get; set; }
        /// <summary>
        /// Sort Order
        /// </summary>
        public SortOrder order { get; set; }
    }
}
