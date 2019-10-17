using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;


namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse der Ueberleitungen
    /// </summary>
    [TestFixture()]
    class AngAntBoTest
    {
        /// <summary>
        /// Dynamic Mock AngAntDao
        /// </summary>
        public DynamicMock AngAntDaoMock;

        // <summary>
        // Dynamic Mock vgDao
        // </summary>
        //public DynamicMock vgDaoMock;

        // <summary>
        // Dynamic Mock EurotaxDBDao
        // </summary>
        //public DynamicMock etdbDaoMock;

        // <summary>
        // Dynamic Mock EurotaxWSDao
        // </summary>
        //public DynamicMock etwsDaoMock;

        // <summary>
        // Dynamic Mock AuskunftDao
        // </summary>
        //public DynamicMock auskDaoMock;

        // <summary>
        // Dynamic Mock ObTypDao
        // </summary>
        //public DynamicMock ObTypDaoMock;

        /// <summary>
        /// Dynamic Mock PrismaParameter
        /// </summary>
        public DynamicMock prismaDaoMock;

        /// <summary>
        /// Dynamic Mock ObTyp
        /// </summary>
        public DynamicMock ObTypMock;

        /// <summary>
        /// Dynamic Mock KundeDao
        /// </summary>
        public DynamicMock kundeDaoMock;

        /// <summary>
        /// Testclass
        /// </summary>
        public AngAntBo Testclass;

        /// <summary>
        /// Testclass
        /// </summary>
        public DynamicMock translateDao;

        /// <summary>
        /// 
        /// </summary>
        public DynamicMock quoteDaoMock;

        /// <summary>
        /// 
        /// </summary>
        public DynamicMock vgDaoMock;

        public DynamicMock eaihotDaoMock;

        public DynamicMock trBoMock;

        /// <summary>
        /// Setup of the test
        /// </summary>
        [SetUp]
        public void UeberleitungenBoTestInit()
        {
            AngAntDaoMock = new DynamicMock(typeof(IAngAntDao));
            kundeDaoMock = new DynamicMock(typeof(IKundeDao));
            prismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            ObTypMock = new DynamicMock(typeof(IObTypDao));
            translateDao = new DynamicMock(typeof(ITranslateDao));
            quoteDaoMock = new DynamicMock(typeof(IQuoteDao));
            vgDaoMock = new DynamicMock(typeof(IVGDao));
            eaihotDaoMock = new DynamicMock(typeof(IEaihotDao));
            trBoMock = new DynamicMock(typeof(ITransactionRisikoBo));

            PrismaParameterBo prismaParameterBo = new PrismaParameterBo((IPrismaDao)prismaDaoMock.MockInstance, (IObTypDao)ObTypMock.MockInstance, PrismaParameterBo.CONDITIONS_BANKNOW);
            TranslateBo translateBo = new TranslateBo((ITranslateDao)translateDao.MockInstance);

            Testclass = new AngAntBo((IAngAntDao)AngAntDaoMock.MockInstance, (IKundeDao)kundeDaoMock.MockInstance, prismaParameterBo, translateBo, (IQuoteDao)quoteDaoMock, (IVGDao) vgDaoMock, (IEaihotDao) eaihotDaoMock, (ITransactionRisikoBo) trBoMock);
        }

        [Test]
        public void testObBrief()
        {
            //using (DdOlExtended context = new DdOlExtended())
            //{

            //   /* ANTOB antob = new ANTOB();
            //    antob.AHKBRUTTO = 9999;
            //    context.ANTOB.Add(antob);*/
               
            //    ANTKALK kalk = new ANTKALK();
            //    context.ANTKALK.Add(kalk);
            //   // kalk.ANTOB = antob;
               

            //    ANGOB angob = new ANGOB();
            //    angob.AHKBRUTTO = 9999;
            //    context.ANGOB.Add(angob);


            //   // kalk.ANGOB = angob;
            //    //angob.ANGKALKList.Add(kalk);
            //   // context.SaveChanges();

            //  ANGOBBRIEF brief = new ANGOBBRIEF();
            //    brief.AART = "TEST";

            //    context.ANGOBBRIEF.Add(brief);
              
            //    angob.ANGOBBRIEFList = brief;
             
                
            //    context.SaveChanges();//System.Data.Objects.SaveOptions.None);

            //   // angob.ANGOBBRIEFList = brief;
               

               

            //}

        }
        /// <summary>
        /// Test call for Angebot to Antrag Test
        /// Die Testwerte hier sind rein wilkürlich vergeben und dienen nur der
        /// Überprüfung ob alle werte Korrekt wieder zurück kommen.
        /// </summary>
        [Test]
        public void processAngebotToAntragTest()
        {
            DateTime Jetzt = DateTime.Now;
            AngebotDto Input = new AngebotDto();
            AngAntVarDto VariantInput = new AngAntVarDto();
            Input.angAntVars = new List<AngAntVarDto>();
            Input.angAntVars.Add(VariantInput);
            KalkulationDto TestKalk = new KalkulationDto();

            TestKalk.angAntKalkDto = new AngAntKalkDto();
            TestKalk.angAntKalkDto.auszahlung = 10000.00;
            TestKalk.angAntKalkDto.auszahlungTyp = 1;
            TestKalk.angAntKalkDto.bgextern = 10000.00;
            TestKalk.angAntKalkDto.bgintern = 10000.00;
            TestKalk.angAntKalkDto.calcRsvgesamt = 10000.00;
            TestKalk.angAntKalkDto.calcRsvmonat = 500.00;
            TestKalk.angAntKalkDto.calcRsvzins = 50.00;
            TestKalk.angAntKalkDto.calcUstzins = 10.00;
            TestKalk.angAntKalkDto.calcZinskosten = 2000.00;
            TestKalk.angAntKalkDto.depot = 200000.00;
            TestKalk.angAntKalkDto.ll = 1000;
            TestKalk.angAntKalkDto.lz = 765;
            TestKalk.angAntKalkDto.rapratebruttoMax = 100.00;
            TestKalk.angAntKalkDto.rapratebruttoMin = 20.00;
            TestKalk.angAntKalkDto.rapzinseffMax = 75.00;
            TestKalk.angAntKalkDto.rapzinseffMin = 15.00;
            TestKalk.angAntKalkDto.rate = 750.00;
            TestKalk.angAntKalkDto.rateBrutto = 1000.00;
            TestKalk.angAntKalkDto.rateUst = 50.00;
            TestKalk.angAntKalkDto.rueckzahlungTyp = 4;
            TestKalk.angAntKalkDto.rw = 500.00;
            TestKalk.angAntKalkDto.rwBrutto = 650.00;
            TestKalk.angAntKalkDto.rwUst = 50.00;
            TestKalk.angAntKalkDto.sysangvar = 2;
            TestKalk.angAntKalkDto.sysantrag = 4;
            TestKalk.angAntKalkDto.syskalk = 123;
            TestKalk.angAntKalkDto.sysobusetype = 5;
            TestKalk.angAntKalkDto.sysprproduct = 133;
            TestKalk.angAntKalkDto.syswaehrung = 0;
            TestKalk.angAntKalkDto.sz = 1000.00;
            TestKalk.angAntKalkDto.szBrutto = 1200.00;
            TestKalk.angAntKalkDto.szUst = 125.00;
            TestKalk.angAntKalkDto.verrechnung = 15000.00;
            TestKalk.angAntKalkDto.verrechnungFlag = true;
            TestKalk.angAntKalkDto.zins = 1.5;
            TestKalk.angAntKalkDto.zinscust = 1.5;
            TestKalk.angAntKalkDto.zinseff = 1.5;
            TestKalk.angAntKalkDto.zinsrap = 1.5;

            TestKalk.angAntProvDto = new List<AngAntProvDto>();
            TestKalk.angAntProvDto.Add(new AngAntProvDto());
            TestKalk.angAntProvDto[0].provision = 500.00;
            TestKalk.angAntProvDto[0].provisionBrutto = 750.00;
            TestKalk.angAntProvDto[0].provisionUst = 25.00;
            TestKalk.angAntProvDto[0].sysangvar = 2;
            TestKalk.angAntProvDto[0].sysantrag = 4;
            TestKalk.angAntProvDto[0].syspartner = 145;
            TestKalk.angAntProvDto[0].sysprov = 22;
            TestKalk.angAntProvDto[0].sysprprovtype = 4;

            TestKalk.angAntSubvDto = new List<AngAntSubvDto>();
            TestKalk.angAntSubvDto.Add(new AngAntSubvDto());
            TestKalk.angAntSubvDto[0].betragBrutto = 200.00;
            TestKalk.angAntSubvDto[0].sysangsubv = 99;
            TestKalk.angAntSubvDto[0].sysangvar = 2;
            TestKalk.angAntSubvDto[0].sysantrag = 4;
            TestKalk.angAntSubvDto[0].sysantsubv = 123;
            TestKalk.angAntSubvDto[0].syssubvg = 12;
            TestKalk.angAntSubvDto[0].syssubvtyp = 2;

            TestKalk.angAntVsDto = new List<AngAntVsDto>();
            TestKalk.angAntVsDto.Add(new AngAntVsDto());
            TestKalk.angAntVsDto[0].praemie = 200.00;
            TestKalk.angAntVsDto[0].sysangvar = 2;
            TestKalk.angAntVsDto[0].sysangvs = 147;
            TestKalk.angAntVsDto[0].sysantrag = 4;
            TestKalk.angAntVsDto[0].sysantvs = 99;
            TestKalk.angAntVsDto[0].sysvs = 341;
            TestKalk.angAntVsDto[0].sysvstyp = 4;


            Input.angAntVars[0].kalkulation = TestKalk;
            Input.angAntVars[0].bezeichnung = "UNit Test Convert";
            Input.angAntVars[0].gueltigBis = new DateTime(2011, 4, 22, 9, 5, 20, 123, DateTimeKind.Local);
            Input.angAntVars[0].rang = 12;
            Input.angAntVars[0].sysangebot = 44;
            Input.angAntVars[0].sysangvar = 2;
            Input.angAntVars[0].inantrag = 1;

            Input.aenderung = new DateTime(2011, 3, 22, 9, 5, 20, 123, DateTimeKind.Local);
            Input.angAntObDto = new AngAntObDto();
            Input.angAntObDto.ahk = 10000.00;
            Input.angebot = "Unittest";

            Input.attribut = "Unittest";

            Input.erfassung = new DateTime(2011, 3, 1, 9, 5, 20, 123, DateTimeKind.Local);
            Input.gueltigBis = new DateTime(2011, 4, 22, 9, 5, 20, 123, DateTimeKind.Local);
            Input.kartennummer = "123456789";
            Input.kkgpflicht = false;
            Input.notstopFlag = false;

            Input.sysberater = 334;
            Input.sysbrand = 23;
            Input.sysid = 1234;
            Input.sysit = 876;
            Input.syskd = 0;
            Input.sysmarktab = 33;
            Input.sysprchannel = 2;
            Input.sysprhgroup = 5;
            Input.syswfuser = 334;
            Input.syswfuserchange = 298;
            Input.testFlag = true;
            Input.vertriebsweg = "Unittest";
            Input.verwendungszweckCode = "Unittest";
            Input.zustand = "Unittest";
            Input.zustandAm = Jetzt;

            AntragDto retval = new AntragDto();
            
            retval = Testclass.processAngebotToAntrag(Input);

            Assert.AreNotSame(retval.kalkulation, Input.angAntVars[0].kalkulation);
            Assert.AreNotSame(retval.kalkulation.angAntKalkDto, Input.angAntVars[0].kalkulation.angAntKalkDto);
            Assert.AreNotSame(retval.kalkulation.angAntProvDto, Input.angAntVars[0].kalkulation.angAntProvDto[0]);
            Assert.AreNotSame(retval.kalkulation.angAntSubvDto, Input.angAntVars[0].kalkulation.angAntSubvDto[0]);
            Assert.AreNotSame(retval.kalkulation.angAntVsDto, Input.angAntVars[0].kalkulation.angAntVsDto[0]);

            Assert.AreEqual(retval.kalkulation.angAntKalkDto.auszahlung, 10000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.auszahlungTyp, 1);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.bgextern, 10000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.bgintern, 10000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.calcRsvgesamt, 10000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.calcRsvmonat, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.calcRsvzins, 50.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.calcUstzins, 10.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.calcZinskosten, 2000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.depot, 200000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.ll, 1000);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.lz, 765);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rapratebruttoMax, 100.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rapratebruttoMin, 20.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rapzinseffMax, 75.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rapzinseffMin, 15.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rate, 750.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rateBrutto, 1000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rateUst, 50.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rueckzahlungTyp, 4);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rw, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rwBrutto, 650.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.rwUst, 50.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysangvar, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysantrag, 4);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.syskalk, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysobusetype, 5);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysprproduct, 133);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.syswaehrung, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sz, 1000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.szBrutto, 1200.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.szUst, 125.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.verrechnung, 15000.00);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.verrechnungFlag, true);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.zins, 1.5);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.zinscust, 1.5);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.zinseff, 1.5);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.zinsrap, 1.5);

            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provision, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionBrutto, 750.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionUst, 25.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysangvar, 0);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysantrag, 4);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].syspartner, 145);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprov, 0);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprprovtype, 4);

            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].betragBrutto, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangsubv, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangvar, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantrag, 4);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantsubv, 123);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvg, 12);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvtyp, 2);

            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].praemie, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvar, 0);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvs, 0);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantrag, 4);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantvs, 99);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvs, 341);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvstyp, 4);

        }

        ///// <summary>
        ///// Test call für Neuwertermittung
        ///// Die Testwerte hier sind rein willkürlich vergeben und dienen nur der
        ///// Überprüfung ob alle werte Korrekt wieder zurück kommen.
        ///// </summary>
        //[Test]
        //public void evaluateNeupreisTest()
        //{
        //    double retval = 0.0;
        //    DateTime Jetzt = DateTime.Now;
        //    NeupreisRequestDto Input = new NeupreisRequestDto();


        //    Input.sysobtyp = 24536; //PORSCHE

        //    NeuwertSettingsDto Settings = new NeuwertSettingsDto();
        //    Settings.External = false;
        //    Settings.sysvgrw = 4711;

        //    AngAntDaoMock.ExpectAndReturn("getAktuellwertSettings", Settings, 24536);

        //    AngAntDaoMock.ExpectAndReturn("evaluateFzNeupreis", 0.5, 24536, 4711);
            
        //    retval = Testclass.evaluateNeupreis(Input);

        //    Assert.AreEqual(retval, 0.5);
        //}

        ///// <summary>
        ///// Test call for Restwertermittlung
        ///// Die Testwerte hier sind rein willkürlich vergeben und dienen nur der
        ///// Überprüfung ob alle werte Korrekt wieder zurück kommen.
        ///// </summary>
        //[Test]
        //public void evaluateRestwertTest()
        //{
        //    double retval = 0.0;
        //    DateTime Jetzt = DateTime.Now;
        //    RestwertRequestDto Input = new RestwertRequestDto();


        //    Input.sysobtyp = 24536; //PORSCHE
        //    Input.perDate = DateTime.Now;
        //    Input.Laufleistung = 30000;
        //    Input.Alter = 5;

        //    RestWertSettingsDto Settings = new RestWertSettingsDto();
        //    Settings.External = false;
        //    Settings.sysvgrw = 4711;

        //    AngAntDaoMock.ExpectAndReturn("getRestwertSettings", Settings, 24536);

        //    vgDaoMock.ExpectAndReturn("getVGValue", 10345.5, Settings.sysvgrw, Input.perDate, Input.Laufleistung.ToString(), Input.Alter.ToString(), VGInterpolationMode.LINEAR);

        //    retval = Testclass.evaluateRestwert(Input);

        //    Assert.AreEqual(retval, 10345.5);
        //}

    }
}
