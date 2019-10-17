namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    //###
    public class CrifGetArchivedReportBo : AbstractCrifBo<TypeGetArchivedReportRequest, TypeGetArchivedReportResponse>
    {
        public CrifGetArchivedReportBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao)
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifGetArchivedReport)
        {
        }

        protected override TypeGetArchivedReportRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            //###
            return Mapper.Map(inDto.GetArchivedReport, new TypeGetArchivedReportRequest());
        }

        protected override TypeGetArchivedReportResponse ExecuteRequest(long sysAuskunft, TypeGetArchivedReportRequest request)
        {
            //###
            return crifWsDao.GetArchivedReport(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeGetArchivedReportResponse response)
        {
            //###
            crifOutDto.GetArchivedReport = Mapper.Map(response, new CrifGetArchivedReportOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            //###
            return inDto.GetArchivedReport.SysCfHeader;
        }
    }
}