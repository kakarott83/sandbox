using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
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
        /// SysAntrag 
        /// </summary>
        public long? sysantrag { get; set; }
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
        /// Persönliche Anwesenheit des Kunden bestätigt.
        /// </summary>
        public int? kdIdentFlag { get; set; }
        /// <summary>
        /// SysAngebot
        /// </summary>
        public long? sysangebot { get; set; }

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
        /// KundenID Partnersystem
        /// </summary>
        public string extreferenz { get; set; }
    }
}