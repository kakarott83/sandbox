using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für Vertrag
    /// </summary>
    public class VertragDto : AngAntDto
    {
        /// <summary>
        /// Primary Antrag Key
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Primary VT Key
        /// </summary>
        public long sysvt { get; set; }

        /// <summary>
        /// Antragsnummer
        /// </summary>
        public String antrag { get; set; }

        /// <summary>
        /// Kunde
        /// </summary>
        public KundeDto kunde { get; set; }

        /// <summary>
        /// Kalkulationsdaten
        /// </summary>
        public KalkulationDto kalkulation { get; set; }

        /// <summary>
        /// Schild/Kennzeichen
        /// </summary>
        public String kennzeichen { get; set; }

        /// <summary>
        /// Marke
        /// </summary>
        public String marke { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        public String model { get; set; }
  
        /// <summary>
        /// stammnummer
        /// </summary>
        public String stammnummer { get; set; }
        /// <summary>
        /// Chassisnummer/Fahrgestellnummer
        /// </summary>
        public String chassisnummer { get; set; }
        /// <summary>
        /// Laufnummer
        /// </summary>
        public String laufnummer { get; set; }

        
        /// <summary>
        /// VertragsArt-Bezeichnung
        /// </summary>
        public String vertragsartbezeichnung { get; set; }

        /// <summary>
        /// Endeam von Antrag
        /// </summary>
        public DateTime? endeAm { get; set; }

        /// <summary>
        /// Ende von Antrag
        /// </summary>
        public DateTime? ende { get; set; }

        /// <summary>
        /// Vertragsbeginn
        /// </summary>
        public DateTime? beginn { get; set; }

        /// <summary>
        /// Freitexteingabe Objektbezeichnung
        /// </summary>
        public String bezeichnung { get; set; }

        /// <summary>
        /// Zustand (Status) Extern
        /// </summary>
        public String zustandExtern { get; set; }

        /// <summary>
        /// endekz von antrag
        /// </summary>
        public int endekz { get; set; }

        /// <summary>
        /// endekz von vertrag
        /// </summary>
        public int vtendekz { get; set; }

        /// <summary>
        /// aktivkz
        /// </summary>
        public int aktivkz { get; set; }

        /// <summary>
        /// denen der Zustandsraum für die Restwertrechnungsdruck erzeugt wurde CR29
        /// </summary>
        public String wfzustSyscode { get; set; }

        /// <summary>
        /// zustand vertrag in VTRUEK
        /// </summary>
        public String vtruekZustand { get; set; }

        /// <summary>
        /// Restwertgarant von antrag
        /// </summary>
        public long? sysrwga { get; set; }

        /// <summary>
        /// Restwertgarant von vertrag
        /// </summary>
        public long? vtsysrwga { get; set; }

        /// <summary>
        /// Pendente Auflösung
        /// </summary>
        public bool isPendenteAufloesung { get; set; }

        /// <summary>
        /// Restwertrechnung verschickt
        /// </summary>
        public bool isRwReVerschickt { get; set; }

        /// <summary>
        /// Restwertrechnung verschickt
        /// </summary>
        public bool isRREChangeAllowed { get; set; }

        /// <summary>
        /// Vertrag Zustand
        /// </summary>
        public String vtzustand { get; set; }

        /// <summary>
        /// vertrag
        /// </summary>
        public String vtvertrag { get; set; }

        /// <summary>
        /// Restwertverlängerung ist möglich
        /// </summary>
        public bool isExtendible { get; set; }

        /// <summary>
        /// Laufzeit in Monaten
        /// </summary>
        public long lz { get; set; }


        /// <summary>
        /// Ende von Antrag
        /// </summary>
        public DateTime? vtende { get; set; }

        /// <summary>
        /// Restwert
        /// </summary>
        public double rw { get; set; }

        /// <summary>
        /// rate
        /// </summary>
        public double ltrate { get; set; }

        /// <summary>
        /// Rate des Vorvertrags (aus ANTABL.AKTUELLERATE)
        /// </summary>
        public double ratevorvt { get; set; }

        /// <summary>
        /// Restwert inkl. Mwst des Vorvertrages
        /// </summary>
        public double rwvorvt { get; set; }

        /// <summary>
        /// OB.ZUBEHOERBRUTTO aus Vorvertrag
        /// </summary>
        public double zubehoervorvt { get; set; }

        /// <summary>
        /// Benutzer
        /// </summary>
        public string benutzer { get; set; }

        /// <summary>
        /// Buchwertberechnung erlaubt
        /// </summary>
        public bool isBuchwertCalculationAllowed { get; set; }

        /// <summary>
        /// aktuelle SYSVTRUEK der Buchwertberechnung
        /// </summary>
        public long currentSysVtruek { get; set; }

        /// <summary>
        /// Informationen zur aktuellsten Buchwertberechnung
        /// </summary>
        public BuchwertInfoDto buchwert { get; set; }
        /// <summary>
        /// Versicherungscode externes Versicherungsprodukt
        /// </summary>
        public string extvscode { get; set; }
    }
}