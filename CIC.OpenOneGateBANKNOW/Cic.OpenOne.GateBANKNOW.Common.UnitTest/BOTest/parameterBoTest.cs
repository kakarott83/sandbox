using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Mocks;
using NUnit.Framework;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using AutoMapper.Configuration;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Testklasse zum Testen der Methoden des parameterBo´s
    /// </summary>
    [TestFixture()]
    public class ParameterBoTest
    {
     

        /// <summary>
        /// Blackboxtest für die listAvailableProductParams Methode
        /// </summary>
        [Test]
        public void listAvailableProductParamsTest()
        {


            IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            DynamicMock PrismaDaoMock = new DynamicMock(typeof(IPrismaDao));
            DynamicMock ObTypDaoMock = new DynamicMock(typeof(IObTypDao));
            PrismaParameterBo bo = new PrismaParameterBo((IPrismaDao)PrismaDaoMock.MockInstance, (IObTypDao)ObTypDaoMock.MockInstance, PrismaParameterBo.CONDITIONS_BANKNOW);

            prKontextDto input = new prKontextDto();
            input.perDate = DateTime.Now;
            input.sysbrand = 1;
            input.sysprchannel = 5;
            input.sysprhgroup = 3;
            input.sysobart = 1;
            input.sysobtyp = 43668;
            input.sysprproduct = 4;
            //input-kontext is not service specific but generally usable for prisma-bo, so no mapping is necessary
            List<ParameterSetConditionLink> allparamSetLinks = new List<ParameterSetConditionLink>()
            {
                new ParameterSetConditionLink()
                {
                    area = 10,
                    sysid = 100,
                    sysprparset = 1,
                    CONDITIONID = 1,
                    TARGETID = 0,
                    VALIDFROM = new DateTime(2011, 01,01),
                    VALIDUNTIL = new DateTime(2012,01,01)
                }
            };

            List<ParameterConditionLink> allparamLinks = new List<ParameterConditionLink>()
            {
                new ParameterConditionLink()
                {
                    area = 10,
                    sysbrand = 1,
                    sysobtyp = 43668,
                    sysprhgroup = 3,
                    sysprkgroup = 0,
                    sysprparset = 1,
                    sysprproduct = 4,
                    TARGETID = 0,
                    VALIDFROM = new DateTime(2011,01,01),
                    VALIDUNTIL = new DateTime(2012,01,01)
                }
            };

            List<ParamDto> allParams = new List<ParamDto>()
            {
                new ParamDto()
                {
                    defvaln = 1,
                    defvalp = 2,
                    disabled = false,
                    maxvaln = 10,
                    maxvalp = 10,
                    meta = "Meta",
                    minvaln = 1,
                    minvalp = 2,
                    name = "Name",
                    steplist = new SteplistDto[]
                    {
                        new SteplistDto()
                        {
                            stepval = 2
                        }
                    },
                    steplistcsv = "Steplistcsv",
                    stepsize = 1,
                    sysID = 10,
                    sysprfld = 10,
                    sysprparset = 10,
                    type = 2,
                    visible = true
                }
            };

            PrismaDaoMock.SetReturnValue("getParamSets", allparamSetLinks);
            PrismaDaoMock.SetReturnValue("getParamConditionLinks", allparamLinks);
            ObTypDaoMock.SetReturnValue("getObTypAscendants", new List<long>{ 1 });
            PrismaDaoMock.SetReturnValue("getParams", allParams);

            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prparams = bo.listAvailableParameter(input);


            IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
            {
                Config.CreateMap<Cic.OpenOne.Common.DTO.Prisma.ParamDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto>();
            },true);
            mapper.Map<Cic.OpenOne.Common.DTO.Prisma.ParamDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.ParamDto[]>(prparams.ToArray());
                

            Assert.IsEmpty(prparams);
        }
    }
}
