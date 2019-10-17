using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse für die simpleSetterBo´s
    /// </summary>
    [TestFixture()]
    public class SimpleSetterBoTest
    {
        /// <summary>
        /// simpleSetterDaoMock
        /// </summary>
        public DynamicMock simpleSetterDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// simpleSetterBo
        /// </summary>
        public SimpleSetterBo simpleSetterBo;

        /// <summary>
        /// Initalisierung der generellen Variablen und des Mocks
        /// </summary>
        [SetUp]
        public void SimpleSetterBoTestInit()
        {
            simpleSetterDaoMock = new DynamicMock(typeof(ISimpleSetterDao));
            simpleSetterBo = new SimpleSetterBo((ISimpleSetterDao)simpleSetterDaoMock.MockInstance);
            emptyMessage = new Message();
        }

        /// <summary>
        /// Blackboxtest für die setGeschaeftsart Methode welche momentan ein leeres osetGeschaeftsartDto zurück liefert
        /// </summary>
        [Test]
        public void setGeschaeftsartTest()
        {
            AngebotDto AngebotDto = new AngebotDto();
            //simpleSetterDaoMock.ExpectAndReturn("setGeschaeftsart", RETURNVALUE, INPUT);
            simpleSetterBo.setGeschaeftsart(AngebotDto.sysid);
        }

        /// <summary>
        /// Blackboxtest für die setKontrollschild Methode welche momentan ein leeres osetKontrollschildDto zurück liefert
        /// </summary>
        [Test]
        public void setKontrollschildTest()
        {
            AntragDto AntragDto = new AntragDto();
            //simpleSetterDaoMock.ExpectAndReturn("setKontrollschild", RETURNVALUE, INPUT);
            simpleSetterBo.setKontrollschild(AntragDto.sysid, "Kontrollschild");
        }

        /// <summary>
        /// Blackboxtest für die setStammnummer Methode welche momentan ein leeres osetStammnummerDto zurück liefert
        /// </summary>
        [Test]
        public void setStammnummerTest()
        {
            AntragDto AntragDto = new AntragDto();
            //simpleSetterDaoMock.ExpectAndReturn("setStammnummer", RETURNVALUE, INPUT);
            simpleSetterBo.setStammnummer(AntragDto.sysid, "Stammnummer");
        }

        /// <summary>
        /// Blackboxtest für die setFarbe Methode welche momentan ein leeres osetFarbeDto  zurück liefert
        /// </summary>
        [Test]
        public void setFarbeTest()
        {
            AntragDto AntragDto = new AntragDto();
            //simpleSetterDaoMock.ExpectAndReturn("setFarbe", RETURNVALUE, INPUT);
            simpleSetterBo.setFarbe(AntragDto.sysid, "Farbe");
        }

        /// <summary>
        /// Blackboxtest für die setChassisnummer Methode welche momentan ein leeres osetChassisnummer zurück liefert
        /// </summary>
        [Test]
        public void setChassisnummerTest()
        {
            AntragDto AntragDto = new AntragDto();
            //simpleSetterDaoMock.ExpectAndReturn("setChassisnummer", RETURNVALUE, INPUT);
            simpleSetterBo.setChassisnummer(AntragDto.sysid, "Chassisnummer");
        }

        /// <summary>
        /// Blackboxtest für die setAblieferdatum Methode welches momentan ein leeres osetAblieferdatumDto zurück liefert
        /// </summary>
        [Test]
        public void setAblieferdatumTest()
        {
            AntragDto AntragDto = new AntragDto();
            //simpleSetterDaoMock.ExpectAndReturn("setAblieferdatum", RETURNVALUE, INPUT);
            simpleSetterBo.setAblieferdatum(AntragDto.sysid, new DateTime(2010,10,10));
        }
    }
}
