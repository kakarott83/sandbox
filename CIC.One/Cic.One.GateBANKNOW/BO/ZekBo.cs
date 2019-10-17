using Cic.One.DTO;
using Cic.One.Util.IO;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.One.GateBANKNOW.BO
{
    public class ZekBo
    {
        /// <summary>
        /// error logging
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// logged in workflow user
        /// </summary>
        public readonly long SysWfUser = 0;
        /// <summary>
        /// Interface handling the real zek requst
        /// </summary>
        IZekBo ZekAnfrage { get; set; }
        /// <summary>
        /// data for zek request
        /// </summary>
        public Cic.One.DTO.ZekDto ZekData
        {
            get
            {
                return zekData;
            }
            set
            {
                if (value == null)
                    zekData = new Cic.One.DTO.ZekDto();
                else
                    zekData = value;

                InitZek();
            }
        }
        /// <summary>
        /// answer from zek
        /// </summary>
        public List<Cic.One.DTO.ZekDto> ZekResponse { get; set; }

        /// <summary>
        /// data for zek request
        /// </summary>
        private Cic.One.DTO.ZekDto zekData { get; set; }

        /// <summary>
        /// create the data structure managing both request and response data of a zek information request
        /// </summary>
        public ZekBo(long sysWfUser, Cic.One.DTO.ZekDto data = null)
        {
            SysWfUser = sysWfUser;
            ZekData = data;

            ZekAnfrage = AuskunftBoFactory.CreateDefaultZekBo();
            ZekResponse = new List<Cic.One.DTO.ZekDto>();
        }

        #region mapping
        private void InitZek()
        {
            if (zekData.person == null)
                zekData.person = new PersonDto();
            if (zekData.contracts == null)
                zekData.contracts = new List<ZekContractDto>();
            if (zekData.pleas == null)
                zekData.pleas = new List<ZekPleaDto>();
            if (zekData.kartenmeldungen == null)
                zekData.kartenmeldungen = new List<ZekKartenmeldungDto>();
            if (zekData.ecodes == null)
                zekData.ecodes = new List<ZekECodeDto>();
            if (zekData.abfragedaten == null)
                zekData.abfragedaten = new AbfragedatenDto();

            zekData.abfragedaten.sysid = DAOFactoryFactory.getInstance().getEntityDao().getSysidFromNumber(zekData.abfragedaten.area, zekData.nummer);

            bool countrynameMissing = zekData.person.sysland != 0; // we need ISO3, so even if there is a landBezeichnung, we need to make sure it's the right one.
            bool nationalitynameMissing = zekData.person.syslandnat != 0; //see country name
            if (countrynameMissing || nationalitynameMissing)
            {
                IDictionaryListsDao dld = CommonDaoFactory.getInstance().getDictionaryListsDao();
                DropListDto[] laender = dld.deliverISO3LAND();

                if (countrynameMissing)
                    zekData.person.landBezeichnung = laender.Where(a => a.sysID == zekData.person.sysland).FirstOrDefault().beschreibung;
                if (nationalitynameMissing)
                    zekData.person.landNatBezeichnung = laender.Where(a => a.sysID == zekData.person.syslandnat).FirstOrDefault().beschreibung;
            }
        }

        /// <summary>
        /// zek returned information we need
        /// </summary>
        /// <param name="response">zek information</param>
        private void MapResponse(AuskunftOLDto response)
        {
            ZekResponse.Clear();
            Cic.One.DTO.ZekDto previousZekData = ObjectCloner.Clone<Cic.One.DTO.ZekDto>(ZekData);


            //no result means no data for the list.
            if (response == null || response.ZekOLOutDto == null)
                return;

            if (response.ZekOLOutDto.message != null)
                _log.Debug("ZEK Informativabfrage Nachricht: " + response.ZekOLOutDto.message.code);

            if (response.ZekOLOutDto.Synonyms == null || response.ZekOLOutDto.Synonyms.Count == 0)
            {
                //unique result, we have the person and contract data we wanted
                ZekData.syszek = (int)response.sysAuskunft;
                if (SetDetails(ZekData, response.ZekOLOutDto))
                    ZekResponse.Add(ZekData); 
            }
            else
            {
                //abiguous result, we have a list of possible persons without contract data


                //cache original input data
                long syszek = ZekData.syszek;
                AccountDto previous = ZekData.person;

                //create the list of result data
                int uniqueId = 1;
                foreach (var person in response.ZekOLOutDto.Synonyms)
                {
                    Cic.One.DTO.ZekDto zek = ObjectCloner.Clone<Cic.One.DTO.ZekDto>(ZekData);
                    zek.person = AutoMapper.Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto, AccountDto>(person);
                    zek.zekcustomerid = person.KundenId;
                    zek.syszek = uniqueId;
                    ZekResponse.Add(zek);
                    uniqueId++;
                }

                //recover original input data
                ZekData.syszek = syszek;
                ZekData.person = previous;
            }

            ZekData = previousZekData;
        }

        /// <summary>
        /// transfer contract data to our data structure
        /// </summary>
        /// <param name="zek">our local object</param>
        /// <param name="details">data including contract data from zek</param>
        /// <returns>There is distinct zek data for setting details</returns>
        private static bool SetDetails(Cic.One.DTO.ZekDto zek, ZekOLOutDto details)
        {
            if (zek == null)
                return false;

            
            zek.contracts.Clear();
            zek.pleas.Clear();

            if (details == null)
                return false; //no distinct zek response means no contract data

            if (details.FoundPerson != null) //only overwrite person data we used for the request if there is response person data
            {
                zek.person = AutoMapper.Mapper.Map<ZekAddressDescriptionDto, AccountDto>(details.FoundPerson);
                zek.zekcustomerid = details.FoundPerson.KundenId;
            }

            if (details.FoundContracts == null)
                return false;

            //map the different contract types to our displayed common contract type

            if (details.FoundContracts.BardarlehenContracts != null)
            {
                foreach (ZekBardarlehenDescriptionDto bar in details.FoundContracts.BardarlehenContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekBardarlehenDescriptionDto, ZekContractDto>(bar));
                }
            }
            if (details.FoundContracts.FestkreditContracts != null)
            {
                foreach (ZekFestkreditDescriptionDto fest in details.FoundContracts.FestkreditContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekFestkreditDescriptionDto, ZekContractDto>(fest));
                }
            }
            if (details.FoundContracts.AmtsinformationContracts != null)
            {
                foreach (ZekAmtsinformationDescriptionDto amt in details.FoundContracts.AmtsinformationContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekAmtsinformationDescriptionDto, ZekContractDto>(amt));
                }
            }
            if (details.FoundContracts.KartenengagementContracts != null)
            {
                foreach (ZekKartenengagementDescriptionDto contract in details.FoundContracts.KartenengagementContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekKartenengagementDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.KarteninformationContracts != null)
            {
                foreach (ZekKarteninformationDescriptionDto contract in details.FoundContracts.KarteninformationContracts)
                {
                    zek.kartenmeldungen.Add(AutoMapper.Mapper.Map<ZekKarteninformationDescriptionDto, ZekKartenmeldungDto>(contract));
                }
            }
            if (details.FoundContracts.KontokorrentkreditContracts != null)
            {
                foreach (ZekKontokorrentkreditDescriptionDto contract in details.FoundContracts.KontokorrentkreditContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.KreditgesuchContracts != null)
            {
                foreach (ZekKreditgesuchDescriptionDto plea in details.FoundContracts.KreditgesuchContracts)
                {
                    zek.pleas.Add(AutoMapper.Mapper.Map<ZekKreditgesuchDescriptionDto, ZekPleaDto>(plea));
                }
            }
            if (details.FoundContracts.LeasingMietvertragContracts != null)
            {
                foreach (ZekLeasingMietvertragDescriptionDto contract in details.FoundContracts.LeasingMietvertragContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.SolidarschuldnerContracts != null)
            {
                foreach (ZekSolidarschuldnerDescriptionDto contract in details.FoundContracts.SolidarschuldnerContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekSolidarschuldnerDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.TeilzahlungskreditContracts != null)
            {
                foreach (ZekTeilzahlungskreditDescriptionDto contract in details.FoundContracts.TeilzahlungskreditContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.TeilzahlungsvertragContracts != null)
            {
                foreach (ZekTeilzahlungsvertragDescriptionDto contract in details.FoundContracts.TeilzahlungsvertragContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.UeberziehnungskreditContracts != null)
            {
                foreach (ZekUeberziehungskreditDescriptionDto contract in details.FoundContracts.UeberziehnungskreditContracts)
                {
                    zek.contracts.Add(AutoMapper.Mapper.Map<ZekUeberziehungskreditDescriptionDto, ZekContractDto>(contract));
                }
            }
            if (details.FoundContracts.ECode178Contracts != null)
            {
                foreach (ZekOLeCode178DtoDescriptionDto ecode in details.FoundContracts.ECode178Contracts)
                {
                    zek.ecodes.Add(AutoMapper.Mapper.Map<ZekOLeCode178DtoDescriptionDto, ZekECodeDto>(ecode));
                }
            }

            return true;
        }
        #endregion mapping

        /// <summary>
        /// check if the request is allowed with given input data. If not, throw an exception.
        /// </summary>
        public void AssertRequestAllowed()
        {
            if (ZekData == null || ZekData.abfragedaten == null)
                throw new ArgumentException("Keine Abfragedaten verfügbar. Die ZEK Abfrage muss in einem Kontext stehen.", "Gebiet/Nummer");

            if (!DAOFactoryFactory.getInstance().getEntityDao().getExists(ZekData.abfragedaten.area, ZekData.abfragedaten.sysid))
            {
                switch (ZekData.abfragedaten.area)
                {
                    case "VT":
                        throw new ArgumentException ("Vertrag " + ZekData.nummer + " wurde nicht gefunden. Zusatzinformation für Support: " + ZekData.abfragedaten.sysid, "Gebiet/Nummer");
					case "ANTRAG":
						throw new ArgumentException ("Antrag " + ZekData.nummer + " wurde nicht gefunden. Zusatzinformation für Support: " + ZekData.abfragedaten.sysid, "Gebiet/Nummer");
					//case "ANGEBOT": // rh ANG als ANT behandeln
					//	throw new ArgumentException ("Angebot " + ZekData.nummer + " wurde nicht gefunden. Zusatzinformation für Support: " + ZekData.abfragedaten.sysid, "Gebiet/Nummer");
                    case "PERSON":
                        throw new ArgumentException ("Vertriebspartner " + ZekData.nummer + " wurde nicht gefunden. Zusatzinformation für Support: " + ZekData.abfragedaten.sysid, "Gebiet/Nummer");
                    default:
                        throw new ArgumentException ("Der Kontext " + ZekData.abfragedaten.area + " " + ZekData.nummer + " wurde nicht gefunden. Zusatzinformation für Support: " + ZekData.abfragedaten.sysid, "Gebiet/Nummer");
                }
            }
        }


        /// <summary>
        /// execute and process the zek request
        /// </summary>
        /// <returns></returns>
        public List<Cic.One.DTO.ZekDto> DoRequest()
        {
            AssertRequestAllowed();

            AuskunftOLDto auskunft = new AuskunftOLDto();
            auskunft.syswfuser = SysWfUser;
            auskunft.sysAuskunft = 0;
            auskunft.externeAbrage = true;
            auskunft.bemerkung = ZekData.bemerkung;
            auskunft.area = ZekData.abfragedaten.area;
            auskunft.sysAreaid = ZekData.abfragedaten.sysid;
            auskunft.ZekOLInDto = new ZekOLInDto();
            auskunft.ZekOLInDto.Anfragegrund = (int)ZekData.abfragedaten.sysauskunfttyp;
            auskunft.ZekOLInDto.Zielverein = ZekData.zielverein;
            auskunft.ZekOLInDto.RequestEntity = new ZekRequestEntityDto();
            auskunft.ZekOLInDto.RequestEntity.AddressDescription = AutoMapper.Mapper.Map<AccountDto, ZekAddressDescriptionDto>(ZekData.person);
            if (ZekData.person != null && ZekData.person.code != null && ZekData.person.code.Length > 0)
                try
                {
                    auskunft.ZekOLInDto.RequestEntity.RefNo = Convert.ToInt32(ZekData.person.code);
                }
                catch (Exception e)
                {
                    _log.Debug("Parse code to RefNo", e);
                }
            auskunft.ZekOLInDto.RequestEntity.AddressDescription.UIDNummer = ZekData.zekcustomerid;
            auskunft.ZekOLInDto.RequestEntity.AddressDescription.KundenId = ZekData.zekcustomerid;

            switch (ZekData.abfragedaten.area)
            {
                case "VT":
                    auskunft.vertragnummer = ZekData.nummer;
                    break;
                case "ANTRAG":
                    auskunft.antragnummer = ZekData.nummer;
                    break;
                case "PERSON":
                    auskunft.vpnummer = ZekData.nummer;
                    break;
            }

            try
            {
                auskunft = ZekAnfrage.informativabfrageOL(auskunft);
                if (auskunft != null)
                    _log.Debug("ZEK Informativabfrage Status: " + auskunft.Status);
                else
                    _log.Debug("Die ZEK Informativabfrage hat kein Ergebnis geliefert.");

				//—————————————————————————————————————————
				// ZEK - DEBUG Schalter 1/2
				//—————————————————————————————————————————
				bool debug = false;
                if (debug)
                    throw new Exception("Debugging");
				//—————————————————————————————————————————
			}
            catch (Exception e)
            {
                _log.Error("Die ZEK Informativabfrage ist fehlgeschlagen.", e);
                if (auskunft != null)
                    auskunft.Status += e.ToString();

				//—————————————————————————————————————————
				// ZEK - DEBUG Schalter 2/2
				//—————————————————————————————————————————
				// Debug data
				bool debug = false;
                if (debug)
                {
                    auskunft.ZekOLOutDto = new ZekOLOutDto();
                    bool distinctResult = true;
                    if (distinctResult)
                    {
                        auskunft.ZekOLOutDto.FoundPerson = AutoMapper.Mapper.Map<AccountDto, ZekAddressDescriptionDto>(ZekData.person);
                        auskunft.ZekOLOutDto.FoundPerson.UIDNummer = "1";
                        auskunft.ZekOLOutDto.FoundContracts = new ZekOLFoundContractsDto();
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts = new List<ZekOLBardarlehenDescriptionDto>();
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts.Add(new ZekOLBardarlehenDescriptionDto());
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[0].anzahlMonatlicheRaten = 9;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[0].kreditbetrag = 500;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[0].filiale = 17;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[0].kreditVertragID = "123";
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts.Add(new ZekOLBardarlehenDescriptionDto());
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[1].anzahlMonatlicheRaten = 9;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[1].kreditbetrag = 1000;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[1].filiale = 17;
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[1].kreditVertragID = "234";
                        auskunft.ZekOLOutDto.FoundContracts.BardarlehenContracts[1].datumErsteRate = "01.09.2016";
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts = new List<ZekOLKreditgesuchDescriptionDto>();
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts.Add(new ZekOLKreditgesuchDescriptionDto());
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[0].Ablehnungsgrund = 4;
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[0].DatumAblehnung = "17.10.2015";
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[0].KreditVertragID = "992";
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[0].Filiale = 41;
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts.Add(new ZekOLKreditgesuchDescriptionDto());
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[1].Ablehnungsgrund = 8;
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[1].DatumAblehnung = "17.10.2015";
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[1].KreditVertragID = "991";
                        auskunft.ZekOLOutDto.FoundContracts.KreditgesuchContracts[1].Filiale = 41;
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts = new List<ZekOLeCode178DtoDescriptionDto>();
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts.Add(new ZekOLeCode178DtoDescriptionDto());
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts[0].Haendlenummer = "17";
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts[0].Datumgueltigab = "17.10.2015";
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts[0].Ecode178id = "991";
                        auskunft.ZekOLOutDto.FoundContracts.ECode178Contracts[0].Fzstammnummer = "41";
                        auskunft.sysAuskunft = 199627;
                    }
                    else
                    {
                        auskunft.ZekOLOutDto.Synonyms = new List<ZekAddressDescriptionDto>();
                        var person = new ZekAddressDescriptionDto();
                        person.UIDNummer = "17";
                        person.FirstName = "Anna";
                        person.Name = "Musterfrau";
                        person.Street = "Münchner Straße";
                        person.KundenId = "17";
                        auskunft.ZekOLOutDto.Synonyms.Add(person);
                        person = new ZekAddressDescriptionDto();
                        person.UIDNummer = "18";
                        person.FirstName = "Hans";
                        person.Name = "Mustermann";
                        person.Street = "Industriestraße";
                        person.KundenId = "18";
                        auskunft.ZekOLOutDto.Synonyms.Add(person);
                        person = AutoMapper.Mapper.Map<AccountDto, ZekAddressDescriptionDto>(ZekData.person);
                        auskunft.ZekOLOutDto.Synonyms.Add(person);
                    }
                }
				//—————————————————————————————————————————
				//end of Debug
				//—————————————————————————————————————————
            }

            MapResponse(auskunft);
            return ZekResponse;
        }
    }
}