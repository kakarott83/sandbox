using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;

using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
{
    /// <summary>
    /// Testklasse für Eurotax
    /// </summary>
    [TestFixture()]
    public class EurotaxBoTest
    {
        /// <summary>
        /// eurotaxWSDaoMock
        /// </summary>
        public DynamicMock eurotaxWSDaoMock;
        /// <summary>
        /// eurotaxDBDaoMock
        /// </summary>
        public DynamicMock eurotaxDBDaoMock;
        /// <summary>
        /// auskunftDaoMock
        /// </summary>
        public DynamicMock auskunftDaoMock;
        /// <summary>
        /// VGDaoMock
        /// </summary>
        public DynamicMock vgDaoMock;
        /// <summary>
        /// ObTypDaoMock
        /// </summary>
        public DynamicMock obtypDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// EurotaxBo
        /// </summary>
        public EurotaxBo EurotaxBo;

        /// <summary>
        /// Initialisierung aller generellen Parameter und Objekten die für die Test benötigt werden
        /// </summary>
        [SetUp]
        public void EurotaxBoTestInit()
        {
            emptyMessage = new Message();
            eurotaxWSDaoMock = new DynamicMock(typeof(IEurotaxWSDao));
            eurotaxDBDaoMock = new DynamicMock(typeof(IEurotaxDBDao));
            auskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            auskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            vgDaoMock = new DynamicMock(typeof(IVGDao));
            obtypDaoMock = new DynamicMock(typeof(IObTypDao));
            EurotaxBo = new EurotaxBo((IEurotaxWSDao)eurotaxWSDaoMock.MockInstance, (IEurotaxDBDao)eurotaxDBDaoMock.MockInstance, (IAuskunftDao)auskunftDaoMock.MockInstance, (IVGDao)vgDaoMock.MockInstance, (IObTypDao)obtypDaoMock.MockInstance);
        }

        /// <summary>
        /// Blackboxtest für die getForecast Methode die als Input ein EurotexInDto erhält
        /// </summary>
        [Test]
        public void GetForecastWithEurotaxInDto()
        {
            EurotaxInDto EurotaxInDto = new EurotaxInDto()
            {
                CurrentMileageValue = 1000,
                EstimatedAnnualMileageValue = 10,
                ForecastPeriodFrom = "Periode",
                ISOCountryCode = new ISOcountryType()
                {
                },
                ISOCurrencyCode = new ISOcurrencyType()
                {
                },
                ISOLanguageCode = new ISOlanguageType()
                {
                },
                NationalVehicleCode = 2,
                RegistrationDate = new DateTime(2010,01,01),
                TotalListPriceOfEquipment = 10000.00,
                Mileage = "Meilen",
                ISOCountryCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType(),
                ISOCurrencyCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType(),
                ISOLanguageCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType(),
                //Version = "Item100"
            };

            auskunftDaoMock.ExpectAndReturn("SaveAuskunft", (long)1, AuskunfttypDao.EurotaxGetForecast);
            eurotaxDBDaoMock.ExpectAndReturn("SaveEurotaxInpFc", (long)1, 1);
            eurotaxDBDaoMock.Expect("SaveEurotaxInDto", EurotaxInDto, 1);
            eurotaxWSDaoMock.Expect("getForecast");
            eurotaxDBDaoMock.SetReturnValue("GetEurotaxAccessData", new String[] {"Loginname", "Password" });
            eurotaxDBDaoMock.Expect("SaveEurotaxOutDto");
            auskunftDaoMock.Expect("UpdateAuskunft");

            ObtypDataRestwertDto RestwertData = new ObtypDataRestwertDto();
            RestwertData.Schwacke = EurotaxInDto.NationalVehicleCode;
            RestwertData.sysobtyp = 33;
            RestwertData.TotalListPriceOfEquipment = 50000;

            //BR10
            obtypDaoMock.ExpectAndReturn("getObTypDataByNVCByString", RestwertData, EurotaxInDto.NationalVehicleCode.ToString());
            if (RestwertData.sysobtyp > 0 ) RestwertData.Schwacke = EurotaxInDto.NationalVehicleCode;


            Cic.OpenOne.GateBANKNOW.Common.DTO.RestWertSettingsDto rwSettings = new Cic.OpenOne.GateBANKNOW.Common.DTO.RestWertSettingsDto();
            rwSettings.External = false;
            rwSettings.sysvgrw = 0;

            eurotaxDBDaoMock.ExpectAndReturn("getRestwertSettings", rwSettings, RestwertData.sysobtyp);

            List<EurotaxOutDto> outDtoList = EurotaxBo.GetForecast(EurotaxInDto);
            EurotaxOutDto EurotexOutDto = outDtoList[0];

            Assert.AreEqual(0 , EurotexOutDto.ErrorCode);
            Assert.AreEqual(0, EurotexOutDto.RetailAmount);
            Assert.AreEqual(0, EurotexOutDto.TradeAmount);
        }

        /// <summary>
        /// Blackboxtest für die getForecast Methode die als Input ein long sysAuskunft erhält
        /// </summary>
        [Test]
        public void GetForecastWithSysAuskunft()
        {
            AuskunftDto AuskunftInDto = new AuskunftDto()
            {
                sysAuskunft = 1,
                EurotaxInDto = new EurotaxInDto(),
                EurotaxOutDto = new EurotaxOutDto(),
            };

            EurotaxInDto EurotaxInDto = new EurotaxInDto()
            {
                CurrentMileageValue = 1000,
                EstimatedAnnualMileageValue = 10,
                ForecastPeriodFrom = "Periode",
                ISOCountryCode = new ISOcountryType()
                {
                },
                ISOCurrencyCode = new ISOcurrencyType()
                {
                },
                ISOLanguageCode = new ISOlanguageType()
                {
                },
                NationalVehicleCode = 2,
                RegistrationDate = new DateTime(2010, 01, 01),
                TotalListPriceOfEquipment = 10000.00,
                Mileage = "Meilen",
                ISOCountryCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType(),
                ISOCurrencyCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType(),
                ISOLanguageCodeValuation = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType(),
                //Version = "Item100"
            };

            auskunftDaoMock.ExpectAndReturn("FindBySysId", AuskunftInDto ,1);
            eurotaxDBDaoMock.SetReturnValue("GetEurotaxAccessData", new String[] { "Loginname", "Password" });
            eurotaxDBDaoMock.ExpectAndReturn("FindBySysId", EurotaxInDto, 1);
            eurotaxWSDaoMock.Expect("getForecast");
            eurotaxDBDaoMock.Expect("SaveEurotaxOutDto");
            auskunftDaoMock.Expect("UpdateAuskunft");
            AuskunftDto AuskunftDto = EurotaxBo.GetForecast(1);
            Assert.AreEqual(null , AuskunftDto.EurotaxOutDto.ErrorDescription);
        }
    }
}
