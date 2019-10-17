using Cic.OpenOne.Common.DAO;
using System;
using NUnit.Framework;
using Cic.OpenOne.Common.BO;
using NMock2;
using Cic.OpenOne.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Util.Mapper;
using AutoMapper.Mappers;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.UnitTest.BO
{

    class Source
    {
        public long? zahl;
        public DateTime? datum;
        public long zahl2;
        public DateTime datum2;
    }
    class Target
    {
        public long? zahl;
        public DateTime? datum;
        public long zahl2;
        public DateTime datum2;
    }

    /// <summary>
    ///MapperTest
    ///</summary>
    [TestFixture]
    public class MapperTest
    {


      
     
        /// <summary>
        /// test nullable mapper
        /// </summary>
        [Test]
        public void testNullableMapper()
        {
            List<IObjectMapper> mappers = new List<IObjectMapper>(MapperRegistry.AllMappers());
            //mappers.Add(new GenericIgnoreNullMapper());
            Configuration cfg = new Configuration(new TypeMapFactory(), mappers);
            
            //Mapper.CreateMap<long?, long?>().ConvertUsing<GenericIgnoreNullMapper>();
            //Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<GenericIgnoreNullMapper>();

            Mapper.Initialize(Conf =>
            {
                Conf.ForSourceType<DateTime?>().AllowNullDestinationValues = false;
                Conf.ForSourceType<long?>().AllowNullDestinationValues = false;
                Conf.CreateMap<Source, Target>();
            });

            Source src = new Source();
            src.datum = null;
            src.zahl = null;
            src.zahl2 = 1111;
            src.datum2 = DateTime.Now;

            Target tgt = new Target();
            tgt.zahl2 = 2222;
            tgt.datum2 = DateTime.Now.AddDays(4);
            tgt.zahl = 10;
            tgt.datum = DateTime.Now;

           // cfg.CreateMap<Source, Target>();
           // MappingEngine mapper = new MappingEngine(cfg);

            Mapper.Map<Source, Target>(src, tgt);

            //int a = 10;
        }
    }
}
