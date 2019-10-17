using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Windows.Media.Animation;
using Cic.OpenOne.GateBANKNOW.TestUI.Converters;
using Cic.OpenOne.GateBANKNOW.TestUI.UserControls;
using Cic.OpenOne.GateBANKNOW.TestUI.DTOS;
using Cic.OpenOne.GateBANKNOW.TestUI.DataAccess;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows.Markup;
using System.IO.Compression;
using System.Reflection;
using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.TestUI.UserControls
{
    /// <summary>
    /// Interaction logic for DecisionEngine.xaml
    /// </summary>
    public partial class DecisionEngine : UserControl
    {
        private ColumnDefinition _extraColLayerInputDecision;
        // INPUT DTOS and dynamic 
        // dynamic _di;
        StateControl DecisionStatus = new StateControl();

        Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto _diorig;

        /// <summary>
        /// Decision Engine
        /// </summary>
        public DecisionEngine()
        {
            InitializeComponent();
            _extraColLayerInputDecision = new ColumnDefinition();
            _extraColLayerInputDecision.Width = new GridLength(700);
            _extraColLayerInputDecision.SharedSizeGroup = "pinColDecision";
            LayerOutputDecision.Visibility = Visibility.Visible;
            btnPinItDecision.IsChecked = true;
            _diorig = new DecisionEngineInDto();
            //Vorführungsdaten
            /*
            _diorig.OrganizationCode = "BNOW";
            _diorig.ProcessCode = "RISK";
            _diorig.ProcessVersion = 1;
            _diorig.InquiryCode = "16018287"; //Wichtig!!!
            _diorig.InquiryDate = System.DateTime.ParseExact("20110119", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
           
            _diorig.FlagVorpruefung = 0;
            _diorig.FlagBonitaetspruefung = 0;
            _diorig.FlagRisikopruefung = 1;
            
            _diorig.Geschaeftsart = StrategyOneRequestBodyRecordNRI_C_GeschaeftsartV.BARKR_V;
            _diorig.Riskflag = 0;
            _diorig.Finanzierungsbetrag = 45000;
            _diorig.Anzahlung_ErsteRate = 0;
            _diorig.Vertragsart = 3;
            _diorig.Laufzeit = 60;
            _diorig.Zinssatz = 12.5m;
            _diorig.PPI_Flag_Paket1 = 1;
            _diorig.PPI_Flag_Paket2 = 1;
            _diorig.Rate = 1075.16m;
            _diorig.KKG_Pflicht = 1;
            _diorig.Nutzungsart = StrategyOneRequestBodyRecordNRI_C_NutzungsartV.PRIVAT;
            _diorig.Zubehoerpreis = 0;
            _diorig.Katalogpreis_Eurotax = 45000;
            _diorig.Fahrzeugpreis_Eurotax = 45000;
            _diorig.Restwert_Eurotax = 0;
            _diorig.Restwert_Banknow = 0;
            _diorig.Anz_Abloesen = 0;
            _diorig.Anz_Eigenabloesen = 1;
            _diorig.Anz_Fremdabloesen = 1;
            _diorig.Summe_Abloesen = 19134.7m;
            _diorig.Summe_Eigenabloesen = 19134.7m;
            _diorig.Name_Abloesebank_1 = "BANK-now AG";
            _diorig.VertriebspartnerID = 18468;
            _diorig.Sprache = StrategyOneRequestBodyRecordNRI_VP_SpracheV.frCH;
            _diorig.flagAktiv = 1;
            _diorig.flagEPOS = 1;
            _diorig.A_KundenID = 65385;
            _diorig.A_MO_Counter = 1;
            _diorig.A_Kundenart = StrategyOneRequestBodyRecordI_A_KundenartV.PRIVAT;
            _diorig.PLZ = "1218";
            _diorig.A_Land = "101";
            _diorig.A_Kanton = "GE";
            _diorig.A_Sprache = StrategyOneRequestBodyRecordI_A_SpracheV.frCH;
            _diorig.A_Telefon_privat = "61819";
            _diorig.A_Telefon_geschaeftlich = "79286";
            _diorig.A_Mobiltelefon = "94989";
            _diorig.A_hier_Wohnhaft_seit =  System.DateTime.ParseExact("20040301", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            _diorig.A_A_PID = "ESR";
            _diorig.A_Zivilstand = StrategyOneRequestBodyRecordI_A_ZivilstandV.VERH;
            _diorig.A_Wohnverhaeltnis = StrategyOneRequestBodyRecordI_A_WohnverhaeltnisV.EHE;
            _diorig.A_Anz_Kinder_bis_6 = 0;
            _diorig.A_Anz_Kinder_ueber_6_bis_10 = 1;
            _diorig.A_Nationalitaet = "101";
            _diorig.A_Berufliche_Situation = StrategyOneRequestBodyRecordI_A_Berufliche_SituationV.ANGEST_UNBEFR;
            _diorig.A_Einkommen_Art = StrategyOneRequestBodyRecordI_A_Einkommen_ArtV.NETTO;
            _diorig.A_Haupteinkommen_Betrag = 5008m;
            _diorig.A_Jaehl_Gratifikation_Bonus = 0;
            _diorig.A_13_Montaslohn = 1;
            _diorig.A_Nebeneinkommen_Betrag = 0;
            _diorig.A_Zusatzeinkommen_Betrag = 0;
            _diorig.A_Anz_der_Betreibungen = 0;
            _diorig.A_Wohnkosten_Miete = 930;
            _diorig.A_Bestehende_Kreditrate = 0;
            _diorig.A_Bestehende_Leasingrate = 0;
            _diorig.A_Regelmaessige_Auslagen_Betrag= 0;
            _diorig.A_Berufsauslagen_Betrag = 0;
          //  _diorig.DV_AG_Zeit =  System.DateTime.ParseExact("", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            _diorig.DV_AG_Status= 1;
            _diorig.DV_Anz_DV_Treffer_Adressvalidierung = 1;
            _diorig.DV_Anz_BPM_l12m = 2;
            _diorig.DV_Anz_BPM_l24m = 2;
            _diorig.DV_Anz_BPM_l36m = 3;
            _diorig.DV_Anz_BPM_m_FStat_01_l36m = 0;
            _diorig.ZEK_Datum_der_Auskunft = System.DateTime.ParseExact("20110113", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            
            _diorig.ZEK_Status = 18;
            _diorig.ZEK_Kunde_Gesamtengagement = 20686;
            _diorig.ZEK_Anz_ZEK_Synonyme = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng = 1;
            ///I_ZEK_Anz_ZEK_Eng = 1;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = 1;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Festkredit = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Leasing = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Teilz = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_0506 = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_04 = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M = 0;
            _diorig.ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M = 0;
            _diorig.ZEK_schlechtester_ZEK_Code = 0;
            _diorig.ZEK_Anz_ZEK_Gesuche = 1;
            _diorig.ZEK_Anz_lfd_ZEK_FremdGes = 0;
            
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = 0;
            _diorig.ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = 0;
             /// I_ZEK_Abl_Anz_05_06_08_12 = 0; ????????

            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = 0;
            _diorig.ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = 0;


            //I_ZEK_Abl = 0; ??????
            //I_ZEK_Amtsinfo = 0; ????????
            

            _diorig.OL_Status = 1;
            _diorig.OL_Anz_KundenIDs = 1;
            _diorig.OL_Maximaler_Bandlisteintrag = 0; //??? I_OL_Badlisteintrag
            // I_OL_Akt_Risikoklasse = 13;
            _diorig.OL_Maximale_Risikoklasse_des_Kunden = 18;
            _diorig.OL_Anz_Mehrfachantraege = 0;
            _diorig.OL_Gesamtengagement = 44839;
            _diorig.OL_Engagement = 18974;
            _diorig.OL_Anz_Vertraege = 1;
            _diorig.OL_Anz_lfd_Vertraege = 1; //I_OL_Anz_Vertraege_aktiv ???
            _diorig.OL_Anz_Vertraege_im_Recovery = 0;
            _diorig.OL_Anz_Mahnungen_1 = 2;
            _diorig.OL_Anz_Mahnungen_2 = 1;
            _diorig.OL_Anz_Mahnungen_3 = 5;
            _diorig.OL_Maximale_Mahnstufe = 1;
            _diorig.OL_Summe_OP = 102.75m;
            _diorig.OL_Anz_OP = 1;
            _diorig.OL_Dauer_Kundenbeziehung = 39;
            */

            // Martin neue Testdaten
            _diorig.OrganizationCode = "BNOW";
            _diorig.ProcessCode = "RISK";
            _diorig.ProcessVersion = 24;
            _diorig.InquiryCode = "14496592"; //Wichtig!!!
            _diorig.InquiryDate = System.DateTime.ParseExact("20070508", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            _diorig.InquiryTime = "10:20";
            _diorig.FlagVorpruefung = 1;
            _diorig.FlagBonitaetspruefung = 0;
            _diorig.FlagRisikopruefung = 0;

            _diorig.Geschaeftsart = StrategyOneRequestBodyRecordNRI_C_GeschaeftsartV.FF_V.ToString();
            _diorig.Riskflag = 0;
            _diorig.Finanzierungsbetrag = 117400;
            _diorig.Anzahlung_ErsteRate = 17600;
            _diorig.Vertragsart = 1;
            _diorig.Laufzeit = 49;
            _diorig.Zinssatz = 4.9m;
            _diorig.PPI_Flag_Paket1 = 0;
            _diorig.PPI_Flag_Paket2 = 0;
            _diorig.Rate = 1625.41m;
            _diorig.Restwert = 35508;
            _diorig.KKG_Pflicht = 0;
            _diorig.Nutzungsart = StrategyOneRequestBodyRecordNRI_C_NutzungsartV.PRIVAT.ToString();
            _diorig.Marke = "PORSCHE";
            _diorig.Zustand = 1;
            _diorig.Zubehoerpreis = 0;
            _diorig.KM_Stand = 16000;
            _diorig.KM_prJahr = 15000;
            _diorig.Inverkehrssetzung = System.DateTime.ParseExact("20050323", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            _diorig.Stammnummer = "137.512.440";
            _diorig.Katalogpreis_Eurotax = 159785;
            _diorig.Fahrzeugpreis_Eurotax = 159785;
            _diorig.Restwert_Eurotax = 22990;
            _diorig.Restwert_Banknow = 22990;
            _diorig.Anz_Abloesen = 0;
            _diorig.Anz_Eigenabloesen = 0;
            _diorig.Anz_Fremdabloesen = 0;
            _diorig.Summe_Abloesen = 0;
            _diorig.Summe_Eigenabloesen = 0;
            //_diorig.Name_Abloesebank_1 = "BANK-now AG";
            _diorig.VertriebspartnerID = 83426;
            _diorig.Sprache = StrategyOneRequestBodyRecordNRI_VP_SpracheV.frCH.ToString();
            _diorig.flagAktiv = 1;
            _diorig.flagEPOS = 0;
            _diorig.PLZ = "1180";

            RecordRRDto rrDto = new RecordRRDto();
            rrDto.A_KundenID = 41718;
            rrDto.A_MO_Counter = 1;
            rrDto.A_Kundenart = StrategyOneRequestBodyRecordI_A_KundenartV.PRIVAT.ToString();
            rrDto.A_PLZ = "1022";
            rrDto.A_Geburtsdatum = System.DateTime.ParseExact("19790725", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            rrDto.A_Land = "CH";
            rrDto.A_Kanton = "VD";
            rrDto.A_Sprache = StrategyOneRequestBodyRecordI_A_SpracheV.frCH.ToString();
            rrDto.A_Telefon_privat = "74974";
            rrDto.A_Telefon_geschaeftlich = "57837";
            rrDto.A_Mobiltelefon = "99552";
            //rrDto.A_hier_Wohnhaft_seit =  System.DateTime.ParseExact("20040301", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            rrDto.A_A_PID = "ESR";
            rrDto.A_Zivilstand = StrategyOneRequestBodyRecordI_A_ZivilstandV.LED.ToString();
            rrDto.A_Wohnverhaeltnis = StrategyOneRequestBodyRecordI_A_WohnverhaeltnisV.ALLEIN.ToString();
            rrDto.A_Anz_Kinder_bis_6 = 0;
            rrDto.A_Anz_Kinder_ueber_6_bis_10 = 0;
            rrDto.A_Anz_Kinder_ueber_10_bis_12 = 1;
            rrDto.A_Anz_unterstuetzungsp_Kinder_ab_12 = 0;
            rrDto.A_Nationalitaet = "101";
            rrDto.A_Berufliche_Situation = StrategyOneRequestBodyRecordI_A_Berufliche_SituationV.SELBSTST.ToString();
            rrDto.A_Einkommen_Art = StrategyOneRequestBodyRecordI_A_Einkommen_ArtV.NETTO.ToString();
            rrDto.A_Arbeitgeber_seit_wann = System.DateTime.ParseExact("20060101", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            rrDto.A_Haupteinkommen_Betrag = 12000;
            rrDto.A_Jaehl_Gratifikation_Bonus = 0;
            rrDto.A_13_Montaslohn = 0;
            rrDto.A_Nebeneinkommen_Betrag = 0;
            rrDto.A_Zusatzeinkommen_Betrag = 0;
            rrDto.A_Anz_der_Betreibungen = 0;
            rrDto.A_Wohnkosten_Miete = 3500;
            rrDto.A_Bestehende_Kreditrate = 0;
            rrDto.A_Bestehende_Leasingrate = 0;
            rrDto.A_Regelmaessige_Auslagen_Betrag = 0;
            rrDto.A_Berufsauslagen_Betrag = 0;
            //  rrDto.DV_AG_Zeit =  System.DateTime.ParseExact("", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            //rrDto.DV_AG_Status= 1;
            //rrDto.DV_Anz_DV_Treffer_Adressvalidierung = 1;
            rrDto.DV_Anz_BPM_l12m = 0;
            rrDto.DV_Anz_BPM_l24m = 0;
            rrDto.DV_Anz_BPM_l36m = 0;
            rrDto.DV_Anz_BPM_m_FStat_01_l36m = 0;
            rrDto.DV_Datum_der_Auskunft = System.DateTime.ParseExact("20070508", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            rrDto.DV_Status_Auskunft_Adressvalidierung = 1;
            rrDto.DV_Anz_DV_Treffer_Adressvalidierung = 1;
            rrDto.DV_KundenID = 29977;
            rrDto.DV_AG_Status = 1;
            rrDto.DV_AG_Rechtsform = 2;

            rrDto.ZEK_Status = 12;
            rrDto.ZEK_Datum_der_Auskunft = System.DateTime.ParseExact("20110131", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            //rrDto.ZEK_Kunde_Gesamtengagement = 20686;
            rrDto.ZEK_Anz_ZEK_Synonyme = 1;
            rrDto.ZEK_Anz_ZEK_Vertraege = 1;
            rrDto.ZEK_Anz_lfd_ZEK_Eng = 1;
            rrDto.ZEK_Anz_ZEK_Gesuche = 1;
            rrDto.ZEK_schlechtester_ZEK_Code = 2;
            //I_ZEK_Anz_ZEK_Eng = 1;
            //rrDto.ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = 1;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_Festkredit = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_Leasing = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_Teilz = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_0506 = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04 = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M = 0;
            rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M = 0;
            rrDto.ZEK_schlechtester_ZEK_Code = 0;
            //rrDto.ZEK_Anz_ZEK_Gesuche = 1;
            rrDto.ZEK_Anz_lfd_ZEK_FremdGes = 0;

            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = 0;
            rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = 0;
            // I_ZEK_Abl_Anz_05_06_08_12 = 0; ????????

            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = 0;
            rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = 0;

            rrDto.OL_OpenLease_Datum_der_Anmeldung = System.DateTime.ParseExact("20070508", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            rrDto.OL_Status = 1;
            rrDto.OL_Anz_KundenIDs = 1;
            rrDto.OL_Maximaler_Bandlisteintrag = 0; //??? I_OL_Badlisteintrag
            _diorig.RecordRRDto = new RecordRRDto[]{
                rrDto
            };

            dynamic _di = new NotifyPropertyChangedProxy(_diorig);
            LayerInputDecision.DataContext = _di;
        }


        private void HandlePinningDecision(object sender, RoutedEventArgs e)
        {
            LayerInputDecision.ColumnDefinitions.Add(_extraColLayerInputDecision);
            btnShowOutputControlDecision.Visibility = Visibility.Collapsed;
            pinImageDecision.Source = new BitmapImage(new Uri(@"..\Images\pin.png", UriKind.Relative));
        }

        private void HandleUnpinningDecision(object sender, RoutedEventArgs e)
        {
            LayerInputDecision.ColumnDefinitions.Remove(_extraColLayerInputDecision);
            btnShowOutputControlDecision.Visibility = Visibility.Visible;
            pinImageDecision.Source = new BitmapImage(new Uri(@"..\Images\pin2.png", UriKind.Relative));
        }


        void HandleButtonExpMouseEnterDecision(object sender, RoutedEventArgs e)
        {

            if (LayerOutputDecision.Visibility != Visibility.Visible)
            {
                LayerOutputTransDecision.X = LayerOutputDecision.ColumnDefinitions[1].Width.Value;
                LayerOutputDecision.Visibility = Visibility.Visible;
                DoubleAnimation aniDecision = new DoubleAnimation(0,
                new Duration(TimeSpan.FromMilliseconds(500)));
                LayerOutputTransDecision.BeginAnimation(TranslateTransform.XProperty, aniDecision);
            }
        }

        void HandleLayerInputDecisionMouseEnter(object sender, RoutedEventArgs e)
        {
           
            if (!btnPinItDecision.IsChecked.GetValueOrDefault()
              && LayerOutputDecision.Visibility == Visibility.Visible)
            {
                double to = LayerOutputDecision.ColumnDefinitions[1].Width.Value;
                DoubleAnimation aniDecision = new DoubleAnimation(to,
                new Duration(TimeSpan.FromMilliseconds(500)));
                aniDecision.Completed += new EventHandler(aniDecision_Completed);
                LayerOutputTransDecision.BeginAnimation(TranslateTransform.XProperty, aniDecision);
            }
        }

        void aniDecision_Completed(object sender, EventArgs e)
        {
           LayerOutputDecision.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Handle Button Decision Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonDecisionClick(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DecisionStatus.setIni();
            this.statusblock.DataContext = null;
            this.statusblock.DataContext = DecisionStatus;
            if (OutputObjectDecision.Items.Count > 0)
                OutputObjectDecision.Items.Clear();
            ObjectExplorerDecision.DataContext = null;
            try
            {
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineExecute DecisionEngineExecute = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DecisionEngineExecute();

                    if (textBoxDecision.Text != "")
                    {
                        Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto output;
                        long a = Int64.Parse(textBoxDecision.Text);
                        output = DecisionEngineExecute.doAuskunft(a);
                        this.OutputObjectDecision.Items.Add(DataLoader.LoadTree2(output));
                        this.statusblock.DataContext = null;
                        DecisionStatus.setEnd();
                        this.statusblock.DataContext = DecisionStatus;
                        
                     }
                    else
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto output = DecisionEngineExecute.doAuskunft(_diorig);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        this.OutputObjectDecision.Items.Add(DataLoader.LoadTree2(output));
                        this.statusblock.DataContext = null;
                        DecisionStatus.setEnd();
                        this.statusblock.DataContext = DecisionStatus;
                        

                    }
            }
            catch (Exception ex)
            {
                DecisionStatus.setError();
                this.statusblock.DataContext = null;
                this.statusblock.DataContext = DecisionStatus;
                MessageBox.Show("Exception caught. " + ex);
                this.statusblock.DataContext = null;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        private void ObjectExplorer_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "typeAttribute")
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString() == "nameAttribute")
            {
                e.Column.Header = "Name";
            }

            if (e.Column.Header.ToString() == "valueAttribute")
            {
                e.Column.Header = "Value";
            }
            if (e.PropertyName == "objectAttribute")
            {
                e.Cancel = true;
            }


        }

        /// <summary>
        /// Handle Button Vorprüfung Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonVorpruefungClick(object sender, RoutedEventArgs e)
        {
            _diorig.FlagVorpruefung = 1;
            _diorig.FlagBonitaetspruefung = 0;
            _diorig.FlagRisikopruefung = 0;

            //ObjectInputDecision.DataContext = DataLoader.LoadDTOInput(_di1);
            ObjectInputDecision.Children.Clear();
            Mouse.OverrideCursor = Cursors.Wait;
            GeneratorInput ginput = new GeneratorInput();
            ginput.filter = "Vorpruefung";
            ginput.GenerateObjectInputInit(_diorig, ObjectInputDecision,"");
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Handle Button Bonität Prüfungs Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonBonitaetspruefungClick(object sender, RoutedEventArgs e)
        {

            _diorig.FlagVorpruefung = 0;
            _diorig.FlagBonitaetspruefung = 1;
            _diorig.FlagRisikopruefung = 0;
            // ObjectInputDecision.DataContext = DataLoader.LoadDTOInput(_di2);
            ObjectInputDecision.Children.Clear();
            Mouse.OverrideCursor = Cursors.Wait;
            GeneratorInput ginput = new GeneratorInput();
            ginput.filter = "Bonitaet";
            ginput.GenerateObjectInputInit(_diorig,ObjectInputDecision,"");
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Handle Button Risikoprüfungs Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonRisikopruefungClick(object sender, RoutedEventArgs e)
        {

            // ObjectInputDecision.DataContext = DataLoader.LoadDTOInput(_di);
            //  GenerationInput(_diorig);
            ObjectInputDecision.Children.Clear();
            Mouse.OverrideCursor = Cursors.Wait;
            GeneratorInput ginput = new GeneratorInput();
            ginput.filter = "Risiko";
            ginput.GenerateObjectInputInit(_diorig, ObjectInputDecision,"");
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ObjectDecisionItemSelected_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedTVI = (TreeViewItem)OutputObjectDecision.SelectedItem;
            if (selectedTVI != null)
            {
                ObjectExplorerDecision.DataContext = DataLoader.LoadDTO(selectedTVI.Tag);
                DecisionOutputExplorerTitle.Text = selectedTVI.Header.ToString();

            }
            /*   else
               {
                   MessageBox.Show("Keine Attribute ausgewählt!");
               }*/


        }

    }
}
