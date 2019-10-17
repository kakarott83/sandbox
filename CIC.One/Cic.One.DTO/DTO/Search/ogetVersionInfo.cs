using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output für die getVersion-Methode
    /// </summary>
    public class ogetVersionInfo : oBaseDto
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// ITA_Frameware
        /// </summary>
        public string ITA_Frameware { get; set; }

        /// <summary>
        /// WFL_Frameware
        /// </summary>
        public string WFL_Frameware { get; set; }

        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright { get; set; }
    }
}