using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Summary description for KalkulationBoTest
    /// </summary>
    [TestFixture()]
    public class KalkulationBoTest
    {
        /// <summary>
        /// Dao Mock, to replace the Gateway's standard DB DAO usage
        /// </summary>
        public DynamicMock KalkulationDaoMock;
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
        public DynamicMock SvcDaoMock;

        /// <summary>
        /// Setup of the test
        /// </summary>
        [SetUp]
        public void KalkulationBoTestInit()
        {
            // Dao Mock
            KalkulationDaoMock = new DynamicMock(typeof(IKalkulationDao));
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            ZinsDaoMock = new DynamicMock(typeof(IZinsDao));
            PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            ProvisionsDaoMock = new DynamicMock(typeof(IProvisionDao));
            AngAntDaoMock = new DynamicMock(typeof(IAngAntDao));
            VGDaoMock = new DynamicMock(typeof(IVGDao));
            EurotaxWSDaoMock = new DynamicMock(typeof(IEurotaxWSDao));
            EurotaxDBDaoMock = new DynamicMock(typeof(IEurotaxDBDao));
            AuskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            KundeDaoMock = new DynamicMock(typeof(IKundeDao));
            ProvisionDaoMock = new DynamicMock(typeof(IProvisionDao));
            SubventionDaoMock = new DynamicMock(typeof(ISubventionDao));
            VersicherungDaoMock = new DynamicMock(typeof(IInsuranceDao));
            MwStDaoMock = new DynamicMock(typeof(IMwStDao));
            QuoteDaoMock = new DynamicMock(typeof(IQuoteDao));
            SvcDaoMock = new DynamicMock(typeof(IPrismaServiceDao));

            AngAntVarDto InputFull = new AngAntVarDto();

            // Object to test
            KalkulationBO = new KalkulationBo((IKalkulationDao)KalkulationDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, 
                                              (IZinsDao)ZinsDaoMock.MockInstance, (IPrismaDao)PrismaDaoMock.MockInstance, (IAngAntDao)AngAntDaoMock.MockInstance,
                                              (IVGDao)VGDaoMock.MockInstance, (IEurotaxWSDao)EurotaxWSDaoMock.MockInstance, (IEurotaxDBDao)EurotaxDBDaoMock.MockInstance,
                                              (IAuskunftDao)AuskunftDaoMock.MockInstance, (IKundeDao)KundeDaoMock.MockInstance, (IProvisionDao)ProvisionDaoMock.MockInstance, 
                                              (ISubventionDao)SubventionDaoMock.MockInstance, (IInsuranceDao)VersicherungDaoMock.MockInstance, (IMwStDao)MwStDaoMock.MockInstance, (IQuoteDao)QuoteDaoMock.MockInstance, "de-CH", (IPrismaServiceDao)SvcDaoMock.MockInstance);
        }

        /// <summary>
        /// Test Create Or Update Kalkulation
        /// New Test: Erzeugt eine neue Kalkulation
        /// WhiteboxTest CreateKalkulation
        /// </summary>
        [Test]
        public void CreateOrUpdateKalkulation1()
        {
            DateTime Gueltig = DateTime.Now;
            AngAntVarDto InputEmpty = new AngAntVarDto();
            InputEmpty.sysangebot = 0;

            AngAntVarDto OutputData = new AngAntVarDto();
            OutputData.bezeichnung = "UnitTest";
            OutputData.gueltigBis = Gueltig;
            OutputData.rang = 3;
            OutputData.sysangebot = 412;
            OutputData.sysangvar = 122;
            OutputData.kalkulation = new KalkulationDto();
            OutputData.kalkulation.angAntKalkDto = new AngAntKalkDto();
            OutputData.kalkulation.angAntKalkDto.auszahlung = 10000.00;
            OutputData.kalkulation.angAntKalkDto.auszahlungTyp = 1;
            OutputData.kalkulation.angAntKalkDto.bgextern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.bgintern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvgesamt = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvmonat = 500.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvzins = 50.00;
            OutputData.kalkulation.angAntKalkDto.calcUstzins = 10.00;
            OutputData.kalkulation.angAntKalkDto.calcZinskosten = 2000.00;
            OutputData.kalkulation.angAntKalkDto.depot = 200000.00;
            OutputData.kalkulation.angAntKalkDto.ll = 1000;
            OutputData.kalkulation.angAntKalkDto.lz = 765;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMax = 100.00;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMin = 20.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMax = 75.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMin = 15.00;
            OutputData.kalkulation.angAntKalkDto.rate = 750.00;
            OutputData.kalkulation.angAntKalkDto.rateBrutto = 1000.00;
            OutputData.kalkulation.angAntKalkDto.rateUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.rueckzahlungTyp = 4;
            OutputData.kalkulation.angAntKalkDto.rw = 500.00;
            OutputData.kalkulation.angAntKalkDto.rwBrutto = 650.00;
            OutputData.kalkulation.angAntKalkDto.rwUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.sysangvar = 122;
            OutputData.kalkulation.angAntKalkDto.sysantrag = 412;
            OutputData.kalkulation.angAntKalkDto.syskalk = 33;
            OutputData.kalkulation.angAntKalkDto.sysobusetype = 0;
            OutputData.kalkulation.angAntKalkDto.sysprproduct = 0;
            OutputData.kalkulation.angAntKalkDto.syswaehrung = 0;
            OutputData.kalkulation.angAntKalkDto.sz = 1000.00;
            OutputData.kalkulation.angAntKalkDto.szBrutto = 1200.00;
            OutputData.kalkulation.angAntKalkDto.szUst = 125.00;
            OutputData.kalkulation.angAntKalkDto.verrechnung = 15000.00;
            OutputData.kalkulation.angAntKalkDto.verrechnungFlag = true;
            OutputData.kalkulation.angAntKalkDto.zins = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinscust = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinseff = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinsrap = 1.5;
            OutputData.kalkulation.angAntProvDto = new List<AngAntProvDto>();
            OutputData.kalkulation.angAntProvDto.Add(new AngAntProvDto());
            OutputData.kalkulation.angAntProvDto[0].provision = 500.00;
            OutputData.kalkulation.angAntProvDto[0].provisionBrutto = 750.00;
            OutputData.kalkulation.angAntProvDto[0].provisionUst = 25.00;
            OutputData.kalkulation.angAntProvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntProvDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntProvDto[0].syspartner = 0;
            OutputData.kalkulation.angAntProvDto[0].sysprov = 145;
            OutputData.kalkulation.angAntProvDto[0].sysprprovtype = 4;
            OutputData.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
            OutputData.kalkulation.angAntSubvDto.Add(new AngAntSubvDto());
            OutputData.kalkulation.angAntSubvDto[0].betragBrutto = 200.00;
            OutputData.kalkulation.angAntSubvDto[0].sysangsubv = 34;
            OutputData.kalkulation.angAntSubvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntSubvDto[0].sysantrag = 0;
            OutputData.kalkulation.angAntSubvDto[0].sysantsubv = 0;
            OutputData.kalkulation.angAntSubvDto[0].syssubvg = 12;
            OutputData.kalkulation.angAntSubvDto[0].syssubvtyp = 2;
            OutputData.kalkulation.angAntVsDto = new List<AngAntVsDto>();
            OutputData.kalkulation.angAntVsDto.Add(new AngAntVsDto());
            OutputData.kalkulation.angAntVsDto[0].praemie = 200.00;
            OutputData.kalkulation.angAntVsDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntVsDto[0].sysangvs = 23;
            OutputData.kalkulation.angAntVsDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntVsDto[0].sysantvs = 341;
            OutputData.kalkulation.angAntVsDto[0].sysvs = 45;
            OutputData.kalkulation.angAntVsDto[0].sysvstyp = 4;

            KalkulationDaoMock.ExpectAndReturn("createKalkulation", OutputData, 0);

            AngAntVarDto retval = KalkulationBO.createOrUpdateKalkulation(InputEmpty);
            
            Assert.AreNotEqual(retval, null);
            Assert.AreNotEqual(retval.kalkulation.angAntKalkDto, null);
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
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.syskalk, 33);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysobusetype, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysprproduct, 0);
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
            Assert.AreNotEqual(retval.kalkulation.angAntProvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntProvDto.Count, 1);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provision, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionBrutto, 750.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionUst, 25.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].syspartner, 0);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprov, 145);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprprovtype, 4);
            Assert.AreNotEqual(retval.kalkulation.angAntSubvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto.Count, 1);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].betragBrutto, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangsubv, 34);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantrag, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantsubv, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvg, 12);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvtyp, 2);
            Assert.AreNotEqual(retval.kalkulation.angAntVsDto, null);
            Assert.AreEqual(retval.kalkulation.angAntVsDto.Count, 1);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].praemie, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvs, 23);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantvs, 341);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvs, 45);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvstyp, 4);
        }


        /// <summary>
        /// Test Create Or Update Kalkulation
        /// New Test: Erzeugt eine neue Kalkulation
        /// WhiteboxTest UpdateKalkulation
        /// </summary>
        [Test]
        public void CreateOrUpdateKalkulation2()
        {
            DateTime Gueltig = DateTime.Now;
            AngAntVarDto InputFull = new AngAntVarDto();
            InputFull.bezeichnung = "UnitTest";
            InputFull.gueltigBis = Gueltig;
            InputFull.rang = 3;
            InputFull.sysangebot = 412;
            InputFull.sysangvar = 122;
            InputFull.kalkulation = new KalkulationDto();
            InputFull.kalkulation.angAntKalkDto = new AngAntKalkDto();
            InputFull.kalkulation.angAntKalkDto.auszahlung = 10000.00;
            InputFull.kalkulation.angAntKalkDto.auszahlungTyp = 1;
            InputFull.kalkulation.angAntKalkDto.bgextern = 10000.00;
            InputFull.kalkulation.angAntKalkDto.bgintern = 10000.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvgesamt = 10000.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvmonat = 500.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvzins = 50.00;
            InputFull.kalkulation.angAntKalkDto.calcUstzins = 10.00;
            InputFull.kalkulation.angAntKalkDto.calcZinskosten = 2000.00;
            InputFull.kalkulation.angAntKalkDto.depot = 200000.00;
            InputFull.kalkulation.angAntKalkDto.ll = 1000;
            InputFull.kalkulation.angAntKalkDto.lz = 765;
            InputFull.kalkulation.angAntKalkDto.rapratebruttoMax = 100.00;
            InputFull.kalkulation.angAntKalkDto.rapratebruttoMin = 20.00;
            InputFull.kalkulation.angAntKalkDto.rapzinseffMax = 75.00;
            InputFull.kalkulation.angAntKalkDto.rapzinseffMin = 15.00;
            InputFull.kalkulation.angAntKalkDto.rate = 750.00;
            InputFull.kalkulation.angAntKalkDto.rateBrutto = 1000.00;
            InputFull.kalkulation.angAntKalkDto.rateUst = 50.00;
            InputFull.kalkulation.angAntKalkDto.rueckzahlungTyp = 4;
            InputFull.kalkulation.angAntKalkDto.rw = 500.00;
            InputFull.kalkulation.angAntKalkDto.rwBrutto = 650.00;
            InputFull.kalkulation.angAntKalkDto.rwUst = 50.00;
            InputFull.kalkulation.angAntKalkDto.sysangvar = 122;
            InputFull.kalkulation.angAntKalkDto.sysantrag = 412;
            InputFull.kalkulation.angAntKalkDto.syskalk = 33;
            InputFull.kalkulation.angAntKalkDto.sysobusetype = 0;
            InputFull.kalkulation.angAntKalkDto.sysprproduct = 0;
            InputFull.kalkulation.angAntKalkDto.syswaehrung = 0;
            InputFull.kalkulation.angAntKalkDto.sz = 1000.00;
            InputFull.kalkulation.angAntKalkDto.szBrutto = 1200.00;
            InputFull.kalkulation.angAntKalkDto.szUst = 125.00;
            InputFull.kalkulation.angAntKalkDto.verrechnung = 15000.00;
            InputFull.kalkulation.angAntKalkDto.verrechnungFlag = true;
            InputFull.kalkulation.angAntKalkDto.zins = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinscust = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinseff = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinsrap = 1.5;
            InputFull.kalkulation.angAntProvDto = new List<AngAntProvDto>();
            InputFull.kalkulation.angAntProvDto.Add(new AngAntProvDto());
            InputFull.kalkulation.angAntProvDto[0].provision = 500.00;
            InputFull.kalkulation.angAntProvDto[0].provisionBrutto = 750.00;
            InputFull.kalkulation.angAntProvDto[0].provisionUst = 25.00;
            InputFull.kalkulation.angAntProvDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntProvDto[0].sysantrag = 412;
            InputFull.kalkulation.angAntProvDto[0].syspartner = 0;
            InputFull.kalkulation.angAntProvDto[0].sysprov = 145;
            InputFull.kalkulation.angAntProvDto[0].sysprprovtype = 4;
            InputFull.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
            InputFull.kalkulation.angAntSubvDto.Add(new AngAntSubvDto());
            InputFull.kalkulation.angAntSubvDto[0].betragBrutto = 200.00;
            InputFull.kalkulation.angAntSubvDto[0].sysangsubv = 34;
            InputFull.kalkulation.angAntSubvDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntSubvDto[0].sysantrag = 0;
            InputFull.kalkulation.angAntSubvDto[0].sysantsubv = 0;
            InputFull.kalkulation.angAntSubvDto[0].syssubvg = 12;
            InputFull.kalkulation.angAntSubvDto[0].syssubvtyp = 2;
            InputFull.kalkulation.angAntVsDto = new List<AngAntVsDto>();
            InputFull.kalkulation.angAntVsDto.Add(new AngAntVsDto());
            InputFull.kalkulation.angAntVsDto[0].praemie = 200.00;
            InputFull.kalkulation.angAntVsDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntVsDto[0].sysangvs = 23;
            InputFull.kalkulation.angAntVsDto[0].sysantrag = 412;
            InputFull.kalkulation.angAntVsDto[0].sysantvs = 341;
            InputFull.kalkulation.angAntVsDto[0].sysvs = 45;
            InputFull.kalkulation.angAntVsDto[0].sysvstyp = 4;

            AngAntVarDto OutputData = new AngAntVarDto();
            OutputData.bezeichnung = "UnitTest";
            OutputData.gueltigBis = Gueltig;
            OutputData.rang = 3;
            OutputData.sysangebot = 412;
            OutputData.sysangvar = 122;
            OutputData.kalkulation = new KalkulationDto();
            OutputData.kalkulation.angAntKalkDto = new AngAntKalkDto();
            OutputData.kalkulation.angAntKalkDto.auszahlung = 10000.00;
            OutputData.kalkulation.angAntKalkDto.auszahlungTyp = 1;
            OutputData.kalkulation.angAntKalkDto.bgextern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.bgintern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvgesamt = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvmonat = 500.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvzins = 50.00;
            OutputData.kalkulation.angAntKalkDto.calcUstzins = 10.00;
            OutputData.kalkulation.angAntKalkDto.calcZinskosten = 2000.00;
            OutputData.kalkulation.angAntKalkDto.depot = 200000.00;
            OutputData.kalkulation.angAntKalkDto.ll = 1000;
            OutputData.kalkulation.angAntKalkDto.lz = 765;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMax = 100.00;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMin = 20.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMax = 75.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMin = 15.00;
            OutputData.kalkulation.angAntKalkDto.rate = 750.00;
            OutputData.kalkulation.angAntKalkDto.rateBrutto = 1000.00;
            OutputData.kalkulation.angAntKalkDto.rateUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.rueckzahlungTyp = 4;
            OutputData.kalkulation.angAntKalkDto.rw = 500.00;
            OutputData.kalkulation.angAntKalkDto.rwBrutto = 650.00;
            OutputData.kalkulation.angAntKalkDto.rwUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.sysangvar = 122;
            OutputData.kalkulation.angAntKalkDto.sysantrag = 412;
            OutputData.kalkulation.angAntKalkDto.syskalk = 33;
            OutputData.kalkulation.angAntKalkDto.sysobusetype = 0;
            OutputData.kalkulation.angAntKalkDto.sysprproduct = 0;
            OutputData.kalkulation.angAntKalkDto.syswaehrung = 0;
            OutputData.kalkulation.angAntKalkDto.sz = 1000.00;
            OutputData.kalkulation.angAntKalkDto.szBrutto = 1200.00;
            OutputData.kalkulation.angAntKalkDto.szUst = 125.00;
            OutputData.kalkulation.angAntKalkDto.verrechnung = 15000.00;
            OutputData.kalkulation.angAntKalkDto.verrechnungFlag = true;
            OutputData.kalkulation.angAntKalkDto.zins = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinscust = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinseff = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinsrap = 1.5;
            OutputData.kalkulation.angAntProvDto = new List<AngAntProvDto>();
            OutputData.kalkulation.angAntProvDto.Add(new AngAntProvDto());
            OutputData.kalkulation.angAntProvDto[0].provision = 500.00;
            OutputData.kalkulation.angAntProvDto[0].provisionBrutto = 750.00;
            OutputData.kalkulation.angAntProvDto[0].provisionUst = 25.00;
            OutputData.kalkulation.angAntProvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntProvDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntProvDto[0].syspartner = 0;
            OutputData.kalkulation.angAntProvDto[0].sysprov = 145;
            OutputData.kalkulation.angAntProvDto[0].sysprprovtype = 4;
            OutputData.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
            OutputData.kalkulation.angAntSubvDto.Add(new AngAntSubvDto());
            OutputData.kalkulation.angAntSubvDto[0].betragBrutto = 200.00;
            OutputData.kalkulation.angAntSubvDto[0].sysangsubv = 34;
            OutputData.kalkulation.angAntSubvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntSubvDto[0].sysantrag = 0;
            OutputData.kalkulation.angAntSubvDto[0].sysantsubv = 0;
            OutputData.kalkulation.angAntSubvDto[0].syssubvg = 12;
            OutputData.kalkulation.angAntSubvDto[0].syssubvtyp = 2;
            OutputData.kalkulation.angAntVsDto = new List<AngAntVsDto>();
            OutputData.kalkulation.angAntVsDto.Add(new AngAntVsDto());
            OutputData.kalkulation.angAntVsDto[0].praemie = 200.00;
            OutputData.kalkulation.angAntVsDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntVsDto[0].sysangvs = 23;
            OutputData.kalkulation.angAntVsDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntVsDto[0].sysantvs = 341;
            OutputData.kalkulation.angAntVsDto[0].sysvs = 45;
            OutputData.kalkulation.angAntVsDto[0].sysvstyp = 4;

            KalkulationDaoMock.ExpectAndReturn("updateKalkulation", OutputData, InputFull);
            
            AngAntVarDto retval = KalkulationBO.createOrUpdateKalkulation(InputFull);

            Assert.AreNotEqual(retval, null);
            Assert.AreNotEqual(retval.kalkulation.angAntKalkDto, null);
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
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.syskalk, 33);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysobusetype, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysprproduct, 0);
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
            Assert.AreNotEqual(retval.kalkulation.angAntProvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntProvDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntProvDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provision, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionBrutto, 750.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionUst, 25.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].syspartner, 0);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprov, 145);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprprovtype, 4);
            Assert.AreNotEqual(retval.kalkulation.angAntSubvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntSubvDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].betragBrutto, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangsubv, 34);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantrag, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantsubv, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvg, 12);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvtyp, 2);
            Assert.AreNotEqual(retval.kalkulation.angAntVsDto, null);
            Assert.AreEqual(retval.kalkulation.angAntVsDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntVsDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].praemie, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvs, 23);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantvs, 341);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvs, 45);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvstyp, 4);
        }

        /// <summary>
        /// Kopiert eine bestehende Kalkulation
        /// </summary>
        [Test]
        public void copyKalkulation()
        {
            DateTime Gueltig = DateTime.Now;
            AngAntVarDto InputFull = new AngAntVarDto();
            InputFull.bezeichnung = "UnitTest";
            InputFull.gueltigBis = Gueltig;
            InputFull.rang = 3;
            InputFull.sysangebot = 412;
            InputFull.sysangvar = 122;
            InputFull.kalkulation = new KalkulationDto();
            InputFull.kalkulation.angAntKalkDto = new AngAntKalkDto();
            InputFull.kalkulation.angAntKalkDto.auszahlung = 10000.00;
            InputFull.kalkulation.angAntKalkDto.auszahlungTyp = 1;
            InputFull.kalkulation.angAntKalkDto.bgextern = 10000.00;
            InputFull.kalkulation.angAntKalkDto.bgintern = 10000.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvgesamt = 10000.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvmonat = 500.00;
            InputFull.kalkulation.angAntKalkDto.calcRsvzins = 50.00;
            InputFull.kalkulation.angAntKalkDto.calcUstzins = 10.00;
            InputFull.kalkulation.angAntKalkDto.calcZinskosten = 2000.00;
            InputFull.kalkulation.angAntKalkDto.depot = 200000.00;
            InputFull.kalkulation.angAntKalkDto.ll = 1000;
            InputFull.kalkulation.angAntKalkDto.lz = 765;
            InputFull.kalkulation.angAntKalkDto.rapratebruttoMax = 100.00;
            InputFull.kalkulation.angAntKalkDto.rapratebruttoMin = 20.00;
            InputFull.kalkulation.angAntKalkDto.rapzinseffMax = 75.00;
            InputFull.kalkulation.angAntKalkDto.rapzinseffMin = 15.00;
            InputFull.kalkulation.angAntKalkDto.rate = 750.00;
            InputFull.kalkulation.angAntKalkDto.rateBrutto = 1000.00;
            InputFull.kalkulation.angAntKalkDto.rateUst = 50.00;
            InputFull.kalkulation.angAntKalkDto.rueckzahlungTyp = 4;
            InputFull.kalkulation.angAntKalkDto.rw = 500.00;
            InputFull.kalkulation.angAntKalkDto.rwBrutto = 650.00;
            InputFull.kalkulation.angAntKalkDto.rwUst = 50.00;
            InputFull.kalkulation.angAntKalkDto.sysangvar = 122;
            InputFull.kalkulation.angAntKalkDto.sysantrag = 412;
            InputFull.kalkulation.angAntKalkDto.syskalk = 33;
            InputFull.kalkulation.angAntKalkDto.sysobusetype = 0;
            InputFull.kalkulation.angAntKalkDto.sysprproduct = 0;
            InputFull.kalkulation.angAntKalkDto.syswaehrung = 0;
            InputFull.kalkulation.angAntKalkDto.sz = 1000.00;
            InputFull.kalkulation.angAntKalkDto.szBrutto = 1200.00;
            InputFull.kalkulation.angAntKalkDto.szUst = 125.00;
            InputFull.kalkulation.angAntKalkDto.verrechnung = 15000.00;
            InputFull.kalkulation.angAntKalkDto.verrechnungFlag = true;
            InputFull.kalkulation.angAntKalkDto.zins = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinscust = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinseff = 1.5;
            InputFull.kalkulation.angAntKalkDto.zinsrap = 1.5;
            InputFull.kalkulation.angAntProvDto = new List<AngAntProvDto>();
            InputFull.kalkulation.angAntProvDto.Add(new AngAntProvDto());
            InputFull.kalkulation.angAntProvDto[0].provision = 500.00;
            InputFull.kalkulation.angAntProvDto[0].provisionBrutto = 750.00;
            InputFull.kalkulation.angAntProvDto[0].provisionUst = 25.00;
            InputFull.kalkulation.angAntProvDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntProvDto[0].sysantrag = 412;
            InputFull.kalkulation.angAntProvDto[0].syspartner = 0;
            InputFull.kalkulation.angAntProvDto[0].sysprov = 145;
            InputFull.kalkulation.angAntProvDto[0].sysprprovtype = 4;
            InputFull.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
            InputFull.kalkulation.angAntSubvDto.Add(new AngAntSubvDto());
            InputFull.kalkulation.angAntSubvDto[0].betragBrutto = 200.00;
            InputFull.kalkulation.angAntSubvDto[0].sysangsubv = 34;
            InputFull.kalkulation.angAntSubvDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntSubvDto[0].sysantrag = 0;
            InputFull.kalkulation.angAntSubvDto[0].sysantsubv = 0;
            InputFull.kalkulation.angAntSubvDto[0].syssubvg = 12;
            InputFull.kalkulation.angAntSubvDto[0].syssubvtyp = 2;
            InputFull.kalkulation.angAntVsDto = new List<AngAntVsDto>();
            InputFull.kalkulation.angAntVsDto.Add(new AngAntVsDto());
            InputFull.kalkulation.angAntVsDto[0].praemie = 200.00;
            InputFull.kalkulation.angAntVsDto[0].sysangvar = 122;
            InputFull.kalkulation.angAntVsDto[0].sysangvs = 23;
            InputFull.kalkulation.angAntVsDto[0].sysantrag = 412;
            InputFull.kalkulation.angAntVsDto[0].sysantvs = 341;
            InputFull.kalkulation.angAntVsDto[0].sysvs = 45;
            InputFull.kalkulation.angAntVsDto[0].sysvstyp = 4;

            AngAntVarDto OutputData = new AngAntVarDto();
            OutputData.bezeichnung = "UnitTest";
            OutputData.gueltigBis = Gueltig;
            OutputData.rang = 3;
            OutputData.sysangebot = 412;
            OutputData.sysangvar = 122;
            OutputData.kalkulation = new KalkulationDto();
            OutputData.kalkulation.angAntKalkDto = new AngAntKalkDto();
            OutputData.kalkulation.angAntKalkDto.auszahlung = 10000.00;
            OutputData.kalkulation.angAntKalkDto.auszahlungTyp = 1;
            OutputData.kalkulation.angAntKalkDto.bgextern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.bgintern = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvgesamt = 10000.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvmonat = 500.00;
            OutputData.kalkulation.angAntKalkDto.calcRsvzins = 50.00;
            OutputData.kalkulation.angAntKalkDto.calcUstzins = 10.00;
            OutputData.kalkulation.angAntKalkDto.calcZinskosten = 2000.00;
            OutputData.kalkulation.angAntKalkDto.depot = 200000.00;
            OutputData.kalkulation.angAntKalkDto.ll = 1000;
            OutputData.kalkulation.angAntKalkDto.lz = 765;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMax = 100.00;
            OutputData.kalkulation.angAntKalkDto.rapratebruttoMin = 20.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMax = 75.00;
            OutputData.kalkulation.angAntKalkDto.rapzinseffMin = 15.00;
            OutputData.kalkulation.angAntKalkDto.rate = 750.00;
            OutputData.kalkulation.angAntKalkDto.rateBrutto = 1000.00;
            OutputData.kalkulation.angAntKalkDto.rateUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.rueckzahlungTyp = 4;
            OutputData.kalkulation.angAntKalkDto.rw = 500.00;
            OutputData.kalkulation.angAntKalkDto.rwBrutto = 650.00;
            OutputData.kalkulation.angAntKalkDto.rwUst = 50.00;
            OutputData.kalkulation.angAntKalkDto.sysangvar = 122;
            OutputData.kalkulation.angAntKalkDto.sysantrag = 412;
            OutputData.kalkulation.angAntKalkDto.syskalk = 33;
            OutputData.kalkulation.angAntKalkDto.sysobusetype = 0;
            OutputData.kalkulation.angAntKalkDto.sysprproduct = 0;
            OutputData.kalkulation.angAntKalkDto.syswaehrung = 0;
            OutputData.kalkulation.angAntKalkDto.sz = 1000.00;
            OutputData.kalkulation.angAntKalkDto.szBrutto = 1200.00;
            OutputData.kalkulation.angAntKalkDto.szUst = 125.00;
            OutputData.kalkulation.angAntKalkDto.verrechnung = 15000.00;
            OutputData.kalkulation.angAntKalkDto.verrechnungFlag = true;
            OutputData.kalkulation.angAntKalkDto.zins = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinscust = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinseff = 1.5;
            OutputData.kalkulation.angAntKalkDto.zinsrap = 1.5;
            OutputData.kalkulation.angAntProvDto = new List<AngAntProvDto>();
            OutputData.kalkulation.angAntProvDto.Add(new AngAntProvDto());
            OutputData.kalkulation.angAntProvDto[0].provision = 500.00;
            OutputData.kalkulation.angAntProvDto[0].provisionBrutto = 750.00;
            OutputData.kalkulation.angAntProvDto[0].provisionUst = 25.00;
            OutputData.kalkulation.angAntProvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntProvDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntProvDto[0].syspartner = 0;
            OutputData.kalkulation.angAntProvDto[0].sysprov = 145;
            OutputData.kalkulation.angAntProvDto[0].sysprprovtype = 4;
            OutputData.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
            OutputData.kalkulation.angAntSubvDto.Add(new AngAntSubvDto());
            OutputData.kalkulation.angAntSubvDto[0].betragBrutto = 200.00;
            OutputData.kalkulation.angAntSubvDto[0].sysangsubv = 34;
            OutputData.kalkulation.angAntSubvDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntSubvDto[0].sysantrag = 0;
            OutputData.kalkulation.angAntSubvDto[0].sysantsubv = 0;
            OutputData.kalkulation.angAntSubvDto[0].syssubvg = 12;
            OutputData.kalkulation.angAntSubvDto[0].syssubvtyp = 2;
            OutputData.kalkulation.angAntVsDto = new List<AngAntVsDto>();
            OutputData.kalkulation.angAntVsDto.Add(new AngAntVsDto());
            OutputData.kalkulation.angAntVsDto[0].praemie = 200.00;
            OutputData.kalkulation.angAntVsDto[0].sysangvar = 122;
            OutputData.kalkulation.angAntVsDto[0].sysangvs = 23;
            OutputData.kalkulation.angAntVsDto[0].sysantrag = 412;
            OutputData.kalkulation.angAntVsDto[0].sysantvs = 341;
            OutputData.kalkulation.angAntVsDto[0].sysvs = 45;
            OutputData.kalkulation.angAntVsDto[0].sysvstyp = 4;

            KalkulationDaoMock.ExpectAndReturn("getKalkulation", OutputData, 122);

            AngAntVarDto retval = KalkulationBO.copyKalkulation(InputFull);

            Assert.AreNotEqual(retval, null);
            Assert.AreNotEqual(retval.kalkulation.angAntKalkDto, null);
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
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.syskalk, 33);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysobusetype, 0);
            Assert.AreEqual(retval.kalkulation.angAntKalkDto.sysprproduct, 0);
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
            Assert.AreNotEqual(retval.kalkulation.angAntProvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntProvDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntProvDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provision, 500.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionBrutto, 750.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].provisionUst, 25.00);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].syspartner, 0);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprov, 145);
            Assert.AreEqual(retval.kalkulation.angAntProvDto[0].sysprprovtype, 4);
            Assert.AreNotEqual(retval.kalkulation.angAntSubvDto, null);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntSubvDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].betragBrutto, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangsubv, 34);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantrag, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].sysantsubv, 0);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvg, 12);
            Assert.AreEqual(retval.kalkulation.angAntSubvDto[0].syssubvtyp, 2);
            Assert.AreNotEqual(retval.kalkulation.angAntVsDto, null);
            Assert.AreEqual(retval.kalkulation.angAntVsDto.Count, 1);
            Assert.AreNotEqual(retval.kalkulation.angAntVsDto[0], null);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].praemie, 200.00);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvar, 122);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysangvs, 23);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantrag, 412);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysantvs, 341);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvs, 45);
            Assert.AreEqual(retval.kalkulation.angAntVsDto[0].sysvstyp, 4);
        }

        /// <summary>
        /// Löscht eine bestehende Kalkulation
        /// </summary>
        [Test]
        public void deleteKalkulation()
        {

            KalkulationDaoMock.Expect("deleteKalkulation", 122);

            KalkulationBO.deleteKalkulation(122);

        }

       /// <summary>
        /// Löst eine Kalkulation
        /// </summary>
        [Test]
        public void solveKalkulation()
        {
            DateTime CurrentTime = DateTime.Now;
            KalkulationDto kalkulation = new KalkulationDto();
            kalkulation.angAntKalkDto = new AngAntKalkDto();
           
            IKalkulationBo kalkulationBO = KalkulationBO;

            prKontextDto prodCtx = new prKontextDto();
            prodCtx.sysprproduct = 232;
            prodCtx.sysperole = 82;
            prodCtx.sysprhgroup = 61;
            prodCtx.perDate = DateTime.Now;
           

            //LeaseNow
            kalkulation.angAntKalkDto.bginternbrutto = 18000;
            
            kalkulation.angAntKalkDto.lz = 48;
            kalkulation.angAntKalkDto.szBrutto = 500;
            kalkulation.angAntKalkDto.rwBrutto = 5000;
            kalkulation.angAntKalkDto.sysprproduct = 232;//leasnow
            kalkulation.angAntProvDto = new List<AngAntProvDto>();
          

            PRRAP prrap = new PRRAP();
            prrap.ACTIVEFLAG = 1;
            prrap.DESCRIPTION = "Unit Test";
            prrap.MAXVALUE = 0;
            prrap.MINVALUE = 100;
            prrap.NAME = "Unit test";
            
            prrap.SOURCEBASIS = 5;
            prrap.SYSBRAND = 200;
            prrap.SYSPRRAP = 150;
            prrap.VALIDFROM = CurrentTime;
            prrap.VALIDUNTIL = CurrentTime;
            

            KalkulationDaoMock.ExpectAndReturn("getPrRap", 46, prrap);

            List<PRRAPVAL> values = new List<PRRAPVAL>();
            values.Add(new PRRAPVAL());
            values[0].SCORE = "Unit Test";
            values[0].SYSPRRAP = 150;
            values[0].SYSPRRAPVAL = 23;

            KalkulationDaoMock.ExpectAndReturn("getRapValues", 150, values);

           
            kalkKontext calcKontext = new kalkKontext();
            calcKontext.zinsNominal = 7.2;
            calcKontext.useZinsNominal = true;

            List<PRPROVSTEP> prprovstep = new List<PRPROVSTEP>();
            prprovstep.Add(new PRPROVSTEP()
            {
                ADJUSTMENTTRIGGER = 1,
                METHOD = 1,
                PROVRATE = 10,
                PROVVAL = 10,
                RANK = 1,
                SOURCEBASIS = 1,
                SYSABLTYP = 1,
                SYSBRAND = 1,
                SYSFSTYP = 1,
                SYSOBTYP = 1,
                SYSPRFLD = 1,
                SYSPRHGROUP = 1,
                SYSPROVSTRCT = 1,
                SYSPRPRODUCT = 1,
                SYSPRPROVSET = 1,
                SYSPRPROVSTEP = 1,
                SYSPRPROVTYPE = 1,
                SYSVART = 1,
                SYSVARTTAB = 1,
                SYSVG = 1,
                SYSVSTYP = 1,
                SYSVTTYP = 1
            });

            ProvisionsDaoMock.SetReturnValue("getProvsteps", prprovstep);

            PRPRODUCT product = new PRPRODUCT();
            product.ACTIVEFLAG = 1;
            product.DESCRIPTION = "UnitTest Product Desc";
            product.NAME = "UnitTest Product";
            product.NAMEINTERN = "UnitTest Product Intern";
            product.SOURCEBASIS = 33;
            product.SYSAKTION = 12345678;
            product.SYSIBOR = 87654321;
            product.SYSINTSTRCT = 6789090;
            product.SYSINTTYPE = 33;
            product.SYSKALKTYP = 99;
            product.SYSPRPRODTYPE = 3;
            product.SYSPRPRODUCT = 232;
            product.SYSPRRAP = 9;
            product.SYSVART = 1;
            product.SYSVARTTAB = 1;
            product.SYSVG = 1;
            product.SYSVTTYP = 44;
            product.TARIFCODE = "Neu";
            product.VALIDFROM = DateTime.Now;
            product.VALIDUNTIL = DateTime.Now;

            List<PRFLD> fields = new List<PRFLD>
                {new PRFLD {SYSPRFLD=1, SYSPRFLDART=1,	NAME="Endalter Kunde", DESCRIPTION="", OBJECTMETA="KALK_BORDER_ENDALTERKUNDE", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=2,	SYSPRFLDART=1,	NAME="Risikoklasse", DESCRIPTION="", OBJECTMETA="KALK_BORDER_KUNDENSCORE", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=3,	SYSPRFLDART=1,	NAME="Laufzeit", DESCRIPTION="Vertragslaufzeit", OBJECTMETA="KALK_BORDER_LZ", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=4,	SYSPRFLDART=2,	NAME="Laufleistung", DESCRIPTION="Fahrzeuglaufleisttung", OBJECTMETA="KALK_BORDER_LL", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=5,	SYSPRFLDART=1,	NAME="Kreditbetrag", DESCRIPTION="Nettokreditbetrag", OBJECTMETA="KALK_BORDER_BGINTERN", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=6,	SYSPRFLDART=1,	NAME="Rate", DESCRIPTION="Monatliche Rate", OBJECTMETA="KALK_BORDER_RATE", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=7,	SYSPRFLDART=2,	NAME="Fahrzeugalter Anfang", DESCRIPTION="Fahrzeugalter am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBALTEROBJ", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=8,	SYSPRFLDART=2,	NAME="Fahrzeugalter Ende", DESCRIPTION="Fahrzeugalter am Vertargsende", OBJECTMETA="KALK_BORDER_ENDALTEROBJ", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=9,	SYSPRFLDART=2,	NAME="Kilometerstand", DESCRIPTION="Kilometrstand am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBNAHMEKM", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=11, SYSPRFLDART=2,	NAME="Kilometerstand Ende", DESCRIPTION="Kilometerstand am Vertargsende", OBJECTMETA="KALK_BORDER_ENDLL", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=12, SYSPRFLDART=2,	NAME="Restwert Leasing", DESCRIPTION="", OBJECTMETA="KALK_BORDER_RW", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=13, SYSPRFLDART=1,	NAME="1_Rate", DESCRIPTION="", OBJECTMETA="KALK_BORDER_SZ", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=15, SYSPRFLDART=1,	NAME="Zins", DESCRIPTION="", OBJECTMETA="KALK_SUBV_ZINS", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=36, SYSPRFLDART=1,	NAME="Betrag Ratenabsicherung", DESCRIPTION="", OBJECTMETA="PROV_BASE_VERSICHERUNG", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=37, SYSPRFLDART=1,	NAME="Fremdablösen", DESCRIPTION="DB-Feld offen", OBJECTMETA="PROV_BASE_ABLEXTERN", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=38, SYSPRFLDART=1,	NAME="Erstauszahlung", DESCRIPTION="(Dispo)", OBJECTMETA="PROV_BASE_DISPO", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=14, SYSPRFLDART=1,	NAME="Restrate Carfinance", DESCRIPTION="", OBJECTMETA="KALK_BORDER_RW", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=35, SYSPRFLDART=1,	NAME="Zinskosten", DESCRIPTION="", OBJECTMETA="Zinskosten", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=78, SYSPRFLDART=2,	NAME="Mehrkilometersatz", DESCRIPTION="", OBJECTMETA="OB_MARK_SATZMEHRKM", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=57, SYSPRFLDART=1,	NAME="Aufschub", DESCRIPTION="	", OBJECTMETA="GESCH_MARK_AUFSCHUB", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=58, SYSPRFLDART=1,	NAME="Provisionsbasis Zins Neugeld", DESCRIPTION="", OBJECTMETA="PROV_BASE_ZINS_NEUGELD", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=59, SYSPRFLDART=1,	NAME="Provisionsbasis Zins Ablöse intern", DESCRIPTION="", OBJECTMETA="PROV_BASE_ZINS_ABLINTERN", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=60, SYSPRFLDART=1,	NAME="Provisionsbasis Zins Ablöse extern", DESCRIPTION="", OBJECTMETA="PROV_BASE_ZINS_ABLEXTERN", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=61, SYSPRFLDART=1,	NAME="Provisionsbasis Umsatz Neugeld", DESCRIPTION="", OBJECTMETA="PROV_BASE_UMSATZ_NEUGELD", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=62, SYSPRFLDART=1,	NAME="Provisionsbasis Umsatz Ablöse intern", DESCRIPTION="", OBJECTMETA="PROV_BASE_UMSATZ_ABLINTERN", CTRLTYPE=0},
                new PRFLD {SYSPRFLD=63, SYSPRFLDART=1,	NAME="Provisionsbasis Umsatz Ablöse extern", DESCRIPTION="", OBJECTMETA="PROV_BASE_UMSATZ_ABLEXTERN", CTRLTYPE=0} };

            PrismaDaoMock.SetReturnValue("getFields", fields);

            List<ParameterSetConditionLink> paramSetCondLinks = new List<ParameterSetConditionLink>{
                new ParameterSetConditionLink{level=1, sysprparset=2, area=99999,	sysid=0, sysparent=0},
                new ParameterSetConditionLink{level=1, sysprparset=43, area=61,	sysid=22, sysparent=0},
                new ParameterSetConditionLink{level=2, sysprparset=3, area=3, sysid=1, sysparent=2},
                new ParameterSetConditionLink{level=2, sysprparset=63, area=10, sysid=1, sysparent=43},
                new ParameterSetConditionLink{level=2, sysprparset=4, area=3, sysid=2, sysparent=2},
                new ParameterSetConditionLink{level=3, sysprparset=132, area=10, sysid=3, sysparent=4},
                new ParameterSetConditionLink{level=3, sysprparset=23, area=10, sysid=8, sysparent=3},
                new ParameterSetConditionLink{level=3, sysprparset=24, area=10, sysid=3, sysparent=3},
                new ParameterSetConditionLink{level=3, sysprparset=68, area=61, sysid=21, sysparent=63},
                new ParameterSetConditionLink{level=3, sysprparset=26, area=10, sysid=7, sysparent=4},
                new ParameterSetConditionLink{level=3, sysprparset=5, area=10, sysid=1, sysparent=3},
                new ParameterSetConditionLink{level=3, sysprparset=25, area=10, sysid=3, sysparent=4},
                new ParameterSetConditionLink{level=3, sysprparset=27, area=10, sysid=4, sysparent=4},
                new ParameterSetConditionLink{level=4, sysprparset=18, area=31, sysid=8, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=16, area=31, sysid=9, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=14, area=31, sysid=4, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=10, area=31, sysid=3, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=8, area=31, sysid=7, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=6, area=31, sysid=1, sysparent=5},
                new ParameterSetConditionLink{level=4, sysprparset=21, area=31, sysid=10, sysparent=5},
                new ParameterSetConditionLink{level=5, sysprparset=9, area=30, sysid=13, sysparent=8},
                new ParameterSetConditionLink{level=5, sysprparset=7, area=30, sysid=13, sysparent=6},
                new ParameterSetConditionLink{level=5, sysprparset=19, area=30, sysid=13, sysparent=18},
                new ParameterSetConditionLink{level=5, sysprparset=17, area=30, sysid=13, sysparent=16},
                new ParameterSetConditionLink{level=5, sysprparset=15, area=30, sysid=13, sysparent=14},
                new ParameterSetConditionLink{level=5, sysprparset=13, area=30, sysid=13, sysparent=10},
                new ParameterSetConditionLink{level=6, sysprparset=130, area=20, sysid=5, sysparent=17},
                new ParameterSetConditionLink{level=6, sysprparset=133, area=10, sysid=2, sysparent=7},
                new ParameterSetConditionLink{level=6, sysprparset=129, area=21, sysid=174, sysparent=17}  };

            PrismaDaoMock.SetReturnValue("getParamSets", paramSetCondLinks);

            List <long> obTypList = new List<long>{25345, 25334, 25233, 24882, 234};
            ObTypDaoMock.SetReturnValue("getObTypAscendants", obTypList);

            List<ParameterConditionLink> paramCondLinks = new List<ParameterConditionLink>{
                new ParameterConditionLink{sysprparset=120, sysprproduct=102, area=14},			
                new ParameterConditionLink{sysprparset=97, sysprproduct=102, area=14},
                new ParameterConditionLink{sysprparset=71, sysprproduct=84, area=14},
                new ParameterConditionLink{sysprparset=88, sysprproduct=53, area=14},
                new ParameterConditionLink{sysprparset=88, sysprproduct=104, area=14},			
                new ParameterConditionLink{sysprparset=88, sysprproduct=186, area=14},		
                new ParameterConditionLink{sysprparset=88, sysprproduct=50, area=14},		
                new ParameterConditionLink{sysprparset=88, sysprproduct=124, area=14},		
                new ParameterConditionLink{sysprparset=89, sysprproduct=56, area=14},		
                new ParameterConditionLink{sysprparset=89, sysprproduct=52, area=14},		
                new ParameterConditionLink{sysprparset=89, sysprproduct=57, area=14},		
                new ParameterConditionLink{sysprparset=89, sysprproduct=102, area=14},		
                new ParameterConditionLink{sysprparset=96, sysprproduct=208, area=14},			
                new ParameterConditionLink{sysprparset=88, sysprproduct=45, area=14},			
                new ParameterConditionLink{sysprparset=88, sysprproduct=188, area=14},		
                new ParameterConditionLink{sysprparset=120, sysprproduct=102, area=14},		
                new ParameterConditionLink{sysprparset=71, sysprproduct=102, area=14},		
                new ParameterConditionLink{sysprparset=97, sysprproduct=172, area=14},		
                new ParameterConditionLink{sysprparset=97, sysprproduct=82, area=14},		
                new ParameterConditionLink{sysprparset=88, sysprproduct=59, area=14},			
                new ParameterConditionLink{sysprparset=88, sysprproduct=86, area=14}};

            PrismaDaoMock.SetReturnValue("getParamConditionLinks", paramCondLinks);

            List<ParamDto> parameters = new List<ParamDto>{
                new ParamDto{sysprparset=43, sysID=54, meta="KALK_BORDER_ENDALTERKUNDE", name="AU / EU / AL Maximales Endalter",    visible=false, disabled=false, type=0, minvaln=0, maxvaln=70, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{sysprparset=5, sysID=14, meta="KALK_BORDER_ENDALTERKUNDE", name="Endalter Leasing",                    visible=false, disabled=false, type=0, minvaln=0, maxvaln=80, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{sysprparset=2, sysID=2, meta="KALK_BORDER_ENDALTERKUNDE", name="Endalter",                             visible=false, disabled=false, type=0, minvaln=0, maxvaln=70, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{sysprparset=68, sysID=56, meta="KALK_BORDER_ENDALTERKUNDE", name="Maximales Endalter Todesfallschutz", visible=false, disabled=false, type=0, minvaln=0, maxvaln=70, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{sysprparset=26, sysID=193, meta="KALK_BORDER_KUNDENSCORE", name="Unterer Cut-Off  Dispo",              visible=false, disabled=false, type=0, minvaln=0, maxvaln=11, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{sysprparset=129, sysID=167, meta="KALK_BORDER_KUNDENSCORE", name="Objekttyp ausweichen",               visible=false, disabled=false, type=0, minvaln=2000, maxvaln=0, defvaln=0,              maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{sysprparset=2, sysID=3, meta="KALK_BORDER_KUNDENSCORE", name="Unterer Cut-Off",                        visible=false, disabled=false, type=0, minvaln=0, maxvaln=13, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{sysprparset=120, sysID=144, meta="KALK_BORDER_LZ", name="Lauzeit TEST ADSP",                           visible=true, disabled=false, type=0, minvaln=0, maxvaln=24, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{sysprparset=5, sysID=16, meta="KALK_BORDER_LZ", name="Laufzeit Leasing",                               visible=false, disabled=false, type=0, minvaln=12, maxvaln=60, defvaln=48,              maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{sysprparset=26, sysID=35, meta="KALK_BORDER_LZ", name="Laufzeit Dispo",                                visible=false, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=36,              maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{sysprparset=89, sysID=69, meta="KALK_BORDER_LZ", name="Laufzeit 6, 72, def. 48",                       visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48,               maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{sysprparset=88, sysID=68, meta="KALK_BORDER_LZ", name="Laufzeiten: Min, Max, Default",                 visible=false, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=36,              maxvalp=0, defvalp=0, stepsize=12, sysprfld=3},
                new ParamDto{sysprparset=27, sysID=71, meta="KALK_BORDER_LZ", name="Laufzeit Express",                              visible=false, disabled=false, type=0, minvaln=6, maxvaln=11, defvaln=11,               maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{sysprparset=0, sysID=95, meta="KALK_BORDER_LZ", name="CICH",                                           visible=true, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=24,              maxvalp=0, defvalp=0, stepsize=2, sysprfld=3},
                new ParamDto{sysprparset=25, sysID=123, meta="KALK_BORDER_LZ", name="Laufzeiten KF Classic",                        visible=false, disabled=false, type=0, minvaln=6, maxvaln=60, defvaln=12,               maxvalp=0, defvalp=0, stepsize=3, sysprfld=3},
                new ParamDto{sysprparset=71, sysID=166, meta="KALK_BORDER_LZ", name="Lauzeit 2",                                    visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0,                  maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{sysprparset=23, sysID=31, meta="KALK_BORDER_LZ", name="Laufzeit Carfinance",                           visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48,               maxvalp=0, defvalp=0, stepsize=6, sysprfld=3},
                new ParamDto{sysprparset=2, sysID=4, meta="KALK_BORDER_LZ", name="Laufzeit",                                        visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48,               maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{sysprparset=21, sysID=29, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Zubehör",             visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=19, sysID=28, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Sonstige Occasion",   visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=17, sysID=27, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Boote Occasion",      visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,     maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=15, sysID=26, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Wohnmobile Occasion", visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=13, sysID=25, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Zweirad Occasion",    visible=false, disabled=false, type=0, minvaln=3000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=10, sysID=24, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing Zweirad",             visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=9, sysID=18, meta="KALK_BORDER_BGINTERN", name="Nettokredit LKW",                          visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=7, sysID=17, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing PKW Occasion",         visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000,    maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=5, sysID=12, meta="KALK_BORDER_BGINTERN", name="Nettokredit Leasing",                      visible=false, disabled=false, type=0, minvaln=10000, maxvaln=1000000, defvaln=45000,   maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=4, sysID=32, meta="KALK_BORDER_BGINTERN", name="Nettokredit KF",                           visible=false, disabled=false, type=0, minvaln=500, maxvaln=250000, defvaln=25000,      maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=0, sysID=214, meta="KALK_BORDER_BGINTERN", name="Kriditrahmen 500 - 20.000",               visible=false, disabled=false, type=0, minvaln=500, maxvaln=20000, defvaln=10000,       maxvalp=0, defvalp=0, stepsize=500, sysprfld=5},
                new ParamDto{sysprparset=130, sysID=150, meta="KALK_BORDER_BGINTERN", name="Kreditbetrag STD HG",                   visible=true, disabled=false, type=0, minvaln=10000, maxvaln=0, defvaln=0,             maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=129, sysID=149, meta="KALK_BORDER_BGINTERN", name="Kreditbetrag Boote",                    visible=true, disabled=false, type=0, minvaln=50000, maxvaln=0, defvaln=0,             maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=27, sysID=36, meta="KALK_BORDER_BGINTERN", name="Nettokredit Express",                     visible=false, disabled=false, type=0, minvaln=1000, maxvaln=10000, defvaln=10000,      maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=26, sysID=34, meta="KALK_BORDER_BGINTERN", name="Nettokredit Dispo",                       visible=false, disabled=false, type=0, minvaln=5000, maxvaln=40000, defvaln=40000,      maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{sysprparset=25, sysID=33, meta="KALK_BORDER_RATE", name="Rate KF Classic",                             visible=false, disabled=false, type=0, minvaln=50, maxvaln=0, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=6},
                new ParamDto{sysprparset=5, sysID=49, meta="KALK_BORDER_UBALTEROBJ", name="Fahrzeugalter Anfang Leasing",           visible=false, disabled=false, type=0, minvaln=0, maxvaln=6, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=7},
                new ParamDto{sysprparset=5, sysID=50, meta="KALK_BORDER_ENDALTEROBJ", name="Fahrzeugalter Ende Leasing",            visible=false, disabled=false, type=0, minvaln=0, maxvaln=96, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=8},
                new ParamDto{sysprparset=24, sysID=51, meta="KALK_BORDER_UBNAHMEKM", name="Kilometerstand Anfang FF Classic",       visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=9},
                new ParamDto{sysprparset=24, sysID=52, meta="KALK_BORDER_ENDLL", name="Kilometerstand Ende FF Classic",             visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=11},
                new ParamDto{sysprparset=10, sysID=23, meta="KALK_BORDER_ENDLL", name="Kilometerstand Ende Leasing Zweirad",        visible=false, disabled=false, type=0, minvaln=0, maxvaln=75000, defvaln=0,             maxvalp=0, defvalp=0, stepsize=0, sysprfld=11},
                new ParamDto{sysprparset=5, sysID=13, meta="KALK_BORDER_RW", name="Restwert",                                       visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=100, defvalp=0, stepsize=0, sysprfld=12},
                new ParamDto{sysprparset=132, sysID=155, meta="KALK_BORDER_SZ", name="Classic mit aufgeschobener Rate",             visible=true, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=2, sysprfld=13},
                new ParamDto{sysprparset=5, sysID=15, meta="KALK_BORDER_SZ", name="1_Rate",                                         visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=50, defvalp=0, stepsize=0, sysprfld=13},
                new ParamDto{sysprparset=23, sysID=30, meta="KALK_BORDER_RW", name="Restrate Carfinance",                           visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=100, defvalp=0, stepsize=0, sysprfld=14},
                new ParamDto{sysprparset=97, sysID=106, meta="KALK_SUBV_ZINS", name="UO Zins CREDIT-now Classic",                   visible=true, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0,                 maxvalp=12, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{sysprparset=2, sysID=57, meta="KALK_SUBV_ZINS", name="Maximaler Effektivzinssatz",                     visible=false, disabled=false, type=0, minvaln=0, maxvaln=15, defvaln=0,                maxvalp=0, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{sysprparset=0, sysID=124, meta="KALK_SUBV_ZINS", name="Sonderkondition <3",                            visible=false, disabled=false, type=0, minvaln=0, maxvaln=3, defvaln=0,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{sysprparset=96, sysID=102, meta="GESCH_MARK_AUFSCHUB", name="Aufschub 3M",                             visible=false, disabled=false, type=0, minvaln=3, maxvaln=3, defvaln=3,                 maxvalp=0, defvalp=0, stepsize=0, sysprfld=57}};

            PrismaDaoMock.SetReturnValue("getParams", parameters);

            PRPRODUCT selproduct = new PRPRODUCT { SYSPRPRODUCT = 232, SYSVART = 3, NAME = "7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)", ACTIVEFLAG = 1, VALIDFROM = new DateTime(2011, 06, 27), SOURCEBASIS = 1, SYSINTSTRCT = 109, SYSPRPRODTYPE = 1, NAMEINTERN = "7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)" };

            PrismaDaoMock.SetReturnValue("getProduct", selproduct);

            VART vart = new VART { SYSVART = 3, BEZEICHNUNG = "CREDIT-now Classic", AKTIVKZ = 1, LGD = 90, CODE = "KREDIT_CLASSIC" };


            PrismaDaoMock.SetReturnValue("getVertragsart", vart);

            List<IntstrctDto> intStrct = new List<IntstrctDto>{new IntstrctDto{sysintstrct=103, sysprproduct=208, sysintsdate=106, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=104, sysprproduct=209, sysintsdate=107, method=3, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=104, sysprproduct=217, sysintsdate=107, method=3, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=105, sysprproduct=210, sysintsdate=108, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=105, sysprproduct=218, sysintsdate=108, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=106, sysprproduct=216, sysintsdate=109, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=106, sysprproduct=211, sysintsdate=109, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=107, sysprproduct=212, sysintsdate=110, method=3, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=108, sysprproduct=213, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=108, sysprproduct=219, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=109, sysprproduct=232, sysintsdate=112, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=109, sysprproduct=220, sysintsdate=112, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=110, sysprproduct=207, sysintsdate=113, method=1, validFrom=new DateTime(2011, 06, 20)},
                                            new IntstrctDto{sysintstrct=110, sysprproduct=221, sysintsdate=113, method=1, validFrom=new DateTime(2011, 06, 20)} };

            ZinsDaoMock.SetReturnValue("getIntstrct", intStrct);

            List<IntsDto> intsRaten = new List<IntsDto>{ new IntsDto{ sysintsdate=106, intrate=11, addrate=	0, redrate=	0, lowerb=0, upperb=0},
                                                        new IntsDto{ sysintsdate=108, intrate=14.5, addrate=0, redrate=0, lowerb=0, upperb=0},
                                                        new IntsDto{ sysintsdate=109, intrate=10, addrate=0, redrate=0, lowerb=0, upperb=0},
                                                        new IntsDto{ sysintsdate=111, intrate=5.5, addrate=0, redrate=0, lowerb=0, upperb=0},
                                                        new IntsDto{ sysintsdate=112, intrate=6.6, addrate=0, redrate=0, lowerb=0, upperb=0},
                                                        new IntsDto{ sysintsdate=113, intrate=10, addrate=0, redrate=0, lowerb=0, upperb=0} };

            ZinsDaoMock.SetReturnValue("getIntsrate", intsRaten);

            List <PRCLPRINTSETDto>zinsVerf = new List<PRCLPRINTSETDto>();

            ZinsDaoMock.SetReturnValue("getProductLinks", zinsVerf);

            List<PRPROVTYPE> provtypes = new List<PRPROVTYPE>{  new PRPROVTYPE{SYSPRPROVTYPE=1, NAME="Umsatzprovision Neugeld", TYP=0, CODE="P_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=2, NAME="Stückprovision Neugeld", TYP=1, CODE="S_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=8, NAME="Stückprovision RSV", TYP=1, CODE="V_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=21, NAME="Umsatzprovision RSV", TYP=0, CODE="V_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=23, NAME="Umsatzprovision Fremdablöse", TYP=0, CODE="P_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=7, NAME="Teilzuschlagsprovision Neugeld", TYP=0, CODE="P_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=25, NAME="Stückprovision Fremdablöse", TYP=1, CODE="S_Code", SYSFT=0},
                                                                new PRPROVTYPE{SYSPRPROVTYPE=24, NAME="Teilzuschlagsprovision Fremdablöse", TYP=0, CODE="P_Code", SYSFT=0}};

            ProvisionDaoMock.SetReturnValue("getProvisionTypes", provtypes);

            List<long> prFlds = new List<long> { 5, 36, 38, 58, 60, 61, 63 };

            ProvisionDaoMock.SetReturnValue("getProvisionedPrFlds", prFlds);

            List<PRPROVSTEP> provSteps = new List<PRPROVSTEP>{
                new PRPROVSTEP{ SYSPRPROVSTEP=198, SYSPRPROVSET=115, SYSPRFLD=36, RANK=60, SOURCEBASIS=2, METHOD=4, SYSVART=7, PROVRATE=0, ADJUSTMENTTRIGGER=7, SYSVSTYP=13, PROVVAL=80, SYSPRPROVTYPE=8},	
                new PRPROVSTEP{ SYSPRPROVSTEP=205, SYSPRPROVSET=117, SYSPRFLD=36, RANK=60, SOURCEBASIS=2, METHOD=4, SYSVART=7, PROVRATE=0, ADJUSTMENTTRIGGER=2, SYSVSTYP=13, PROVVAL=80, SYSPRPROVTYPE=8},		
                new PRPROVSTEP{ SYSPRPROVSTEP=198, SYSPRPROVSET=115, SYSPRFLD=36, RANK=60, SOURCEBASIS=2, METHOD=4, SYSVART=7, PROVRATE=0, ADJUSTMENTTRIGGER=7, SYSVSTYP=13, PROVVAL=80, SYSPRPROVTYPE=8},		
                new PRPROVSTEP{ SYSPRPROVSTEP=198, SYSPRPROVSET=115, SYSPRFLD=36, RANK=60, SOURCEBASIS=2, METHOD=4, SYSVART=7, PROVRATE=0, ADJUSTMENTTRIGGER=7, SYSVSTYP=13, PROVVAL=80, SYSPRPROVTYPE=8} };		

            ProvisionDaoMock.SetReturnValue("getProvsteps", provSteps);

            List<PrSubvTriggerDto> subvTriggers = new List<PrSubvTriggerDto>{ new PrSubvTriggerDto{ sysprfldtrg=15, sysprproduct=212, sysprsubv=24, trgtype=2}};

            SubventionDaoMock.SetReturnValue("getSubventionTriggers", subvTriggers);

            List<long> GroupList = new List<long> { 123, 124, 156, 167 };
            ObTypDaoMock.SetReturnValue("getPrhGroups", GroupList);

            QuoteDaoMock.SetReturnValue("getQuote", 3.5);

            byte rateError = 0;

            kalkulationBO.calculate(kalkulation, prodCtx, calcKontext, "de-CH", ref rateError);
            

            Assert.AreEqual(kalkulation.angAntKalkDto.rate, 328.2);
            Assert.AreEqual(kalkulation.angAntKalkDto.rateBrutto, 328.2);
            Assert.AreEqual(kalkulation.angAntKalkDto.calcZinskosten, 3253.6);



            //CreditNow
            kalkulation.angAntKalkDto.bginternbrutto = 17000;
            
            
            kalkulation.angAntKalkDto.lz = 60;
            kalkulation.angAntKalkDto.szBrutto = 0;
            kalkulation.angAntKalkDto.rwBrutto = 0;
            kalkulation.angAntKalkDto.sysprproduct = 228;//kredit-now
            
            calcKontext.zinsNominal = 13.9;
            calcKontext.useZinsNominal = true;

            rateError = 0;

            kalkulationBO.calculate(kalkulation, prodCtx, calcKontext, "de-CH", ref rateError);

            Assert.AreEqual(kalkulation.angAntKalkDto.rate, 387.55);
            Assert.AreEqual(kalkulation.angAntKalkDto.rateBrutto, 387.55);
            Assert.AreEqual(kalkulation.angAntKalkDto.calcZinskosten, 6253.00);


            //TZK
            kalkulation.angAntKalkDto.bginternbrutto = 50000;
            kalkulation.angAntKalkDto.lz = 48;
            kalkulation.angAntKalkDto.szBrutto = 0;
            kalkulation.angAntKalkDto.rwBrutto = 15000;
            kalkulation.angAntKalkDto.sysprproduct = 234;//TZK

            calcKontext.zinsNominal = 7.9;
            calcKontext.useZinsNominal = true;

            rateError = 0;

            kalkulationBO.calculate(kalkulation, prodCtx, calcKontext, "de-CH", ref rateError);

            Assert.AreEqual(kalkulation.angAntKalkDto.rate, 943.7);
            Assert.AreEqual(kalkulation.angAntKalkDto.calcUstzins, 0.0);
            Assert.AreEqual(kalkulation.angAntKalkDto.calcZinskosten, 10297.6);

            //Diff Leasing
            kalkulation.angAntKalkDto.bginternbrutto = 75000;

            kalkulation.angAntKalkDto.zinscust = 3.9;
            kalkulation.angAntKalkDto.lz = 48;
            kalkulation.angAntKalkDto.szBrutto = 0;
            kalkulation.angAntKalkDto.rwBrutto = 25000;
            kalkulation.angAntKalkDto.sysprproduct = 233;//leasing

            calcKontext.zinsNominal = 5.9;
            calcKontext.useZinsNominal = true;

            rateError = 0;

            kalkulationBO.calculate(kalkulation, prodCtx, calcKontext, "de-CH", ref rateError);

            Assert.AreEqual(kalkulation.angAntKalkDto.rateBrutto, 1207.95);

            //Diff Leasing Mit SZ
            kalkulation.angAntKalkDto.bginternbrutto = 75000;

            kalkulation.angAntKalkDto.zinscust = 3.9;
            kalkulation.angAntKalkDto.lz = 48;
            kalkulation.angAntKalkDto.szBrutto = 15000;
            kalkulation.angAntKalkDto.rwBrutto = 25000;
            kalkulation.angAntKalkDto.sysprproduct = 233;//leasing

            calcKontext.zinsNominal = 5.9;
            calcKontext.useZinsNominal = true;

            rateError = 0;

            kalkulationBO.calculate(kalkulation, prodCtx, calcKontext, "de-CH", ref rateError);

            Assert.AreEqual(kalkulation.angAntKalkDto.rateBrutto, 869.95);
        }
    }
}
