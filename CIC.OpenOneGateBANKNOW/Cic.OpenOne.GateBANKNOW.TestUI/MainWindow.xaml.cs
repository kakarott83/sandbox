using System;

using System.Windows;
using Cic.OpenOne.GateBANKNOW.TestUI.UserControls;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using System.Collections.Generic;

using AutoMapper;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.TestUI
{
    using Common.BO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            versionblock.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BankNowModelProfile>();
            });

            //EUROTAX
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetForecast eurotaxGetForecast = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetForecast();
            Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto _eiorig = new Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto();
            _eiorig.ISOCountryCode = ISOcountryType.CH;
            _eiorig.ISOCurrencyCode = ISOcurrencyType.CHF;
            _eiorig.ISOLanguageCode = ISOlanguageType.DE;
            _eiorig.CurrentMileageValue = 20000;
            _eiorig.EstimatedAnnualMileageValue = 20000;
            _eiorig.ForecastPeriodFrom = "12";
            _eiorig.ForecastPeriodUntil = "12";
            _eiorig.NationalVehicleCode = 102176315;
            _eiorig.RegistrationDate = new DateTime(2010, 12, 12);
            //    _eiorig.Version = "Item101";
            NewAuskunft eurotaxGF = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Eurotax", _eiorig, eurotaxGetForecast, typeof(EurotaxInDto), "doAuskunft", "GetForecast");


            var tabItemEurotaxGF = new System.Windows.Controls.TabItem();
            tabItemEurotaxGF.Header = "EurotaxGetForecast";
            tabItemEurotaxGF.TabIndex = 1;
            tabItemEurotaxGF.Content = eurotaxGF;
            tabcontrolEurotax.Items.Add(tabItemEurotaxGF);


            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetValuation eurotaxGetValuation = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetValuation();
            Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto _eurotaxGetValuation = new Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto();
            _eurotaxGetValuation.ISOCountryCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType.CH;
            _eurotaxGetValuation.ISOCurrencyCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType.CHF;
            _eurotaxGetValuation.ISOLanguageCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType.DE;
            _eurotaxGetValuation.NationalVehicleCode = 102176315;
            _eurotaxGetValuation.RegistrationDate = new DateTime(2010, 12, 12);
            // _eurotaxGetValuation.Version = "Item112";
            NewAuskunft eurotaxV = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Eurotax", _eurotaxGetValuation, eurotaxGetValuation, typeof(EurotaxInDto), "doAuskunft", "GetValuation");

            var tabItemEurotaxV = new System.Windows.Controls.TabItem();
            tabItemEurotaxV.Header = "EurotaxGetValuation";
            tabItemEurotaxV.TabIndex = 2;
            tabItemEurotaxV.Content = eurotaxV;
            tabcontrolEurotax.Items.Add(tabItemEurotaxV);


            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetRemo eurotaxGetRemo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetRemo();
            Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto _eurotaxGetRemo = new Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto();
            _eurotaxGetRemo.ISOCountryCode = ISOcountryType.CH;
            _eurotaxGetRemo.ISOCurrencyCode = ISOcurrencyType.CHF;
            _eurotaxGetRemo.ISOLanguageCode = ISOlanguageType.DE;
            _eurotaxGetRemo.CurrentMileageValue = 20000;
            _eurotaxGetRemo.EstimatedAnnualMileageValue = 20000;
            _eurotaxGetRemo.ForecastPeriodFrom = "12";
            _eurotaxGetRemo.ForecastPeriodUntil = "12";
            _eurotaxGetRemo.NationalVehicleCode = 102176315;
            _eurotaxGetRemo.RegistrationDate = new DateTime(2010, 12, 12);
            // _eurotaxGetValuation.Version = "Item112";
            NewAuskunft eurotaxRemo = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Eurotax", _eurotaxGetRemo, eurotaxGetRemo, typeof(EurotaxInDto), "doAuskunft", "GetForecast");

            var tabItemEurotaxRemo = new System.Windows.Controls.TabItem();
            tabItemEurotaxRemo.Header = "EurotaxGetREMO";
            tabItemEurotaxRemo.TabIndex = 2;
            tabItemEurotaxRemo.Content = eurotaxRemo;
            tabcontrolEurotax.Items.Add(tabItemEurotaxRemo);

            //DELTAVISTA
            DeltavistaInDto _deltavistaInDto = new DeltavistaInDto()
            {
                AddressDescription = new DVAddressDescriptionDto()
                {
                    Birthdate = "22.12.1989",
                    City = "Hamburg",
                    Country = "Germany",
                    Fax = "1223456",
                    FirstName = "Klaus",
                    Housenumber = "100a",
                    LegalForm = 2,
                    MaidenName = "MaidenName1",
                    Mobile = "01701233456",
                    Name = "Lustig",
                    Sex = 1,
                    Street = "Fiktivstraße 3",
                    Telephone = "0401234567",
                    Zip = "21031"
                },
                OrderDescription = new DVOrderDescriptionDto()
                {
                    BAProductId = 0,
                    EWKProductId = 0,
                    cresuraReportId = 0
                },
                AddressId = 100
            };

            //Deltavista AddressIdentification

            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVAddressIdentification dVAddressIdentification = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVAddressIdentification();

            NewAuskunft deltavistaAddressIdentification = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dVAddressIdentification, typeof(DeltavistaInDto), "doAuskunft", "DVAddressIdentification");

            var tabItemDeltavistaAI = new System.Windows.Controls.TabItem();
            tabItemDeltavistaAI.Header = "DeltavistaAddressIdentification";
            tabItemDeltavistaAI.TabIndex = 2;
            tabItemDeltavistaAI.Content = deltavistaAddressIdentification;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaAI);


            //Deltavista AddressIdentification

            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVAddressIdentificationArb dVAddressIdentificationArb = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVAddressIdentificationArb();

            NewAuskunft deltavistaAddressIdentificationArb = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dVAddressIdentificationArb, typeof(DeltavistaInDto), "doAuskunft", "DVAddressIdentification");

            var tabItemDeltavistaAIArb = new System.Windows.Controls.TabItem();
            tabItemDeltavistaAIArb.Header = "DeltavistaAddressIdentificationArb";
            tabItemDeltavistaAIArb.TabIndex = 2;
            tabItemDeltavistaAIArb.Content = deltavistaAddressIdentificationArb;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaAIArb);

            //Deltavista CompanyDetails
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetCompanyDetails dVgetCompanyDetails = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetCompanyDetails();

            NewAuskunft deltavistaCompanyDetails = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dVgetCompanyDetails, typeof(DeltavistaInDto), "doAuskunft", "DVgetCompanyDetails");

            var tabItemDeltavistaCD = new System.Windows.Controls.TabItem();
            tabItemDeltavistaCD.Header = "DeltavistaCompanyDetails";
            tabItemDeltavistaCD.TabIndex = 2;
            tabItemDeltavistaCD.Content = deltavistaCompanyDetails;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaCD);

            //Deltavista GetDebtDetails
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetDebtDetails dVgetDebtDetails = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetDebtDetails();

            NewAuskunft deltavistagetDebtDetails = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dVgetDebtDetails, typeof(DeltavistaInDto), "doAuskunft", "DVgetDebtDetails");

            var tabItemDeltavistaDD = new System.Windows.Controls.TabItem();
            tabItemDeltavistaDD.Header = "DeltavistaGetDebtDetails";
            tabItemDeltavistaDD.TabIndex = 2;
            tabItemDeltavistaDD.Content = deltavistagetDebtDetails;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaDD);

            //Deltavista orderCresuraReport
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVorderCresuraReport dVorderCresura = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVorderCresuraReport();

            NewAuskunft deltavistaorderCresura = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dVorderCresura, typeof(DeltavistaInDto), "doAuskunft", "DVorderCresuraReport");

            var tabItemDeltavistaOC = new System.Windows.Controls.TabItem();
            tabItemDeltavistaOC.Header = "DeltavistaOrderCresuraReport";
            tabItemDeltavistaOC.TabIndex = 2;
            tabItemDeltavistaOC.Content = deltavistaorderCresura;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaOC);

            //Deltavista getReport
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetReport dvGetReport = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.DVgetReport();

            NewAuskunft deltavistagetReport = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Deltavista", null, dvGetReport, typeof(DeltavistaInDto), "doAuskunft", "DVgetReport");

            var tabItemDeltavistaGR = new System.Windows.Controls.TabItem();
            tabItemDeltavistaGR.Header = "DeltavistaGetReport";
            tabItemDeltavistaGR.TabIndex = 2;
            tabItemDeltavistaGR.Content = deltavistagetReport;
            tabcontrolDeltavista.Items.Add(tabItemDeltavistaGR);


            //ZEK ZekInformativabfrage
            Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto _zekInDto = new Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekInDto()
            {
                Ablehnungsgrund = 1,
                Anfragegrund = 1,
                DatumAblehnung = "2011-01-01",
                KreditgesuchID = "10",
                PreviousKreditgesuchID = "",
                RequestEntities = new List<ZekRequestEntityDto>
                {
                    new ZekRequestEntityDto()
                    {
                        AddressDescription = new ZekAddressDescriptionDto()
                        {
                            Birthdate = "1989-12-22",
                            City = "Hamburg",
                            Country = "Germany",
                            DatumWohnhaftSeit = "1989-12-22",
                            FirmaZusatz = null,
                            FirstName = "Sydney",
                            FoundingDate = null,
                            Housenumber = "1a",
                            KundenId = "1337",
                            LegalForm = 1,
                            MaidenName = null,
                            Name = "Döffert",
                            Nationality = "German",
                            NogaCode = "02",
                            Profession = "Förster",
                            Sex = 1,
                            Zip = "21031",
                            ZipAdd = null,
                            Zivilstandscode = 1 
                        },
                        DebtorRole = 0,
                        ForceNewAddress = 0,
                        PreviousReturnCode = null,
                        RefNo = 1337
                    },
                     new ZekRequestEntityDto()
                    {
                        AddressDescription = new ZekAddressDescriptionDto()
                        {
                            Birthdate = "1999-12-22",
                            City = "Freilassing,",
                            Country = "Germany",
                            DatumWohnhaftSeit = "1999-12-22",
                            FirmaZusatz = null,
                            FirstName = "Sydney",
                            FoundingDate = null,
                            Housenumber = "1a",
                            KundenId = "1338",
                            LegalForm = 1,
                            MaidenName = null,
                            Name = "Döffert",
                            Nationality = "German",
                            NogaCode = "02",
                            Profession = "Förster",
                            Sex = 1,
                            Zip = "21031",
                            ZipAdd = null,
                            Zivilstandscode = 1 
                        },
                        DebtorRole = 1,
                        ForceNewAddress = 0,
                        PreviousReturnCode = null,
                        RefNo = 1338
                    }
                },
                RequestEntity = new ZekRequestEntityDto(),
                Zielverein = 1
            };

            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekInformativabfrage zekInformativabfrage = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekInformativabfrage();

            NewAuskunft ZekAuskunftInformativabfrage = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekInformativabfrage, typeof(ZekInDto), "doAuskunft", "zekInformativabfrage");

            var tabItemZekIA = new System.Windows.Controls.TabItem();
            tabItemZekIA.Header = "ZekInformativabfrage";
            tabItemZekIA.TabIndex = 2;
            tabItemZekIA.Content = ZekAuskunftInformativabfrage;
            tabcontrolZEK.Items.Add(tabItemZekIA);

            //ZEK ZekKreditgesuchAblehnen
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekKreditgesuchAblehnen zekKreditgesuchAblehnen = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekKreditgesuchAblehnen();

            NewAuskunft zekAuskunftKreditgesuchAblehnen = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekKreditgesuchAblehnen, typeof(ZekInDto), "doAuskunft", "zekKreditgesuchAblehnen");

            var tabItemZekKA = new System.Windows.Controls.TabItem();
            tabItemZekKA.Header = "ZekKreditgesuchAblehnen";
            tabItemZekKA.TabIndex = 2;
            tabItemZekKA.Content = zekAuskunftKreditgesuchAblehnen;
            tabcontrolZEK.Items.Add(tabItemZekKA);

            //ZEK ZekKreditgesuchNeu
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekKreditgesuchNeu zekKreditgesuchNeu = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekKreditgesuchNeu();

            NewAuskunft zekAuskunftKreditgesuchNeu = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekKreditgesuchNeu, typeof(ZekInDto), "doAuskunft", "zekKreditgesuchNeu");

            var tabItemZekKN = new System.Windows.Controls.TabItem();
            tabItemZekKN.Header = "ZekKreditgesuchNeu";
            tabItemZekKN.TabIndex = 2;
            tabItemZekKN.Content = zekAuskunftKreditgesuchNeu;
            tabcontrolZEK.Items.Add(tabItemZekKN);

            //ZEK ZekRegisterBardarlehen
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterBardarlehen zekRegisterBardarlehen = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterBardarlehen();

            NewAuskunft zekAuskunftRegisterBardarlehen = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekRegisterBardarlehen, typeof(ZekInDto), "doAuskunft", "zekRegisterBardarlehen");

            var tabItemZekRB = new System.Windows.Controls.TabItem();
            tabItemZekRB.Header = "ZekRegisterBardarlehen";
            tabItemZekRB.TabIndex = 2;
            tabItemZekRB.Content = zekAuskunftRegisterBardarlehen;
            tabcontrolZEKEC4.Items.Add(tabItemZekRB);

            //ZEK ZekRegisterFestkredit
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterFestkredit zekRegisterFestkredit = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterFestkredit();

            NewAuskunft zekAuskunftRegisterFestkredit = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekRegisterFestkredit, typeof(ZekInDto), "doAuskunft", "zekRegisterFestkredit");

            var tabItemZekRF = new System.Windows.Controls.TabItem();
            tabItemZekRF.Header = "ZekRegisterFestkredit";
            tabItemZekRF.TabIndex = 2;
            tabItemZekRF.Content = zekAuskunftRegisterFestkredit;
            tabcontrolZEKEC4.Items.Add(tabItemZekRF);

            //ZEK ZekRegisterKontokorrent
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterKontokorrentkredit zekRegisterKontokorrent = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterKontokorrentkredit();

            NewAuskunft zekAuskunftRegisterKK = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekRegisterKontokorrent, typeof(ZekInDto), "doAuskunft", "zekRegisterKontokorrent");

            var tabItemZekRK = new System.Windows.Controls.TabItem();
            tabItemZekRK.Header = "ZekRegisterKontokorrentkredit";
            tabItemZekRK.TabIndex = 2;
            tabItemZekRK.Content = zekAuskunftRegisterKK;
            tabcontrolZEKEC4.Items.Add(tabItemZekRK);

            //ZEK ZekRegisterLeasingMiet
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterLeasingMietvertrag zekRegisterLM = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterLeasingMietvertrag();

            NewAuskunft zekAuskunftRegisterLM = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekRegisterLM, typeof(ZekInDto), "doAuskunft", "zekRegisterLM");

            var tabItemZekLM = new System.Windows.Controls.TabItem();
            tabItemZekLM.Header = "ZekRegisterLeasingMietvertrag";
            tabItemZekLM.TabIndex = 2;
            tabItemZekLM.Content = zekAuskunftRegisterLM;
            tabcontrolZEKEC4.Items.Add(tabItemZekLM);

            //ZEK ZekRegisterTeilzahlung
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterTeilzahlungskredit zekRegisterTZ = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekRegisterTeilzahlungskredit();

            NewAuskunft zekAuskunftRegisterTZ = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekRegisterTZ, typeof(ZekInDto), "doAuskunft", "zekRegisterTZ");

            var tabItemZekTZ = new System.Windows.Controls.TabItem();
            tabItemZekTZ.Header = "ZekRegisterTeilzahlung";
            tabItemZekTZ.TabIndex = 2;
            tabItemZekTZ.Content = zekAuskunftRegisterTZ;
            tabcontrolZEKEC4.Items.Add(tabItemZekTZ);

            //ZEK zekMeldungKarten
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekMeldungKartenengagement zekMeldungKarten = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekMeldungKartenengagement();

            NewAuskunft zekAuskunftMeldungKarten = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekMeldungKarten, typeof(ZekInDto), "doAuskunft", "zekMeldungKarten");

            var tabItemZekMK = new System.Windows.Controls.TabItem();
            tabItemZekMK.Header = "ZekMeldungKartenengagement";
            tabItemZekMK.TabIndex = 2;
            tabItemZekMK.Content = zekAuskunftMeldungKarten;
            tabcontrolZEKEC4.Items.Add(tabItemZekMK);

            //ZEK meldungUeberziehungskredit
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekMeldungUeberziehungskredit zekmMeldungUeberziehungskredit = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.ZekMeldungUeberziehungskredit();

            NewAuskunft zekAuskunftMeldungUeber = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekmMeldungUeberziehungskredit, typeof(ZekInDto), "doAuskunft", "zekmMeldungUeberziehungskredit");

            var tabItemZekMU = new System.Windows.Controls.TabItem();
            tabItemZekMU.Header = "ZekMeldungUeberziehungskredit";
            tabItemZekMU.TabIndex = 2;
            tabItemZekMU.Content = zekAuskunftMeldungUeber;
            tabcontrolZEKEC4.Items.Add(tabItemZekMU);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateAd = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterUA = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateAd, typeof(ZekInDto), "updateAddress", "zekUpdateAd");

            var tabItemZekUA = new System.Windows.Controls.TabItem();
            tabItemZekUA.Header = "ZekUpdateAddress";
            tabItemZekUA.TabIndex = 2;
            tabItemZekUA.Content = zekAuskunftRegisterUA;
            tabcontrolZEKEC6.Items.Add(tabItemZekUA);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateBd = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterUBD = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateBd, typeof(ZekInDto), "updateBardarlehen", "zekUpdateBd");

            var tabItemZekUBD = new System.Windows.Controls.TabItem();
            tabItemZekUBD.Header = "ZekUpdateBardarlehen";
            tabItemZekUBD.TabIndex = 2;
            tabItemZekUBD.Content = zekAuskunftRegisterUBD;
            tabcontrolZEKEC7.Items.Add(tabItemZekUBD);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateFk = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterUFK = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateFk, typeof(ZekInDto), "updateFestkredit", "zekUpdateFk");

            var tabItemZekUFK = new System.Windows.Controls.TabItem();
            tabItemZekUFK.Header = "ZekUpdateFestkredit";
            tabItemZekUFK.TabIndex = 2;
            tabItemZekUFK.Content = zekAuskunftRegisterUFK;
            tabcontrolZEKEC7.Items.Add(tabItemZekUFK);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateKK = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterUKK = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateKK, typeof(ZekInDto), "updateKontokorrentkredit", "zekUpdateKK");

            var tabItemZekUKK = new System.Windows.Controls.TabItem();
            tabItemZekUKK.Header = "ZekUpdateKontokorrentkredit";
            tabItemZekUKK.TabIndex = 2;
            tabItemZekUKK.Content = zekAuskunftRegisterUKK;
            tabcontrolZEKEC7.Items.Add(tabItemZekUKK);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateLM = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterULM = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateLM, typeof(ZekInDto), "updateLeasingMietvertrag", "zekUpdateLM");

            var tabItemZekULM = new System.Windows.Controls.TabItem();
            tabItemZekULM.Header = "ZekUpdateLeasingMietvertrag";
            tabItemZekULM.TabIndex = 2;
            tabItemZekULM.Content = zekAuskunftRegisterULM;
            tabcontrolZEKEC7.Items.Add(tabItemZekULM);

            // 
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateTz = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftRegisterUTK = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, zekUpdateTz, typeof(ZekInDto), "updateTeilzahlungskredit", "zekUpdateTz");

            var tabItemZekUTK = new System.Windows.Controls.TabItem();
            tabItemZekUTK.Header = "ZekUpdateTeilzahlungskredit";
            tabItemZekUTK.TabIndex = 2;
            tabItemZekUTK.Content = zekAuskunftRegisterUTK;
            tabcontrolZEKEC7.Items.Add(tabItemZekUTK);

            // zekUpdateContractsBatch
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekUpdateContractsBatch = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftUpdateContractsBatch = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekUpdateContractsBatch,
                                                                typeof(ZekInDto), "updateContractsBatch", "zekUpdateContractsBatch");

            var tabItemZekUpdateContractsBatch = new System.Windows.Controls.TabItem();
            tabItemZekUpdateContractsBatch.Header = "zekUpdateContractsBatch";
            tabItemZekUpdateContractsBatch.TabIndex = 2;
            tabItemZekUpdateContractsBatch.Content = zekAuskunftUpdateContractsBatch;
            tabcontrolZEKEC7.Items.Add(tabItemZekUpdateContractsBatch);


            // Close-Methods (Vertragsabmeldung EC5)
            // ZekCloseBardarlehen
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseBardarlehen = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseBardarlehen = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseBardarlehen, typeof(ZekInDto), "closeBardarlehen", "zekCloseBardarlehen");

            var tabItemZekCloseBardarlehen = new System.Windows.Controls.TabItem();
            tabItemZekCloseBardarlehen.Header = "ZekCloseBardarlehen";
            tabItemZekCloseBardarlehen.TabIndex = 2;
            tabItemZekCloseBardarlehen.Content = zekAuskunftCloseBardarlehen;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseBardarlehen);

            // ZEKCloseLeasingMietvertrag
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseLeasingMietvertrag = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseLeasingMietvertrag = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseLeasingMietvertrag, typeof(ZekInDto), "closeLeasingMietvertrag", "zekCloseLeasingMietvertrag");

            var tabItemZekCloseLeasingMietvertrag = new System.Windows.Controls.TabItem();
            tabItemZekCloseLeasingMietvertrag.Header = "ZekCloseLeasingMietvertrag";
            tabItemZekCloseLeasingMietvertrag.TabIndex = 2;
            tabItemZekCloseLeasingMietvertrag.Content = zekAuskunftCloseLeasingMietvertrag;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseLeasingMietvertrag);

            // ZEKCloseFestkredit
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseFestkredit = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseFestkredit = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseFestkredit, typeof(ZekInDto), "closeFestkredit", "zekCloseFestkredit");

            var tabItemZekCloseFestkredit = new System.Windows.Controls.TabItem();
            tabItemZekCloseFestkredit.Header = "ZekCloseFestkredit";
            tabItemZekCloseFestkredit.TabIndex = 2;
            tabItemZekCloseFestkredit.Content = zekAuskunftCloseFestkredit;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseFestkredit);

            // ZEKCloseTeilzahlungskredit
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseTeilzahlungskredit = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseTeilzahlungskredit = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseTeilzahlungskredit, typeof(ZekInDto), "closeTeilzahlungskredit", "zekCloseTeilzahlungskredit");

            var tabItemZekCloseTeilzahlungskredit = new System.Windows.Controls.TabItem();
            tabItemZekCloseTeilzahlungskredit.Header = "ZekCloseTeilzahlungskredit";
            tabItemZekCloseTeilzahlungskredit.TabIndex = 2;
            tabItemZekCloseTeilzahlungskredit.Content = zekAuskunftCloseTeilzahlungskredit;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseTeilzahlungskredit);

            // ZEKCloseKontokorrentkredit
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseKontokorrentkredit = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseKontokorrentkredit = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseKontokorrentkredit, typeof(ZekInDto), "closeKontokorrentkredit", "zekCloseKontokorrentkredit");

            var tabItemZekCloseKontokorrentkredit = new System.Windows.Controls.TabItem();
            tabItemZekCloseKontokorrentkredit.Header = "ZekCloseKontokorrentkredit";
            tabItemZekCloseKontokorrentkredit.TabIndex = 2;
            tabItemZekCloseKontokorrentkredit.Content = zekAuskunftCloseKontokorrentkredit;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseKontokorrentkredit);


            // zekCloseContractsBatch
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekCloseContractsBatch = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftCloseContractsBatch = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekCloseContractsBatch,
                                                                typeof(ZekInDto), "closeContractsBatch", "zekCloseContractsBatch");

            var tabItemZekCloseContractsBatch = new System.Windows.Controls.TabItem();
            tabItemZekCloseContractsBatch.Header = "zekCloseContractsBatch";
            tabItemZekCloseContractsBatch.TabIndex = 2;
            tabItemZekCloseContractsBatch.Content = zekAuskunftCloseContractsBatch;
            tabcontrolZEKEC5.Items.Add(tabItemZekCloseContractsBatch);



            // zekeCode178Anmelden
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekeCode17Anmeldenbo = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekeCode178Anmelden = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekeCode17Anmeldenbo,
                                                                typeof(ZekInDto), "eCode178Anmelden", "zekeCode178Anmelden");

            var tabItemZekeCode178Anmelden = new System.Windows.Controls.TabItem();
            tabItemZekeCode178Anmelden.Header = "eCode178Anmelden";
            tabItemZekeCode178Anmelden.TabIndex = 2;
            tabItemZekeCode178Anmelden.Content = zekeCode178Anmelden;
            tabcontrolECode178.Items.Add(tabItemZekeCode178Anmelden);


              // zekeCode178Abmelden
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekeCode178Abmeldenbo = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekeCode178Abmelden = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekeCode178Abmeldenbo,
                                                                typeof(ZekInDto), "eCode178Abmelden", "zekeCode178Abmelden");

            var tabItemZekeCode178Abmelden = new System.Windows.Controls.TabItem();
            tabItemZekeCode178Abmelden.Header = "eCode178Abmelden";
            tabItemZekeCode178Abmelden.TabIndex = 2;
            tabItemZekeCode178Abmelden.Content = zekeCode178Abmelden;
            tabcontrolECode178.Items.Add(tabItemZekeCode178Abmelden);

            // zekeCode178Abmelden
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekeCode178Abfragebo = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekeCode178Abfrage = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekeCode178Abfragebo,
                                                                typeof(ZekInDto), "eCode178Abfrage", "zekeCode178Abfrage");

            var tabItemZekeCode178Abfrage = new System.Windows.Controls.TabItem();
            tabItemZekeCode178Abfrage.Header = "eCode178Abfrage";
            tabItemZekeCode178Abfrage.TabIndex = 2;
            tabItemZekeCode178Abfrage.Content = zekeCode178Abfrage;
            tabcontrolECode178.Items.Add(tabItemZekeCode178Abfrage);

            // zekeCode178Mutieren
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekeCode178Mutierenbo = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekeCode178Mutieren = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekeCode178Mutierenbo,
                                                                typeof(ZekInDto), "eCode178Mutieren", "zekeCode178Mutieren");

            var tabItemZekeCode178Mutieren = new System.Windows.Controls.TabItem();
            tabItemZekeCode178Mutieren.Header = "eCode178Mutieren";
            tabItemZekeCode178Mutieren.TabIndex = 2;
            tabItemZekeCode178Mutieren.Content = zekeCode178Mutieren;
            tabcontrolECode178.Items.Add(tabItemZekeCode178Mutieren);


            // zekgetARMs
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo zekGetARMsbo = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekGetARMs = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", _zekInDto, zekeCode178Mutierenbo,
                                                                typeof(ZekInDto), "getARMs", "zekGetARMs");

            var tabItemZekGetARMs = new System.Windows.Controls.TabItem();
            tabItemZekGetARMs.Header = "GetARMs";
            tabItemZekGetARMs.TabIndex = 2;
            tabItemZekGetARMs.Content = zekGetARMs;
            tabcontrolZekGetARMs.Items.Add(tabItemZekGetARMs);

            // NOTIFICATION

            INotificationGatewayBo _GatewayMail = BOFactory.getInstance().createNotificationGateway();

            NewAuskunft gatewayMail = new NewAuskunft("Notification", null, _GatewayMail, typeof(inotificationMailDto), "sendMail", "");

            var tabItemNotification = new System.Windows.Controls.TabItem();
            tabItemNotification.Header = "Mail";
            tabItemNotification.TabIndex = 1;
            tabItemNotification.Content = gatewayMail;
            notification.Items.Add(tabItemNotification);

            INotificationGatewayBo _GatewayFax = BOFactory.getInstance().createNotificationGateway();

            NewAuskunft gatewayFax = new NewAuskunft("Notification", null, _GatewayFax, typeof(inotificationFaxDto), "sendFax", "");

            var tabItemNotificationFax = new System.Windows.Controls.TabItem();
            tabItemNotificationFax.Header = "Fax";
            tabItemNotificationFax.TabIndex = 1;
            tabItemNotificationFax.Content = gatewayFax;
            notification.Items.Add(tabItemNotificationFax);

            INotificationGatewayBo _GatewaySms = BOFactory.getInstance().createNotificationGateway();

            NewAuskunft gatewaySms = new NewAuskunft("Notification", null, _GatewaySms, typeof(inotificationSmsDto), "sendSms", "");

            var tabItemNotificationSms = new System.Windows.Controls.TabItem();
            tabItemNotificationSms.Header = "Sms";
            tabItemNotificationSms.TabIndex = 1;
            tabItemNotificationSms.Content = gatewaySms;
            notification.Items.Add(tabItemNotificationSms);

            // Aggregation
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.AggregationCallByValues aggregation = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.AggregationCallByValues();

            NewAuskunft aggrAuskunft = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("Aggregation", null, aggregation, typeof(AggregationInDto), "doAuskunft", "");

            var tabItemAggregation = new System.Windows.Controls.TabItem();
            tabItemAggregation.Header = "Aggregation";
            tabItemAggregation.TabIndex = 1;
            tabItemAggregation.Content = aggrAuskunft;
            tabcontrolAggregation.Items.Add(tabItemAggregation);



            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.IZekBo informativeabfrageOL = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.AuskunftBoFactory.CreateDefaultZekBo();

            NewAuskunft zekAuskunftInformativeabfrageOL = new Cic.OpenOne.GateBANKNOW.TestUI.UserControls.NewAuskunft("ZEK", null, informativeabfrageOL, typeof(AuskunftOLDto), "informativabfrageOL", "");

            var tabItemZekInformativeabfrageOL = new System.Windows.Controls.TabItem();
            tabItemZekInformativeabfrageOL.Header = "InformativeabfrageOL";
            tabItemZekInformativeabfrageOL.TabIndex = 1;
            tabItemZekInformativeabfrageOL.Content = zekAuskunftInformativeabfrageOL;
            tabcontrolZekInformativeabfrageOL.Items.Add(tabItemZekInformativeabfrageOL);
            

            /*         
            
            // Search
            //  Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<AntragDto> AntragBO = new Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<AntragDto>();
            Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<VertragDto> AntragBO = new Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<VertragDto>();
         
            //  NewAuskunft testAuskunftsearch = new NewAuskunft("Search", null, AntragBO, typeof(iSearchDto), "search", "");
            NewAuskunft testAuskunftsearch = new NewAuskunft("Search", null, AntragBO, typeof(iSearchDto), "search", "");

            var tabItemSearch = new System.Windows.Controls.TabItem();
            tabItemSearch.Header = "Search";
            tabItemSearch.TabIndex = 1;
            tabItemSearch.Content = testAuskunftsearch;
            tabcontrol.Items.Add(tabItemSearch);

            //Search
            iSearchDto s = new iSearchDto();
            Filter f = new Filter();
            f.fieldname = "Auskunft";
            f.value = "2";
            f.filterType = FilterType.Equal;
            s.filters= new Filter[]{f};
            Filter f2 = new Filter();
            f2.fieldname = "Auskunft";
            f2.value = "2";
            f2.filterType = FilterType.Equal;
            s.filters = new Filter[] { f2 };

            Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntDto> BOs = new Cic.OpenOne.GateBANKNOW.Common.BO.SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntDto>();
            BOs.setPermission(2345, true, "SYSUSER");

            NewAuskunft testAuskunftsearch2 = new NewAuskunft("Search", s, BOs, typeof(iSearchDto), "search", "");

            var tabItemSearch2 = new System.Windows.Controls.TabItem();
            tabItemSearch2.Header = "Search2";
            tabItemSearch2.TabIndex = 1;
            tabItemSearch2.Content = testAuskunftsearch2;
            tabcontrol.Items.Add(tabItemSearch2);
    
            // Testdto

            filterGoogle testBO = new filterGoogle();

            NewAuskunft testAuskunft = new NewAuskunft("TEST", null, testBO, typeof(TestDto), "getQuery", "");

            var tabItemtestAuskunft = new System.Windows.Controls.TabItem();
            tabItemtestAuskunft.Header = "Search";
            tabItemtestAuskunft.TabIndex = 1;
            tabItemtestAuskunft.Content = testAuskunft;
            tabcontrol.Items.Add(tabItemtestAuskunft);

              
             
            TestBO testBO = new TestBO();

            NewAuskunft testAuskunft = new NewAuskunft("TEST", null, testBO, typeof(TestDto), "doAuskunft", "");

            var tabItemtestAuskunft = new System.Windows.Controls.TabItem();
            tabItemtestAuskunft.Header = "testBO";
            tabItemtestAuskunft.Content = testAuskunft;
            tabcontrol.Items.Add(tabItemtestAuskunft);

            NewAuskunft testBOinput = new NewAuskunft("TEST", null, testBO, typeof(TestDto), "doInput", "");

            var tabItemtestBOinput = new System.Windows.Controls.TabItem();
            tabItemtestBOinput.Header = "doInput";
            tabItemtestBOinput.Content = testBOinput;
            tabcontrol.Items.Add(tabItemtestBOinput);

      
            EaihotDao eaihot = new EaihotDao();
            Cic.OpenOne.GateBANKNOW.Common.BO.BuchwertBo BO = new Cic.OpenOne.GateBANKNOW.Common.BO.BuchwertBo(eaihot);

            NewAuskunft testBUCHWERTBOinput = new NewAuskunft("TEST", null, BO, typeof(igetBuchwertDto), "getBuchwert", "");

            var tabItemtestBOinput = new System.Windows.Controls.TabItem();
            tabItemtestBOinput.Header = "getBuchwert";
            tabItemtestBOinput.Content = testBUCHWERTBOinput;
            tabcontrol.Items.Add(tabItemtestBOinput);
            */

            /*
            Cic.OpenOne.GateBANKNOW.Common.BO.ITransactionRisikoBo BO = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createTransactionRisikoBO();


            NewAuskunft testTransactionRisikoBOinput = new NewAuskunft("TEST", null, BO, typeof(icheckTrRiskByIdDto), "checkTrRiskBySysid", "");

           


            var tabItemtestBOinput = new System.Windows.Controls.TabItem();
            tabItemtestBOinput.Header = "TransactionRisiko";
            tabItemtestBOinput.TabIndex = 1;
            tabItemtestBOinput.Content = testTransactionRisikoBOinput;
            tabcontrolTransactionRisiko.Items.Add(tabItemtestBOinput);


            */


            Cic.OpenOne.GateBANKNOW.Common.BO.TransactionRisikoGUIBo BO = new Cic.OpenOne.GateBANKNOW.Common.BO.TransactionRisikoGUIBo();
            NewAuskunft testTransactionRisikoBOinput = new NewAuskunft("TEST", null, BO, typeof(icheckTrRiskByIdDto), "checkantragbyIdGUI", "");
            var tabItemtestBOinput = new System.Windows.Controls.TabItem();
            tabItemtestBOinput.Header = "checkantragbyIdGUI";
            tabItemtestBOinput.TabIndex = 1;
            tabItemtestBOinput.Content = testTransactionRisikoBOinput;
            tabcontrolTransactionRisiko.Items.Add(tabItemtestBOinput);

            
            
            Cic.OpenOne.GateBANKNOW.Common.BO.IKundenRisikoBo KRBO = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createKundenRisikoBO("de-CH");
            NewAuskunft testKundenRisikoBOinput = new NewAuskunft("TEST", null, KRBO, typeof(icheckKrRiskByIdDto), "checkKrRiskById", "");
            var tabItemtestKRBOinput = new System.Windows.Controls.TabItem();
            tabItemtestKRBOinput.Header = "checkKrRiskById";
            tabItemtestKRBOinput.TabIndex = 1;
            tabItemtestKRBOinput.Content = testKundenRisikoBOinput;
            tabcontrolTransactionRisikoKunde.Items.Add(tabItemtestKRBOinput);

            Cic.OpenOne.GateBANKNOW.Common.DTO.igetCreditLimitsGUIDto _eiorigKRBO = new Cic.OpenOne.GateBANKNOW.Common.DTO.igetCreditLimitsGUIDto();
            _eiorigKRBO.sysperole = 671;
            _eiorigKRBO.sysprbildwelt = 11;
            _eiorigKRBO.sysvart = 3;
            _eiorigKRBO.sysprusetype = 1;
            _eiorigKRBO.sysprchannel = 2;
            _eiorigKRBO.perDate = DateTime.Now;
            _eiorigKRBO.syskdtyp = 1;

            NewAuskunft testCreditLimitinput = new NewAuskunft("TEST", _eiorigKRBO, KRBO, typeof(igetCreditLimitsGUIDto), "getCreditLimits", "");
            var tabItemtestCreditLimitinput = new System.Windows.Controls.TabItem();
            tabItemtestCreditLimitinput.Header = "getCreditLimits";
            tabItemtestCreditLimitinput.TabIndex = 1;
            tabItemtestCreditLimitinput.Content = testCreditLimitinput;
            tabcontrolCreditLimit.Items.Add(tabItemtestCreditLimitinput);

        }
    }
}