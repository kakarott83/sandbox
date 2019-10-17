//TODO: @DP: Fix Test

//namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest.AuskunftTest
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using AutoMapper;
//    using BO.Auskunft.CRIF;
//    using BO.Auskunft.CrifHelper;

//    using CIC.Database.CRIF.EF6.Model;

//    using CrifSoapService;
//    using DAO.Auskunft;
//    using DTO.Auskunft.Crif;
//    using NUnit.Framework;
//    using NUnit.Mocks;

//    using OpenOne.Common.DAO;

//    using Service.DTO;

//    [TestFixture("Crif Bo Test", Ignore = true)]
//    public class CrifBoTest
//    {
//        private ContextFactory contextFactory = new ContextFactory();

//        [Test]
//        public void GetReportBo_DecisionMatrixTest()
//        {
//            //Arrange
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "DECISION",
//                additionalOutput = new KeyValuePair[2]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "Kdecis",
//                        value = "Vdecis"
//                    },
//                    new KeyValuePair()
//                    {
//                        key = "key2",
//                        value = "value2"
//                    },
//                },
//                archivingId = 500,
//                archivingIdSpecified = true,
//                decisionMatrix = new DecisionMatrix()
//                {
//                    creditLimit = "10",
//                    decision = Decision.YELLOW_GREEN,
//                    decisionText = "test",
//                    ratings = new Rating[]
//                    {
//                        new Rating()
//                        {
//                            ratingType = "ratingType1",
//                            rating = "rating1"
//                        },
//                        new Rating()
//                        {
//                            ratingType = "ratingType2",
//                            rating = "rating2"
//                        },
//                    },
//                    subdecisions = new Subdecision[]
//                    {
//                        new Subdecision()
//                        {
//                            decision = Decision.LIGHT_GREEN,
//                            infoText = "info",
//                            type = "type",
//                            value = "value"
//                        },
//                        new Subdecision()
//                        {
//                            decision = Decision.GREEN,
//                            infoText = "info2",
//                            type = "type",
//                            value = "value"
//                        },
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);
//            //a => a.SYSAUSKUNFT == sysAuskunft

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include("CFDECISION") //TODO does not load the table
//                                        .Include("CFDECISION.CFSUBDECISION") // TODO does not load the table
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT == sysAuskunft)
//                                        .OrderByDescending(a => a.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                //CFDECISION cf = new CFDECISION();
//                //bool l = cf.CFSUBDECISION.IsLoaded;

//                Assert.NotNull(outElement);
//                Assert.AreEqual(element.archivingId, outElement.ARCHIVINGID);

//                var additionalOutput = context.CFKEYVALUE.Where(a => a.AREA == "CFOUTGETREPORT" && a.SYSID == outElement.SYSCFOUTGETREPORT);
//                Assert.AreEqual(element.additionalOutput.Length, additionalOutput.Count());

//                Assert.IsNotNull(outElement.CFDECISION); // TODO das funktioniert nicht weil CFDECISION Table wird nicht Included
//                //var ratings = context.CFKEYVALUE.Where(a => a.AREA == "CFDECISION" && a.SYSID == outElement.CFDECISION.SYSCFDECISION).ToList();
//            }
//        }

//        [Test]
//        public void GetListOfReadyOfflineReportsBoTest()
//        {
//            //Arrange
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));
//            var element = new TypeGetListOfReadyOfflineReportsResponse()
//            {
//                additionalOutput = new KeyValuePair[2]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "value"
//                    },
//                    new KeyValuePair()
//                    {
//                        key = "key2",
//                        value = "value2"
//                    },
//                },
//                archivingId = 15,
//                archivingIdSpecified = true,
//                offlineReportIdentifiers = new OfflineReportIdentifier[]
//                {
//                    new OfflineReportIdentifier()
//                    {
//                        orderReferenceNumber = 1,
//                        referenceNumber = "Test"
//                    },
//                    new OfflineReportIdentifier()
//                    {
//                        orderReferenceNumber = 2,
//                        referenceNumber = "Test2"
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetListOfReadyOfflineReports", element);

//            var bo = new CrifGetListOfReadyOfflineReportsBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208717;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();

//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTLISTOFFLINEREPORTS
//                                        .Where(a => a.SYSAUSKUNFT == sysAuskunft)
//                                        .OrderByDescending(a => a.SYSCFOUTLISTOFFLINEREPORTS)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.AreEqual(element.archivingId, outElement.ARCHIVINGID);

//                var additionalOutput = context.CFKEYVALUE.Where(a => a.AREA == "CFOUTLISTOFFLINEREPORTS" && a.SYSID == outElement.SYSCFOUTLISTOFFLINEREPORTS);
//                Assert.AreEqual(2, additionalOutput.Count());

//                var offlineReportIdentifiers = context.CFOFFLINEREPORTID.Where(a => a.SYSCFOUTLISTOFFLINEREPORTS == outElement.SYSCFOUTLISTOFFLINEREPORTS);
//                Assert.AreEqual(2, offlineReportIdentifiers.Count());
//            }
//        }

//        [Test]
//        public void GetArchivedReportBoTest()
//        {
//            //Arrange
//            DynamicMock wsDao = new DynamicMock(typeof(ICrifWSDao));

//            TypeGetArchivedReportResponse element = new TypeGetArchivedReportResponse()
//            {
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "value",
//                        key = "key"
//                    },
//                },
//                report = "report",
//                archivingId = 155,
//                archivingIdSpecified = true,
//            };

//            string methodName = "GetArchivedReport";
//            wsDao.SetReturnValue(methodName, element);

//            CrifGetArchivedReportBo bo = new CrifGetArchivedReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208716;

//            Mapper.Initialize(configuration =>
//            {
//                configuration.AddProfile<BankNowModelProfileServices>();
//                configuration.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //ASSERTS
//            using (CrifExtended context = contextFactory.Create<CRIFContext>())
//            {
//                CFOUTARCHIVEDREPORT outElement = context.CFOUTARCHIVEDREPORT
//                                                        .Where(cfoutarchivedreport => cfoutarchivedreport.SYSAUSKUNFT == sysAuskunft)
//                                                        .OrderByDescending(cfoutarchivedreport => cfoutarchivedreport.SYSCFOUTARCHIVEDREPORT)
//                                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.AreEqual(element.archivingId, outElement.ARCHIVINGID);
//                Assert.That(element.report == outElement.REPORT);

//                IQueryable<CFKEYVALUE> additionalOutput = context.CFKEYVALUE.Where(cfkeyvalue => cfkeyvalue.AREA == "CFOUTARCHIVEDREPORT" && cfkeyvalue.SYSID == outElement.SYSCFOUTARCHIVEDREPORT);
//                Assert.AreEqual(1, additionalOutput.Count());
//            }
//        }

//        [Test]
//        public void GetPollOfflineReportBoTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypePollOfflineReportResponseResponse()
//            {
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "value"
//                    },
//                },
//                report = "REPORT",
//                archivingId = 6,
//                archivingIdSpecified = true,
//                debts = new DebtEntry[]
//                {
//                    new DebtEntry()
//                    {
//                        amount = new Amount()
//                        {
//                            amount = 29,
//                            currency = "EUR"
//                        },
//                        dateOpen = "2011-04.04",
//                        paymentStatus = PaymentStatus.IN_PROCESS,
//                        text = "text",
//                        origin = "origin",
//                        riskClass = RiskClass.LEGAL_DEFAULTED,
//                        dateClose = "2010.03.03",
//                        debtType = DebtType.COLLECTION,
//                        amountOpen = new Amount()
//                        {
//                            amount = 10,
//                            currency = "EUR"
//                        },
//                        paymentStatusText = "payed"
//                    },
//                },
//                offlineReportIdentifier = new OfflineReportIdentifier()
//                {
//                    referenceNumber = "ref",
//                    orderReferenceNumber = 9
//                },
//                offlineReportStatus = OfflineReportStatus.PENDING,
//                offlineReportStatusSpecified = true,
//            };

//            wsDao.SetReturnValue("PollOfflineReport", element);

//            var bo = new CrifPollOfflineReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208718;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();

//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            bo.doAuskunft(sysAuskunft);

//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTPOLLOFFLINEREP
//                                        .Include("CFDEBT")
//                                        .Include(cfoutpollofflinerep => cfoutpollofflinerep.CFOFFLINEREPORTID)
//                    //.Include("CFOFFLINEREPORTID")
//                                        .Where(cfoutpollofflinerep => cfoutpollofflinerep.SYSAUSKUNFT == sysAuskunft)
//                                        .OrderByDescending(cfoutpollofflinerep => cfoutpollofflinerep.SYSCFOUTPOLLOFFLINEREP)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);

//                Assert.AreEqual(element.archivingId, outElement.ARCHIVINGID);

//                var additionalOutput = context.CFKEYVALUE.Where(cfkeyvalue => cfkeyvalue.AREA == "CFOUTPOLLOFFLINEREP" && cfkeyvalue.SYSID == outElement.SYSCFOUTPOLLOFFLINEREP);
//                Assert.AreEqual(1, additionalOutput.Count());

//                var offlineReportIdentifiers = context.CFOFFLINEREPORTID.Where(cfofflinereportid => cfofflinereportid.SYSCFOFFLINEREPORTID == outElement.SYSCFOFFLINEREPORTID);
//                Assert.AreEqual(1, offlineReportIdentifiers.Count());

//                var debtEntry = context.CFDEBT.Where(cfdebt => cfdebt.SYSCFOUTPOLLOFFLINEREP == outElement.SYSCFOUTPOLLOFFLINEREP);
//                Assert.AreEqual(1, debtEntry.Count());

//                Assert.NotNull(outElement.CFDEBT);

//                Assert.NotNull(outElement.CFOFFLINEREPORTID);
//            }
//        }

//        [Test]
//        public void GetOfflineReportBoTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeOrderOfflineReportResponse()
//            {
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "KEY",
//                        value = "VALUE"
//                    },
//                },
//                archivingId = 23,
//                archivingIdSpecified = true,
//                orderReferenceNumber = 25
//            };

//            wsDao.SetReturnValue("OrderOfflineReport", element);

//            var bo = new CrifOrderOfflineReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208719;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();

//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            bo.doAuskunft(sysAuskunft);

//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTORDEROFFLINE
//                                        .Where(cfoutorderoffline => cfoutorderoffline.SYSAUSKUNFT == sysAuskunft)
//                                        .OrderByDescending(cfoutorderoffline => cfoutorderoffline.SYSCFOUTORDEROFFLINE)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//            }
//        }

//        [Test]
//        public void GetDebtDetailsBoTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            TypeGetDebtDetailsResponse element = new TypeGetDebtDetailsResponse()
//            {
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "value",
//                        key = "key"
//                    },
//                },
//                archivingId = 22,
//                archivingIdSpecified = true,
//                debts = new DebtEntry[]
//                {
//                    new DebtEntry()
//                    {
//                        amount = new Amount()
//                        {
//                            amount = 23,
//                            currency = "EUR"
//                        },
//                        dateOpen = "2011.04.05",
//                        paymentStatus = PaymentStatus.IN_PROCESS,
//                        text = "text",
//                        origin = "oriri",
//                        riskClass = RiskClass.NO_NEGATIVE,
//                        dateClose = "2003.03.01",
//                        debtType = DebtType.INFORMATION,
//                        amountOpen = new Amount()
//                        {
//                            amount = 11,
//                            currency = "EUR",
//                        },
//                        paymentStatusText = "payment",
//                    },
//                },
//            };

//            wsDao.SetReturnValue("GetDebtDetails", element);

//            var bo = new CrifGetDebtDetailsBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());

//            var sysAuskunft = 208720;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();

//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            bo.doAuskunft(sysAuskunft);

//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTDEBTDETAILS
//                                        .Include("CFDEBT.CFOUTPOLLOFFLINEREP")
//                                        .Where(cfoutdebtdetails => cfoutdebtdetails.SYSAUSKUNFT == sysAuskunft)
//                                        .OrderByDescending(cfoutdebtdetails => cfoutdebtdetails.SYSCFOUTDEBTDETAILS)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//            }
//        }

//        [Test]
//        public void GetReportBo_PublicationTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "valueP",
//                        key = "keyP"
//                    },
//                },
//                report = "reportP",
//                archivingId = 177,
//                archivingIdSpecified = true,
//                reportDetails = new ReportDetails()
//                {
//                    publicationList = new PublicationList()
//                    {
//                        isTruncated = true,
//                        publications = new Publication[]
//                        {
//                            new Publication()
//                            {
//                                text = "text",
//                                language = "de",
//                                publicationCategory = PublicationCategory.CHANGE,
//                                publicationCategoryOriginal = "publication",
//                                publicationRegion = "de",
//                                publicationDate = "2003-07-07",
//                                publicationLabels = null //TODO it is a problem with the Crif specification and Database
//                                //new[]
//                                //{
//                                //    "key",
//                                //    "value"
//                                //},
//                            },
//                        }
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);

//                Assert.NotNull(outElement.CFPUBLICATION);
//            }
//        }

