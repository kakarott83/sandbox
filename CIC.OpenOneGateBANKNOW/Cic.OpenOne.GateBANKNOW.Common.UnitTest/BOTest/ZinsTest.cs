using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using CIC.Database.PRISMA.EF6.Model;
using NUnit.Framework;
using NUnit.Mocks;
using System;
using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse zum Testen der Methoden des parameterBo´s
    /// </summary>
    [TestFixture()]
    public class ZinsTest
    {
        DynamicMock ZinsDaoMock;
        DynamicMock PrismaDaoMock;
        DynamicMock ObTypDaoMock;
        DynamicMock VGDaoMock;
        ZinsBo bo;

        List<PRRAP> RAPout;
        List <PRRAPVAL> RAPvalues1;
        List <PRRAPVAL> RAPvalues2;
        /// <summary>
        /// Setup der Testdaten
        /// </summary>
        [SetUp]
        public void ZinsTestSetup()
        {
            ZinsDaoMock = new DynamicMock(typeof(IZinsDao));
            PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            VGDaoMock = new DynamicMock(typeof(IVGDao));
            bo = new ZinsBo((IZinsDao)ZinsDaoMock.MockInstance, (IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, ZinsBo.CONDITIONS_BANKNOW, "de-CH", (IVGDao)VGDaoMock.MockInstance);

            PRPRODUCT selproduct = new PRPRODUCT { SYSPRPRODUCT = 232, SYSVART = 3, NAME = "7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)", ACTIVEFLAG = 1, VALIDFROM = new DateTime(2011, 06, 27), SOURCEBASIS = 1, SYSINTSTRCT = 109, SYSPRPRODTYPE = 1, NAMEINTERN = "7.4 FF CREDIT-now Classic (Fahrzeugfinanzierung inkl Zubehörsfinanzierung)" };

            PrismaDaoMock.SetReturnValue("getProduct", selproduct);

            List<IntsDto> outBand = new List<IntsDto>()
            {
                new IntsDto { sysintsdate=107, lowerb=0, upperb=10000, intrate=8.8, addrate=0, redrate=1},
                new IntsDto { sysintsdate=114, lowerb=0, upperb=20000, intrate=7, addrate=0, redrate=0},
                new IntsDto { sysintsdate=114, lowerb=20000, upperb=40000, intrate=6.5, addrate=0, redrate=0},
                new IntsDto { sysintsdate=114, lowerb=40000, upperb=0, intrate=6, addrate=0, redrate=0},
                new IntsDto { sysintsdate=107, lowerb=10000, upperb=20000, intrate=8.8, addrate=0, redrate=2},
                new IntsDto { sysintsdate=107, lowerb=20000, upperb=0, intrate=8.8, addrate=0, redrate=3}
            };
            ZinsDaoMock.SetReturnValue("getIntsband", outBand);

            List<IntsDto> outMatu = new List<IntsDto>()
            {
                new IntsDto{ sysintsdate=117, intrate=7, addrate=0, redrate=0},
                new IntsDto{ sysintsdate=116, maturity=7, intrate=7, addrate=0, redrate=0},
                new IntsDto{ sysintsdate=116, maturity=1, intrate=8, addrate=0, redrate=0},
                new IntsDto{ sysintsdate=116, maturity=13, intrate=6, addrate=0, redrate=0}
            };
            ZinsDaoMock.SetReturnValue("getIntsmatu", outMatu);

            List<IntstrctDto> outStrct = new List<IntstrctDto>
            {
                new IntstrctDto{ sysintstrct=103, sysprproduct=208, sysintsdate=106, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=104, sysprproduct=217, sysintsdate=107, method=3, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=104, sysprproduct=209, sysintsdate=107, method=3, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=105, sysprproduct=210, sysintsdate=108, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=105, sysprproduct=218, sysintsdate=108, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=106, sysprproduct=216, sysintsdate=109, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=106, sysprproduct=247, sysintsdate=109, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=106, sysprproduct=211, sysintsdate=109, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=107, sysprproduct=212, sysintsdate=110, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=108, sysprproduct=219, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=108, sysprproduct=227, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=108, sysprproduct=228, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=108, sysprproduct=213, sysintsdate=111, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=109, sysprproduct=214, sysintsdate=112, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=109, sysprproduct=220, sysintsdate=112, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=109, sysprproduct=223, sysintsdate=112, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=110, sysprproduct=207, sysintsdate=113, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=110, sysprproduct=221, sysintsdate=113, method=1, validFrom=new DateTime(2011, 06, 20)},
                new IntstrctDto{ sysintstrct=111, sysprproduct=222, sysintsdate=114, method=3, validFrom=new DateTime(2011, 01, 01)},
                new IntstrctDto{ sysintstrct=113, sysprproduct=224, sysintsdate=116, method=2, validFrom=new DateTime(2011, 01, 01)}
            };
            ZinsDaoMock.SetReturnValue("getIntstrct", outStrct);

            List<IntsDto> outRate = new List<IntsDto>()
            {
                new IntsDto{ sysintsdate=106, intrate=11, addrate=0, redrate=0, lowerb=0, upperb=0},
                new IntsDto{ sysintsdate=108, intrate=14.5, addrate=0, redrate=0, lowerb=0, upperb=0},
                new IntsDto{ sysintsdate=109, intrate=10, addrate=0, redrate=0, lowerb=0, upperb=0},
                new IntsDto{ sysintsdate=110, intrate=5.9, addrate=0, redrate=0, lowerb=3, upperb=5.5},
                new IntsDto{ sysintsdate=111, intrate=7.5, addrate=0, redrate=0, lowerb=0, upperb=0},
                new IntsDto{ sysintsdate=112, intrate=6.6, addrate=0, redrate=0, lowerb=0, upperb=0},
                new IntsDto{ sysintsdate=113, intrate=10, addrate=0, redrate=0, lowerb=0, upperb=0}
            };
            ZinsDaoMock.SetReturnValue("getIntsrate", outRate);

            List<IborDto> outIbor = new List<IborDto>();
            outIbor.Add(new IborDto());
            outIbor[0].m1 = 12.23;
            outIbor[0].m3 = 36.69;
            outIbor[0].m6 = 73.38;
            outIbor[0].m9 = 110.07;
            outIbor[0].name = "UnitTest";
            outIbor[0].ovm12n = 1234.34;
            outIbor[0].ovn = 123.34;
            outIbor[0].sysprproduct = 5;
            outIbor[0].sysswaehrung = 0;
            outIbor[0].tn = 12345.34;
            outIbor[0].validFrom = DateTime.Now;
            outIbor[0].w1 = 123.56;
            ZinsDaoMock.SetReturnValue("getIbor", outIbor);

            List<PRCLPRINTSETDto> outLinks = new List<PRCLPRINTSETDto>();
            outLinks.Add(new PRCLPRINTSETDto());
            outLinks[0].rank = 3;
            outLinks[0].sysprclprintset = 33;
            outLinks[0].sysprintest = 1;
            outLinks[0].sysprproduct = 5;
            ZinsDaoMock.SetReturnValue("getProductLinks", outLinks);

            List<PRINTSETDto> OutZinsGroups = new List<PRINTSETDto>();
            OutZinsGroups.Add(new PRINTSETDto());
            OutZinsGroups[0].sysprintset = 1;
            OutZinsGroups[0].validfrom = DateTime.Now;
            OutZinsGroups[0].validuntil = DateTime.Now;
            ZinsDaoMock.SetReturnValue("getIntGroups", OutZinsGroups);

            List<InterestConditionLink> OutAllsteps = new List<InterestConditionLink>();
            OutAllsteps.Add(new InterestConditionLink());
            OutAllsteps[0].adjustmenttrigger = 111;
            OutAllsteps[0].intrate = 44;
            OutAllsteps[0].method = 32;
            OutAllsteps[0].rank = 3;
            OutAllsteps[0].sourcebasis = 33;
            OutAllsteps[0].sysbrand = 34;
            OutAllsteps[0].sysibor = 3;
            OutAllsteps[0].sysintstrct = 45;
            OutAllsteps[0].sysinttype = 9;
            OutAllsteps[0].sysobart = 90;
            OutAllsteps[0].sysobtyp = 28;
            OutAllsteps[0].sysperole = 99;
            OutAllsteps[0].sysprhgroup = 12;
            OutAllsteps[0].sysprintset = 1;
            OutAllsteps[0].sysprkgroup = 91;
            OutAllsteps[0].sysvg = 64;
            OutAllsteps[0].CONDITIONTYPE = ConditionLinkType.PRKGROUP;
            ZinsDaoMock.SetReturnValue("getIntSteps", OutAllsteps);

            RAPout = new List<PRRAP>
            {
                new PRRAP { SYSPRRAP=24, NAME="6.2", DESCRIPTION="6.2 KF CREDIT-now Classic mit aufgeschobener Ratenfälligkeit", SOURCEBASIS=1, ACTIVEFLAG=1, SYSBRAND=0, MINVALUE=9, MAXVALUE=11},
                new PRRAP { SYSPRRAP=25, NAME="6.3", DESCRIPTION="6.3 KF CREDIT-now Dispo", SOURCEBASIS=1, ACTIVEFLAG=1, SYSBRAND=0, MINVALUE=3, MAXVALUE=11},
                new PRRAP { SYSPRRAP=44, NAME="6.1", SOURCEBASIS=1, ACTIVEFLAG=1, VALIDFROM=new DateTime (2011, 01, 01), SYSBRAND=0, MINVALUE=5, MAXVALUE=15},
                new PRRAP { SYSPRRAP=44, NAME="6.1", SOURCEBASIS=1, ACTIVEFLAG=1, VALIDFROM=new DateTime (2011, 01, 01), SYSBRAND=0, MINVALUE=5, MAXVALUE=15}
            };


            RAPvalues1 = new List<PRRAPVAL>
            {
                new PRRAPVAL{ SYSPRRAPVAL=34, SYSPRRAP=44, VALUE=12, SCORE="240" },
                new PRRAPVAL{ SYSPRRAPVAL=31, SYSPRRAP=44, VALUE=9, SCORE="200" },
                new PRRAPVAL{ SYSPRRAPVAL=30, SYSPRRAP=44, VALUE=11, SCORE="100" },
            };

            RAPvalues2 = new List<PRRAPVAL>
            {
                new PRRAPVAL{ SYSPRRAPVAL=32, SYSPRRAP=24, VALUE=11, SCORE="100" },
            };

        }

        /// <summary>
        /// Blackboxtest für die listAvailableProductParams Methode
        /// </summary>
        [Test]
        public void deliverZinsTest1()
        {

            //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
            prKontextDto input = new prKontextDto();
            input.perDate = DateTime.Now;
            input.sysbrand = 1;
            input.sysprchannel = 0;
            input.sysprhgroup = 3;
            input.sysobart = 0;
            input.sysobtyp = 43668;
            input.sysprproduct = 228;

            double zins = bo.getZins(input, 36, 30000);

            Assert.AreEqual(7.5, zins);
        }

        /// <summary>
        /// Blackboxtest für die listAvailableProductParams Methode
        /// </summary>
        [Test]
        public void deliverZinsTest2()
        {
            //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
            prKontextDto input = new prKontextDto();
            input.perDate = DateTime.Now;
            input.sysbrand = 1;
            input.sysprchannel = 0;
            input.sysprhgroup = 3;
            input.sysobart = 0;
            input.sysobtyp = 43668;
            input.sysprproduct = 208;

            double zins = bo.getZins(input, 36, 30000);

            Assert.AreEqual(11, zins);

        }

        /// <summary>
        /// Blackboxtest für die getRapZinsByScore Methode
        /// </summary>
        [Test]
        public void getRapZinsByScoreTest1()
        {

            long sysprproduct = 221;
            String Score = "220";
            PRRAP RapVal = RAPout[0];

            ZinsDaoMock.ExpectAndReturn("getPrRap", RAPout[3], sysprproduct);
            ZinsDaoMock.ExpectAndReturn("getRapValues", RAPvalues1, 44);
            decimal zins = bo.getRapZinsByScore(sysprproduct, Score);

            Assert.AreEqual(9.0, zins);
        }

        /// <summary>
        /// Blackboxtest für die getRapZinsByScore Methode
        /// </summary>
        [Test]
        public void getRapZinsByScoreTest2()
        {

            long sysprproduct = 221;
            String Score = "100";
            PRRAP RapVal = RAPout[0];

            ZinsDaoMock.ExpectAndReturn("getPrRap", RAPout[3], sysprproduct);
            ZinsDaoMock.ExpectAndReturn("getRapValues", RAPvalues1, 44);
            decimal zins = bo.getRapZinsByScore(sysprproduct, Score);

            Assert.AreEqual(11.0, zins);
        }

        /// <summary>
        /// Blackboxtest für die getRapZinsByScore Methode
        /// </summary>
        [Test]
        public void getRapZinsByScoreTest3()
        {
            long sysprproduct = 207;
            String Score = "220";
            PRRAP RapVal = RAPout[0];

            ZinsDaoMock.ExpectAndReturn("getPrRap", RapVal, sysprproduct);
            ZinsDaoMock.ExpectAndReturn("getRapValues", RAPvalues2, RapVal.SYSPRRAP);
            decimal zins = bo.getRapZinsByScore(sysprproduct, Score);

            Assert.AreEqual(11, zins);
        }

        /// <summary>
        /// Blackboxtest für die getRapZinsByScore Methode
        /// </summary>
        [Test]
        public void getPrRapTest()
        {
            long sysprproduct = 207;
            PRRAP RapVal = RAPout[0];

            ZinsDaoMock.ExpectAndReturn("getPrRap", RapVal, sysprproduct);
            PRRAP RapValOut = bo.getPrRap(sysprproduct);

            Assert.IsNotNull(RapValOut);

            Assert.AreEqual(RapVal, RapValOut);
        }
    }
}
