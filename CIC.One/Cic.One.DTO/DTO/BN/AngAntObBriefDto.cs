using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// ANGOB Brief Dto
    /// </summary>
    public class AngAntObBriefDto
    {
      /// <summary>
      /// primary key
      /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Stammnummer (xxx.xxx.xxx)
        /// </summary>
        public String stammnummer { get; set; }
        /// <summary>
        /// Chassisnummer/Fahrgestellnummer
        /// </summary>
        public String fident { get; set; }
        /// <summary>
        /// Laufnummer
        /// </summary>
        public String laufnummer { get; set; }
        /// <summary>
        /// Importeurcode
        /// </summary>
        public String impcode { get; set; }
        /// <summary>
        /// Motorart
        /// </summary>
        public String motor { get; set; }
        /// <summary>
        /// Leistung
        /// </summary>
        public short kw { get; set; }
        /// <summary>
        /// Energieeffizienz
        /// </summary>
        public String energieeff { get; set; }
        /// <summary>
        /// Höchstgeschwindigkeit
        /// </summary>
        public short kmh { get; set; }
        /// <summary>
        /// Aufbau(Coupe, Targa …)
        /// </summary>
        public String aufbau { get; set; }
        /// <summary>
        /// Antriebsart (2x2, 4x4)
        /// </summary>
        public String antrieb { get; set; }
        /// <summary>
        /// Getriebe (Schaltung, Automatik)
        /// </summary>
        public String getriebe { get; set; }
        /// <summary>
        /// Treibstoff (Benzin, Diesel …)
        /// </summary>
        public String treibstoff { get; set; }
        /// <summary>
        /// Anzahl Plätze
        /// </summary>
        public short sitze { get; set; }
        /// <summary>
        /// Reifen vorne
        /// </summary>
        public String reifv { get; set; }
        /// <summary>
        /// Reifen hinten
        /// </summary>
        public String reifmuh { get; set; }
        /// <summary>
        /// Radstand
        /// </summary>
        public short stand { get; set; }
        /// <summary>
        /// Leergewicht inkl. Fahrer
        /// </summary>
        public short leergew { get; set; }
        /// <summary>
        /// Gesamtgewicht
        /// </summary>
        public short zulgew { get; set; }
        /// <summary>
        /// Nutzlast
        /// </summary>
        public short last { get; set; }
        /// <summary>
        /// Dachlast
        /// </summary>
        public short lastd { get; set; }
        /// <summary>
        /// Anhängelast ungebremst
        /// </summary>
        public short lastamb { get; set; }
        /// <summary>
        /// Anhängelast gebremst
        /// </summary>
        public short lastaob { get; set; }
        /// <summary>
        /// Verbrauch städtisch
        /// </summary>
        public double verbrauchstadt { get; set; }
        /// <summary>
        /// Verbrauch ausserstädtisch
        /// </summary>
        public double verbrauchausser { get; set; }
        /// <summary>
        /// Verbrauch gesamt
        /// </summary>
        public double verbrauchgesamt { get; set; }
        /// <summary>
        /// Tankinhalt
        /// </summary>
        public short tank { get; set; }
        /// <summary>
        /// Co2-Emissionen
        /// </summary>
        public double co2emi { get; set; }
        /// <summary>   
        /// NoX-Emissionen
        /// </summary>
        public double nox { get; set; }

        /// <summary>
        /// Hubraum
        /// </summary>
        public long hubraum { get; set; }

        /// <summary>
        /// Ecodestatus
        /// </summary>
        public string ecodestatus { get; set; }

        /// <summary>
        /// ecodeid
        /// </summary>
        public string ecodeid { get; set; }
        
        /// <summary>
        /// Aufbau
        /// </summary>
        public string aart { get; set; }
    }
}