//        [Test]
//        public void GetReportBo_WowRelationTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                archivingId = 12,
//                archivingIdSpecified = true,
//                report = "report",
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "value",
//                        key = "key"
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    whoOwnsWhom = new WhoOwnsWhom()
//                    {
//                        wowRelations = new WowRelation[]
//                        {
//                            new WowRelation()
//                            {
//                                voteShareSpecified = true,
//                                capitalShareSpecified = true,
//                                capitalShare = 2,
//                                voteShare = 3,
//                                isMajorityOwner = true,
//                                ownedSubject = 3,
//                                owner = 1,
//                            },
//                        }
//                    }
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFWOWRELATION);
//            }
//        }

//        [Test]
//        public void GetReportBo_WowAddressTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                archivingId = 55,
//                archivingIdSpecified = true,
//                report = "report",
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "valueWA",
//                        key = "keyWA"
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    whoOwnsWhom = new WhoOwnsWhom()
//                    {
//                        wowAddresses = new WowAddress[]
//                        {
//                            new WowAddress()
//                            {
//                                id = 1,
//                                hasDebtsSpecified = true,
//                                hasDebts = HasDebts.TRUE,
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierText = "ident",
//                                        identifierType = IdentifierType.AT_DVR_NR,
//                                    },
//                                },
//                                address = new CompanyAddressDescription()
//                                {
//                                    coName = "coName",
//                                    companyName = "CiC",
//                                    location = null, //WHY IS THIS NULL ???
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "ctext",
//                                            contactTypeSpecified = true,
//                                            contactType = ContactType.EMAIL,
//                                        },
//                                    }
//                                }
//                            },
//                        }
//                    }
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFWOWADDRESS);
//            }
//        }

//        [Test]
//        public void GetReportBo_AddressHistoryTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "AddresHist",
//                archivingId = 75,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "value"
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    addressHistory = new AddressWithDeliverability[]
//                    {
//                        new AddressWithDeliverability()
//                        {
//                            addressInputDate = "2010-01-01",
//                            deliverability = Deliverability.SMALL,
//                            addressId = new Identifier()
//                            {
//                                identifierText = "YES",
//                                identifierType = IdentifierType.CH_LEGACY_ADDRESS_ID,
//                            },
//                            address = new CompanyAddressDescription()
//                            {
//                                coName = "COCO",
//                                companyName = "COMPANIA",
//                                location = new Location()
//                                {
//                                    city = "Madrid",
//                                    country = "SP",
//                                    houseNumber = "2",
//                                    zip = "13660",
//                                    street = "Avda. de la Estacion",
//                                    regionCode = "MA",
//                                    subRegionCode = "EM",
//                                    apartment = "2",
//                                },
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactText = "Hctext",
//                                        contactTypeSpecified = true,
//                                        contactType = ContactType.EMAIL,
//                                    },
//                                }
//                            }
//                        },
//                    },
//                    whoOwnsWhom = new WhoOwnsWhom()
//                    {
//                        wowAddresses = new WowAddress[]
//                        {
//                            new WowAddress()
//                            {
//                                id = 1,
//                                hasDebtsSpecified = true,
//                                hasDebts = HasDebts.TRUE,
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierText = "ident",
//                                        identifierType = IdentifierType.AT_DVR_NR,
//                                    },
//                                },
//                                address = new CompanyAddressDescription()
//                                {
//                                    coName = "CMP",
//                                    companyName = "CompaniaMother",
//                                    location = null,
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "ctext",
//                                            contactTypeSpecified = true,
//                                            contactType = ContactType.EMAIL,
//                                        },
//                                    }
//                                }
//                            },
//                        },
//                        wowRelations = new WowRelation[]
//                        {
//                            new WowRelation()
//                            {
//                                voteShareSpecified = true,
//                                capitalShareSpecified = true,
//                                capitalShare = 2,
//                                voteShare = 3,
//                                isMajorityOwner = true,
//                                ownedSubject = 3,
//                                owner = 1,
//                            },
//                        }
//                    }
//                }
//            };
//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFADDRESSHISTORY);
//            }
//        }

//        [Test]
//        public void GetReportBo_BranchOfficeListItemTest()
//        {
//            //Arrange
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "report",
//                archivingId = 9,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "value",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    branchOfficeList = new BranchOfficeList()
//                    {
//                        isTruncated = true,
//                        branchOffices = new BranchOfficeListItem[]
//                        {
//                            new BranchOfficeListItem()
//                            {
//                                description = "BRANCH",
//                                registered = NullableBoolean.@true,
//                                registeredSpecified = true,
//                                period = new Period()
//                                {
//                                    startDate = "2011-04-03",
//                                    endDate = "2001-01-01",
//                                },
//                                address = new CompanyAddressDescription()
//                                {
//                                    coName = "coName",
//                                    companyName = "CiCOne",
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "TEXT",
//                                            contactType = ContactType.MOBILE,
//                                            contactTypeSpecified = true,
//                                        },
//                                    },
//                                    location = new Location()
//                                    {
//                                        country = "BR",
//                                        regionCode = "BY",
//                                        subRegionCode = "MUC",
//                                        city = "Berlin",
//                                        zip = "83483",
//                                    },
//                                },
//                            },
//                        },
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFBRANCHOFFICE);
//            }
//        }

//        [Test]
//        public void GetReportBo_DebtEntryTest()
//        {
//            //Arrange
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "CFDEBT",
//                archivingId = 45,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "value",
//                        key = "key",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    debts = new DebtEntry[]
//                    {
//                        new DebtEntry()
//                        {
//                            text = "txtDeb",
//                            origin = "origDeb",
//                            dateOpen = "2011-03-04",
//                            dateClose = "2019-04-04",
//                            paymentStatusText = "ppp",
//                            paymentStatus = PaymentStatus.WRITTEN_OFF,
//                            riskClass = RiskClass.LEGAL_DEFAULTED,
//                            amount = new Amount()
//                            {
//                                amount = 33.56,
//                                currency = "EUR",
//                            },
//                            amountOpen = new Amount()
//                            {
//                                amount = 33.77,
//                                currency = "EUR"
//                            },
//                            debtType = DebtType.COLLECTION,
//                        },
//                    },
//                },
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFDEBT);
//            }
//        }

//        [Test] 
//        public void GetReportBo_SchufaResponseTest()
//        {
//            //Arrange
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "report",
//                archivingId = 22,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "value",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    schufaResponseData = new SchufaResponseData()
//                    {
//                        schufaScore = new SchufaScore()
//                        {
//                            description = "TestA",
//                            scoreCategory = "Category",
//                            scoreText = "TEXT",
//                            scoreError = "ERROR",
//                            scoreValue = "55",
//                            riskQuota = 13,
//                            riskQuotaSpecified = true,
//                            scoreInfoText = null,
//                            //new string[]
//                            //{
//                            //    "one"
//                            //}
//                        },
//                        schufaPersonData = new SchufaPersonData()
//                        {
//                            placeOfBirth = "DE",
//                            title = "TESTB",
//                            previousAddress = new Location()
//                            {
//                                city = "mun",
//                                apartment = "apart",
//                                country = "de",
//                                houseNumber = "1",
//                                regionCode = "BY",
//                                street = "street",
//                                subRegionCode = "by",
//                                zip = "82538",
//                            },
//                        },
//                        schufaIdentification = new SchufaIdentification()
//                        {
//                            personWithoutBirthdate = false,
//                            identityReservationPerson = false,
//                            identityReservationAddress = false,
//                        },
//                        schufaFeatures = new SchufaBaseFeature[]
//                        {
//                            new SchufaFeature()
//                            {
//                                accountNumber = "55",
//                                text = "TESTF",
//                                type = "TESTF",
//                                date = null,
//                                description = "DESCF",
//                                featureCode = "CODEF",
//                                installmentType = "INSTAF",
//                                numberOfInstallments = "7",
//                                featureWithoutBirthdate = NullableBoolean.@true,
//                                featureWithoutBirthdateSpecified = true,
//                                ownFeature = NullableBoolean.@true,
//                                ownFeatureSpecified = true,
//                                amount = new Amount()
//                                {
//                                    currency = "EU",
//                                    amount = 76,
//                                },
//                            },
//                            new SchufaTextFeature()
//                            {
//                                text = "TESTD",
//                                type = "TESTD",
//                                description = "DESCD",
//                                ownFeature = NullableBoolean.@true,
//                                ownFeatureSpecified = true,
//                                featureWithoutBirthdate = NullableBoolean.@true,
//                                featureWithoutBirthdateSpecified = true,
//                                featureCode = "FEATURD",
//                            },
//                            //new SchufaBaseFeature()
//                            //{
//                            //    text = "TESTC",
//                            //    type = "TYPEC",
//                            //    description = "DESCC",
//                            //    featureWithoutBirthdate = NullableBoolean.@false,
//                            //    featureWithoutBirthdateSpecified = true,
//                            //    ownFeature = NullableBoolean.@true,
//                            //    ownFeatureSpecified = true,
//                            //    featureCode = "CODEC",
//                            //},
//                        },
//                    },
//                },
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                //context.ContextOptions.LazyLoadingEnabled = true;

//                var outElement = context.CFOUTGETREPORT
//                                        .Include("CFSCHUFA")
//                                        .Include("CFSCHUFA.CFSCHUFAFEATURE")
//                                        .Include("CFADDRESSMATCH")
//                                        .Include("CFADDRESSMATCH.CFCANDIDATE")
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                //TODO include is just not working, it does not load the tables  :(

//                //context.CFOUTGETREPORT.Include(cfoutgetreport => cfoutgetreport.CFSCHUFA);

//                //var schufa = context.CFSCHUFA.Where(cfschufa => cfschufa.SYSCFSCHUFA == outElement.SYSCFSCHUFA);

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFSCHUFA);
//            }
//        }

//        [Test]
//        public void GetReportBo_OrganizationPositionTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "ORGANIZATION",
//                archivingId = 61,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "key",
//                        value = "Organiz.",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    organizationPositionList = new OrganizationPositionList()
//                    {
//                        isTruncated = true,
//                        organizationPositions = new OrganizationPosition[]
//                        {
//                            new OrganizationPosition()
//                            {
//                                firstName = "DEVA",
//                                name = "PAVA",
//                                nationality = "DE",
//                                homeTown = "GER",
//                                city = "MUNCHEN",
//                                share = 7,
//                                shareSpecified = true,
//                                signatureOriginal = "SIGNAT",
//                                signatureType = SignatureType.WITHOUT,
//                                signatureTypeSpecified = true,
//                                hasDebts = HasDebts.FALSE,
//                                hasDebtsSpecified = true,
//                                nrOfPositionsBankrupt = "3",
//                                nrOfPositions = "9",
//                                period = new Period()
//                                {
//                                    startDate = "1999-04-04",
//                                    endDate = "1999-08-08",
//                                },
//                                birthDate = "1989-04-04",
//                                addressId = new Identifier()
//                                {
//                                    identifierText = "identify",
//                                    identifierType = IdentifierType.SCHUFA_ID,
//                                },
//                                highestFunction = new OrganizationPositionFunction()
//                                {
//                                    functionType = FunctionType.MANAGEMENT,
//                                    functionPriority = "5",
//                                    functionTypeOriginal = "4",
//                                },
//                                //furtherFunctions = new OrganizationPositionFunction[]
//                                //{
//                                //    new OrganizationPositionFunction()
//                                //    {
//                                //        functionType = FunctionType.RECEIVERSHIP,
//                                //        functionPriority = "9",
//                                //        functionTypeOriginal = "5",
//                                //    },
//                                //},
//                                organizationPositions = new OrganizationPosition[]
//                                {
//                                    new OrganizationPosition()
//                                    {
//                                        firstName = "DEVB",
//                                        name = "PAVB",
//                                        nationality = "GR",
//                                        homeTown = "GER",
//                                        city = "BERLIN",
//                                        share = 7,
//                                        shareSpecified = true,
//                                        signatureOriginal = "SIGNAT",
//                                        signatureType = SignatureType.WITHOUT,
//                                        signatureTypeSpecified = true,
//                                        hasDebts = HasDebts.FALSE,
//                                        hasDebtsSpecified = true,
//                                        nrOfPositionsBankrupt = "3",
//                                        nrOfPositions = "9",
//                                        period = new Period()
//                                        {
//                                            startDate = "1999-04-04",
//                                            endDate = "1999-08-08",
//                                        },
//                                        birthDate = "1989-04-04",
//                                        addressId = new Identifier()
//                                        {
//                                            identifierText = "identify",
//                                            identifierType = IdentifierType.SCHUFA_ID,
//                                        },
//                                        highestFunction = new OrganizationPositionFunction()
//                                        {
//                                            functionType = FunctionType.SHAREHOLDERS,
//                                            functionPriority = "5",
//                                            functionTypeOriginal = "4",
//                                        },
//                                        furtherFunctions = new OrganizationPositionFunction[]
//                                        {
//                                            new OrganizationPositionFunction()
//                                            {
//                                                functionType = FunctionType.SUPERVISORY_BOARD,
//                                                functionPriority = "9",
//                                                functionTypeOriginal = "5",
//                                            },
//                                        },
//                                    },
//                                }
//                            }
//                        },
//                    }
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFORGANIZATIONPOSITION);
//            }
//        }

