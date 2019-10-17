using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;

using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse für das roleContextListsBo
    /// </summary>
    [TestFixture()]
    public class RoleContextListsBoTest
    {
        
        /// <summary>
        /// roleContextListsDaoMock
        /// </summary>
        public DynamicMock roleContextListsDaoMock;

        /// <summary>
        /// roleContextListsDaoMock
        /// </summary>
        public DynamicMock TranslateDaoMock;

        /// <summary>
        /// roleContextListBo
        /// </summary>
        public RoleContextListsBo roleContextListBo;

        /// <summary>
        /// Initialisiert die generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void roleContextListsBoTestInit()
        {
            roleContextListsDaoMock = new DynamicMock(typeof(IRoleContextListsDao));
            TranslateDaoMock = new DynamicMock(typeof(ITranslateDao));
            roleContextListBo = new RoleContextListsBo((IRoleContextListsDao)roleContextListsDaoMock.MockInstance, (ITranslateDao)TranslateDaoMock.MockInstance);
          

        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Alerts als Liste
        /// </summary>
        [Test]
        public void listAvailableAlertsTest()
        {
            AvailableAlertsDto[] AlertList = new AvailableAlertsDto[]
                {
                    new AvailableAlertsDto()
                    {
                       
                        sysID = 1U,
                        kunde = "Testkunde1",
                    }
                };
            roleContextListsDaoMock.SetReturnValue("listAvailableAlerts", AlertList);
            AvailableAlertsDto[] AvailableAlertsDto = roleContextListBo.listAvailableAlerts("ch-DE", 10);
            Assert.AreEqual("Testkunde1", AvailableAlertsDto[0].kunde);
       }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Brands als Liste
        /// </summary>
        [Test]
        public void listAvailableBrandsTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Cobrand BANK-now",
                        bezeichnung = "BANK-now"
                    },
                    new DropListDto()
                    {
                      
                        sysID = 2U,
                        beschreibung = "Cobrand Porsche",
                        bezeichnung = "Porsche"  
                    },
                    new DropListDto()
                    {
                      
                        sysID = 3U,
                        beschreibung = "Cobrand Hyundai",
                        bezeichnung = "Hyundai"
                    }
                };

            roleContextListsDaoMock.ExpectAndReturn("getBrands", DropListDto, 0);
            DropListDto[] AvailableBrandsDto = roleContextListBo.listAvailableBrands(0);
            Assert.AreEqual("BANK-now", AvailableBrandsDto[0].bezeichnung);
            Assert.AreEqual("Cobrand Porsche", AvailableBrandsDto[1].beschreibung);
            Assert.AreEqual(3UL, AvailableBrandsDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Channels als Liste
        /// </summary>
        [Test]
        public void listAvailableChannelsTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                        
                        sysID = 1U,
                        beschreibung = "Fahrzeugfinanzierung",
                        bezeichnung = "FF"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Kreditfinanzierung",
                        bezeichnung = "KF"  
                    },
                };

            DropListDto[] OutDropList;

            OutDropList = new DropListDto[]
                {
                    new DropListDto()
                    {
                        
                        sysID = 1U,
                        beschreibung = "Fahrzeugfinanzierung",
                        bezeichnung = "FF"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Kreditfinanzierung",
                        bezeichnung = "KF"  
                    },
                };

            TranslateDaoMock.ExpectAndReturn("readoutTranslationList", 2, "BCHANNEL", "ch-DE");
            TranslateDaoMock.ExpectAndReturn("TranslateList", OutDropList, DropListDto, "BCHANNEL");
            roleContextListsDaoMock.ExpectAndReturn("getChannels", DropListDto, 0);
            DropListDto[] AvailableChannelsDto = roleContextListBo.listAvailableChannels(0, "ch-DE");
            Assert.AreEqual("FF", AvailableChannelsDto[0].bezeichnung);
            Assert.AreEqual("Kreditfinanzierung", AvailableChannelsDto[1].beschreibung);
            Assert.AreEqual(2UL, AvailableChannelsDto[1].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Kundentypen als Liste
        /// </summary>
        [Test]
        public void listAvailableKundentypenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Private Kunden",
                        bezeichnung = "Privat"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Gewerbliche Kunden",
                        bezeichnung = "Gewerblich"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3U,
                        beschreibung = "Selbständige Kunden",
                        bezeichnung = "Selbständig"
                    }
                };

            DropListDto[] OutDropList;

            OutDropList = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Private Kunden",
                        bezeichnung = "Privat"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Gewerbliche Kunden",
                        bezeichnung = "Gewerblich"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3U,
                        beschreibung = "Selbständige Kunden",
                        bezeichnung = "Selbständig"
                    }
                };

            TranslateDaoMock.ExpectAndReturn("readoutTranslationList", 3, "KDTYP", "ch-DE");
            TranslateDaoMock.ExpectAndReturn("TranslateList", OutDropList, DropListDto, "KDTYP");
            roleContextListsDaoMock.SetReturnValue("getKundentypen", DropListDto);
            DropListDto[] AvailableKundentypenDto = roleContextListBo.listAvailableKundentypen("ch-DE");
            Assert.AreEqual("Privat", AvailableKundentypenDto[0].bezeichnung);
            Assert.AreEqual("Gewerbliche Kunden", AvailableKundentypenDto[1].beschreibung);
            Assert.AreEqual(3UL, AvailableKundentypenDto[2].sysID);
        }

        

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Nutzungsarten als Liste
        /// </summary>
        [Test]
        public void listAvailableNutzungsartenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Private Nutzung",
                        bezeichnung = "Privat"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2U,
                        beschreibung = "Gewerbliche Nutzung",
                        bezeichnung = "Gewerblich"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3U,
                        beschreibung = "Demoleasing Nutzung (Vorführfahrzeug)",
                        bezeichnung = "Demo"
                    }
                };

            DropListDto[] OutDropList;
            OutDropList = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Private Nutzung",
                        bezeichnung = "Privat"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2U,
                        beschreibung = "Gewerbliche Nutzung",
                        bezeichnung = "Gewerblich"  
                    },
                    new DropListDto()
                    {
                       
                        sysID = 3U,
                        beschreibung = "Demoleasing Nutzung (Vorführfahrzeug)",
                        bezeichnung = "Demo"
                    }
                };

            TranslateDaoMock.ExpectAndReturn("readoutTranslationList", 3, "OBUSETYPE", "ch-DE");
            TranslateDaoMock.ExpectAndReturn("TranslateList", OutDropList, DropListDto, "OBUSETYPE");
            roleContextListsDaoMock.SetReturnValue("getNutzungsarten", DropListDto);
            DropListDto[] AvailableNutzungsartenDto = roleContextListBo.listAvailableNutzungsarten("ch-DE");
            Assert.AreEqual("Privat", AvailableNutzungsartenDto[0].bezeichnung);
            Assert.AreEqual("Gewerbliche Nutzung", AvailableNutzungsartenDto[1].beschreibung);
            Assert.AreEqual(3UL, AvailableNutzungsartenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Objektarten als Liste
        /// </summary>
        [Test]
        public void listAvailableObjektartenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                        
                        sysID = 1U,
                        beschreibung = "Neuwagen",
                        bezeichnung = "Neu"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Gebrauchtwagen / Occasion",
                        bezeichnung = "Gebraucht"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3U,
                        beschreibung = "Oldtimer / Liebhaberfahrzeug",
                        bezeichnung = "Liebhaber"
                    }
                };

            DropListDto[] OutDropList;

            OutDropList = new DropListDto[]
                {
                    new DropListDto()
                    {
                        
                        sysID = 1U,
                        beschreibung = "Neuwagen",
                        bezeichnung = "Neu"
                    },
                    new DropListDto()
                    {
                       
                        sysID = 2U,
                        beschreibung = "Gebrauchtwagen / Occasion",
                        bezeichnung = "Gebraucht"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3U,
                        beschreibung = "Oldtimer / Liebhaberfahrzeug",
                        bezeichnung = "Liebhaber"
                    }
                };

            TranslateDaoMock.ExpectAndReturn("readoutTranslationList", 3, "OBART", "ch-DE");
            TranslateDaoMock.ExpectAndReturn("TranslateList", OutDropList, DropListDto, "OBART");
            roleContextListsDaoMock.SetReturnValue("getObjektarten", DropListDto);
            DropListDto[] AvailableObjektartenDto = roleContextListBo.listAvailableObjektarten("ch-DE");
            Assert.AreEqual("Neu", AvailableObjektartenDto[0].bezeichnung);
            Assert.AreEqual("Gebrauchtwagen / Occasion", AvailableObjektartenDto[1].beschreibung);
            Assert.AreEqual(3UL, AvailableObjektartenDto[2].sysID);
        }

        /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren Objekttypen als Liste
        /// </summary>
        [Test]
        public void listAvailableObjekttypenTest()
        {
            DropListDto[] DropListDto;

            DropListDto = new DropListDto[]
                {
                    new DropListDto()
                    {
                       
                        sysID = 1U,
                        beschreibung = "Personenwagen",
                        bezeichnung = "PW"
                    },
                    new DropListDto()
                    {
                        
                        sysID = 2U,
                        beschreibung = "Kleinlastwagen",
                        bezeichnung = "LCV"  
                    },
                    new DropListDto()
                    {
                        
                        sysID = 3U,
                        beschreibung = "Motorrad",
                        bezeichnung = "Moto"
                    }
                };

            roleContextListsDaoMock.SetReturnValue("getObjekttypen", DropListDto);
            DropListDto[] AvailableObjekttypenDto = roleContextListBo.listAvailableObjekttypen("de-CH");
            Assert.AreEqual("PW", AvailableObjekttypenDto[0].bezeichnung);
            Assert.AreEqual("Kleinlastwagen", AvailableObjekttypenDto[1].beschreibung);
            Assert.AreEqual(3UL, AvailableObjekttypenDto[2].sysID);
        }
    }
}
