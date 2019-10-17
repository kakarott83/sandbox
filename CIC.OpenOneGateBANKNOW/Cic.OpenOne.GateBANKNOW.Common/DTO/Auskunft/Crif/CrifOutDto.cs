namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Crif
{
    using CrifSoapService;

    public class CrifOutDto
    {
        public CrifIdentifyAddressOutDto IdentifyAddress { get; set; }

        //###
        public CrifGetArchivedReportOutDto GetArchivedReport { get; set; }
        public CrifTError Error { get; set; }
        public CrifGetListOfReadyOfflineReportsOutDto GetListOfReadyOfflineReports { get; set; }
        public CrifPollOfflineReportOutDto PollOfflineReport { get; set; }
        public CrifOrderOfflineReportOutDto OrderOfflineReport { get; set; }
        public CrifGetDebtDetailsOutDto GetDebtDetails { get; set; }
        public CrifGetReportOutDto GetReport { get; set; }
    }

    public class CrifGetReportOutDto : TypeGetReportResponse
    {
    }

    public class CrifGetDebtDetailsOutDto : TypeGetDebtDetailsResponse
    {
    }

    public class CrifOrderOfflineReportOutDto : TypeOrderOfflineReportResponse
    {
    }

    public class CrifPollOfflineReportOutDto : TypePollOfflineReportResponseResponse
    {
    }

    public class CrifGetListOfReadyOfflineReportsOutDto : TypeGetListOfReadyOfflineReportsResponse
    {
    }

    //###
    public class CrifGetArchivedReportOutDto : TypeGetArchivedReportResponse
    {
    }

    public class CrifIdentifyAddressOutDto : TypeIdentifyAddressResponse
    {
    }

    public class CrifTError
    {
        public int Code { get; set; }
        public string MessageText { get; set; }
        public string FaultCode { get; set; }
        public string Message { get; set; }
    }
}