//        [Test]
//        public void GetReportBo_FinancialStatementTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "Fstatement",
//                archivingId = 16,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "keyFIN",
//                        value = "valueFIN",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    financialStatements = new FinancialStatement[]
//                    {
//                        new FinancialStatement()
//                        {
//                            companyName = "GALACTIC",
//                            currencyCode = "EUR",
//                            financialReportingStandard = "Fstandard",
//                            period = new Period()
//                            {
//                                startDate = "1977-05-05",
//                                endDate = "2000-06-06",
//                            },
//                            profitAndLoss = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 23,
//                                    identifierText = "TEXT",
//                                    identifier = "IDENT",
//                                    valueSpecified = true,
//                                    parentId = "1",
//                                    id = 1,
//                                    position = 0,
//                                },
//                            },
//                            balanceSheet = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 66,
//                                    id = 4,
//                                    identifierText = "BALANCE",
//                                    identifier = "ident",
//                                    valueSpecified = true,
//                                    position = 1,
//                                    parentId = "2",
//                                },
//                            },
//                            cashFlow = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 23333,
//                                    identifierText = "CASH",
//                                    id = 4444,
//                                    position = 4,
//                                    parentId = "5",
//                                    identifier = "3",
//                                    valueSpecified = true,
//                                },
//                            },
//                            furtherFigures = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    id = 66,
//                                    value = 77,
//                                    valueSpecified = true,
//                                    position = 5,
//                                    identifierText = "FURTHER",
//                                    parentId = "2",
//                                    identifier = "TEST"
//                                },
//                            },
//                        },
//                    },
//                    //financialStatementsGroup = new FinancialStatement[]
//                    //{
//                    //    new FinancialStatement()
//                    //    {

//                    //    }, 
//                    //}
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFFINSTATEMENT);
//            }
//        }

//        [Test]
//        public void GetReportBo_ScoreAnalysisTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "score",
//                archivingId = 98,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "valueSc",
//                        key = "keySC",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    scoreAnalysis = new ScoreAnalysis()
//                    {
//                        score = 1,
//                        averageScoreAll = "55",
//                        averageScoreIndustry = "3",
//                        scoreScaleRange = new Range()
//                        {
//                            from = 33,
//                            fromSpecified = true,
//                            to = 45,
//                            toSpecified = true,
//                        },
//                        scoreDecisionRanges = new ScoreDecisionRange[]
//                        {
//                            new ScoreDecisionRange()
//                            {
//                                decision = Decision.YELLOW,
//                                scoreRange = new Range()
//                                {
//                                    to = 55,
//                                    toSpecified = true,
//                                    from = 45,
//                                    fromSpecified = true,
//                                },
//                            },
//                        },
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFSCORE)
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFSCORE);
//            }
//        }

//        [Test]
//        public void GetReportBo_IdVerificationresponseDataTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "Verif",
//                archivingId = 19,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "vslue",
//                        key = "jfde",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    idVerificationResponseData = new IdVerificationResponseData()
//                    {
//                        warnings = null
//                        //new string[]
//                        //{
//                        //    "Big", "Mac"
//                        //},
//                        ,
//                        rejectionReasons = null
//                        //new string[]
//                        //{
//                        //    "Mac", "BIG"
//                        //},
//                        ,
//                        processingResult = IdVerificationProcessingResult.MANUALY_APPROVED,
//                        content = new IdVerificationContent()
//                        {
//                            checks = new IdVerificationChecks()
//                            {
//                                isComplete = NullableBoolean.@true,
//                                isCompleteSpecified = true,
//                                isBirthDateVerified = NullableBoolean.@true,
//                                isBirthDateVerifiedSpecified = true,
//                                isCompositeCheckDigitVerified = NullableBoolean.@true,
//                                isCompositeCheckDigitVerifiedSpecified = true,
//                                isDocumentNumberVerified = NullableBoolean.@true,
//                                isDocumentNumberVerifiedSpecified = true,
//                                isExpirationDateVerified = NullableBoolean.@true,
//                                isExpirationDateVerifiedSpecified = true,
//                                isIssuingStateOrOrganizationVerified = NullableBoolean.@true,
//                                isIssuingStateOrOrganizationVerifiedSpecified = true,
//                                isNationalIdentificationNumberVerified = NullableBoolean.@true,
//                                isNationalIdentificationNumberVerifiedSpecified = true,
//                                isValid = NullableBoolean.@true,
//                                isValidSpecified = true,
//                                isNationalityVerified = NullableBoolean.@true,
//                                isNationalityVerifiedSpecified = true,
//                            },
//                            document = new IdVerificationDocument()
//                            {
//                                mrz1 = "mrz1",
//                                mrz2 = "mrz2",
//                                mrz3 = "mrz3",
//                                issuingDate = "1990-04-04",
//                                expirationDate = "1995-09-09",
//                                signDate = "1998-04-04",
//                                validityFromDate = "2004-06-06",
//                                issuingStateOrOrganization = "state",
//                                documentNumber = "34",
//                                documentDescription = "descript",
//                                documentType = IdVerificationDocumentType.UNDETERMINED,
//                                documentTypeSpecified = true,
//                            },
//                            documentImages = new BinaryData[]
//                            {
//                                new BinaryData()
//                                {
//                                    data = "data",
//                                    dataClassification = "classif",
//                                    mimeType = "mime",
//                                },
//                            },
//                            person = new IdVerificationPerson()
//                            {
//                                address1 = "A1",
//                                address2 = "A2",
//                                nationalIdentificationNumber = "455",
//                                nationality = "BE",
//                                placeOfBirth = "BE",
//                                address = new PersonAddressDescription()
//                                {
//                                    firstName = "Dave",
//                                    lastName = "Davidoff",
//                                    birthDate = "1988-05-05",
//                                    coName = "coName",
//                                    sex = Sex.MALE,
//                                    sexSpecified = true,
//                                    maidenName = "maiden",
//                                    middleName = "Doro",
//                                    location = new Location()
//                                    {
//                                        city = "Ma",
//                                        apartment = "apt",
//                                        zip = "82538",
//                                        street = "stein",
//                                        country = "DE",
//                                        houseNumber = "81",
//                                        regionCode = "BY",
//                                        subRegionCode = "MU",
//                                    },
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "type",
//                                            contactTypeSpecified = true,
//                                            contactType = ContactType.WEB,
//                                        },
//                                    },
//                                },
//                            },
//                        },
//                        comparisonElements = new IdVerificationComparisonElement[]
//                        {
//                            new IdVerificationComparisonElement()
//                            {
//                                documentVsMrzSimilarity = "19",
//                                percentage = "10",
//                                providedVsMrzSimilarity = "20",
//                                providedVsDocumentSimilarity = "30",
//                                documentValue = "12",
//                                fieldName = "field",
//                                mrzValue = "16",
//                                providedValue = "3",
//                            },
//                        },
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFVERIFICATION)
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFVERIFICATION);
//            }
//        }

//        [Test]
//        public void GetReportBo_ComplianceCheckedTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "COMPLIANCE",
//                archivingId = 505,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "compliance",
//                        value = "compliance",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    complianceCheckResult = new ComplianceCheckResult()
//                    {
//                        checkedEntity = new ComplianceCheckedEntity()
//                        {
//                            relationType = "COMPLIANCE",
//                            foundEntities = new ComplianceFoundEntity[]
//                            {
//                                new ComplianceFoundEntity()
//                                {
//                                    country = "TestA",
//                                    age = "25",
//                                    crifRefId = 5,
//                                    primaryName = "cfcompliance",
//                                    matchInformation = new ComplianceMatchInformation()
//                                    {
//                                        matchedBirthdate = "1988-09-09",
//                                        confidenceBirthdate = 4,
//                                        confidenceName = 8,
//                                        matchedName = "Philipp"
//                                    },

//                                    //TODO: not mapping string[]
//                                    //additionalInformations = 
//                                    //furtherCountries = 
//                                    //birthdates = 
//                                    //furtherNames = 
//                                    //keywords = 
//                                },
//                            },
//                            checkedEntities = new ComplianceCheckedEntity[]
//                            {
//                                new ComplianceCheckedEntity()
//                                {
//                                    relationType = "TestB",
//                                    foundEntities = new ComplianceFoundEntity[]
//                                    {
//                                        new ComplianceFoundEntity()
//                                        {
//                                            country = "GR",
//                                            age = "27",
//                                            crifRefId = 365,
//                                            primaryName = "DEVID",
//                                            matchInformation = new ComplianceMatchInformation()
//                                            {
//                                                confidenceName = 34,
//                                                matchedName = "Devid",
//                                                confidenceBirthdate = 87,
//                                                matchedBirthdate = "1989-21-04",
//                                            },
//                                            listDescription = new ComplianceListDescription()
//                                            {
//                                                name = "PETROS",
//                                                publishingOrganization = "PETROol",
//                                                dateLastUpdated = "2015-04-04",
//                                                category = "ready",
//                                            },
//                                        },
//                                    },
//                                },
//                            },
//                            checkedAddress = new CompanyAddressDescription()
//                            {
//                                coName = "COMPLIANCE",
//                                companyName = "MTU",
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactText = "COMPLIANCE",
//                                        contactTypeSpecified = true,
//                                        contactType = ContactType.WEB,
//                                    },
//                                },
//                                location = new Location()
//                                {
//                                    country = "AA",
//                                    city = "FD",
//                                    houseNumber = "8",
//                                    street = "mamamama",
//                                    subRegionCode = "MUC",
//                                    regionCode = "BY",
//                                    apartment = "apart",
//                                    zip = "88749",
//                                },
//                            },
//                        }
//                    }
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFCOMPLIANCE)
//                                        .Include("CFCOMPLIANCE.CFADDRESS")
//                                        .Include(cfoutgetreport => cfoutgetreport.CFADDRESSMATCH)
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFCOMPLIANCE);
//            }
//        }

//        [Test]
//        public void GetReportBo_BusinessIndustryLicenseTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "TESTLicense",
//                archivingId = 23,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "keyLic",
//                        value = "keyVal",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    businessIndustryLicenses = new BusinessIndustryLicense[]
//                    {
//                        new BusinessIndustryLicense()
//                        {
//                            period = new Period()
//                            {
//                                startDate = "1999-09-09",
//                                endDate = "2004-08-08",
//                            },
//                            industryName = "FARMA",
//                            issuedTo = "ME",
//                            status = BusinessIndustryLicenseStatus.INACTIVE,
//                            industryCode = new IndustryCode()
//                            {
//                                code = "TESTcode",
//                                codeDescription = "TESTB",
//                                type = "TEST",
//                                isMainIndustryCode = NullableBoolean.@true,
//                                isMainIndustryCodeSpecified = true,
//                                period = new Period()
//                                {
//                                    startDate = "1989-05-05",
//                                    endDate = "1999-04-04",
//                                },
//                            },
//                        },
//                    },
//                }
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFBUSINESSLIC)
//                                        .Include("CFBUSINESSLIC.CFKEYPERIOD")
//                                        .Include(cfoutgetreport => cfoutgetreport.CFCOMPLIANCE)
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFBUSINESSLIC);
//            }
//        }

//        [Test]
//        public void GetReportBo_CompanyBaseDataTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "COMPANY_TEST",
//                archivingId = 91,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "keyCompany",
//                        value = "valueCompany",
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    branchOfficeList = new BranchOfficeList()
//                    {
//                        isTruncated = true,
                        
//                    },
//                    paymentDelay = new PaymentDelay()
//                    {
//                        inTimeRatioSpecified = true,
//                        inTimeRatio = 4,
//                        paymentType = "cash",
//                        paymentExpectedTypeSpecified = true,
//                        paymentExpectedType = PaymentExpectedType.IN_TIME,
//                        avgDelayShortTerm = "2",
//                        avgDelayLongTerm = "5",
//                    },

