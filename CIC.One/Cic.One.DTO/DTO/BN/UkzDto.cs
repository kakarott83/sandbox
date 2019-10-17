using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// zusatzdatenFirma
    /// </summary>
    public class UkzDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long sysukz { get; set; }
        /// <summary>
        /// Jahresumsatz 
        /// </summary>
        public double? jumsatz { get; set; }
        /// <summary>
        /// Eigenkapital 
        /// </summary>
        public double? ekapital { get; set; }
        /// <summary>
        /// Bilanzsumme 
        /// </summary>
        public double? bilanzwert { get; set; }
        /// <summary>
        /// Jahresgewinn (Ergebnis aus BWA) 
        /// </summary>
        public double? ergebnis1 { get; set; }
        /// <summary>
        /// Flüssige Mittel 
        /// </summary>
        public double? liquiditaet { get; set; }
        /// <summary>
        /// kurzfristige Verbindlichkeiten 
        /// </summary>
        public double? obliboeigen { get; set; }
        /// <summary>
        /// Datum letzter Jahresabschluss 
        /// </summary>
        public DateTime? ljabschl { get; set; }
        /// <summary>
        /// Anzahl Mitarbeiter 
        /// </summary>
        public short anzma { get; set; }
        /// <summary>
        /// Anzahl Betreibungen 
        /// </summary>
        public short anzvollstr { get; set; }
        /// <summary>
        /// Höhe Betreibungen 
        /// </summary>
        public double? betragvollstr { get; set; }
        /// <summary>
        /// Konkurs/Pfändung/Verlustschein 
        /// </summary>
        public bool konkursFlag { get; set; }

        /// <summary>
        /// infomailflag
        /// </summary>
        public int infomailflag { get; set; }

        /// <summary>
        /// infosmsflag
        /// </summary>
        public int infosmsflag { get; set; }

        /// <summary>
        /// infotelflag
        /// </summary>
        public int infotelflag { get; set; }

        /// <summary>
        /// Identifikationsdatum
        /// </summary>
        public DateTime? legitDatum { get; set; }
        /// <summary>
        /// Identifikationsmethode
        /// </summary>
        public string legitMethodCode { get; set; }
        /// <summary>
        /// Derjenige, der den Kunden identifiziert hat
        /// </summary>
        public string legitAbnehmer { get; set; }
        /// <summary>
        /// Kunde Identifiziert
        /// </summary>
        public int kdidentflag { get; set; }

    }
}