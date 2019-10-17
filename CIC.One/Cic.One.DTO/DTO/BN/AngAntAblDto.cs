using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ablöseinformation Dto
    /// </summary>
    public class AngAntAblDto
    {
        /// <summary>
        /// Primärschlüssel Ablöseangebot 
        /// </summary>
        public long sysangabl { get; set; }
        /// <summary>
        /// Verweis zum Angebot 
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// Primärschlüssel Ablöseantrag 
        /// </summary>
        public long sysantabl { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// Verweis zum Ablösetyp (Eigen, Fremd) 
        /// </summary>
        public long sysabltyp { get; set; }
        /// <summary>
        /// Ablösetyp-Bezeichnung
        /// </summary>
        public String ablTypBezeichnung { get; set; }

        /// <summary>
        /// flagEpos
        /// </summary>
        public bool flagEpos { get; set; }
        /// <summary>
        /// Ablösesumme 
        /// </summary>
        public double betrag { get; set; }
        /// <summary>
        /// Aktuelle Rate abzulösender Vertrag 
        /// </summary>
        public double aktuelleRate { get; set; }
        /// <summary>
        /// Ablöse geprüft 
        /// </summary>
        public bool geprueftFlag { get; set; }

        /// <summary>
        /// Verweis zum abzulösenden Vertrag im Eigenbestand 
        /// </summary>
        public long sysvorvt { get; set; }
        /// <summary>
        /// Bezeichnung des abzulösenden Vertrags
        /// </summary>
        public String vorVtBezeichnung { get; set; }

        /// <summary>
        /// Fremdbank 
        /// </summary>
        public String bank { get; set; }
        /// <summary>
        /// Fremdbank Bankleitzahl (Clearingnummer) 
        /// </summary>
        public String blz { get; set; }
        /// <summary>
        /// Fremdbank Bic 
        /// </summary>
        public String bic { get; set; }
        /// <summary>
        /// Fremdbank Kontonummer des Kunden 
        /// </summary>
        public String kontonr { get; set; }
        /// <summary>
        /// Fremdbank Iban des Kunden 
        /// </summary>
        public String iban { get; set; }
        /// <summary>
        /// Fremdbank Vertrags-/Kontonummer des abzulösenden Vertrags 
        /// </summary>
        public String fremdvertrag { get; set; }
        /// <summary>
        /// Fremdbank Empfängerangaben Name, Vorname 
        /// </summary>
        public String empfaenger { get; set; }
        /// <summary>
        /// Fremdbank Empfängerangaben Strasse 
        /// </summary>
        public String strasse { get; set; }
        /// <summary>
        /// Fremdbank Empfängerangaben Plz 
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Fremdbank Empfängerangaben Ort 
        /// </summary>
        public String ort { get; set; }
        /// <summary>
        /// Bemerkung 
        /// </summary>
        public String bemerkung { get; set; }
        /// <summary>
        /// Datum des Ablöse 
        /// </summary>
        public DateTime datkalk { get; set; }
        /// <summary>
        /// Datum der Ablöseberechnung 
        /// </summary>
        public DateTime datkalkper { get; set; }

        // Ticket#2012072510000088 
        /// <summary>
        /// sysbn
        /// </summary>
        public long sysbn { get; set; }
    }
}
