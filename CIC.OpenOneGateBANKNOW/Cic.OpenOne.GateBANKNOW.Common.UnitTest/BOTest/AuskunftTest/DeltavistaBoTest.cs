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
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DeltavistaRef;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
{
    /// <summary>
    /// Testklasse für Deltavista
    /// </summary>
    [TestFixture()]
    public class DeltavistaBoTest
    {
        /// <summary>
        /// DeltavistaWSDaoMock
        /// </summary>
        public DynamicMock DeltavistaWSDaoMock;
        /// <summary>
        /// DeltavistaDBDaoMock
        /// </summary>
        public DynamicMock DeltavistaDBDaoMock;
        /// <summary>
        /// AuskunftDaoMock
        /// </summary>
        public DynamicMock AuskunftDaoMock;
        /// <summary>
        /// emptyMessage
        /// </summary>
        public Message emptyMessage;
        /// <summary>
        /// DeltavistaBo
        /// </summary>
        public DeltavistaBo DeltavistaBo;

        /// <summary>
        /// Initialisierung der generellen Parameter zum testen von Deltavista
        /// </summary>
        [SetUp]
        public void DeltavistaBoTestInit()
        {
            emptyMessage = new Message();
            DeltavistaWSDaoMock = new DynamicMock(typeof(IDeltavistaWSDao));
            DeltavistaDBDaoMock = new DynamicMock(typeof(IDeltavistaDBDao));
            AuskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            DeltavistaBo = new DeltavistaBo((IDeltavistaWSDao)DeltavistaWSDaoMock.MockInstance, (IDeltavistaDBDao)DeltavistaDBDaoMock.MockInstance, (IAuskunftDao)AuskunftDaoMock.MockInstance);
        }

        /// <summary>
        /// Blackboxtest für die Methode getIdentifierAdress welche mit einem DeltavistaInDto aufgerufen wird.
        /// </summary>
        [Test]
        public void getIdentifierAddressByDeltavistaInDtoTest()
        {
            DeltavistaInDto DeltavistaInDto = new DeltavistaInDto()
            {
                AddressDescription = new DVAddressDescriptionDto() 
                { 
                    Birthdate = "22.12.1989", 
                    City= "Hamburg",
                    Country = "Germany",
                    Fax = "1223456",
                    FirstName = "Klaus",
                    Housenumber = "100a",
                    LegalForm = 2,
                    MaidenName = "MaidenName1",
                    Mobile = "01701233456",
                    Name = "Lustig",
                    Sex = 1,
                    Street = "Fiktivstraße 3",
                    Telephone = "0401234567",
                    Zip = "21031"
                },
                AddressId = 100
            };

            AddressIdentificationResponse AdressIdentificationResponse = new AddressIdentificationResponse()
            {
                candidateListe = new AddressMatch[]
                {
                    new AddressMatch()
                    {
                        address = new AddressDescription()
                        {
                            birthdate = "22.12.1989", 
                            city= "Hamburg",
                            country = "Germany",
                            fax = "1223456",
                            firstName = "Klaus",
                            housenumber = "100a",
                            legalForm = 2,
                            maidenName = "MaidenName1",
                            mobile = "01701233456",
                            name = "Lustig",
                            sex = 1,
                            street = "Fiktivstraße 3",
                            telephone = "0401234567",
                            zip = "21031"
                        },
                        addressId = 1,
                        character = 2,
                        confidence = 3,
                        correction = new AddressCorrection()
                        {
                            city = "Berlin",
                            corrCity = 2,
                            corrHousenumber = 12,
                            corrStreet = 15,
                            corrZip = 10000,
                            housenumber = "12",
                            street = "Berlin Hauptstraße",
                            zip = "10000"
                        },
                        difference = 2,
                        similarity = 4,
                        status = 1
                    },

                    new AddressMatch()
                    {
                        address = new AddressDescription()
                        {
                            birthdate = "22.12.1989", 
                            city= "Hamburg",
                            country = "Germany",
                            fax = "1223456",
                            firstName = "Klaus",
                            housenumber = "100a",
                            legalForm = 2,
                            maidenName = "MaidenName1",
                            mobile = "01701233456",
                            name = "Lustig",
                            sex = 1,
                            street = "Fiktivstraße 3",
                            telephone = "0401234567",
                            zip = "21031"
                        },
                        addressId = 1,
                        character = 2,
                        confidence = 3,
                        correction = new AddressCorrection()
                        {
                            city = "Berlin",
                            corrCity = 2,
                            corrHousenumber = 12,
                            corrStreet = 15,
                            corrZip = 10000,
                            housenumber = "12",
                            street = "Berlin Hauptstraße",
                            zip = "10000"
                        },
                        difference = 2,
                        similarity = 4,
                        status = 1
                        
                    }
                },
                foundAddress = new AddressMatch()
                {
                    address = new AddressDescription()
                    {
                        birthdate = "22.12.1989", 
                        city= "Hamburg",
                        country = "Germany",
                        fax = "1223456",
                        firstName = "Klaus",
                        housenumber = "100a",
                        legalForm = 2,
                        maidenName = "MaidenName1",
                        mobile = "01701233456",
                        name = "Lustig",
                        sex = 1,
                        street = "Fiktivstraße 3",
                        telephone = "0401234567",
                        zip = "21031"
                    },
                    addressId = 1,
                    character = 2,
                    confidence = 3,
                    correction = new AddressCorrection()
                    {
                        city = "Berlin",
                        corrCity = 2,
                        corrHousenumber = 12,
                        corrStreet = 15,
                        corrZip = 10000,
                        housenumber = "12",
                        street = "Berlin Hauptstraße",
                        zip = "10000"
                    },
                    difference = 2,
                    similarity = 4,
                    status = 1
                },
                transactionError = new TransactionError()
                {
                    code = 1,
                    text = "Fehlercode 1"
                },
                verificationDecision = 1
            };

            DeltavistaWSDaoMock.SetReturnValue("getAddressId", AdressIdentificationResponse);
            DeltavistaOutDto DeltavistaOutDto = DeltavistaBo.getIdentifiedAddress(DeltavistaInDto).DeltavistaOutDto;
            Assert.AreEqual(0, DeltavistaOutDto.Capital);
        }

        /// <summary>
        /// Blackboxtest für die getCompanyDetails Methode welche mit einem DeltavistaInDto augerufen wird
        /// </summary>
        [Test]
        public void getCompanyDetailsByAddressIdTest()
        {
            DeltavistaInDto DeltavistaInDto = new DeltavistaInDto()
            {
                AddressDescription = new DVAddressDescriptionDto()
                {
                    Birthdate = "22.12.1989",
                    City = "Hamburg",
                    Country = "Germany",
                    Fax = "1223456",
                    FirstName = "Klaus",
                    Housenumber = "100a",
                    LegalForm = 2,
                    MaidenName = "MaidenName1",
                    Mobile = "01701233456",
                    Name = "Lustig",
                    Sex = 1,
                    Street = "Fiktivstraße 3",
                    Telephone = "0401234567",
                    Zip = "21031"
                },
                AddressId = 100
            };

            CompanyDetailsResponse CompanyDetailsResponse = new CompanyDetailsResponse()
            {
                auditingCompany = "auditing Company",
                capital = 3,
                capitalPayed = 100,
                chNumber = "123456",
                companyStatus = 5,
                contactList = new ContactDescription[]
                {
                    new ContactDescription()
                    {
                        contactDetails = "Kontakt Details hier",
                        contactType = 2,
                        dateLastVerified = "01.01.2011"
                    },

                    new ContactDescription()
                    {
                        contactDetails = "Neue Kontaktdetails hier",
                        contactType = 5,
                        dateLastVerified = "01.01.2010",
                    }
                },
                dateEntry = "Daten Eingang",
                dateKnownSince = "Daten sind bekannt seid",
                dateLastVerified = "01.01.2011",
                foundingDate = "01.12.1990",
                hqAddress = new AddressDescription()
                {
                    birthdate = "22.12.1989",
                    city = "Hamburg",
                    country = "Germany",
                    fax = "1223456",
                    firstName = "Klaus",
                    housenumber = "100a",
                    legalForm = 2,
                    maidenName = "MaidenName1",
                    mobile = "01701233456",
                    name = "Lustig",
                    sex = 1,
                    street = "Hauptquartierstraße",
                    telephone = "0401234567",
                    zip = "21031"
                },
                keyValueList = new KeyValueItem[]
                {
                    new KeyValueItem()
                    {
                        key = "Schlüßel 1",
                        value = "Wert zu Schlüßel 1"
                    },

                    new KeyValueItem()
                    {
                        key = "Schlüßel 2",
                        value = "Wert zu Schlüßel 2"
                    }
                },
                lastShabDate = "Letztes Shab Datum",
                lastShabPublication = "Letzte Shab veröffentlichung",
                leaderShipSize = 100,
                leaderShipSizeNeg = 10,
                managementList = new ManagementMember[]
                {
                    new ManagementMember()
                    {
                        address = new AddressDescription()
                        {
                            birthdate = "09.02.1990",
                            city = "Münsingen",
                            country = "Germany",
                            fax = "6543211",
                            firstName = "Peter",
                            housenumber = "10a",
                            legalForm = 1,
                            maidenName = "MaidenName100",
                            mobile = "01701233434234",
                            name = "Wichtig",
                            sex = 2,
                            street = "Staßenstraße 3",
                            telephone = "23124325423567",
                            zip = "55567"
                        },
                        endDate = "12.12.2012",
                        functionName = "Funktionsname",
                        hometown = "München",
                        signatureRight = 100,
                        startDate = "01.01.2011"
                    },

                    new ManagementMember()
                    {
                        address = new AddressDescription()
                        {
                            birthdate = "19.12.1990",
                            city = "Bergheim",
                            country = "Germany",
                            fax = "11111",
                            firstName = "Dieter",
                            housenumber = "1",
                            legalForm = 4,
                            maidenName = "MaidenName2",
                            mobile = "01709999999",
                            name = "Nachname",
                            sex = 1,
                            street = "Straße 1337",
                            telephone = "1234",
                            zip = "66567"
                        },
                        endDate = "02.11.2011",
                        functionName = "Funktionsname2",
                        hometown = "Hannover",
                        signatureRight = 10,
                        startDate = "01.01.2009"
                    }
                    
                },
                managementSize = 2,
                noga08Description = "Noga08Beschreibung",
                nogaCode08 = "NogaCode08",
                numberOfEmployees = 20,
                numberOfShares = 10,
                purpose = "Purpose",
                samePhoneList = new AddressDescription[]
                {
                    new AddressDescription()
                    {
                        birthdate = "01.01.1970",
                        city = "Berlin",
                        country = "Germany",
                        fax = "22222",
                        firstName = "Dieter",
                        housenumber = "1",
                        legalForm = 4,
                        maidenName = "MaidenName2",
                        mobile = "01709999999",
                        name = "Nachname",
                        sex = 1,
                        street = "Straße 1337",
                        telephone = "1234",
                        zip = "66567"
                    },
                    new AddressDescription()
                    {
                        birthdate = "19.12.1990",
                        city = "Bergheim",
                        country = "Germany",
                        fax = "11111",
                        firstName = "Dieter",
                        housenumber = "1",
                        legalForm = 4,
                        maidenName = "MaidenName2",
                        mobile = "01709999999",
                        name = "Nachname",
                        sex = 1,
                        street = "Straße 1337",
                        telephone = "1234",
                        zip = "66567"
                    }
                },
                transactionError = new TransactionError()
                {
                    code = 1,
                    text = "Hier ist der Fehler 1 zu sehen"
                }
            };

            DeltavistaWSDaoMock.SetReturnValue("getCompanyDetailsByAddressId", CompanyDetailsResponse);
            DeltavistaOutDto DeltavistaOutDto = DeltavistaBo.getCompanyDetailsByAddressId(DeltavistaInDto).DeltavistaOutDto;
            Assert.AreEqual("Hauptquartierstraße", DeltavistaOutDto.HqAddress.Street);
            Assert.AreEqual("Nachname", DeltavistaOutDto.SamePhoneList[0].Name);
        }

        /// <summary>
        /// Blackboxtest für die getDebtDetails Methode welche mit einem DeltavistaInDto aufgerufen wird
        /// </summary>
        [Test]
        public void getDebtDetailsByAddressIdTest()
        {
            DeltavistaInDto DeltavistaInDto = new DeltavistaInDto()
            {
                AddressDescription = new DVAddressDescriptionDto()
                {
                    Birthdate = "22.12.1989",
                    City = "Hamburg",
                    Country = "Germany",
                    Fax = "1223456",
                    FirstName = "Klaus",
                    Housenumber = "100a",
                    LegalForm = 2,
                    MaidenName = "MaidenName1",
                    Mobile = "01701233456",
                    Name = "Lustig",
                    Sex = 1,
                    Street = "Fiktivstraße 3",
                    Telephone = "0401234567",
                    Zip = "21031"
                },
                AddressId = 100
            };

            DebtDetailsResponse DebtDetailsResponse = new DebtDetailsResponse()
            {
                debtList = new DebtEntry[]
                {
                    new DebtEntry()
                    {
                        amount = 1000,
                        amountOpen = 100,
                        dateClose = "Daten geschloßen",
                        dateOpen = "Daten offen",
                        debtType = 1,
                        origin = "Origin1",
                        paymentStatus = 1,
                        riskClass = 100,
                        text = "Test der Schulden"
                    },

                    new DebtEntry()
                    {
                        amount = 2000,
                        amountOpen = 200,
                        dateClose = "Daten geschloßen1",
                        dateOpen = "Daten offen1",
                        debtType = 2,
                        origin = "Origin2",
                        paymentStatus = 2,
                        riskClass = 500,
                        text = "Test der Schulden2"
                    }
                },
                returnCode = 1,
                transactionError = new TransactionError()
                {
                    code = 0,
                    text = "Nichts"
                }
            };

            DeltavistaWSDaoMock.SetReturnValue("getDebtDetailsByAddressId", DebtDetailsResponse);
            DeltavistaOutDto DeltavistaOutDto = DeltavistaBo.getDebtDetailsByAddressId(DeltavistaInDto).DeltavistaOutDto;
            Assert.AreEqual("Nichts", DeltavistaOutDto.TransactionError.Text);
            Assert.AreEqual(2000, DeltavistaOutDto.DebtList[1].Amount);
        }
    }
}
