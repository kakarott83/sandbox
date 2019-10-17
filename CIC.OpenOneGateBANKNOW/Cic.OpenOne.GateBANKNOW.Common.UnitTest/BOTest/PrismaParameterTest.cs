using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    [TestFixture()]
    class PrismaParameterTest
    {
        /// <summary>
        /// PrismaDaoMock
        /// </summary>
        public DynamicMock PrismaDaoMock;

        /// <summary>
        /// PrismaDaoMock
        /// </summary>
        public DynamicMock ObTypDaoMock;

        /// <summary>
        /// UebersetzungDaoMock
        /// </summary>
        public DynamicMock UebersetzungDaoMock;

        /// <summary>
        /// Prisma Services Class
        /// </summary>
        PrismaParameterBo PrismaParameterBo;
        List<PRFLD> fieldList;
        [SetUp]
        public void PrismaParameterTestInit()
        {
            PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            UebersetzungDaoMock = new DynamicMock(typeof(ITranslateDao));
            PrismaParameterBo = new PrismaParameterBo((IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, PrismaParameterBo.CONDITIONS_BANKNOW);
            fieldList = new List<PRFLD>
            {
                new PRFLD{SYSPRFLD=1, SYSPRFLDART=1, NAME="Endalter Kunde", OBJECTMETA="KALK_BORDER_ENDALTERKUNDE", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=2, SYSPRFLDART=1, NAME="Risikoklasse", OBJECTMETA="KALK_BORDER_KUNDENSCORE", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=3, SYSPRFLDART=1, NAME="Laufzeit", DESCRIPTION="Vertragslaufzeit", OBJECTMETA="KALK_BORDER_LZ", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=4, SYSPRFLDART=2, NAME="Laufleistung", DESCRIPTION="Fahrzeuglaufleisttung", OBJECTMETA="KALK_BORDER_LL", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=5, SYSPRFLDART=1, NAME="Kreditbetrag", DESCRIPTION="Nettokreditbetrag", OBJECTMETA="KALK_BORDER_BGINTERN", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=6, SYSPRFLDART=1, NAME="Rate", DESCRIPTION="Monatliche Rate", OBJECTMETA="KALK_BORDER_RATE", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=7, SYSPRFLDART=2, NAME="Fahrzeugalter Anfang", DESCRIPTION="Fahrzeugalter am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBALTEROBJ", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=8, SYSPRFLDART=2, NAME="Fahrzeugalter Ende", DESCRIPTION="Fahrzeugalter am Vertargsende", OBJECTMETA="KALK_BORDER_ENDALTEROBJ", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=9, SYSPRFLDART=2, NAME="Kilometerstand", DESCRIPTION="Kilometrstand am Vertragsanfang", OBJECTMETA="KALK_BORDER_UBNAHMEKM", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=11, SYSPRFLDART=2, NAME="Kilometerstand Ende", DESCRIPTION="Kilometerstand am Vertargsende", OBJECTMETA="KALK_BORDER_ENDLL", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=12, SYSPRFLDART=2, NAME="Restwert Leasing", OBJECTMETA="KALK_BORDER_RW", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=13, SYSPRFLDART=1, NAME="1_Rate", OBJECTMETA="KALK_BORDER_SZ", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=15, SYSPRFLDART=1, NAME="Zins", OBJECTMETA="KALK_SUBV_ZINS", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=36, SYSPRFLDART=1, NAME="Betrag Ratenabsicherung", OBJECTMETA="PROV_BASE_VERSICHERUNG", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=37, SYSPRFLDART=1, NAME="Fremdablösen", DESCRIPTION="DB-Feld offen", OBJECTMETA="PROV_BASE_ABLEXTERN", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=38, SYSPRFLDART=1, NAME="Erstauszahlung", DESCRIPTION="(Dispo)", OBJECTMETA="PROV_BASE_DISPO", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=14, SYSPRFLDART=1, NAME="Restrate Carfinance", OBJECTMETA="KALK_BORDER_RW", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=35, SYSPRFLDART=1, NAME="Zinskosten", OBJECTMETA="Zinskosten", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=78, SYSPRFLDART=2, NAME="Mehrkilometersatz", OBJECTMETA="OB_MARK_SATZMEHRKM", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=57, SYSPRFLDART=1, NAME="Aufschub", OBJECTMETA="GESCH_MARK_AUFSCHUB", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=58, SYSPRFLDART=1, NAME="Provisionsbasis Zins Neugeld", OBJECTMETA="PROV_BASE_ZINS_NEUGELD", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=59, SYSPRFLDART=1, NAME="Provisionsbasis Zins Ablöse intern", OBJECTMETA="PROV_BASE_ZINS_ABLINTERN", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=60, SYSPRFLDART=1, NAME="Provisionsbasis Zins Ablöse extern", OBJECTMETA="PROV_BASE_ZINS_ABLEXTERN", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=61, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Neugeld", OBJECTMETA="PROV_BASE_UMSATZ_NEUGELD", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=62, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Ablöse intern", OBJECTMETA="PROV_BASE_UMSATZ_ABLINTERN", CTRLTYPE = 0},
                new PRFLD{SYSPRFLD=63, SYSPRFLDART=1, NAME="Provisionsbasis Umsatz Ablöse extern", OBJECTMETA="PROV_BASE_UMSATZ_ABLEXTERN", CTRLTYPE = 0}
            };


            PrismaDaoMock.SetReturnValue("getFields", fieldList);


            List<ParameterSetConditionLink> ParamsetList = new List<ParameterSetConditionLink>
            {
                new ParameterSetConditionLink {level=1, sysprparset=2, area=99999, sysid=0},	
                new ParameterSetConditionLink {level=1, sysprparset=43, area=61, sysid=22},	
                new ParameterSetConditionLink {level=2, sysprparset=3, area=3, sysid=1, sysparent=2},
                new ParameterSetConditionLink {level=2, sysprparset=63, area=10, sysid=1, sysparent=43},
                new ParameterSetConditionLink {level=2, sysprparset=4, area=3, sysid=2, sysparent=2},
                new ParameterSetConditionLink {level=3, sysprparset=132, area=10, sysid=3, sysparent=4},
                new ParameterSetConditionLink {level=3, sysprparset=23, area=10, sysid=8, sysparent=3},
                new ParameterSetConditionLink {level=3, sysprparset=24, area=10, sysid=3, sysparent=3},
                new ParameterSetConditionLink {level=3, sysprparset=68, area=60, sysid=18, sysparent=63},
                new ParameterSetConditionLink {level=3, sysprparset=26, area=10, sysid=7, sysparent=4},
                new ParameterSetConditionLink {level=3, sysprparset=5, area=10, sysid=1, sysparent=3},
                new ParameterSetConditionLink {level=3, sysprparset=25, area=10, sysid=3, sysparent=4},
                new ParameterSetConditionLink {level=3, sysprparset=27, area=10, sysid=4, sysparent=4},
                new ParameterSetConditionLink {level=4, sysprparset=18, area=31, sysid=8, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=16, area=31, sysid=9, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=14, area=31, sysid=4, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=10, area=31, sysid=3, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=8, area=31, sysid=7, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=6, area=31, sysid=1, sysparent=5},
                new ParameterSetConditionLink {level=4, sysprparset=21, area=31, sysid=10, sysparent=5},
                new ParameterSetConditionLink {level=5, sysprparset=9, area=30, sysid=13, sysparent=8},
                new ParameterSetConditionLink {level=5, sysprparset=7, area=30, sysid=13, sysparent=6},
                new ParameterSetConditionLink {level=5, sysprparset=19, area=30, sysid=13, sysparent=18},
                new ParameterSetConditionLink {level=5, sysprparset=17, area=30, sysid=13, sysparent=16},
                new ParameterSetConditionLink {level=5, sysprparset=15, area=30, sysid=13, sysparent=14},
                new ParameterSetConditionLink {level=5, sysprparset=13, area=30, sysid=13, sysparent=10},
                new ParameterSetConditionLink {level=6, sysprparset=130, area=20, sysid=5, sysparent=17},
                new ParameterSetConditionLink {level=6, sysprparset=133, area=10, sysid=2, sysparent=7},
                new ParameterSetConditionLink {level=6, sysprparset=129, area=21, sysid=174, sysparent=17}
            };

            PrismaDaoMock.SetReturnValue("getParamSets", ParamsetList);

            List<ParameterConditionLink> ParamCondLinkListe = new List<ParameterConditionLink>
            {
                new ParameterConditionLink { sysprparset=120, sysprproduct=102, area=14},			
                new ParameterConditionLink { sysprparset=97, sysprproduct=102, area=14},
                new ParameterConditionLink { sysprparset=71, sysprproduct=84, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=53, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=104, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=186, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=50, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=124, area=14},
                new ParameterConditionLink { sysprparset=89, sysprproduct=56, area=14},
                new ParameterConditionLink { sysprparset=89, sysprproduct=52, area=14},
                new ParameterConditionLink { sysprparset=89, sysprproduct=57, area=14},
                new ParameterConditionLink { sysprparset=89, sysprproduct=102, area=14},
                new ParameterConditionLink { sysprparset=96, sysprproduct=208, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=45, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=188, area=14},
                new ParameterConditionLink { sysprparset=120, sysprproduct=102, area=14},
                new ParameterConditionLink { sysprparset=71, sysprproduct=102, area=14},
                new ParameterConditionLink { sysprparset=97, sysprproduct=172, area=14},
                new ParameterConditionLink { sysprparset=97, sysprproduct=82, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=59, area=14},
                new ParameterConditionLink { sysprparset=88, sysprproduct=214, area=14},
                new ParameterConditionLink { sysprparset=3, sysprproduct=512, area=14}
            };

            PrismaDaoMock.SetReturnValue("getParamConditionLinks", ParamCondLinkListe);

            List<ParamDto> ParamListe = new List<ParamDto>
            {
                new ParamDto{ sysprparset=5,    sysID=14,   meta="KALK_BORDER_ENDALTERKUNDE",   name="Endalter Leasing",                        visible=false, disabled=false, type=0, minvaln=0,   maxvaln=80, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{ sysprparset=2,    sysID=2,    meta="KALK_BORDER_ENDALTERKUNDE",   name="Endalter",                                visible=false, disabled=false, type=0, minvaln=0,   maxvaln=70, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{ sysprparset=68,   sysID=56,   meta="KALK_BORDER_ENDALTERKUNDE",   name="Maximales Endalter Todesfallschutz",      visible=false, disabled=false, type=0, minvaln=0,   maxvaln=70, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{ sysprparset=43,   sysID=54,   meta="KALK_BORDER_ENDALTERKUNDE",   name="AU / EU / AL Maximales Endalter",         visible=false, disabled=false, type=0, minvaln=0,   maxvaln=65, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=1},
                new ParamDto{ sysprparset=26,   sysID=193,  meta="KALK_BORDER_KUNDENSCORE",     name="Unterer Cut-Off  Dispo",                  visible=false, disabled=false, type=0, minvaln=0,   maxvaln=11, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{ sysprparset=43,   sysID=232,  meta="KALK_BORDER_KUNDENSCORE",     name="Test Grau",                               visible=false, disabled=false, type=0, minvaln=0,   maxvaln=11, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{ sysprparset=129,  sysID=167,  meta="KALK_BORDER_KUNDENSCORE",     name="Objekttyp ausweichen",                    visible=false, disabled=false, type=0, minvaln=2000, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{ sysprparset=2,    sysID=3,    meta="KALK_BORDER_KUNDENSCORE",     name="Unterer Cut-Off",                         visible=false, disabled=false, type=0, minvaln=0,    maxvaln=13, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=2},
                new ParamDto{ sysprparset=89,   sysID=69,   meta="KALK_BORDER_LZ",              name="Laufzeit 6, 72, def. 48",                 visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48, maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{ sysprparset=2,    sysID=4,    meta="KALK_BORDER_LZ",              name="Laufzeit",                                visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48, maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{ sysprparset=23,   sysID=31,   meta="KALK_BORDER_LZ",              name="Laufzeit Carfinance",                     visible=false, disabled=false, type=0, minvaln=6, maxvaln=72, defvaln=48, maxvalp=0, defvalp=0, stepsize=6, sysprfld=3},
                new ParamDto{ sysprparset=120,  sysID=144,  meta="KALK_BORDER_LZ",              name="Lauzeit TEST ADSP",                       visible=true, disabled=false, type=0, minvaln=0, maxvaln=24, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{ sysprparset=71,   sysID=166,  meta="KALK_BORDER_LZ",              name="Lauzeit 2",                               visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=3},
                new ParamDto{ sysprparset=25,   sysID=123,  meta="KALK_BORDER_LZ",              name="Laufzeiten KF Classic",                   visible=false, disabled=false, type=0, minvaln=6, maxvaln=60, defvaln=12, maxvalp=0, defvalp=0, stepsize=3, sysprfld=3},
                new ParamDto{ sysprparset=0, sysID=95,   meta="KALK_BORDER_LZ",              name="CICH",                                    visible=true, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=24, maxvalp=0, defvalp=0, stepsize=2, sysprfld=3},
                new ParamDto{ sysprparset=26,   sysID=35,   meta="KALK_BORDER_LZ",              name="Laufzeit Dispo",                          visible=false, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=36, maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{ sysprparset=88,   sysID=68,   meta="KALK_BORDER_LZ",              name="Laufzeiten: Min, Max, Default",           visible=false, disabled=false, type=0, minvaln=12, maxvaln=36, defvaln=36, maxvalp=0, defvalp=0, stepsize=12, sysprfld=3},
                new ParamDto{ sysprparset=27,   sysID=71,   meta="KALK_BORDER_LZ",              name="Laufzeit Express",                        visible=false, disabled=false, type=0, minvaln=6, maxvaln=11, defvaln=11, maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{ sysprparset=5,    sysID=16,   meta="KALK_BORDER_LZ",              name="Laufzeit Leasing",                        visible=false, disabled=false, type=0, minvaln=12, maxvaln=60, defvaln=48, maxvalp=0, defvalp=0, stepsize=1, sysprfld=3},
                new ParamDto{ sysprparset=3,    sysID=5,    meta="KALK_BORDER_LL",              name="Laufleistung",                            visible=false, disabled=false, type=0, minvaln=0, maxvaln=1000000, defvaln=10000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=4},
                new ParamDto{ sysprparset=15,   sysID=26,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Wohnmobile Occasion", visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=17,   sysID=27,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Boote Occasion",      visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=19,   sysID=28,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Sonstige Occasion",   visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=21,   sysID=29,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Zubehör",             visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=10,   sysID=24,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Zweirad",             visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=13,   sysID=25,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing Zweirad Occasion",    visible=false, disabled=false, type=0, minvaln=3000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=5,    sysID=12,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing",                     visible=false, disabled=false, type=0, minvaln=5000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=7,    sysID=17,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Leasing PKW Occasion",        visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=9,    sysID=18,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit LKW",                         visible=false, disabled=false, type=0, minvaln=7000, maxvaln=1000000, defvaln=45000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=3,    sysID=6,    meta="KALK_BORDER_BGINTERN",        name="Nettokredit",                             visible=false, disabled=false, type=0, minvaln=500, maxvaln=250000, defvaln=25000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=130,  sysID=150,  meta="KALK_BORDER_BGINTERN",        name="Kreditbetrag STD HG",                     visible=true, disabled=false, type=0, minvaln=10000, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=129,  sysID=149,  meta="KALK_BORDER_BGINTERN",        name="Kreditbetrag Boote",                      visible=true, disabled=false, type=0, minvaln=50000, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=27,   sysID=36,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Express",                     visible=false, disabled=false, type=0, minvaln=1000, maxvaln=10000, defvaln=10000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=0, sysID=214,  meta="KALK_BORDER_BGINTERN",        name="Kriditrahmen 500 - 20.000",               visible=false, disabled=false, type=0, minvaln=500, maxvaln=20000, defvaln=10000, maxvalp=0, defvalp=0, stepsize=500, sysprfld=5},
                new ParamDto{ sysprparset=26,   sysID=34,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit Dispo",                       visible=false, disabled=false, type=0, minvaln=5000, maxvaln=40000, defvaln=40000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=4,    sysID=32,   meta="KALK_BORDER_BGINTERN",        name="Nettokredit KF",                          visible=false, disabled=false, type=0, minvaln=500, maxvaln=250000, defvaln=25000, maxvalp=0, defvalp=0, stepsize=0, sysprfld=5},
                new ParamDto{ sysprparset=3,    sysID=7,    meta="KALK_BORDER_RATE",            name="Rate",                                    visible=false, disabled=false, type=0, minvaln=50, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=6},
                new ParamDto{ sysprparset=25,   sysID=33,   meta="KALK_BORDER_RATE",            name="Rate KF Classic",                         visible=false, disabled=false, type=0, minvaln=50, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=6},
                new ParamDto{ sysprparset=5,    sysID=49,   meta="KALK_BORDER_UBALTEROBJ",      name="Fahrzeugalter Anfang Leasing",            visible=false, disabled=false, type=0, minvaln=0, maxvaln=6, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=7},
                new ParamDto{ sysprparset=5,    sysID=50,   meta="KALK_BORDER_ENDALTEROBJ",     name="Fahrzeugalter Ende Leasing",              visible=false, disabled=false, type=0, minvaln=0, maxvaln=96, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=8},
                new ParamDto{ sysprparset=3,    sysID=10,   meta="KALK_BORDER_UBNAHMEKM",       name="Kilometerstand Anfang",                   visible=false, disabled=false, type=0, minvaln=0, maxvaln=5000, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=9},
                new ParamDto{ sysprparset=24,   sysID=51,   meta="KALK_BORDER_UBNAHMEKM",       name="Kilometerstand Anfang FF Classic",        visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=9},
                new ParamDto{ sysprparset=24,   sysID=52,   meta="KALK_BORDER_ENDLL",           name="Kilometerstand Ende FF Classic",          visible=false, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=11},
                new ParamDto{ sysprparset=3,    sysID=11,   meta="KALK_BORDER_ENDLL",           name="Kilometerstand Ende",                     visible=false, disabled=false, type=0, minvaln=0, maxvaln=200000, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=11},
                new ParamDto{ sysprparset=10,   sysID=23,   meta="KALK_BORDER_ENDLL",           name="Kilometerstand Ende Leasing Zweirad",     visible=false, disabled=false, type=0, minvaln=0, maxvaln=75000, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=11},
                new ParamDto{ sysprparset=5,    sysID=13,   meta="KALK_BORDER_RW",              name="Restwert",                                visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0, maxvalp=100, defvalp=0, stepsize=0, sysprfld=12},
                new ParamDto{ sysprparset=5,    sysID=15,   meta="KALK_BORDER_SZ",              name="1_Rate",                                  visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0, maxvalp=50, defvalp=0, stepsize=0, sysprfld=13},
                new ParamDto{ sysprparset=132,  sysID=155,  meta="KALK_BORDER_SZ",              name="Classic mit aufgeschobener Rate",         visible=true, disabled=false, type=0, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, steplistcsv="2", sysprfld=13},
                new ParamDto{ sysprparset=2,    sysID=57,   meta="KALK_SUBV_ZINS",              name="Maximaler Effektivzinssatz",              visible=false, disabled=false, type=0, minvaln=0, maxvaln=15, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{ sysprparset=0, sysID=124,  meta="KALK_SUBV_ZINS",              name="Sonderkondition <3",                      visible=false, disabled=false, type=0, minvaln=0, maxvaln=3, defvaln=0, maxvalp=0, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{ sysprparset=97,   sysID=106,  meta="KALK_SUBV_ZINS",              name="UO Zins CREDIT-now Classic",              visible=true, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0, maxvalp=12.5, defvalp=0, stepsize=0, sysprfld=15},
                new ParamDto{ sysprparset=96,   sysID=102,  meta="GESCH_MARK_AUFSCHUB",         name="Aufschub 3M",                             visible=false, disabled=false, type=0, minvaln=3, maxvaln=3, defvaln=3, maxvalp=0, defvalp=0, stepsize=0, sysprfld=57},
                new ParamDto{ sysprparset=3,    sysID=194,  meta="OB_MARK_SATZMEHRKM",          name="Mehrkilometersatz",                       visible=false, disabled=false, type=1, minvaln=0, maxvaln=0, defvaln=0, maxvalp=0, defvalp=0.0005, stepsize=0, sysprfld=78}
            };

            PrismaDaoMock.SetReturnValue("getParams", ParamListe);
        }

        [Test]
        public void getParameterTestAufschub()
        {

            prKontextDto context = new prKontextDto{perDate=DateTime.Now,
                                                    sysperole=674,
                                                    sysbrand=0,
                                                    syskdtyp=1,
                                                    sysobart=13,
                                                    sysobtyp=2841,
                                                    sysprchannel=1,
                                                    sysprhgroup=0,
                                                    sysprinttype=0,
                                                    sysprkgroup=0,
                                                    sysprproduct =208,
                                                    sysprusetype=2,
                                                    sysvart=0 };

            List <long> obTypListe = new List <long> {2841, 2838, 2837, 1095, 234};

            ObTypDaoMock.SetReturnValue("getObTypAscendants", obTypListe);

            VART VerttragsArt = new VART { SYSVART=3, BEZEICHNUNG="CREDIT-now Classic", AKTIVKZ=1, LGD=90, CODE="KREDIT_CLASSIC" };

            PrismaDaoMock.ExpectAndReturn("getVertragsart", VerttragsArt, context.sysprproduct);

            ParamDto Data = PrismaParameterBo.getParameter(context, EnumUtil.GetStringValue(PrismaParameters.Aufschub));
            
            Assert.IsNotNull(Data);

            Assert.AreEqual(3.0, Data.defvaln);
            Assert.AreEqual(96, Data.sysprparset);
            Assert.AreEqual(102, Data.sysID);
            Assert.AreEqual("GESCH_MARK_AUFSCHUB", Data.meta);
            Assert.AreEqual("Aufschub 3M", Data.name);
            Assert.AreEqual(false, Data.visible);
            Assert.AreEqual(false, Data.disabled);
            Assert.AreEqual(0, Data.type); 
            Assert.AreEqual(3, Data.minvaln);
            Assert.AreEqual(3, Data.maxvaln);
            Assert.AreEqual(3, Data.defvaln);
            Assert.AreEqual(0, Data.maxvalp);
            Assert.AreEqual(0, Data.defvalp);
            Assert.AreEqual(0, Data.stepsize);
            Assert.AreEqual(57, Data.sysprfld);
        }

        [Test]
        public void getParameterTestSatzMehrKm()
        {

            prKontextDto context = new prKontextDto
            {
                perDate = DateTime.Now,
                sysperole = 674,
                sysbrand = 0,
                syskdtyp = 1,
                sysobart = 13,
                sysobtyp = 2841,
                sysprchannel = 1,
                sysprhgroup = 0,
                sysprinttype = 0,
                sysprkgroup = 0,
                sysprproduct = 208,
                sysprusetype = 2,
                sysvart = 0
            };

            List<long> obTypListe = new List<long> { 2841, 2838, 2837, 1095, 234 };

            ObTypDaoMock.SetReturnValue("getObTypAscendants", obTypListe);

            VART VerttragsArt = new VART { SYSVART = 3, BEZEICHNUNG = "CREDIT-now Classic", AKTIVKZ = 1, LGD = 90, CODE = "KREDIT_CLASSIC" };

            PrismaDaoMock.ExpectAndReturn("getVertragsart", VerttragsArt, context.sysprproduct);

            ParamDto Data = PrismaParameterBo.getParameter(context, EnumUtil.GetStringValue(PrismaParameters.SatzMehrKm));

            Assert.IsNotNull(Data);

            Assert.AreEqual(0, Data.defvaln);
            Assert.AreEqual(3, Data.sysprparset);
            Assert.AreEqual(194, Data.sysID);
            Assert.AreEqual("OB_MARK_SATZMEHRKM", Data.meta);
            Assert.AreEqual("Mehrkilometersatz", Data.name);
            Assert.AreEqual(false, Data.visible);
            Assert.AreEqual(false, Data.disabled);
            Assert.AreEqual(1, Data.type);
            Assert.AreEqual(0, Data.minvaln);
            Assert.AreEqual(0, Data.maxvaln);
            Assert.AreEqual(0, Data.defvaln);
            Assert.AreEqual(0, Data.maxvalp);
            Assert.AreEqual(0.0005, Data.defvalp);
            Assert.AreEqual(0, Data.stepsize);
            Assert.AreEqual(78, Data.sysprfld);
        }
    }
}