//                    publicationList = new PublicationList()
//                    {
//                        isTruncated = true,
//                        publications = new Publication[]
//                        {
//                            new Publication()
//                            {
//                                text = "TEST",
//                                language = "LAN",
//                                publicationCategory = PublicationCategory.ANNUAL_BALANCE_SHEET,
//                                publicationCategoryOriginal = "CAT",
//                                publicationDate = "1999-01-01",
//                                publicationRegion = "BY",
//                                publicationLabels = new string[]
//                                {
//                                    "one",
//                                    "two",
//                                },
//                            }, 
//                        },
//                    },
//                    companyBaseData = new CompanyBaseData()
//                    {
//                        activityStatusOriginal = "STATUS",
//                        activityStatus = ActivityStatus.ACTIVE,
//                        legalFormText = "LEGAL",
//                        legalFormType = LegalFormType.LIMITED_COMPANY,
//                        legalFormTypeOriginal = "LIMITED",
//                        registryType = RegistryType.TRADE_LICENCE,
//                        identifiers = new Identifier[]
//                        {
//                            new Identifier()
//                            {
//                                identifierText = "TESTCOMP",
//                                identifierType = IdentifierType.AT_OENB,
//                            },
//                        },
//                        mainAddress = new CompanyAddressDescription()
//                        {
//                            coName = "M.A.N",
//                            companyName = "MAN GmbH",
//                            contactItems = new ContactItem[]
//                            {
//                                new ContactItem()
//                                {
//                                    contactText = "cfc",
//                                    contactTypeSpecified = true,
//                                    contactType = ContactType.MOBILE,
//                                },
//                            },
//                            location = new Location()
//                            {
//                                country = "SP",
//                                city = "Barcelona",
//                                street = "some",
//                                houseNumber = "12",
//                                regionCode = "SP",
//                                subRegionCode = "BS",
//                                apartment = "19",
//                                zip = "7489",
//                            },
//                        },
//                        companyDetailData = new CompanyDetailData()
//                        {
//                            ultimateMotherCompany = null,
//                            activityIndex = ActivityIndex.HIGH,
//                            activityIndexSpecified = true,
//                            dateFinancialStatement = "1990-01-01",
//                            dateFinancialStatementHandedIn = "2011-01-01",
//                            knownSince = "1999-01-01",
//                            turnoverCurrency = "EUR",
//                            turnoverInExport = NullableBoolean.@true,
//                            turnoverInExportSpecified = true,
//                            turnoverRange = new Range()
//                            {
//                                from = 1,
//                                to = 10,
//                                toSpecified = true,
//                                fromSpecified = true,
//                            },
//                            nrOfEmployees = new Range()
//                            {
//                                from = 11,
//                                to = 21,
//                                toSpecified = true,
//                                fromSpecified = true,
//                            },
//                            sizeClass = CompanySizeClass.L,
//                            sizeClassSpecified = true,
//                            industryCodes = new IndustryCode[]
//                            {
//                                new IndustryCode()
//                                {
//                                    code = "COMPANY",
//                                    codeDescription = "COMP",
//                                    type = "TYPE",
//                                    isMainIndustryCode = NullableBoolean.@true,
//                                    isMainIndustryCodeSpecified = true,
//                                    period = new Period()
//                                    {
//                                        startDate = "1998-04-04",
//                                        endDate = "2021-04-04",
//                                    },
//                                },
//                            },
//                            bankAccounts = new BankAccount[]
//                            {
//                                new BankAccount()
//                                {
//                                    currency = "EUR",
//                                    bankDescription = "Postbank",
//                                    iban = "3827432434",
//                                    swiftCode = "483272",
//                                    localAccountNr = "489302",
//                                    bank = new CompanyAddressDescription()
//                                    {
//                                        coName = "BANK",
//                                        companyName = "BANKCOMP",
//                                        location = new Location()
//                                        {
//                                            country = "DE",
//                                            regionCode = "BY",
//                                            subRegionCode = "MUC",
//                                            zip = "4837424",
//                                            city = "Munchen",
//                                            street = "eisenstr.",
//                                            houseNumber = "12",
//                                            apartment = "1",
//                                        },
//                                        contactItems = new ContactItem[]
//                                        {
//                                            new ContactItem()
//                                            {
//                                                contactText = "TEST",
//                                                contactTypeSpecified = true,
//                                                contactType = ContactType.OTHER,
//                                            },
//                                        }
//                                    },
//                                },
//                            },
//                        },
//                        companyRegistrationData = new CompanyRegistrationData()
//                        {
//                            capital = new Amount()
//                            {
//                                amount = 34,
//                                currency = "EUR",
//                            },
//                            capitalInKind = NullableBoolean.@true,
//                            capitalInKindSpecified = true,
//                            capitalPayed = new Amount()
//                            {
//                                amount = 55,
//                                currency = "EUR",
//                            },
//                            foundingDate = "2014-05-05",
//                            purpose = "COMPANY",
//                            hasAuditingCompanySpecified = false,
//                            hasAuditingCompany = AuditingCompanyStatus.NO_AUDITOR_NOT_MANDATORY,
//                            registeredOfficeCity = "BARCA",
//                            auditingCompany = null,
//                        },
//                    },
//                },
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFCOMPANY)
//                                        .Include("CFCOMPANY.CFADDRESS")
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFCOMPANY);
//            }
//        }

//        [Test]
//        public void GetReportBo_CompanyBaseGetReportForeignKeyTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "FOREIGN COMPANY",
//                archivingId = 211,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "KEY",
//                        value = "Fcompany",
//                    },
//                },

//                #region test
//                reportDetails = new ReportDetails()
//                {
//                    #endregion
//                    publicationList = new PublicationList()
//                    {
//                        isTruncated = true,
//                    },
//                    branchOfficeList = new BranchOfficeList()
//                    {
//                        isTruncated = true,
//                    },
//                    paymentDelay = new PaymentDelay()
//                    {
//                        inTimeRatioSpecified = true,
//                        inTimeRatio = 4,
//                        paymentType = "cash",
//                        paymentExpectedTypeSpecified = true,
//                        paymentExpectedType = PaymentExpectedType.IN_TIME,
//                        avgDelayShortTerm = "2",
//                        avgDelayLongTerm = "5",
//                    },
//                    organizationPositionList = new OrganizationPositionList()
//                    {
//                        isTruncated = true,
//                    },
//                    furtherRelations = new FurtherRelations()
//                    {
//                        probableRelations = new CompanyBaseData[]
//                        {
//                            new CompanyBaseData()
//                            {
//                                activityStatusOriginal = "active",
//                                legalFormText = "legal",
//                                legalFormTypeOriginal = "illegal",
//                                legalFormType = LegalFormType.MULTI_PERSON_COMPANY,
//                                registryType = RegistryType.REGISTER_OF_ASSOCIATIONS,
//                                activityStatus = ActivityStatus.IN_LIQUIDATION,
//                                companyDetailData = new CompanyDetailData()
//                                {
//                                    ultimateMotherCompany = null,
//                                    dateFinancialStatement = "2005-04-04",
//                                    activityIndex = ActivityIndex.LOW,
//                                    activityIndexSpecified = true,
//                                    dateFinancialStatementHandedIn = "2001-05-06",
//                                    knownSince = "1990-06-06",
//                                    sizeClass = CompanySizeClass.S,
//                                    sizeClassSpecified = true,
//                                    turnoverCurrency = "EUR",
//                                    turnoverInExport = NullableBoolean.@true,
//                                    turnoverInExportSpecified = true,
//                                    nrOfEmployees = new Range()
//                                    {
//                                        from = 33,
//                                        to = 44,
//                                        fromSpecified = true,
//                                        toSpecified = true,
//                                    },
//                                    turnoverRange = new Range()
//                                    {
//                                        from = 55,
//                                        to = 77,
//                                        toSpecified = true,
//                                        fromSpecified = true,
//                                    },
//                                    industryCodes = new IndustryCode[]
//                                    {
//                                        new IndustryCode()
//                                        {
//                                            code = "Company_Code",
//                                            codeDescription = "CCompany",
//                                            isMainIndustryCode = NullableBoolean.@true,
//                                            isMainIndustryCodeSpecified = true,
//                                            type = "Company",
//                                            period = new Period()
//                                            {
//                                                startDate = "2000-06-06",
//                                                endDate = "2005-06-06",
//                                            },
//                                        },
//                                    },
//                                    bankAccounts = new BankAccount[]
//                                    {
//                                        new BankAccount()
//                                        {
//                                            currency = "EUR",
//                                            bankDescription = "ALPHA",
//                                            swiftCode = "821748372945",
//                                            iban = "5834750423",
//                                            localAccountNr = "8574395743",
//                                            bank = new CompanyAddressDescription()
//                                            {
//                                                location = new Location()
//                                                {
//                                                    country = "AR",
//                                                    city = "Monaco",
//                                                    street = "Kings Way",
//                                                    zip = "483746",
//                                                    regionCode = "By",
//                                                    subRegionCode = "Muc",
//                                                    houseNumber = "15",
//                                                },
//                                                contactItems = new ContactItem[]
//                                                {
//                                                    new ContactItem()
//                                                    {
//                                                        contactText = "COMPANY",
//                                                        contactTypeSpecified = true,
//                                                        contactType = ContactType.PHONE,
//                                                    },
//                                                },
//                                                coName = "MARS",
//                                                companyName = "MILKY",
//                                            },
//                                        },
//                                    },
//                                },
//                                companyRegistrationData = new CompanyRegistrationData()
//                                {
//                                    auditingCompany = null,
//                                    //new CompanyBaseData()
//                                    //{
//                                    //   activityStatusOriginal = "TEST",
//                                    //},
//                                    capitalInKind = NullableBoolean.@true,
//                                    capitalInKindSpecified = true,
//                                    purpose = "FOREIGN COMPANY",
//                                    registeredOfficeCity = "registered",
//                                    foundingDate = "1999-05-05",
//                                    hasAuditingCompanySpecified = false,
//                                    capital = new Amount()
//                                    {
//                                        amount = 33.4,
//                                        currency = "EUR",
//                                    },
//                                    capitalPayed = new Amount()
//                                    {
//                                        amount = 44.5,
//                                        currency = "EUR",
//                                    },
//                                    hasAuditingCompany = AuditingCompanyStatus.NO_AUDITOR_RESIGNED,
//                                },
//                                mainAddress = new CompanyAddressDescription()
//                                {
//                                    location = null,
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "text",
//                                            contactTypeSpecified = true,
//                                            contactType = ContactType.WEB,
//                                        },
//                                    }
//                                },
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierText = "ident",
//                                        identifierType = IdentifierType.AT_OENB,
//                                    },
//                                },
//                            },
//                        }
//                    }
//                }
//            };

//            wsDao.SetReturnValue
//                ("GetReport",
//                    element
//                );

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize
//                (cfg =>
//                {
//                    cfg.AddProfile<BankNowModelProfileServices>();
//                    cfg.AddProfile<AuskunftModelCrifProfile>();
//                }
//                );

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context =
//                new
//                    CrifExtended())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFCOMPANY)
//                                        .Include("CFCOMPANY.CFADDRESS")
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//            }
//        }

//        [Test]
//        public void IdentifyAddressBoTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeIdentifyAddressResponse()
//            {
//                archivingId = 124,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        value = "ADDRESS",
//                        key = "ADDRESS",
//                    },
//                },
//                addressMatchResult = new AddressMatchResult()
//                {
//                    candidates = new Candidate[]
//                    {
//                        new Candidate()
//                        {
//                            address = new CompanyAddressDescription()
//                            {
//                                location = new Location()
//                                {
//                                    city = "Munich",
//                                    country = "DE",
//                                    apartment = "2",
//                                    zip = "82538",
//                                    street = "stein",
//                                    houseNumber = "8",
//                                    regionCode = "By",
//                                    subRegionCode = "MUC",
//                                },
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactText = "trejik",
//                                        contactTypeSpecified = true,
//                                        contactType = ContactType.EMAIL,
//                                    },
//                                },
//                                coName = "CIC",
//                                companyName = "TWIX",
//                            },
//                            candidateRank = 55,
//                            groupId = 23,

//                            //

//                            //    address = new PersonAddressDescription()
//                            //    {
//                            //        location = new Location()
//                            //        {
//                            //            city = "AB",
//                            //            country = "BA",
//                            //            apartment = "23",
//                            //            zip = "438743",
//                            //            street = "342743982",
//                            //            houseNumber = "8",
//                            //            regionCode = "BY",
//                            //            subRegionCode = "MUC",
//                            //        },
//                            //        sex = Sex.FEMALE,
//                            //        sexSpecified = true,
//                            //        birthDate = "1999-04-04",
//                            //        coName = "CO",
//                            //        firstName = "ME",
//                            //        lastName = "MEMEME",
//                            //        maidenName = "SLJJDL",
//                            //        middleName = "JDIS",
//                            //        contactItems = new ContactItem[]
//                            //        {
//                            //            new ContactItem()
//                            //            {
//                            //                contactText = "contact",
//                            //                contactTypeSpecified = true,
//                            //                contactType = ContactType.FAX,
//                            //            },
//                            //        },
//                            //    },
//                            //    candidateRank = 34,
//                            //    groupId = 33,
//                            //    identifiers = new Identifier[]
//                            //    {
//                            //        new Identifier()
//                            //        {
//                            //             identifierText = "er",
//                            //             identifierType = IdentifierType.ADDRESS_ID,
//                            //        },
//                            //    }
//                            //
//                        },
//                    },
//                    addressMatchResultType = AddressMatchResultType.MATCH,
//                    locationIdentification = new LocationIdentification()
//                    {
//                        houseType = "BIG",
//                        locationIdentificationType = LocationIdentificationType.HOUSE_CONFIRMED,
//                        requestLocationNormalized = new Location()
//                        {
//                            city = "AKAKA",
//                            country = "GR",
//                            apartment = "AKAKAKA",
//                            zip = "8739",
//                            street = "6734",
//                            houseNumber = "8",
//                            regionCode = "By",
//                            subRegionCode = "MU",
//                        },
//                    },
//                    character = CharacterType.LAST_NAME_IN_HOUSE,
//                    characterSpecified = true,
//                    nameHint = NameHint.PROMINENT,
//                    nameHintSpecified = true,
//                    foundAddress = new MatchedAddress()
//                    {
//                        identificationType = IdentificationType.IDENTITY_IN_UNIVERSE,
//                        //address = new PersonAddressDescription()
//                        //{
//                        //    location = new Location()
//                        //    {
//                        //        city = "AB",
//                        //        country = "BA",
//                        //        apartment = "23",
//                        //        zip = "438743",
//                        //        street = "342743982",
//                        //        houseNumber = "8",
//                        //        regionCode = "BY",
//                        //        subRegionCode = "MUC",
//                        //    },
//                        //    sex = Sex.FEMALE,
//                        //    sexSpecified = true,
//                        //    birthDate = "1999-04-04",
//                        //    coName = "CO",
//                        //    firstName = "ME",
//                        //    lastName = "MEMEME",
//                        //    maidenName = "SLJJDL",
//                        //    middleName = "JDIS",
//                        //    contactItems = new ContactItem[]
//                        //    {
//                        //        new ContactItem()
//                        //        {
//                        //            contactText = "contact",
//                        //            contactTypeSpecified = true,
//                        //            contactType = ContactType.FAX,
//                        //        },
//                        //    },
//                        //},
//                        //identifiers = new Identifier[]
//                        //{
//                        //    new Identifier()
//                        //    {
//                        //        identifierText = "AR",
//                        //        identifierType = IdentifierType.ADDRESS_ID,
//                        //    },
//                        //}
//                    },
//                },
//            };

