using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    public class iSearchObtypDto:iSearchDto
    {
        public CarConfigSearchType obtypSearchType { get; set; }//used for java-client to access the enum-values
        /// <summary>
        /// allows filtering via sysobtyp in rvtpos.sysabrregel = sysobtyp
        /// when zero, no filtering occurs
        /// when rvt has zero rvtpos with code OBTYP, no filtering occurs
        /// </summary>
        public long sysrvt { get; set; }
        public long sysperole { get; set; }
    }

    public enum CarConfigSearchType 
    {
        NONE = 0,
        ANTRIEBSART = 1,
        CO2 = 2,
        TREIBSTOFF = 3,
        GETRIEBEART = 4,
        AUFBAU = 5,
        ZEITRAUMVON = 6,
        ZEITRAUMBIS = 7,
        MODELL = 8,
        MARKE = 9,
        TYPENCODE = 10,
        OBJEKTTYP = 11,
        MARKEBEZEICHNUNG = 12,
        SCHWACKE = 13,
        FUZZY = 14,
        ID = 15,
        VIN = 16,
        KWVON = 17,
        KWBIS = 18,
        TSN = 19,
        HSN = 20,
        KOMMNR = 21

    }
}