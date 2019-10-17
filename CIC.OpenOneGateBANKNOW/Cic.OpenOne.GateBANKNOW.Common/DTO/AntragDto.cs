using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für AntragDto
    /// </summary>
    public class AntragDto : AngAntDto
    {
        /// <summary>
        /// Primary Antrag Key
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Antragsnummer
        /// </summary>
        public String antrag { get; set; }

        /// <summary>
        /// Kunde
        /// </summary>
        public KundeDto kunde { get; set; }

        /// <summary>
        /// Mitantragsteller
        /// </summary>
        public KundeDto mitantragsteller { get; set; }

        /// <summary>
        /// AntragstellerTyp Partner(800)/Solidarschuldner(120)/Bürge(130)
        /// Verwendet für den 2. AS
        /// </summary>
        public int mitantragstellerTyp { get; set; }

        /// <summary>
        /// Kalkulationsdaten
        /// </summary>
        public KalkulationDto kalkulation;

        /// <summary>
        /// Drucksperre
        /// </summary>
        public int Drucksperre { get; set; }

        /// <summary>
        /// Verweis zum Produkt
        /// </summary>
        public long? sysprprod { get; set; }
        /// <summary>
        /// Produkt-Bezeichnung
        /// </summary>
        public String prProductBezeichnung { get; set; }
        /// <summary>
        /// Produkt-Code
        /// </summary>
        public String prProductCode { get; set; }

        /// <summary>
        /// Zusammenfassung
        /// </summary>
        public String zusammenfassung { get; set; }

        /// <summary>
        /// Liste der AuflagenTexten
        /// </summary>
        public String[] auflagenText { get; set; }

        /// <summary>
        /// Liste der Zustände in chronologischer Reihenfolge
        /// </summary>
        public ZustandDto[] zustaende { get; set; }

        /// <summary>
        /// Restwertgarant
        /// </summary>
        public long? sysrwga { get; set; }

        /// <summary>
        /// Zustand (Status) Extern
        /// </summary>
        public String zustandExtern { get; set; }

        // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung
        /// <summary>
        /// Ablöse 1
        /// </summary>
        public double abloese1 { get; set; }

        /// <summary>
        /// Ablöse 2
        /// </summary>
        public double abloese2 { get; set; }

        /// <summary>
        /// Ablöse 3
        /// </summary>
        public double abloese3 { get; set; }

        /// <summary>
        /// Buchwertgarantie
        /// </summary>
        public int flagBWGarantie { get; set; }

        /// <summary>
        /// Vertragstyp
        /// </summary>
        public long? sysvttyp { get; set; }

        /// <summary>
        /// prozesscode / wert „UMSCHREIBUNG“
        /// </summary>
        public String prozesscode { get; set; }


        /// <summary>
        /// Verknüfung zu PRJOKER
        /// </summary>
        public long sysprjoker { get; set; }

        /// <summary>
        /// zustandBemerkung / ddlkppos ANTRAGSZUSTAND BNRNEUN CR 24
        /// </summary>
        public string zustandBemerkung { get; set; }

        /// <summary>
        /// Verlängerungen verweisen auf den Vorvertrags
        /// </summary>
        public long sysvorvt { get; set; }

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
        /// Zähler der RW-Verlängerung (aus ANTRAG.COUNTRENEWVAL)
        /// </summary>
        public long contractextcount { get; set; }

        /// <summary>
        /// Flag für RW-Verlängerung (1 für countrenewval>0 sonst 0)
        /// </summary>
        public int contractext { get { return contractextcount > 0 ? 1 : 0; } set { } }

        /// <summary>
        /// Vorvertragsnummer
        /// </summary>
        public String nrvorvt { get; set; }

        /// <summary>
        /// Vertrag-ID
        /// </summary>
        public long sysvt { get; set; }

        /// <summary>
        /// Vorvertragsnummer
        /// </summary>
        public string fform { get; set; }

        /// <summary>
        /// ProFinLock
        /// </summary>
        public int ProFinLock { get; set; }

        /// <summary>
        /// Auszahlungsdatum 
        /// </summary>
        public DateTime? auszahlungsdatum { get; set; }

        /// <summary>
        /// Opportunity-ID
        /// </summary>
        public String extreferenz { get; set; }

        /// <summary>
        /// Flag für gültige Auskunft, wenn true ist keine erneute CRIF-Anfrage notwendig
        /// auskunft.statusnum=0 und it.crrsid!=null und it.feststellungspflicht!=null
        /// </summary>
        public int CRIFOK { get; set; }


        /// <summary>
        /// KNE Bestaetigung - ANTOPTION.OPTION2
        /// </summary>
        public String KNEBestaetigung { get; set; }

        /// <summary>
        /// Versicherungscode externes Versicherungsprodukt
        /// </summary>
        public string extvscode { get; set; }

        /// <summary>
        /// Reference to Offer
        /// </summary>
        public long sysAngebot { get; set; }
    }
}