//            wsDao.SetReturnValue("IdentifyAddress", element);

//            var bo = new CrifIdentifyAddressBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208696;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTIDENTADDRESS
//                                        .Include(cfoutidentaddress => cfoutidentaddress.CFADDRESSMATCH)
//                                        .Include("CFADDRESSMATCH.CFADDRESS")
//                                        .Where(cfoutidentaddress => cfoutidentaddress.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutidentaddress => cfoutidentaddress.SYSCFOUTIDENTADDRESS)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFADDRESSMATCH);
//            }
//        }

//        [Test]
//        public void GetReportBo_AddressMatchResultTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "ADDRESSMATCH",
//                archivingId = 99,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "KEY",
//                        value = "Address"
//                    },
//                },
//                addressMatchResult = new AddressMatchResult()
//                {
//                    nameHint = NameHint.FICTIONAL,
//                    nameHintSpecified = true,
//                    character = CharacterType.IDENTITY_WITH_CONFLICTING_HOUSE,
//                    characterSpecified = true,
//                    addressMatchResultType = AddressMatchResultType.MATCH,
//                    foundAddress = new MatchedAddress()
//                    {
//                        identificationType = IdentificationType.IDENTITY_IN_HOUSE,
//                        identifiers = new Identifier[]
//                        {
//                            new Identifier()
//                            {
//                                identifierText = "ADDRESS",
//                                identifierType = IdentifierType.SCHUFA_ID,
//                            },
//                        },
//                        address = new CompanyAddressDescription()
//                        {
//                            coName = "NAME",
//                            companyName = "BLUB",
//                            location = new Location()
//                            {
//                                country = "DE",
//                                city = "CI",
//                                zip = "4387493",
//                                regionCode = "LA",
//                            },
//                            contactItems = new ContactItem[]
//                            {
//                                new ContactItem()
//                                {
//                                    contactText = "CONTACT",
//                                    contactTypeSpecified = true,
//                                    contactType = ContactType.PHONE,
//                                },
//                            },
//                        },
//                    },
//                    //locationIdentification = new LocationIdentification()
//                    //{
//                    //    locationIdentificationType = LocationIdentificationType.HOUSE_CONFIRMED,
//                    //    houseType = "DOUBLE",
//                    //    requestLocationNormalized = new Location()
//                    //    {
//                    //        country = "KA",
//                    //        regionCode = "BY",
//                    //        zip = "111111",
//                    //    },
//                    //},
//                    //candidates = new Candidate[]
//                    //{
//                    //    new Candidate()
//                    //    {
//                    //        candidateRank = 4,
//                    //        groupId = 44,
//                    //        identifiers = new Identifier[]
//                    //        {
//                    //            new Identifier()
//                    //            {
//                    //                identifierText = "TESTH",
//                    //                identifierType = IdentifierType.UNIT_ID,
//                    //            },
//                    //        },
//                    //        address = new PersonAddressDescription()
//                    //        {
//                    //            firstName = "GEORG",
//                    //            lastName = "STEFANIDIS",
//                    //            sex = Sex.MALE,
//                    //            sexSpecified = true,
//                    //            location = new Location()
//                    //            {
//                    //                country = "GR",
//                    //                city = "Kreta",
//                    //                zip = "3744",
//                    //                street = "street",
//                    //            },
//                    //            contactItems = new ContactItem[]
//                    //            {
//                    //                new ContactItem()
//                    //                {
//                    //                    contactText = "contact",
//                    //                    contactType = ContactType.OTHER,
//                    //                    contactTypeSpecified = true,
//                    //                },
//                    //            },
//                    //        },
//                    //    },
//                    //},
//                },
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize
//                (cfg =>
//                {
//                    cfg.AddProfile<BankNowModelProfileServices>();
//                    cfg.AddProfile<AuskunftModelCrifProfile>();
//                }
//                );

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Include(cfoutgetreport => cfoutgetreport.CFADDRESSMATCH)
//                                        .Include("CFADDRESSMATCH.CFADDRESS")
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//                Assert.NotNull(outElement.CFADDRESSMATCH);
//            }
//        }

//        [Test]
//        public void MappingSchufaFeatureTest()
//        {
//            var features = new SchufaBaseFeature[]
//            {
//                new SchufaFeature()
//                {
//                    type = "FEATURE",
//                    text = "SCHUFA",
//                    ownFeature = NullableBoolean.@true,
//                    ownFeatureSpecified = true,
//                    featureWithoutBirthdate = NullableBoolean.@true,
//                    featureWithoutBirthdateSpecified = true,
//                    accountNumber = "67",
//                    date = "2001-09-09",
//                    numberOfInstallments = "39",
//                    installmentType = "installed",
//                    featureCode = "CODE",
//                    description = "DESCRIPT",
//                    amount = new Amount()
//                    {
//                        amount = 99,
//                        currency = "EUR"
//                    },
//                },
//                new SchufaTextFeature()
//                {
//                    type = "TxtFeature",
//                    text = "Schufa",
//                    description = "TEST",
//                    featureWithoutBirthdate = NullableBoolean.@true,
//                    featureWithoutBirthdateSpecified = true,
//                    ownFeature = NullableBoolean.@true,
//                    ownFeatureSpecified = true,
//                    featureCode = "Fcode",
//                },
//                //new SchufaBaseFeature()
//                //{
//                //    type = "Type",
//                //    text = "Text",
//                //    description = "Desc",
//                //    featureWithoutBirthdate = NullableBoolean.@true,
//                //    featureWithoutBirthdateSpecified = true,
//                //    ownFeature = NullableBoolean.@true,
//                //    ownFeatureSpecified = true,
//                //    featureCode = "feature",
//                //},
//            };


//            Mapper.CreateMap<SchufaBaseFeature, CFSCHUFAFEATURE>()
//                  .ConstructUsing(feature =>
//                  {
//                      var schufaFeature = feature as SchufaFeature;
//                      var schufaTextFeature = feature as SchufaTextFeature;
//                      CFSCHUFAFEATURE result = new CFSCHUFAFEATURE();

//                      if (schufaFeature != null)
//                      {
//                          Mapper.Map(schufaFeature, result);
//                      }
//                      if (schufaTextFeature != null)
//                      {
//                          Mapper.Map(schufaTextFeature, result);
//                      }
//                      return result;
//                  })
//                  .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.Ignore())
//                  .ForMember(dest => dest.OWNFEATURE, opt => opt.Ignore())
//                ;

//            Mapper.CreateMap<SchufaFeature, CFSCHUFAFEATURE>()
//                  .ForMember(dest => dest.AMOUNT, opt => opt.Ignore())
//                  .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.Ignore())
//                  .ForMember(dest => dest.OWNFEATURE, opt => opt.Ignore())
//                  .ForMember(dest => dest.DATE, opt => opt.Ignore())
//                  .ForMember(dest => dest.NUMBEROFINSTALLEMENTS, opt => opt.Ignore())
//                  .ForMember(dest => dest.INSTALLMENTTYPE, opt => opt.MapFrom(src => src.installmentType))
//                  //.AfterMap((dto, cfschufafeature) => { Mapper.Map(dto.amount, cfschufafeature); })
//                ;

//            Mapper.CreateMap<SchufaTextFeature, CFSCHUFAFEATURE>()
//                  .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.Ignore())
//                  .ForMember(dest => dest.OWNFEATURE, opt => opt.Ignore())
//                ;

//            var config = Mapper.Configuration;
//            var elements = Mapper.Map(features, new List<CFSCHUFAFEATURE>());
//            var elements2 = Mapper.DynamicMap<List<CFSCHUFAFEATURE>>(features);

//            var elements3 = new List<CFSCHUFAFEATURE>();
//            foreach (var feature in features)
//            {
//                var element = Mapper.Map(feature, new CFSCHUFAFEATURE());
//                //var element2 = Mapper.DynamicMap(feature, new CFSCHUFAFEATURE());
//                elements3.Add(element);
//            }
//        }

//        /// <summary>
//        /// This tests the whole GetReport Table
//        /// </summary>
//        [Test]
//        public void GetReportBoTest()
//        {
//            var wsDao = new DynamicMock(typeof(ICrifWSDao));

//            var element = new TypeGetReportResponse()
//            {
//                report = "GetReport",
//                archivingId = 100,
//                archivingIdSpecified = true,
//                additionalOutput = new KeyValuePair[]
//                {
//                    new KeyValuePair()
//                    {
//                        key = "8",
//                        value = "GetTest",
//                    },
//                },

//                //###### CFDECISION ########
//                decisionMatrix = new DecisionMatrix()
//                {
//                    creditLimit = "5",
//                    decisionText = "Decision",
//                    decision = Decision.YELLOW_GREEN,
//                    ratings = new Rating[]
//                    {
//                        new Rating()
//                        {
//                            rating = "1",
//                            ratingType = "3",
//                        },
//                    },
//                    subdecisions = new Subdecision[]
//                    {
//                        new Subdecision()
//                        {
//                            value = "value",
//                            type = "type",
//                            infoText = "INFO",
//                            decision = Decision.ORANGE,
//                        },
//                    },
//                },

//                //###### CFADDRESSMATCH ###########
//                addressMatchResult = new AddressMatchResult()
//                {
//                    addressMatchResultType = AddressMatchResultType.CANDIDATES,
//                    character = CharacterType.CITY_CONFIRMED,
//                    characterSpecified = true,
//                    nameHint = NameHint.PARTIAL,
//                    nameHintSpecified = true,
//                    locationIdentification = new LocationIdentification()
//                    {
//                        houseType = "HOUSE",
//                        locationIdentificationType = LocationIdentificationType.STREET_CONFIRMED_HOUSE_NOT_PROVIDED,
//                        requestLocationNormalized = null, //TODO: check this out 
//                    },
//                    foundAddress = new MatchedAddress()
//                    {
//                        identificationType = IdentificationType.OWNER,
//                        identifiers = new Identifier[]
//                        {
//                            new Identifier()
//                            {
//                                identifierText = "GetRep",
//                                identifierType = IdentifierType.CH_UID,
//                            },
//                        },
//                        address = new CompanyAddressDescription()
//                        {
//                            companyName = "SONY",
//                            coName = "Sony",
//                            location = new Location()
//                            {
//                                country = "DE",
//                                city = "Munich",
//                                street = "Balanstraße 73",
//                                regionCode = "By",
//                                subRegionCode = "MUC",
//                                zip = "81673",
//                                houseNumber = "7",
//                                apartment = "NO",
//                            },
//                            contactItems = new ContactItem[]
//                            {
//                                new ContactItem()
//                                {
//                                    contactType = ContactType.PHONE,
//                                    contactTypeSpecified = true,
//                                    contactText = "089230370",
//                                },
//                            },
//                        },
//                    },
//                    candidates = new Candidate[]
//                    {
//                        new Candidate()
//                        {
//                            candidateRank = 9,
//                            groupId = 11,
//                            identifiers = new Identifier[]
//                            {
//                                new Identifier()
//                                {
//                                    identifierType = IdentifierType.AT_OENB,
//                                    identifierText = "OENB",
//                                },
//                            },
//                            address = new PersonAddressDescription()
//                            {
//                                firstName = "Marco",
//                                lastName = "Polo",
//                                birthDate = "1254-07-07",
//                                sex = Sex.MALE,
//                                sexSpecified = true,
//                                maidenName = "NO",
//                                middleName = "NO",
//                                coName = "NO",
//                                location = new Location()
//                                {
//                                    country = "Italia",
//                                    city = "Venedig",
//                                    zip = "30100",
//                                    regionCode = "IT",
//                                    subRegionCode = "VE",
//                                    street = "Santa Lucia",
//                                    houseNumber = "18",
//                                    apartment = "NO",
//                                },
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactType = ContactType.OTHER,
//                                        contactTypeSpecified = true,
//                                        contactText = "Dove",
//                                    },
//                                },
//                            },
//                        },
//                    },
//                },
//                reportDetails = new ReportDetails()
//                {
//                    // ######## CFDEBT #########
//                    debts = new DebtEntry[]
//                    {
//                        new DebtEntry()
//                        {
//                            debtType = DebtType.INFORMATION,
//                            text = "SONY",
//                            origin = "Money",
//                            paymentStatusText = "OK",
//                            dateOpen = "1999-06-06",
//                            dateClose = "2017-01-01",
//                            riskClass = RiskClass.PRE_LEGAL,
//                            paymentStatus = PaymentStatus.WRITTEN_OFF,
//                            amount = new Amount()
//                            {
//                                amount = 189,
//                                currency = "EUR"
//                            },
//                            amountOpen = new Amount()
//                            {
//                                amount = 289,
//                                currency = "EUR"
//                            },
//                        },
//                    },

