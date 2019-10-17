namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifPollOfflineReportBo : AbstractCrifBo<TypePollOfflineReportResponseRequest, TypePollOfflineReportResponseResponse>
    {
        public CrifPollOfflineReportBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao)
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifPollOfflineReport)
        {
        }

        protected override TypePollOfflineReportResponseRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.PollOfflineReport, new TypePollOfflineReportResponseRequest());
        }

        protected override TypePollOfflineReportResponseResponse ExecuteRequest(long sysAuskunft, TypePollOfflineReportResponseRequest request)
        {
            return crifWsDao.PollOfflineReport(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypePollOfflineReportResponseResponse response)
        {
            crifOutDto.PollOfflineReport = Mapper.Map(response, new CrifPollOfflineReportOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.PollOfflineReport.SysCfHeader;
        }
    }
}