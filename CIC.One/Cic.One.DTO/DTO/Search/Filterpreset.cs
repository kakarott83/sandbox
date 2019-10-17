using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cic.One.DTO
{
    /// <summary>
    /// Filterpreset Klasse
    /// Allows configuring a filter-template choosable from the user
    /// </summary>
    public class Filterpreset
    {
        /// <summary>
        /// Preset Name
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// Preset Configuration (JSON)
        /// </summary>
        public String config { get; set; }

        /// <summary>
        /// Filtertemplate selected by default
        /// </summary>
        public bool selected { get; set; }
        /// <summary>
        /// Filter can be removed from user
        /// </summary>
        public bool removable { get; set; }
        
    }
}