//                    // ###### CFSCHUFA #########
//                    schufaResponseData = new SchufaResponseData()
//                    {
//                        schufaScore = new SchufaScore()
//                        {
//                            scoreError = "NO",
//                            scoreValue = "2",
//                            scoreText = "Two",
//                            description = "SCORE",
//                            scoreCategory = "IDON",
//                            riskQuota = 19,
//                            riskQuotaSpecified = true,
//                            scoreInfoText = new[]
//                            {
//                                "SinfoText1",
//                                "SinfoText2"
//                            },
//                        },
//                        schufaIdentification = new SchufaIdentification()
//                        {
//                            personWithoutBirthdate = false,
//                            identityReservationAddress = true,
//                            identityReservationPerson = true,
//                        },
//                        schufaPersonData = new SchufaPersonData()
//                        {
//                            placeOfBirth = "Italy",
//                            title = "NO",
//                            previousAddress = new Location()
//                            {
//                                country = "CH",
//                                city = "Hong Kong",
//                                zip = "045300",
//                                regionCode = "CH",
//                                subRegionCode = "HK",
//                                street = "Zar",
//                                houseNumber = "22",
//                                apartment = "NO",
//                            },
//                        },
//                        schufaFeatures = new SchufaBaseFeature[]
//                        {
//                            new SchufaFeature()
//                            {
//                                type = "FEATURE",
//                                text = "SCHUFA",
//                                ownFeature = NullableBoolean.@true,
//                                ownFeatureSpecified = true,
//                                featureWithoutBirthdate = NullableBoolean.@true,
//                                featureWithoutBirthdateSpecified = true,
//                                accountNumber = "67",
//                                date = "2001-09-09",
//                                numberOfInstallments = "39",
//                                installmentType = "installed",
//                                featureCode = "CODE",
//                                description = "DESCRIPT",
//                                amount = new Amount()
//                                {
//                                    amount = 99,
//                                    currency = "EUR"
//                                },
//                            },
//                            new SchufaTextFeature()
//                            {
//                                type = "TxtFeature",
//                                text = "Schufa",
//                                description = "TEST",
//                                featureWithoutBirthdate = NullableBoolean.@true,
//                                featureWithoutBirthdateSpecified = true,
//                                ownFeature = NullableBoolean.@true,
//                                ownFeatureSpecified = true,
//                                featureCode = "Fcode",
//                            },
//                            new SchufaBaseFeature()
//                            {
//                                type = "Type",
//                                text = "Text",
//                                description = "Desc",
//                                featureWithoutBirthdate = NullableBoolean.@true,
//                                featureWithoutBirthdateSpecified = true,
//                                ownFeature = NullableBoolean.@true,
//                                ownFeatureSpecified = true,
//                                featureCode = "feature",
//                            },
//                        },
//                    },

//                    //##### CFADDRESSHISTORY #######
//                    addressHistory = new AddressWithDeliverability[]
//                    {
//                        new AddressWithDeliverability()
//                        {
//                            addressInputDate = "2000-05-05",
//                            deliverability = Deliverability.MEDIUM,
//                            addressId = new Identifier()
//                            {
//                                identifierText = "TEXT",
//                                identifierType = IdentifierType.AT_DVR_NR,
//                            },
//                            address = new CompanyAddressDescription()
//                            {
//                                coName = "NO",
//                                companyName = "MARS",
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactText = "FAXME",
//                                        contactTypeSpecified = true,
//                                        contactType = ContactType.FAX,
//                                    },
//                                },
//                                location = new Location()
//                                {
//                                    country = "MO",
//                                    regionCode = "ST",
//                                    zip = "10989",
//                                    city = "Ulanbator",
//                                    street = "ulan",
//                                    houseNumber = "99",
//                                    subRegionCode = "GH",
//                                    apartment = "NO"
//                                },
//                            },
//                        },
//                    },

//                    //##### CFOUTGETREPORT #######
//                    paymentDelay = new PaymentDelay()
//                    {
//                        inTimeRatioSpecified = true,
//                        inTimeRatio = 55,
//                        avgDelayShortTerm = "9",
//                        avgDelayLongTerm = "19",
//                        paymentType = "cash",
//                        paymentExpectedTypeSpecified = true,
//                        paymentExpectedType = PaymentExpectedType.DELAYED_MINOR,
//                    },

//                    //###### CFSCORE ##########
//                    scoreAnalysis = new ScoreAnalysis()
//                    {
//                        score = 86,
//                        averageScoreIndustry = "3",
//                        averageScoreAll = "13",
//                        scoreScaleRange = new Range()
//                        {
//                            from = 9,
//                            to = 99,
//                            toSpecified = true,
//                            fromSpecified = true,
//                        },
//                        scoreDecisionRanges = new ScoreDecisionRange[]
//                        {
//                            new ScoreDecisionRange()
//                            {
//                                decision = Decision.YELLOW,
//                                scoreRange = new Range()
//                                {
//                                    from = 2,
//                                    to = 22,
//                                    toSpecified = true,
//                                    fromSpecified = true,
//                                },
//                            },
//                        },
//                    },

//                    // ######## CFWOWRELATION & CFWOWADDRESSl
//                    whoOwnsWhom = new WhoOwnsWhom()
//                    {
//                        wowRelations = new WowRelation[]
//                        {
//                            new WowRelation()
//                            {
//                                owner = 1,
//                                ownedSubject = 1,
//                                voteShare = 5,
//                                voteShareSpecified = true,
//                                capitalShare = 55,
//                                capitalShareSpecified = true,
//                                isMajorityOwner = true,
//                            },
//                        },
//                        wowAddresses = new WowAddress[]
//                        {
//                            new WowAddress()
//                            {
//                                hasDebts = HasDebts.FALSE,
//                                hasDebtsSpecified = true,
//                                id = 12345,
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierType = IdentifierType.CH_LEGACY_ADDRESS_ID,
//                                        identifierText = "LEGACY",
//                                    },
//                                },
//                                address = new CompanyAddressDescription()
//                                {
//                                    coName = "WOWADD",
//                                    companyName = "Toshiba",
//                                    location = new Location()
//                                    {
//                                        country = "JP",
//                                        city = "Tokyo",
//                                        zip = "84372",
//                                        regionCode = "JP",
//                                        subRegionCode = "TK",
//                                        street = "Mushimura",
//                                        houseNumber = "67",
//                                        apartment = "NO",
//                                    },
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "EMAIL",
//                                            contactTypeSpecified = true,
//                                            contactType = ContactType.EMAIL,
//                                        },
//                                    },
//                                },
//                            },
//                        },
//                    },

//                    //####### CFPUBLICATION #########
//                    publicationList = new PublicationList()
//                    {
//                        isTruncated = true,
//                        publications = new Publication[]
//                        {
//                            new Publication()
//                            {
//                                text = "PUBLICATION",
//                                language = "GR",
//                                publicationRegion = "GR",
//                                publicationCategoryOriginal = "NO",
//                                publicationCategory = PublicationCategory.LIQUIDATION,
//                                publicationDate = "2016-02-02",
//                                publicationLabels = new string[]
//                                {
//                                    "pLabel1",
//                                    "pLabel2",
//                                },
//                            },
//                        },
//                    },

//                    // ######## CFORGANIZATIONPOSITION ############
//                    organizationPositionList = new OrganizationPositionList()
//                    {
//                        isTruncated = true,
//                        organizationPositions = new OrganizationPosition[]
//                        {
//                            new OrganizationPosition()
//                            {
//                                firstName = "Devid",
//                                name = "Pavlicek",
//                                nationality = "ME",
//                                homeTown = "Munich",
//                                city = "Munich",
//                                share = 4,
//                                shareSpecified = true,
//                                signatureType = SignatureType.COMPLEX,
//                                signatureTypeSpecified = true,
//                                hasDebts = HasDebts.FALSE,
//                                hasDebtsSpecified = true,
//                                birthDate = "1988-01-01",
//                                period = new Period()
//                                {
//                                    startDate = "2004-04-04",
//                                    endDate = "2022-04-04",
//                                },
//                                nrOfPositions = "2",
//                                nrOfPositionsBankrupt = "0",
//                                signatureOriginal = "NO",
//                                addressId = new Identifier()
//                                {
//                                    identifierText = "UID",
//                                    identifierType = IdentifierType.AT_UID,
//                                },
//                                highestFunction = new OrganizationPositionFunction()
//                                {
//                                    functionType = FunctionType.MANAGEMENT,
//                                    functionPriority = "1",
//                                    functionTypeOriginal = "Management",
//                                },
//                                furtherFunctions = new OrganizationPositionFunction[]
//                                {
//                                    new OrganizationPositionFunction()
//                                    {
//                                        functionPriority = "2",
//                                        functionType = FunctionType.SUPERVISORY_BOARD,
//                                        functionTypeOriginal = "NO",
//                                    },
//                                },

//                                organizationPositions = new OrganizationPosition[]
//                                {
//                                    new OrganizationPosition()
//                                    {
//                                        city = "Maroco",
//                                        nationality = "Mar",
//                                        period = new Period()
//                                        {
//                                            startDate = "1010-01-01",
//                                            endDate = "2000-01-01",
//                                        },
//                                        birthDate = "1999-01-01",
//                                        firstName = "Mango",
//                                        name = "Diao",
//                                        share = 9,
//                                        shareSpecified = true,
//                                        signatureType = SignatureType.ALONE,
//                                        signatureTypeSpecified = true,
//                                        nrOfPositions = "19",
//                                        nrOfPositionsBankrupt = "5",
//                                        homeTown = "London",
//                                        hasDebts = HasDebts.TRUE,
//                                        hasDebtsSpecified = true,
//                                        signatureOriginal = "SIGN",
//                                        addressId = new Identifier()
//                                        {
//                                            identifierText = "TEXT",
//                                            identifierType = IdentifierType.AT_UID,
//                                        },
//                                        //organizationPositions =new OrganizationPosition[]
//                                        //{
//                                        //    new OrganizationPosition()
//                                        //    {
//                                        //        city = "Lilingwe",
//                                        //        period = new Period()
//                                        //        {
//                                        //            startDate = "2000-01-01",
//                                        //            endDate = "2001-01-01",
//                                        //        },
//                                        //        organizationPositions = new OrganizationPosition[]
//                                        //        {
//                                        //            new OrganizationPosition()
//                                        //            {
                                                        
//                                        //            }, 
//                                        //        }
//                                        //    }, 
//                                        //}

//                                    },
//                                },
//                            },
//                        },
//                    },

//                    // ######### CFBRANCHOFFICE ##########
//                    branchOfficeList = new BranchOfficeList()
//                    {
//                        isTruncated = true,
//                        branchOffices = new BranchOfficeListItem[]
//                        {
//                            new BranchOfficeListItem()
//                            {
//                                description = "BRANCH",
//                                registered = NullableBoolean.@true,
//                                registeredSpecified = true,
//                                period = new Period()
//                                {
//                                    startDate = "1989-02-02",
//                                    endDate = "2010-01-01",
//                                },
//                                address = new CompanyAddressDescription()
//                                {
//                                    coName = "NO",
//                                    companyName = "Microsoft",
//                                    location = new Location()
//                                    {
//                                        city = "Munich",
//                                        country = "DE",
//                                        regionCode = "BYC",
//                                        subRegionCode = "MUC",
//                                        zip = "83762",
//                                        street = "Ludwigstr.",
//                                        houseNumber = "18",
//                                        apartment = "NO",
//                                    },
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "Post",
//                                            contactType = ContactType.OTHER,
//                                            contactTypeSpecified = true,
//                                        },
//                                    },
//                                },
//                            },
//                        },
//                    },

