using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateBANKNOW.DTO
{
    /// <summary>
    /// Kalkulationsdaten BN Partner Interface (PORSCHE)
    /// </summary>
    public class KalkulationDto
    {
        /*	Barkaufpreis/Kreditbetrag/Kreditlimit exkl Zinsen, Ratenabsicherung, Steuern BRUTTO 	*/
        public double bginternbrutto { get; set; }
        /*	Laufleistung bzw Jahreskilometer 	*/
        public long ll { get; set; }
        /*	Laufzeit 	*/
        public short lz { get; set; }
        /*	Rate Brutto 	*/
        public double rateBrutto { get; set; }
        /*	Restwert Brutto Restrate	*/
        public double rwBrutto { get; set; }
        /*	Verweis zur Nutzungsart (privat, geschäftlich, demo)  Mapping Excel NUTZUNGSART	*/
        public long sysobusetype { get; set; }
        /*	Sonderzahlung Brutto 1. Rate (Wenn 1. Rate = Folgeraten -> SzBrutto=0)	*/
        public double szBrutto { get; set; }
        /*	Zinssatz nominell Jahr 	*/
        public double zins { get; set; }
        /*	Zinsatz Kundenzins bei Differenzleasing	*/
        public double zinscust { get; set; }
        /*	Kaution (Depot) 	*/
        public double depot { get; set; }
        /*	Satzmehrkm OB_MARK_SATZMEHRKM 	*/
        public double satzmehrkm { get; set; }


    }
}