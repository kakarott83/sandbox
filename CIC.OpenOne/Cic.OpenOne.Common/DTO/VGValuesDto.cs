using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// VG Wertetabelle
    /// </summary>
    public class VGValuesDto
    {
        /// <summary>
        /// Wertegruppen ID
        /// </summary>
        public long sysvg { get; set; }
        /// <summary>
        /// X-Format
        /// </summary>
        public String XFormat { get; set; }
        /// <summary>
        /// Y-Format
        /// </summary>
        public String YFormat { get; set; }
        /// <summary>
        /// Gültig Von
        /// </summary>
        public DateTime ValidFrom { get; set; }
        /// <summary>
        /// Gültig bis
        /// </summary>
        public DateTime ValidUntil { get; set; }
        /// <summary>
        /// Zeilenskalierung
        /// </summary>
        public String LineScale { get; set; }
        /// <summary>
        /// SpaltenSkalierung
        /// </summary>
        public String ColScale { get; set; }
        /// <summary>
        /// Wert 1
        /// </summary>
        public double? V1 { get; set; }
        /// <summary>
        /// X1 Wert
        /// </summary>
        public String X1 { get; set; }
        /// <summary>
        /// Y1 wert
        /// </summary>
        public String Y1 { get; set; }
        /// <summary>
        /// Wert 2
        /// </summary>
        public double? V2 { get; set; }
        /// <summary>
        /// X2 Wert
        /// </summary>
        public String X2 { get; set; }
        /// <summary>
        /// Y2 Wert
        /// </summary>
        public String Y2 { get; set; }
        /// <summary>
        /// Wert 3
        /// </summary>
        public double? V3 { get; set; }
        /// <summary>
        /// Wert 4
        /// </summary>
        public double? V4 { get; set; }
    }
}
