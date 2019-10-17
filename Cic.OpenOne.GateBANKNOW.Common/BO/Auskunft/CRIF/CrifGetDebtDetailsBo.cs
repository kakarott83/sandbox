namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using System;
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifGetDebtDetailsBo:AbstractCrifBo<TypeGetDebtDetailsRequest,TypeGetDebtDetailsResponse>
    {
        public CrifGetDebtDetailsBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao) 
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifGetDebtDetail)
        {
        }

        protected override TypeGetDebtDetailsRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.GetDebtDetails, new TypeGetDebtDetailsRequest());
        }

        protected override TypeGetDebtDetailsResponse ExecuteRequest(long sysAuskunft, TypeGetDebtDetailsRequest request)
        {
            return crifWsDao.GetDebtDetails(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeGetDebtDetailsResponse response)
        {
            crifOutDto.GetDebtDetails = Mapper.Map(response, new CrifGetDebtDetailsOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.GetDebtDetails.SysCfHeader;
        }
    }
}
