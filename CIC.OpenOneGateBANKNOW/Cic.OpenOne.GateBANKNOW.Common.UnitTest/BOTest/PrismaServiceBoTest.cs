using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse für PrismaServiceBo
    /// </summary>
    [TestFixture()]
    public class PrismaServiceBoTest
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
        PrismaServiceBo PrismaServices;
        /// <summary>
        /// Initialisiert alle generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void PrismaServiceBoTestInit()
        {
            PrismaDaoMock = new DynamicMock(typeof(IPrismaServiceDao));
            ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            UebersetzungDaoMock = new DynamicMock(typeof(ITranslateDao));
            PrismaServices = new PrismaServiceBo((IPrismaServiceDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, (ITranslateDao)UebersetzungDaoMock.MockInstance);
        }
        
        /// <summary>
        /// Blackboxtest für die listAvailableServices Methode 
        /// </summary>
        [Test]
        public void listAvailableServicesTest()
        {
            srvKontextDto input = new srvKontextDto();
            DateTime inDate = DateTime.Now;

            input = new srvKontextDto();
            input.syskdtyp = 1;
            input.sysobart = 1;
            input.sysobtyp = 1;
            input.sysprkgroup = 1;
            input.sysprprodukt = 1;
            input.sysperole = 1;
            input.perDate = inDate;


            List<PRVSDto> OutServices = new List<PRVSDto>();
            OutServices.Add(new PRVSDto());
            OutServices[0].BESCHREIBUNG = "Service Test Beschreibung";
            OutServices[0].BEZEICHNUNG = "Service Test";
            OutServices[0].CODE = "Service Test Code";
            OutServices[0].DISABLEDGRP = 0;
            OutServices[0].DISABLEDPOS = 0;
            OutServices[0].EDITABLE = true;
            OutServices[0].FLAGDEFAULT = 1;
            OutServices[0].METHOD = 33;
            OutServices[0].MITFIN = 1;
            OutServices[0].NEEDEDGRP = 1;
            OutServices[0].NEEDEDPOS = 1;
            OutServices[0].POSID = 1;
            OutServices[0].RANK = 3;
            OutServices[0].SETID = 95;
            OutServices[0].SYSPERSON = 124;
            OutServices[0].SYSPRPRODUCT = 234;
            OutServices[0].SYSVSART = 3;
            OutServices[0].SYSVSTYP = 4;
            OutServices[0].VALIDFROM = inDate;
            OutServices[0].VALIDFROMGRP = inDate;
            OutServices[0].VALIDUNTIL = inDate;
            OutServices[0].VALIDUNTILGRP = inDate;


            PrismaDaoMock.ExpectAndReturn("getVSTYPForProduct", OutServices, inDate, 1);

            List<ServiceConditionLink> outConditions = new List<ServiceConditionLink>();
            outConditions.Add(new ServiceConditionLink());
            outConditions[0].ACTIVEFLAG = 1;
            outConditions[0].syskdtyp = 4;
            outConditions[0].sysobart = 3;
            outConditions[0].sysobtyp = 2;
            outConditions[0].sysprkgroup = 33;
            outConditions[0].SYSVSTYP = 134;
            outConditions[0].TARGETID = 34;
            outConditions[0].VALIDFROM = inDate;
            outConditions[0].VALIDUNTIL = inDate;

            PrismaDaoMock.SetReturnValue("getServiceConditionLinks", outConditions);

            List<long> outDes = new List<long>();
            outDes.Add(1);
            outDes.Add(2);
            outDes.Add(3);

            ObTypDaoMock.SetReturnValue("getObTypAscendants", outDes);
            
            List<AvailableServiceDto> rval = PrismaServices.listAvailableServices(input, "ch-DE");
                
            Assert.IsNotNull(rval);
        }


    }
}