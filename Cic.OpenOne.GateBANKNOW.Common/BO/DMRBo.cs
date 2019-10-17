namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CIC.Database.OL.EF6.Model;
    using DAO;

    using Devart.Data.Oracle;

    using DTO;

    using OpenOne.Common.Model.DdOl;
    using OpenOne.Common.Model.DdOw;
    using OpenOne.Common.Util;
    using OpenOne.Common.Util.Config;
    

    public class DMRBo : IDMRBo
    {
        private const string QUERYDMRMETADATAANGEBOT = @"select 
ang.angebot ContractID,
ang.syskd Contact1Person,
ang.sysit Contact1It,
ang.sysid,
ang.sysvm,
ang.erfassungsclient,
angobContact2.sysperson Contact2Person,
angobContact2.sysit Contact2It
from 
angebot ang,
(select sysangebot, sysperson, sysit from angobsich, sichtyp
where angobsich.syssichtyp = sichtyp.syssichtyp
and sichtyp.rang in (120, 130, 800)) angobContact2
where
 ang.sysid = angobContact2.sysangebot (+) and
 ang.sysid = :sysAngebot";

        private const string QUERYDMRMETADATAANTRAG = @"select 
ant.antrag ContractID,
ant.syskd Contact1Person,
ant.sysit Contact1It,
ant.sysid, 
ant.sysvm,
ant.erfassungsclient,
antobContact2.sysperson Contact2Person,
antobContact2.sysit Contact2It
from 
antrag ant,
(select sysantrag, sysperson, sysit from antobsich, sichtyp
where antobsich.syssichtyp = sichtyp.syssichtyp
and sichtyp.rang in (120, 130, 800)) antobContact2
where
 ant.sysid = antobContact2.sysantrag (+) and
 ant.sysid = :sysAntrag";

        private readonly IDMRDao dmrDao;

        private DMRConfig config;

        public DMRBo(IDMRDao dmrDao)
        {
            this.dmrDao = dmrDao;
            this.config = new DMRConfig()
            {
                Url = AppConfig.getValueFromDb("SETUP", "DMR", "INTERFACEURL", "http://www.test.de"),
                ClientName = AppConfig.getValueFromDb("SETUP", "DMR", "CLIENTNAME", "BANK-now"),
                ProcessName = AppConfig.getValueFromDb("SETUP", "DMR", "PROCESSNAME", "DMR"),
                ProcessStepName = AppConfig.getValueFromDb("SETUP", "DMR", "PROCESSSTEPNAME", "OL_INPUT"),
                DocumentType = AppConfig.getValueFromDb("SETUP", "DMR", "DOCUMENTTYPE", "BATCH"),

                ChannelInfoB2B = AppConfig.getValueFromDb("SETUP", "DMR", "CHANNELINFOB2B", "01_Platin"),
                ChannelInfoB2C = AppConfig.getValueFromDb("SETUP", "DMR", "CHANNELINFOB2C", "01_Platin"),
                ChannelInfoB2P = AppConfig.getValueFromDb("SETUP", "DMR", "CHANNELINFOB2P", "01_Platin")
            };
        }

        public void createOrUpdateDMR(long sysDmsAkte)
        {
            using (var ctx = new DdOwExtended())
            using (var ddOlCtx = new DdOlExtended())
            {
                var dmsakte = ctx.DMSAKTE.Single(a => a.SYSDMSAKTE == sysDmsAkte);
                var dmsdoc = ctx.DMSDOC.Single(a => a.SYSDMSDOC == dmsakte.SYSID.Value);

                var parametersAngebot = new List<OracleParameter>
                {
                    new OracleParameter
                    {
                        ParameterName = "sysAngebot",
                        Value = dmsakte.SYSANGEBOT
                    },
                };

                var parametersAntrag = new List<OracleParameter>
                {
                    new OracleParameter
                    {
                        ParameterName = "sysAntrag",
                        Value = dmsakte.SYSANTRAG
                    },
                };

                DMRMetadataDto metadataAngebot = ddOlCtx.ExecuteStoreQuery<DMRMetadataDto>(QUERYDMRMETADATAANGEBOT, parametersAngebot.ToArray()).FirstOrDefault();
                DMRMetadataDto metadataAntrag = ddOlCtx.ExecuteStoreQuery<DMRMetadataDto>(QUERYDMRMETADATAANTRAG, parametersAntrag.ToArray()).FirstOrDefault();

                if (metadataAngebot == null && metadataAntrag == null)
                {
                    throw new ArgumentException("Could not load metadata for SysAngebot: '" + dmsakte.SYSANGEBOT + "' SysAntrag: '" + dmsakte.SYSANTRAG + "'");
                }

                var metadataCombined = metadataAntrag != null ? metadataAntrag.Apply(metadataAngebot) : metadataAngebot;

                PERSON antragstellerPerson = null;
                IT antragstellerIt = null;

                PERSON mitantragstellerPerson = null;
                IT mitAntragstellerIt = null;

                if (metadataCombined.Contact1Person.HasValue && metadataCombined.Contact1Person.Value > 0)
                {
                    antragstellerPerson = ddOlCtx.PERSON.FirstOrDefault(a => a.SYSPERSON == metadataCombined.Contact1Person.Value);
                }

                if (antragstellerPerson == null && metadataCombined.Contact1It.HasValue && metadataCombined.Contact1It.Value > 0)
                {
                    antragstellerIt = ddOlCtx.IT.FirstOrDefault(a => a.SYSIT == metadataCombined.Contact1It.Value);
                }

                if (metadataCombined.Contact2Person.HasValue && metadataCombined.Contact2Person.Value > 0)
                {
                    mitantragstellerPerson = ddOlCtx.PERSON.FirstOrDefault(a => a.SYSPERSON == metadataCombined.Contact2Person.Value);
                }

                if (mitantragstellerPerson == null && metadataCombined.Contact2It.HasValue && metadataCombined.Contact2It.Value > 0)
                {
                    mitAntragstellerIt = ddOlCtx.IT.FirstOrDefault(a => a.SYSIT == metadataCombined.Contact2It.Value);
                }

                var inputDto = new DMRInputDto()
                {
                    ClientName = config.ClientName,
                    ProcessName = config.ProcessName,
                    ProcessStepName = config.ProcessStepName,
                    DocumentType = config.DocumentType,
                };

                inputDto.AddToField("ContractID", metadataCombined.ContractID);

                if (antragstellerPerson != null || antragstellerIt != null)
                {
                    AddPersonData(inputDto, "", metadataCombined.Contact1It, antragstellerPerson, antragstellerIt);
                }
                else
                {
                    throw new ArgumentException("Could not load metadata for antragsteller (SysPerson: " + metadataCombined.Contact1Person + ", SysIt: " + metadataCombined.Contact1It + ")");
                }

                if (mitantragstellerPerson != null || mitAntragstellerIt != null)
                {
                    AddPersonData(inputDto, "2", metadataCombined.Contact2It, mitantragstellerPerson, mitAntragstellerIt);
                }

                var erfassungsclient = metadataCombined.Erfassungsclient;

                // B2B
                if (erfassungsclient == 10)
                {
                    string segName = "";

                    long? sysVm = metadataCombined.SysVm;
                    if (sysVm.HasValue)
                    {
                        segName = ddOlCtx.PERSON.Include("SEG")
                            .Where(a => a.SYSPERSON == sysVm)
                            .Select(vermittler => vermittler.SEG.NAME)
                            .FirstOrDefault();
                    }

                    inputDto.AddToField("ChannelType", "B2B");
                    inputDto.AddToField("ChannelInfo", !string.IsNullOrEmpty(segName) ? segName : this.config.ChannelInfoB2B);
                }
                // B2C
                else if (erfassungsclient == 30)
                {
                    inputDto.AddToField("ChannelType", "B2C");
                    inputDto.AddToField("ChannelInfo", this.config.ChannelInfoB2C);
                }

                inputDto.AddToField("ScanDate", string.Format("{0:dd.MM.yyyy hh:mm:ss}", DateTimeHelper.CreateDate(dmsdoc.GEDRUCKTAM, dmsdoc.GEDRUCKTUM)));

                inputDto.AddToMedia(dmsdoc.DATEINAME, MimeUtil.GetMIMEType(dmsdoc.DATEINAME), dmsdoc.INHALT);

                var result = dmrDao.sendToDmr(config, inputDto);

                if (result.StatusCode != 201)
                {
                    var errorMessage = "";
                    switch (result.StatusCode)
                    {
                        case 400:
                            errorMessage = "Bad Request / Request not valid. Error during execution of the web-service. (" + result.StatusCode + ")";
                            break;
                        case 401:
                            errorMessage = "Unauthorized / User could not be authenticated. (" + result.StatusCode + ")";
                            break;
                        case 403:
                            errorMessage = "Forbidden / User does not have the permission to execute the web-service. (" + result.StatusCode + ")";
                            break;
                        case 404:
                            errorMessage = "Not Found / Resource could not be found. (" + result.StatusCode + ")";
                            break;
                        default:
                            errorMessage = "Unknown Error: " + result.ErrorMessage + " (" + result.StatusCode + ")";
                            break;
                    }
                    dmsakte.EXTID = null;
                    dmsakte.ERRMESSAGE = errorMessage;
                    dmsakte.ERRCODE = result.StatusCode.ToString();
                }
                else
                {
                    dmsakte.EXTID = result.UploadID;
                    dmsakte.ERRMESSAGE = null;
                    dmsakte.ERRCODE = null;
                }

                dmsakte.SOURCETYPE = 2;
                dmsakte.CHGDATE = DateTime.Now;
                dmsakte.CHGTIME = DateTimeHelper.DateTimeToClarionTime(dmsakte.CHGDATE);
                ctx.SaveChanges();
            }
        }

        private void AddPersonData(DMRInputDto inputDto, string addition, long? sysit, PERSON person, IT it)
        {
            if (sysit.HasValue)
            {
                inputDto.AddToField("Contact" + addition + "ItID", sysit.ToString());
            }

            if (person != null)
            {
                inputDto.AddToField("Contact" + addition + "ID", person.SYSPERSON.ToString());

                inputDto.AddToField("Contact" + addition + "LastName", person.NAME);
                inputDto.AddToField("Contact" + addition + "FirstName", person.VORNAME);
                inputDto.AddToField("Contact" + addition + "Street", person.STRASSE);
                inputDto.AddToField("Contact" + addition + "StreetNumber", person.HSNR);
                inputDto.AddToField("Contact" + addition + "PLZ", person.PLZ);
                inputDto.AddToField("Contact" + addition + "City", person.ORT);
                inputDto.AddToField("Contact" + addition + "Birthdate", string.Format("{0:dd.MM.yyyy}", person.GEBDATUM));
            }
            else
            {
                inputDto.AddToField("Contact" + addition + "LastName", it.NAME);
                inputDto.AddToField("Contact" + addition + "FirstName", it.VORNAME);
                inputDto.AddToField("Contact" + addition + "Street", it.STRASSE);
                inputDto.AddToField("Contact" + addition + "StreetNumber", it.HSNR);
                inputDto.AddToField("Contact" + addition + "PLZ", it.PLZ);
                inputDto.AddToField("Contact" + addition + "City", it.ORT);
                inputDto.AddToField("Contact" + addition + "Birthdate", string.Format("{0:dd.MM.yyyy}", it.GEBDATUM));
            }
        }
    }
}