//                    //######### CFVERIFICATION ########
//                    idVerificationResponseData = new IdVerificationResponseData()
//                    {
//                        processingResult = IdVerificationProcessingResult.UNDETETERMINED,
//                        rejectionReasons = new string[]
//                        {
//                            "ABC",
//                            "DEF",
//                            "GHI",
//                        },
//                        warnings = new string[]
//                        {
//                            "_1",
//                            "_2",
//                            "_3",
//                        },
//                        content = new IdVerificationContent()
//                        {
//                            checks = new IdVerificationChecks()
//                            {
//                                isCompositeCheckDigitVerified = NullableBoolean.@true,
//                                isCompositeCheckDigitVerifiedSpecified = true,
//                                isBirthDateVerified = NullableBoolean.@true,
//                                isBirthDateVerifiedSpecified = true,
//                                isDocumentNumberVerified = NullableBoolean.@true,
//                                isDocumentNumberVerifiedSpecified = true,
//                                isExpirationDateVerified = NullableBoolean.@true,
//                                isExpirationDateVerifiedSpecified = true,
//                                isValid = NullableBoolean.@true,
//                                isValidSpecified = true,
//                                isComplete = NullableBoolean.@true,
//                                isCompleteSpecified = true,
//                                isIssuingStateOrOrganizationVerified = NullableBoolean.@true,
//                                isIssuingStateOrOrganizationVerifiedSpecified = true,
//                                isNationalIdentificationNumberVerified = NullableBoolean.@true,
//                                isNationalityVerifiedSpecified = true,
//                                isNationalityVerified = NullableBoolean.@true,
//                                isNationalIdentificationNumberVerifiedSpecified = true,
//                            },
//                            document = new IdVerificationDocument()
//                            {
//                                documentNumber = "12",
//                                documentType = IdVerificationDocumentType.PASSPORT,
//                                documentTypeSpecified = true,
//                                issuingDate = "1999-01-01",
//                                expirationDate = "2029-09-09",
//                                issuingStateOrOrganization = "DE",
//                                validityFromDate = "2001-01-01",
//                                documentDescription = "DOCS",
//                                signDate = "1999-01-01",
//                                mrz2 = "NO2",
//                                mrz3 = "NO3",
//                                mrz1 = "NO1",
//                            },
//                            documentImages = new BinaryData[]
//                            {
//                                new BinaryData()
//                                {
//                                    data = "IMG",
//                                    mimeType = "jpg",
//                                    dataClassification = "MEDIA",
//                                },
//                            },
//                            person = new IdVerificationPerson()
//                            {
//                                nationality = "BR",
//                                placeOfBirth = "Britain",
//                                nationalIdentificationNumber = "23476238746",
//                                address1 = "IBIZA",
//                                address2 = "Gobi",
//                                address = new PersonAddressDescription()
//                                {
//                                    firstName = "Marco",
//                                    lastName = "Sigismund",
//                                    birthDate = "1999-02-02",
//                                    coName = "NO",
//                                    maidenName = "NO",
//                                    middleName = "NO",
//                                    sex = Sex.MALE,
//                                    sexSpecified = true,
//                                    location = new Location()
//                                    {
//                                        city = "DOnotKNOW",
//                                        country = "IT",
//                                        regionCode = "LK",
//                                        subRegionCode = "JI",
//                                        zip = "823746",
//                                        street = "Gaboristr.",
//                                        houseNumber = "44",
//                                        apartment = "NO",
//                                    },
//                                    contactItems = new ContactItem[]
//                                    {
//                                        new ContactItem()
//                                        {
//                                            contactText = "Signal",
//                                            contactType = ContactType.OTHER,
//                                            contactTypeSpecified = true,
//                                        },
//                                    },
//                                },
//                            },
//                        },
//                        comparisonElements = new IdVerificationComparisonElement[]
//                        {
//                            new IdVerificationComparisonElement()
//                            {
//                                documentVsMrzSimilarity = "22",
//                                providedVsMrzSimilarity = "12",
//                                providedVsDocumentSimilarity = "44",
//                                fieldName = "Comparison",
//                                mrzValue = "98",
//                                percentage = "7",
//                                documentValue = "150",
//                                providedValue = "350",
//                            },
//                        },
//                    },

//                    // ############## CFFINSTATEMENT ##################
//                    financialStatements = new FinancialStatement[]
//                    {
//                        new FinancialStatement()
//                        {
//                            companyName = "Sony",
//                            currencyCode = "EUR",
//                            financialReportingStandard = "FINANC",
//                            period = new Period()
//                            {
//                                startDate = "2011-01-01",
//                                endDate = "2010-02-02",
//                            },
//                            balanceSheet = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 10343,
//                                    identifierText = "DONT",
//                                    id = 32,
//                                    position = 3,
//                                    valueSpecified = true,
//                                    parentId = "1",
//                                    identifier = "IDENT",
//                                },
//                            },
//                            cashFlow = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 50,
//                                    identifierText = "GJKSD",
//                                    position = 9,
//                                    valueSpecified = true,
//                                    id = 424353,
//                                    identifier = "11111",
//                                    parentId = "4",
//                                },
//                            },
//                            creditRatios = new CreditRatio[]
//                            {
//                                new CreditRatio()
//                                {
//                                    value = 70,
//                                    creditRatioIdentifier = "CREDIT",
//                                },
//                            },
//                            profitAndLoss = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 100,
//                                    id = 2435,
//                                    identifierText = "TEXT",
//                                    position = 8,
//                                    parentId = "3",
//                                    valueSpecified = true,
//                                    identifier = "85437840",
//                                },
//                            },
//                            furtherFigures = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 400,
//                                    identifierText = "Ident",
//                                    id = 345,
//                                    position = 3,
//                                    parentId = "5",
//                                    identifier = "325",
//                                    valueSpecified = true,
//                                }, 
//                            },
//                        },
//                    },
//                    financialStatementsGroup = new FinancialStatement[]
//                    {
//                        new FinancialStatement()
//                        {
//                            companyName = "Toyota",
//                            currencyCode = "EUR",
//                            financialReportingStandard = "WHAT",
//                            period = new Period()
//                            {
//                                startDate = "2006-07-07",
//                                endDate = "2018-06-06",
//                            },
//                            profitAndLoss = new FinancialStatementElement[]
//                            {
//                                new FinancialStatementElement()
//                                {
//                                    value = 450,
//                                    id = 4357,
//                                    identifier = "TOyot",
//                                    identifierText = "Toyot",
//                                    parentId = "3",
//                                    position = 2,
//                                    valueSpecified = true,
//                                },
//                            },
//                            creditRatios = new CreditRatio[]
//                            {
//                                new CreditRatio()
//                                {
//                                    value = 99,
//                                    creditRatioIdentifier = "CREDIT2"
//                                },
//                            },
//                        },
//                    },

//                    // ######## CFCOMPANY #############
//                    companyBaseData = new CompanyBaseData()
//                    {
//                        registryType = RegistryType.REGISTER_OF_ASSOCIATIONS,
//                        legalFormType = LegalFormType.PUBLIC,
//                        legalFormText = "TEST",
//                        legalFormTypeOriginal = "TEST",
//                        activityStatusOriginal = "TEST",
//                        activityStatus = ActivityStatus.INSOLVENT,
//                        companyRegistrationData = new CompanyRegistrationData()
//                        {
//                            auditingCompany = new CompanyBaseData()
//                            {
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierText = "company",
//                                        identifierType = IdentifierType.CH_LEGACY_ADDRESS_ID,
//                                    },
//                                },
//                                activityStatus = ActivityStatus.INACTIVE,
//                                activityStatusOriginal = "AUDIT",
//                                legalFormText = "AUDIT",
//                                legalFormType = LegalFormType.PUBLIC,
//                                legalFormTypeOriginal = "AUDIT",
//                                registryType = RegistryType.OTHER,
//                                mainAddress = new CompanyAddressDescription()
//                                {
//                                    location = new Location()
//                                    {
//                                        city = "Paris",
//                                        country = "FR",
//                                        houseNumber = "22",
//                                        zip = "83247",
//                                        street = "Burbur",
//                                    },
//                                    companyName = "AUDIT"
//                                },
//                            },
//                            purpose = "TEST",
//                            hasAuditingCompanySpecified = true,
//                            hasAuditingCompany = AuditingCompanyStatus.NO_AUDITOR_NOT_MANDATORY,
//                            capitalInKind = NullableBoolean.@true,
//                            capitalInKindSpecified = true,
//                            foundingDate = "1990-04-04",
//                            registeredOfficeCity = "000000000",
//                            capital = new Amount()
//                            {
//                                amount = 400,
//                                currency = "EUR"
//                            },
//                            capitalPayed = new Amount()
//                            {
//                                amount = 300,
//                                currency = "EUR",
//                            },
//                        },
//                        identifiers = new Identifier[]
//                        {
//                            new Identifier()
//                            {
//                                identifierText = "VAT",
//                                identifierType = IdentifierType.CH_UID_VAT,
//                            },
//                        },
//                        companyDetailData = new CompanyDetailData()
//                        {
//                            ultimateMotherCompany = new CompanyBaseData()
//                            {
//                                identifiers = new Identifier[]
//                                {
//                                    new Identifier()
//                                    {
//                                        identifierText = "MOTHER",
//                                        identifierType = IdentifierType.SCHUFA_ID,
//                                    },
//                                },
//                                activityStatus = ActivityStatus.COMPOSITION_AGREEMENT,
//                                activityStatusOriginal = "MOTHER",
//                                legalFormType = LegalFormType.LIMITED_COMPANY,
//                                legalFormTypeOriginal = "MOTHER",
//                                legalFormText = "MOTHER",
//                                registryType = RegistryType.UNKNOWN,
//                            },
//                            knownSince = "1800-04-04",
//                            nrOfEmployees = new Range()
//                            {
//                                from = 12,
//                                to = 34,
//                                toSpecified = true,
//                                fromSpecified = true,
//                            },
//                            sizeClass = CompanySizeClass.M,
//                            sizeClassSpecified = true,
//                            activityIndex = ActivityIndex.MEDIUM,
//                            activityIndexSpecified = true,
//                            turnoverInExport = NullableBoolean.@true,
//                            turnoverInExportSpecified = true,
//                            turnoverCurrency = "EUR",
//                            dateFinancialStatement = "2009-05-05",
//                            dateFinancialStatementHandedIn = "2011-03-03",
//                            turnoverRange = new Range()
//                            {
//                                from = 20,
//                                to = 40,
//                                toSpecified = true,
//                                fromSpecified = true,
//                            },
//                            industryCodes = new IndustryCode[]
//                            {
//                                new IndustryCode()
//                                {
//                                    type = "SPECIAL",
//                                    code = "CODE",
//                                    codeDescription = "DESCRIPT",
//                                    isMainIndustryCode = NullableBoolean.@true,
//                                    isMainIndustryCodeSpecified = true,
//                                    period = new Period()
//                                    {
//                                        startDate = "2000-01-01",
//                                        endDate = "1990-01-01",
//                                    },
//                                },
//                            },
//                            bankAccounts = new BankAccount[]
//                            {
//                                new BankAccount()
//                                {
//                                    currency = "EUR",
//                                    iban = "823947234",
//                                    swiftCode = "98237498",
//                                    localAccountNr = "097832",
//                                    bankDescription = "BANK",
//                                    bank = new CompanyAddressDescription()
//                                    {
//                                        coName = "NO",
//                                        companyName = "TWINKY",
//                                        location = new Location()
//                                        {
//                                            country = "MR",
//                                            city = "TK",
//                                            zip = "48327",
//                                            regionCode = "BB",
//                                            subRegionCode = "DD",
//                                            street = "golden.",
//                                            houseNumber = "34",
//                                            apartment = "NO",
//                                        },
//                                        contactItems = new ContactItem[]
//                                        {
//                                            new ContactItem()
//                                            {
//                                                contactText = "WEB",
//                                                contactType = ContactType.WEB,
//                                                contactTypeSpecified = true,
//                                            },
//                                        },
//                                    },
//                                },
//                            },
//                        },
//                        mainAddress = new CompanyAddressDescription()
//                        {
//                            coName = "NO",
//                            companyName = "Postbank",
//                            location = new Location()
//                            {
//                                city = "Geretsried",
//                                country = "DE",
//                                regionCode = "BY",
//                                subRegionCode = "MUC",
//                                street = "einestr.",
//                                houseNumber = "44",
//                                zip = "82538",
//                                apartment = "NO",
//                            },
//                            contactItems = new ContactItem[]
//                            {
//                                new ContactItem()
//                                {
//                                    contactText = "Bird",
//                                    contactType = ContactType.OTHER,
//                                    contactTypeSpecified = true,
//                                },
//                            },
//                        },
//                        companyHistoryItems = new CompanyHistoryItem[]
//                            {
//                                new CompanyHistoryItem()
//                                {
//                                    period = new Period()
//                                    {
//                                        startDate = "2000-01-01",
//                                        endDate = "2001-01-01",
//                                    },
//                                    type = CompanyHistoryItemType.ACTIVITY_STATUS,
//                                },
//                                new CompanyHistoryItemActivityStatus()
//                                {
//                                    activityStatus = ActivityStatus.IN_LIQUIDATION,
//                                    activityStatusOriginal = "TEST",
//                                    type = CompanyHistoryItemType.CAPITAL_PAYED,
//                                    period = new Period()
//                                    {
//                                        startDate = "2002-01-01",
//                                        endDate = "2003-01-01",
//                                    },
//                                },
//                                new CompanyHistoryItemAddress()
//                                {
//                                    type = CompanyHistoryItemType.RESOLUTION,
//                                    period = new Period()
//                                    {
//                                        startDate = "2004-01-01",
//                                        endDate = "2005-01-01",
//                                    },
//                                    address = new PersonAddressDescription()
//                                    {
//                                        sex = Sex.FEMALE,
//                                        sexSpecified = true,
//                                        birthDate = "1990-01-01",
//                                        coName = "CO",
//                                        lastName = "Dito",
//                                        firstName = "Mara",
//                                        maidenName = "NO",
//                                        middleName = "NO",
//                                        contactItems = new ContactItem[]
//                                        {
//                                            new ContactItem()
//                                            {
//                                                contactText = "contact",
//                                                contactTypeSpecified = true,
//                                                contactType = ContactType.PHONE,
//                                            },
//                                        },
//                                        location = new Location()
//                                        {
//                                            city = "Cairo",
//                                            country = "EG",
//                                            zip = "438974",
//                                            street = "cairo",
//                                            houseNumber = "1",
//                                            subRegionCode = "EG",
//                                            apartment = "NO",
//                                            regionCode = "NO"
//                                        },
//                                    },
//                                },
//                                new CompanyHistoryItemAmount()
//                                {
//                                    amount = new Amount()
//                                    {
//                                        amount = 44,
//                                        currency = "EUR",
//                                    },
//                                    type = CompanyHistoryItemType.DOMICIL,
//                                    period = new Period()
//                                    {
//                                        startDate = "2004-01-01",
//                                        endDate = "2005-01-01",
//                                    },
//                                },
//                                new CompanyHistoryItemLegalForm()
//                                {
//                                    type = CompanyHistoryItemType.PURPOSE,
//                                    legalFormText = "Legal",
//                                    legalFormType = LegalFormType.SINGLE_PERSON_COMPANY,
//                                    legalFormTypeOriginal = "String",
//                                    period = new Period()
//                                    {
//                                        startDate = "2000-01-01",
//                                        endDate = "2005-01-01",
//                                    }
//                                },
//                                new CompanyHistoryItemLocation()
//                                {
//                                    type = CompanyHistoryItemType.REGISTERED_OFFICE_CITY,
//                                    period = new Period()
//                                    {
//                                        startDate = "2011-01-01",
//                                        endDate = "2014-01-01",
//                                    },
//                                    location = new Location()
//                                    {
//                                        city = "Dakar",
//                                        country = "SE",
//                                        zip = "48739",
//                                        regionCode = "DK",
//                                        subRegionCode = "DK",
//                                        street = "senegal",
//                                        houseNumber = "66",
//                                        apartment = "NO",
//                                    },
//                                },
//                                new CompanyHistoryItemText()
//                                {
//                                    text = "History",
//                                    type = CompanyHistoryItemType.NAME,
//                                    period = new Period()
//                                    {
//                                        startDate = "2019-01-01",
//                                        endDate = "2021-01-01",
//                                    },
//                                },
//                            },
//                    //},
//                    //furtherRelations = new FurtherRelations()
//                    //{
//                    //    obviousRelations = new CompanyBaseData[]
//                    //    {
//                    //new CompanyBaseData()
//                    //{
//                    //    activityStatus = ActivityStatus.IN_LIQUIDATION,
//                    //    activityStatusOriginal = "FURTHER",
//                    //    legalFormTypeOriginal = "FURTHER",
//                    //    legalFormType = LegalFormType.PUBLIC,
//                    //    legalFormText = "FURTH",
//                    //    registryType = RegistryType.TRADE_LICENCE,
//                    //    identifiers = new Identifier[]
//                    //    {
//                    //        new Identifier()
//                    //        {
//                    //            identifierText = "IDENT",
//                    //            identifierType = IdentifierType.UNIT_ID,
//                    //        },
//                    //    },
//                    //    companyRegistrationData = new CompanyRegistrationData()
//                    //    {
//                    //        auditingCompany = null,
//                    //        capitalInKind = NullableBoolean.@true,
//                    //        capitalInKindSpecified = true,
//                    //        hasAuditingCompany = AuditingCompanyStatus.NO_AUDITOR_NOT_MANDATORY,
//                    //        hasAuditingCompanySpecified = false,
//                    //        purpose = "FurtherRelations",
//                    //        foundingDate = "2004-04-04",
//                    //        registeredOfficeCity = "REGISTER",
//                    //        capital = new Amount()
//                    //        {
//                    //            amount = 84,
//                    //            currency = "EUR",
//                    //        },
//                    //        capitalPayed = new Amount()
//                    //        {
//                    //            amount = 13,
//                    //            currency = "EUR",
//                    //        },
//                    //    },
//                    //    companyDetailData = new CompanyDetailData()
//                    //    {
//                    //        ultimateMotherCompany = null,
//                    //        turnoverCurrency = "EUR",
//                    //        sizeClass = CompanySizeClass.XL,
//                    //        sizeClassSpecified = true,
//                    //        dateFinancialStatement = "1990-03-03",
//                    //        dateFinancialStatementHandedIn = "1991-02-02",
//                    //        turnoverInExport = NullableBoolean.@true,
//                    //        turnoverInExportSpecified = true,
//                    //        knownSince = "1995-04-04",
//                    //        activityIndex = ActivityIndex.VERY_HIGH,
//                    //        activityIndexSpecified = true,
//                    //        nrOfEmployees = new Range()
//                    //        {
//                    //            from = 1,
//                    //            to = 99,
//                    //            toSpecified = true,
//                    //            fromSpecified = true,
//                    //        },
//                    //        turnoverRange = new Range()
//                    //        {
//                    //            from = 5,
//                    //            to = 55,
//                    //            toSpecified = true,
//                    //            fromSpecified = true,
//                    //        },
//                    //        industryCodes = new IndustryCode[]
//                    //        {
//                    //            new IndustryCode()
//                    //            {
//                    //                type = "Further",
//                    //                codeDescription = "Further",
//                    //                code = "FURTHE",
//                    //                isMainIndustryCode = NullableBoolean.@true,
//                    //                isMainIndustryCodeSpecified = true,
//                    //                period = new Period()
//                    //                {
//                    //                    startDate = "2000-01-01",
//                    //                    endDate = "2002-01-01",
//                    //                },
//                    //            },
//                    //        },
//                    //        bankAccounts = new BankAccount[]
//                    //        {
//                    //            new BankAccount()
//                    //            {
//                    //                currency = "EUR",
//                    //                iban = "4837429",
//                    //                swiftCode = "43879",
//                    //                localAccountNr = "9342875",
//                    //                bankDescription = "FRTHER",
//                    //                bank = new CompanyAddressDescription()
//                    //                {
//                    //                    coName = "NO",
//                    //                    companyName = "Philipps",
//                    //                    location = new Location()
//                    //                    {
//                    //                        city = "Some",
//                    //                        country = "NE",
//                    //                        zip = "84322",
//                    //                        regionCode = "DF",
//                    //                        subRegionCode = "RE",
//                    //                        street = "grasf",
//                    //                        houseNumber = "9",
//                    //                        apartment = "NO",
//                    //                    },
//                    //                    contactItems = new ContactItem[]
//                    //                    {
//                    //                        new ContactItem()
//                    //                        {
//                    //                            contactText = "PHONE",
//                    //                            contactType = ContactType.PHONE,
//                    //                            contactTypeSpecified = true,
//                    //                        },
//                    //                    },
//                    //                },
//                    //            },
//                    //        },
//                    //    },
//                    //},
//                    //    },
//                },

