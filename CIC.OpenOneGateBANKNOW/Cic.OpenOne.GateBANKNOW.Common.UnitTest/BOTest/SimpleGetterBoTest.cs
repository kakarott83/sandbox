using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse des SimpleGetterBo´s
    /// </summary>
    [TestFixture()]
    public class SimpleGetterBoTest
    {
        /// <summary>
        /// SimpleGetterDaoMock
        /// </summary>
        //public DynamicMock SimpleGetterDaoMock;
        public DynamicMock SimpleGetterDaoMock = new DynamicMock(typeof(ISimpleGetterDao));
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// simpleGetterBo
        /// </summary>
        public SimpleGetterBo simpleGetterBo;

        /// <summary>
        /// Initialisiert alle generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void SimpleGetterBoTestInit()
        {
            //SimpleGetterDaoMock = new DynamicMock(typeof(ISimpleGetterDao));
            simpleGetterBo = new SimpleGetterBo((ISimpleGetterDao)SimpleGetterDaoMock.MockInstance );
            emptyMessage = new Message();
        }
        
        /// <summary>
        /// Blackboxtest für die getProfile Methode welche ein ogetProfileDto zurück liefert
        /// </summary>
        [Test]
        public void getProfileTest()
        {
            PUSER puser = new PUSER();
            PERSON person = new PERSON();

            person.NAME = "Musterhändler";
            person.VORNAME = "Franz";
            person.STRASSE = "Musterstrasse";
            person.HSNR = "1";
            person.PLZ = "4711";
            person.ORT = "Köln";
            person.TELEFON = "0123456789";
            person.FAX = "0123456789";
            person.EMAIL = "franz@musterhaendler.de";
            person.URL = "musterhaendler.de";
            person.SYSPERSON.ToString();
            person.HANDY = "1234567";

            puser.USERID = "0";
            puser.EXTERNEID = "0";

            SimpleGetterDaoMock.ExpectAndReturn("findPersonBySysperole", person, 0);
            SimpleGetterDaoMock.ExpectAndReturn("getPuser", puser, person);
            ProfilDto ogetProfileDto = simpleGetterBo.getProfil(0); // TODO sysVpPerole
            
            Assert.AreEqual("Musterhändler", ogetProfileDto.name);
            Assert.AreEqual("0", ogetProfileDto.benutzerId);
        }

        /// <summary>
        /// Blackboxtest für die getKam Methode welche ein ogetKamDto zurück liefert
        /// </summary>
        [Test]
        public void getKamTest()
        {
            PERSON person = new PERSON();

            person.NAME = "Muster Key Account Manager";
            person.VORNAME = "Franz";
            person.TELEFON = "0123456789";

            SimpleGetterDaoMock.ExpectAndReturn("findKamPersonBySysperole", person, 0);
            KamDto KamDto = simpleGetterBo.getKam(0);
            Assert.AreEqual("Muster Key Account Manager", KamDto.name);
        }

        /// <summary>
        /// Blackboxtest für die getAbwicklungsort Methode welche ein ogetAbwicklungsortDto zurück liefert
        /// </summary>
        [Test]
        public void getAbwicklungsortTest()
        {
            PERSON person = new PERSON();


            person.HSNR = "1";
            person.STRASSE = "Musterstrasse";
            person.PLZ = "4711";
            person.ORT = "Köln";
            person.TELEFON = "0123456789";

            SimpleGetterDaoMock.ExpectAndReturn("findAbwicklungsortPersonBySysperole", person, 0);
            AbwicklungsortDto AbwicklungsortDto = simpleGetterBo.getAbwicklungsort(0);
            Assert.AreEqual("1", AbwicklungsortDto.hausnummer);
        } 

    }
}