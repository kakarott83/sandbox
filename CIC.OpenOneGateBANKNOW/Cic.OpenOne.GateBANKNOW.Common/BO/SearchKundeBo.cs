using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    using System.Globalization;

    using AutoMapper;

    using CrifSoapService;

    using DAO.Auskunft;

    using DTO;
    using AutoMapper.Configuration;

    public interface ISearchKundeBo
    {
        osearchKundeExternNonGenericDto searchKundeExternNonGeneric(KundeExternDto searchInput);
    }

    public abstract class AbstractSearchKundeBo : ISearchKundeBo
    {
        protected readonly CrifWSDao crifWsDao;
        protected readonly ICrifDBDao crifDbDao;

        protected AbstractSearchKundeBo(CrifWSDao crifWsDao, ICrifDBDao crifDbDao)
        {
            this.crifWsDao = crifWsDao;
            this.crifDbDao = crifDbDao;
        }

        public abstract osearchKundeExternNonGenericDto searchKundeExternNonGeneric(KundeExternDto searchInput);
    }

    public class SearchKundeBo : AbstractSearchKundeBo
    {
        //private IMapper mapper;
        public SearchKundeBo(CrifWSDao crifWsDao, ICrifDBDao crifDbDao)
            : base(crifWsDao, crifDbDao)
        {
           /* mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("SEARCHKUNDE", delegate (MapperConfigurationExpression cfg)
            {
                cfg.CreateMap<Candidate, KundeExternResultDto>()
                .ConstructUsing(src => GetKundeExtern(src.address))
                .ForMember(dest => dest.rang, opt => opt.MapFrom(src => src.candidateRank))
                .ForMember(dest => dest.groupId, opt => opt.MapFrom(src => src.groupId))
                .ForMember(dest => dest.adressId, opt => opt.MapFrom(src => GetAddressId(src.identifiers)))
                ;


                cfg.CreateMap<Location, KundeExternResultDto>()
                    .ForMember(dest => dest.ort, opt => opt.MapFrom(src => src.city))
                    .ForMember(dest => dest.land, opt => opt.MapFrom(src => src.country))
                    .ForMember(dest => dest.hsnr, opt => opt.MapFrom(src => src.houseNumber))
                    .ForMember(dest => dest.regionCode, opt => opt.MapFrom(src => src.regionCode))
                    .ForMember(dest => dest.strasse, opt => opt.MapFrom(src => src.street))
                    .ForMember(dest => dest.subRegionCode, opt => opt.MapFrom(src => src.subRegionCode))
                    .ForMember(dest => dest.plz, opt => opt.MapFrom(src => src.zip))
                    ;

                cfg.CreateMap<PersonAddressDescription, KundeExternResultDto>()
                    .ForMember(dest => dest.gebdatum, opt => opt.MapFrom(src => GetDateTime(src.birthDate)))
                    .ForMember(dest => dest.coname, opt => opt.MapFrom(src => src.coName))
                    .ForMember(dest => dest.vorname, opt => opt.MapFrom(src => src.firstName))
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.lastName))
                    .ForMember(dest => dest.geburtsName, opt => opt.MapFrom(src => src.maidenName))
                    .ForMember(dest => dest.zweiterVorname, opt => opt.MapFrom(src => src.middleName))
                    .ForMember(dest => dest.anredeCode, opt => opt.MapFrom(src => GetAnredeCode(src.sex, src.sexSpecified)))
                    .ForMember(dest => dest.firma, opt => opt.MapFrom(src => 0))
                    ;

                cfg.CreateMap<CompanyAddressDescription, KundeExternResultDto>()
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.companyName))
                    .ForMember(dest => dest.coname, opt => opt.MapFrom(src => src.coName))
                    .ForMember(dest => dest.firma, opt => opt.MapFrom(src => 1))
                    ;

                cfg.CreateMap<AddressDescription, KundeExternResultDto>()
                    .Include<PersonAddressDescription, KundeExternResultDto>()
                    .Include<CompanyAddressDescription, KundeExternResultDto>()
                    ;

                cfg.CreateMap<MatchedAddress, KundeExternResultDto>()
                    .ConstructUsing(src => GetKundeExtern(src.address))
                    .ForMember(dest => dest.rang, opt => opt.MapFrom(src => -1))
                    .ForMember(dest => dest.identifikationsTyp, opt => opt.MapFrom(src => src.identificationType.ToString()))
                    .ForMember(dest => dest.adressId, opt => opt.MapFrom(src => GetAddressId(src.identifiers)))
                    ;

                cfg.CreateMap<LocationIdentification, osearchKundeExternNonGenericDto>()
                    .ForMember(dest => dest.haustyp, opt => opt.MapFrom(src => src.houseType))
                    .ForMember(dest => dest.adresstyp, opt => opt.MapFrom(src => src.locationIdentificationType.ToString()))
                    ;

                cfg.CreateMap<AddressMatchResult, osearchKundeExternNonGenericDto>()
                    .ForMember(dest => dest.character, opt => opt.MapFrom(src => GetStringValue(src.character, src.characterSpecified)))
                    .ForMember(dest => dest.result, opt => opt.MapFrom(src => GetResults(src.foundAddress, src.candidates)))
                    .ForMember(dest => dest.ergebnis, opt => opt.MapFrom(src => src.addressMatchResultType.ToString()))
                    .ForMember(dest => dest.namehint, opt => opt.MapFrom(src => GetStringValue(src.nameHint, src.nameHintSpecified)))
                    ;

                cfg.CreateMap<TypeIdentifyAddressResponse, osearchKundeExternNonGenericDto>();
            });*/
        }

        public static KundeExternResultDto GetKundeExtern(AddressDescription address)
        {
            var result = new KundeExternResultDto();
            var personAddressDescription = address as PersonAddressDescription;
            var companyAddressDescription = address as CompanyAddressDescription;

            if (personAddressDescription != null)
            {
                Mapper.Map(personAddressDescription, result);
            }
            else if (companyAddressDescription != null)
            {
                Mapper.Map(companyAddressDescription, result);
            }

            if (address != null)
            {
                Mapper.Map(address.location, result);
                foreach (var contactItem in address.contactItems ?? Enumerable.Empty<ContactItem>())
                {
                    switch (contactItem.contactType)
                    {
                        case ContactType.PHONE:
                            result.telefonnummer = contactItem.contactText;
                            break;
                        case ContactType.MOBILE:
                            result.handynummer = contactItem.contactText;
                            break;
                        case ContactType.FAX:
                            result.faxnummer = contactItem.contactText;
                            break;
                        case ContactType.EMAIL:
                            result.email = contactItem.contactText;
                            break;
                        case ContactType.WEB:
                            result.web = contactItem.contactText;
                            break;
                        case ContactType.OTHER:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return result;
        }

        public static string GetAddressId(Identifier[] identifiers)
        {
            var id = identifiers.FirstOrDefault(identifier => identifier.identifierType == IdentifierType.ADDRESS_ID);
            if (id != null)
            {
                return id.identifierText;
            }

            return null;
        }

        public static string GetAnredeCode(Sex sex, bool sexSpecified)
        {
            if (!sexSpecified)
            {
                return "3";
            }

            switch (sex)
            {
                case Sex.MALE:
                    return "2";
                case Sex.FEMALE:
                    return "1";
                case Sex.UNKNOWN:
                    return "3";
                default:
                    return "3";
            }
        }

        public static DateTime? GetDateTime(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            string pattern1 = "yyyy-MM-dd";
            string pattern2 = "yyyy";
            DateTime resultDate;
            if (DateTime.TryParseExact(date, new[] { pattern1, pattern2 }, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate))
            {
                return resultDate;
            }
            return null;
        }

        public override osearchKundeExternNonGenericDto searchKundeExternNonGeneric(KundeExternDto searchInput)
        {
            var request = new TypeIdentifyAddressRequest()
            {
                searchedAddress = GetAddress(searchInput) 
            };

            this.crifDbDao.FillHeader(0, request, 0, "CRIF_MANU");
            var response = this.crifWsDao.IdentifyAddress(request);
            var result = Mapper.Map(response, new osearchKundeExternNonGenericDto());
            Mapper.Map(response.addressMatchResult, result);
            if(response.addressMatchResult != null)
            {
                Mapper.Map(response.addressMatchResult.locationIdentification, result);
            }
            return result;
        }

        public static List<KundeExternResultDto> GetResults(MatchedAddress foundAddress, Candidate[] candidates)
        {
            var foundKundeExtern = Mapper.Map<MatchedAddress, KundeExternResultDto>(foundAddress);
            if (foundAddress != null)
            {
                Mapper.Map(foundAddress.address, foundKundeExtern);
            }

            var candidatesKundeExtern = Mapper.Map(candidates, new List<KundeExternResultDto>());

            var results = new[]
                {
                    foundKundeExtern
                }
                .Concat(candidatesKundeExtern)
                .Where(kunde => kunde != null);

            return results.ToList();
        }

        public static string GetStringValue(Enum @enum, bool specified)
        {
            if (!specified)
            {
                return null;
            }
            return @enum.ToString();
        }

        public static AddressDescription GetAddress(KundeExternDto searchInput)
        {
            AddressDescription result;
            if (searchInput.firma == 1)
            {
                result = new CompanyAddressDescription()
                {
                    coName = searchInput.coname,
                    companyName = searchInput.name
                };
            }
            else
            {
                result = new PersonAddressDescription()
                {
                    coName = searchInput.coname,
                    birthDate =  GetDate(searchInput.gebdatum),
                    firstName = searchInput.vorname,
                    lastName = searchInput.name,
                    maidenName = searchInput.geburtsName,
                    middleName = searchInput.zweiterVorname,
                    sex = GetGender(searchInput.anredeCode),
                    sexSpecified = true
                };
            }

            result.location = GetLocation(searchInput);

            result.contactItems = new[]
            {
                GetContactItem(searchInput.email, ContactType.EMAIL),
                GetContactItem(searchInput.faxnummer, ContactType.FAX),
                GetContactItem(searchInput.handynummer, ContactType.MOBILE),
                GetContactItem(searchInput.telefonnummer, ContactType.PHONE),
                GetContactItem(searchInput.web, ContactType.WEB)
            };

            result.contactItems = result.contactItems.Where(contactItem => !string.IsNullOrEmpty(contactItem.contactText)).ToArray();

            return result;
        }

        public static Sex GetGender(string anredeCode)
        {
            if (anredeCode == "1")
            {
                return Sex.FEMALE;
            }
            else if (anredeCode == "2")
            {
                return Sex.MALE;
            }
            return Sex.UNKNOWN;
        }

        private static Location GetLocation(KundeExternDto searchInput)
        {
            return new Location()
            {
                apartment = null,
                city = searchInput.ort,
                country = searchInput.land,
                houseNumber = searchInput.hsnr,
                regionCode = searchInput.regionCode,
                subRegionCode = searchInput.subRegionCode,
                street = searchInput.strasse,
                zip = searchInput.plz
            };
        }

        private static string GetDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }
            return date.Value.ToString("yyyy-MM-dd");
        }

        private static ContactItem GetContactItem(string text, ContactType type)
        {
            return new ContactItem()
            {
                contactText = text,
                contactType = type,
                contactTypeSpecified = true
            };
        }
    }
}
