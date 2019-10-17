using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class iFinVariantenDruckenDto
    {
        /// <summary>
        /// SysID des Antrags
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// ISOLanguageCode
        /// </summary>
        public string ISOLanguageCode { get; set; }
    }
}