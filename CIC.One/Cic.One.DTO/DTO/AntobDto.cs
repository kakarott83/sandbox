using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AntobDto : ObjektDto
    {
        /// <summary>
        /// Extended Object Info
        /// </summary>
        public AntobIniDto zusatzdaten { get; set; }

        
        public double lpBrutto { get; set; }

        /// <summary>
        /// Brief-Daten
        /// </summary>
        public AntobbriefDto briefdaten { get; set; }
        /// <summary>
        /// Briefnummer
        /// </summary>
        public String brief { get; set; }

    }
}