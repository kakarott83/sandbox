using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Fibu Interface DTO
    /// </summary>
    public class iFibuAccountDTO
    {
        /// <summary>
        /// SysCreate - System.DateTime.Now
        /// </summary>
        public string SysCreate { get; set; }

        /// <summary>
        /// Belegart -> TR 23.09.2013 http://jira.cic-software.de:8080/browse/BNRSIEBEN-253
        /// 1 A4
        /// </summary>
        public string Belegart { get; set; }

        /// <summary>
        /// Rechnungsnummer-> RG_RGnummer(RG_Rechnungsnummer)
        /// 2 B4 -> In Konzept "Rechnungsnummer"
        /// </summary>
        public string Rechnung { get; set; }

        /// <summary>
        /// // 0 = keine Gutschrift 1 = Gutschrift -> RG_Gutschrift
        /// 3 C4 
        /// </summary>
        public string Gutschrift { get; set; }

        /// <summary>
        /// Belegtext
        /// 4 D4 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Belegdatum des Einzel-beleges -> RG_Belegdatum
        /// 5 E5
        /// </summary>
        public string BelegDatum { get; set; }

        /// <summary>
        /// Importzeitpunkt des Massenbeleges -> RG_Valutadatum
        /// 6 F4
        /// </summary>
        public string ValutaDatum { get; set; }
        /// <summary>
        /// Bruttobetrag -> FI_BETRAG
        /// 7 G4
        /// </summary>
        public string Bruttobetrag { get; set; }

        /// <summary>
        /// Definiert den Lastschrifteinzug -> RG_LSVDTA
        /// 8 H4 In Konzept "LSVDTA"
        /// </summary>
        public string Einzug { get; set; }

        /// <summary>
        /// Definiert das Wieder-einzugsdatum (ab-Datum) -> LSV/DTA Datum
        /// 9 I4 In Konzept "LSVDTADatum"
        /// </summary>
        public string EinzugDatum { get; set; }

        /// <summary>
        /// ESR-Nummer -> RG_ESR
        /// 10 J4
        /// </summary>
        public string ESR { get; set; }

        /// <summary>
        /// Vertragsnummer -> VT_Vertragsnummer
        /// 11 K4
        /// </summary>
        public string Vertrag { get; set; }

        /// <summary>
        /// Objektbezeichnung -> VT_Objektnummer
        /// 12 L4
        /// </summary>
        public string Objekt { get; set; }
        /// <summary>
        /// Massbezeichnung -> VT_MASS
        /// 13 M4
        /// </summary>
        public string Mass { get; set; }

        /// <summary>
        /// Zahlungsvereinbarung -> TR 23.09.2013
        /// 14 N4 In Konzept "Zahlungsvereinbarung"
        /// </summary>
        public string ZVBSkonto { get; set; }

        /// <summary>
        /// SollHaben -> TR 23.09.2013 http://jira.cic-software.de:8080/browse/BNRSIEBEN-253
        /// 15 O4
        /// </summary>
        public string SollHaben { get; set; }

        /// <summary>
        /// Zinsrelevant -> TR 23.09.2013 http://jira.cic-software.de:8080/browse/BNRSIEBEN-253
        /// 16 P4
        /// </summary>
        public string Zinsrelevant { get; set; }

        /// <summary>
        /// Definiert die Buchungs-art -> FI_Buchungsart
        /// 17 Q4
        /// </summary>
        public string Buchungsart { get; set; }

        /// <summary>
        /// Konto  -> TR 23.09.2013 http://jira.cic-software.de:8080/browse/BNRSIEBEN-253
        /// Enthält ein Nebenkonto oder ein Sachkonto aus der FI-Logik
        /// 18 R4
        /// </summary>
        public string Konto { get; set; }

        /// <summary>
        /// Gegenkonto -> TR 23.09.2013 http://jira.cic-software.de:8080/browse/BNRSIEBEN-253
        /// -> Enthält das Gegenkonto aus der FI-Logik
        /// 19 S4
        /// </summary>
        public string Gegenkonto { get; set; }

        /// <summary>
        /// Steuerschlüssel -> FI_Steuerschlüssel -> In Konzept "MWST-Code"
        /// 20 T4
        /// </summary>
        public string SteuerCode { get; set; }

        /// <summary>
        /// FI_Steuer FI_Steuerbetrag -> In Konzept "Steuerbetrag"
        /// 21 U4
        /// </summary>
        public string Steuer { get; set; }

        /// <summary>
        /// Betroffene Kostenstelle -> FI_Kostenstelle -> In Konzept "KST"
        /// 22 V4
        /// </summary>
        public string Kostenstelle { get; set; }

        /// <summary>
        /// Betroffene Projektkostenstelle -> FI_Projektkostenstelle -> In Konzept "P-KST"
        /// 23 W4
        /// </summary>
        public string Projektkostenstelle { get; set; }

        /// <summary>
        /// Übersteuert die Periode (Jahr) des Valutadatums für die Journalisierung -> FI_Buchungsjahr
        /// 24 X4
        /// </summary>
        public string Buchungsjahr { get; set; }

        /// <summary>
        /// Übersteuert die Periode (Monat) des Valutada-tums für die Journalisierung -> FI_Buchungsmonat
        /// 25 Y4
        /// </summary>
        public string Buchungsmonat { get; set; }

        /// <summary>
        /// Affiliate kKonto
        /// 26 Z4
        /// </summary>
        public string AffiliateKonto { get; set; }

        /// <summary>
        /// Affiliate kKonto
        /// 27 AA4
        /// </summary>
        public string AffiliateGegenkonto { get; set; }

        /// <summary>
        /// Steuerschlüssel -> FI_Steuerschlüssel
        /// ! TR 23.09.2013 not used by BankNow
        /// </summary>
        public string SteuerKz { get; set; }

        /// <summary>
        /// Rechnungskreis 
        /// TR 23.09.2013 not used by BankNow
        /// </summary>
        public string Kreis { get; set; }

        /// <summary>
        /// Belegnummer (via Nummernkreis erzeugt) – wird übernommen in FI.BELEG/RN.BELEG
        /// TR 23.09.2013 not used by BankNow
        /// </summary>
        public string Beleg { get; set; }

        /// <summary>
        /// Mit dieser BuchungsID wurde die Belegposition verbucht 
        /// TR 23.09.2013 not used by BankNow
        /// </summary>
        public string SysFI { get; set; }

        /// <summary>
        /// Gebucht am
        /// TR 23.09.2013 not used by BankNow
        /// </summary>
        public string FICreateDate { get; set; }

        /// <summary>
        /// Gebucht um
        /// TR 23.09.2013 not used by BankNow
        /// </summary>
        public string FICreateTime { get; set; }

        /// <summary>
        /// Rechnungstyp. Muss auf den RNTYP:Bezeichnung matchen -> RG_Rechnungstyp
        /// TR 23.09.2013 from R7 not used by BankNow
        /// </summary>
        public string Rechnungstyp { get; set; }

        /// <summary>
        /// Nebenkonto_Nr
        /// TR 23.09.2013 from R7 not used by BankNow
        /// </summary>
        public string HabenKonto { get; set; }

        /// <summary>
        /// -> FI_Gegenkonto
        /// TR 23.09.2013 from R7 not used by BankNow
        /// </summary>
        public string SollKonto { get; set; }

    }
}