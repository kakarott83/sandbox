using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
{
    /// <summary>
    /// Testklasse für ZekBo
    /// </summary>
    [TestFixture()]
    public class ZekBoTest
    {
        DynamicMock zekWSDaoMock;
        DynamicMock zekDBDaoMock;
        DynamicMock auskunftDaoMock;
        ZekBo zekBo;

        /// <summary>
        /// Initialisierung der standard Parameter und der Mocks
        /// </summary>
        [SetUp]
        public void ZekBoInit()
        {
            zekDBDaoMock = new DynamicMock(typeof(IZekDBDao));
            zekWSDaoMock = new DynamicMock(typeof(IZekWSDao));
            auskunftDaoMock = new DynamicMock(typeof(IAuskunftDao));
            zekBo = new ZekBo((IZekWSDao)zekWSDaoMock.MockInstance, (IZekDBDao)zekDBDaoMock.MockInstance, (IAuskunftDao)auskunftDaoMock.MockInstance);
        }

        /// <summary>
        /// Blackboxtest für die KreditgesuchNeu Methode die ein ZekOutDto zurück liefert
        /// </summary>
        [Test]
        public void kreditgesuchNeuTestWithInDto()
        {
            ZekInDto zekInDto = new ZekInDto()
            {
                Ablehnungsgrund = 1,
                Anfragegrund = 1,
                DatumAblehnung = "01.01.2010",
                KreditgesuchID = "10",
                PreviousKreditgesuchID = "",
                RequestEntities = new List<ZekRequestEntityDto>
                {
                    new ZekRequestEntityDto()
                    {
                        AddressDescription = new ZekAddressDescriptionDto()
                        {
                            Birthdate = "22.12.1989",
                            City = "Hamburg",
                            Country = "Germany",
                            DatumWohnhaftSeit = "22.12.1989",
                            FirmaZusatz = null,
                            FirstName = "Sydney",
                            FoundingDate = null,
                            Housenumber = "1a",
                            KundenId = "1337",
                            LegalForm = 1,
                            MaidenName = null,
                            Name = "Döffert",
                            Nationality = "German",
                            NogaCode = "02",
                            Profession = "Förster",
                            Sex = 1,
                            Zip = "21031",
                            ZipAdd = null,
                            Zivilstandscode = 1 
                        },
                        DebtorRole = 0,
                        ForceNewAddress = 0,
                        PreviousReturnCode = null,
                        RefNo = 1337
                    }
                },
                RequestEntity = null,
                Zielverein = 1
            };

            IdentityDescriptor identityDescriptor = new IdentityDescriptor()
            {
                clientUserId = "1337",
                name = "Doeffert1337",
                password = "pw123"
            };

            CommonMultiResponse commonMultiResponse = new CommonMultiResponse()
            {
                kreditGesuchID = "10",
                kreditVertragID = "11",
                responses = new ResponseDescription[]
                {
                    new ResponseDescription()
                    {
                        foundContracts = new FoundContracts()
                        {
                            amtsinformationContracts = null,
                            bardarlehenContracts = null,
                            festkreditContracts = null,
                            gesamtEngagement = null,
                            kartenengagementContracts = null,
                            karteninformationContracts = null,
                            kontokorrentkreditContracts = null,
                            kreditGesuchContracts = null,
                            leasingMietvertragContracts = null,
                            solidarschuldnerContracts = null,
                            teilzahlungskreditContracts = null,
                            ueberziehungskreditContracts = null
                        },
                        foundPerson = new AddressDescription()
                        {
                            birthdate = "22.12.1989",
                            city = "Hamburg",
                            country = "Germany",
                            datumWohnhaftSeit = "22.12.1989",
                            firmaZusatz = null,
                            firstname = "Sydney",
                            foundingDate = null,
                            housenumber = "1a",
                            kundenId = "1337",
                            legalForm = 1,
                            maidenName = null,
                            name = "Döffert",
                            nationality = "German",
                            nogaCode = "02",
                            profession = "Förster",
                            sex = 1,
                            zip = "21031",
                            zipAdd = null,
                            zivilstandscode = 1,
                            street = "Straße"
                        },
                        refNo = 1337,
                        returnCode = new ReturnCode()
                        {
                            code = 0,
                            text = "",
                        },
                        synonymes = null
                    }
                },
            };

            zekDBDaoMock.SetReturnValue("GetIdentityDescriptor", identityDescriptor);
            zekWSDaoMock.SetReturnValue("kreditgesuchNeu", commonMultiResponse);
            ZekOutDto zekOutDto = zekBo.kreditgesuchNeu(zekInDto).ZekOutDto;
            Assert.AreEqual("10" , zekOutDto.KreditgesuchID);
            Assert.AreEqual("11", zekOutDto.KreditVertragID);
            Assert.AreEqual("1337", zekOutDto.Responses[0].FoundPerson.KundenId);
        }

        /// <summary>
        /// Blackboxtest für die Informativabfrage Methode welche ein ZekOutDto zurück liefert
        /// </summary>
        [Test]
        public void informativabfrageTestWithInDto()
        {
            ZekInDto zekInDto = new ZekInDto()
            {
                Ablehnungsgrund = 1,
                Anfragegrund = 1,
                DatumAblehnung = "01.01.2010",
                KreditgesuchID = "10",
                PreviousKreditgesuchID = "",
                RequestEntities = new List<ZekRequestEntityDto>
                {
                    new ZekRequestEntityDto()
                    {
                        AddressDescription = new ZekAddressDescriptionDto()
                        {
                            Birthdate = "22.12.1989",
                            City = "Hamburg",
                            Country = "Germany",
                            DatumWohnhaftSeit = "22.12.1989",
                            FirmaZusatz = null,
                            FirstName = "Sydney",
                            FoundingDate = null,
                            Housenumber = "1a",
                            KundenId = "1337",
                            LegalForm = 1,
                            MaidenName = null,
                            Name = "Döffert",
                            Nationality = "German",
                            NogaCode = "02",
                            Profession = "Förster",
                            Sex = 1,
                            Zip = "21031",
                            ZipAdd = null,
                            Zivilstandscode = 1 
                        },
                        DebtorRole = 0,
                        ForceNewAddress = 0,
                        PreviousReturnCode = null,
                        RefNo = 1337
                    }
                },
                RequestEntity = new ZekRequestEntityDto()
                {
                    AddressDescription = new ZekAddressDescriptionDto()
                    {
                        Birthdate = "22.12.1989",
                        City = "Hamburg",
                        Country = "Germany",
                        DatumWohnhaftSeit = "22.12.1989",
                        FirmaZusatz = null,
                        FirstName = "Sydney",
                        FoundingDate = null,
                        Housenumber = "1a",
                        KundenId = "1337",
                        LegalForm = 1,
                        MaidenName = null,
                        Name = "Döffert",
                        Nationality = "German",
                        NogaCode = "02",
                        Profession = "Förster",
                        Sex = 1,
                        Zip = "21031",
                        ZipAdd = null,
                        Zivilstandscode = 1
                    },
                    DebtorRole = 0,
                    ForceNewAddress = 0,
                    PreviousReturnCode = null,
                    RefNo = 1337
                },
                Zielverein = 1
            };

            IdentityDescriptor identityDescriptor = new IdentityDescriptor()
            {
                clientUserId = "1337",
                name = "Doeffert1337",
                password = "pw123"
            };

            InfoResponse infoResponse = new InfoResponse()
            {
                foundContracts = new FoundContracts()
                {
                    amtsinformationContracts = null,
                    bardarlehenContracts = null,
                    festkreditContracts = null,
                    gesamtEngagement = null,
                    kartenengagementContracts = null,
                    karteninformationContracts = null,
                    kontokorrentkreditContracts = null,
                    kreditGesuchContracts = null,
                    leasingMietvertragContracts = null,
                    solidarschuldnerContracts = null,
                    teilzahlungskreditContracts = null,
                    ueberziehungskreditContracts = null
                },
                foundPerson = new AddressDescription()
                {
                    birthdate = "22.12.1989",
                    city = "Hamburg",
                    country = "Germany",
                    datumWohnhaftSeit = "22.12.1989",
                    firmaZusatz = null,
                    firstname = "Sydney",
                    foundingDate = null,
                    housenumber = "1a",
                    kundenId = "1337",
                    legalForm = 1,
                    maidenName = null,
                    name = "Döffert",
                    nationality = "German",
                    nogaCode = "02",
                    profession = "Förster",
                    sex = 1,
                    zip = "21031",
                    zipAdd = null,
                    zivilstandscode = 1,
                    street = "Straße"
                },
                returnCode = new ReturnCode()
                {
                    code = 0,
                    text = "",
                },
                synonymes = null            
            };

            zekDBDaoMock.SetReturnValue("GetIdentityDescriptor", identityDescriptor);
            zekWSDaoMock.SetReturnValue("informativabfrage", infoResponse);
            ZekOutDto zekOutDto = zekBo.informativabfrage(zekInDto).ZekOutDto;
            Assert.IsNull(zekOutDto.Synonyms);
            Assert.AreEqual("Döffert", zekOutDto.FoundPerson.Name);
            Assert.AreEqual(0, zekOutDto.ReturnCode.Code);
        }

        /// <summary>
        /// Blackboxtest für die KreditgesuchAblehnen Methode welche ein ZekOutDto zurück liefert
        /// </summary>
        [Test]
        public void kreditgesuchAblehnenTestWithInDto()
        {
            ZekInDto zekInDto = new ZekInDto()
            {
                Ablehnungsgrund = 1,
                Anfragegrund = 1,
                DatumAblehnung = "01.01.2010",
                KreditgesuchID = "10",
                PreviousKreditgesuchID = "",
                RequestEntities = new List<ZekRequestEntityDto>
                {
                    new ZekRequestEntityDto()
                    {
                        AddressDescription = new ZekAddressDescriptionDto()
                        {
                            Birthdate = "22.12.1989",
                            City = "Hamburg",
                            Country = "Germany",
                            DatumWohnhaftSeit = "22.12.1989",
                            FirmaZusatz = null,
                            FirstName = "Sydney",
                            FoundingDate = null,
                            Housenumber = "1a",
                            KundenId = "1337",
                            LegalForm = 1,
                            MaidenName = null,
                            Name = "Döffert",
                            Nationality = "German",
                            NogaCode = "02",
                            Profession = "Förster",
                            Sex = 1,
                            Zip = "21031",
                            ZipAdd = null,
                            Zivilstandscode = 1 
                        },
                        DebtorRole = 0,
                        ForceNewAddress = 0,
                        PreviousReturnCode = null,
                        RefNo = 1337
                    }
                },
                RequestEntity = new ZekRequestEntityDto()
                {
                    AddressDescription = new ZekAddressDescriptionDto()
                    {
                        Birthdate = "22.12.1989",
                        City = "Hamburg",
                        Country = "Germany",
                        DatumWohnhaftSeit = "22.12.1989",
                        FirmaZusatz = null,
                        FirstName = "Sydney",
                        FoundingDate = null,
                        Housenumber = "1a",
                        KundenId = "1337",
                        LegalForm = 1,
                        MaidenName = null,
                        Name = "Döffert",
                        Nationality = "German",
                        NogaCode = "02",
                        Profession = "Förster",
                        Sex = 1,
                        Zip = "21031",
                        ZipAdd = null,
                        Zivilstandscode = 1
                    },
                    DebtorRole = 0,
                    ForceNewAddress = 0,
                    PreviousReturnCode = null,
                    RefNo = 1337
                },
                Zielverein = 1
            };

            IdentityDescriptor identityDescriptor = new IdentityDescriptor()
            {
                clientUserId = "1337",
                name = "Doeffert1337",
                password = "pw123"
            };

            CreditClaimRejectionResponse creditClaimRejectionResponse = new CreditClaimRejectionResponse()
            {
                returnCode = new ReturnCode()
                {
                    code = 1,
                    text = "Rejected"
                },
                synonymes = new AddressDescription[]
                {
                    new AddressDescription()
                    {
                        birthdate = "22.12.1989",
                        city = "Hamburg",
                        country = "Germany",
                        datumWohnhaftSeit = "22.12.1989",
                        firmaZusatz = null,
                        firstname = "Sydney",
                        foundingDate = null,
                        housenumber = "1a",
                        kundenId = "1337",
                        legalForm = 1,
                        maidenName = null,
                        name = "Döffert",
                        nationality = "German",
                        nogaCode = "02",
                        profession = "Förster",
                        sex = 1,
                        zip = "21031",
                        zipAdd = null,
                        zivilstandscode = 1,
                        street = "Straße"
                    }
                },
                transactionError = new TransactionError()
                {
                    code = 0,
                    text = "Kein Fehler"
                }
            };

            zekDBDaoMock.SetReturnValue("GetIdentityDescriptor", identityDescriptor);
            zekWSDaoMock.SetReturnValue("kreditgesuchAblehnen", creditClaimRejectionResponse);
            ZekOutDto zekOutDto = zekBo.kreditgesuchAblehnen(zekInDto).ZekOutDto;
            Assert.AreEqual(0, zekOutDto.TransactionError.Code);
            Assert.AreEqual("Döffert", zekOutDto.Synonyms[0].Name);
            Assert.AreEqual("Rejected", zekOutDto.ReturnCode.Text);
        }
    }
}
