using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Webservice für Deltavista Zugriffe Schnittstelle
    /// </summary>
    public interface IDeltavistaWSDao
    {
        /// <summary>
        /// interface method to call Deltavista WebService getIdentifiedAddress()
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressDescription"></param>
        /// <param name="companyRegistrationNumber">UID</param>
        /// <returns></returns>
        DeltavistaRef.AddressIdentificationResponse getAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor, 
                                                                 DeltavistaRef.AddressDescription addressDescription, 
                                                                 String companyRegistrationNumber);

        /// <summary>
        /// interface method to call Deltavista WebService getCompanyDetails()
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        DeltavistaRef.CompanyDetailsResponse getCompanyDetailsByAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor, int addressId);

        /// <summary>
        /// interface method to call Deltavista WebService getDebtDetails()
        /// </summary>
        /// <param name="identityDescriptor"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        DeltavistaRef.DebtDetailsResponse getDebtDetailsByAddressId(DeltavistaRef.IdentityDescriptor identityDescriptor, int addressId);

        /// <summary>
        /// orderCresuraReport
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
        /// <returns></returns>
        DeltavistaRef2.CresuraReportResponse orderCresuraReport(DeltavistaRef2.IdentityDescriptor idDesc, DeltavistaRef2.AddressDescription addressDescription, 
                                                                DeltavistaRef2.OrderDescriptor orderDescriptor, string refNo, string reason, string contactEmail, 
                                                                string contactFaxNr, string contactName, string contactTelDirect, string binaryPOI, 
                                                                string binaryPOItype);

        /// <summary>
        /// getReport
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="addressId"></param>
        /// <param name="reportId"></param>
        /// <param name="targetFormat"></param>
        /// <returns></returns>
        DeltavistaRef2.ReportResponse getReport(DeltavistaRef2.IdentityDescriptor idDesc, int addressId, int reportId, string targetFormat);

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto">soapXMLDto</param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
    }
}