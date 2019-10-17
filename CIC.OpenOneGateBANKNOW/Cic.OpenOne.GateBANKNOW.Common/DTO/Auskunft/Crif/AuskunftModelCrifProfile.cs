namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Crif
{
    using AutoMapper;
    using Cic.OpenOne.Common.Model.CRIF;
    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EQUIFAX;
    using CIC.Database.CRIF.EF6.Model;
    using CrifSoapService;
    using Devart.Data.Oracle;
    using OpenOne.Common.Util.Logging;
    using Schufa;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class AuskunftModelCrifProfile : AuskunftModelProfileBase
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private class EntityResolver<TEntity, TDestination> : IMemberValueResolver<object, object, long?, TDestination> 
            where TEntity : class
        {

            public TDestination Resolve(object source, object destination, long? sourceMember, TDestination destMember, ResolutionContext resContext)
            {
                if (!sourceMember.HasValue || sourceMember.Value == 0)
                    return default(TDestination);

                EntityKey key = null;

                using (CRIFExtended context = new CRIFExtended())
                {
                    try
                    {
                        var type = typeof(TEntity);
                        var entities = context.getObjectContext().MetadataWorkspace.GetEntityContainer(context.getObjectContext().DefaultContainerName, DataSpace.CSpace).BaseEntitySets;
                        var entityType = entities.First(meta => meta.ElementType.Name == type.Name).ElementType;
                        key = new EntityKey(context.getObjectContext().DefaultContainerName + "." + entityType.Name, entityType.KeyMembers[0].Name, sourceMember.Value);

                        var entity = (TEntity)context.getObjectContext().GetObjectByKey(key);
                        return resContext.Mapper.Map<TEntity, TDestination>(entity);
                    }
                    catch (Exception ex)
                    {
                        if (key != null)
                        {
                            _log.Debug("(EntityResolver) Could not find entity with key: " + key.ToString(), ex);
                        }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// provides the primary key
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        public class EntityStorer<TElement, TEntity> : IMemberValueResolver<object, object, TElement, long?>
            
        {
            public long? Resolve(object source, object destination, TElement sourceMember, long? destMember, ResolutionContext resContext)
            {
                if (sourceMember == null)
                    return null;

                using (CRIFExtended context = new CRIFExtended())
                {
                    try
                    {
                        var entity = resContext.Mapper.Map(sourceMember, sourceMember.GetType(), typeof(TEntity));
                        var entityName = typeof(TEntity).Name;
                        context.getObjectContext().AddObject(entityName, entity);
                        context.SaveChanges();
                        context.Detach(entity);

                        var key = context.getObjectContext().CreateEntityKey(entityName, entity);
                        var firstKey = key.EntityKeyValues.FirstOrDefault();

                        if (firstKey != null && firstKey.Value is long)
                        {
                            //ReportPropertyChanged
                            ReportPropertyChanged(entity, firstKey.Key);
                            return (long)firstKey.Value;
                        }
                        return null;
                    }
                    catch (Exception ex)
                    {
                        _log.Debug("Could not save entity of type '" + sourceMember.GetType() + "' in table '" + typeof(TEntity) + "'", ex);
                        throw;
                    }
                }
            }
        }

        public static void ReportPropertyChanged(object entity, string propertyName)
        {
            var key = GetKey(entity, propertyName);
            List<Action> list;
            if (callbacks.TryGetValue(key, out list))
            {
                foreach (var action in list)
                {
                    action();
                }

                callbacks.Remove(key);
            }
        }

        private static readonly Dictionary<Tuple<object, string>, List<Action>> callbacks = new Dictionary<Tuple<object, string>, List<Action>>();
        

        private static void RegisterPropertyChanged<TEntity>(TEntity entity, string keyName, Action<long?> savedAction)
        {
            var key = GetKey(entity, keyName);
            List<Action> list;
            if (!callbacks.TryGetValue(key, out list))
            {
                list = new List<Action>();
                callbacks.Add(key, list);
            }

            list.Add(() =>
            {
                var property = typeof(TEntity).GetProperty(keyName);
                var keyValue = property.GetValue(entity) as long?;
                if (keyValue != 0)
                {
                    savedAction(keyValue);
                }
            });
        }

        private static Tuple<object, string> GetKey(object entity, string keyName)
        {
            return Tuple.Create(entity, keyName);
        }

        private class KeyValueResolver<T, TDestination> : IMemberValueResolver<object, object, long?, TDestination[]>
        {
            private readonly string context;
            private readonly string area;

            public KeyValueResolver(string context = null)
            {
                this.context = context;
                area = typeof(T).Name;
            }

            public TDestination[] Resolve(object source, object destination, long? sourceMember, TDestination[] destMember, ResolutionContext resContext)
            {
                if (!sourceMember.HasValue || sourceMember.Value == 0)
                    return null;

                using (CRIFExtended ctx = new CRIFExtended())
                {
                    if(string.IsNullOrEmpty(context))
                    {
                        var resultList = ctx.CFKEYVALUE.SqlQuery("Select * from CFKEYVALUE where sysid=:sysid and area=:area", new object[] { new OracleParameter("sysid", sourceMember.Value), new OracleParameter("area", area) });
                        return resContext.Mapper.Map(resultList, new List<TDestination>()).ToArray();
                    }
                    else
                    {
                        var resultList = ctx.CFKEYVALUE.SqlQuery("Select * from CFKEYVALUE where sysid=:sysid and area=:area and context=:context", new object[] { new OracleParameter("sysid", sourceMember.Value), new OracleParameter("area", area), new OracleParameter("context", context) });
                        return resContext.Mapper.Map(resultList, new List<TDestination>()).ToArray();
                    }

                    /*
                    var resultList = ctx.CFKEYVALUE.Where(
                        cfKeyValue => cfKeyValue.SYSID == sourceMember.Value
                                      && cfKeyValue.AREA == area
                                      && (string.IsNullOrEmpty(context) || context == cfKeyValue.CONTEXT))
                              .ToList();

                    return resContext.Mapper.Map(resultList, new List<TDestination>()).ToArray();*/
                }
            }
        }

        public class EnumResolver<T> : IMemberValueResolver<object, object, string, T?> where T : struct
        {
            public T? Resolve(object source, object destination, string sourceMember, T? destMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(sourceMember))
                    return null;

                T result;
                if (Enum.TryParse(sourceMember, out result))
                {
                    return result;
                }
                return null;
            }
        }

        public AuskunftModelCrifProfile()
        {
            CreateMap<bool, int?>().ConvertUsing(value => value ? 1 : 0);
            CreateMap<bool, int>().ConvertUsing(value => value ? 1 : 0);
            CreateMap<bool?, int?>().ConvertUsing(value => value == true ? 1 : 0);


            // ---------------------------------------
            // ---------------------------------------
            // CrifT to DB
            // ---------------------------------------
            // ---------------------------------------


            // Implemented below
            //CreateMap<AddressDescription, CFADDRESS>();
            //CreateMap<PersonAddressDescription, CFADDRESS>();
            //CreateMap<CompanyAddressDescription, CFADDRESS>();


            CreateMap<IdVerificationPerson, CFPERSON>()
                .ForMember(dest => dest.NATIONALID, opt => opt.MapFrom(src => src.nationalIdentificationNumber))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                ;

            CreateMap<IdVerificationDocument, CFIDVERIFICAT>()
                .ForMember(dest => dest.DOCUMENTTYPE, opt => opt.MapFrom(src => GetSpecified(src.documentType, src.documentTypeSpecified)))
                ;

            CreateMap<IdVerificationRequestData, CFIDVERIFICAT>()
                .ForMember(dest => dest.DOCUMENTTYPE, opt => opt.MapFrom((src => GetSpecified(src.documentType, src.documentTypeSpecified))))
                .ForMember(dest => dest.SYSCFPERSON, opt => opt.ResolveUsing(new EntityStorer<IdVerificationPerson, CFPERSON>(), src => src.person))
                .AfterMap((idVerification, cfIdVerification, resContext) =>
                {
                    resContext.Mapper.Map(idVerification.document, cfIdVerification);
                    SaveKeyValuesOnChanged(cfIdVerification, idVerification.documentImage);
                });


            CreateMap<Identifier, CFINPGETREPORT>()
                .ForMember(dest => dest.IDENTIFIERTYPE, opt => opt.MapFrom(src => src.identifierType))
                .ForMember(dest => dest.IDENTIFIERTEXT, opt => opt.MapFrom(src => src.identifierText));

            CreateMap<CrifGetReportInDto, CFINPGETREPORT>()
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.searchedAddress))
                .ForMember(dest => dest.SYSCFIDVERIFICAT, opt => opt.ResolveUsing(new EntityStorer<IdVerificationRequestData, CFIDVERIFICAT>(), src => src.idVerificationRequestData))

                .ForMember(dest => dest.TARGETREPORTFORMAT, opt => opt.MapFrom((src => GetSpecified(src.targetReportFormat, src.targetReportFormatSpecified))))
                .AfterMap((report, cfreport, resContext) =>
                {
                    resContext.Mapper.Map(report.identifier, cfreport);
                    SaveKeyValuesOnChanged(cfreport, report.additionalInput);
                });



            // ---------------------------------------
            // ---------------------------------------
            // DB to CrifT
            // ---------------------------------------
            // ---------------------------------------

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //-----------------------------------------CrifIdentifyAddress------------------------------------------------
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


            CreateMap<CFHEADER, IdentityDescriptor>();
            CreateMap<CFHEADER, Control>();

            CreateMap<CFHEADER, TypeBaseRequest>()
                .ForMember(dest => dest.control, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.identityDescriptor, opt => opt.MapFrom(src => src));

            CreateMap<CFKEYVALUE, ContactItem>()
                .ForMember(dest => dest.contactType, opt => opt.MapFrom(src => ParseEnum<ContactType>(src.KEYVAL)))
                .ForMember(dest => dest.contactText, opt => opt.MapFrom(src => src.VALUE))
                .ForMember(dest => dest.contactTypeSpecified, opt => opt.MapFrom(src => IsSpecified<ContactType>(src.KEYVAL)));

            CreateMap<CFADDRESS, Location>();

            CreateMap<CFADDRESS, CompanyAddressDescription>()
                .ForMember(dest => dest.contactItems, opt => opt.ResolveUsing(new KeyValueResolver<CFADDRESS, ContactItem>("contactItems"), src => (long?)src.SYSCFADDRESS))
                .ForMember(dest => dest.location, opt => opt.MapFrom(src => src))
                ;

            CreateMap<CFADDRESS, PersonAddressDescription>()
                .ForMember(dest => dest.contactItems, opt => opt.ResolveUsing(new KeyValueResolver<CFADDRESS, ContactItem>("contactItems"), src => src.SYSCFADDRESS))
                .ForMember(dest => dest.location, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.sex, opt => opt.MapFrom(src => ParseEnum<Sex>(src.SEX)))
                .ForMember(dest => dest.sexSpecified, opt => opt.MapFrom(src => IsSpecified<Sex>(src.SEX)));

            CreateMap<CFADDRESS, AddressDescription>()
                .ConstructUsing((cfaddress, resContext) =>
                {
                    if (cfaddress.ISCOMPANY == 1)
                    {
                        return resContext.Mapper.Map(cfaddress, new CompanyAddressDescription());
                    }
                    return resContext.Mapper.Map(cfaddress, new PersonAddressDescription());
                });

            CreateMap<CFKEYVALUE, KeyValuePair>()
                .ForMember(dest => dest.key, opt => opt.MapFrom(src => src.KEYVAL))
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => src.VALUE));

            // Identify Address
            CreateMap<CFINPIDENTADDR, CrifIdentifyAddressInDto>()
                .ForMember(dest => dest.searchedAddress, opt => opt.ResolveUsing(new EntityResolver<CFADDRESS, AddressDescription>(), src => src.SYSCFADDRESS))
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPIDENTADDR, KeyValuePair>(), src => src.SYSCFINPIDENTADDR))
                ;

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //-----------------------------------------CrifGetArchivedReportInDto------------------------------------------
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //###
            CreateMap<CFINPARCHDREP, CrifGetArchivedReportInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPARCHDREP, KeyValuePair>(), src => src.SYSCFINPARCHDREP))
                .ForMember(dest => dest.archivingId, opt => opt.MapFrom(cfinparchdrep => TryParseToLong(cfinparchdrep.ARCHIVINGID)))
                ;

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------------------CrifGetListOfReadyOfflinceReports----------------------------------------
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            CreateMap<CFINPLISTOFFRE, CrifGetListOfReadyOfflineReportsInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPLISTOFFRE, KeyValuePair>(), src => src.SYSCFINPLISTOFFRE))
                ;

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------------------CrifPollOfflineReport------------------------------------------------
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            CreateMap<CFINPPOLLOFF, CrifPollOfflineReportInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPPOLLOFF, KeyValuePair>(), src => src.SYSCFINPPOLLOFF))
                ;

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------------------CrifOrderOfflineReport---------------------------------------------------
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            CreateMap<CFINPORDEROFF, Identifier>()
                .ForMember(dest => dest.identifierType, opt => opt.MapFrom(src => ParseEnum<IdentifierType>(src.IDENTIFIERTYPE)));

            CreateMap<CFINPORDEROFF, CrifOrderOfflineReportInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPORDEROFF, KeyValuePair>(), src => src.SYSCFINPORDEROFF))
                .ForMember(dest => dest.identifier,
                    opt =>
                    {
                        opt.Condition(inp => !string.IsNullOrEmpty(inp.IDENTIFIERTYPE));
                        opt.MapFrom(src => src);
                    })
                .ForMember(dest => dest.orderAddress, opt => opt.ResolveUsing(new EntityResolver<CFADDRESS, AddressDescription>(), src => src.SYSCFADDRESS))
                .ForMember(dest => dest.binaryPOIType, opt => opt.MapFrom(src => ParseEnum<PoiType>(src.BINARYPOITYPE)))
                .ForMember(dest => dest.binaryPOITypeSpecified, opt => opt.MapFrom(src => IsSpecified<PoiType>(src.BINARYPOITYPE)))
                .ForMember(dest => dest.offlineReportType, opt => opt.ResolveUsing(src =>
                {
                    if (src != null)
                    {
                        return src.OFFLINEREPORTTYPE.Replace("_EXP", "_EXPRESS");
                    }
                    return null;
                }));

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------------------CrifGetDebtDetail-----------------------------------------------------
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            CreateMap<CFINPDEBTDET, Identifier>()
                .ForMember(dest => dest.identifierType, opt => opt.MapFrom(src => ParseEnum<IdentifierType>(src.IDENTIFIERTYPE)));

            CreateMap<CFINPDEBTDET, CrifGetDebtDetailsInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPDEBTDET, KeyValuePair>(), src => src.SYSCFINPDEBTDET))
                .ForMember(dest => dest.identifier,
                    opt =>
                    {
                        opt.Condition(inp => !string.IsNullOrEmpty(inp.IDENTIFIERTYPE));
                        opt.MapFrom(src => src);
                    })
                ;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------------------CrifGetReport------------------------------------------------
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            CreateMap<CFINPGETREPORT, Identifier>()
                .ForMember(dest => dest.identifierType, opt => opt.MapFrom(src => ParseEnum<IdentifierType>(src.IDENTIFIERTYPE)));

            CreateMap<CFIDVERIFICAT, IdVerificationDocument>()
                .ForMember(dest => dest.documentType, opt => opt.MapFrom(src => ParseEnum<IdVerificationDocumentType>(src.DOCUMENTTYPE))) //is this right ?
                .ForMember(dest => dest.documentTypeSpecified, opt => opt.MapFrom(src => ParseEnum<IdVerificationDocumentType>(src.DOCUMENTTYPE))) //is this right?
                ;

            CreateMap<CFPERSON, IdVerificationPerson>()
                .ForMember(dest => dest.nationalIdentificationNumber, opt => opt.MapFrom(src => src.NATIONALID))
                .ForMember(dest => dest.address, opt => opt.ResolveUsing(new EntityResolver<CFADDRESS, PersonAddressDescription>(), src => src.SYSCFADDRESS))
                ;

            CreateMap<CFKEYVALUE, string>()
                .ConstructUsing(a => a.VALUE);

            CreateMap<CFIDVERIFICAT, IdVerificationRequestData>()
                .ForMember(dest => dest.documentImage, opt => opt.ResolveUsing(new KeyValueResolver<CFIDVERIFICAT, string>(), src => src.SYSCFIDVERIFICAT))
                .ForMember(dest => dest.documentType, opt => opt.MapFrom((src => ParseEnum<IdVerificationDocumentType>(src.DOCUMENTTYPE))))
                .ForMember(dest => dest.documentTypeSpecified, opt => opt.MapFrom(src => IsSpecified<IdVerificationDocumentType>(src.DOCUMENTTYPE)))
                .ForMember(dest => dest.document, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.person, opt => opt.ResolveUsing(new EntityResolver<CFPERSON, IdVerificationPerson>(), src => src.SYSCFPERSON))
                ;

            CreateMap<CFINPGETREPORT, CrifGetReportInDto>()
                .ForMember(dest => dest.additionalInput, opt => opt.ResolveUsing(new KeyValueResolver<CFINPGETREPORT, KeyValuePair>(), src => src.SYSCFINPGETREPORT))
                .ForMember(dest => dest.targetReportFormat, opt => opt.MapFrom((src => ParseEnum<TargetReportFormat>(src.TARGETREPORTFORMAT))))
                .ForMember(dest => dest.targetReportFormatSpecified, opt => opt.MapFrom(src => IsSpecified<TargetReportFormat>(src.TARGETREPORTFORMAT)))
                .ForMember(dest => dest.identifier,
                    opt =>
                    {
                        opt.Condition(inp => !string.IsNullOrEmpty(inp.IDENTIFIERTYPE));
                        opt.MapFrom(src => src);
                    })
                .ForMember(dest => dest.idVerificationRequestData, opt => opt.ResolveUsing(new EntityResolver<CFIDVERIFICAT, IdVerificationRequestData>(), src => src.SYSCFIDVERIFICAT))
                .ForMember(dest => dest.searchedAddress, opt => opt.ResolveUsing(new EntityResolver<CFADDRESS, AddressDescription>(), src => src.SYSCFADDRESS)) //already defined
                ;

            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // CrifT to Request
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------

            CreateMap<CrifIdentifyAddressInDto, TypeIdentifyAddressRequest>();

            //###
            CreateMap<CrifGetArchivedReportInDto, TypeGetArchivedReportRequest>();

            CreateMap<CrifGetListOfReadyOfflineReportsInDto, TypeGetListOfReadyOfflineReportsRequest>();

            CreateMap<CrifPollOfflineReportInDto, TypePollOfflineReportResponseRequest>();

            CreateMap<CrifOrderOfflineReportInDto, TypeOrderOfflineReportRequest>();

            CreateMap<CrifGetDebtDetailsInDto, TypeGetDebtDetailsRequest>();

            CreateMap<CrifGetReportInDto, TypeGetReportRequest>();

            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // Response to CrifT
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------
            // ---------------------------------------

            CreateMap<TypeIdentifyAddressResponse, CrifIdentifyAddressOutDto>();

            //###
            CreateMap<TypeGetArchivedReportResponse, CrifGetArchivedReportOutDto>();

            CreateMap<TypeGetListOfReadyOfflineReportsResponse, CrifGetListOfReadyOfflineReportsOutDto>();

            CreateMap<TypePollOfflineReportResponseResponse, CrifPollOfflineReportOutDto>();

            CreateMap<TypeOrderOfflineReportResponse, CrifOrderOfflineReportOutDto>();

            CreateMap<TypeGetDebtDetailsResponse, CrifGetDebtDetailsOutDto>();

            CreateMap<TypeGetReportResponse, CrifGetReportOutDto>();

            // ---------------------------------------
            // ---------------------------------------
            // Response to DB
            // ---------------------------------------
            // ---------------------------------------

            //**************************************************************************************************************
            //-----------------------------------CrifIdentifyAddress----------------------------------------------------
            //**************************************************************************************************************

            CreateMap<CrifTError, CFERROR>();

            CreateMap<Location, CFADDRESS>()
                .ForMember(dest => dest.STREET, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.ZIP, opt => opt.MapFrom(src => src.zip))
                .ForMember(dest => dest.CITY, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.COUNTRY, opt => opt.MapFrom(src => src.country))
                .ForMember(dest => dest.HOUSENUMBER, opt => opt.MapFrom(src => src.houseNumber))
                .ForMember(dest => dest.APARTMENT, opt => opt.MapFrom(src => src.apartment))
                .ForMember(dest => dest.REGIONCODE, opt => opt.MapFrom(src => src.regionCode))
                .ForMember(dest => dest.SUBREGIONCODE, opt => opt.MapFrom(src => src.subRegionCode))
                .ForMember(dest => dest.ISCOMPANY, opt => opt.Ignore())
                .ForMember(dest => dest.SEX, opt => opt.Ignore())
                ;

            CreateMap<ContactItem, CFKEYVALUE>()
                .ForMember(dest => dest.KEYVAL, opt => opt.MapFrom(src => GetSpecified(src.contactType, src.contactTypeSpecified)))
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => src.contactText));

            CreateMap<AddressDescription, CFADDRESS>()
                .Include<PersonAddressDescription, CFADDRESS>()
                .Include<CompanyAddressDescription, CFADDRESS>()
                .AfterMap((address, cfaddress, resContext) =>
                {
                    resContext.Mapper.Map(address.location, cfaddress);
                    SaveKeyValuesOnChanged(cfaddress, address.contactItems, "contactItems");
                })
                ;

            CreateMap<PersonAddressDescription, CFADDRESS>()
                .ForMember(dest => dest.SEX, opt => opt.MapFrom(src => GetSpecified(src.sex, src.sexSpecified)))
                .ForMember(dest => dest.ISCOMPANY, opt => opt.UseValue(0))
                .ForMember(dest => dest.NATIONALITY, opt => opt.MapFrom(src => src.nationality))
                .AfterMap((address, cfaddress, resContext) =>
                {
                    resContext.Mapper.Map(address.location, cfaddress);
                    SaveKeyValuesOnChanged(cfaddress, address.contactItems, "contactItems");
                })
                ;

            CreateMap<CompanyAddressDescription, CFADDRESS>()
                .ForMember(dest => dest.ISCOMPANY, opt => opt.MapFrom(src => 1))
                .AfterMap((address, cfaddress, resContext) =>
                {
                    resContext.Mapper.Map(address.location, cfaddress);
                    SaveKeyValuesOnChanged(cfaddress, address.contactItems, "contactItems");
                })
                ;

            CreateMap<Identifier, CFKEYVALUE>()
                .ForMember(dest => dest.KEYVAL, opt => opt.MapFrom(src => src.identifierType))
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => src.identifierText));

            CreateMap<MatchedAddress, CFADDRESS>()
                .AfterMap((address, cfaddress, resContext) =>
                {
                    resContext.Mapper.Map(address.address, cfaddress, address.address.GetType(), typeof(CFADDRESS));
                    SaveKeyValuesOnChanged(cfaddress, address.identifiers, "identifiers");
                })
                ;

            CreateMap<LocationIdentification, CFADDRESSMATCH>()
                .ForMember(dest => dest.SYSCFADDRESSNORM, opt => opt.ResolveUsing(new EntityStorer<Location, CFADDRESS>(), src => src.requestLocationNormalized))
                ;

            CreateMap<Candidate, CFCANDIDATE>()
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                .AfterMap((candidate, cfcandidate) => { SaveKeyValuesOnChanged(cfcandidate, candidate.identifiers); });

            CreateMap<AddressMatchResult, CFADDRESSMATCH>()
                .ForMember(dest => dest.NAMEHINT, opt => opt.MapFrom(src => GetSpecified(src.nameHint, src.nameHintSpecified)))
                .ForMember(dest => dest.CHARACTER, opt => opt.MapFrom(src => GetSpecified(src.character, src.characterSpecified)))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<MatchedAddress, CFADDRESS>(), src => src.foundAddress))
                .AfterMap((dto, cfaddressmatch, resContext) =>
                {
                    resContext.Mapper.Map(dto.locationIdentification, cfaddressmatch);
                    SaveEntitiesOnChanged<Candidate, CFADDRESSMATCH, CFCANDIDATE>(cfaddressmatch, dto.candidates, (cfcandidate, sysid) => cfcandidate.SYSCFADDRESSMATCH = sysid);
                })
                ;

            CreateMap<KeyValuePair, CFKEYVALUE>()
                .ForMember(dest => dest.KEYVAL, opt => opt.MapFrom(src => src.key))
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => src.value));

            CreateMap<CrifIdentifyAddressOutDto, CFOUTIDENTADDR>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .ForMember(dest => dest.SYSCFADDRESSMATCH, opt => opt.ResolveUsing(new EntityStorer<AddressMatchResult, CFADDRESSMATCH>(), src => src.addressMatchResult))
                .AfterMap((dto, cfoutidentaddress) => SaveKeyValuesOnChanged(cfoutidentaddress, dto.additionalOutput))
                ;

            //**************************************************************************************************************
            //-----------------------------------CrifGetArchivedReport----------------------------------------------------
            //**************************************************************************************************************
            //###
            CreateMap<CrifGetArchivedReportOutDto, CFOUTARCHREP>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .AfterMap((dto, cfoutarchivedreport) => SaveKeyValuesOnChanged(cfoutarchivedreport, dto.additionalOutput))
                ;

            //***************************************************************************************************************
            //------------------------------------CrifGetListOfReadyOfflinceReports------------------------------------------------
            //***************************************************************************************************************

            //CreateMap<OfflineReportIdentifier, CFOFFLINEREPID>();

            CreateMap<CrifGetListOfReadyOfflineReportsOutDto, CFOUTLISTOFFRE>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .AfterMap((dto, cfoutlistofflinereports) =>
                {
                    SaveEntitiesOnChanged<OfflineReportIdentifier, CFOUTLISTOFFRE, CFOFFLINEREPID>(cfoutlistofflinereports, dto.offlineReportIdentifiers, (cfofflinereportid, sysid) => cfofflinereportid.SYSCFOUTLISTOFFRE = sysid);
                    SaveKeyValuesOnChanged(cfoutlistofflinereports, dto.additionalOutput);
                })
                ;

            //**********************************************************************************************************
            //------------------------------------CrifPollOfflineReport------------------------------------------------
            //**********************************************************************************************************

            CreateMap<OfflineReportIdentifier, CFOFFLINEREPID>();

            CreateMap<Amount, CFDEBT>()
                .ForMember(dest => dest.AMOUNT, opt => opt.MapFrom(src => src.amount))
                .ForMember(dest => dest.AMOUNTCUR, opt => opt.MapFrom(src => src.currency))
                ;

            CreateMap<DebtEntry, CFDEBT>()
                .ForMember(dest => dest.DATECLOSE, opt => opt.MapFrom(src => GetDate(src.dateClose)))
                .ForMember(dest => dest.DATEOPEN, opt => opt.MapFrom(src => GetDate(src.dateOpen)))
                .ForMember(dest => dest.AMOUNT, opt => opt.Ignore())
                .ForMember(dest => dest.AMOUNTOPEN, opt => opt.Ignore())
                .AfterMap((dto, cfdebt, resContext) =>
                {
                    resContext.Mapper.Map(dto.amount, cfdebt);
                    if (dto.amountOpen != null)
                    {
                        cfdebt.AMOUNTOPEN = (decimal)dto.amountOpen.amount;
                        cfdebt.AMOUNTOPENCUR = dto.amountOpen.currency;
                    }
                })
                ;

            CreateMap<CrifPollOfflineReportOutDto, CFOUTPOLLOFF>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .ForMember(dest => dest.OFFLINEREPORTSTATUS, opt => opt.MapFrom(src => GetSpecified(src.offlineReportStatus, src.offlineReportStatusSpecified)))
                .ForMember(dest => dest.SYSCFOFFLINEREPID, opt => opt.ResolveUsing(new EntityStorer<OfflineReportIdentifier, CFOFFLINEREPID>(), src => src.offlineReportIdentifier))
                .ForMember(dest => dest.REJECTIONREASON, opt => opt.MapFrom(src => GetSpecified(src.rejectionReason, src.rejectionReasonSpecified)))
                .AfterMap((dto, cfoutpollofflinerep, resContext) =>
                {
                    SaveKeyValuesOnChanged(cfoutpollofflinerep, dto.additionalOutput);
                    SaveEntitiesOnChanged<DebtEntry, CFOUTPOLLOFF, CFDEBT>(cfoutpollofflinerep, dto.debts, (cfdept, sysid) => cfdept.SYSCFOUTPOLLOFF = sysid);
                })
                ;

            //**********************************************************************************************************
            //------------------------------------CrifOrderOfflineReport------------------------------------------------
            //**********************************************************************************************************
            CreateMap<CrifOrderOfflineReportOutDto, CFOUTORDEROFF>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                ;

            //**********************************************************************************************************
            //------------------------------------CrifGetDebtDetail------------------------------------------------
            //**********************************************************************************************************

            //CreateMap<DebtEntry, CFDEBT>();

            CreateMap<CrifGetDebtDetailsOutDto, CFOUTDEBTDET>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .AfterMap((dto, cfoutdebtdetails, resContext) =>
                {
                    SaveEntitiesOnChanged<DebtEntry, CFOUTDEBTDET, CFDEBT>(cfoutdebtdetails, dto.debts, (cfdebt, sysid) => cfdebt.SYSCFOUTDEBTDET = sysid);
                    SaveKeyValuesOnChanged(cfoutdebtdetails, dto.additionalOutput);
                })
                ;

            //**********************************************************************************************************
            //**********************************************************************************************************
            //------------------------------------CrifGetReport------------------------------------------------
            //**********************************************************************************************************
            //**********************************************************************************************************

            CreateMap<string, CFKEYVALUE>()
                .ConstructUsing(input => new CFKEYVALUE()
                {
                    VALUE = input
                });

            ////-- CFADDRESSMATCH End --//
            //CreateMap<Candidate, CFCANDIDATE>() //already done above
            //    ;

            //CreateMap<AddressMatchResult, CFADDRESSMATCH>() //already done above
                ;

            //-- CFADDRESSMATCH Start --//

            //ⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞ  CFDECISION END  ⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞ 
            CreateMap<Rating, CFKEYVALUE>()
                .ForMember(dest => dest.KEYVAL, opt => opt.MapFrom(src => src.ratingType))
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => src.rating))
                ;

            CreateMap<Subdecision, CFSUBDECISION>(); //done, automapper will find the rest

            CreateMap<DecisionMatrix, CFDECISION>()
                .ForMember(dest => dest.CREDITLIMIT, opt => opt.MapFrom(src => TryParseToInt(src.creditLimit)))
                .AfterMap((dto, cfdecision, resContext) => SaveEntitiesOnChanged<Subdecision, CFDECISION, CFSUBDECISION>(cfdecision, dto.subdecisions, (cfsub, sysid) => cfsub.SYSCFDECISION = sysid))
                .AfterMap(((dto, cfdecision, resContext) => SaveKeyValuesOnChanged(cfdecision, dto.ratings)))
                ;

            //ⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞ  CFDECISION START  ⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞⓞ 

            //۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩  CFPUBLICATION END  ۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩

            CreateMap<Publication, CFPUBLICATION>()
                .ForMember(dest => dest.TEXT, opt => opt.MapFrom(src => Truncate(src.text, 255)))
                .ForMember(dest => dest.PUBLICATIONDATE, opt => opt.MapFrom(src => GetDate(src.publicationDate)))
                .AfterMap((dto, cfpublication, resContext) => SaveKeyValuesOnChanged(cfpublication, dto.publicationLabels))
                ;

            //۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩  CFPUBLICATION START  ۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩۩

            //☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊  CFWOWRELATION/ADDRESS END  ☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊
            CreateMap<WowRelation, CFWOWRELATION>()
                .ForMember(dest => dest.CAPITALSHARE, opt => opt.MapFrom(src => GetSpecified(src.capitalShare, src.capitalShareSpecified)))
                .ForMember(dest => dest.VOTESHARE, opt => opt.MapFrom(src => GetSpecified(src.voteShare, src.voteShareSpecified)))
                ;

            CreateMap<WowAddress, CFWOWADDRESS>()
                .ForMember(dest => dest.HASDEBTS, opt => opt.MapFrom(src => GetSpecified(src.hasDebts, src.hasDebtsSpecified)))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                .AfterMap((dto, cfwowaddress) => SaveKeyValuesOnChanged(cfwowaddress, dto.identifiers))
                ;

            //☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊  CFWOWRELATION/ADDRESS START  ☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊☊

            //๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑  CFADDRESSHISTORY END  ๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑

            CreateMap<Identifier, CFADDRESSHIST>()
                .ForMember(dest => dest.ADDRESSID, opt => opt.MapFrom(src => src.identifierText))
                .ForMember(dest => dest.ADDRESSIDTYPE, opt => opt.MapFrom(src => src.identifierType))
                ;

            CreateMap<AddressWithDeliverability, CFADDRESSHIST>()
                .ForMember(dest => dest.ADDRESSINPUTDATE, opt => opt.MapFrom(src => GetDate(src.addressInputDate)))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                .AfterMap((dto, cfaddresshistory, resContext) => { resContext.Mapper.Map(dto.addressId, cfaddresshistory); })
                ;

            //๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑  CFADDRESSHISTORY START  ๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑๑

            //◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊  CFBRANCHOFFICE END  ◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊

            CreateMap<Period, CFBRANCHOFFICE>()
                .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.startDate)))
                .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.endDate)))
                ;

            CreateMap<BranchOfficeListItem, CFBRANCHOFFICE>()
                .ForMember(dest => dest.REGISTERED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.registered), src.registeredSpecified)))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<CompanyAddressDescription, CFADDRESS>(), src => src.address))
                .AfterMap((dto, cfbranchoffice, resContext) => { resContext.Mapper.Map(dto.period, cfbranchoffice); })
                ;

            //◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊  CFBRANCHOFFICE START  ◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊◊

            ////-------------------------------------   CFDEBT END   -------------------------------------
            //CreateMap<DebtEntry, CFDEBT>() //already defined above
                ;

            //-------------------------------------   CFDEBT START   -------------------------------------

            //●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●  CFSCHUFA END  ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●

            CreateMap<Amount, CFSCHUFAFEAT>()
                .ForMember(dest => dest.AMOUNT, opt => opt.MapFrom(src => src.amount))
                .ForMember(dest => dest.AMOUNTCUR, opt => opt.MapFrom(src => src.currency))
                ;

            CreateMap<SchufaFeature, CFSCHUFAFEAT>()
                .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.Ignore())
                .ForMember(dest => dest.OWNFEATURE, opt => opt.Ignore())
                .ForMember(dest => dest.DATUM, opt => opt.MapFrom(src => GetDate(src.date)))
                .ForMember(dest => dest.NUMBEROFINSTALLEMENTS, opt => opt.MapFrom(src => TryParseToInt(src.numberOfInstallments)))
                .ForMember(dest => dest.INSTALLMENTTYPE, opt => opt.MapFrom(src => src.installmentType))
                .ForMember(dest => dest.AMOUNT, opt => opt.Ignore())
                .AfterMap((dto, cfschufafeature, resContext) => { resContext.Mapper.Map(dto.amount, cfschufafeature); })
                ;

            CreateMap<SchufaTextFeature, CFSCHUFAFEAT>()
                .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.Ignore())
                .ForMember(dest => dest.OWNFEATURE, opt => opt.Ignore())
                ;

            //include does not work in this AutoMapper version as expected, that is why we use ConstructUsing
            CreateMap<SchufaBaseFeature, CFSCHUFAFEAT>()
                .ConstructUsing((feature, resContext) =>
                {
                    var result = new CFSCHUFAFEAT();
                    resContext.Mapper.Map(feature, result, feature.GetType(), result.GetType());
                    return result;
                })
                .ForMember(dest => dest.FEATUREWITHOUTBIRTHDATE, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.featureWithoutBirthdate), src.featureWithoutBirthdateSpecified)))
                .ForMember(dest => dest.OWNFEATURE, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.ownFeature), src.ownFeatureSpecified)))
                ;

            CreateMap<SchufaPersonData, CFSCHUFA>()
                .ForMember(dest => dest.TITLE, opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.PLACEOFBIRTH, opt => opt.MapFrom(src => src.placeOfBirth))
                ;

            CreateMap<SchufaScore, CFSCHUFA>()
                .ForMember(dest => dest.DESCRIPTION, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.SCOREVALUE, opt => opt.MapFrom(src => TryParseToInt(src.scoreValue)))
                .ForMember(dest => dest.SCORECATEGORY, opt => opt.MapFrom(src => src.scoreCategory))
                .ForMember(dest => dest.SCORETEXT, opt => opt.MapFrom(src => src.scoreText))
                .ForMember(dest => dest.SCOREERROR, opt => opt.MapFrom(src => src.scoreError))
                .ForMember(dest => dest.RISKQUOTA, opt => opt.MapFrom(src => GetSpecified(src.riskQuota, src.riskQuotaSpecified)))
                ;

            CreateMap<SchufaIdentification, CFSCHUFA>()
                .ForMember(dest => dest.IDENTITYRESERVATIONPERSON, opt => opt.MapFrom(src => src.identityReservationPerson))
                .ForMember(dest => dest.IDENTITYRESERVATIONADDRESS, opt => opt.MapFrom(src => src.identityReservationAddress))
                .ForMember(dest => dest.PERSONWITHOUTBIRTHDATE, opt => opt.MapFrom(src => src.personWithoutBirthdate))
                ;

            CreateMap<SchufaResponseData, CFSCHUFA>()
                .ForMember(dest => dest.SYSCFADDRESSPREVIOUS, opt => opt.ResolveUsing(new EntityStorer<Location, CFADDRESS>(), src => src.schufaPersonData.previousAddress))
                .AfterMap((dto, cfschufa, resContext) =>
                {
                    resContext.Mapper.Map(dto.schufaPersonData, cfschufa);
                    resContext.Mapper.Map(dto.schufaScore, cfschufa);
                    resContext.Mapper.Map(dto.schufaIdentification, cfschufa);

                    SaveEntitiesOnChanged<SchufaBaseFeature, CFSCHUFA, CFSCHUFAFEAT>(cfschufa, dto.schufaFeatures, (cfschufafeature, sysid) => cfschufafeature.SYSCFSCHUFA = sysid);
                })
                ;

            //I am not sure about <Location,CFADDRESS> because of the Foreign Key
            //●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●  CFSCHUFA START  ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●

            //★★★★★★★★★★★★★★★★★★★★  CFORGANIZATIONPOSITION END ★★★★★★★★★★★★★★★★★★★★

            CreateMap<OrganizationPositionFunction, CFPOSITIONFUNC>()
                .ForMember(dest => dest.FUNCTIONPRIORITY, opt => opt.MapFrom(src => StringToNullableInt(src.functionPriority)))
                ;

            //CreateMap<OrganizationPosition, CFORGAPOSITION>();

            CreateMap<Period, CFORGAPOSITION>()
                .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.startDate)))
                .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.endDate)))
                ;

            CreateMap<Identifier, CFORGAPOSITION>()
                .ForMember(dest => dest.ADDRESSID, opt => opt.MapFrom(src => src.identifierText))
                .ForMember(dest => dest.ADDRESSIDTYPE, opt => opt.MapFrom(src => src.identifierType))
                ;

            CreateMap<OrganizationPosition, CFORGAPOSITION>()
                .ForMember(dest => dest.SHAREVALUE, opt => opt.MapFrom(src => GetSpecified(src.share, src.shareSpecified)))
                .ForMember(dest => dest.SIGNATURETYPE, opt => opt.MapFrom((src => GetSpecified(src.signatureType, src.signatureTypeSpecified))))
                .ForMember(dest => dest.HASDEBTS, opt => opt.MapFrom(src => GetSpecified(src.hasDebts, src.hasDebtsSpecified)))
                .ForMember(dest => dest.NROFPOSITIONS, opt => opt.MapFrom(src => StringToNullableInt(src.nrOfPositions)))
                .ForMember(dest => dest.NROFPOSITIONSBANKRUPT, opt => opt.MapFrom(src => StringToNullableInt(src.nrOfPositionsBankrupt)))
                .ForMember(dest => dest.SYSCFPOSITIONFUNC, opt => opt.ResolveUsing(new EntityStorer<OrganizationPositionFunction, CFPOSITIONFUNC>(), src => src.highestFunction))
                .AfterMap((dto, cforganizationposition, resContext) =>
                {
                    resContext.Mapper.Map(dto.period, cforganizationposition);
                    resContext.Mapper.Map(dto.addressId, cforganizationposition);

                    SaveEntitiesOnChanged<OrganizationPositionFunction, CFORGAPOSITION, CFPOSITIONFUNC>(cforganizationposition, dto.furtherFunctions, (cfpositionfunction, sysid) => cfpositionfunction.SYSCFORGAPOSITION = sysid);
                    SaveEntitiesOnChanged<OrganizationPosition, CFORGAPOSITION, CFORGAPOSITION>(cforganizationposition, dto.organizationPositions, (cforganizationposition1, sysid) => cforganizationposition1.SYSCFORGANIZATIONPOSITIONOTHER = sysid);
                })
                ;

            //★★★★★★★★★★★★★★★★★★★★  CFORGANIZATIONPOSITION START ★★★★★★★★★★★★★★★★★★★★

            //＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊  CFFINSTATEMENT END ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

            CreateMap<CreditRatio, CFCREDITRATIO>()
                ;

            CreateMap<FinancialStatementElement, CFFINELEMENT>()
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => GetSpecified(src.value, src.valueSpecified)))
                .ForMember(dest => dest.PARENTID, opt => opt.MapFrom(src => StringToNullableInt(src.parentId)))
                ;

            CreateMap<Period, CFFINSTATEMENT>()
                .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.startDate)))
                .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.endDate)))
                ;

            CreateMap<FinancialStatement, CFFINSTATEMENT>()
                .AfterMap((dto, cffinstatement, resContext) =>
                {
                    SaveEntitiesOnChanged<FinancialStatementElement, CFFINSTATEMENT, CFFINELEMENT>(cffinstatement, dto.balanceSheet, (cffinelement, sysid) => cffinelement.SYSCFFINSTATEMENTCASHFLOW = sysid);
                    SaveEntitiesOnChanged<FinancialStatementElement, CFFINSTATEMENT, CFFINELEMENT>(cffinstatement, dto.cashFlow, (cffinelement, sysid) => cffinelement.SYSCFFINSTATEMENTFURTHER = sysid);
                    SaveEntitiesOnChanged<FinancialStatementElement, CFFINSTATEMENT, CFFINELEMENT>(cffinstatement, dto.furtherFigures, (cffinelement, sysid) => cffinelement.SYSCFFINSTATEMENTPROFITLOSS = sysid);
                    SaveEntitiesOnChanged<FinancialStatementElement, CFFINSTATEMENT, CFFINELEMENT>(cffinstatement, dto.profitAndLoss, (cffinelement, sysid) => cffinelement.SYSCFFINSTATEMENTBALANCE = sysid);
                    SaveEntitiesOnChanged<CreditRatio, CFFINSTATEMENT, CFCREDITRATIO>(cffinstatement, dto.creditRatios, (cfcreditratio, sysid) => cfcreditratio.SYSCFFINSTATEMENT = sysid); //mapper just now

                    resContext.Mapper.Map(dto.period, cffinstatement);
                })
                ;

            //＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊  CFFINSTATEMENT START ＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

            //◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈  CFSCORE END  ◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈

            CreateMap<Range, CFSCORE>()
                .ForMember(dest => dest.RANGEFROM, opt => opt.MapFrom(src => GetSpecified(src.from, src.fromSpecified)))
                .ForMember(dest => dest.RANGETO, opt => opt.MapFrom(src => GetSpecified(src.to, src.toSpecified)))
                ;

            CreateMap<Range, CFSCORERANGE>()
                .ForMember(dest => dest.RANGEFROM, opt => opt.MapFrom(src => GetSpecified(src.from, src.fromSpecified)))
                .ForMember(dest => dest.RANGETO, opt => opt.MapFrom(src => GetSpecified(src.to, src.toSpecified)))
                ;

            CreateMap<ScoreDecisionRange, CFSCORERANGE>()
                .AfterMap((dto, cfscorerange, resContext) => { resContext.Mapper.Map(dto.scoreRange, cfscorerange); })
                ;

            CreateMap<ScoreAnalysis, CFSCORE>()
                .ForMember(dest => dest.AVERAGESCOREINDUSTRY, opt => opt.MapFrom(src => StringToNullableInt(src.averageScoreIndustry)))
                .ForMember(dest => dest.AVERAGESCOREALL, opt => opt.MapFrom(src => StringToNullableInt(src.averageScoreAll)))
                .AfterMap((dto, cfscore, resContext) =>
                {
                    SaveEntitiesOnChanged<ScoreDecisionRange, CFSCORE, CFSCORERANGE>(cfscore, dto.scoreDecisionRanges, (cfscorerange, sysid) => cfscorerange.SYSCFSCORE = sysid);
                    resContext.Mapper.Map(dto.scoreScaleRange, cfscore);
                })
                ;

            //◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈  CFSCORE START  ◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈◈

            //♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞ CFVERIFICATION END  ♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞

            CreateMap<IdVerificationComparisonElement, CFVERIFICATCOM>()
                .ForMember(dest => dest.PERCENTAGE, opt => opt.MapFrom(src => StringToNullableInt(src.percentage)))
                .ForMember(dest => dest.PROVIDEDVSDOCSIMILARITY, opt => opt.MapFrom(src => StringToNullableInt(src.providedVsDocumentSimilarity)))
                .ForMember(dest => dest.PROVIDEDVSMRZSIMILARITY, opt => opt.MapFrom(src => StringToNullableInt(src.providedVsMrzSimilarity)))
                .ForMember(dest => dest.DOCUMENTVSMRZSIMILARITY, opt => opt.MapFrom(src => StringToNullableInt(src.documentVsMrzSimilarity)))
                ;

            CreateMap<BinaryData, CFDOCUMENTIMG>()
                ;

            CreateMap<IdVerificationDocument, CFDOCUMENT>()
                .ForMember(dest => dest.ISSUINGSTATEORORGA, opt => opt.MapFrom(src => src.issuingStateOrOrganization))
                .ForMember(dest => dest.ISSUINGDATE, opt => opt.MapFrom(src => GetDate(src.issuingDate)))
                .ForMember(dest => dest.EXPIRATIONDATE, opt => opt.MapFrom(src => GetDate(src.expirationDate)))
                .ForMember(dest => dest.VALIDITYFROMDATE, opt => opt.MapFrom(src => GetDate(src.validityFromDate)))
                .ForMember(dest => dest.SIGNDATE, opt => opt.MapFrom(src => GetDate(src.signDate)))
                .ForMember(dest => dest.DOCUMENTTYPE, opt => opt.MapFrom(src => GetSpecified(src.documentType, src.documentTypeSpecified)))
                ;

            CreateMap<IdVerificationPerson, CFPERSON>()
                .ForMember(dest => dest.NATIONALID, opt => opt.MapFrom(src => src.nationalIdentificationNumber))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                ;

            CreateMap<IdVerificationChecks, CFVERIFICATION>()
                .ForMember(dest => dest.ISVALID, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isValid), src.isValidSpecified)))
                .ForMember(dest => dest.ISCOMPLETE, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isComplete), src.isCompleteSpecified)))
                .ForMember(dest => dest.ISCOMPOSITECHECKDIGITVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isCompositeCheckDigitVerified), src.isCompositeCheckDigitVerifiedSpecified)))
                .ForMember(dest => dest.ISBIRTHDATEVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isBirthDateVerified), src.isBirthDateVerifiedSpecified)))
                .ForMember(dest => dest.ISEXPIRATIONDATEVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isExpirationDateVerified), src.isExpirationDateVerifiedSpecified)))
                .ForMember(dest => dest.ISDOCUMENTNUMBERVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isDocumentNumberVerified), src.isDocumentNumberVerifiedSpecified)))
                .ForMember(dest => dest.ISISSUINGSTATEORORGVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isIssuingStateOrOrganizationVerified), src.isIssuingStateOrOrganizationVerifiedSpecified)))
                .ForMember(dest => dest.ISNATIONALIDVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isNationalIdentificationNumberVerified), src.isNationalIdentificationNumberVerifiedSpecified)))
                .ForMember(dest => dest.ISNATIONALITYVERIFIED, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isNationalityVerified), src.isNationalityVerifiedSpecified)))
                ;

            CreateMap<IdVerificationContent, CFVERIFICATION>()
                .ForMember(dest => dest.SYSCFDOCUMENT, opt => opt.ResolveUsing(new EntityStorer<IdVerificationDocument, CFDOCUMENT>(), src => src.document))
                .ForMember(dest => dest.SYSCFPERSON, opt => opt.ResolveUsing(new EntityStorer<IdVerificationPerson, CFPERSON>(), src => src.person))
                .AfterMap((dto, cfverification, resContext) =>
                {
                    SaveEntitiesOnChanged<BinaryData, CFVERIFICATION, CFDOCUMENTIMG>(cfverification, dto.documentImages, (cfdocumentimg, sysid) => cfdocumentimg.SYSCFVERIFICATION = sysid);
                    resContext.Mapper.Map(dto.checks, cfverification);
                })
                ;

            CreateMap<IdVerificationResponseData, CFVERIFICATION>()
                .AfterMap((dto, cfverification, resContext) =>
                {
                    SaveKeyValuesOnChanged(cfverification, dto.warnings, "warnings");
                    SaveKeyValuesOnChanged(cfverification, dto.rejectionReasons, "rejectionReasons");
                    SaveEntitiesOnChanged<IdVerificationComparisonElement, CFVERIFICATION, CFVERIFICATCOM>(cfverification, dto.comparisonElements, (cfverificationcomp, sysid) => cfverificationcomp.SYSCFVERIFICATION = sysid);
                    resContext.Mapper.Map(dto.content, cfverification);
                })
                ;

            //♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞ CFVERIFICATION START  ♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞♞

            //♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨  CFCOMPLIANCE END  ♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨

            CreateMap<ComplianceListDescription, CFCOMPLIANCEDE>()
                .ForMember(dest => dest.DATELASTUPDATED, opt => opt.MapFrom(src => GetDate(src.dateLastUpdated)))
                ;

            CreateMap<ComplianceMatchInformation, CFCOMPLIANCE>()
                .ForMember(dest => dest.CONFIDENCENAME, opt => opt.MapFrom(src => src.confidenceName))
                .ForMember(dest => dest.CONFIDENCEBIRTHDATE, opt => opt.MapFrom(src => src.confidenceBirthdate))
                .ForMember(dest => dest.MATCHEDNAME, opt => opt.MapFrom(src => src.matchedName))
                .ForMember(dest => dest.MATCHEDBIRTHDATE, opt => opt.MapFrom(src => src.matchedBirthdate))
                ;

            CreateMap<ComplianceFoundEntity, CFCOMPLIANCE>()
                .ForMember(dest => dest.SYSCFCOMPLIANCEDE, opt => opt.ResolveUsing(new EntityStorer<ComplianceListDescription, CFCOMPLIANCEDE>(), src => src.listDescription))
                .AfterMap((dto, cfcompliance, resContext) =>
                {
                    SaveKeyValuesOnChanged(cfcompliance, dto.furtherNames, "furtherNames");
                    SaveKeyValuesOnChanged(cfcompliance, dto.birthdates, "birthdates");
                    SaveKeyValuesOnChanged(cfcompliance, dto.titles, "titles");
                    SaveKeyValuesOnChanged(cfcompliance, dto.furtherCountries, "furtherCountries");
                    SaveKeyValuesOnChanged(cfcompliance, dto.birthplaces, "birthplaces");
                    SaveKeyValuesOnChanged(cfcompliance, dto.passportsOrIds, "passportsOrIds");
                    SaveKeyValuesOnChanged(cfcompliance, dto.knownAddresses, "knownAddresses");
                    SaveKeyValuesOnChanged(cfcompliance, dto.additionalInformations, "additionalInformations");

                    resContext.Mapper.Map(dto.matchInformation, cfcompliance);
                })
                ;

            //CreateMap<ComplianceCheckedEntity, CFCOMPLIANCE>(); //already defined below

            CreateMap<ComplianceCheckedEntity, CFCOMPLIANCE>()
                .AfterMap((dto, cfcompliance) => SaveEntitiesOnChanged<ComplianceCheckedEntity, CFCOMPLIANCE, CFCOMPLIANCE>(cfcompliance, dto.checkedEntities, (cfcompliance1, sysid) => cfcompliance1.SYSCFCOMPLIANCECHECKED = sysid))
                .AfterMap((dto, cfcompliance) => SaveEntitiesOnChanged<ComplianceFoundEntity, CFCOMPLIANCE, CFCOMPLIANCE>(cfcompliance, dto.foundEntities, (cfcompliance1, sysid) => cfcompliance1.SYSCFCOMPLIANCEFOUND = sysid))
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.checkedAddress))
                ;

            //♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨  CFCOMPLIANCE START  ♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨♨

            // ☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠  CFBUSINESSLIC END  ☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠

            CreateMap<Period, CFKEYPERIOD>()
                .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.startDate)))
                .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.endDate)))
                .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                ;

            CreateMap<IndustryCode, CFKEYPERIOD>()
                .ForMember(dest => dest.TYPE, opt => opt.MapFrom(src => src.type))
                .ForMember(dest => dest.ISMAININDUSTRYCODE, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.isMainIndustryCode), src.isMainIndustryCodeSpecified)))
                .AfterMap((dto, cfindustry, resContext) => { resContext.Mapper.Map(dto.period, cfindustry); })
                ;

            CreateMap<BusinessIndustryLicense, CFBUSINESSLIC>()
                .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.period.startDate)))
                .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.period.endDate)))
                .ForMember(dest => dest.SYSCFKEYPERIOD, opt => opt.ResolveUsing(new EntityStorer<IndustryCode, CFKEYPERIOD>(), src => src.industryCode))
                ;

            // ☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠  CFBUSINESSLIC START  ☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠☠

            // ☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢  CFCOMPANY END  ☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢


            CreateMap<BankAccount, CFBANKACCOUNT>()
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<CompanyAddressDescription, CFADDRESS>(), src => src.bank))
                ;

            CreateMap<Range, CFCOMPANY>()
                .ForMember(dest => dest.TURNOVERTO, opt => opt.MapFrom(src => GetSpecified(src.to, src.toSpecified)))
                .ForMember(dest => dest.TURNOVERFROM, opt => opt.MapFrom(src => GetSpecified(src.from, src.fromSpecified)))
                .ForMember(dest => dest.EMPLOYEESFROM, opt => opt.MapFrom(src => GetSpecified(src.from, src.fromSpecified)))
                .ForMember(dest => dest.EMPLOYEESTO, opt => opt.MapFrom(src => GetSpecified(src.to, src.toSpecified)))
                ;

            CreateMap<CompanyDetailData, CFCOMPANY>()
                .ForMember(dest => dest.SIZECLASS, opt => opt.MapFrom(src => GetSpecified(src.sizeClass, src.sizeClassSpecified)))
                .ForMember(dest => dest.TURNOVERCURRENCY, opt => opt.MapFrom(src => src.turnoverCurrency))
                .ForMember(dest => dest.TURNOVERINEXPORT, opt => opt.MapFrom((src => GetSpecified(EnumToBool(src.turnoverInExport), src.turnoverInExportSpecified))))
                .ForMember(dest => dest.ACTIVITYINDEX, opt => opt.MapFrom(src => GetSpecified(src.activityIndex, src.activityIndexSpecified)))
                .ForMember(dest => dest.KNOWNSINCE, opt => opt.MapFrom(src => GetDate(src.knownSince)))
                .ForMember(dest => dest.DATEFINANCIALSTATEMENT, opt => opt.MapFrom(src => GetDate(src.dateFinancialStatement)))
                .ForMember(dest => dest.DATEFINANCIALSTATEMENTHANDEDIN, opt => opt.MapFrom(src => GetDate(src.dateFinancialStatementHandedIn)))
                .ForMember(dest => dest.SYSCFCOMPANYMOTHER, opt => opt.ResolveUsing(new EntityStorer<CompanyBaseData, CFCOMPANY>(), src => src.ultimateMotherCompany))
                .AfterMap((dto, cfcompany, resContext) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<IndustryCode, CFCOMPANY, CFKEYPERIOD>(cfcompany, dto.industryCodes, (cfkeyperiod, sysid) => cfkeyperiod.SYSCFCOMPANY = sysid);
                    SaveEntitiesOnChanged<BankAccount, CFCOMPANY, CFBANKACCOUNT>(cfcompany, dto.bankAccounts, (cfbankaccount, sysid) => cfbankaccount.SYSCFCOMPANY = sysid);

                    if (dto.turnoverRange != null)
                    {
                        cfcompany.TURNOVERFROM = GetSpecified((decimal)dto.turnoverRange.from, dto.turnoverRange.fromSpecified);
                        cfcompany.TURNOVERTO = GetSpecified((decimal)dto.turnoverRange.to, dto.turnoverRange.toSpecified);
                    }

                    if (dto.nrOfEmployees != null)
                    {
                        cfcompany.EMPLOYEESFROM = resContext.Mapper.Map<decimal?, long?>(GetSpecified(dto.nrOfEmployees.from, dto.nrOfEmployees.fromSpecified));
                        cfcompany.EMPLOYEESTO = resContext.Mapper.Map<decimal?, long?>(GetSpecified(dto.nrOfEmployees.to, dto.nrOfEmployees.toSpecified));
                    }
                })
                ;

            CreateMap<Amount, CFCOMPANY>()
                .ForMember(dest => dest.CAPITALAMOUNT, opt => opt.MapFrom(src => src.amount))
                .ForMember(dest => dest.CAPITALCURRENCY, opt => opt.MapFrom(src => src.currency))
                ;

            CreateMap<CompanyRegistrationData, CFCOMPANY>()
                .ForMember(dest => dest.REGISTEREDOFFICECITY, opt => opt.MapFrom(src => src.registeredOfficeCity))
                .ForMember(dest => dest.FOUNDINGDATE, opt => opt.MapFrom(src => GetDate(src.foundingDate)))
                .ForMember(dest => dest.CAPITALINKIND, opt => opt.MapFrom(src => GetSpecified(EnumToBool(src.capitalInKind), src.capitalInKindSpecified)))
                .ForMember(dest => dest.HASAUDITINGCOMPANY, opt => opt.MapFrom(src => GetSpecified(src.hasAuditingCompany, src.hasAuditingCompanySpecified)))
                .ForMember(dest => dest.PURPOSE, opt => opt.MapFrom(src => src.purpose))
                .ForMember(dest => dest.SYSCFCOMPANYAUDITING, opt => opt.ResolveUsing(new EntityStorer<CompanyBaseData, CFCOMPANY>(), src => src.auditingCompany))
                .AfterMap((dto, cfcompany, resContext) =>
                {
                    if (dto == null)
                        return;

                    resContext.Mapper.Map(dto.capital, cfcompany);
                    if (dto.capitalPayed != null)
                    {
                        cfcompany.CAPITALPAYEDAMOUNT = (decimal)dto.capitalPayed.amount;
                        cfcompany.CAPITALPAYEDCURRENCY = dto.capitalPayed.currency;
                    }
                })
                ;

            CreateMap<CompanyHistoryItemActivityStatus, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                ;

            CreateMap<CompanyHistoryItemAddress, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<AddressDescription, CFADDRESS>(), src => src.address))
                ;

            CreateMap<Amount, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.AMOUNT, opt => opt.MapFrom(src => src.amount))
                  .ForMember(dest => dest.AMOUNTCUR, opt => opt.MapFrom(src => src.currency))
                ;

            CreateMap<CompanyHistoryItemAmount, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.AMOUNT, opt => opt.Ignore())
                  .AfterMap((dto, cfcomphist, resContext) => { resContext.Mapper.Map(dto.amount, cfcomphist); });

            CreateMap<CompanyHistoryItemLegalForm, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.LEGALFORMTYPEORIG, opt => opt.MapFrom(src => src.legalFormTypeOriginal))
                ;

            CreateMap<CompanyHistoryItemLocation, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.SYSCFADDRESSLOCATION, opt => opt.ResolveUsing(new EntityStorer<Location, CFADDRESS>(), src => src.location))
                ;

            CreateMap<CompanyHistoryItemText, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                ;

            CreateMap<Period, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.Ignore())
                  .ForMember(dest => dest.PERIODSTART, opt => opt.MapFrom(src => GetDate(src.startDate)))
                  .ForMember(dest => dest.PERIODEND, opt => opt.MapFrom(src => GetDate(src.endDate)));

            CreateMap<CompanyHistoryItem, CFCOMPHIST>()
                  .ForMember(dest => dest.TYPE, opt => opt.MapFrom(src => src.type))
                  .ConstructUsing((histItem, resContext) =>
                  {
                      var result = new CFCOMPHIST();
                      resContext.Mapper.Map(histItem, result, histItem.GetType(), typeof(CFCOMPHIST));
                      return result;
                  })
                  .AfterMap((dto, cfcomphist, resContext) => { resContext.Mapper.Map(dto.period, cfcomphist); });

            CreateMap<CompanyBaseData, CFCOMPANY>()
                .ForMember(dest => dest.SYSCFADDRESS, opt => opt.ResolveUsing(new EntityStorer<CompanyAddressDescription, CFADDRESS>(), src => src.mainAddress))
                .ForMember(dest => dest.LEGALFORMTEXT, opt => opt.MapFrom(src => Truncate(src.legalFormText, 10)))
                .AfterMap((dto, cfcompany, resContext) =>
                {
                    if (dto == null)
                        return;

                    SaveKeyValuesOnChanged(cfcompany, dto.identifiers);

                    SaveEntitiesOnChanged<CompanyHistoryItem, CFCOMPANY, CFCOMPHIST>(cfcompany, dto.companyHistoryItems, (cfcomphist, sysid) => cfcomphist.SYSCFCOMPANY = sysid);

                    resContext.Mapper.Map(dto.companyDetailData, cfcompany);
                    resContext.Mapper.Map(dto.companyRegistrationData, cfcompany);
                })
                ;

            // ☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢  CFCOMPANY START  ☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢☢

            //☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯  CFOUTGETREPORT END  ☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯
            CreateMap<PublicationList, CFOUTGETREPORT>()
                .ForMember(dest => dest.PUBLICATIONSTRUNCATED, opt => opt.MapFrom(src => src.isTruncated))
                .AfterMap((dto, cfoutgetrep) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<Publication, CFOUTGETREPORT, CFPUBLICATION>(cfoutgetrep, dto.publications, (cfrep, sysid) => cfrep.SYSCFOUTGETREPORT = sysid);
                }) //I am not sure if correct!
                ;

            CreateMap<PaymentDelay, CFOUTGETREPORT>()
                .ForMember(dest => dest.INTIMERATIO, opt => opt.MapFrom(src => GetSpecified(src.inTimeRatio, src.inTimeRatioSpecified)))
                .ForMember(dest => dest.PAYMENTEXPECTEDTYPE, opt => opt.MapFrom(src => GetSpecified(src.paymentExpectedType, src.paymentExpectedTypeSpecified)))
                .ForMember(dest => dest.AVGDELAYSHORTTERM, opt => opt.MapFrom(src => StringToNullableInt(src.avgDelayShortTerm)))
                .ForMember(dest => dest.AVGDELAYLONGTERM, opt => opt.MapFrom(src => StringToNullableInt(src.avgDelayLongTerm)))
                .ForMember(dest => dest.PAYMENTTYPE, opt => opt.MapFrom(src => src.paymentType))
                ;

            CreateMap<BranchOfficeList, CFOUTGETREPORT>()
                .ForMember(dest => dest.BRANCHOFFICESTRUNCATED, opt => opt.MapFrom(src => src.isTruncated))
                .AfterMap((dto, cfoutgetreport) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<BranchOfficeListItem, CFOUTGETREPORT, CFBRANCHOFFICE>(cfoutgetreport, dto.branchOffices, (cfbranchoffice, sysid) => cfbranchoffice.SYSCFOUTGETREPORT = sysid);
                })
                ;

            CreateMap<FurtherRelations, CFOUTGETREPORT>()
                .AfterMap((dto, cfoutgetreport) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<CompanyBaseData, CFOUTGETREPORT, CFCOMPANY>(cfoutgetreport, dto.obviousRelations, (cfcompany, sysid) => cfcompany.SYSCFOUTGETREPORTOBVIOUS = sysid);
                    SaveEntitiesOnChanged<CompanyBaseData, CFOUTGETREPORT, CFCOMPANY>(cfoutgetreport, dto.probableRelations, (cfcompany, sysid) => cfcompany.SYSCFOUTGETREPORTPROBABLE = sysid);
                    SaveEntitiesOnChanged<CompanyBaseData, CFOUTGETREPORT, CFCOMPANY>(cfoutgetreport, dto.samePhoneNumber, (cfcompany, sysid) => cfcompany.SYSCFOUTGETREPORTSAMETEL = sysid);
                })
                ;

            CreateMap<WhoOwnsWhom, CFOUTGETREPORT>()
                .AfterMap((dto, cfoutgetrep) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<WowRelation, CFOUTGETREPORT, CFWOWRELATION>(cfoutgetrep, dto.wowRelations, (cfrep, sysid) => cfrep.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<WowAddress, CFOUTGETREPORT, CFWOWADDRESS>(cfoutgetrep, dto.wowAddresses, (cfrep, sysid) => cfrep.SYSCFOUTGETREPORT = sysid);
                })
                ;

            CreateMap<ComplianceCheckResult, CFOUTGETREPORT>()
                .AfterMap((dto, cfoutgetreport) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<ComplianceListDescription, CFOUTGETREPORT, CFCOMPLIANCEDE>(cfoutgetreport, dto.listDescriptions, (cfcompliancedescription, sysid) => cfcompliancedescription.SYSCFOUTGETREPORT = sysid);
                })
                .ForMember(dest => dest.SYSCFCOMPLIANCE, opt => opt.ResolveUsing(new EntityStorer<ComplianceCheckedEntity, CFCOMPLIANCE>(), src => src.checkedEntity))
                ;

            CreateMap<OrganizationPositionList, CFOUTGETREPORT>()
                .ForMember(dest => dest.POSITIONSTRUNCATED, opt => opt.MapFrom(src => src.isTruncated))
                .AfterMap((dto, cfoutgetreport) =>
                {
                    if (dto == null)
                        return;

                    SaveEntitiesOnChanged<OrganizationPosition, CFOUTGETREPORT, CFORGAPOSITION>(cfoutgetreport, dto.organizationPositions, (cforganizationposition, sysid) => cforganizationposition.SYSCFOUTGETREPORT = sysid);
                })
                ;

            CreateMap<ControlPerson, CFADDRESS>()
                .ConstructUsing((controlPerson, resContext) =>
                {
                    var result = new CFADDRESS();
                    resContext.Mapper.Map(controlPerson.controlPersonAddress, result, controlPerson.controlPersonAddress.GetType(), result.GetType());
                    return result;
                })
                .AfterMap((dto, cfaddress, resContext) =>
                {
                    if (dto == null)
                        return;
                    
                    SaveKeyValuesOnChanged(cfaddress,
                        new[]
                        {
                            new CFKEYVALUE() { VALUE = dto.controlPersonType.ToString(), KEYVAL = "controlPersonType"}
                        },
                        "controlPerson");
                });

            CreateMap<ReportDetails, CFOUTGETREPORT>()
                .ForMember(dest => dest.SYSCFSCHUFA, opt => opt.ResolveUsing(new EntityStorer<SchufaResponseData, CFSCHUFA>(), src => src.schufaResponseData))
                .ForMember(dest => dest.SYSCFSCORE, opt => opt.ResolveUsing(new EntityStorer<ScoreAnalysis, CFSCORE>(), src => src.scoreAnalysis))
                .ForMember(dest => dest.SYSCFVERIFICATION, opt => opt.ResolveUsing(new EntityStorer<IdVerificationResponseData, CFVERIFICATION>(), src => src.idVerificationResponseData))
                .ForMember(dest => dest.SYSCFCOMPANY, opt => opt.ResolveUsing(new EntityStorer<CompanyBaseData, CFCOMPANY>(), src => src.companyBaseData))
                .AfterMap((dto, cfoutgetreport, resContext) =>
                {
                    if (dto == null)
                    {
                        return;
                    }

                    SaveEntitiesOnChanged<AddressWithDeliverability, CFOUTGETREPORT, CFADDRESSHIST>(cfoutgetreport, dto.addressHistory, (cfrep, sysid) => cfrep.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<DebtEntry, CFOUTGETREPORT, CFDEBT>(cfoutgetreport, dto.debts, (cfdebt, sysid) => cfdebt.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<FinancialStatement, CFOUTGETREPORT, CFFINSTATEMENT>(cfoutgetreport, dto.financialStatements, (cffinstatement, sysid) => cffinstatement.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<FinancialStatement, CFOUTGETREPORT, CFFINSTATEMENT>(cfoutgetreport, dto.financialStatementsGroup, (cffinstatement, sysid) => cffinstatement.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<BusinessIndustryLicense, CFOUTGETREPORT, CFBUSINESSLIC>(cfoutgetreport, dto.businessIndustryLicenses, (cfbusinesslic, sysid) => cfbusinesslic.SYSCFOUTGETREPORT = sysid);

                    SaveEntitiesOnChanged<ControlPerson, CFOUTGETREPORT, CFADDRESS>(cfoutgetreport, dto.controlPersonsExt, (cfaddress, sysid) => cfaddress.SYSCFOUTGETREPORT = sysid);
                    SaveEntitiesOnChanged<AddressDescription, CFOUTGETREPORT, CFADDRESS>(cfoutgetreport, dto.controlPersons, (cfaddress, sysid) => cfaddress.SYSCFOUTGETREPORT = sysid);

                    resContext.Mapper.Map(dto.publicationList, cfoutgetreport);
                    resContext.Mapper.Map(dto.paymentDelay, cfoutgetreport);
                    resContext.Mapper.Map(dto.branchOfficeList, cfoutgetreport);
                    resContext.Mapper.Map(dto.furtherRelations, cfoutgetreport);
                    resContext.Mapper.Map(dto.whoOwnsWhom, cfoutgetreport);
                    resContext.Mapper.Map(dto.complianceCheckResult, cfoutgetreport);
                    resContext.Mapper.Map(dto.organizationPositionList, cfoutgetreport);
                })
                ;

            CreateMap<CrifGetReportOutDto, CFOUTGETREPORT>()
                .ForMember(dest => dest.ARCHIVINGID, opt => opt.MapFrom(src => GetSpecified(src.archivingId, src.archivingIdSpecified)))
                .ForMember(dest => dest.SYSCFDECISION, opt => opt.ResolveUsing(new EntityStorer<DecisionMatrix, CFDECISION>(), src => src.decisionMatrix))
                .ForMember(dest => dest.SYSCFADDRESSMATCH, opt => opt.ResolveUsing(new EntityStorer<AddressMatchResult, CFADDRESSMATCH>(), src => src.addressMatchResult))
                .AfterMap((dto, cfoutgetreport, resContext) =>
                {
                    if (dto == null)
                        return;

                    SaveKeyValuesOnChanged(cfoutgetreport, dto.additionalOutput);
                    resContext.Mapper.Map(dto.reportDetails, cfoutgetreport);
                })
                ;

            //☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯  CFOUTGETREPORT START  ☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯
            CreateMap <RISK, EQUIFAXRISKOUT > ();
            CreateMap<ARAAttribute, EQUIFAXRISKOUT>();
                


        }

        private string Truncate(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Substring(0, Math.Min(text.Length, length));
        }

        /// <summary>
        /// converts a string to a int, or returns null
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int? StringToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i))
            {
                return i;
            }
            return null;
        }

        /// <summary>
        /// Converts a string to int if possible, else returns null
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int? TryParseToInt(string s)
        {
            int i;
            if (!int.TryParse(s, out i))
            {
                return null;
            }
            else
            {
                return i;
            }
        }

        /// <summary>
        /// Converts a string to long if possible, else returns null
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private long? TryParseToLong(string s)
        {
            long i;
            if (!long.TryParse(s, out i))
            {
                return null;
            }

            return i;
        }

        /// <summary>
        /// Turns the NullableBoolean enums to Bools
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        private bool EnumToBool(NullableBoolean term)
        {
            if (term == NullableBoolean.@true)
            {
                return true;
            }
            else
                return false;
        }

        private DateTime? GetDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            DateTime result;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            if (DateTime.TryParseExact(dateString, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        private void SaveEntitiesOnChanged<TElement, TEntity, TChildEntity>(TEntity parent, TElement[] elements, Action<TChildEntity, long?> setForeignKey)
            //where TEntity : EntityObject
        {
            if (elements == null || !elements.Any())
            {
                return;
            }

            OnEntitySaved(parent, sysid =>
            {
                using (CRIFExtended context = new CRIFExtended())
                {
                    var childEntities = Mapper.Map(elements, new List<TChildEntity>());

                    foreach (var child in childEntities ?? new List<TChildEntity>())
                    {
                        setForeignKey(child, sysid);
                        context.getObjectContext().AddObject(typeof(TChildEntity).Name, child);
                    }
                    context.SaveChanges();
                    foreach (var child in childEntities)
                    {
                        context.getObjectContext().Detach(child);
                        ReportPropertyChanged(child, "SYS" + typeof(TChildEntity).Name);
                    }
                }
            });
        }

        private void OnEntitySaved<TEntity>(TEntity entity, Action<long?> savedAction)
            //where TEntity : EntityObject
        {
            var keyName = "SYS" + typeof(TEntity).Name;
            var property = typeof(TEntity).GetProperty(keyName);
            if (property == null) return;
            var keyValue = property.GetValue(entity) as long?;

            if (keyValue != 0)
            {
                using (CRIFExtended context = new CRIFExtended())
                {
                    ObjectStateEntry entry = context.getObjectContext().ObjectStateManager.GetObjectStateEntry(entity);
                    if (entry.State == System.Data.Entity.EntityState.Detached)
                    {
                        savedAction(keyValue);
                    }
                }
            }
            else
            {
                RegisterPropertyChanged(entity, keyName, savedAction);
            }
        }

        private void SaveKeyValuesOnChanged<TEntity, TKeyValue>(TEntity entity, TKeyValue[] keyValues, string context = null)
            //where TEntity : EntityObject
        {
            if (keyValues == null || keyValues.Length == 0)
                return;

            OnEntitySaved(entity,
                key =>
                {
                    using (CRIFExtended dbContext = new CRIFExtended())
                    {
                        var area = typeof(TEntity).Name;
                        var cfkeyValues = Mapper.Map(keyValues, new List<CFKEYVALUE>());
                        foreach (var keyValuePair in cfkeyValues)
                        {
                            keyValuePair.AREA = area;
                            keyValuePair.SYSID = key;
                            if (!string.IsNullOrEmpty(context))
                            {
                                keyValuePair.CONTEXT = context;
                            }
                            dbContext.CFKEYVALUE.Add(keyValuePair);
                        }
                        dbContext.SaveChanges();
                    }
                });
        }

        private T? GetSpecified<T>(T value, bool specified)
            where T : struct
        {
            if (!specified)
            {
                return null;
            }

            return value;
        }

        private T ParseEnum<T>(string value) where T : struct
        {
            T enumValue;
            if (Enum.TryParse(value, out enumValue))
            {
                return enumValue;
            }
            return default(T);
        }

        private bool IsSpecified<T>(string value) where T : struct
        {
            T enumValue;
            return !string.IsNullOrEmpty(value) && Enum.TryParse(value, out enumValue);
        }
    }
}