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

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
{
    /// <summary>
    /// Testklasse für Kremo
    /// </summary>
    [TestFixture()]
    public class KREMOBoTest
    {
        /// <summary>
        /// KREMOWSDaoMock
        /// </summary>
        public DynamicMock KREMOWSDaoMock;
        /// <summary>
        /// KREMODBDaoMock
        /// </summary>
        public DynamicMock KREMODBDaoMock;
        /// <summary>
        /// AuskunftDaoMock
        /// </summary>
        public DynamicMock AuskunftDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// KREMOBo
        /// </summary>
        public KREMOBo KREMOBo;

        /// <summary>
        /// Hier werden alle generellen Objekte und Mocks initalisiert
        /// </summary>
        [SetUp]
        public void dictionaryListsBoTestInit()
        {
            emptyMessage = new Message();
            KREMOWSDaoMock = new DynamicMock(typeof(IKREMOWSDao));
            KREMODBDaoMock = new DynamicMock(typeof(IKREMODBDao));
            AuskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            KREMOBo = new KREMOBo((IKREMOWSDao)KREMOWSDaoMock.MockInstance, (IKREMODBDao)KREMODBDaoMock.MockInstance, (IAuskunftDao)AuskunftDaoMock.MockInstance);
        }

        /// <summary>
        /// Blackboxtest für die callByValue Methode die als Input ein KREMOInDto erhält
        /// </summary>
        [Test]
        public void callByValuesTestWithKREMOInDto()
        {
            KREMOInDto KREMOInDto = new KREMOInDto()
            {
                Anredecode = 1,
                Anredecode2 = 2,
                Anzkind1 = 1,
                Anzkind2 = 2,
                Anzkind3 = 3,
                Anzkind4 = 4,
                Einkbrutto = 10000,
                Einkbrutto2 = 20000,
                Einknetto = 8000,
                Einknetto2 = 16000,
                Famstandcode = 10,
                Famstandcode2 = 20,
                GebDatum = 19801012,
                GebDatum2 = 19801210,
                Glz = 234567,
                Grundcode = 1,
                Kalkcode = 10,
                Kantoncode = 100,
                Kantoncode2 = 200,
                Kreditsumme = 20000,
                Miete = 1000,
                Nebeneinkbrutto = 100000,
                Nebeneinkbrutto2 = 10000,
                Nebeneinknetto = 80000,
                Nebeneinknetto2 = 8000,
                Plz = 2100,
                Plz2 = 2200,
                Qstflag = 100,
                Qstflag2 = 100,
                Rw = 20,
                SysAuskunft = 1337,
                SysKremo = 10,
                Unterhalt = 0,
                Unterhalt2 = 0,
                Zins = 10,
                Zinsnomflag = 1
            };
            AuskunftDaoMock.ExpectAndReturn("SaveAuskunft", (long)1, AuskunfttypDao.KREMOCallByValues);
            KREMODBDaoMock.ExpectAndReturn("SaveKREMOInDto", (long)1, KREMOInDto);
            KREMODBDaoMock.Expect("SaveKREMOInp", 1,1);
            KREMOWSDaoMock.SetReturnValue("CallKremoByValues", (long)100);
            AuskunftDaoMock.Expect("UpdateAuskunft", 1, 100);
            KREMODBDaoMock.Expect("SaveKREMOOutDto");
            KREMOOutDto KREMOOutDto = KREMOBo.callByValues(KREMOInDto).KremoOutDto;
            Assert.AreEqual(100, KREMOOutDto.ReturnCode);
        }

        /// <summary>
        /// Blackboxtest für die callByValues Methode welche als Input ein long SysAuskunf erhält
        /// </summary>
        [Test]
        public void callByValuesTestWithSysAuskunft()
        {
            AuskunftDto AuskunftDto = new AuskunftDto()
            {
                Anfragedatum = new DateTime(2011, 2, 12),
                Anfrageuhrzeit = (long)10.24,
                DecisionEngineInDto = new DecisionEngineInDto(),
                DecisionEngineOutDto = new DecisionEngineOutDto(),
                EurotaxInDto = new EurotaxInDto(),
                EurotaxOutDto = new EurotaxOutDto(),
                Fehlercode = "",
                KremoInDto = new KREMOInDto(),
                KremoOutDto = new KREMOOutDto(),
                Status = "",
                sysAuskunft = (long)1,
                sysAuskunfttyp = (long)1
            };

            KREMOInDto KREMOInDto = new KREMOInDto()
            {
                Anredecode = 1,
                Anredecode2 = 2,
                Anzkind1 = 1,
                Anzkind2 = 2,
                Anzkind3 = 3,
                Anzkind4 = 4,
                Einkbrutto = 10000,
                Einkbrutto2 = 20000,
                Einknetto = 8000,
                Einknetto2 = 16000,
                Famstandcode = 10,
                Famstandcode2 = 20,
                GebDatum = 19801012,
                GebDatum2 = 19801210,
                Glz = 234567,
                Grundcode = 1,
                Kalkcode = 10,
                Kantoncode = 100,
                Kantoncode2 = 200,
                Kreditsumme = 20000,
                Miete = 1000,
                Nebeneinkbrutto = 100000,
                Nebeneinkbrutto2 = 10000,
                Nebeneinknetto = 80000,
                Nebeneinknetto2 = 8000,
                Plz = 2100,
                Plz2 = 2200,
                Qstflag = 100,
                Qstflag2 = 100,
                Rw = 20,
                SysAuskunft = 1337,
                SysKremo = 10,
                Unterhalt = 0,
                Unterhalt2 = 0,
                Zins = 10,
                Zinsnomflag = 1
            };

            AuskunftDaoMock.ExpectAndReturn("FindBySysId", AuskunftDto, (long)1);
            KREMODBDaoMock.ExpectAndReturn("FindBySysId", KREMOInDto, (long)1);
            KREMODBDaoMock.Expect("SaveKREMOInp", 1, 1);
            KREMOWSDaoMock.SetReturnValue("CallKremoByValues", (long)100);
            AuskunftDaoMock.Expect("UpdateAuskunft", 1, 100);
            KREMODBDaoMock.Expect("SaveKREMOOutDto");
            AuskunftDto AuskunftOutDto = KREMOBo.callByValues((long)1);
            Assert.AreEqual(1, AuskunftOutDto.sysAuskunft);
            Assert.AreEqual("", AuskunftOutDto.Status);
            Assert.AreEqual(100000, AuskunftOutDto.KremoInDto.Nebeneinkbrutto);
            Assert.AreEqual(0, AuskunftOutDto.KremoOutDto.SysKremo);
        }

        /// <summary>
        /// Blackboxtest für die getVersion Methode welche als Input ein KREMOInDto erhält
        /// </summary>
        [Test]
        public void getVersionTest()
        {
            KREMOInDto KREMOInDto = new KREMOInDto()
            {
                Anredecode = 1,
                Anredecode2 = 2,
                Anzkind1 = 1,
                Anzkind2 = 2,
                Anzkind3 = 3,
                Anzkind4 = 4,
                Einkbrutto = 10000,
                Einkbrutto2 = 20000,
                Einknetto = 8000,
                Einknetto2 = 16000,
                Famstandcode = 10,
                Famstandcode2 = 20,
                GebDatum = 19801012,
                GebDatum2 = 19801210,
                Glz = 234567,
                Grundcode = 1,
                Kalkcode = 10,
                Kantoncode = 100,
                Kantoncode2 = 200,
                Kreditsumme = 20000,
                Miete = 1000,
                Nebeneinkbrutto = 100000,
                Nebeneinkbrutto2 = 10000,
                Nebeneinknetto = 80000,
                Nebeneinknetto2 = 8000,
                Plz = 2100,
                Plz2 = 2200,
                Qstflag = 100,
                Qstflag2 = 100,
                Rw = 20,
                SysAuskunft = 1337,
                SysKremo = 10,
                Unterhalt = 0,
                Unterhalt2 = 0,
                Zins = 10,
                Zinsnomflag = 1
            };

            KREMOWSDaoMock.SetReturnValue("CallKremoGetVersion", "1.3.3.7");
            KREMOOutDto KREMOOutDto = KREMOBo.getVersion(KREMOInDto);
            Assert.AreEqual("1.3.3.7", KREMOOutDto.Version);
        }
    }
}
