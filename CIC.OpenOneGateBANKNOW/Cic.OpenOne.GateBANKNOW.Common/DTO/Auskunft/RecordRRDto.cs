using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Record Data Repeatable Transfer Object
    /// </summary>
    public class RecordRRDto
    {
        #region Applicant
        
        /// <summary>
        /// Getter/Setter A Customer ID
        /// </summary>
        public decimal? A_KundenID { get; set; }

        /// <summary>
        /// Getter/Setter A MO Counter
        /// </summary>
        public decimal? A_MO_Counter { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_KundenartV
        /// </summary>
        public string A_Kundenart { get; set; }

        /// <summary>
        /// Getter/Setter A date of Birth
        /// </summary>
        public DateTime? A_Geburtsdatum { get; set; }

        /// <summary>
        /// Getter/Setter A ZIP
        /// </summary>
        public string A_PLZ { get; set; }

        /// <summary>
        /// Getter/Setter A Country
        /// </summary>
        public string A_Land { get; set; }

        /// <summary>
        /// Getter/Setter A District
        /// </summary>
        public string A_Kanton { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_SpracheV
        /// </summary>
        public string A_Sprache { get; set; }

        /// <summary>
        ///  Getter/Setter StrategyOneRequestBodyRecordI_A_StatusV
        /// </summary>
        public string A_Status { get; set; }

        /// <summary>
        /// Getter/Setter A Phone private
        /// </summary>
        public string A_Telefon_privat { get; set; }

        /// <summary>
        /// Getter/Setter A Phone business
        /// </summary>
        public string A_Telefon_geschaeftlich { get; set; }

        /// <summary>
        /// Getter/Setter A Phone Mobile
        /// </summary>
        public string A_Mobiltelefon { get; set; }

        /// <summary>
        /// Getter/Setter A E-Mail
        /// </summary>
        public string A_E_Mail { get; set; }

        /// <summary>
        /// Getter/Setter A Resident since
        /// </summary>
        public DateTime? A_hier_Wohnhaft_seit { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_AuszahlungsartV
        /// </summary>
        public string A_Auszahlungsart { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_RueckzahlungsartV
        /// </summary>
        public string A_Rueckzahlungsart { get; set; }

        /// <summary>
        /// Getter/Setter A Employee Credit Suisse Group
        /// </summary>
        public decimal? A_marbeiter_Credit_Suisse_Group { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_CS_EinheitV
        /// </summary>
        public string A_CS_Einheit { get; set; }

        /// <summary>
        /// Getter/Setter A Personal ID
        /// </summary>
        public string A_A_PID { get; set; }

        /// <summary>
        /// Getter/Setter A Instradierung
        /// </summary>
        public string A_Instradierung { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_ZivilstandV
        /// </summary>
        public string A_Zivilstand { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_WohnverhaeltnisV
        /// </summary>
        public string A_Wohnverhaeltnis { get; set; }

        /// <summary>
        /// Getter/Setter A Number children to 6
        /// </summary>
        public decimal? A_Anz_Kinder_bis_6 { get; set; }

        /// <summary>
        /// Getter/Setter A Number children 6 to 10
        /// </summary>
        public decimal? A_Anz_Kinder_ueber_6_bis_10 { get; set; }

        /// <summary>
        /// Getter/Setter A Number children 10 to 12
        /// </summary>
        public decimal? A_Anz_Kinder_ueber_10_bis_12 { get; set; }

        /// <summary>
        /// Getter/Setter A Number children greater 12
        /// </summary>
        public decimal? A_Anz_unterstuetzungsp_Kinder_ab_12 { get; set; }

        /// <summary>
        /// Getter/Setter A NAtionality
        /// </summary>
        public string A_Nationalitaet { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_AuslaenderausweisV
        /// </summary>
        public string A_Auslaenderausweis { get; set; }

        /// <summary>
        /// Getter/Setter A Foreign passport Date of Imigration
        /// </summary>
        public DateTime? A_Auslaenderausweis_Einreisedatum { get; set; }

        /// <summary>
        /// Getter/Setter A Foreign Passport Valid till
        /// </summary>
        public DateTime? A_Auslaenderausweis_Gueltigkeitsdatum { get; set; }

        ///<summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_Berufliche_SituationV
        /// </summary>
        public string A_Berufliche_Situation { get; set; }

        /// <summary>
        /// Getter/Setter A Employed since
        /// </summary>
        public DateTime? A_Arbeitgeber_seit_wann { get; set; }

        /// <summary>
        /// Getter/Setter A Employed till
        /// </summary>
        public DateTime? A_Arbeitgeber_beschaeftigt_bis { get; set; }
        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_Einkommen_ArtV
        /// </summary>
        public string A_Einkommen_Art { get; set; }

        /// <summary>
        /// Getter/Setter A Main income ammount
        /// </summary>
        public decimal? A_Haupteinkommen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter A Anual Gratification Bonus
        /// </summary>
        public decimal? A_Jaehl_Gratifikation_Bonus { get; set; }

        /// <summary>
        /// Getter/Setter A thirteenth wage
        /// </summary>
        public decimal? A_13_Montaslohn { get; set; }

        /// <summary>
        /// Getter/Setter A Tax Deducted at source
        /// </summary>
        public decimal? A_Quellensteuerpflichtig { get; set; }

        /// <summary>
        /// Getter/Setter Extra Income Ammount
        /// </summary>
        public decimal? A_Nebeneinkommen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter A Additional Income Ammount
        /// </summary>
        public decimal? A_Zusatzeinkommen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_ZusatzeinkommenV
        /// </summary>
        public string A_Zusatzeinkommen { get; set; }

        /// <summary>
        /// Getter/Setter A Number of Enfocements
        /// </summary>
        public decimal? A_Anz_der_Betreibungen { get; set; }

        /// <summary>
        /// Getter/Setter A Ammount of Enforcements
        /// </summary>
        public decimal? A_Hoehe_der_Betreibungen { get; set; }

        /// <summary>
        /// Getter/Setter A Levy's of Execution
        /// </summary>
        public decimal? A_Verlustscheine_Pfaendungen { get; set; }

        /// <summary>
        /// Getter/Setter A Cost of living/rent
        /// </summary>
        public decimal? A_Wohnkosten_Miete { get; set; }

        /// <summary>
        /// Getter/Setter A Exisiting Credit rates
        /// </summary>
        public decimal? A_Bestehende_Kreditrate { get; set; }

        /// <summary>
        /// Getter/Setter A Existing Leasing Rates
        /// </summary>
        public decimal? A_Bestehende_Leasingrate { get; set; }

        /// <summary>
        /// Getter/Setter A regular espenditures ammount
        /// </summary>
        public decimal? A_Regelmaessige_Auslagen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_Regelmaessige_AuslagenV
        /// </summary>
        public string A_Regelmaessige_Auslagen { get; set; }

        /// <summary>
        /// Getter/Setter Backing Ammount
        /// </summary>
        public decimal? A_Unterstuetzungsbeitraege_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_UnterstuetzungsbeitraegeV
        /// </summary>
        public string A_Unterstuetzungsbeitraege { get; set; }

        /// <summary>
        /// Getter/Setter Work related Expenditures Ammount
        /// </summary>
        public decimal? A_Berufsauslagen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_A_BerufsauslagenV
        /// </summary>
        public string A_Berufsauslagen { get; set; }

        /// <summary>
        /// Getter/Setter A Phone number 1
        /// </summary>
        public string A_Telefon_1 { get; set; }

        /// <summary>
        /// Getter/Setter A Phone Number 2
        /// </summary>
        public string A_Telefon_2 { get; set; }

        /// <summary>
        /// Getter/Setter A Anual turnover
        /// </summary>
        public decimal? A_Jahresumsatz { get; set; }

        /// <summary>
        /// Getter/Setter A capital resources
        /// </summary>
        public decimal? A_Eigenkapital { get; set; }

        /// <summary>
        /// Getter/Setter A Balance ammount
        /// </summary>
        public decimal? A_Bilanzsumme { get; set; }

        /// <summary>
        /// Getter/Setter A Anual gain
        /// </summary>
        public decimal? A_Jahregewinn { get; set; }

        /// <summary>
        /// Getter/Setter A available funds
        /// </summary>
        public decimal? A_fluessige_mtel { get; set; }

        /// <summary>
        /// Getter/Setter A short-term liabilitys
        /// </summary>
        public decimal? A_Kurzfristige_Verbindlichkeiten { get; set; }

        /// <summary>
        /// Getter/Setter A Registered in Comercial Register
        /// </summary>
        public decimal? A_In_Handelsregister_eingetragen { get; set; }

        /// <summary>
        /// Getter/Setter A Auditor 
        /// </summary>
        public decimal? A_Revisionsstelle_vorhanden { get; set; }

        /// <summary>
        /// Getter/Setter A Date of last annual fiscal statement
        /// </summary>
        public DateTime? A_Datum_letzter_Jahresabschluss { get; set; }

        /// <summary>
        /// Getter/Setter A Number of Employees
        /// </summary>
        public decimal? A_Anz_Mitarbeiter { get; set; }

        /// <summary>
        /// Getter/Setter EhePartnerFlag
        /// </summary>
        public decimal? A_EhePartnerFlag { get; set; }

        /// <summary>
        /// Getter/Setter Weitere Verpflichtungen Betrag
        /// </summary>
        public decimal? A_Weitere_Verpflichtungen_Betrag { get; set; }

        /// <summary>
        /// Getter/Setter Weitere Verpflichtungen
        /// </summary>
        public string A_Weitere_Verpflichtungen { get; set; }

        /// <summary>
        /// Getter/Setter UID
        /// </summary>
        public string A_UID { get; set; }

        /// <summary>
        /// Getter/Setter A_AG_NAME
        /// </summary>
        public string A_AG_NAME { get; set; }

        /// <summary>
        /// Getter/Setter A_Nebeneinkommen_seit_wann
        /// </summary>
        public DateTime? A_Nebeneinkommen_seit_wann { get; set; }

        /// <summary>
        ///  Getter/Setter Arbeitgeber_Beschaeftigt_bis2
        /// </summary>
        public DateTime? A_Arbeitgeber_Beschaeftigt_bis2 { get; set; }

        /// <summary>
        /// Getter/Setter  Berufliche_Situation2
        /// </summary>
        public string A_Berufliche_Situation2 { get; set; }

        /// <summary>
        /// Getter/Setter Geschlecht
        /// </summary>
        public decimal A_Geschlecht { get; set; }

        /// <summary>
        /// Getter/Setter DV_AG_PLZ1 
        /// </summary>
        public string A_AG_PLZ1 { get; set; }

        /// <summary>
        /// Getter/Setter DV_AG_PLZ2 
        /// </summary>
        public string A_AG_PLZ2 { get; set; }

        /// <summary>
        /// Getter/Setter A_AG_NAME 2
        /// </summary>
        public string A_AG_NAME2 { get; set; }

        /// <summary>
        ///  Getter/Setter  A_AG_seit_wann2
        /// </summary>
        public DateTime? A_Arbeitgeber_seit_wann2 { get; set; }

        /// <summary>
        /// Getter/Setter Kinderzulage
        /// </summary>
        public decimal? A_ZULAGEKIND { get; set; }

        /// <summary>
        ///  Getter/Setter ZULAGEAUSBILDUNG 
        /// </summary>
        public decimal? A_ZULAGEAUSBILDUNG { get; set; }

        /// <summary>
        ///  Getter/Setter A_ZULAGESONST
        /// </summary>
        public decimal? A_ZULAGESONST { get; set; }

        #endregion

        #region Deltavista
        /// <summary>
        /// Getter/Setter Date of Information
        /// </summary>
        public DateTime? DV_Datum_der_Auskunft { get; set; }

        /// <summary>
        /// Getter/Setter Time of Information
        /// </summary>
        public string DV_Zeit_der_Auskunft { get; set; }

        /// <summary>
        /// Getter/Setter State Information Address Validating
        /// </summary>
        public decimal? DV_Status_Auskunft_Adressvalidierung { get; set; }

        /// <summary>
        /// Getter/Setter Number Hits Address Validating
        /// </summary>
        public decimal? DV_Anz_DV_Treffer_Adressvalidierung { get; set; }

        /// <summary>
        /// Getter/Setter Customer ID
        /// </summary>
        public decimal? DV_KundenID { get; set; }

        /// <summary>
        /// Getter/Setter ADDRESS ID
        /// </summary>
        public string DV_ADDRESS_ID { get; set; }

        /// <summary>
        /// Getter/Setter Date of Birth
        /// </summary>
        public DateTime? DV_Geburtsdatum { get; set; }

        /// <summary>
        /// Getter/Setter ZIP
        /// </summary>
        public string DV_PLZ { get; set; }

        /// <summary>
        /// Getter/Setter Country
        /// </summary>
        public string DV_Land { get; set; }

        /// <summary>
        /// Getter/Setter  Date of first Notification
        /// </summary>
        public DateTime? DV_Datum_der_ersten_Meld { get; set; }

        /// <summary>
        /// Getter/Setter resident at current address since
        /// </summary>
        public DateTime? DV_Datum_an_der_aktuellen_Adresse_seit { get; set; }

        /// <summary>
        /// Getter/Setter Company state
        /// </summary>
        public decimal? DV_Firmenstatus { get; set; }

        /// <summary>
        /// Getter/Setter Date of founding
        /// </summary>
        public DateTime? DV_Gruendungsdatum { get; set; }

        /// <summary>
        /// Getter/Setter NOGA Code Brnach of trade
        /// </summary>
        public string DV_NOGA_Code_Branche { get; set; }

        /// <summary>
        /// Getter/Setter Legal form
        /// </summary>
        public decimal? DV_Rechtform { get; set; }

        /// <summary>
        /// Getter/Setter Capital
        /// </summary>
        public decimal? DV_Kapital { get; set; }

        /// <summary>
        /// Getter/Setter Fraud field
        /// </summary>
        public string DV_Fraud_Feld { get; set; }


        /// <summary>
        /// Getter/Setter Anzahl Bonitäts-Meldungen
        /// </summary>
        public decimal? DV_Anz_BPM { get; set; }


        /// <summary>
        /// Getter/Setter Number BPM l12m
        /// </summary>
        public decimal? DV_Anz_BPM_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM l24m
        /// </summary>
        public decimal? DV_Anz_BPM_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM l36m
        /// </summary>
        public decimal? DV_Anz_BPM_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM l48m
        /// </summary>
        public decimal? DV_Anz_BPM_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM l60m
        /// </summary>
        public decimal? DV_Anz_BPM_l60m { get; set; }



        /// <summary>
        /// Getter/Setter Number BPM m FStat 01
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01 { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 02
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02 { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 03
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03 { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 04
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04 { get; set; }



        /// <summary>
        /// Getter/Setter Number BPM m FStat 01 l12m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 01 l24m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 01 l36m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 01 l48m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 01 l60m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_01_l60m { get; set; }



        /// <summary>
        /// Getter/Setter Number BPM m FStat 02 l12m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 02 l24m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 02 l36m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 02 l48m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 02 l60m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_02_l60m { get; set; }



        /// <summary>
        /// Getter/Setter Number BPM m FStat 03 l12m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 03 l24m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 03 l36m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 03 l48m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 03 l60m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_03_l60m { get; set; }



        /// <summary>
        /// Getter/Setter Number BPM m FStat 04 l12m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 04 l24m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 04 l36m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 04 l48m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Number BPM m FStat 04 l60m
        /// </summary>
        public decimal? DV_Anz_BPM_m_FStat_04_l60m { get; set; }



        /// <summary>
        /// Getter/Setter Worst BPM m FStat
        /// </summary>
        public decimal? DV_Schlechtester_FStat { get; set; }

        /// <summary>
        /// Getter/Setter Worst BPM m FStat l12m
        /// </summary>
        public decimal? DV_Schlechtester_FStat_l12m { get; set; }

        /// <summary>
        /// Getter/Setter Worst BPM m FStat l24m
        /// </summary>
        public decimal? DV_Schlechtester_FStat_l24m { get; set; }

        /// <summary>
        /// Getter/Setter Worst BPM m FStat l36m
        /// </summary>
        public decimal? DV_Schlechtester_FStat_l36m { get; set; }

        /// <summary>
        /// Getter/Setter Worst BPM m FStat l48m
        /// </summary>
        public decimal? DV_Schlechtester_FStat_l48m { get; set; }

        /// <summary>
        /// Getter/Setter Worst BPM m FStat l60m
        /// </summary>
        public decimal? DV_Schlechtester_FStat_l60m { get; set; }


        /// <summary>
        /// Getter/Setter UID
        /// </summary>
        public string DV_UID { get; set; }
        /// <summary>
        /// Getter/Setter DV_Anz_DecisionMaker
        /// </summary>
        public decimal? DV_Anz_DecisionMaker { get; set; }

        /// <summary>
        /// Getter/Setter DV_Datum_juengster_Eintrag
        /// </summary>
        public DateTime? DV_Datum_juengster_Eintrag { get; set; }

        /// <summary>
        /// Getter/Setter DV_Kritischer_Glaeubiger
        /// </summary>
        public decimal? DV_Kritischer_Glaeubiger { get; set; }
    
        /// <summary>
        /// Getter/Setter DV_Summe_offener_Betreibungen
        /// </summary>
        public decimal? DV_Summe_offener_Betreibungen { get; set; }

        /// <summary>
        /// Getter/Setter DV_Anzahl_offene_Betreibungen
        /// </summary>
        public decimal? DV_Anzahl_offene_Betreibungen { get; set; }

        /// <summary>
        /// Getter/Setter DV_Datum_juengste_Betreibung
        /// </summary>
        public DateTime? DV_Datum_juengste_Betreibung { get; set; }


        /// <summary>
        /// Getter/Setter DV_Organisation_belastet
        /// </summary>
        public decimal? DV_Organisation_belastet { get; set; }


        /// <summary>
        /// Getter/Setter DV_Score
        /// </summary>
        public decimal? DV_Score { get; set; }
        /// <summary>
        /// Getter/Setter DV_Payment_Delay
        /// </summary>
        public decimal? DV_Payment_Delay { get; set; }


       

        /// <summary>
        /// Getter/Setter DV_First_SHAB_Date
        /// </summary>
        public DateTime? DV_First_SHAB_Date { get; set; }

        /// <summary>
        /// Getter/Setter DV_Risk_Alert
        /// </summary>
        public decimal? DV_Risk_Alert { get; set; }


        /// <summary>
        /// Getter/Setter DV_Uid_SameAsName
        /// </summary>
        public decimal? DV_Uid_SameAsName { get; set; }


        /// <summary>
        /// Getter/Setter AG Date 
        /// </summary>
        [AttributeFilter("Bonitaet")]
        public DateTime? DV_AG_Datum { get; set; }

        /// <summary>
        /// Getter/Setter AG Time
        /// </summary>
        public string DV_AG_Zeit { get; set; }

        /// <summary>
        /// Getter/Setter AG State
        /// </summary>
        public decimal? DV_AG_Status { get; set; }

        /// <summary>
        /// Getter/Setter DV_AG_ADDRESS_ID
        /// </summary>
        public string DV_AG_ADDRESS_ID { get; set; }

        /// <summary>
        /// Getter/Setter AG legal form
        /// </summary>
        public decimal? DV_AG_Rechtsform { get; set; }

        /// <summary>
        /// Getter/Setter Founding date
        /// </summary>
        public DateTime? DV_AG_Gruendungsdatum { get; set; }

        /// <summary>
        /// Getter/Setter Company state
        /// </summary>
        public decimal? DV_AG_Firmenstatus { get; set; }

        /// <summary>
        /// Getter/Setter AG Decision Maker
        /// </summary>
        public decimal? DV_AG_Decision_Maker { get; set; }

        /// <summary>
        /// Getter/Setter AG NOGA Code
        /// </summary>
        public string DV_AG_NOGA_Code { get; set; }

        /// <summary>
        /// Getter/Setter Arbeit UID
        /// </summary>
        public string DV_AG_UID { get; set; }

        /// <summary>
        /// Getter/Setter DV_AG_Kapital
        /// </summary>
        public decimal? DV_AG_Kapital { get; set; }

        /// <summary>
        /// Getter/Setter DV_Anz_BM_m_FStat_00
        /// </summary>
        public decimal? DV_Anz_BM_m_FStat_00 { get; set; }
        
        #endregion

        #region ZEK
        /// <summary>
        /// Getter/Setter Date of Information
        /// </summary>
        public DateTime? ZEK_Datum_der_Auskunft { get; set; }

        /// <summary>
        /// Getter/Setter State
        /// </summary>
        public decimal? ZEK_Status { get; set; }

        /// <summary>
        /// Getter/Setter total commitment
        /// </summary>
        public decimal? ZEK_Kunde_Gesamtengagement { get; set; }

        /// <summary>
        /// Getter/Setter Synonyms
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Synonyme { get; set; }

        /// <summary>
        /// Getter/Setter Contracts
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Vertraege { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment 
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment cash loan
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Bardarlehen { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment fixed credit
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Festkredit { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment leasing
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Leasing { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment downpay
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Teilz { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment downpay
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Kontokorrent { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment cards
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_Kartenengagement { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_0506 
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_0506 { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_04
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_04 { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_04l12M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_04l24M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_04l36M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment BCode_03
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_03 { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment 03l12M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment 03l24M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M { get; set; }

        /// <summary>
        /// Getter/Setter sequential number commitment 03l36M
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M { get; set; }

        /// <summary>
        /// Getter/Setter worst ZEK Code
        /// </summary> 
        public decimal? ZEK_schlechtester_ZEK_Code { get; set; }

        /// <summary>
        /// Getter/Setter Number ZEK Inquiries
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Gesuche { get; set; }

        /// <summary>
        /// Getter/Setter Number of sequential number external Inquiries
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_FremdGes { get; set; }

        /// <summary>
        /// Getter/Setter Number of sequential number internal Inquiries
        /// </summary> 
        public decimal? ZEK_Anz_lfd_ZEK_EigenGes { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_04_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_07_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_09_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_10_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_13_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_14_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_99_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_04_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_07_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_09_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_10_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_13_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_14_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_99_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_05060812
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_05060812 { get; set; }

        /// <summary>
        /// Getter/Setter Number of all AblCode_040709
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_Ges_m_AblCode_040709 { get; set; }

        /// <summary>
        /// Getter/Setter Worst AblCode
        /// </summary> 
        public decimal? ZEK_schlechtester_ZEK_AblCode { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_21_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_21_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_21_l36M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_21_l48M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_21_l60M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_22_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_22_l24M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_22_l36M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_22_l48M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_22_l60M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_KMeld_m_ErCode_23_24_25_26
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_AmtsMelden_01_05
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_AmtsMelden_01_05 { get; set; }

        /// <summary>
        /// Getter/Setter Number of ZEK_AmtsMelden_01_05_l12M
        /// </summary> 
        public decimal? ZEK_Anz_ZEK_AmtsMelden_01_05_l12M { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Eng_m_BCode_04_laufend
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_04_laufend { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Eng_m_BCode_04_saldier
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_04_saldiert { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Eng_m_BCode_03_laufend
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_03_laufend { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Eng_m_BCode_03_saldiert
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_03_saldiert { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Ges_m_AblCode_13_14_BN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_13_14_BN { get; set; }

        /// <summary>
        /// etter/Setter ZEK_Anz_Ges_m_AblCode_13_14_noBN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_13_14_noBN { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Ges_m_AblCode_10_BN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_10_BN { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_Ges_m_AblCode_10_noBN
        /// </summary>
        public decimal? ZEK_Anz_Ges_m_AblCode_10_noBN { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv
        /// </summary>
        public decimal? ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv
        /// </summary>
        public decimal? ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv
        /// </summary>
        public decimal? ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv { get; set; }

        /// <summary>
        /// Getter/Setter ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv
        /// </summary>
        public decimal? ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv { get; set; }

        /// <summary>
        /// Getter/Setter Anzahl Negativmeldungen Vertragsstatus 3,4 und Bonitätscode 3,4,5,6
        /// </summary>
        public decimal? ZEK_Negativeintraege { get; set; }

        /// <summary>
        /// Getter/Setter Anzahl Positivmeldungen Vertragsstatus 4 und Bonitätscode1,2
        /// </summary>
        public decimal? ZEK_Positiveintraege { get; set; }


        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_61
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_61 { get; set; }

        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_61_l12M 
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_61_l12M { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_61_l24M { get; set; }

        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_61_l36M
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_61_l36M { get; set; }

        /// <summary>
        /// ZEK_Anz_Eng_m_BCode_61_laufend
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_61_laufend { get; set; }

        /// <summary>
        /// ZEK_Anz_Eng_m_BCode_61_saldiert
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_61_saldiert { get; set; }

        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_71
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_71 { get; set; }

        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_71_l12M
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_71_l12M { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_71_l24M { get; set; }

        /// <summary>
        /// ZEK_Anz_ZEK_Eng_m_BCode_71_l36M 
        /// </summary>
        public decimal? ZEK_Anz_ZEK_Eng_m_BCode_71_l36M { get; set; }

        /// <summary>
        /// ZEK_Anz_Eng_m_BCode_71_laufend
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_71_laufend { get; set; }

        /// <summary>
        /// ZEK_Anz_Eng_m_BCode_71_saldiert
        /// </summary>
        public decimal? ZEK_Anz_Eng_m_BCode_71_saldiert { get; set; }



        #endregion

        #region OpenLease
        /// <summary>
        /// Getter/Setter Date of OpenLease registration
        /// </summary>
        public DateTime? OL_OpenLease_Datum_der_Anmeldung { get; set; }

        /// <summary>
        /// Getter/Setter State
        /// </summary>
        public decimal? OL_Status { get; set; }

        /// <summary>
        /// Getter/Setter Number Customer ID's
        /// </summary>
        public decimal? OL_Anz_KundenIDs { get; set; }

        /// <summary>
        /// Getter/Setter Minimal Customer Date
        /// </summary>
        public DateTime? OL_Minimales_Datum_Kunde { get; set; }

        /// <summary>
        /// Getter/Setter Maximal Volume entry
        /// </summary>
        public decimal? OL_Maximaler_Bandlisteintrag { get; set; }

        /// <summary>
        /// Getter/Setter Maximum current Riskclass of Customer
        /// </summary>
        public decimal? OL_Maximale_akt_RKlasse_des_Kunden { get; set; }

        /// <summary>
        /// Getter/Setter Maximal Riskclass of customer
        /// </summary>
        public decimal? OL_Maximale_Risikoklasse_des_Kunden { get; set; }

        /// <summary>
        /// Number of  applications
        /// </summary>
        public decimal? OL_Anz_Antraege { get; set; }

        /// <summary>
        /// Number of Multi-applications
        /// </summary>
        public decimal? OL_Anz_Mehrfachantraege { get; set; }

        /// <summary>
        /// Getter/Setter Number of cancellations
        /// </summary>
        public decimal? OL_Anz_Annulierungen_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Number of resignations
        /// </summary>
        public decimal? OL_Anz_Verzichte_l12M { get; set; }

        /// <summary>
        /// Getter/Setter Possible commitment
        /// </summary>
        public decimal? OL_Eventualengagement { get; set; }

        /// <summary>
        /// Getter/Setter Entire Commitment
        /// </summary>
        public decimal? OL_Gesamtengagement { get; set; }

        /// <summary>
        /// Getter/Setter Household commitment
        /// </summary>
        public decimal? OL_Haushaltsengagement { get; set; }

        /// <summary>
        /// Getter/Setter Last rent
        /// </summary>
        public decimal? OL_Letzte_Miete { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_OL_Letzter_ZivilstandV
        /// </summary>
        public string OL_Letzter_Zivilstand { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_OL_Letztes_WohnverhaeltnisV
        /// </summary>
        public string OL_Letztes_Wohnverhaeltnis { get; set; }

        /// <summary>
        /// Getter/Setter Last nationality
        /// </summary>
        public string OL_Letzte_Nationalitaet { get; set; }

        /// <summary>
        /// Getter/Setter StrategyOneRequestBodyRecordI_OL_Letztes_ArbeitsverhaeltnisV
        /// </summary>
        public string OL_Letztes_Arbeitsverhaeltnis { get; set; }

        /// <summary>
        /// Getter/Setter Last main income
        /// </summary>
        public decimal? OL_Letztes_Haupteinkommen { get; set; }

        /// <summary>
        /// Getter/Setter Last extra income
        /// </summary>
        public decimal? OL_Letztes_Nebeneinkommen { get; set; }

        /// <summary>
        /// Getter/Setter Last additional income
        /// </summary>
        public decimal? OL_Letztes_Zusatzeinkommen { get; set; }

        /// <summary>
        /// Getter/Setter Last Bonus
        /// </summary>
        public decimal? OL_Letzter_Bonus { get; set; }

        /// <summary>
        /// Getter/Setter Commitment
        /// </summary>
        public decimal? OL_Engagement { get; set; }

        /// <summary>
        /// Getter/Setter Number contracts
        /// </summary>
        public decimal? OL_Anz_Vertraege { get; set; }

        /// <summary>
        /// Getter/Setter Number active Contracts
        /// </summary>
        public decimal? OL_Anz_lfd_Vertraege { get; set; }

        /// <summary>
        /// Getter/Setter Number contracts in recovery
        /// </summary>
        public decimal? OL_Anz_Vertraege_im_Recovery { get; set; }

        /// <summary>
        /// Getter/Setter Number Warnings 1
        /// </summary>
        public decimal? OL_Anz_Mahnungen_1 { get; set; }

        /// <summary>
        /// Getter/Setter Number Warnings 2
        /// </summary>
        public decimal? OL_Anz_Mahnungen_2 { get; set; }

        /// <summary>
        /// Getter/Setter Number Warnings 3
        /// </summary>
        public decimal? OL_Anz_Mahnungen_3 { get; set; }

        /// <summary>
        /// Getter/Setter Maximum Warning level
        /// </summary>
        public decimal? OL_Maximale_Mahnstufe { get; set; }

        /// <summary>
        /// Getter/Setter Number moratoriums
        /// </summary>
        public decimal? OL_Anz_Stundungen { get; set; }

        /// <summary>
        /// Getter/Setter Number Payment agreements
        /// </summary>
        public decimal? OL_Anz_Zahlungsvereinbarungen { get; set; }

        /// <summary>
        /// Getter/Setter ammount of OP's
        /// </summary>
        public decimal? OL_Summe_OP { get; set; }

        /// <summary>
        /// Getter/Setter Number of OP's
        /// </summary>
        public decimal? OL_Anz_OP { get; set; }

        /// <summary>
        /// Getter/Setter effective customer relationship
        /// </summary>
        public decimal? OL_Effektive_Kundenbeziehung { get; set; }

        /// <summary>
        /// Getter/Setter Duration Customer relationship
        /// </summary>
        public decimal? OL_Dauer_Kundenbeziehung { get; set; }

        /// <summary>
        /// Getter/Setter OL_Anz_manAblehnungen_l6M
        /// </summary>
        public decimal? OL_Anz_manAblehnungen_l6M { get; set; }

        /// <summary>
        /// Getter/Setter OL_Anz_manAblehnungen_l12M
        /// </summary>
        public decimal? OL_Anz_manAblehnungen_l12M { get; set; }

        /// <summary>
        /// Getter/Setter OL_Anz_Vertraege_mit_Spezialfall 
        /// </summary>
        public decimal? OL_Anz_Vertraege_mit_Spezialfall { get; set; }

        /// <summary>
        /// Getter/Setter  OL_Anz_lfd_Vertraege_mit_Spezialfall
        /// </summary>
        public decimal? OL_Anz_lfd_Vertraege_mit_Spezialfall { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_Mahnung_1
        /// </summary>
        public DateTime? OL_Datum_Mahnung_1 { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_Mahnung_2
        /// </summary>
        public DateTime? OL_Datum_Mahnung_2 { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_Mahnung_3
        /// </summary>
        public DateTime? OL_Datum_Mahnung_3 { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_letzte_Stundung
        /// </summary>
        public DateTime? OL_Datum_letzte_Stundung { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_letzte_ZVB 
        /// </summary>
        public DateTime? OL_Datum_letzte_ZVB { get; set; }

        /// <summary>
        /// Getter/Setter OL_Datum_Aufstockungssperre
        /// </summary>
        public DateTime? OL_Datum_Aufstockungssperre { get; set; }

        /// <summary>
        /// Getter/Setter OL_Anzahl_Aufstockungssperren
        /// </summary>
        public Decimal? OL_Anzahl_Aufstockungssperren { get; set; }

        /// <summary>
        /// Getter/Setter  Letzte_Einkommensart 
        /// </summary>
        public string OL_Letzte_Einkommensart { get; set; }

        /// <summary>
        /// Getter/Setter Gbezeichnung BNR10
        /// </summary>
        public Decimal? OL_Gbezeichnung { get; set; }

        /// <summary>
        ///  Getter/Setter Ratenpause BNR10
        /// </summary>
        public Decimal? OL_Ratenpausen { get; set; }


        //BNR13
        /// <summary>
        ///  Getter/Setter Datum erster Antrag des Kunden (inkl. gefundener Dublette)  min(antrag.erfassung)
        /// </summary>
        public DateTime? OL_Datum_erster_Antrag{ get; set; }

        /// <summary>
        /// Datum letzter Antrag des Kunden (inkl. gefundener Dublette)  max(antrag.erfassung)
        /// </summary>
        public DateTime? OL_Datum_letzter_Antrag { get; set; }


        /// <summary>
        /// Durchschnittliche Anzahl erster Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten  (rnmahn.mahnstufe = 1)
        /// </summary>
        public Decimal? OL_Anzahl_Mahnstufe1_L6M { get; set; }

        /// <summary>
        ///  Durchschnittliche Anzahl zweiter Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten  (rnmahn.mahnstufe = 2)
        /// </summary>
        public Decimal? OL_Anzahl_Mahnstufe2_L6M { get; set; }


        /// <summary>
        /// Durchschnittliche Anzahl dritter Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten  (rnmahn.mahnstufe = 2)
        /// </summary>
        public Decimal? OL_Anzahl_Mahnstufe3_L6M { get; set; }

        /// <summary>
        /// Durchschnittliche Anzahl Einzahlung pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten  count(fi.gebuchtdatum)
        /// </summary>
        public Decimal? OL_Anzahl_Einzahlungen_L12M { get; set; }

        /// <summary>
        /// Durchschnittlicher offene Posten (Rückstand) über alle Verträge, zum aktuellen Zeitpunkt
        /// </summary>
        public Decimal? OL_Zahlungsrueckstand { get; set; }

        /// <summary>
        /// Durchschnittliche Reduktion Buchsaldo pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten
        /// </summary>
        public Decimal? OL_Saldoreduktion_L12M { get; set; }


        #endregion

        #region CR 480
        /// <summary>
        /// Getter/Setter 
        /// </summary>        
        public decimal? Netzwerkbeziehung { get; set; }

        /// <summary>
        /// Getter/Setter 
        /// </summary>        
        public decimal? Fluktuationsrate { get; set; }

        /// <summary>
        /// Getter/Setter 
        /// </summary>        
        public decimal? Fraudmngt { get; set; }
        #endregion
        #region DEBUGET
        /// <summary>
        /// Getter/Setter BU_AnfrageDatum
        /// </summary>
        public DateTime BU_AnfrageDatum { get; set; }

        /// <summary>
        /// Getter/Setter Antragsteller
        /// </summary>
        public Decimal? BU_Antragsteller { get; set; }

        /// <summary>
        /// Getter/Setter Kremocode
        /// </summary>
        public Decimal? BU_Kremocode { get; set; }

        /// <summary>
        /// Grundbetrag 
        /// </summary>
        public Decimal? BU_Grundbetrag { get; set; }

        /// <summary>
        /// Getter/Setter Krankenkasse
        /// </summary>
        public Decimal? BU_Krankenkasse { get; set; }

        /// <summary>
        /// Getter/Setter Quellsteuer
        /// </summary>
        public Decimal? BU_Quellsteuer { get; set; }

        /// <summary>
        /// Getter/Setter Sozialauslagen
        /// </summary>
        public Decimal? BU_Sozialauslagen { get; set; }

        /// <summary>
        /// Getter/Setter Budgetueberschuss
        /// </summary>
        public Decimal? BU_Budgetueberschuss { get; set; }

        /// <summary>
        /// Getter/Setter Budgetueberschuss_gesamt
        /// </summary>
        public Decimal? BU_Budgetueberschuss_gesamt { get; set; }

        /// <summary>
        /// Berechnetes Nettoeinkommen
        /// </summary>
        public Decimal? BU_Einknettoberech { get; set; }

        /// <summary>
        /// Berechnetes Nettoeinkommen2
        /// </summary>
        public Decimal? BU_Einknettoberech2 { get; set; }

        /// <summary>
        /// Berechnetes Extbetrkostentat 
        /// </summary>
        public Decimal? BU_Betreuungskosten_Extern { get; set; } 

         /// <summary>
        /// Berechnetes Arbeitswegausl
        /// </summary>
        public Decimal? BU_Arbeitswegpauschale { get; set; }

        /// <summary>
        /// Berechnetes Krankenkasse
        /// </summary>
        public Decimal? BU_Krankenkassenpraemie { get; set; } 




        #endregion
    }
}