//                    //####### CFCOMPLIANCEDESCRIPTION #############
//                    complianceCheckResult = new ComplianceCheckResult()
//                    {
//                        checkedEntity = new ComplianceCheckedEntity()
//                        {
//                            relationType = "Good",
//                            checkedAddress = new CompanyAddressDescription()
//                            {
//                                coName = "YES",
//                                companyName = "DOLCE",
//                                location = null,
//                                contactItems = new ContactItem[]
//                                {
//                                    new ContactItem()
//                                    {
//                                        contactText = "PHONE",
//                                        contactType = ContactType.PHONE,
//                                        contactTypeSpecified = true,
//                                    },
//                                },
//                            },
//                            foundEntities = new ComplianceFoundEntity[]
//                            {
//                                new ComplianceFoundEntity()
//                                {
//                                    primaryName = "COLA",
//                                    country = "DE",
//                                    age = "22",
//                                    crifRefId = 4,
//                                    matchInformation = new ComplianceMatchInformation()
//                                    {
//                                        matchedName = "DEVID",
//                                        confidenceBirthdate = 3,
//                                        matchedBirthdate = "1989-03-03",
//                                        confidenceName = 4,
//                                    },
//                                    listDescription = new ComplianceListDescription()
//                                    {
//                                        name = "PEPSI",
//                                        publishingOrganization = "COLA",
//                                        dateLastUpdated = "2016-02-02",
//                                        category = "Drink",
//                                    },
//                                    additionalInformations = new string[]
//                                    {
//                                        "addI1",
//                                        "addI2",
//                                    },
//                                    furtherCountries = new string[]
//                                    {
//                                        "furt1",
//                                        "furth2",
//                                    },
//                                    birthdates = new string[]
//                                    {
//                                        "1987-01-01",
//                                        "1989-01-01",
//                                    },
//                                    titles = new string[]
//                                    {
//                                        "Prof.",
//                                        "Dr."
//                                    },
//                                    furtherNames = new string[]
//                                    {
//                                        "furthN1",
//                                        "furthN2",
//                                    },
//                                    knownAddresses = new string[]
//                                    {
//                                        "address1",
//                                        "address2",
//                                    },
//                                    passportsOrIds = new string[]
//                                    {
//                                        "pass1",
//                                        "pass2",
//                                    },
//                                    birthplaces = new string[]
//                                    {
//                                        "ger",
//                                        "it",
//                                        "fr"
//                                    },
//                                    keywords = new string[]
//                                    {
//                                        "key1",
//                                        "key2",
//                                    },
//                                },
//                            },
//                            //checkedEntities = new ComplianceCheckedEntity[]
//                            //{
//                            //    new ComplianceCheckedEntity()
//                            //    {
//                            //        relationType = "CheckdEn",
//                            //        checkedEntities = new ComplianceCheckedEntity[]
//                            //        {
//                            //            new ComplianceCheckedEntity()
//                            //            {
                                            
//                            //            }, 
//                            //        }
//                            //    },
//                            //}
//                        },
//                        listDescriptions = new ComplianceListDescription[]
//                        {
//                            new ComplianceListDescription()
//                            {
//                                name = "LIST",
//                                publishingOrganization = "ORGANIZ",
//                                dateLastUpdated = "2000-01-01",
//                                category = "CATE",
//                            },
//                        },
//                    },

//                    //######## CFBUSINESSLIC ###########
//                    businessIndustryLicenses = new BusinessIndustryLicense[]
//                    {
//                        new BusinessIndustryLicense()
//                        {
//                            period = new Period()
//                            {
//                                startDate = "2000-01-01",
//                                endDate = "2001-01-01",
//                            },
//                            industryName = "NAME",
//                            issuedTo = "JKJJ",
//                            status = BusinessIndustryLicenseStatus.ACTIVE,
//                            industryCode = new IndustryCode()
//                            {
//                                type = "Industry",
//                                isMainIndustryCode = NullableBoolean.@true,
//                                isMainIndustryCodeSpecified = true,
//                                code = "CFKEY",
//                                codeDescription = "CODEDESC",
//                                period = new Period()
//                                {
//                                    startDate = "2005-05-05",
//                                    endDate = "2006-06-06",
//                                },
//                            },
//                        },
//                    },
//                },
//            };

//            wsDao.SetReturnValue("GetReport", element);

//            var bo = new CrifGetReportBo((ICrifWSDao) wsDao.MockInstance, new CrifDBDao(), new AuskunftDao());
//            var sysAuskunft = 208721;

//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            //Act
//            bo.doAuskunft(sysAuskunft);

//            //Assert
//            using (var context = contextFactory.Create<CRIFContext>())
//            {
//                var outElement = context.CFOUTGETREPORT
//                                        .Where(cfoutgetreport => cfoutgetreport.SYSAUSKUNFT.Value == sysAuskunft)
//                                        .OrderByDescending(cfoutgetreport => cfoutgetreport.SYSCFOUTGETREPORT)
//                                        .FirstOrDefault();

//                Assert.NotNull(outElement);
//            }
//        }

//        [Test]
//        public void AutoMapperIncludeTest()
//        {
//            Mapper.Initialize(cfg =>
//            {
//                cfg.AddProfile<BankNowModelProfileServices>();
//                cfg.AddProfile<AuskunftModelCrifProfile>();
//            });

//            CompanyAddressDescription ca = new CompanyAddressDescription()
//            {
//                companyName = "Test",
//                location = new Location() {apartment = "apartment"}
//            };

//            var cfAddress = Mapper.Map(ca, new CFADDRESS());
//            var cfAddress2 = Mapper.Map((AddressDescription) ca, new CFADDRESS());
//            var cfAddress3 = Mapper.DynamicMap<CFADDRESS>(ca);
//            var cfAddress4 = Mapper.DynamicMap<CFADDRESS>((AddressDescription) ca);

//            Assert.AreEqual(cfAddress2.APARTMENT, cfAddress.APARTMENT);
//            Assert.AreEqual(cfAddress3.APARTMENT, cfAddress.APARTMENT);
//            Assert.AreEqual(cfAddress3.APARTMENT, cfAddress4.APARTMENT);
//        }
//    }
//}