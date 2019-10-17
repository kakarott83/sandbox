namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifGetListOfReadyOfflineReportsBo : AbstractCrifBo<TypeGetListOfReadyOfflineReportsRequest, TypeGetListOfReadyOfflineReportsResponse>
    {
        public CrifGetListOfReadyOfflineReportsBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao)
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifGetListOfReadyOfflineReports)
        {
        }

        protected override TypeGetListOfReadyOfflineReportsRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.GetListOfReadyOfflineReports, new TypeGetListOfReadyOfflineReportsRequest());
        }

        protected override TypeGetListOfReadyOfflineReportsResponse ExecuteRequest(long sysAuskunft, TypeGetListOfReadyOfflineReportsRequest request)
        {
            return crifWsDao.GetListOfReadyOfflineReports(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeGetListOfReadyOfflineReportsResponse response)
        {
            crifOutDto.GetListOfReadyOfflineReports = Mapper.Map(response, new CrifGetListOfReadyOfflineReportsOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.GetListOfReadyOfflineReports.SysCfHeader;
        }
    }
}