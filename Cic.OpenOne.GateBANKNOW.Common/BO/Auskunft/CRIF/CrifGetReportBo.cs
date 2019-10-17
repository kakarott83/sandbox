namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifGetReportBo:AbstractCrifBo<TypeGetReportRequest,TypeGetReportResponse>
    {
        public CrifGetReportBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao, string auskunftTyp) 
            : base(crifWsDao, crifDbDao, auskunftDao, auskunftTyp)
        {
        }

        protected override TypeGetReportRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.GetReport, new TypeGetReportRequest());
        }

        protected override TypeGetReportResponse ExecuteRequest(long sysAuskunft, TypeGetReportRequest request)
        {
            return crifWsDao.GetReport(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeGetReportResponse response)
        {
            crifOutDto.GetReport = Mapper.Map(response, new CrifGetReportOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.GetReport.SysCfHeader;
        }
    }
}
