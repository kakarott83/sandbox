namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Crif
{
    using CrifSoapService;

    public class CrifInDto
    {
        public CrifIdentifyAddressInDto IdentifyAddress { get; set; }

        //###
        public CrifGetArchivedReportInDto GetArchivedReport { get; set; }

        public CrifGetListOfReadyOfflineReportsInDto GetListOfReadyOfflineReports { get; set; }

        public CrifPollOfflineReportInDto PollOfflineReport { get; set; }
        public CrifOrderOfflineReportInDto OrderOfflineReport { get; set; }
        public CrifGetDebtDetailsInDto GetDebtDetails { get; set; }
        public CrifGetReportInDto GetReport { get; set; }
    }

    public class CrifGetReportInDto : TypeGetReportRequest
    {
        public long SysCfHeader { get; set; }
    }

    public class CrifGetDebtDetailsInDto : TypeGetDebtDetailsRequest
    {
        public long SysCfHeader { get; set; }
    }

    public class CrifOrderOfflineReportInDto : TypeOrderOfflineReportRequest
    {
        public long SysCfHeader { get; set; }
    }

    public class CrifPollOfflineReportInDto : TypePollOfflineReportResponseRequest
    {
        public long SysCfHeader { get; set; }
    }

    public class CrifGetListOfReadyOfflineReportsInDto : TypeGetListOfReadyOfflineReportsRequest
    {
        public long SysCfHeader { get; set; }
    }

    //###
    public class CrifGetArchivedReportInDto : TypeGetArchivedReportRequest
    {
        public long SysCfHeader { get; set; }
    }

    public class CrifIdentifyAddressInDto : TypeIdentifyAddressRequest
    {
        public long SysCfHeader { get; set; }
    }
}