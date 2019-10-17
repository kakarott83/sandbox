using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Decision Engine In Data Transfer Object
    /// </summary>
    public class DecisionEngineInDto
    {
        #region RecordNR

        #region Envelope
        /// <summary>
        /// Getter/Setter Inquiry code
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string InquiryCode { get; set; }

        /// <summary>
        /// Getter/Setter Process code
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string ProcessCode { get; set; }

        /// <summary>
        /// Getter/Setter Organization code
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string OrganizationCode { get; set; }

        /// <summary>
        /// Getter/Setter Process Version
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public int? ProcessVersion { get; set; }

        /// <summary>
        /// Getter/Setter Inquiry date
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public DateTime? InquiryDate { get; set; }

        /// <summary>
        /// Getter/Setter Inquiry time
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string InquiryTime { get; set; }

        /// <summary>
        /// Getter/Setter Flag Pre-assessment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? FlagVorpruefung { get; set; }

        /// <summary>
        /// Getter/Setter Credit rating assessment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? FlagBonitaetspruefung { get; set; }

        /// <summary>
        /// Getter/Setter Risk assessment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? FlagRisikopruefung { get; set; }

        /// <summary>
        /// Getter/Setter Erfassung Datum
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public DateTime? Erfassung { get; set; }

        /// <summary>
        /// Getter/Setter Erfassung Datum
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string Ciconeversion { get; set; }

        #endregion

        #region Contract

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordNRI_C_GeschaeftsartV
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string Geschaeftsart { get; set; }

        /// <summary>
        /// Getter/Setter Flagsimulation
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Flagsimulation { get; set; }

        /// <summary>
        /// Getter/Setter ErfassungskanalV
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Erfassungskanal { get; set; }

        /// <summary>
        /// Getter/Setter credit ammount
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Finanzierungsbetrag { get; set; }

        /// <summary>
        /// Getter/Setter Rate
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Getter/Setter Downpay first rate
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Anzahlung_ErsteRate { get; set; }

        /// <summary>
        /// Getter/Setter Contract type
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Vertragsart { get; set; }

        /// <summary>
        /// Getter/Setter contract period
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Laufzeit { get; set; }

        /// <summary>
        /// Getter/Setter Interest rate
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Zinssatz { get; set; }

        /// <summary>
        /// Getter/Setter Plags Package 1
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? PPI_Flag_Paket1 { get; set; }

        /// <summary>
        /// Getter/Setter Flags Package 2
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? PPI_Flag_Paket2 { get; set; }

        /// <summary>
        /// Getter/Setter PPI BETRAG
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? PPI_Betrag { get; set; }


        /// <summary>
        /// Getter/Setter Flag Stop bei Kreditanpassunge
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Erneute_Pruefung { get; set; }


        /// <summary>
        /// Getter/Setter Finanzierungsbetrag_Bewilligt
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Finanzierungsbetragbew  { get; set; }


        /// <summary>
        /// Getter/Setter Bewilligter PPI Betrag
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? PPI_Betrag_Bewilligt { get; set; }

        /// <summary>
        /// Getter/Setter Toleranzwert Quote RISK_DEC_OBTYP
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Toleranzriskdec { get; set; }



        /// <summary>
        /// Getter/Setter terminal value
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Restwert { get; set; }

        /// <summary>
        /// Getter/Setter Security deposit
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Kaution { get; set; }

        /// <summary>
        /// Getter/Setter KKG requirement
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? KKG_Pflicht { get; set; }

        /// <summary>
        /// Getter/Setter risk flag
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Riskflag { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordNRI_C_NutzungsartV
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Nutzungsart { get; set; }

        /// <summary>
        /// Getter/Setter Budget overflow 1
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Budgetueberschuss_1 { get; set; }

        /// <summary>
        /// Getter/Setter Budget overflow 2
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Budgetueberschuss_2 { get; set; }

        /// <summary>
        /// Getter/Setter Budget overflow sum
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Budgetueberschuss_gesamt { get; set; }

        /// <summary>
        /// Getter/Setter KremoCode
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? KremoCode { get; set; }

        /// <summary>
        /// Getter/Setter AuszahlungsArt
        /// </summary>
        [AttributeFilter("Risiko")]
        public string AuszahlungsArt { get; set; }

        /// <summary>
        /// Getter/Setter BuchwertGarantie
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? BuchwertGarantie { get; set; }

        /// <summary>
        /// Getter/Setter sysVTTYP
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? sysVTTYP { get; set; }

        /// <summary>
        /// Getter/Setter Umschreibung
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Umschreibung { get; set; }

        /// <summary>
        /// Getter/Setter  Randomnumber - Gelieferte Nummer von der DE im 1. Durchlauf 
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? RandomNumber { get; set; }

        /// <summary>
        /// Getter/Setter restwertverlängerung 
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? RW_Verl { get; set; }

        /// <summary>
        /// Getter/Setter Vertragsversion_NEU
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Vertragsversion_NEU { get; set; }

        /// <summary>
        /// / Getter/Setter Restwertgarant Defect #11033
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Restwertgarant { get; set; }

        #endregion

        #region Object

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordNRI_O_ObjektartV
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Objektart { get; set; }

        /// <summary>
        /// Getter/Setter Brand
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Marke { get; set; }

        /// <summary>
        /// Getter/Setter Model
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Modell { get; set; }

        /// <summary>
        /// Getter/Setter condition
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Zustand { get; set; }

        /// <summary>
        /// Getter/Setter mileage
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? KM_Stand { get; set; }

        /// <summary>
        /// Getter/Setter Annual mileage
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? KM_prJahr { get; set; }

        /// <summary>
        /// Getter/Setter First registration 
        /// </summary>
        [AttributeFilter("Risiko")]
        public DateTime? Inverkehrssetzung { get; set; }

        /// <summary>
        /// Getter/Setter Serial number
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Stammnummer { get; set; }

        /// <summary>
        /// Getter/Setter Extas price
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Zubehoerpreis { get; set; }

        /// <summary>
        /// Getter/Setter Catalog price Eurotax
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Katalogpreis_Eurotax { get; set; }

        /// <summary>
        /// Getter/Setter Vehicle price Eurotax
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Fahrzeugpreis_Eurotax { get; set; }

        /// <summary>
        /// Getter/Setter terminal value Eurotax
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Restwert_Eurotax { get; set; }

        /// <summary>
        /// Getter/Setter Terminal value BankNow
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Restwert_Banknow { get; set; }

        /// <summary>
        /// Getter/Setter Restwertabsicherung Deckungssumme
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Restwertabsicherung { get; set; }

        /// <summary>
        /// Getter/Setter  Marktwert_Cluster
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Marktwert_Cluster { get; set; }


        /// <summary>
        /// Getter/Setter  Expected_Loss
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Expected_Loss { get; set; }

        /// <summary>
        /// Getter/Setter  Expected_Loss_Prozent
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Expected_Loss_Prozent { get; set; }

        /// <summary>
        /// Getter/Setter Expected_Loss_LGD
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Expected_Loss_LGD { get; set; }

        /// <summary>
        /// Getter/Setter Profitabilitaet_Prozent
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Profitabilitaet_Prozent { get; set; }

        /// <summary>
        /// Getter/Setter VIN-Nummer (Erfassung Fahrzeug in ePOS)
        /// </summary>
        [AttributeFilter("Risiko")]
        public string VIN_Nummer { get; set; }

        

        #endregion

        #region Sales Abloesedaten

        /// <summary>
        /// Getter/Setter Number of transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Anz_Abloesen { get; set; }

        /// <summary>
        /// Getter/Setter Number of internal transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Anz_Eigenabloesen { get; set; }

        /// <summary>
        /// Getter/Setter Number of external transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Anz_Fremdabloesen { get; set; }

        /// <summary>
        /// Getter/Setter Sum of transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Summe_Abloesen { get; set; }

        /// <summary>
        /// Getter/Setter Sum of internal transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Summe_Eigenabloesen { get; set; }

        /// <summary>
        /// Getter/Setter Sum external transfers
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal? Summe_Fremdabloesen { get; set; }

        /// <summary>
        /// Getter/Setter Transfer bank 1
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public string Name_Abloesebank_1 { get; set; }

        /// <summary>
        /// Getter/Setter Transfer bank 2
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public string Name_Abloesebank_2 { get; set; }

        /// <summary>
        /// Getter/Setter Transfer bank 3
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public string Name_Abloesebank_3 { get; set; }

        /// <summary>
        /// Getter/Setter Transfer bank 4
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public string Name_Abloesebank_4 { get; set; }

        /// <summary>
        /// Getter/Setter Transfer bank 5
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public string Name_Abloesebank_5 { get; set; }

        /// <summary>
        /// Getter/Setter Validabl
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal Validabl { get; set; }

        //BNR13

        /// <summary>
        /// 
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal Abl_Produkt { get; set; }

        /// <summary>
        /// Totale Vertragsdauer (Mt.) der vorgängig aufeinanderfolgenden, abgelösten Verträge (unabhängig von Vertragsart)
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal Abl_Dauer_Total { get; set; }

        /// <summary>
        /// Anzahl der vorgängigen, abgelösten Verträge mit gleicher Vertragsart (dabei 7,9 als ein Produkt gelten  Rahmenkredit) 
        /// basierend auf dem gewählten Produkt auf dem neuen Antrag (die Vertragsarten 7 , 9 (Dispo und Card) werden als ein Produkt betrachtet)
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal Abl_Anz_Vertragsart { get; set; }

        /// <summary>
        /// Dauer der vorgängigen, abgelösten Verträge mit gleicher Vertragsart (dabei 7,9 als ein Produkt gelten  Rahmenkredit)
        /// basierend auf dem gewählten Produkt auf dem neuen Antrag (die Vertragsarten 7 , 9 (Dispo und Card) werden als ein Produkt betrachtet)
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public decimal Abl_Dauer_Vertragsart { get; set; }

        /// <summary>
        /// Definierte Laufzeit auf dem letzten abgelösten/abzulösenden Vertrag (dabei 7 , 9 als ein Produkt gelten  Rahmenkredit), 
        /// basierend auf dem gewählten Produkt auf dem neuen Antrag (die Vertragsarten 7 , 9 (Dispo und Card) werden als ein Produkt betrachtet)
        /// </summary>
        [AttributeFilter("Bonitaet,Risiko")]
        public Decimal Abl_LZ_Vorvertrag_Vertragsart { get; set; }



        #endregion

        #region Vertriebspartner
        
        /// <summary>
        /// Getter/Setter Distribution partner ID
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? VertriebspartnerID { get; set; }

        /// <summary>
        /// Getter/Setter Distribution partner Type
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string Vertriebspartnerart { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordNRI_VP_RechtsformV
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string Rechtsform { get; set; }

        /// <summary>
        /// Getter/Setter ZIP
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string PLZ { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordNRI_VP_SpracheV
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string Sprache { get; set; }

        /// <summary>
        /// Getter/Setter Active flag
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? flagAktiv { get; set; }

        /// <summary>
        /// Getter/Setter EPOS flag
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? flagEPOS { get; set; }

        /// <summary>
        /// Getter/Setter VSB Flag
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? flagVSB { get; set; }

        /// <summary>
        /// Getter/Setter Garantor Limit
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Garantenlimite { get; set; }

        /// <summary>
        /// Getter/Setter Volume commitment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Volumenengagement { get; set; }

        /// <summary>
        /// Getter/Setter Final Volume commitment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Eventualvolumenengagement { get; set; }

        /// <summary>
        /// Getter/Setter Terminal value commitment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Restwertengagement { get; set; }

        /// <summary>
        /// Getter/Setter Final Terminal value commitment
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Eventualrestwertengagement { get; set; }

        /// <summary>
        /// Getter/Setter Number applications
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Anz_Antraege { get; set; }

        /// <summary>
        /// Getter/Setter Number of pending Applications
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Anz_pendente_Antraege { get; set; }

        /// <summary>
        /// Number Contracts
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Anz_Vertraege { get; set; }

        /// <summary>
        /// Number of active contracts
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Anz_lfd_Vertraege { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public string UIDNummer { get; set; }

        /// <summary>
        /// Strategic_Accoun
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Strategic_Account { get; set; }

        /// <summary>
        /// Badlisteintrag 
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public decimal? Badlisteintrag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Vermittlerart { get; set; }


        #endregion

        #region Fraud
        /// <summary>
        /// 
        /// </summary>
        [AttributeFilter("Risiko")]
        public string[] I_F_Values { get; set; }

        #endregion

        #endregion

        #region CR 480
        /// <summary>
        /// Getter/Setter 
        /// </summary>
        [AttributeFilter("Risiko")]
        public string Ausstattung { get; set; }

        /// <summary>
        /// Getter/Setter 
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Demolimit { get; set; }

        /// <summary>
        /// Getter/Setter 
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Demoengagement { get; set; }

        /// <summary>
        /// Getter/Setter 
        /// </summary>
        [AttributeFilter("Risiko")]
        public decimal? Eventualdemoengagement { get; set; }



        #endregion

        #region RecordRR

        /// <summary>
        /// Getter/Setter Data record RR Data Transfer Object
        /// </summary>
        [AttributeFilter("Vorpruefung,Bonitaet,Risiko")]
        public RecordRRDto[] RecordRRDto { get; set; }

        #endregion
    }
}