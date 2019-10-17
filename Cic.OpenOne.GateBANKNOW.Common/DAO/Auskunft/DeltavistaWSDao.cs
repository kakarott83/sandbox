using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Deltavista Web Service Data Access Object
    /// </summary>
    public class DeltavistaWSDao : IDeltavistaWSDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        DeltavistaRef.DVSoapServiceV4Client Client;
        DeltavistaRef2.DVSoapServiceClient Client2;
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();

        /// <summary>
        /// Sets endpoint address for Deltavista Webservice and calls its method getIdentifiedAddress, used for Adressvalidierung
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressDescription"></param>
        /// <param name="companyRegistrationNumber"></param>
        /// <returns></returns>
        public DeltavistaRef.AddressIdentificationResponse getAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor,
                                        DeltavistaRef.AddressDescription addressDescription, String companyRegistrationNumber)
        {
            try
            {
                Client = new DeltavistaRef.DVSoapServiceV4Client();

                Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Deltavista AddressIdentification Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;

                DeltavistaRef.AddressIdentificationResponse response =
                            Client.getIdentifiedAddress(identityDescriptor, addressDescription, companyRegistrationNumber);

                _log.Info("Deltavista AddressIdentification Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));

                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Deltavista AddressIdentification Webserviceaufruf. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Sets endpoint address for Deltavista Webservice and calls its method getCompanyDetailsByAddressId, used for Firmenauskunft
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public DeltavistaRef.CompanyDetailsResponse getCompanyDetailsByAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor, int addressId)
        {
            try
            {
                Client = new DeltavistaRef.DVSoapServiceV4Client();

                Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Deltavista CompanyDetails Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                DeltavistaRef.CompanyDetailsResponse response = Client.getCompanyDetailsByAddressId(identityDescriptor, addressId, null);
                _log.Info("Deltavista CompanyDetails Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Deltavista CompanyDetails Webserviceaufruf. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Sets endpoint address for Deltavista Webservice and calls its method getDebtDetailsByAddressId, used for Bonitätsauskunft
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public DeltavistaRef.DebtDetailsResponse getDebtDetailsByAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor, int addressId)
        {
            try
            {
                Client = new DeltavistaRef.DVSoapServiceV4Client();

                Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Deltavista DebtDetails Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                DeltavistaRef.DebtDetailsResponse response = Client.getDebtDetailsByAddressId(identityDescriptor, addressId);

                _log.Info("Deltavista DebtDetails Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Deltavista DebtDetails Webserviceaufruf. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls Deltavista WS getCresuraReport, used for Betreibungsauskunft, Einwohnerkontrolle
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="addressDescription"></param>
        /// <param name="orderDescriptor"></param>
        /// <param name="refNo"></param>
        /// <param name="reason"></param>
        /// <param name="contactEmail"></param>
        /// <param name="contactFaxNr"></param>
        /// <param name="contactName"></param>
        /// <param name="contactTelDirect"></param>
        /// <param name="binaryPOI"></param>
        /// <param name="binaryPOItype"></param>
        /// <returns>referenceNumber and transactionError</returns>
        public DeltavistaRef2.CresuraReportResponse orderCresuraReport(DeltavistaRef2.IdentityDescriptor idDesc, DeltavistaRef2.AddressDescription addressDescription,
                                                                       DeltavistaRef2.OrderDescriptor orderDescriptor, string refNo, string reason,
                                                                       string contactEmail, string contactFaxNr, string contactName, string contactTelDirect, string binaryPOI, string binaryPOItype)
        {
            try
            {
                Client2 = new DeltavistaRef2.DVSoapServiceClient();

                Client2.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Deltavista orderCresura Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                DeltavistaRef2.CresuraReportResponse response = Client2.orderCresuraReport(idDesc, addressDescription, orderDescriptor, refNo, reason,
                                                                                           contactEmail, contactFaxNr, contactName, contactTelDirect, binaryPOI, binaryPOItype);
                _log.Info("Deltavista orderCresura Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Deltavista orderCresura Webserviceaufruf. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls Deltavista WS getReport, used for Handelsregisterauskunft
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="addressId"></param>
        /// <param name="reportId"></param>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        public DeltavistaRef2.ReportResponse getReport(DeltavistaRef2.IdentityDescriptor idDesc, int addressId, int reportId, string targetFormat)
        {
            try
            {
                Client2 = new DeltavistaRef2.DVSoapServiceClient();

                Client2.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Deltavista getReport Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                DeltavistaRef2.ReportResponse response = Client2.getReportByAddressId(idDesc, addressId, reportId, targetFormat);
                _log.Info("Deltavista getReport Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Deltavista getReport Webserviceaufruf. ", ex);
                throw ex;
            }
        }

        #region Get/Set Methods

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto()
        {
            return this.soapXMLDto;
        }

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto">soapXMLDto</param>
        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }

        #endregion Get/Set Methods
    }
}