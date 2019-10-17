using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// General Angebot Antrag Ob Dto
    /// </summary>
    public class AngAntObDto
    {
        /// <summary>
        /// PKEY
        /// </summary>
        public long sysob { get; set; }
        /// <summary>
        /// Objekt-Bezeichnung
        /// </summary>
        public String obBezeichnung { get; set; }

        /// <summary>
        /// Verweis zum Angebot
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// Verweis zum Antrag
        /// </summary>
        public long sysantrag { get; set; }

        
        /// <summary>
        /// Verweis zum Objekttyp (Typ=Level5 bzw ETGTYPE)
        /// </summary>
        public long sysobtyp { get; set; }
        /// <summary>
        /// Objekttyp-Bezeichnung
        /// </summary>
        public String obTypBezeichnung { get; set; }
        
        /// <summary>
        /// Verweis zur Objektart (Neu, gebraucht etc.)
        /// </summary>
        public long sysobart { get; set; }
        /// <summary>
        /// Objektart-Bezeichnung
        /// </summary>
        public String obArtBezeichnung { get; set; }
        
        
        
        /// <summary>
        /// Freitexteingabe
        /// </summary>
        public String bezeichnung { get; set; }
        /// <summary>
        /// Marke aus Objekttyp (Typ=Level2 bzw ETGMAKE)
        /// </summary>
        public String hersteller { get; set; }
        /// <summary>
        /// Fabrikat bzw Modell aus Objekttyp (Typ=Level4 bzw ETGMODEL)
        /// </summary>
        public String fabrikat { get; set; }
        /// <summary>
        /// Baujahr
        /// </summary>
        public DateTime? baujahr { get; set; }
        /// <summary>
        /// Baumonat
        /// </summary>
        public String baumonat { get; set; }
        /// <summary>
        /// Erstzulassung
        /// </summary>
        public DateTime? erstzulassung { get; set; }
        /// <summary>
        /// Lieferdatum (voraussichtlich)
        /// </summary>
        public DateTime? liefer { get; set; }
        /// <summary>
        /// Schild/Kennzeichen
        /// </summary>
        public String kennzeichen { get; set; }
        /// <summary>
        /// Farbe (aussen)
        /// </summary>
        public String farbeA { get; set; }
        /// <summary>
        /// Übernahmekilometer
        /// </summary>
        public long ubnahmeKm { get; set; }
        /// <summary>
        /// Jahreskilometer
        /// </summary>
        public long jahresKm { get; set; }
        /// <summary>
        /// Betrag pro Mehrkilometer
        /// </summary>
        public double satzmehrKm { get; set; }
        /// <summary>
        /// Neupreis
        /// </summary>
        public double grund { get; set; }
        /// <summary>
        /// Neupreis Umsatzsteuer
        /// </summary>
        public double grundUst { get; set; }
        /// <summary>
        /// Neupreis Brutto
        /// </summary>
        public double grundBrutto { get; set; }
        /// <summary>
        /// Zubehör (Gesamtsumme)
        /// </summary>
        public double zubehoer { get; set; }
        /// <summary>
        /// Zubehör Umsatzsteuer
        /// </summary>
        public double zubehoerUst { get; set; }
        /// <summary>
        /// Zubehör Brutto
        /// </summary>
        public double zubehoerBrutto { get; set; }
        /// <summary>
        /// Barkaufpreis
        /// </summary>
        public double ahk { get; set; }
        /// <summary>
        /// Barkaufpreis Umsatzsteuer
        /// </summary>
        public double ahkUst { get; set; }
        /// <summary>
        /// Barkaufpreis Brutto
        /// </summary>
        public double ahkBrutto { get; set; }
        /// <summary>
        /// Fahrer (Name, Vorname)
        /// </summary>
        public String fahrer { get; set; }
        /// <summary>
        /// Verweis zum Fahrertyp, bspw wie LN oder Dritter (Lookup und Übersetzung)
        /// </summary>
        public String fahrerCode { get; set; }
        /// <summary>
        /// Versicherung Name
        /// </summary>
        public String versicherungName { get; set; }
        /// <summary>
        /// Versicherung Ort
        /// </summary>
        public String versicherungOrt { get; set; }
        /// <summary>
        /// Versicherung Nummer
        /// </summary>
        public String versicherungNr { get; set; }

        /// <summary>
        /// Brief
        /// </summary>
        public AngAntObBriefDto brief { get; set; }

        /// <summary>
        /// Fahrzeugart (Boot, PW)
        /// </summary>
        public String fzart { get; set; }

        /// <summary>
        /// Typengenehmigung
        /// </summary>
        public String typengenehmigung { get; set; }

        /// <summary>
        /// Schwacke-Code
        /// </summary>
        public String schwacke { get; set; }

        /// <summary>
        /// Polsterfarbe
        /// </summary>
        public String polsterfarbe { get; set; }

        /// <summary>
        /// Innerfarbe
        /// </summary>
        public String farbeI { get; set; }

        /// <summary>
        /// Seriennummer
        /// </summary>
        public String serie { get; set; }

        /// <summary>
        /// Ausstattung
        /// </summary>
        public List<AngAntObAustDto> aust { get; set; }

        /// <summary>
        /// Baugruppe
        /// </summary>
        public String baugruppe { get; set; }

        /// <summary>
        /// zustand
        /// </summary>
        public String zustand { get; set; }

        /// <summary>
        /// nutzungsart
        /// </summary>
        public String nutzungsart { get; set; }

        /// <summary>
        /// nutzungsart
        /// </summary>
        public String quellerw { get; set; }

        /// <summary>
        /// Modell
        /// </summary>
        public String modell { get; set; }
    }
}
