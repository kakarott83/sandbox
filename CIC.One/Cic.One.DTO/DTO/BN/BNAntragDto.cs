using Cic.One.DTO.BN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Parameterklasse für Angebot
    /// </summary>
    public class BNAntragDto : Cic.One.DTO.AngAntDto
    {
        public override long getEntityId()
        {
            return sysid;
        }
        public override string getEntityBezeichnung()
        {
            return antrag;
        }

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
        public BNKundeDto kunde { get; set; }

        /// <summary>
        /// Mitantragsteller
        /// </summary>
        public BNKundeDto mitantragsteller { get; set; }

        /// <summary>
        /// AntragstellerTyp Partner(800)/Solidarschuldner(120)/Bürge(130)
        /// Verwendet für den 2. AS
        /// </summary>
        public int mitantragstellerTyp { get; set; }

        /// <summary>
        /// Kalkulationsdaten
        /// </summary>
        public BNKalkulationDto kalkulation;

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

        //CIC ONE WEB Erweiterungen zu BANKNOW-Objekt

        /// <summary>
        /// Vertrag-ID
        /// </summary>
        public long sysvt { get; set; }

        /// <summary>
        /// Mitantragsteller
        /// </summary>
        public long sysMa { get; set; }

        /// <summary>
        /// Produktinformationen aus der Kalkulation
        /// </summary>
        public ProduktInfoDto produkt { get; set; }

        /// <summary>
        /// Auflagenliste
        /// </summary>
        public List<RatingAuflageDto> auflagen { get; set; }

        /// <summary>
        /// Regelliste
        /// </summary>
        public List<AuskunftRegelDto> regeln { get; set; }


        /// <summary>
        /// Eingangskanal
        /// </summary>
        public String eingangskanal { get; set; }

        /// <summary>
        /// Kampagnencode
        /// </summary>
        public String kampagnencode { get; set; }

        /// <summary>
        /// Kampagnencode
        /// </summary>
        public String finanzierungsart { get; set; }

        /// <summary>
        /// Mitantragsteller sysit
        /// </summary>
        public long sysItMa { get; set; }
    }
}
