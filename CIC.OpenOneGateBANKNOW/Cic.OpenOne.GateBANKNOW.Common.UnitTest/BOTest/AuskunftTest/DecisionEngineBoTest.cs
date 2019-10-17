using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
{
    /// <summary>
    /// Testklasse für die DecisionEngine
    /// </summary>
    [TestFixture()]
    public class DecisionEngineBoTest
    {
        /// <summary>
        /// decisionEngineWSDaoMock
        /// </summary>
        public DynamicMock decisionEngineWSDaoMock;
        /// <summary>
        /// decisionEngineDBDaoMock
        /// </summary>
        public DynamicMock decisionEngineDBDaoMock;
        /// <summary>
        /// auskunftDaoMock
        /// </summary>
        public DynamicMock auskunftDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public DecisionEngineBo decisionEngineBo;

        /// <summary>
        /// Initialisierung aller generellen Parameter und Objekte für die Tests
        /// </summary>
        [SetUp]
        public void EurotaxBoTestInit()
        {
            emptyMessage = new Message();
            decisionEngineWSDaoMock = new DynamicMock(typeof(IDecisionEngineWSDao));
            decisionEngineDBDaoMock = new DynamicMock(typeof(IDecisionEngineDBDao));
            auskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            decisionEngineBo = new DecisionEngineBo((IDecisionEngineWSDao)decisionEngineWSDaoMock.MockInstance, (IDecisionEngineDBDao)decisionEngineDBDaoMock.MockInstance, (IAuskunftDao)auskunftDaoMock.MockInstance);
        }

        /// <summary>
        /// Blackboxtest für die execute Methode die mit einem DecisionEngineInDto aufgerufen wird
        /// </summary>
        [Test]
        public void executeWithDecisionEngineInDto()
        {
            DecisionEngineInDto decisionEngineInDto = new DecisionEngineInDto()
            {
                OrganizationCode = "BNOW",
                ProcessCode = "RISK",
                ProcessVersion = 1,
                InquiryCode = "14496592", //Wichtig!!!
                InquiryDate = System.DateTime.ParseExact("20070508", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                InquiryTime = "10:20",
                FlagVorpruefung = 1,
                FlagBonitaetspruefung = 0,
                FlagRisikopruefung = 0,
                Geschaeftsart = StrategyOneRequestBodyRecordNRI_C_GeschaeftsartV.FF_V.ToString(),
                Riskflag = 0,
                Finanzierungsbetrag = 117400,
                Anzahlung_ErsteRate = 17600,
                Vertragsart = 1,
                Laufzeit = 49,
                Zinssatz = 4.9m,
                PPI_Flag_Paket1 = 0,
                PPI_Flag_Paket2 = 0,
                Rate = 1625.41m,
                Restwert = 35508,
                KKG_Pflicht = 0,
                Nutzungsart = StrategyOneRequestBodyRecordNRI_C_NutzungsartV.PRIVAT.ToString(),
                Marke = "PORSCHE",
                Zustand = 1,
                Zubehoerpreis = 0,
                KM_Stand = 16000,
                KM_prJahr = 15000,
                Inverkehrssetzung = System.DateTime.ParseExact("20050323", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                Stammnummer = "137.512.440",
                Katalogpreis_Eurotax = 159785,
                Fahrzeugpreis_Eurotax = 159785,
                Restwert_Eurotax = 22990,
                Restwert_Banknow = 22990,
                Anz_Abloesen = 0,
                Anz_Eigenabloesen = 0,
                Anz_Fremdabloesen = 0,
                Summe_Abloesen = 0,
                Summe_Eigenabloesen = 0,
            //_diorig.Name_Abloesebank_1 = "BANK-now AG";
                VertriebspartnerID = 83426,
                Sprache = StrategyOneRequestBodyRecordNRI_VP_SpracheV.frCH.ToString(),
                flagAktiv = 1,
                flagEPOS = 0,
                PLZ = "1180",
                RecordRRDto = new RecordRRDto[]
                {
                    new RecordRRDto()
                    {
                        A_13_Montaslohn = 10000,
                        A_A_PID = "A_APID",
                        A_Anz_der_Betreibungen = 1,
                        A_Anz_Kinder_bis_6 = 0,
                        A_Anz_Kinder_ueber_10_bis_12 = 0,
                        A_Anz_Kinder_ueber_6_bis_10 = 0,
                        A_Anz_Mitarbeiter = 100,
                        A_Anz_unterstuetzungsp_Kinder_ab_12 = 0,
                        A_Arbeitgeber_beschaeftigt_bis = new DateTime(2020,01,01),
                        A_Arbeitgeber_seit_wann = new DateTime(2000,01,01),
                        A_Auslaenderausweis = null,
                        A_Auslaenderausweis_Einreisedatum = null,
                        A_Auslaenderausweis_Gueltigkeitsdatum = null,
                        A_Auszahlungsart = "BAR",
                        A_Berufliche_Situation = "ANGEST_UNBEFR",
                        A_Berufsauslagen = null,
                        A_Berufsauslagen_Betrag = null,
                        A_Bestehende_Kreditrate = 1000,
                        A_Bestehende_Leasingrate = 300,
                        A_Bilanzsumme = 1300,
                        A_CS_Einheit = "CS",
                        A_Datum_letzter_Jahresabschluss = new DateTime(2010,01,11),
                        A_E_Mail = "EineE@mail.com",
                        A_Eigenkapital = 1000,
                        A_Einkommen_Art = "BRUTTO",
                        A_fluessige_mtel = 2,
                        A_Geburtsdatum = new DateTime(1980,12,12),
                        A_Haupteinkommen_Betrag = 10000,
                        A_hier_Wohnhaft_seit = new DateTime(1980,12,12),
                        A_Hoehe_der_Betreibungen = 1000,
                        A_In_Handelsregister_eingetragen = null,
                        A_Instradierung = "Instradierung",
                        A_Jaehl_Gratifikation_Bonus = 20,
                        A_Jahregewinn = 10,
                        A_Jahresumsatz = 100000000,
                        A_Kanton = "Be",
                        A_Kundenart = "PRIVAT",
                        A_KundenID = 1337,
                        A_Kurzfristige_Verbindlichkeiten = 0,
                        A_Land = "Schweiz",
                        A_marbeiter_Credit_Suisse_Group = 0,
                        A_MO_Counter = 0,
                        A_Mobiltelefon = "017012334566",
                        A_Nationalitaet = "Schweizer",
                        A_Nebeneinkommen_Betrag = 1000000,
                        A_PLZ = "1234",
                        A_Quellensteuerpflichtig = 0,
                        A_Regelmaessige_Auslagen = "ZUSATZVER",
                        A_Regelmaessige_Auslagen_Betrag = 0,
                        A_Revisionsstelle_vorhanden = 0,
                        A_Rueckzahlungsart = "ESR",
                        A_Sprache = "deCH",
                        A_Status = "PARTNER",
                        A_Telefon_1 = "1234",
                        A_Telefon_2 = null,
                        A_Telefon_geschaeftlich = null,
                        A_Telefon_privat = null,
                        A_Unterstuetzungsbeitraege = "ANDERES",
                        A_Unterstuetzungsbeitraege_Betrag = 0,
                        A_Verlustscheine_Pfaendungen = 10,
                        A_Wohnkosten_Miete = 1000,
                        A_Wohnverhaeltnis = "ALLEIN",
                        A_Zivilstand = "LED",
                        A_Zusatzeinkommen = "MIETEINNAHMEN",
                        A_Zusatzeinkommen_Betrag = 100000,
                        DV_AG_Datum = null,
                        DV_AG_Decision_Maker = null,
                        DV_AG_Firmenstatus = null,
                        DV_AG_Gruendungsdatum = null,
                        DV_AG_NOGA_Code = null,
                        DV_AG_Rechtsform = null,
                        DV_AG_Status = null,
                        DV_AG_Zeit = null,
                        DV_Anz_BPM_l12m = null,
                        DV_Anz_BPM_l24m = null,
                        DV_Anz_BPM_l36m = null,
                        DV_Anz_BPM_m_FStat_01_l12m = null,
                        DV_Anz_BPM_m_FStat_01_l24m = null,
                        DV_Anz_BPM_m_FStat_01_l36m = null,
                        DV_Anz_BPM_m_FStat_02_l12m = null,
                        DV_Anz_BPM_m_FStat_02_l24m = null,
                        DV_Anz_BPM_m_FStat_02_l36m = null,
                        DV_Anz_BPM_m_FStat_03_l12m = null,
                        DV_Anz_BPM_m_FStat_03_l24m = null,
                        DV_Anz_BPM_m_FStat_03_l36m = null,
                        DV_Anz_BPM_m_FStat_04_l12m = null,
                        DV_Anz_BPM_m_FStat_04_l24m = null,
                        DV_Anz_BPM_m_FStat_04_l36m = null,
                        DV_Anz_DV_Treffer_Adressvalidierung = null,
                        DV_Datum_an_der_aktuellen_Adresse_seit = null,
                        DV_Datum_der_Auskunft = null,
                        DV_Datum_der_ersten_Meld = null,
                        DV_Firmenstatus = null,
                        DV_Fraud_Feld = null,
                        DV_Geburtsdatum = null,
                        DV_Gruendungsdatum = null,
                        DV_Kapital = null,
                        DV_KundenID = null,
                        DV_Land = null,
                        DV_NOGA_Code_Branche = null,
                        DV_PLZ = null,
                        DV_Rechtform = null,
                        DV_Schlechtester_FStat_l12m = null,
                        DV_Schlechtester_FStat_l24m = null,
                        DV_Schlechtester_FStat_l36m = null,
                        DV_Status_Auskunft_Adressvalidierung = null,
                        DV_Zeit_der_Auskunft = null,
                        OL_Dauer_Kundenbeziehung = null,
                        OL_Maximale_akt_RKlasse_des_Kunden = null,
                        OL_Anz_Annulierungen_l12M = null,
                        OL_Anz_Antraege = null,
                        OL_Anz_KundenIDs = null,
                        OL_Anz_lfd_Vertraege = null,
                        OL_Anz_Mahnungen_1 = null,
                        OL_Anz_Mahnungen_2 = null,
                        OL_Anz_Mahnungen_3 = null,
                        OL_Anz_Mehrfachantraege = null,
                        OL_Anz_OP = null,
                        OL_Anz_Stundungen = null,
                        OL_Anz_Vertraege = null,
                        OL_Anz_Vertraege_im_Recovery = null,
                        OL_Anz_Verzichte_l12M = null,
                        OL_Anz_Zahlungsvereinbarungen = null,
                        OL_Effektive_Kundenbeziehung = null,
                        OL_Engagement = null,
                        OL_Eventualengagement = null,
                        OL_Gesamtengagement = null,
                        OL_Haushaltsengagement = null,
                        OL_Letzte_Miete = null,
                        OL_Letzte_Nationalitaet = null,
                        OL_Letzter_Bonus = null,
                        OL_Letzter_Zivilstand = null,
                        OL_Letztes_Arbeitsverhaeltnis = null,
                        OL_Letztes_Haupteinkommen = null,
                        OL_Letztes_Nebeneinkommen = null,
                        OL_Letztes_Wohnverhaeltnis = null,
                        OL_Letztes_Zusatzeinkommen = null,
                        OL_Maximale_Mahnstufe = null,
                        OL_Maximale_Risikoklasse_des_Kunden = null,
                        OL_Maximaler_Bandlisteintrag = null,
                        OL_Minimales_Datum_Kunde = null,
                        OL_OpenLease_Datum_der_Anmeldung = null,
                        OL_Status = null,
                        OL_Summe_OP = null,
                        ZEK_Anz_lfd_ZEK_Eng = null,
                        ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03 = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04 = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_0506 = null,
                        ZEK_Anz_lfd_ZEK_Eng_Festkredit = null,
                        ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = null,
                        ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = null,
                        ZEK_Anz_lfd_ZEK_Eng_Leasing = null,
                        ZEK_Anz_lfd_ZEK_Eng_Teilz = null,
                        ZEK_Anz_lfd_ZEK_FremdGes = null,
                        ZEK_Anz_ZEK_AmtsMelden_01_05 = null,
                        ZEK_Anz_ZEK_AmtsMelden_01_05_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_05060812 = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = null,
                        ZEK_Anz_ZEK_Gesuche = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = null,
                        ZEK_Anz_ZEK_Synonyme = null,
                        ZEK_Anz_ZEK_Vertraege = null,
                        ZEK_Datum_der_Auskunft = null,
                        ZEK_Kunde_Gesamtengagement = null,
                        ZEK_schlechtester_ZEK_AblCode = null,
                        ZEK_schlechtester_ZEK_Code = null,
                        ZEK_Status = null
                    }
                }
            };

            // Not possible to use using because StrategyOneResponse exists in DTO.Auskunft and in DAO.Auskunft.DecisionEngineRef
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef.StrategyOneResponse StrategyOneResponse = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef.StrategyOneResponse()
            {
                errorCode = 12,
                executionTime = (long)10.12,
                log = "hier was gelogtes",
                message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<StrategyOneResponse type=\"A\">" +
	                            "<Header>" +
		                            "<InquiryCode>15725684</InquiryCode>" +
		                            "<ProcessCode>RISK</ProcessCode>" +
		                            "<OrganizationCode>BNOW</OrganizationCode>" +
		                            "<ProcessVersion>1</ProcessVersion>" +
	                            "</Header>" +
	                        "<Body>" +
		                        "<RecordNR>" +
			                        "<I_INQUIRY_DATE v=\"2010-05-11\"/>" +
			                        "<I_INQUIRY_TIME/>" +
			                        "<I_IO_VP v=\"0\"/>" +
			                        "<I_IO_BP v=\"0\"/>" +
			                        "<I_IO_RP v=\"1\"/>" +
			                        "<I_C_Geschaeftsart v=\"BARKR_V\"/>" +
			                        "<I_C_Erfassungskanal/>" +
			                        "<I_C_Riskflag v=\"0\"/>" +
			                        "<I_C_Finanzierungsbetrag v=\"8000\"/>" +
			                        "<I_C_Anzahlung_Erste_Rate v=\"0\"/>" +
			                        "<I_C_Vertragsart v=\"3\"/>" +
			                        "<I_C_Laufzeit v=\"48\"/>" +
			                        "<I_C_Zinssatz v=\"12.5\"/>" +
			                        "<I_C_PPI_Flag_Paket1 v=\"1\"/>" +
			                        "<I_C_PPI_Flag_Paket2 v=\"1\"/>" +
			                        "<I_C_Rate v=\"224.47\"/>" +
			                        "<I_C_Restwert v=\"0\"/>" +
			                        "<I_C_Kaution/>" +
			                        "<I_C_KKG_Pflicht v=\"1\"/>" +
			                        "<I_C_Nutzungsart v=\"PRIVAT\"/>" +
			                        "<I_C_Budgetueberschuss_1/>" +
			                        "<I_C_Budgetueberschuss_2/>" +
			                        "<I_C_Budgetueberschuss_gesamt/>" +
			                        "<I_O_Objektart/>" +
			                        "<I_O_Marke/>" +
			                        "<I_O_Modell/>" +
			                        "<I_O_Zustand/>" +
			                        "<I_O_KM_Stand/>" +
			                        "<I_O_KM_prJahr/>" +
			                        "<I_O_1_Inverkehrssetzung/>" +
			                        "<I_O_Stammnummer v=\"137.512.440\"/>" +
			                        "<I_O_Zubehoerpreis v=\"0\"/>" +
			                        "<I_O_Katalogpreis_Eurotax v=\"8000\"/>" +
			                        "<I_O_Fahrzeugpreis_Eurotax v=\"8000\"/>" +
			                        "<I_O_Restwert_Eurotax v=\"0\"/>" +
			                        "<I_O_Restwert_BANK_now v=\"0\"/>" +
			                        "<I_S_Anz_Abloesen v=\"0\"/>" +
			                        "<I_S_Anz_Eigenabloesen v=\"1\"/>" +
			                        "<I_S_Anz_Fremdabloesen v=\"1\"/>" +
 			                        "<I_S_Summe_Abloesen v=\"0\"/>" +
			                        "<I_S_Summe_Eigenabloesen v=\"1\"/>" +
			                        "<I_S_Summe_Fremdabloesen v=\"1\"/>" +
			                        "<I_S_Name_Abloesebank_1 v=\"224,47\"/>" +
			                        "<I_S_Name_Abloesebank_2 v=\"0\"/>" +
			                        "<I_S_Name_Abloesebank_3 v=\"0\"/>" +
			                        "<I_S_Name_Abloesebank_4 v=\"3\"/>" +
			                        "<I_S_Name_Abloesebank_5 v=\"12,5\"/>" +
			                        "<I_VP_VertriebspartnerID v=\"62346\"/>" +
			                        "<I_VP_Vertriebspartnerart/>" +
			                        "<I_VP_Rechtsform/>" +
			                        "<I_VP_PLZ v=\"1201\"/>" +
			                        "<I_VP_Sprache v=\"fr-CH\"/>" +
			                        "<I_VP_flagAktiv v=\"1\"/>" +
			                        "<I_VP_flagEPOS v=\"1\"/>" +
			                        "<I_VP_flagVSB/>" +
  			                        "<I_VP_Garantenlimite/>" +
			                        "<I_VP_Volumenengagement/>" +
			                        "<I_VP_Eventualvolumenengagement/>" +
			                        "<I_VP_Restwertengagement/>" +
			                        "<I_VP_Eventualrestwertengagement/>" +
			                        "<I_VP_Anz_Antraege/>" +
			                        "<I_VP_Anz_pendente_Antraege/>" +
			                        "<I_VP_Anz_Vertraege/>" +
			                        "<I_VP_Anz_lfd_Vertraege/>" +
		                        "</RecordNR>" +
		                        "<RecordRR>" +
			                        "<Record>" +
				                        "<I_A_KundenID v=\"11429\"/>" +
				                        "<I_A_MO_Counter v=\"1\"/>" +
				                        "<I_A_Kundenart v=\"PRIVAT\"/>" +
				                        "<I_A_Geburtsdatum v=\"1958-07-23\"/>" +
				                        "<I_A_PLZ v=\"1000\"/>" +
				                        "<I_A_Land v=\"CH\"/>" +
				                        "<I_A_Kanton v=\"VD\"/>" +
				                        "<I_A_Sprache v=\"fr-CH\"/>" +
				                        "<I_A_Status/>" +
				                        "<I_A_Telefon_privat v=\"51741\"/>" +
				                        "<I_A_Telefon_geschaeftlich v=\"61170\"/>" +
				                        "<I_A_Mobiltelefon v=\"99606\"/>" +
				                        "<I_A_E_Mail/>" +
				                        "<I_A_hier_Wohnhaft_seit/>" +
				                        "<I_A_Auszahlungsart/>" +
				                        "<I_A_Rueckzahlungsart/>" +
				                        "<I_A_marbeiter_Credit_Suisse_Group/>" +
				                        "<I_A_CS_Einheit/>" +
				                        "<I_A_A_PID v=\"ESR\"/>" +
				                        "<I_A_Instradierung/>" +
				                        "<I_A_Zivilstand v=\"VERH\"/>" +
				                        "<I_A_Wohnverhaeltnis v=\"EHE\"/>" +
				                        "<I_A_Anz_Kinder_bis_6 v=\"0\"/>" +
				                        "<I_A_Anz_Kinder_ueber_6_bis_10 v=\"0\"/>" +
				                        "<I_A_Anz_Kinder_ueber_10_bis_12 v=\"0\"/>" +
				                        "<I_A_Anz_unterstuetzungsp_Kinder_ab_12 v=\"0\"/>" +
				                        "<I_A_Nationalitaet v=\"101\"/>" +
				                        "<I_A_Auslaenderausweis/>" +
				                        "<I_A_Auslaenderausweis_Einreisedatum/>" +
				                        "<I_A_Auslaenderausweis_Gueltigkeitsdatum/>" +
				                        "<I_A_Berufliche_Situation v=\"ANGEST_UNBEFR\"/>" +
				                        "<I_A_Arbeitgeber_Seit_wann v=\"1984-01-01\"/>" +
				                        "<I_A_Arbeitgeber_Beschaeftigt_bis/>" +
				                        "<I_A_Einkommen_Art v=\"NETTO\"/>" +
				                        "<I_A_Haupteinkommen_Betrag v=\"6833\"/>" +
				                        "<I_A_Jaehrl_Gratifikation_Bonus v=\"0\"/>" +
				                        "<I_A_13_Monatslohn v=\"0\"/>" +
				                        "<I_A_Quellensteuerpflichtig/>" +
				                        "<I_A_Nebeneinkommen_Betrag v=\"0\"/>" +
				                        "<I_A_Zusatzeinkommen_Betrag v=\"0\"/>" +
				                        "<I_A_Zusatzeinkommen/>" +
				                        "<I_A_Anz_der_Betreibungen v=\"0\"/>" +
				                        "<I_A_Hoehe_der_Betreibungen/>" +
				                        "<I_A_Verlustscheine_Pfaendungen/>" +
				                        "<I_A_Wohnkosten_Miete v=\"1625\"/>" +
				                        "<I_A_Bestehende_Kreditrate v=\"0\"/>" +
				                        "<I_A_Bestehende_Leasingrate v=\"0\"/>" +
				                        "<I_A_Regelmaessige_Auslagen_Betrag v=\"0\"/>" +
				                        "<I_A_Regelmaessige_Auslagen/>" +
				                        "<I_A_Unterstuetzungsbeitraege_Betrag/>" +
				                        "<I_A_Unterstuetzungsbeitraege/>" +
				                        "<I_A_Berufsauslagen_Betrag v=\"0\"/>" +
				                        "<I_A_Berufsauslagen/>" +
				                        "<I_A_Telefon_1/>" +
				                        "<I_A_Telefon_2/>" +
				                        "<I_A_Jahresumsatz/>" +
				                        "<I_A_Eigenkapital/>" +
				                        "<I_A_Bilanzsumme/>" +
				                        "<I_A_Jahresgewinn/>" +
				                        "<I_A_Fluessige_mtel/>" +
				                        "<I_A_Kurzfristige_Verbindlichkeiten/>" +
				                        "<I_A_In_Handelsregister_eingetragen/>" +
				                        "<I_A_Revisionsstelle_vorhanden/>" +
				                        "<I_A_Datum_letzter_Jahresabschluss/>" +
				                        "<I_A_Anz_Mitarbeiter/>" +
				                        "<I_DV_Datum_der_Auskunft v=\"2010-05-11\"/>" +
				                        "<I_DV_Zeit_der_Auskunft/>" +
				                        "<I_DV_Status_Auskunft_Adressvalidierung v=\"1\"/>" +
                                        "<I_DV_Anz_DV_Treffer_Adressvalidierung v=\"1\"/>" +
                                        "<I_DV_KundenID v=\"56439\"/>" +
                                        "<I_DV_Geburtsdatum/>" +
                                        "<I_DV_PLZ/>" +
                                        "<I_DV_Land/>" +
                                        "<I_DV_Datum_der_ersten_Meld/>" +
                                        "<I_DV_Datum_an_der_aktuellen_Adresse_seit/>" +
                                        "<I_DV_Firmenstatus/>" +
                                        "<I_DV_Gruendungsdatum/>" +
                                        "<I_DV_NOGA_Code_Branche/>" +
                                        "<I_DV_Rechtsform/>" +
                                        "<I_DV_Kapital/>" +
                                        "<I_DV_Fraud_Feld/>" +
                                        "<I_DV_Anz_BPM_l12M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_l24M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_l36M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l36M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l36M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_03_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_03_l36M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l36M/>" +
                                        "<I_DV_Schlechtester_FStat_l12M/>" +
                                        "<I_DV_Schlechtester_FStat_l24M/>" +
                                        "<I_DV_Schlechtester_FStat_l36M/>" +
                                        "<I_DV_AG_Datum/>" +
                                        "<I_DV_AG_Zeit/>" +
                                        "<I_DV_AG_Status v=\"1\"/>" +
                                        "<I_DV_AG_Rechtsform v=\"2\"/>" +
                                        "<I_DV_AG_Gruendungsdatum/>" +
                                        "<I_DV_AG_Firmenstatus/>" +
                                        "<I_DV_AG_Decision_Maker/>" +
                                        "<I_DV_AG_NOGA_Code/>" +
                                        "<I_ZEK_Datum_der_Auskunft v=\"2011-01-31\"/>" +
                                        "<I_ZEK_Status v=\"12\"/>" +
                                        "<I_ZEK_Kunde_Gesamtengagement v=\"10739\"/>" +
                                        "<I_ZEK_Anz_ZEK_Synonyme v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Vertraege v=\"4\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng v=\"3\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Bardarlehen v=\"1\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Festkredit v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Leasing v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_TeilzVertrag v=\"1\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Kontokorrent v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Kartenengagement v=\"1\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_05_06 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l36M v=\"0\"/>" +
                                        "<I_ZEK_schlechtester_ZEK_BCode v=\"1\"/>" +
                                        "<I_ZEK_Anz_ZEK_Gesuche v=\"2\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_FremdGes v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_05_06_08_12/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode/>" +
                                        "<I_ZEK_schlechtester_ZEK_AblCode/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_AmtsMelden_01_05/>" +
                                        "<I_ZEK_Anz_ZEK_AmtsMelden_01_05_l12M/>" +
                                        "<I_OL_OpenLease_Datum_der_Auskunft v=\"2010-05-11\"/>" +
                                        "<I_OL_Status v=\"1\"/>" +
                                        "<I_OL_Anz_KundenIDs v=\"1\"/>" +
                                        "<I_OL_Minimales_Datum_Kunde_seit/>" +
                                        "<I_OL_Maximaler_Badlisteintrag v=\"0\"/>" +
                                        "<I_OL_Maximale_akt_RKlasse_des_Kunden/>" +
                                        "<I_OL_Maximale_Risikoklasse_des_Kunden/>" +
                                        "<I_OL_Anz_Antraege/>" +
                                        "<I_OL_Anz_Mehrfachantraege/>" +
                                        "<I_OL_Anz_Annulierungen_l12M/>" +
                                        "<I_OL_Anz_Verzichte_l12M/>" +
                                        "<I_OL_Eventualengagement/>" +
                                        "<I_OL_Gesamtengagement/>" +
                                        "<I_OL_Haushaltengagement/>" +
                                        "<I_OL_Letzte_Miete/>" +
                                        "<I_OL_Letzter_Zivilstand/>" +
                                        "<I_OL_Letztes_Wohnverhaeltnis/>" +
                                        "<I_OL_Letzte_Nationalitaet/>" +
                                        "<I_OL_Letztes_Arbeitsverhaeltnis/>" +
                                        "<I_OL_Letztes_Haupteinkommen/>" +
                                        "<I_OL_Letztes_Nebeneinkommen/>" +
                                        "<I_OL_Letztes_Zusatzeinkommen/>" +
                                        "<I_OL_Letzter_Bonus/>" +
                                        "<I_OL_Engagement/>" +
                                        "<I_OL_Anz_Vertraege/>" +
                                        "<I_OL_Anz_lfd_Vertraege/>" +
                                        "<I_OL_Anz_Vertraege_im_Recovery/>" +
                                        "<I_OL_Anz_Mahnungen_1/>" +
                                        "<I_OL_Anz_Mahnungen_2/>" +
                                        "<I_OL_Anz_Mahnungen_3/>" +
                                        "<I_OL_Maximale_Mahnstufe/>" +
                                        "<I_OL_Anz_Stundungen/>" +
                                        "<I_OL_Anz_Zahlungsvereinbarungen/>" +
                                        "<I_OL_Summe_OP/>" +
                                        "<I_OL_Anz_OP/>" +
                                        "<I_OL_Effektive_Kundenbeziehung/>" +
                                        "<I_OL_Dauer_Kundenbeziehung/>" +
                                    "</Record>" +
                                "</RecordRR>" +
                            "</Body>" +
                        "</StrategyOneResponse>"
            };

            decisionEngineWSDaoMock.SetReturnValue("execute", StrategyOneResponse);
            try
            {
                AuskunftDto auskunftDto = decisionEngineBo.execute(decisionEngineInDto);
                Assert.AreNotEqual(null, auskunftDto.DecisionEngineOutDto.Code);
            }
            catch (Exception ex)
            {
                // Entsorgt die nicht verwendung der Exception
                String message = ex.Message;
            }
            
        }

        /// <summary>
        /// Blackboxtest für die execute Methode mit dem Inputwert sysAuskunft(long)
        /// </summary>
        [Test]
        public void executeWithsysAuskunft()
        {
            AuskunftDto auskunftOutDto = new AuskunftDto()
            {
                Anfragedatum = new DateTime(2011,02,08),
                Anfrageuhrzeit = (long)11.27,
                DecisionEngineInDto = new DecisionEngineInDto(),
                DecisionEngineOutDto = new DecisionEngineOutDto(),
                Fehlercode = "Keinen Fehler",
                KremoInDto = new KREMOInDto(),
                KremoOutDto = new KREMOOutDto(),
                RecordRRDto = new RecordRRDto(),
                Status = "Neu",
                sysAuskunfttyp = 2,
                sysAuskunft = 1,
                EurotaxInDto = new EurotaxInDto(),
                EurotaxOutDto = new EurotaxOutDto(),
            };

            DecisionEngineInDto decisionEngineInDto = new DecisionEngineInDto()
            {
                OrganizationCode = "BNOW",
                ProcessCode = "RISK",
                ProcessVersion = 1,
                InquiryCode = "14496592", //Wichtig!!!
                InquiryDate = System.DateTime.ParseExact("20070508", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                InquiryTime = "10:20",
                FlagVorpruefung = 1,
                FlagBonitaetspruefung = 0,
                FlagRisikopruefung = 0,
                Geschaeftsart = StrategyOneRequestBodyRecordNRI_C_GeschaeftsartV.FF_V.ToString(),
                Riskflag = 0,
                Finanzierungsbetrag = 117400,
                Anzahlung_ErsteRate = 17600,
                Vertragsart = 1,
                Laufzeit = 49,
                Zinssatz = 4.9m,
                PPI_Flag_Paket1 = 0,
                PPI_Flag_Paket2 = 0,
                Rate = 1625.41m,
                Restwert = 35508,
                KKG_Pflicht = 0,
                Nutzungsart = StrategyOneRequestBodyRecordNRI_C_NutzungsartV.PRIVAT.ToString(),
                Marke = "PORSCHE",
                Zustand = 1,
                Zubehoerpreis = 0,
                KM_Stand = 16000,
                KM_prJahr = 15000,
                Inverkehrssetzung = System.DateTime.ParseExact("20050323", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                Stammnummer = "137.512.440",
                Katalogpreis_Eurotax = 159785,
                Fahrzeugpreis_Eurotax = 159785,
                Restwert_Eurotax = 22990,
                Restwert_Banknow = 22990,
                Anz_Abloesen = 0,
                Anz_Eigenabloesen = 0,
                Anz_Fremdabloesen = 0,
                Summe_Abloesen = 0,
                Summe_Eigenabloesen = 0,
                //_diorig.Name_Abloesebank_1 = "BANK-now AG";
                VertriebspartnerID = 83426,
                Sprache = StrategyOneRequestBodyRecordNRI_VP_SpracheV.frCH.ToString(),
                flagAktiv = 1,
                flagEPOS = 0,
                PLZ = "1180",
                RecordRRDto = new RecordRRDto[]
                {
                    new RecordRRDto()
                    {
                        A_13_Montaslohn = 10000,
                        A_A_PID = "A_APID",
                        A_Anz_der_Betreibungen = 1,
                        A_Anz_Kinder_bis_6 = 0,
                        A_Anz_Kinder_ueber_10_bis_12 = 0,
                        A_Anz_Kinder_ueber_6_bis_10 = 0,
                        A_Anz_Mitarbeiter = 100,
                        A_Anz_unterstuetzungsp_Kinder_ab_12 = 0,
                        A_Arbeitgeber_beschaeftigt_bis = new DateTime(2020,01,01),
                        A_Arbeitgeber_seit_wann = new DateTime(2000,01,01),
                        A_Auslaenderausweis = null,
                        A_Auslaenderausweis_Einreisedatum = null,
                        A_Auslaenderausweis_Gueltigkeitsdatum = null,
                        A_Auszahlungsart = "BAR",
                        A_Berufliche_Situation = "ANGEST_UNBEFR",
                        A_Berufsauslagen = null,
                        A_Berufsauslagen_Betrag = null,
                        A_Bestehende_Kreditrate = 1000,
                        A_Bestehende_Leasingrate = 300,
                        A_Bilanzsumme = 1300,
                        A_CS_Einheit = "CS",
                        A_Datum_letzter_Jahresabschluss = new DateTime(2010,01,11),
                        A_E_Mail = "EineE@mail.com",
                        A_Eigenkapital = 1000,
                        A_Einkommen_Art = "BRUTTO",
                        A_fluessige_mtel = 2,
                        A_Geburtsdatum = new DateTime(1980,12,12),
                        A_Haupteinkommen_Betrag = 10000,
                        A_hier_Wohnhaft_seit = new DateTime(1980,12,12),
                        A_Hoehe_der_Betreibungen = 1000,
                        A_In_Handelsregister_eingetragen = null,
                        A_Instradierung = "Instradierung",
                        A_Jaehl_Gratifikation_Bonus = 20,
                        A_Jahregewinn = 10,
                        A_Jahresumsatz = 100000000,
                        A_Kanton = "Be",
                        A_Kundenart = "PRIVAT",
                        A_KundenID = 1337,
                        A_Kurzfristige_Verbindlichkeiten = 0,
                        A_Land = "Schweiz",
                        A_marbeiter_Credit_Suisse_Group = 0,
                        A_MO_Counter = 0,
                        A_Mobiltelefon = "017012334566",
                        A_Nationalitaet = "Schweizer",
                        A_Nebeneinkommen_Betrag = 1000000,
                        A_PLZ = "1234",
                        A_Quellensteuerpflichtig = 0,
                        A_Regelmaessige_Auslagen = "ZUSATZVER",
                        A_Regelmaessige_Auslagen_Betrag = 0,
                        A_Revisionsstelle_vorhanden = 0,
                        A_Rueckzahlungsart = "ESR",
                        A_Sprache = "deCH",
                        A_Status = "PARTNER",
                        A_Telefon_1 = "1234",
                        A_Telefon_2 = null,
                        A_Telefon_geschaeftlich = null,
                        A_Telefon_privat = null,
                        A_Unterstuetzungsbeitraege = "ANDERES",
                        A_Unterstuetzungsbeitraege_Betrag = 0,
                        A_Verlustscheine_Pfaendungen = 10,
                        A_Wohnkosten_Miete = 1000,
                        A_Wohnverhaeltnis = "ALLEIN",
                        A_Zivilstand = "LED",
                        A_Zusatzeinkommen = "MIETEINNAHMEN",
                        A_Zusatzeinkommen_Betrag = 100000,
                        DV_AG_Datum = null,
                        DV_AG_Decision_Maker = null,
                        DV_AG_Firmenstatus = null,
                        DV_AG_Gruendungsdatum = null,
                        DV_AG_NOGA_Code = null,
                        DV_AG_Rechtsform = null,
                        DV_AG_Status = null,
                        DV_AG_Zeit = null,
                        DV_Anz_BPM_l12m = null,
                        DV_Anz_BPM_l24m = null,
                        DV_Anz_BPM_l36m = null,
                        DV_Anz_BPM_m_FStat_01_l12m = null,
                        DV_Anz_BPM_m_FStat_01_l24m = null,
                        DV_Anz_BPM_m_FStat_01_l36m = null,
                        DV_Anz_BPM_m_FStat_02_l12m = null,
                        DV_Anz_BPM_m_FStat_02_l24m = null,
                        DV_Anz_BPM_m_FStat_02_l36m = null,
                        DV_Anz_BPM_m_FStat_03_l12m = null,
                        DV_Anz_BPM_m_FStat_03_l24m = null,
                        DV_Anz_BPM_m_FStat_03_l36m = null,
                        DV_Anz_BPM_m_FStat_04_l12m = null,
                        DV_Anz_BPM_m_FStat_04_l24m = null,
                        DV_Anz_BPM_m_FStat_04_l36m = null,
                        DV_Anz_DV_Treffer_Adressvalidierung = null,
                        DV_Datum_an_der_aktuellen_Adresse_seit = null,
                        DV_Datum_der_Auskunft = null,
                        DV_Datum_der_ersten_Meld = null,
                        DV_Firmenstatus = null,
                        DV_Fraud_Feld = null,
                        DV_Geburtsdatum = null,
                        DV_Gruendungsdatum = null,
                        DV_Kapital = null,
                        DV_KundenID = null,
                        DV_Land = null,
                        DV_NOGA_Code_Branche = null,
                        DV_PLZ = null,
                        DV_Rechtform = null,
                        DV_Schlechtester_FStat_l12m = null,
                        DV_Schlechtester_FStat_l24m = null,
                        DV_Schlechtester_FStat_l36m = null,
                        DV_Status_Auskunft_Adressvalidierung = null,
                        DV_Zeit_der_Auskunft = null,
                        OL_Dauer_Kundenbeziehung = null,
                        OL_Maximale_akt_RKlasse_des_Kunden = null,
                        OL_Anz_Annulierungen_l12M = null,
                        OL_Anz_Antraege = null,
                        OL_Anz_KundenIDs = null,
                        OL_Anz_lfd_Vertraege = null,
                        OL_Anz_Mahnungen_1 = null,
                        OL_Anz_Mahnungen_2 = null,
                        OL_Anz_Mahnungen_3 = null,
                        OL_Anz_Mehrfachantraege = null,
                        OL_Anz_OP = null,
                        OL_Anz_Stundungen = null,
                        OL_Anz_Vertraege = null,
                        OL_Anz_Vertraege_im_Recovery = null,
                        OL_Anz_Verzichte_l12M = null,
                        OL_Anz_Zahlungsvereinbarungen = null,
                        OL_Effektive_Kundenbeziehung = null,
                        OL_Engagement = null,
                        OL_Eventualengagement = null,
                        OL_Gesamtengagement = null,
                        OL_Haushaltsengagement = null,
                        OL_Letzte_Miete = null,
                        OL_Letzte_Nationalitaet = null,
                        OL_Letzter_Bonus = null,
                        OL_Letzter_Zivilstand = null,
                        OL_Letztes_Arbeitsverhaeltnis = null,
                        OL_Letztes_Haupteinkommen = null,
                        OL_Letztes_Nebeneinkommen = null,
                        OL_Letztes_Wohnverhaeltnis = null,
                        OL_Letztes_Zusatzeinkommen = null,
                        OL_Maximale_Mahnstufe = null,
                        OL_Maximale_Risikoklasse_des_Kunden = null,
                        OL_Maximaler_Bandlisteintrag = null,
                        OL_Minimales_Datum_Kunde = null,
                        OL_OpenLease_Datum_der_Anmeldung = null,
                        OL_Status = null,
                        OL_Summe_OP = null,
                        ZEK_Anz_lfd_ZEK_Eng = null,
                        ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03 = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04 = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M = null,
                        ZEK_Anz_lfd_ZEK_Eng_BCode_0506 = null,
                        ZEK_Anz_lfd_ZEK_Eng_Festkredit = null,
                        ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = null,
                        ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = null,
                        ZEK_Anz_lfd_ZEK_Eng_Leasing = null,
                        ZEK_Anz_lfd_ZEK_Eng_Teilz = null,
                        ZEK_Anz_lfd_ZEK_FremdGes = null,
                        ZEK_Anz_ZEK_AmtsMelden_01_05 = null,
                        ZEK_Anz_ZEK_AmtsMelden_01_05_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_05060812 = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = null,
                        ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = null,
                        ZEK_Anz_ZEK_Gesuche = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = null,
                        ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = null,
                        ZEK_Anz_ZEK_Synonyme = null,
                        ZEK_Anz_ZEK_Vertraege = null,
                        ZEK_Datum_der_Auskunft = null,
                        ZEK_Kunde_Gesamtengagement = null,
                        ZEK_schlechtester_ZEK_AblCode = null,
                        ZEK_schlechtester_ZEK_Code = null,
                        ZEK_Status = null
                    }
                }
            };

            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef.StrategyOneResponse StrategyOneResponse = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DecisionEngineRef.StrategyOneResponse()
            {
                errorCode = 12,
                executionTime = (long)10.12,
                log = "hier was gelogtes",
                message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<StrategyOneResponse type=\"A\">" +
                                "<Header>" +
                                    "<InquiryCode>15725684</InquiryCode>" +
                                    "<ProcessCode>RISK</ProcessCode>" +
                                    "<OrganizationCode>BNOW</OrganizationCode>" +
                                    "<ProcessVersion>1</ProcessVersion>" +
                                "</Header>" +
                            "<Body>" +
                                "<RecordNR>" +
                                    "<I_INQUIRY_DATE v=\"2010-05-11\"/>" +
                                    "<I_INQUIRY_TIME/>" +
                                    "<I_IO_VP v=\"0\"/>" +
                                    "<I_IO_BP v=\"0\"/>" +
                                    "<I_IO_RP v=\"1\"/>" +
                                    "<I_C_Geschaeftsart v=\"BARKR_V\"/>" +
                                    "<I_C_Erfassungskanal/>" +
                                    "<I_C_Riskflag v=\"0\"/>" +
                                    "<I_C_Finanzierungsbetrag v=\"8000\"/>" +
                                    "<I_C_Anzahlung_Erste_Rate v=\"0\"/>" +
                                    "<I_C_Vertragsart v=\"3\"/>" +
                                    "<I_C_Laufzeit v=\"48\"/>" +
                                    "<I_C_Zinssatz v=\"12.5\"/>" +
                                    "<I_C_PPI_Flag_Paket1 v=\"1\"/>" +
                                    "<I_C_PPI_Flag_Paket2 v=\"1\"/>" +
                                    "<I_C_Rate v=\"224.47\"/>" +
                                    "<I_C_Restwert v=\"0\"/>" +
                                    "<I_C_Kaution/>" +
                                    "<I_C_KKG_Pflicht v=\"1\"/>" +
                                    "<I_C_Nutzungsart v=\"PRIVAT\"/>" +
                                    "<I_C_Budgetueberschuss_1/>" +
                                    "<I_C_Budgetueberschuss_2/>" +
                                    "<I_C_Budgetueberschuss_gesamt/>" +
                                    "<I_O_Objektart/>" +
                                    "<I_O_Marke/>" +
                                    "<I_O_Modell/>" +
                                    "<I_O_Zustand/>" +
                                    "<I_O_KM_Stand/>" +
                                    "<I_O_KM_prJahr/>" +
                                    "<I_O_1_Inverkehrssetzung/>" +
                                    "<I_O_Stammnummer v=\"137.512.440\"/>" +
                                    "<I_O_Zubehoerpreis v=\"0\"/>" +
                                    "<I_O_Katalogpreis_Eurotax v=\"8000\"/>" +
                                    "<I_O_Fahrzeugpreis_Eurotax v=\"8000\"/>" +
                                    "<I_O_Restwert_Eurotax v=\"0\"/>" +
                                    "<I_O_Restwert_BANK_now v=\"0\"/>" +
                                    "<I_S_Anz_Abloesen v=\"0\"/>" +
                                    "<I_S_Anz_Eigenabloesen v=\"1\"/>" +
                                    "<I_S_Anz_Fremdabloesen v=\"1\"/>" +
                                    "<I_S_Summe_Abloesen v=\"0\"/>" +
                                    "<I_S_Summe_Eigenabloesen v=\"1\"/>" +
                                    "<I_S_Summe_Fremdabloesen v=\"1\"/>" +
                                    "<I_S_Name_Abloesebank_1 v=\"224,47\"/>" +
                                    "<I_S_Name_Abloesebank_2 v=\"0\"/>" +
                                    "<I_S_Name_Abloesebank_3 v=\"0\"/>" +
                                    "<I_S_Name_Abloesebank_4 v=\"3\"/>" +
                                    "<I_S_Name_Abloesebank_5 v=\"12,5\"/>" +
                                    "<I_VP_VertriebspartnerID v=\"62346\"/>" +
                                    "<I_VP_Vertriebspartnerart/>" +
                                    "<I_VP_Rechtsform/>" +
                                    "<I_VP_PLZ v=\"1201\"/>" +
                                    "<I_VP_Sprache v=\"fr-CH\"/>" +
                                    "<I_VP_flagAktiv v=\"1\"/>" +
                                    "<I_VP_flagEPOS v=\"1\"/>" +
                                    "<I_VP_flagVSB/>" +
                                    "<I_VP_Garantenlimite/>" +
                                    "<I_VP_Volumenengagement/>" +
                                    "<I_VP_Eventualvolumenengagement/>" +
                                    "<I_VP_Restwertengagement/>" +
                                    "<I_VP_Eventualrestwertengagement/>" +
                                    "<I_VP_Anz_Antraege/>" +
                                    "<I_VP_Anz_pendente_Antraege/>" +
                                    "<I_VP_Anz_Vertraege/>" +
                                    "<I_VP_Anz_lfd_Vertraege/>" +
                                "</RecordNR>" +
                                "<RecordRR>" +
                                    "<Record>" +
                                        "<I_A_KundenID v=\"11429\"/>" +
                                        "<I_A_MO_Counter v=\"1\"/>" +
                                        "<I_A_Kundenart v=\"PRIVAT\"/>" +
                                        "<I_A_Geburtsdatum v=\"1958-07-23\"/>" +
                                        "<I_A_PLZ v=\"1000\"/>" +
                                        "<I_A_Land v=\"CH\"/>" +
                                        "<I_A_Kanton v=\"VD\"/>" +
                                        "<I_A_Sprache v=\"fr-CH\"/>" +
                                        "<I_A_Status/>" +
                                        "<I_A_Telefon_privat v=\"51741\"/>" +
                                        "<I_A_Telefon_geschaeftlich v=\"61170\"/>" +
                                        "<I_A_Mobiltelefon v=\"99606\"/>" +
                                        "<I_A_E_Mail/>" +
                                        "<I_A_hier_Wohnhaft_seit/>" +
                                        "<I_A_Auszahlungsart/>" +
                                        "<I_A_Rueckzahlungsart/>" +
                                        "<I_A_marbeiter_Credit_Suisse_Group/>" +
                                        "<I_A_CS_Einheit/>" +
                                        "<I_A_A_PID v=\"ESR\"/>" +
                                        "<I_A_Instradierung/>" +
                                        "<I_A_Zivilstand v=\"VERH\"/>" +
                                        "<I_A_Wohnverhaeltnis v=\"EHE\"/>" +
                                        "<I_A_Anz_Kinder_bis_6 v=\"0\"/>" +
                                        "<I_A_Anz_Kinder_ueber_6_bis_10 v=\"0\"/>" +
                                        "<I_A_Anz_Kinder_ueber_10_bis_12 v=\"0\"/>" +
                                        "<I_A_Anz_unterstuetzungsp_Kinder_ab_12 v=\"0\"/>" +
                                        "<I_A_Nationalitaet v=\"101\"/>" +
                                        "<I_A_Auslaenderausweis/>" +
                                        "<I_A_Auslaenderausweis_Einreisedatum/>" +
                                        "<I_A_Auslaenderausweis_Gueltigkeitsdatum/>" +
                                        "<I_A_Berufliche_Situation v=\"ANGEST_UNBEFR\"/>" +
                                        "<I_A_Arbeitgeber_Seit_wann v=\"1984-01-01\"/>" +
                                        "<I_A_Arbeitgeber_Beschaeftigt_bis/>" +
                                        "<I_A_Einkommen_Art v=\"NETTO\"/>" +
                                        "<I_A_Haupteinkommen_Betrag v=\"6833\"/>" +
                                        "<I_A_Jaehrl_Gratifikation_Bonus v=\"0\"/>" +
                                        "<I_A_13_Monatslohn v=\"0\"/>" +
                                        "<I_A_Quellensteuerpflichtig/>" +
                                        "<I_A_Nebeneinkommen_Betrag v=\"0\"/>" +
                                        "<I_A_Zusatzeinkommen_Betrag v=\"0\"/>" +
                                        "<I_A_Zusatzeinkommen/>" +
                                        "<I_A_Anz_der_Betreibungen v=\"0\"/>" +
                                        "<I_A_Hoehe_der_Betreibungen/>" +
                                        "<I_A_Verlustscheine_Pfaendungen/>" +
                                        "<I_A_Wohnkosten_Miete v=\"1625\"/>" +
                                        "<I_A_Bestehende_Kreditrate v=\"0\"/>" +
                                        "<I_A_Bestehende_Leasingrate v=\"0\"/>" +
                                        "<I_A_Regelmaessige_Auslagen_Betrag v=\"0\"/>" +
                                        "<I_A_Regelmaessige_Auslagen/>" +
                                        "<I_A_Unterstuetzungsbeitraege_Betrag/>" +
                                        "<I_A_Unterstuetzungsbeitraege/>" +
                                        "<I_A_Berufsauslagen_Betrag v=\"0\"/>" +
                                        "<I_A_Berufsauslagen/>" +
                                        "<I_A_Telefon_1/>" +
                                        "<I_A_Telefon_2/>" +
                                        "<I_A_Jahresumsatz/>" +
                                        "<I_A_Eigenkapital/>" +
                                        "<I_A_Bilanzsumme/>" +
                                        "<I_A_Jahresgewinn/>" +
                                        "<I_A_Fluessige_mtel/>" +
                                        "<I_A_Kurzfristige_Verbindlichkeiten/>" +
                                        "<I_A_In_Handelsregister_eingetragen/>" +
                                        "<I_A_Revisionsstelle_vorhanden/>" +
                                        "<I_A_Datum_letzter_Jahresabschluss/>" +
                                        "<I_A_Anz_Mitarbeiter/>" +
                                        "<I_DV_Datum_der_Auskunft v=\"2010-05-11\"/>" +
                                        "<I_DV_Zeit_der_Auskunft/>" +
                                        "<I_DV_Status_Auskunft_Adressvalidierung v=\"1\"/>" +
                                        "<I_DV_Anz_DV_Treffer_Adressvalidierung v=\"1\"/>" +
                                        "<I_DV_KundenID v=\"56439\"/>" +
                                        "<I_DV_Geburtsdatum/>" +
                                        "<I_DV_PLZ/>" +
                                        "<I_DV_Land/>" +
                                        "<I_DV_Datum_der_ersten_Meld/>" +
                                        "<I_DV_Datum_an_der_aktuellen_Adresse_seit/>" +
                                        "<I_DV_Firmenstatus/>" +
                                        "<I_DV_Gruendungsdatum/>" +
                                        "<I_DV_NOGA_Code_Branche/>" +
                                        "<I_DV_Rechtsform/>" +
                                        "<I_DV_Kapital/>" +
                                        "<I_DV_Fraud_Feld/>" +
                                        "<I_DV_Anz_BPM_l12M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_l24M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_l36M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_01_l36M v=\"0\"/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_0_2_l36M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_03_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_03_l36M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l12M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l24M/>" +
                                        "<I_DV_Anz_BPM_m_FStat_04_l36M/>" +
                                        "<I_DV_Schlechtester_FStat_l12M/>" +
                                        "<I_DV_Schlechtester_FStat_l24M/>" +
                                        "<I_DV_Schlechtester_FStat_l36M/>" +
                                        "<I_DV_AG_Datum/>" +
                                        "<I_DV_AG_Zeit/>" +
                                        "<I_DV_AG_Status v=\"1\"/>" +
                                        "<I_DV_AG_Rechtsform v=\"2\"/>" +
                                        "<I_DV_AG_Gruendungsdatum/>" +
                                        "<I_DV_AG_Firmenstatus/>" +
                                        "<I_DV_AG_Decision_Maker/>" +
                                        "<I_DV_AG_NOGA_Code/>" +
                                        "<I_ZEK_Datum_der_Auskunft v=\"2011-01-31\"/>" +
                                        "<I_ZEK_Status v=\"12\"/>" +
                                        "<I_ZEK_Kunde_Gesamtengagement v=\"10739\"/>" +
                                        "<I_ZEK_Anz_ZEK_Synonyme v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Vertraege v=\"4\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng v=\"3\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Bardarlehen v=\"1\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Festkredit v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Leasing v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_TeilzVertrag v=\"1\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Kontokorrent v=\"0\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_Eng_Kartenengagement v=\"1\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_05_06 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_04_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Eng_m_BCode_03_l36M v=\"0\"/>" +
                                        "<I_ZEK_schlechtester_ZEK_BCode v=\"1\"/>" +
                                        "<I_ZEK_Anz_ZEK_Gesuche v=\"2\"/>" +
                                        "<I_ZEK_Anz_lfd_ZEK_FremdGes v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_05_06_08_12/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_Ges_m_AblCode/>" +
                                        "<I_ZEK_schlechtester_ZEK_AblCode/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 v=\"0\"/>" +
                                        "<I_ZEK_Anz_ZEK_AmtsMelden_01_05/>" +
                                        "<I_ZEK_Anz_ZEK_AmtsMelden_01_05_l12M/>" +
                                        "<I_OL_OpenLease_Datum_der_Auskunft v=\"2010-05-11\"/>" +
                                        "<I_OL_Status v=\"1\"/>" +
                                        "<I_OL_Anz_KundenIDs v=\"1\"/>" +
                                        "<I_OL_Minimales_Datum_Kunde_seit/>" +
                                        "<I_OL_Maximaler_Badlisteintrag v=\"0\"/>" +
                                        "<I_OL_Maximale_akt_RKlasse_des_Kunden/>" +
                                        "<I_OL_Maximale_Risikoklasse_des_Kunden/>" +
                                        "<I_OL_Anz_Antraege/>" +
                                        "<I_OL_Anz_Mehrfachantraege/>" +
                                        "<I_OL_Anz_Annulierungen_l12M/>" +
                                        "<I_OL_Anz_Verzichte_l12M/>" +
                                        "<I_OL_Eventualengagement/>" +
                                        "<I_OL_Gesamtengagement/>" +
                                        "<I_OL_Haushaltengagement/>" +
                                        "<I_OL_Letzte_Miete/>" +
                                        "<I_OL_Letzter_Zivilstand/>" +
                                        "<I_OL_Letztes_Wohnverhaeltnis/>" +
                                        "<I_OL_Letzte_Nationalitaet/>" +
                                        "<I_OL_Letztes_Arbeitsverhaeltnis/>" +
                                        "<I_OL_Letztes_Haupteinkommen/>" +
                                        "<I_OL_Letztes_Nebeneinkommen/>" +
                                        "<I_OL_Letztes_Zusatzeinkommen/>" +
                                        "<I_OL_Letzter_Bonus/>" +
                                        "<I_OL_Engagement/>" +
                                        "<I_OL_Anz_Vertraege/>" +
                                        "<I_OL_Anz_lfd_Vertraege/>" +
                                        "<I_OL_Anz_Vertraege_im_Recovery/>" +
                                        "<I_OL_Anz_Mahnungen_1/>" +
                                        "<I_OL_Anz_Mahnungen_2/>" +
                                        "<I_OL_Anz_Mahnungen_3/>" +
                                        "<I_OL_Maximale_Mahnstufe/>" +
                                        "<I_OL_Anz_Stundungen/>" +
                                        "<I_OL_Anz_Zahlungsvereinbarungen/>" +
                                        "<I_OL_Summe_OP/>" +
                                        "<I_OL_Anz_OP/>" +
                                        "<I_OL_Effektive_Kundenbeziehung/>" +
                                        "<I_OL_Dauer_Kundenbeziehung/>" +
                                    "</Record>" +
                                "</RecordRR>" +
                            "</Body>" +
                        "</StrategyOneResponse>"
            };

            auskunftDaoMock.ExpectAndReturn("FindBySysId", auskunftOutDto, 1);
            decisionEngineDBDaoMock.ExpectAndReturn("FindBySysId", decisionEngineInDto, 1);
            decisionEngineWSDaoMock.SetReturnValue("execute", StrategyOneResponse);
            decisionEngineDBDaoMock.Expect("SaveDecisionEngineOutput");
            AuskunftDto auskunftDto = decisionEngineBo.execute(1);
            Assert.AreEqual(2 , auskunftDto.sysAuskunfttyp);
        }
    }
}
