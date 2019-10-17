using System.Globalization;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    using AutoMapper;
    using Cic.OpenOne.Common.Model.DdIc;
    using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;
    using CIC.Database.IC.EF6.Model;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Linq;
    using Type;
    using Cic.OpenOne.Common.Util.Extension;
    public class AuskunftModelSchufaProfile : AuskunftModelProfileBase
    {
        protected class CustomBoolResolver : AutoMapper.IMemberValueResolver<object , object ,bool?, string>
        {
            public string Resolve(object a, object b, bool? source, string tg, ResolutionContext c)
            {
                if (source == null)
                    return null;
                return (source.Value ? "1" : "0");
            }
        }

        protected class CustomToBoolResolver : IMemberValueResolver<object, object,String, bool>
        {

            public bool Resolve(object a, object b, String source, bool tg, ResolutionContext c)
            {
                if (source == null)
                    return false;
                return (source == "1" ? true : false);
            }
        }
        
        public class CustomToDateTimeResolver : IMemberValueResolver<object, object, string,DateTime?>
        {
            
                public DateTime? Resolve(object a, object b, string source, DateTime? tg, ResolutionContext c)
            {
                if (string.IsNullOrEmpty(source))
                    return null;

                DateTime date;
                if (DateTime.TryParse(source, out date))
                    return date;
                else
                    return null;
            }
        }

        public class CustomToEnumResolver<T> : IMemberValueResolver<object, object, string, T?> where T : struct
        {
            
                public T? Resolve(object a, object b, string source, T? tg, ResolutionContext c)
            {
                if (String.IsNullOrEmpty(source) || !typeof(T).IsEnum)
                    return null;
                try
                {
                    return (T)System.Enum.Parse(typeof(T), source, true);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public class EntityResolverFromKey<TTargetMember, TEntity> : IMemberValueResolver<object, object, EntityReference, TTargetMember> 
        {
                public TTargetMember Resolve(object a, object b, EntityReference source, TTargetMember tg, ResolutionContext c)
            {
                if (source == null || source.EntityKey == null || source.EntityKey.EntityKeyValues == null ||
                    source.EntityKey.EntityKeyValues.Length == 0 || source.EntityKey.EntityKeyValues.FirstOrDefault() == null)
                {
                    return default(TTargetMember);
                }
                var element = source.EntityKey.EntityKeyValues.FirstOrDefault().Value;
                if (element is long && (long)element == 0)
                {
                    return default(TTargetMember);
                }


                using (DdIcExtended context = new DdIcExtended())
                {
                    try
                    {
                        TEntity temp = (TEntity)context.getObjectContext().GetObjectByKey(source.EntityKey);
                        return c.Mapper.Map<TEntity, TTargetMember>(temp);
                    }
                    catch (Exception)
                    {
                        return default(TTargetMember);
                    }
                }
            }
        }

        public class EntityResolver<TTargetMember, TEntity> : IMemberValueResolver<object, object, long?, TTargetMember> 
        {
            
                public TTargetMember Resolve(object a, object b, long? source, TTargetMember tg, ResolutionContext c)
            {
                if (!source.HasValue || source.Value == 0)
                    return default(TTargetMember);

                using (DdIcExtended context = new DdIcExtended())
                {
                    try
                    {
                        
                        TEntity temp = (TEntity)context.getObjectContext().GetObjectByKey(context.getEntityKey(typeof(TEntity), source.Value));
                        return c.Mapper.Map<TEntity, TTargetMember>(temp);
                    }
                    catch (Exception)
                    {
                        return default(TTargetMember);
                    }
                }
            }
        }

        public class AdressResolver : EntityResolver<SchufaTAdresse, SFADRESSE> { }
        public class PersonResolver : EntityResolver<SchufaTVerbraucherdaten, SFPERSON> { }

        public class AdressResolverFromKey : EntityResolverFromKey<SchufaTAdresse, SFADRESSE> { }
        public class PersonResolverFromKey : EntityResolverFromKey<SchufaTVerbraucherdaten, SFPERSON> { }
        
        public class SachbearbeiterResolverFromKey : EntityResolverFromKey<SchufaTSachbearbeiter, SFSBEARBEITER> { }
        public class SachbearbeiterResolver : EntityResolver<SchufaTSachbearbeiter, SFSBEARBEITER> { }
        public class MerkmalResolverFromKey : EntityResolverFromKey<SchufaTMerkmal, SFMERKMAL> { }
        public class MerkmalResolver : EntityResolver<SchufaTMerkmal, SFMERKMAL> { }
        public class AktionsHeaderResolverFromKey : EntityResolverFromKey<SchufaTAktionsHeader, SFAKTIONHEADER> { }
        public class AktionsHeaderResolver : EntityResolver<SchufaTAktionsHeader, SFAKTIONHEADER> { }
        public class ProductInfoResolverFromKey : EntityResolverFromKey<SchufaTProduktinformationen, SFPRODUKTINFO> { }
        public class ProductInfoResolver : EntityResolver<SchufaTProduktinformationen, SFPRODUKTINFO> { }


        private class EntityStorer<TElement, TEntity> : IMemberValueResolver<object, object, TElement, long?>
           // where TEntity : EntityObject
        {

            public long? Resolve(object source, object destomatopm, TElement sourceMember, long? destMember, ResolutionContext resContext)
            {

                if (sourceMember == null)
                    return null;

                using (DdIcExtended context = new DdIcExtended())
                {
                    try
                    {
                        var entity = resContext.Mapper.Map(sourceMember, sourceMember.GetType(), typeof(TEntity));
                        var entityName = typeof(TEntity).Name;
                        
                        context.getObjectContext().AddObject(entityName, entity);
                        context.SaveChanges();

                        var key = context.getObjectContext().CreateEntityKey(entityName, entity);
                        var firstKey = key.EntityKeyValues.FirstOrDefault();

                        if (firstKey != null && firstKey.Value is long)
                        {
                            return (long) firstKey.Value;
                        }
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }
    
        public AuskunftModelSchufaProfile()
        {
            

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //SchufaT zur Datenbank
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            
            //Common Objects
            CreateMap<SchufaTMerkmal, SFMERKMAL>()
                 .ForMember(dest => dest.DATUM, opt => opt.MapFrom(src => MapDatumFromSchufaToDb(src.Datum)));

            CreateMap<SchufaTAktionsHeader, SFAKTIONHEADER>()
                 .ForMember(dest => dest.TEILNEHMERREFERENZ, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                 .ForMember(dest => dest.TIMESTAMP, opt => opt.MapFrom(src => src.timestamp))
                 .ForMember(dest => dest.RUECKGABEAKTIONSDATEN, opt => opt.MapFrom(src => src.RueckgabeAktionsdaten))
                 ;

            CreateMap<SchufaTAdresse, SFADRESSE>();
            CreateMap<SchufaTVerbraucherdaten, SFPERSON>()
                 .ForMember(dest => dest.SFADRESSE, opt => opt.MapFrom(src => src.AktuelleAdresse))
                 .ForMember(dest => dest.SYSSFADRESSEVOR,
                 opt => opt.ResolveUsing<SchufaTAdresse>(new EntityStorer<SchufaTAdresse,  SFADRESSE>(), src => src.Voradresse))
                 
                 .ForMember(dest => dest.GEBURTSDATUM, opt => opt.MapFrom(src => MapDatumFromSchufaToDb(src.Geburtsdatum)))
                 ;

            CreateMap<SchufaTSachbearbeiter, SFSBEARBEITER>();

            CreateMap<SchufaTPerson, SFPERSON>()
                 .ForMember(dest => dest.SFADRESSE, opt => opt.MapFrom(src => src.AktuelleAdresse))
                 .ForMember(dest => dest.SYSSFADRESSEVOR,
                 opt => opt.ResolveUsing<SchufaTAdresse>(new EntityStorer<SchufaTAdresse, SFADRESSE>(), src => src.Voradresse))
                 
                 .ForMember(dest => dest.GEBURTSDATUM, opt => opt.MapFrom(src => src.GeburtsdatumIso))
                 .ForMember(dest => dest.TITEL, opt => opt.MapFrom(src => src.Namenszusatz))
                 ;

            CreateMap<SchufaTProduktinformationen, SFPRODUKTINFO>()
                .ForMember(dest => dest.ABRUFVERFAHREN, opt => opt.MapFrom(src => src.Abrufverfahren))
                .ForMember(dest => dest.AUFTRAGSART, opt => opt.MapFrom(src => src.Auftragsart));
            
            //Daten Objekte
            CreateMap<SchufaTMeldung, SFINPMELDUNG>()
                 .ForMember(dest => dest.SFAKTIONHEADER, opt => opt.MapFrom(src => src.AktionsHeader))
                 .ForMember(dest => dest.SFPERSON, opt => opt.MapFrom(src => src.Verbraucherdaten))
                 .ForMember(dest => dest.SFSBEARBEITER, opt => opt.MapFrom(src => src.Ansprechpartner))
                 .ForMember(dest => dest.SFADRESSE, opt => opt.MapFrom(src => src.NeueAdresse))
                 .ForMember(dest => dest.SYSSFPERSONKOR, opt => opt.ResolveUsing<SchufaTVerbraucherdaten>(new EntityStorer<SchufaTVerbraucherdaten, SFPERSON>(), src => src.KorrigierteVerbraucherdaten))
                 
                 .ForMember(dest => dest.SFMERKMAL, opt => opt.MapFrom(src => src.Meldemerkmal))
                ;

            CreateMap<SchufaTAbrufNachmeldungPI, SFINPNACHMELDG>()
                .ForMember(dest => dest.SFAKTIONHEADER, opt => opt.MapFrom(src => src.AktionsHeader))
                .ForMember(dest => dest.SFPRODUKTINFO, opt => opt.MapFrom(src => src.Produktinformationen))
                ;

            CreateMap<SchufaTAnfrageBonitaetsauskunft, SFINPBONIAUSK>()
                .ForMember(dest => dest.SFAKTIONHEADER, opt => opt.MapFrom(src => src.AktionsHeader))
                .ForMember(dest => dest.ABRUFREFERENZ, opt => opt.MapFrom(src => src.Abrufreferenz))
                .ForMember(dest => dest.SFMERKMAL, opt => opt.MapFrom(src => src.Anfragemerkmal))
                .ForMember(dest => dest.SFPERSON, opt => opt.MapFrom(src => src.Verbraucherdaten))
                ;

            CreateMap<SchufaTAbrufManuelleWeiterverarbeitung, SFINPBONIAUSK>()
                .ForMember(dest => dest.ABRUFREFERENZ, opt => opt.MapFrom(src => src.Abrufreferenz))
                .ForMember(dest => dest.SFAKTIONHEADER, opt => opt.MapFrom(src => src.AktionsHeader));
            
            // ---------------------------------------
            // ---------------------------------------
            // DB to SchufaT
            // ---------------------------------------
            // --------------------------------------


            CreateMap<SFAKTIONHEADER, SchufaTAktionsHeader>()
                .ForMember(dest => dest.RueckgabeAktionsdaten, opt => opt.MapFrom(src => src.RUECKGABEAKTIONSDATEN))
                .ForMember(dest => dest.Teilnehmerreferenz, opt => opt.MapFrom(src => src.TEILNEHMERREFERENZ))
                .ForMember(dest => dest.timestamp, opt => opt.MapFrom(src => src.TIMESTAMP));

            CreateMap<SFADRESSE, SchufaTAdresse>();
            CreateMap<SFSBEARBEITER, SchufaTSachbearbeiter>();

            CreateMap<SFPERSON, SchufaTVerbraucherdaten>()
                .ForMember(dest => dest.Voradresse,
                opt => opt.ResolveUsing<AdressResolver,long?>(src => src.SYSSFADRESSEVOR)) //soll Voradresse sein
                .ForMember(dest => dest.AktuelleAdresse,
                opt => opt.ResolveUsing<AdressResolver, long?>(src => src.SYSSFADRESSE))
                
                .ForMember(dest => dest.Geschlecht, opt => opt.MapFrom(src => src.GESCHLECHT))
                .ForMember(dest => dest.SchufaKlauselDatum, opt => opt.MapFrom(src => src.SCHUFAKLAUSELDATUM))
                .ForMember(dest => dest.Geburtsdatum, opt => opt.MapFrom(src => MapDatumFromDbToSchufa(src.GEBURTSDATUM)));


            CreateMap<SFMERKMAL, SchufaTMerkmal>()
                .ForMember(dest => dest.Waehrung, opt => opt.MapFrom(src => src.WAEHRUNG))
                .ForMember(dest => dest.Ratenart, opt => opt.MapFrom(src => src.RATENART))
                .ForMember(dest => dest.Datum, opt => opt.MapFrom(src => MapDatumFromDbToSchufa(src.DATUM)))
            ;

            CreateMap<SFPERSON, SchufaTPerson>()
                .ForMember(dest => dest.Voradresse,
                opt => opt.ResolveUsing<AdressResolver, long?>(src => src.SYSSFADRESSEVOR)) //soll Voradresse sein
                
                .ForMember(dest => dest.AktuelleAdresse,
                opt => opt.ResolveUsing<AdressResolver, long?>(src => src.SYSSFADRESSE))
                
                .ForMember(dest => dest.Geschlecht, opt => opt.MapFrom(src => src.GESCHLECHT))
                .ForMember(dest => dest.Namenszusatz, opt => opt.MapFrom(src => src.TITEL))
                .ForMember(dest => dest.GeburtsdatumIso, opt => opt.MapFrom(src => src.GEBURTSDATUM))
                ;

            CreateMap<SFPRODUKTINFO, SchufaTProduktinformationen>()
                .ForMember(dest => dest.Abrufverfahren, opt => opt.MapFrom(src => src.ABRUFVERFAHREN))
                .ForMember(dest => dest.Auftragsart, opt => opt.MapFrom(src => src.AUFTRAGSART));
            
            
            //Meldung
            CreateMap<SFINPMELDUNG, SchufaMeldungInDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                ;

            CreateMap<SFINPMELDUNG, SchufaTMeldung>()
                .ForMember(dest => dest.AktionsHeader,
                opt => opt.ResolveUsing<AktionsHeaderResolver, long?>(src => src.SYSSFAKTIONHEADER))
                .ForMember(dest => dest.Verbraucherdaten,
                opt => opt.ResolveUsing<PersonResolver, long?>(src => src.SYSSFPERSON))                
                .ForMember(dest => dest.Ansprechpartner,
                opt => opt.ResolveUsing<SachbearbeiterResolver, long?>(src => src.SYSSFSBEARBEITER))
                .ForMember(dest => dest.KorrigierteVerbraucherdaten,
                 opt => opt.ResolveUsing<PersonResolver, long?>(src => src.SYSSFPERSONKOR))                 
                .ForMember(dest => dest.NeueAdresse, opt => opt.ResolveUsing<AdressResolver, long?>(src => src.SYSSFADRESSE))                
                .ForMember(dest => dest.Sterbedaten, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Meldemerkmal,
                opt => opt.ResolveUsing<MerkmalResolver, long?>(src => src.SYSSFMERKMAL))                
                .ForMember(dest => dest.Meldeart, opt => opt.MapFrom(src => src.MELDEART))
                .ForMember(dest => dest.Zusatzinformation, opt => opt.MapFrom(src => src.ZUSATZINFO))
                ;

            CreateMap<SFINPMELDUNG, SchufaTSterbedaten>()
                .ForMember(dest => dest.Sterbedatum, opt => opt.MapFrom(src => src.STERBEDATUM))
                .ForMember(dest => dest.Sterbeurkundennummer, opt => opt.MapFrom(src => src.STERBEURKUNDENNUMMER))
                .ForMember(dest => dest.Vertragsreferenz, opt => opt.MapFrom(src => src.VERTRAGSREFERENZ))
                ;

            //Abruf Nachmeldung

            CreateMap<SFINPNACHMELDG, SchufaAbrufNachmeldungInDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                ;

            CreateMap<SFINPNACHMELDG, SchufaTAbrufNachmeldungPI>()
                .ForMember(dest => dest.AktionsHeader,
                opt => opt.ResolveUsing<AktionsHeaderResolver, long?>(src => src.SYSSFAKTIONHEADER))                
                .ForMember(dest => dest.Produktinformationen,
                opt => opt.ResolveUsing<ProductInfoResolver, long?>(src => src.SYSSFPRODUKTINFO))
                
                ;

            //AnfrageBonitätsauskunft

            CreateMap<SFINPBONIAUSK, SchufaAnfrageBonitaetsauskunftInDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));

            CreateMap<SFINPBONIAUSK, SchufaTAnfrageBonitaetsauskunft>()
                .ForMember(dest => dest.Abrufreferenz, opt => opt.MapFrom(src => src.ABRUFREFERENZ))
                .ForMember(dest => dest.AktionsHeader,
                opt => opt.ResolveUsing<AktionsHeaderResolver, long?>(src => src.SYSSFAKTIONHEADER))                
                .ForMember(dest => dest.Anfragemerkmal,
                opt => opt.ResolveUsing<MerkmalResolver, long?>(src => src.SYSSFMERKMAL))
                .ForMember(dest => dest.Verbraucherdaten,
                opt => opt.ResolveUsing<PersonResolver, long?>(src => src.SYSSFPERSON))
                
                ;

            // Abruf Manuelle Weiterverarbeitung
            CreateMap<SFINPBONIAUSK, SchufaAbrufManuelleWeiterverarbeitungInDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));

            CreateMap<SFINPBONIAUSK, SchufaTAbrufManuelleWeiterverarbeitung>()
                .ForMember(dest => dest.Abrufreferenz, opt => opt.MapFrom(src => src.ABRUFREFERENZ))
                .ForMember(dest => dest.AktionsHeader,
                opt => opt.ResolveUsing<AktionsHeaderResolver, long?>(src => src.SYSSFAKTIONHEADER))
                ;

            // ---------------------------------------
            // ---------------------------------------
            // SchufaT to Request
            // ---------------------------------------
            // ---------------------------------------

            CreateMap<SchufaTAktionsHeader, AktionType>()
                .ForMember(dest => dest.teilnehmerreferenz, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                ;

            //Common 
            CreateMap<SchufaTAdresse, AdressType>()
                .ForMember(type => type.PLZ, opt => opt.MapFrom(adresse => adresse.PLZ))
                .ForMember(type => type.land, opt => opt.MapFrom(adresse => adresse.Land))
                .ForMember(type => type.ort, opt => opt.MapFrom(adresse => adresse.Ort))
                .ForMember(type => type.strasse, opt => opt.MapFrom(adresse => adresse.Strasse));

            CreateMap<SchufaTVerbraucherdaten, VerbraucherdatenType>()
                .ForMember(type => type.aktuelleAdresse, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.AktuelleAdresse))
                .ForMember(type => type.geburtsdatum, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geburtsdatum))
                .ForMember(type => type.geburtsort, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geburtsort))
                .ForMember(type => type.geschlecht, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geschlecht))
                .ForMember(type => type.nachname, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Nachname))
                .ForMember(type => type.schufaKlauselDatum, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.SchufaKlauselDatum))
                .ForMember(type => type.titel, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Titel))
                .ForMember(type => type.voradresse, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Voradresse))
                .ForMember(type => type.vorname, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Vorname))
                .ForMember(type => type.SCHUFAID, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.SCHUFAID))
                ;

            CreateMap<SchufaTVerbraucherdaten, KorrekturVerbraucherdatenType>()
                .ForMember(type => type.geburtsdatum, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geburtsdatum))
                .ForMember(type => type.geburtsort, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geburtsort))
                .ForMember(type => type.geschlecht, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Geschlecht))
                .ForMember(type => type.nachname, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Nachname))
                .ForMember(type => type.titel, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Titel))
                .ForMember(type => type.vorname, opt => opt.MapFrom(verbraucherdaten => verbraucherdaten.Vorname))
                ;

            CreateMap<SchufaTSachbearbeiter, SachbearbeiterType>()
                .ForMember(type => type.EMailAdresse, opt => opt.MapFrom(sachbearbeiter => sachbearbeiter.EMailAdresse))
                .ForMember(type => type.sachbearbeiter, opt => opt.MapFrom(sachbearbeiter => sachbearbeiter.Sachbearbeiter))
                .ForMember(type => type.abteilung, opt => opt.MapFrom(sachbearbeiter => sachbearbeiter.Abteilung))
                .ForMember(type => type.faxnummer, opt => opt.MapFrom(sachbearbeiter => sachbearbeiter.Faxnummer))
                .ForMember(type => type.telefonnummer, opt => opt.MapFrom(sachbearbeiter => sachbearbeiter.Telefonnummer));
            
            CreateMap<SchufaTMerkmal, BetragType>()
                .ForMember(type => type.value, opt => opt.MapFrom(merkmal => string.IsNullOrEmpty(merkmal.Betrag) ? null : merkmal.Betrag))
                .ForMember(type => type.waehrung, opt => opt.MapFrom(merkmal => merkmal.Waehrung));

            CreateMap<SchufaTMerkmal, MeldemerkmalType>()
                .ForMember(type => type.betrag, opt => opt.MapFrom(merkmal => string.IsNullOrEmpty(merkmal.Betrag) ? null : merkmal))
                .ForMember(type => type.datum, opt => opt.MapFrom(merkmal => merkmal.Datum))
                .ForMember(type => type.kontonummer, opt => opt.MapFrom(merkmal => merkmal.Kontonummer))
                .ForMember(type => type.merkmalcode, opt => opt.MapFrom(merkmal => merkmal.Merkmalcode))
                .ForMember(type => type.ratenart, opt => opt.MapFrom(merkmal => merkmal.Ratenart))
                .ForMember(type => type.ratenzahl, opt => opt.MapFrom(merkmal => merkmal.Ratenzahl));

            CreateMap<SchufaTMerkmal, AnfragemerkmalType>()
                .ForMember(type => type.betrag, opt => opt.MapFrom(merkmal => string.IsNullOrEmpty(merkmal.Betrag) ? null : merkmal));

            CreateMap<SchufaTSterbedaten, SterbedatenType>()
                .ForMember(dest => dest.sterbedatum, opt => opt.MapFrom(src => src.Sterbedatum))
                .ForMember(dest => dest.sterbeurkundennummer, opt => opt.MapFrom(src => src.Sterbeurkundennummer));

            CreateMap<SchufaTProduktinformationen, ProduktinformationenType>()
                .ForMember(dest => dest.abrufverfahren, opt => opt.MapFrom(src => src.Abrufverfahren))
                .ForMember(dest => dest.auftragsart, opt => opt.MapFrom(src => src.Auftragsart));


            // Meldung

            CreateMap<SchufaTMeldung, KorrekturAdresseType>()
                .ForMember(kva => kva.ansprechpartner, opt => opt.MapFrom(dto => dto.Ansprechpartner))
                .ForMember(kva => kva.korrigierteAdresse, opt => opt.MapFrom(dto => dto.NeueAdresse))
                .ForMember(kva => kva.verbraucherdaten, opt => opt.MapFrom(dto => dto.Verbraucherdaten))
                .ForMember(kva => kva.version, opt => opt.MapFrom(dto => "1.0"));

            CreateMap<SchufaTMeldung, MeldungVertragsdatenType>()
                .ForMember(vd => vd.ansprechpartner, opt => opt.MapFrom(dto => dto.Ansprechpartner))
                .ForMember(kva => kva.verbraucherdaten, opt => opt.MapFrom(dto => dto.Verbraucherdaten))
                .ForMember(kva => kva.meldeart, opt => opt.MapFrom(dto => dto.Meldeart))
                .ForMember(kva => kva.meldemerkmal, opt => opt.MapFrom(dto => dto.Meldemerkmal))
                .ForMember(kva => kva.zusatzinformation, opt => opt.MapFrom(dto => dto.Zusatzinformation))
                .ForMember(kva => kva.version, opt => opt.MapFrom(dto => "1.0"));

            CreateMap<SchufaTMeldung, KorrekturVerbraucherType>()
                .ForMember(dest => dest.verbraucherdaten, opt => opt.MapFrom(src => src.Verbraucherdaten))
                .ForMember(dest => dest.ansprechpartner, opt => opt.MapFrom(src => src.Ansprechpartner))
                .ForMember(dest => dest.korrigierteVerbraucherdaten, opt => opt.MapFrom(src => src.KorrigierteVerbraucherdaten))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            CreateMap<SchufaTMeldung, MeldungTodesfallType>()
                .ForMember(dest => dest.verbraucherdaten, opt => opt.MapFrom(src => src.Verbraucherdaten))
                .ForMember(dest => dest.ansprechpartner, opt => opt.MapFrom(src => src.Ansprechpartner))
                .ForMember(dest => dest.sterbedaten, opt => opt.MapFrom(src => src.Sterbedaten))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            CreateMap<SchufaTMeldung, NeumeldungNachnameType>()
                .ForMember(dest => dest.verbraucherdaten, opt => opt.MapFrom(src => src.Verbraucherdaten))
                .ForMember(dest => dest.ansprechpartner, opt => opt.MapFrom(src => src.Ansprechpartner))
                .ForMember(dest => dest.neuerNachname, opt => opt.MapFrom(src => src.NeuerNachname))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            CreateMap<SchufaTMeldung, NeumeldungAdresseType>()
                .ForMember(dest => dest.verbraucherdaten, opt => opt.MapFrom(src => src.Verbraucherdaten))
                .ForMember(dest => dest.ansprechpartner, opt => opt.MapFrom(src => src.Ansprechpartner))
                .ForMember(dest => dest.neueAdresse, opt => opt.MapFrom(src => src.NeueAdresse))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));
            

            // Abruf Nachmeldung

            CreateMap<SchufaTAbrufNachmeldungPI, AbrufNachmeldungType>()
                .ForMember(dest => dest.produktinformationen, opt => opt.MapFrom(src => src.Produktinformationen))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            // Anfrage Bonitätsauskunft

            CreateMap<SchufaTAnfrageBonitaetsauskunft, AnfrageBonitaetsauskunftType>()
                .ForMember(dest => dest.abrufreferenz, opt => opt.MapFrom(src => src.Abrufreferenz))
                .ForMember(dest => dest.anfragemerkmal, opt => opt.MapFrom(src => src.Anfragemerkmal))
                .ForMember(dest => dest.verbraucherdaten, opt => opt.MapFrom(src => src.Verbraucherdaten))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            // Abruf Manuelle Weiterverarbeitung
            
            CreateMap<SchufaTAbrufManuelleWeiterverarbeitung, AbrufManuelleWeiterverarbeitungType>()
                .ForMember(dest => dest.abrufreferenz, opt => opt.MapFrom(src => src.Abrufreferenz))
                .ForMember(dest => dest.version, opt => opt.MapFrom(src => "1.0"));

            // ---------------------------------------
            // ---------------------------------------
            // Response to SchufaT
            // ---------------------------------------
            // ---------------------------------------

            CreateMap<FehlermeldungType, SchufaTAusnahmeDto>()
                .ForMember(dto => dto.FehlerlisteFachlich, opt => opt.MapFrom(type => type.fehlermeldung.Select(a=>a.fachlicherFehler).Where(a=>a != null)))
                .ForMember(dto => dto.FehlerlisteTechnisch, opt => opt.MapFrom(type => type.fehlermeldung.Select(a=>a.technischerFehler).Where(a=>a != null)))
                ;

            CreateMap<FachlicherFehlerType, SchufaTFehler>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.fehlertext))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.fehlercode))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.feldname));

            CreateMap<TechnischerFehlerType, SchufaTFehler>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.fehlertext))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.fehlercode));

            //Meldung

            CreateMap<ReaktionType, SchufaMeldungOutDto>()
                .ForMember(dto => dto.SchufaReferenz, opt => opt.MapFrom(type => type.schufaReferenz))
                .ForMember(dto => dto.Teilnehmerreferenz, opt => opt.MapFrom(type => type.teilnehmerreferenz));

            CreateMap<MeldungsbestaetigungType, SchufaMeldungOutDto>();

            //Abruf Nachmeldung

            CreateMap<ReaktionType, SchufaAbrufNachmeldungOutDto>()
                .ForMember(dto => dto.SchufaReferenz, opt => opt.MapFrom(type => type.schufaReferenz))
                .ForMember(dto => dto.Teilnehmerreferenz, opt => opt.MapFrom(type => type.teilnehmerreferenz))
                .ForMember(dto => dto.KeineNachrichtenVerfuegbar, opt => opt.MapFrom(type => type.keineNachrichtenVerfuegbar != null));

            CreateMap<NachmeldungType, SchufaAbrufNachmeldungOutDto>();

            // Anfrage Bonitätsauskunft

            CreateMap<ReaktionType, SchufaAnfrageBonitaetsauskunftOutDto>()
                .ForMember(dto => dto.SchufaReferenz, opt => opt.MapFrom(type => type.schufaReferenz))
                .ForMember(dto => dto.Teilnehmerreferenz, opt => opt.MapFrom(type => type.teilnehmerreferenz))
                .ForMember(dto => dto.Abrufversion, opt => opt.MapFrom(src => src.manuelleWeiterverarbeitung != null ? src.manuelleWeiterverarbeitung.version : null))
                .ForMember(dto => dto.Abrufreferenz, opt => opt.MapFrom(src => src.manuelleWeiterverarbeitung != null ? src.manuelleWeiterverarbeitung.abrufreferenz : null))
                ;

            CreateMap<BonitaetsauskunftType, SchufaAnfrageBonitaetsauskunftOutDto>();
            
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //Response zur Datenbank
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------

            CreateMap<SchufaTAusnahmeDto, SFAUSNAHME>()
                .ForMember(dest => dest.CODE, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.MESSAGE, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.SCHUFAREFERENZ, opt => opt.MapFrom(src => src.SchufaReferenz))
                .ForMember(dest => dest.TEILNEHMERREFERENZ, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                ;

            CreateMap<SchufaTFehler, SFFEHLER>()
                .ForMember(dest => dest.DETAIL, opt => opt.MapFrom(src => src.Detail))
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TEXT, opt => opt.MapFrom(src => src.Text))
                ;


            CreateMap<string, SFSCOREINFOTXT>()
                .ForMember(dest => dest.VALUE, opt => opt.MapFrom(src => src));

            CreateMap<ScoreinformationenType, SFSCOREINFO>()
                .ForMember(dest => dest.BESCHREIBUNG, opt => opt.MapFrom(src => src.beschreibung))
                .ForMember(dest => dest.RISIKOQUOTE, opt => opt.MapFrom(src => src.risikoquote))
                .ForMember(dest => dest.SCOREBEREICH, opt => opt.MapFrom(src => src.scorebereich))
                .ForMember(dest => dest.SCOREFEHLER, opt => opt.MapFrom(src => src.scorefehler))
                .ForMember(dest => dest.SFSCOREINFOTXTList, opt => opt.MapFrom(src => src.scoreinfotext))
                .ForMember(dest => dest.SCORETEXT, opt => opt.MapFrom(src => src.scoretext))
                .ForMember(dest => dest.SCOREWERT, opt => opt.MapFrom(src => src.scorewert))
                ;

            CreateMap<MerkmalNachmeldungType, SFMERKMAL>()
                .ForMember(dest => dest.BETRAG, opt => opt.MapFrom(src => src.betrag == null ? null : src.betrag.value))
                .ForMember(dest => dest.WAEHRUNG, opt => opt.MapFrom(src => src.betrag == null ? null : src.betrag.waehrung))
                .ForMember(dest => dest.DATUM, opt => opt.MapFrom(src => MapDatumFromSchufaToDb(src.datum)))
                .ForMember(dest => dest.BESCHREIBUNG, opt => opt.MapFrom(src => Truncate(src.beschreibung, 512)))
                ;

            CreateMap<TextmerkmalNachmeldungType, SFMERKMAL>()
                .ForMember(dest => dest.BESCHREIBUNG, opt => opt.MapFrom(src => Truncate(src.beschreibung, 512)));

            CreateMap<MerkmalType, SFMERKMAL>()
                .ForMember(dest => dest.BETRAG, opt => opt.MapFrom(src => src.betrag == null ? null : src.betrag.value))
                .ForMember(dest => dest.WAEHRUNG, opt => opt.MapFrom(src => src.betrag == null ? null : src.betrag.waehrung))
                .ForMember(dest => dest.DATUM, opt => opt.MapFrom(src => MapDatumFromSchufaToDb(src.datum)))
                .ForMember(dest => dest.BESCHREIBUNG, opt => opt.MapFrom(src => Truncate(src.beschreibung, 512)))
                ;

            CreateMap<TextmerkmalType, SFMERKMAL>()
                .ForMember(dest => dest.BESCHREIBUNG, opt => opt.MapFrom(src => Truncate(src.beschreibung, 512)));

            CreateMap<AdressType, SFADRESSE>();
            
                 
            CreateMap<VerbraucherdatenAuskunftType, SFPERSON>()
                .ForMember(dest => dest.SFADRESSE, opt => opt.MapFrom(src => src.aktuelleAdresse))
                .ForMember(dest => dest.SYSSFADRESSEVOR,
                opt => opt.ResolveUsing<AdressType>(new EntityStorer<AdressType,  SFADRESSE>(), src => src.voradresse))
                .ForMember(dest => dest.IDENTVORBEHALTPERSON, opt => opt.MapFrom(src => src.identitaetsvorbehaltPerson))
                .ForMember(dest => dest.IDENTVORBEHALTADRESSE, opt => opt.MapFrom(src => src.identitaetsvorbehaltAdresse))
                .ForMember(dest => dest.GEBURTSDATUM, opt => opt.MapFrom(src => MapDatumFromSchufaToDb(src.geburtsdatum)))
                ;


            //Meldung
            CreateMap<SchufaMeldungOutDto, SFOUTMELDUNG>()
                .ForMember(dest => dest.SCHUFAREFERENZ, opt => opt.MapFrom(src => src.SchufaReferenz))
                .ForMember(dest => dest.TEILNEHMERREFERENZ, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                .ForMember(dest => dest.TEXT, opt => opt.MapFrom(src => src.text));

            //Abruf Nachmeldung
            CreateMap<NachmeldemerkmalTypeConverterMerkmalElementType, SFMERKMAL>()
                .ConstructUsing(type =>
                {
                    var merkmal = new SFMERKMAL();
                    if (type.merkmal != null)
                    {
                        Mapper.Map(type.merkmal, merkmal);
                    }
                    if (type.textmerkmal != null)
                    {
                        Mapper.Map(type.textmerkmal, merkmal);
                    }
                    return merkmal;
                })
                .ForAllMembers(opt => opt.Ignore());


            CreateMap<SchufaAbrufNachmeldungOutDto, SFOUTNACHMELDG>()
                .ForMember(dest => dest.SFSCOREINFOList, opt => opt.MapFrom(src => src.scoreinformationen))
                .ForMember(dest => dest.TEILNEHMERREFERENZ, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                .ForMember(dest => dest.TEILNEHMERKENNUNG, opt => opt.MapFrom(src => src.teilnehmerkennung))
                .ForMember(dest => dest.SFMERKMALList, opt => opt.MapFrom(src => src.merkmale))
                .ForMember(dest => dest.AUSWEISGEPRUEFTEIDENT, opt => opt.MapFrom(src => src.ausweisgepruefteIdentitaet))
                .ForMember(dest => dest.SCHUFAREFERENZ, opt => opt.MapFrom(src => src.SchufaReferenz))
                .ForMember(dest => dest.SFPERSON, opt => opt.MapFrom(src => src.verbraucherdaten));

            // Anfrage Bonitätsauskunft

            CreateMap<AuskunftsmerkmalTypeConverterMerkmalElementType, SFMERKMAL>()
                .ConstructUsing(type =>
                {
                    var merkmal = new SFMERKMAL();
                    if (type.merkmal != null)
                    {
                        Mapper.Map(type.merkmal, merkmal);
                    }
                    if (type.textmerkmal != null)
                    {
                        Mapper.Map(type.textmerkmal, merkmal);
                    }
                    return merkmal;
                })
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<VerarbeitungsinformationType, SFVERARBINFO>()
                .ForMember(dest => dest.ERGEBNISTYP, opt => opt.MapFrom(src => src.ergebnistyp))
                .ForMember(dest => dest.TEXT, opt => opt.MapFrom(src => src.text));

            CreateMap<SchufaAnfrageBonitaetsauskunftOutDto, SFOUTBONIAUSK>()
                .ForMember(dest => dest.TEILNEHMERREFERENZ, opt => opt.MapFrom(src => src.Teilnehmerreferenz))
                .ForMember(dest => dest.SCHUFAREFERENZ, opt => opt.MapFrom(src => src.SchufaReferenz))
                .ForMember(dest => dest.AUSWEISGEPIDENT, opt => opt.MapFrom(src => src.ausweisgepruefteIdentitaet))
                .ForMember(dest => dest.SFMERKMALList, opt => opt.MapFrom(src => src.merkmale))
                .ForMember(dest => dest.SFSCOREINFOList, opt => opt.MapFrom(src => src.scoreinformationen))
                .ForMember(dest => dest.TEILNEHMERKENNUNG, opt => opt.MapFrom(src => src.teilnehmerkennung))
                .ForMember(dest => dest.SFVERARBINFO, opt => opt.MapFrom(src => src.verarbeitungsinformation))
                .ForMember(dest => dest.SFPERSON, opt => opt.MapFrom(src => src.verbraucherdaten))
                .ForMember(dest => dest.ABRUFREFERENZ, opt => opt.MapFrom(src => src.Abrufreferenz))
                .ForMember(dest => dest.ABRUFVERSION, opt => opt.MapFrom(src => src.Abrufversion));
        }

        private object MapDatumFromDbToSchufa(DateTime? datum)
        {
            if (!datum.HasValue)
                return "00.00.0000";

            return datum.Value;
        }

        private object MapDatumFromSchufaToDb(string datum)
        {
            if (datum == "UNBEFRISTET")
                return DateTime.MaxValue;

            if (datum == "UNBEKANNT")
                return null;

            if (datum == "00.00.0000")
                return null;

            if (string.IsNullOrEmpty(datum))
                return null;

            return DateTime.ParseExact(datum, "dd'.'MM'.'yyyy", CultureInfo.InvariantCulture);
        }

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) 
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private T CheckType<T>(object p) where T : class
        {
            if (p is T)
            {
                return (T)p;
            }
            return null;
        }

        private List<T> CheckTypeList<T>(object p) where T : class
        {
            if (p is IEnumerable)
            {
                List<T> list = new List<T>();
                foreach (var item in p as IEnumerable)
                {
                    if (item is T)
                    {
                        list.Add((T)item);
                    }
                }
                if (list.Count > 0)
                    return list;
            }
            return null;
        }

        private List<string> CheckStringList<T>(object p)
        {
            if (p is IEnumerable)
            {
                List<string> list = new List<string>();
                foreach (var item in p as IEnumerable)
                {
                    if (item is T)
                    {
                        list.Add(item.ToString());
                    }
                }
                if (list.Count > 0)
                    return list;
            }
            return null;
        }
        private List<string> CheckStringList<T,T2>(object p)
        {
            if (p is IEnumerable)
            {
                List<string> list = new List<string>();
                foreach (var item in p as IEnumerable)
                {
                    if (item is T || item is T2)
                    {
                        list.Add(item.ToString());
                    }
                }
                if (list.Count > 0)
                    return list;
            }
            return null;
        }

        private T? CheckAnyType<T>(object p) where T:struct
        {
            if (p is T)
            {
                return (T)p;
            }
            return null;
        }

    }
}