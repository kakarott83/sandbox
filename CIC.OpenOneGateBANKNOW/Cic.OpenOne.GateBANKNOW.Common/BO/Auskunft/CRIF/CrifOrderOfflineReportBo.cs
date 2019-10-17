namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft.Crif;

    public class CrifOrderOfflineReportBo : AbstractCrifBo<TypeOrderOfflineReportRequest, TypeOrderOfflineReportResponse>
    {
        public CrifOrderOfflineReportBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao)
            : base(crifWsDao, crifDbDao, auskunftDao, AuskunfttypDao.CrifGetOfflineReport)
        {
        }

        protected override TypeOrderOfflineReportRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            var element = new TypeOrderOfflineReportRequest();
            Mapper.Map(inDto.OrderOfflineReport, element);
            return element;
        }

        protected override TypeOrderOfflineReportResponse ExecuteRequest(long sysAuskunft, TypeOrderOfflineReportRequest request)
        {
            return crifWsDao.OrderOfflineReport(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeOrderOfflineReportResponse response)
        {
            crifOutDto.OrderOfflineReport = Mapper.Map(response, new CrifOrderOfflineReportOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.OrderOfflineReport.SysCfHeader;
        }
    }
}