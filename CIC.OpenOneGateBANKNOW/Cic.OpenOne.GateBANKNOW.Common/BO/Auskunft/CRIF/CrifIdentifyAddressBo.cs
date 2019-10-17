namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifIdentifyAddressBo : AbstractCrifBo<TypeIdentifyAddressRequest, TypeIdentifyAddressResponse>
    {
        public CrifIdentifyAddressBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao) 
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifIdentifyAddress)
        {
        }

        protected override TypeIdentifyAddressRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.IdentifyAddress, new TypeIdentifyAddressRequest());
        }

        protected override TypeIdentifyAddressResponse ExecuteRequest(long sysAuskunft, TypeIdentifyAddressRequest request)
        {
            return crifWsDao.IdentifyAddress(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeIdentifyAddressResponse response)
        {
            crifOutDto.IdentifyAddress = Mapper.Map(response, new CrifIdentifyAddressOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.IdentifyAddress.SysCfHeader;
        }
    }
}