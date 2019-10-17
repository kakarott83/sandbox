using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.BO.Prisma;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    [TestFixture()]
    class BankNowCalculatorTest
    {
        /// <summary>
        /// Dao Mock, to replace the Gateway's standard DB DAO usage
        /// </summary>
        public DynamicMock KalkulationDaoMock;
        public DynamicMock SvcDaoMock;
        /// <summary>
        /// Dao Mock for Object type Dao
        /// </summary>
        public DynamicMock ObTypDaoMock;
        /// <summary>
        /// Dao Mock for Zins Dao
        /// </summary>
        public DynamicMock ZinsDaoMock;
        /// <summary>
        /// Dao Mock for Prisma Dao
        /// </summary>
        public DynamicMock PrismaDaoMock;
        /// <summary>
        /// Dao Mock für Provisions DAO
        /// </summary>
        public DynamicMock ProvisionsDaoMock;
        /// <summary>
        /// DAO Mock für Angebot/Antrag DAO
        /// </summary>
        public DynamicMock AngAntDaoMock;
        /// <summary>
        /// DAO Mock für VG DAO
        /// </summary>
        public DynamicMock VGDaoMock;
        /// <summary>
        /// Eurotax Webservice DAO Mock
        /// </summary>
        public DynamicMock EurotaxWSDaoMock;
        /// <summary>
        /// Eurotax DB DAO Mock
        /// </summary>
        public DynamicMock EurotaxDBDaoMock;
        /// <summary>
        /// Auskunft DAO Mock
        /// </summary>
        public DynamicMock AuskunftDaoMock;
        /// <summary>
        /// Kunde DAO Mock
        /// </summary>
        public DynamicMock KundeDaoMock;
        /// <summary>
        /// Provisionen DAO Mock
        /// </summary>
        public DynamicMock ProvisionDaoMock;
        /// <summary>
        /// Subventionen DAO Mock
        /// </summary>
        public DynamicMock SubventionDaoMock;
        /// <summary>
        /// Versicherungen DAO Mock
        /// </summary>
        public DynamicMock VersicherungDaoMock;
        /// <summary>
        /// Object to be tested
        /// </summary>
        public KalkulationBo KalkulationBO;
        /// <summary>
        /// Mehrwertsteuerermittlungs DAO Mock
        /// </summary>
        public DynamicMock MwStDaoMock;
        /// <summary>
        /// Quote DAO Mock
        /// </summary>
        public DynamicMock QuoteDaoMock;



        BankNowCalculator Calculator;
        /// <summary>
        /// Setup of the test
        /// </summary>
        [SetUp]
        public void BankNowCalculatorTestInit()
        {
            // Dao Mock
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            ZinsDaoMock = new DynamicMock(typeof(IZinsDao));
            PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            VGDaoMock = new DynamicMock(typeof(IVGDao));
            ProvisionDaoMock = new DynamicMock(typeof(IProvisionDao));
            SubventionDaoMock = new DynamicMock(typeof(ISubventionDao));
            VersicherungDaoMock = new DynamicMock(typeof(IInsuranceDao));
            MwStDaoMock = new DynamicMock(typeof(IMwStDao));
            QuoteDaoMock = new DynamicMock(typeof(IQuoteDao));
            KalkulationDaoMock = new DynamicMock(typeof(IKalkulationDao));
            SvcDaoMock = new DynamicMock(typeof(IPrismaServiceDao));
            Calculator = new BankNowCalculator((IObTypDao)ObTypDaoMock.MockInstance, (IZinsDao)ZinsDaoMock.MockInstance, (IPrismaDao)PrismaDaoMock.MockInstance,
                                               (IVGDao)VGDaoMock.MockInstance, (IProvisionDao)ProvisionDaoMock.MockInstance, (ISubventionDao)SubventionDaoMock.MockInstance,
                                               (IInsuranceDao)VersicherungDaoMock.MockInstance, (IMwStDao)MwStDaoMock.MockInstance, (IQuoteDao)QuoteDaoMock.MockInstance, "de-CH", (IKalkulationDao)KalkulationDaoMock,(IPrismaServiceDao)SvcDaoMock.MockInstance);
        
            KalkulationDaoMock = null;
            ProvisionsDaoMock = null;
            AngAntDaoMock = null;
            EurotaxWSDaoMock = null;
            EurotaxDBDaoMock = null;
            AuskunftDaoMock = null;
            KundeDaoMock = null;
            KalkulationBO = null;
        }

        [Test]
        public void calcInsuranceTest()
        {
            List<AngAntVsDto> VsList = new List<AngAntVsDto>{
                new AngAntVsDto{code="10L1-", lz=48, mitfinflag=1, ppy=12, praemie=0.0, praemieOrg=0.0, praemiep=0.0, sysangvar=0, sysangvs = 0, sysantrag	= 1418, sysantvs=0, sysvs=100, sysvstyp=16,	vsBezeichnung=null, vsTypBezeichnung=null},
                new AngAntVsDto{code="10L1-", lz=48, mitfinflag=1, ppy=12, praemie=0.0, praemieOrg=0.0, praemiep=0.0, sysangvar=0, sysangvs = 0, sysantrag	= 1418, sysantvs=0, sysvs=100, sysvstyp=9,	vsBezeichnung=null, vsTypBezeichnung=null}
            };
            KalkulationDto Kalk = new KalkulationDto()
            {
                angAntAblDto = new List<AngAntAblDto>{
                    new AngAntAblDto{ablTypBezeichnung="", aktuelleRate=0.0, bank="", bemerkung="", betrag=0.0, bic="", blz="", datkalk=new DateTime(2011, 08, 19), datkalkper=new DateTime(2011, 08, 19), empfaenger="", fremdvertrag="", geprueftFlag=false, iban="", kontonr="", ort="", plz="", strasse="", sysabltyp=2, sysangabl=0, sysangebot=0, sysantabl=0, sysantrag=1418, sysvorvt=0, vorVtBezeichnung=""},
                    new AngAntAblDto{ablTypBezeichnung="", aktuelleRate=0.0, bank="", bemerkung="", betrag=0.0, bic="", blz="", datkalk=new DateTime(2011, 08, 19), datkalkper=new DateTime(2011, 08, 19), empfaenger="", fremdvertrag="", geprueftFlag=false, iban="", kontonr="", ort="", plz="", strasse="", sysabltyp=1, sysangabl=0, sysangebot=0, sysantabl=0, sysantrag=1418, sysvorvt=0, vorVtBezeichnung=""},
                },
                angAntKalkDto = new AngAntKalkDto
                {
                    aufschub = 0,
                    auszahlung = 0.0,
                    auszahlungTyp = 0,
                    bgextern = 0.0,
                    bgexternbrutto = 0.0,
                    bgintern = 120000.0,
                    bginternbrutto = 120000.0,
                    calcRsvgesamt = 0.0,
                    calcRsvmonat = 0.0,
                    calcRsvmonatMax = 0.0,
                    calcRsvmonatMin = 0.0,
                    calcRsvzins = 0.0,
                    calcRsvzinsTmp = 0.0,
                    calcUstzins = 1111.0,
                    calcZinskosten = 9507.25,
                    calcZinskostenMax = 0.0,
                    calcZinskostenMin = 0.0,
                    depot = 0.0,
                    ersteRateBruttoInklAbsicherung = 0.0,
                    ll = 0,
                    lz = 48,
                    ob_mark_satzmehrkm = 0.0005,
                    obUseTypeBezeichnung = null,
                    prProductBezeichnung = null,
                    rapratebruttoMax = 0.0,
                    rapratebruttoMin = 0.0,
                    rapzinseffMax = 0.0,
                    rapzinseffMin = 0.0,
                    rate = 2622.0,
                    rateBrutto = 2622.0,
                    rateBruttoInklAbsicherung = 0.0,
                    rateUst = 0.0,
                    rueckzahlungTyp = 0,
                    rw = 10000.0,
                    rwBrutto = 10000.0,
                    rwUst = 0.0,
                    satzmehrkm = 0.0,
                    sysangvar = 0,
                    sysantrag = 1418,
                    syskalk = 0,
                    sysobusetype = 2,
                    sysprhgrp = 0,
                    sysprproduct = 213,
                    syswaehrung = 0,
                    sz = 0.0,
                    szBrutto = 0.0,
                    szUst = 0.0,
                    verrechnung = 0.0,
                    verrechnungFlag = false,
                    waehrungBezeichnung = null,
                    zins = 5.366039,
                    zinscust = 0.0,
                    zinseff = 5.5,
                    zinsrap = 7.5
                },
                angAntProvDto = new List<AngAntProvDto>(),
                angAntProvDtoRapMax = new List<AngAntProvDto>(),
                angAntProvDtoRapMin = new List<AngAntProvDto>(),
                angAntSubvDto = new List<AngAntSubvDto>(),
                angAntVsDto = new List<AngAntVsDto>{
                    new AngAntVsDto{code="10L1-", lz=48, mitfinflag=1, ppy=12, praemie=0.0, praemieOrg=0.0, praemiep=0.0, sysangvar=0, sysangvs = 0, sysantrag	= 1418, sysantvs=0, sysvs=100, sysvstyp=16,	vsBezeichnung=null, vsTypBezeichnung=null},
                    new AngAntVsDto{code="10L1-", lz=48, mitfinflag=1, ppy=12, praemie=0.0, praemieOrg=0.0, praemiep=0.0, sysangvar=0, sysangvs = 0, sysantrag	= 1418, sysantvs=0, sysvs=100, sysvstyp=9,	vsBezeichnung=null, vsTypBezeichnung=null}
                }
            };
            DateTime ActDate = DateTime.Now;
            CalculationInputParameters paramsIn = new CalculationInputParameters
            {
        		barwert=120000.0, 
		        ersteRate=0.0,
		        isCredit=true,
		        isDiffLeasing=false,
		        istzk=true,
		        laufzeit=48.0,
		        mwst=8.0,
		        restwert=10000.0,
		        ust=0.0,
		        zins=7.2539028291500607
            };

            VSTYP VersTyp1 = new VSTYP
            {
                ACTIVEFLAG = 1,
                ANTEILLS = 50.0M,
                ANTEILVS = 50.0M,
                BESCHREIBUNG = "Restschuldversicherung laufzeitunabhängig ohne Prämie",
                BEZEICHNUNG = "Todesfallversicherung pauschal ohne Prämien",
                CODE = "TODESFALL_PAUSCHAL_NULL",
                CODEMETHOD = "L_RIP",
                CODETYPE = 0,
                EFFEKTEN = 0.0M,
                FLAGDEFAULT = 0,
                FLAGEFFEKTEN = 0,
                FLAGERSATZWG = 0,
                FLAGINKASSOART = 0,
                FLAGINSASSEN = 0,
                FLAGNB = 0,
                FLAGNEUWERT = 0,
                FLAGPARKSCHADEN = 0,
                FLAGPAUSCHAL = 1,
                FLAGRS = 0,
                FLAGSBHP = 0,
                FLAGSBVK = 0,
                FLAGVERUNTR = 0,
                FLAGZUBINKL = 0,
                INSASSEN = 0.0M,
                KLASSEHP = null,
                KLASSEVK = null,
                MAXABLAUFALTER = 0,
                MAXEINALTER = 0,
                MAXLAUFZEIT = 0,
                MAXVSL = 0.0M,
                MAXVSLPS = 0.0M,
                MINEINALTER = 0,
                NB = 0.0M,
                NBP = 0.0M,
                PARKSCHADEN = 0.0M,
                PRAEMIEHP = 0.0M,
                PRAEMIEIN = 0.0M,
                PRAEMIERS = 0.0M,
                PRAEMIERSV = 0.0M,
                PRAEMIEVK = 0.0M,
                REGION = null,
                RS = 0.0M,
                SAPRAEMIENFREI = 0.0M,
                SBHP = 0.0M,
                SBVK = 0.0M,
                SFKHP = null,
                SFKVK = null,
                SYSKORRTYP1 = null,
                SYSKORRTYP2 = null,
                SYSQUOTE = 128,
                SYSVG = null,
                SYSVS = 100,
                SYSVSART = 1,
                SYSVSTYP = 16,
                VALIDFROM = null,
                VALIDUNTIL = null
            };

            VSTYP VersTyp2 = new VSTYP
            {
 		        ACTIVEFLAG=1,
		        ANTEILLS= 57.142857000000006M,
		        ANTEILVS= 42.857142999999994M,
		        BESCHREIBUNG="Arbeitsunfähigkeitversicherung mit dem laufzeitabhängigen Prämienmodell",
		        BEZEICHNUNG="AU/EU/AL Laufzeitprämie",
		        CODE="AUEUAL_LZ",
		        CODEMETHOD="K_AUEUAL",
                CODETYPE = 0,
                EFFEKTEN = 0.0M,
                FLAGDEFAULT = 0,
                FLAGEFFEKTEN = 0,
                FLAGERSATZWG = 0,
                FLAGINKASSOART = 0,
                FLAGINSASSEN = 0,
                FLAGNB = 0,
                FLAGNEUWERT = 0,
                FLAGPARKSCHADEN = 0,
                FLAGPAUSCHAL = 1,
                FLAGRS = 0,
                FLAGSBHP = 0,
                FLAGSBVK = 0,
                FLAGVERUNTR = 0,
                FLAGZUBINKL = 0,
                INSASSEN = 0.0M,
                KLASSEHP = null,
                KLASSEVK = null,
                MAXABLAUFALTER = 0,
                MAXEINALTER = 0,
                MAXLAUFZEIT = 0,
                MAXVSL = 0.0M,
                MAXVSLPS = 0.0M,
                MINEINALTER = 0,
                NB = 0.0M,
                NBP = 0.0M,
                PARKSCHADEN = 0.0M,
                PRAEMIEHP = 0.0M,
                PRAEMIEIN = 0.0M,
                PRAEMIERS = 0.0M,
                PRAEMIERSV = 0.0M,
                PRAEMIEVK = 0.0M,
                REGION = null,
                RS = 0.0M,
                SAPRAEMIENFREI = 0.0M,
                SBHP = 0.0M,
                SBVK = 0.0M,
                SFKHP = null,
                SFKVK = null,
                SYSKORRTYP1 = null,
                SYSKORRTYP2 = null,
		        SYSVG=42,
		        SYSVS=100,
		        SYSVSART=2,
		        SYSVSTYP=9,
		        VALIDFROM=null,
		        VALIDUNTIL=null
            };

            VersicherungDaoMock.SetReturnValue("getVSTYP", VersTyp2);

            CalculationOutputParameters Data = Calculator.calcInsurance(VsList, Kalk, ActDate, new InsuranceBo((IInsuranceDao)VersicherungDaoMock.MockInstance, (IQuoteDao)QuoteDaoMock.MockInstance), paramsIn, CalcType.DEFAULT, true);

            Assert.IsNotNull(Data);
            Assert.AreEqual(121596.8509291177, Data.barwert);
            Assert.AreEqual(121596.8509291177, Data.barwertOhneAbsicherung);
            Assert.AreEqual(48.0, Data.laufzeit);
            Assert.AreEqual(1596.8509291177104, Data.mwstTeilzahlungszuschlag);
            Assert.AreEqual(120000.0, Data.provisionsBasis);
            Assert.AreEqual(2745.9402330063672, Data.rate);
            Assert.AreEqual(2745.9402330063672, Data.rateBrutto);
            Assert.AreEqual(false, Data.vorschuessig);
            Assert.AreEqual(7.5000000000000178, Data.zinseff);
        }

        [Test]
        public void calculateProvisionsTest()
        {
            DateTime actDate = DateTime.Now;
            provKontextDto provKontext = new provKontextDto()
                {   perDate = actDate,
		            sysabltyp=0,
		            sysbrand=0,
		            sysfstyp=0,
		            sysobtyp=24699,
		            sysprhgroup=0,
		            sysprkgroup=0,
		            sysprproduct=214,
		            sysvart=3,
		            sysvarttab=0,
		            sysvstyp=0,
		            sysvttyp=0
                };
            double umsatzbasis = 45000;
            double zinsbasis = 844.8;
            double gesAblFaktor = 0;
            double extAblFaktor = 0;
            double laufzeit = 6;
            double auszahlungsBasis = 45000;
            IProvisionBo provBo = new ProvisionBo((IProvisionDao)ProvisionDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, (IPrismaParameterBo)new PrismaParameterBo((IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, PrismaParameterBo.CONDITIONS_BANKNOW), (IVGDao) VGDaoMock.MockInstance);
            prKontextDto prodCtx = new prKontextDto
            {
		        sysbrand=0,
		        syskdtyp=1,
		        sysobart=13,
		        sysobtyp=24699,
		        sysprchannel=1,
		        sysprhgroup=0,
		        sysprinttype=0,
		        sysprkgroup=0,
		        sysprproduct=214,
		        sysprusetype=2,
		        sysvart=3
            };
            List<AngAntVsDto> versicherungen = new List<AngAntVsDto>{
                new AngAntVsDto{
		                code="",
		                lz=6,
		                mitfinflag=1,
		                ppy=1,
		                praemie=0.0,
		                praemieOrg=0.0,
		                praemiep=0.0,
		                sysangvar=0,
		                sysangvs=0,
		                sysantrag=1418,
		                sysantvs=0,
		                sysvs=100,
		                sysvstyp=16,
		                vsBezeichnung=null,
		                vsTypBezeichnung=null
                },
                new AngAntVsDto{
		                code="",
		                lz=6,
		                mitfinflag=1,
		                ppy=1,
		                praemie=40.1,
		                praemieOrg=40.080000000000005,
		                praemiep=0.334,
		                sysangvar=0,
		                sysangvs=0,
		                sysantrag=1418,
		                sysantvs=0,
		                sysvs=100,
		                sysvstyp=9,
		                vsBezeichnung=null,
		                vsTypBezeichnung=null
                }
            };
            List<AngAntAblDto> abloesen = new List<AngAntAblDto>
            {
		        new AngAntAblDto{
		            ablTypBezeichnung="",
		            aktuelleRate=0.0,
		            bank="",
		            bemerkung="",
		            betrag=0.0,
		            bic="",
		            blz="",
        		    datkalk=new DateTime(2011, 08, 17),
        		    datkalkper=new DateTime(2011, 08, 17),
		            empfaenger="",
		            fremdvertrag="",
		            geprueftFlag=false,
		            iban="",
		            kontonr="",
		            ort="",
		            plz="",
		            strasse="",
		            sysabltyp=2,
		            sysangabl=0,
		            sysangebot=0,
		            sysantabl=0,
		            sysantrag=1418,
		            sysvorvt=0,
		            vorVtBezeichnung=""
                },
		        new AngAntAblDto{
		            ablTypBezeichnung="",
		            aktuelleRate=0.0,
		            bank="",
		            bemerkung="",
		            betrag=0.0,
		            bic="",
		            blz="",
        		    datkalk=new DateTime(2011, 08, 17),
        		    datkalkper=new DateTime(2011, 08, 17),
		            empfaenger="",
		            fremdvertrag="",
		            geprueftFlag=false,
		            iban="",
		            kontonr="",
		            ort="",
		            plz="",
		            strasse="",
		            sysabltyp=1,
		            sysangabl=0,
		            sysangebot=0,
		            sysantabl=0,
		            sysantrag=1418,
		            sysvorvt=0,
		            vorVtBezeichnung=""
                }
            };
            List<AngAntProvDto> overwriteProvisions = new List<AngAntProvDto>();
            bool istzk = false;
            bool isDispo = false;

            List<PRFLD> FieldList = new List<PRFLD>{
                new PRFLD{SYSPRFLD=1, SYSPRFLDART=1, NAME="Endalter Kunde", OBJECTMETA="KALK_BORDER_ENDALTERKUNDE", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=2, SYSPRFLDART=1, NAME="Risikoklasse", OBJECTMETA="KALK_BORDER_KUNDENSCORE", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=3, SYSPRFLDART=1, NAME="Laufzeit", DESCRIPTION="Vertragslaufzeit", OBJECTMETA="KALK_BORDER_LZ", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=4, SYSPRFLDART=2, NAME="Laufleistung", DESCRIPTION="Fahrzeuglaufleisttung", OBJECTMETA="KALK_BORDER_LL", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=5, SYSPRFLDART=1, NAME="Kreditbetrag", DESCRIPTION="Nettokreditbetrag", OBJECTMETA="KALK_BORDER_BGINTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=6, SYSPRFLDART=1, NAME="Rate", DESCRIPTION="Monatliche Rate", OBJECTMETA="KALK_BORDER_RATE", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=7, SYSPRFLDART=2, NAME="Fahrzeugalter Anfang", DESCRIPTION="Fahrzeugalter am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBALTEROBJ", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=8, SYSPRFLDART=2, NAME="Fahrzeugalter Ende", DESCRIPTION="Fahrzeugalter am Vertargsende", OBJECTMETA="KALK_BORDER_ENDALTEROBJ", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=9, SYSPRFLDART=2, NAME="Kilometerstand", DESCRIPTION="Kilometrstand am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBNAHMEKM", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=11, SYSPRFLDART=2, NAME="Kilometerstand Ende", DESCRIPTION="Kilometerstand am Vertargsende", OBJECTMETA="KALK_BORDER_ENDLL", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=12, SYSPRFLDART=2, NAME="Restwert Leasing", DESCRIPTION=null, OBJECTMETA="KALK_BORDER_RW", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=13, SYSPRFLDART=1, NAME="1_Rate", DESCRIPTION=null, OBJECTMETA="KALK_BORDER_SZ", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=15, SYSPRFLDART=1, NAME="Zins", DESCRIPTION=null, OBJECTMETA="KALK_SUBV_ZINS", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=36, SYSPRFLDART=1, NAME="Provisionsbasis AUEU RSV Umsatz", DESCRIPTION="Für Umsatzprovision", OBJECTMETA="PROV_BASE_VERSICHERUNG", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=37, SYSPRFLDART=1, NAME="Fremdablösen", DESCRIPTION="DB-Feld offen", OBJECTMETA="PROV_BASE_ABLEXTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=38, SYSPRFLDART=1, NAME="Erstauszahlung", DESCRIPTION="(Dispo)", OBJECTMETA="PROV_BASE_DISPO", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=14, SYSPRFLDART=1, NAME="Restrate Carfinance", DESCRIPTION=null, OBJECTMETA="KALK_BORDER_RW", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=35, SYSPRFLDART=1, NAME="Zinskosten", DESCRIPTION=null, OBJECTMETA="Zinskosten", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=78, SYSPRFLDART=2, NAME="Mehrkilometersatz", DESCRIPTION=null, OBJECTMETA="OB_MARK_SATZMEHRKM", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=79, SYSPRFLDART=2, NAME="RW-Schwelle für Kautionszwang", DESCRIPTION="Schwelle, nach der die Kaution zwangsläufig verlangt wird", OBJECTMETA="KALK_RW_SCHWELLE_KAUTION", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=100, SYSPRFLDART=1, NAME="Provisionsbasis Stückprovision Neugeld", DESCRIPTION=null, OBJECTMETA="PROV_MARK_STUECK_NEUGELD", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=101, SYSPRFLDART=1, NAME="Provisionsbasis AUEU RSV Stückprovision", DESCRIPTION=null, OBJECTMETA="PROV_MARK_STUECK_VERSICHERUNG", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=57, SYSPRFLDART=1, NAME="Aufschub", DESCRIPTION=null, OBJECTMETA="GESCH_MARK_AUFSCHUB", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=58, SYSPRFLDART=1, NAME="Provisionsbasis Zins Neugeld", DESCRIPTION=null, OBJECTMETA="PROV_BASE_ZINS_NEUGELD", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=59, SYSPRFLDART=1, NAME="Provisionsbasis Zins Ablöse intern", DESCRIPTION=null, OBJECTMETA="PROV_BASE_ZINS_ABLINTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=60, SYSPRFLDART=1, NAME="Provisionsbasis Zins Ablöse extern", DESCRIPTION=null, OBJECTMETA="PROV_BASE_ZINS_ABLEXTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=61, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Neugeld", DESCRIPTION=null, OBJECTMETA="PROV_BASE_UMSATZ_NEUGELD", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=62, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Ablöse intern", DESCRIPTION=null, OBJECTMETA="PROV_BASE_UMSATZ_ABLINTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=63, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Ablöse extern", DESCRIPTION=null, OBJECTMETA="PROV_BASE_UMSATZ_ABLEXTERN", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=80, SYSPRFLDART=2, NAME="1.Rate in % vom Barkaufpreis", DESCRIPTION="Höhe der ersten Rate > X% des Barkaufpreises", OBJECTMETA="KALK_BORDER_SZ_PROZENT", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=120, SYSPRFLDART=1, NAME="Provisionsbasis Tod RSV Umsatz", DESCRIPTION="Provisionsbasis Todesfallschutz Versicherung", OBJECTMETA="PROV_BASE_VERSICHERUNG_RIP", CTRLTYPE=0},
                new PRFLD{SYSPRFLD=122, SYSPRFLDART=1, NAME="Provisionsbasis Tod RSV Stückprovision", DESCRIPTION=null, OBJECTMETA="PROV_MARK_STUECK_VERSICHERUNG_RIP", CTRLTYPE=0},
            };

            PrismaDaoMock.SetReturnValue("getFields", FieldList);

            List<AngAntProvDto> Data = Calculator.calculateProvisions(provKontext, umsatzbasis, zinsbasis, gesAblFaktor, extAblFaktor, laufzeit, auszahlungsBasis, provBo, prodCtx, versicherungen, abloesen, overwriteProvisions, istzk, isDispo, gesAblFaktor, null, 120000.0);
        }
    }
}
