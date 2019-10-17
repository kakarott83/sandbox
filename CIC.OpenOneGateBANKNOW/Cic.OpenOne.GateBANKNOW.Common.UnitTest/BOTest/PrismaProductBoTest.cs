using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse für PrismaBo
    /// </summary>
    [TestFixture()]
    public class PrismaProductBoTest
    {
        DynamicMock PrismaDaoMock;
        DynamicMock ObTypDaoMock;
        DynamicMock transDaoMock;
        PrismaProductBo bo;
        List<CIC.Database.PRISMA.EF6.Model.PRPRODUCT> allProduct;
        String isoCode = "de-CH";

        List<long> prhGroups;
        List<ProductConditionLink> allConditionsHg;
        List<ProductConditionLink> allConditionsBr;
        List<ProductConditionLink> allConditionsChnl;
        List<ProductConditionLink> allConditionsOb;
        List<ProductConditionLink> allConditionsObart;
        List<ProductConditionLink> allConditionsUsetype;
        List<ProductConditionLink> allConditionsPrktyp;
        List<ProductConditionLink> allConditionsKg;

        /// <summary>
        /// Initialisiert alle generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void PrismaProductBoTestInit()
        {

            PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            transDaoMock = new DynamicMock(typeof(ITranslateDao));

            bo = new PrismaProductBo((IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, (ITranslateDao)transDaoMock.MockInstance, PrismaProductBo.CONDITIONS_BANKNOW, isoCode);

            allProduct = new List<PRPRODUCT>{
                new PRPRODUCT { SYSPRPRODUCT=207, SYSVART=3, NAME="6.1 KF CREDIT-now Classic",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=110, SYSPRRAP=44, SYSPRPRODTYPE=1, NAMEINTERN="6.1 KF CREDIT-now Classic"},
                new PRPRODUCT { SYSPRPRODUCT=208, SYSVART=3, NAME="6.2 KF CREDIT-now Classic mit aufgeschobener Ratenfälligkeit",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=103, SYSPRRAP=24, SYSPRPRODTYPE=1, NAMEINTERN="6.2 KF CREDIT-now Classic mit aufgeschobener Ratenfälligkeit"},
                new PRPRODUCT { SYSPRPRODUCT=209, SYSVART=7, NAME="6.3 KF CREDIT-now Dispo",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=104, SYSPRRAP=25, SYSPRPRODTYPE=1, NAMEINTERN="6.3 KF CREDIT-now Dispo"},
                new PRPRODUCT { SYSPRPRODUCT=210, SYSVART=4, NAME="6.4 KF CREDIT-now Express",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=105, SYSPRPRODTYPE=1, NAMEINTERN="6.4 KF CREDIT-now Express"},
                new PRPRODUCT { SYSPRPRODUCT=211, SYSVART=1, NAME="7.1 FF Leasing",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=106, SYSPRPRODTYPE=1, NAMEINTERN="7.1 FF Leasing"},
                new PRPRODUCT { SYSPRPRODUCT=212, SYSVART=1, NAME="7.2 Differenzleasing Betragsabhängig SUB",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=107, SYSPRPRODTYPE=1, NAMEINTERN="7.2 Differenzleasing Betragsabhängig"},
                new PRPRODUCT { SYSPRPRODUCT=213, SYSVART=5, NAME="7.3 FF CREDIT-now Car-/Yacht-/Motofinance (Teilzahlungskaufvertrag)",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=108, SYSPRPRODTYPE=1, NAMEINTERN="7.3 FF CREDIT-now Car-/Yacht-/Motofinance (Teilzahlungskaufvertrag)"},
                new PRPRODUCT { SYSPRPRODUCT=214, SYSVART=3, NAME="7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=109, SYSPRPRODTYPE=1, NAMEINTERN="7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)"},
                new PRPRODUCT { SYSPRPRODUCT=216, SYSVART=1, NAME="7.1 FF Leasing_Schnellkalk	Schnellkalkulation Leasing",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=106, SYSPRPRODTYPE=3, NAMEINTERN="Schnellkalkulation Leasing"},
                new PRPRODUCT { SYSPRPRODUCT=217, SYSVART=7, NAME="6.3 KF CREDIT-now Dispo_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=104, SYSPRPRODTYPE=3, NAMEINTERN="Schnellkalkulation Dispo"},
                new PRPRODUCT { SYSPRPRODUCT=218, SYSVART=4, NAME="6.4 KF CREDIT-now Express_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=105, SYSPRPRODTYPE=3, NAMEINTERN="Schnellkalk Express"},
                new PRPRODUCT { SYSPRPRODUCT=219, SYSVART=5, NAME="7.3 FF CREDIT-now Car-/Yacht-/Motofinance (Teilzahlungskaufvertrag)_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=108, SYSPRPRODTYPE=3, NAMEINTERN="Motofinance Schnellkalkulation"},
                new PRPRODUCT { SYSPRPRODUCT=220, SYSVART=3, NAME="7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=109, SYSPRPRODTYPE=3, NAMEINTERN="Classic FF Schnellkalkulation"},
                new PRPRODUCT { SYSPRPRODUCT=221, SYSVART=3, NAME="6.1 KF CREDIT-now Classic_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=110, SYSPRRAP=44, SYSPRPRODTYPE=3, NAMEINTERN="Schnellkalkulation Classic"},
                new PRPRODUCT { SYSPRPRODUCT=222, SYSVART=1, NAME="7.2.5 Differenzleasing Betragsabhängig",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=111, SYSPRPRODTYPE=1, NAMEINTERN="7.2.5."},
                new PRPRODUCT { SYSPRPRODUCT=223, SYSVART=3, NAME="6.1 TEST KF CREDIT-now Classic_Copy für FF<>KF Testfall",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=109, SYSPRPRODTYPE=1, NAMEINTERN="Testfall FF<>KF"},
                new PRPRODUCT { SYSPRPRODUCT=224, SYSVART=1, NAME="7.1 Test LZ-Abhängig FF Leasing	Test LZ-Zinsstruktur",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=113, SYSPRPRODTYPE=1, NAMEINTERN="SAS Test"},
                new PRPRODUCT { SYSPRPRODUCT=225, SYSVART=3, NAME="6.1 Test VG KF CREDIT-now Classic",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=2, SYSVG=64, SYSPRPRODTYPE=1, NAMEINTERN=""},
                new PRPRODUCT { SYSPRPRODUCT=226, SYSVART=4, NAME="T_RH_Demo_Kredit",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,08,16), VALIDUNTIL=new DateTime(2011,08,31), SOURCEBASIS=0, SYSPRPRODTYPE=1, NAMEINTERN="asfjaslöfjasfklj"},
                new PRPRODUCT { SYSPRPRODUCT=227, SYSVART=8, NAME="7.3  PLUS FF CREDIT-now Carfinance Plus",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=108, SYSPRPRODTYPE=1, NAMEINTERN="Test TZK+"},
                new PRPRODUCT { SYSPRPRODUCT=228, SYSVART=8, NAME="7.3  PLUS FF CREDIT-now Carfinance Plus_Schnellkalk",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=108, SYSPRPRODTYPE=3, NAMEINTERN="Test TZK + Schnell"},
                new PRPRODUCT { SYSPRPRODUCT=247, SYSVART=1, NAME="7.1 FF Leasing_Porsche",  ACTIVEFLAG=1, VALIDFROM=new DateTime(2011,06, 27), SOURCEBASIS=1, SYSINTSTRCT=106, SYSPRPRODTYPE=1}				
            };



            PrismaDaoMock.SetReturnValue("getProducts", allProduct);

            prhGroups = new List<long> { 208, 124, 156, 167 };
            ObTypDaoMock.SetReturnValue("getPrhGroups", prhGroups);

            allConditionsHg = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ SYSPRPRODUCT=212, sysprhgroup=209, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=216, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=211, sysprhgroup=209, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=219, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=213, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=212, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=220, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=207, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=209, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=210, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=211, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=221, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=207, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=208, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=209, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=210, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=217, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=218, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=221, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=222, sysprhgroup=209, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=222, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=222, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=223, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=223, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=224, sysprhgroup=209, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=224, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=224, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=227, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=227, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=228, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=228, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=208, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=247, sysprhgroup=191, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=225, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=225, sysprhgroup=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=214, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=214, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=217, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=218, sysprhgroup=189, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=219, sysprhgroup=190, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=212, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=211, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=213, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=216, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=220, sysprhgroup=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=226, sysprhgroup=212, ACTIVEFLAG=1, VALIDFROM=new DateTime(2011, 08, 21), VALIDUNTIL=new DateTime(2011, 08, 31)},
                new ProductConditionLink(){ SYSPRPRODUCT=226, sysprhgroup=211, ACTIVEFLAG=1, VALIDUNTIL=new DateTime(2011, 08, 11)},
                new ProductConditionLink(){ SYSPRPRODUCT=207, sysprhgroup=213, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=221, sysprhgroup=213, ACTIVEFLAG=1},
                new ProductConditionLink(){ SYSPRPRODUCT=208, sysprhgroup=213, ACTIVEFLAG=1}
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsHg, "prclprhg");

            allConditionsBr = new List<ProductConditionLink>();
            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsBr, "prclprbr");

            allConditionsChnl = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=216, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=207, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=208, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=209, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=210, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=211, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=212, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=213, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=214, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=221, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=222, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=223, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=224, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=227, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=228, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=247, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=225, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=217, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=218, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=219, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=1, SYSPRPRODUCT=220, ACTIVEFLAG=1},
                new ProductConditionLink(){ sysbchannel=2, SYSPRPRODUCT=226, ACTIVEFLAG=1}
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsChnl, "prclprbchnl");

            allConditionsObart = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ sysobart=12, SYSPRPRODUCT=226, ACTIVEFLAG=1, VALIDUNTIL=new DateTime(2011, 08, 11)},
                new ProductConditionLink(){ sysobart=13, SYSPRPRODUCT=226, ACTIVEFLAG=1, VALIDFROM=new DateTime(2011, 08, 21), VALIDUNTIL=new DateTime(2011, 08, 31)}
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsObart, "prclprobart");

            allConditionsUsetype = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ sysobusetype=21, SYSPRPRODUCT=226, ACTIVEFLAG=1},
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsUsetype, "prclprusetype");

            allConditionsPrktyp = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ syskdtyp=1, SYSPRPRODUCT=207, ACTIVEFLAG=1, VALIDFROM=new DateTime(2002, 11, 30), VALIDUNTIL=new DateTime(2002, 11, 30)},
                new ProductConditionLink(){ syskdtyp=1, SYSPRPRODUCT=225, ACTIVEFLAG=1, VALIDFROM=new DateTime(2002, 11, 30), VALIDUNTIL=new DateTime(2002, 11, 30)}
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsPrktyp, "prclprktyp");

            allConditionsOb = new List<ProductConditionLink>()
            {
                new ProductConditionLink(){ SYSPRPRODUCT=247, sysobtyp=24536, ACTIVEFLAG=1}		
            };

            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsOb, "prclprob");

            allConditionsKg = new List<ProductConditionLink>();
            PrismaDaoMock.ExpectAndReturn("getProductConditionLinks", allConditionsKg, "prclprkg");







        }
       /* /// <summary>
        /// Blackboxtest für die Ausgabe der verfügbaren News als Liste
        /// TODO
        /// </summary>
        [Test]
        public void listAvailableNewsTest()
        {

            IPrismaDao pDao = new PrismaDao();
            IObTypDao obDao = new ObTypDao();
            DynamicMock PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            DynamicMock ObTypDaoMock = new DynamicMock(typeof(IObTypDao));

            PrismaNewsBo bo = new PrismaNewsBo((IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, PrismaNewsBo.CONDITIONS_BANKNOW, "de-CH");

            prKontextDto input = new prKontextDto();

            input.sysbrand = 1;
            input.sysprchannel = 5;
            input.sysprhgroup = 3;

            List<PRNEWS> inputnews = new List<PRNEWS>()
            {
                new PRNEWS()
                {
                    CATCHWORDS = "News",
                    DATUM = DateTime.Now,
                    SYSPRNEWS = 1,
                    UHRZEIT = 121212,
                    VALIDFROM = new DateTime(2010,01,01),
                    VALIDUNTIL = new DateTime(2012,01,01),
                }
            };


            /*
               PrismaDaoMock.SetReturnValue("getProducts", allProduct);
               PrismaDaoMock.SetReturnValue("getProductConditionLinks", allConditions);
               ObTypDaoMock.SetReturnValue("getObTypAscendants", obTypAscendants);
               ObTypDaoMock.SetReturnValue("getPrhGroups", prhGroups);
             */
            /*PrismaDaoMock.SetReturnValue("getNews", inputnews);
            List<AvailableNewsDto> news = bo.listAvailableNews(input, "de-CH", false);

            Assert.IsEmpty(news);
        }*/

        /// <summary>
        /// Blackboxtest für die getProfile Methode welche ein ogetProfileDto zurück liefert
        /// </summary>
        [Test]
        public void listAvailableProductsTest()
        {
            prKontextDto input = new prKontextDto()
            {
                perDate = DateTime.Now,
                sysbrand = 0,
                sysprchannel = 2,
                sysprhgroup = 208,
                sysobart = 1,
                sysobtyp = 24536,
                sysprproduct = 210
            };

            List<long> obTypAscendants = new List<long>() {24536, 234};

            ObTypDaoMock.ExpectAndReturn("getObTypAscendants", obTypAscendants, input.sysobtyp);
            List<PRPRODUCT> products = bo.listAvailableProducts(input);

            Assert.IsNotNull(products);

            Assert.IsNotEmpty(products);

            Assert.AreEqual(allProduct[0], products[0]);
            Assert.AreEqual(allProduct[1], products[1]);
            Assert.AreEqual(allProduct[2], products[2]);
            Assert.AreEqual(allProduct[3], products[3]);
            Assert.AreEqual(allProduct[13], products[4]);
            Assert.AreEqual(allProduct[15], products[5]);
            Assert.AreEqual(allProduct[17], products[6]);
            Assert.AreEqual(allProduct[9], products[7]);
            Assert.AreEqual(allProduct[10], products[8]);

        }

        /// <summary>
        /// Test für das holen eines Spezifischen Produktes
        /// </summary>
        [Test]
        public void getProductTest()
        {
            PRPRODUCT productOut = allProduct[7];

            PrismaDaoMock.ExpectAndReturn("getProduct", productOut, allProduct[7].SYSPRPRODUCT, isoCode);

            PRPRODUCT product = bo.getProduct(allProduct[7].SYSPRPRODUCT);

            Assert.IsNotNull(product);

            Assert.AreEqual(allProduct[7], product);
        }

        /// <summary>
        /// Test für das der Vertragsart eines Spezifischen Produktes
        /// </summary>
        [Test]
        public void getVertragsartTest()
        {
            PRPRODUCT productOut = allProduct[7];

            VART outVart = new VART { SYSVART = 3, BEZEICHNUNG = "CREDIT-now Classic", AKTIVKZ = 1, LGD = 90, CODE = "KREDIT_CLASSIC" };

            PrismaDaoMock.ExpectAndReturn("getVertragsart", outVart, allProduct[7].SYSPRPRODUCT);

            VART vart = bo.getVertragsart(allProduct[7].SYSPRPRODUCT);

            Assert.IsNotNull(vart);

            Assert.AreEqual(outVart, vart);
        }
    }
}