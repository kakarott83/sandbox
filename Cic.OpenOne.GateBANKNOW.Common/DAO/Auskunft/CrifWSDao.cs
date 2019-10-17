namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using CrifSoapService;
    using OpenOne.Common.DTO;
    using OpenOne.Common.Util.Logging;

    public interface ICrifWSDao
    {
        SoapXMLDto getSoapXMLDto();
        TypeIdentifyAddressResponse IdentifyAddress(TypeIdentifyAddressRequest request);

        //###
        TypeGetArchivedReportResponse GetArchivedReport(TypeGetArchivedReportRequest request);
        TypeGetListOfReadyOfflineReportsResponse GetListOfReadyOfflineReports(TypeGetListOfReadyOfflineReportsRequest request);
        TypePollOfflineReportResponseResponse PollOfflineReport(TypePollOfflineReportResponseRequest request);
        TypeOrderOfflineReportResponse OrderOfflineReport(TypeOrderOfflineReportRequest request);
        TypeGetDebtDetailsResponse GetDebtDetails(TypeGetDebtDetailsRequest request);
        TypeGetReportResponse GetReport(TypeGetReportRequest request);

        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
    }

    public class CrifWSDao : ICrifWSDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();

        public TypeIdentifyAddressResponse IdentifyAddress(TypeIdentifyAddressRequest request)
        {
            return ExecuteRequest(client => client.identifyAddress(request));
        }

        public TypeGetArchivedReportResponse GetArchivedReport(TypeGetArchivedReportRequest request)
        {
            //###
            return ExecuteRequest(client => client.getArchivedReport(request));
        }

        public TypeGetListOfReadyOfflineReportsResponse GetListOfReadyOfflineReports(TypeGetListOfReadyOfflineReportsRequest request)
        {
            return ExecuteRequest(client => client.getListOfReadyOfflineReports(request));
        }

        public TypePollOfflineReportResponseResponse PollOfflineReport(TypePollOfflineReportResponseRequest request)
        {
            return ExecuteRequest(client => client.pollOfflineReportResponse(request));
        }

        public TypeOrderOfflineReportResponse OrderOfflineReport(TypeOrderOfflineReportRequest request)
        {
            return ExecuteRequest(client => client.orderOfflineReport(request));
        }

        public TypeGetDebtDetailsResponse GetDebtDetails(TypeGetDebtDetailsRequest request)
        {
            return ExecuteRequest(client => client.getDebtDetails(request));
        }

        public TypeGetReportResponse GetReport(TypeGetReportRequest request)
        {
            return ExecuteRequest(client => client.getReport(request));
        }

        public T ExecuteRequest<T>(Func<CrifSoapServicePortTypeV1_0Client, T> func, [CallerMemberName] string callingMethod = "")
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var client = GetClient();
                _log.Info(string.Format("CRIF {0} Webserviceaufruf gestartet.", callingMethod));

                T response = func(client);

                _log.Info(string.Format("CRIF {0} Webserviceaufruf Dauer : {1}", callingMethod, DateTime.Now - startTime));

                return response;
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler im CRIF {0} Webserviceaufruf. Dauer : {1}", callingMethod, DateTime.Now - startTime), ex);
                throw ex;
            }
        }

        private CrifSoapServicePortTypeV1_0Client GetClient()
        {
            var client = new CrifSoapServicePortTypeV1_0Client();
            client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));
            return client;
        }

        public SoapXMLDto getSoapXMLDto()
        {
            return this.soapXMLDto;
        }

        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }
    }
}