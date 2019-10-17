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

using AutoMapper.Mappers;
using System.Reflection;
using System.ComponentModel;
using Cic.OpenOne.Common.Util.Mapper;
using CIC.Database.OL.EF6.Model;
using AutoMapper.Configuration;
#pragma warning disable 0649

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testcases for Automapper-Usage
    /// </summary>
    [TestFixture()]
    public class MapperBoTest
    {
       
        /// <summary>
        /// Initialisiert alle generellen Variablen und den Mock
        /// </summary>
        [SetUp]
        public void MapperBoTestInit()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SANDBOXTest()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void smallToBigDtoTest()
        {

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {

                Config.CreateMap<Source, Target>();
                Config.CreateMap<double, decimal?>().ConvertUsing<DoubleDecimalConverter>();


            }, true);

        

            

            Source source = new Source();
            source.A = 10;
            source.B = 20;

            Target target = mapper.Map<Source, Target>(source);

        }

        /// <summary>
        /// Blackboxtest Mapping Arrays
        /// To Map a type of array, the array item type has to be configured, not the array!
        /// The call to Map has to use the array type
        /// </summary>
        [Test]
        public void arrayMapTest()
        {
           
            List<Service.DTO.AngAntVsDto> vslist = new List<Service.DTO.AngAntVsDto>();
            Service.DTO.AngAntVsDto vs = new Service.DTO.AngAntVsDto();
            vslist.Add(vs);
            vs.praemie = 100;
            vs = new Service.DTO.AngAntVsDto();
            vslist.Add(vs);
            vs.praemie = 200;
            

            Service.DTO.AngAntVsDto[] data = vslist.ToArray();

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
            }, true);


           
            Cic.OpenOne.Common.DTO.AngAntVsDto[] result = mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto[], Cic.OpenOne.Common.DTO.AngAntVsDto[]>(data);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// uses the automapper to deepcopy an object
        ///  as this creates different mappings for probably already mapped types a own configuration instance has to be used
        /// </summary>
        [Test]
        public void deepCloneMapTest()
        {
            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto>();
            }, true);

            

            Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto source = new Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto();
            source.angAntKalkDto = new Service.DTO.AngAntKalkDto();
            source.angAntKalkDto.auszahlung = 50000;
           
            Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto target = new Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto();

            target = mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto>(source, target);
            target.angAntKalkDto.auszahlung = 100000;

            Assert.AreNotEqual(source.angAntKalkDto.auszahlung, target.angAntKalkDto.auszahlung);
        }


        /// <summary>
        /// tests why the common attribute type is mapped
        /// </summary>
        [Test]
        public void TypeMapTest()
        {
           

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, ITADRESSE>().ForMember(dest => dest.TYPE, opt => opt.Ignore());
            }, true);

            Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto source = new Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto();
            source.name = "TEST";



            ITADRESSE target = mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, ITADRESSE>(source);
           
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// Blackboxtest Mapping Arrays
        /// If one Type of the structure is not configured with createMap an exception will be raised:
        ///    Trying to map Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto to Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto.
        ///    Using mapping configuration for Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto to Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto
        ///    Eine Ausnahme vom Typ "AutoMapper.AutoMapperMappingException" wurde ausgelöst.
        /// </summary>
        [Test]
        public void deepStructureMapTest()
        {
            Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto input = new Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto();
            input.angebot = "TEST";
            input.angAntVars = new List<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto>();
            Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto var = new Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto();
            input.angAntVars.Add(var);
            var.bezeichnung = "Variante 1";
            var.kalkulation = new Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto();
            var.kalkulation.angAntVsDto = new List<Service.DTO.AngAntVsDto>();
            Service.DTO.AngAntVsDto vs = new Service.DTO.AngAntVsDto();
            var.kalkulation.angAntVsDto.Add(vs);
            vs.praemie = 100;
            vs = new Service.DTO.AngAntVsDto();
            var.kalkulation.angAntVsDto.Add(vs);
            vs.praemie = 200;

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVarDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>();
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AngAntObBriefDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>();
            }, true);

          
            Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebotInput = mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto>(input);

            Assert.IsNotNull(angebotInput.angAntVars);
        }

        /// <summary>
        /// Blackboxtest Mapping nullable values
        /// Result:
        ///  * Mapping a non-nullable value to a nullable value results in the default value of the non-nullable type (e.g. 01.01.0001 for dateTime)
        ///  * Mapping a nullable value having null to a non-nullable value results in the default value of the non-nullable type (e.g. 01.01.0001 for dateTime)
        ///  String is also nullable without ?
        /// </summary>
        [Test]
        public void nullableMapTest()
        {

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<long, long?>().ConvertUsing<GenericNullableConverter<long, long?>>();
                Config.CreateMap<long?, long?>().ConvertUsing<GenericNullableConverter<long?, long?>>();
                Config.CreateMap<DateTime, DateTime?>().ConvertUsing<GenericNullableConverter<DateTime, DateTime?>>();

                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>().ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null)); 
                Config.CreateMap<Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>();

            }, true);


          
          

            Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto nullableKunde = new Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto();
            Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto targetKunde = new Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto();

            nullableKunde.erfassung = DateTime.Now;
            nullableKunde.brandBezeichnung = "TEST";
            Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto notnullableKunde = mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>(nullableKunde);
            Assert.IsNotNull(notnullableKunde.erfassung);

            nullableKunde.erfassung =null;
            nullableKunde.brandBezeichnung = null;
            nullableKunde.syskd = null;
            targetKunde.syskd = 222;
            targetKunde.erfassung = DateTime.Now;
            notnullableKunde = mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>(nullableKunde,targetKunde);
            Assert.IsNull(notnullableKunde.erfassung);
            
            //Mapper.AssertConfigurationIsValid();
           /* notnullableKunde.einreisedatum = DateTime.Now;
            nullableKunde = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(notnullableKunde);
            Assert.IsNotNull(nullableKunde.einreisedatum);

            notnullableKunde = new Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto();
            
            nullableKunde = Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(notnullableKunde);
            Assert.IsNull(nullableKunde.einreisedatum);*/
        }

        /// <summary>
        /// dbTest
        /// </summary>
        [Test]
        public void dbTest()
        {

            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<SourceDb, TargetDb>().ForAllMembers(opt => opt.Condition((src, dest, srcVal, destVal, c) => srcVal != null)); ;

            }, true);


            
            using (DdOlExtended context = new DdOlExtended())
            {
                TargetDb target = new TargetDb();
                SourceDb t = new SourceDb();
                t.ASTR = "TEST1";
                SourceDb t2 = new SourceDb();
                t2.BSTR = "TEST2";

                mapper.Map(t, target);
                mapper.Map(t2, target);
            }
          
            
        }


    }
    class Source
    {
        public double A;
        public double B;
      
    }
    class Target
    {
        public double A;
        public decimal? B;
        public decimal? C;
        public decimal D;
        public double TESTFAILA;
       
    }
    class SourceDb
    {
      
        public string ASTR;
        public string BSTR;
    }
    class TargetDb
    {
      
        public string ASTR;
        public string BSTR;
    }
}
#pragma warning restore 0649