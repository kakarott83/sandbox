using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Diese Klasse testet alle dictionaryListsBo Methoden auf korrekte Arbeitsweise.
    /// </summary>
    [TestFixture()]
    public class DictionaryListsBoTest
    {
        /// <summary>
        /// dictionaryListsDaoMock
        /// </summary>
        public DynamicMock dictionaryListsDaoMock;
        /// <summary>
        /// translateDaoMock
        /// </summary>
        public DynamicMock translateDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Message emptyMessage;
        /// <summary>
        /// dictionaryListsBo
        /// </summary>
        public DictionaryListsBo dictionaryListsBo;

        /// <summary>
        /// Hier werden alle generellen Objekte und Mocks initalisiert
        /// </summary>
        [SetUp]
        public void dictionaryListsBoTestInit()
        {
            emptyMessage = new Cic.OpenOne.GateBANKNOW.Common.DTO.Message();
            dictionaryListsDaoMock = new DynamicMock(typeof(IDictionaryListsDao));
            translateDaoMock = new DynamicMock(typeof(ITranslateDao));
            dictionaryListsBo = new DictionaryListsBo((IDictionaryListsDao)dictionaryListsDaoMock.MockInstance, (ITranslateDao)translateDaoMock.MockInstance, "de-CH");
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der Liste mit Anreden.
        /// </summary>
        [Test]
        public void listAnredenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                    
                        sysID = 1L,
                        beschreibung = "Geschlechtsneutrale Anrede",
                        bezeichnung = "Sehr geehrte Damen und Herren"
                    },
                    new DropListDto()
                    {
                      
                        sysID = 2L,
                        beschreibung = "Anrede Frauen",
                        bezeichnung = "Sehr geehrte Frau"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3L,
                        beschreibung = "Anrede Herren",
                        bezeichnung = "Sehr geehrter Herr"
                    }
                };

            dictionaryListsDaoMock.ExpectAndReturn("findByDDLKPPOSCode", DropListDto, DDLKPPOSType.ANREDEN, "de-CH", DDLKPPOSDomain.KURZ.ToString() );
            DropListDto[] olistAnredenDto = dictionaryListsBo.listAnreden();
            Assert.AreEqual("Sehr geehrte Damen und Herren", olistAnredenDto[0].bezeichnung);
            Assert.AreEqual("Anrede Frauen", olistAnredenDto[1].beschreibung);
            Assert.AreEqual(3UL, olistAnredenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der Liste mit Ländern
        /// </summary>
        [Test]
        public void listLaenderTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "CH",
                        bezeichnung = "Schweiz"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2L,
                        beschreibung = "DE",
                        bezeichnung = "Deutschland"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3L,
                        beschreibung = "AT",
                        bezeichnung = "Österreich"
                    }
                };


            dictionaryListsDaoMock.SetReturnValue("deliverLAND", DropListDto);
            translateDaoMock.SetReturnValue("TranslateList", DropListDto);
            DropListDto[] olistLaenderDto = dictionaryListsBo.listLaender();
            Assert.AreEqual("Schweiz", olistLaenderDto[0].bezeichnung);
            Assert.AreEqual("DE", olistLaenderDto[1].beschreibung);
            Assert.AreEqual(3UL, olistLaenderDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der Liste mit Kantonen
        /// </summary>
        [Test]
        public void listKantoneTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "-",
                        bezeichnung = "AG"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2L,
                        beschreibung = "-",
                        bezeichnung = "BE"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3L,
                        beschreibung = "-",
                        bezeichnung = "ZH"
                    }
                };

            dictionaryListsDaoMock.SetReturnValue("deliverSTAAT", DropListDto);
            translateDaoMock.SetReturnValue("TranslateList", DropListDto);
            DropListDto[] olistKantoneDto = dictionaryListsBo.listKantone("de-CH");
            Assert.AreEqual("-", olistKantoneDto[1].beschreibung);
            Assert.AreEqual(3UL, olistKantoneDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Liste mit Beruflichen Situationen
        /// </summary>
        [Test]
        public void listBeruflicheSituationenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                        
                        sysID = 1L,
                        beschreibung = "Angestellte Beschäftigung",
                        bezeichnung = "Angestellt"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2L,
                        beschreibung = "Selbständige Beschäftigung",
                        bezeichnung = "Selbständig"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3L,
                        beschreibung = "Keine Beschäftigung",
                        bezeichnung = "Arbeitslos"
                    }
                };

            dictionaryListsDaoMock.ExpectAndReturn("findByDDLKPPOSCode", DropListDto, DDLKPPOSType.BERUFLICHESIT, "de-CH");
            DropListDto[] olistBeruflicheSituationenDto = dictionaryListsBo.listBeruflicheSituationen();
            Assert.AreEqual("Angestellt", olistBeruflicheSituationenDto[0].bezeichnung);
            Assert.AreEqual("Selbständige Beschäftigung", olistBeruflicheSituationenDto[1].beschreibung);
            Assert.AreEqual(3UL, olistBeruflicheSituationenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Liste mit Sprachen
        /// </summary>
        [Test]
        public void listSprachenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "de-CH",
                        bezeichnung = "Deutsch"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2L,
                        beschreibung = "fr-CH",
                        bezeichnung = "Französisch"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3L,
                        beschreibung = "it-CH",
                        bezeichnung = "Italienisch"
                    }
                };

            dictionaryListsDaoMock.SetReturnValue("deliverCTLANG", DropListDto);
            translateDaoMock.SetReturnValue("TranslateList", DropListDto);
            DropListDto[] olistSprachenDto = dictionaryListsBo.listSprachen();
            Assert.AreEqual("Deutsch", olistSprachenDto[0].bezeichnung);
            Assert.AreEqual("fr-CH", olistSprachenDto[1].beschreibung);
            Assert.AreEqual(3UL, olistSprachenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Liste mit Wohnsituationen
        /// </summary>
        [Test]
        public void listWohnSituationenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "Wohnsituation Alleinstehend",
                        bezeichnung = "Alleinstehend"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2L,
                        beschreibung = "Wohnsituation Ehepaar",
                        bezeichnung = "Ehepaar"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3L,
                        beschreibung = "Wohnsituation zusammenlebend",
                        bezeichnung = "Zusammenlebend"
                    }
                };

            dictionaryListsDaoMock.ExpectAndReturn("findByDDLKPPOSCode", DropListDto, DDLKPPOSType.WOHNSITUATIONEN, "de-CH");
            DropListDto[] olistWohnSituationenDto = dictionaryListsBo.listWohnSituationen();
            Assert.AreEqual("Alleinstehend", olistWohnSituationenDto[0].bezeichnung);
            Assert.AreEqual("Wohnsituation Ehepaar", olistWohnSituationenDto[1].beschreibung);
            Assert.AreEqual(3UL, olistWohnSituationenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Liste mit Zivilständen
        /// </summary>
        [Test]
        public void listZivilstaendeTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "Zivilstand ledig",
                        bezeichnung = "Ledig"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2L,
                        beschreibung = "Zivilstand verheiratet",
                        bezeichnung = "Verheiratet"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3L,
                        beschreibung = "Zivilstand geschieden",
                        bezeichnung = "Geschieden"
                    }
                };

            dictionaryListsDaoMock.ExpectAndReturn("findByDDLKPPOSCode", DropListDto, DDLKPPOSType.ZIVILSTAENDE, "de-CH");
            DropListDto[] olistZivilstaendeDto = dictionaryListsBo.listZivilstaende();
            Assert.AreEqual("Ledig", olistZivilstaendeDto[0].bezeichnung);
            Assert.AreEqual("Zivilstand verheiratet", olistZivilstaendeDto[1].beschreibung);
            Assert.AreEqual(3UL, olistZivilstaendeDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Liste mit Nationaliäten
        /// </summary>
        [Test]
        public void listNationalitaetenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1L,
                        beschreibung = "-",
                        bezeichnung = "Schweiz"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2L,
                        beschreibung = "-",
                        bezeichnung = "Deutschland"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3L,
                        beschreibung = "-",
                        bezeichnung = "Österreich"
                    }
                };

            dictionaryListsDaoMock.ExpectAndReturn("findByDDLKPPOSCode", DropListDto, DDLKPPOSType.NATIONALITAETEN, "de-CH");
            translateDaoMock.SetReturnValue("TranslateList", DropListDto);
            DropListDto[] olistNationalitaetenDto = dictionaryListsBo.listNationalitaeten();
            Assert.AreEqual("Schweiz", olistNationalitaetenDto[0].bezeichnung);
            Assert.AreEqual("-", olistNationalitaetenDto[1].beschreibung);
            Assert.AreEqual(3UL, olistNationalitaetenDto[2].sysID);
        }
    }
}
