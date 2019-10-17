using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Eurotax Web Service Data Access Object Interface
    /// </summary>
    public interface IEurotaxWSDao
    {
        /// <summary>
        /// Eurotax Webservice GetForecast: Restwertabfrage für Neuwagen
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sysId">sysId vom ObTyp</param>
        void getForecast(ref EurotaxRef.GetForecastRequest1 request, long sysId);

        /// <summary>
        /// Eurotax Webservice GetValuation: Liefert die Listenpreise für Gebrauchtwagen
        /// </summary>
        /// <param name="header"></param>
        /// <param name="settings"></param>
        /// <param name="valuation"></param>
        /// <param name="sysId">sysId vom ObTyp</param>
        void getValuation(ref EurotaxValuationRef.ETGHeaderType header, ref EurotaxValuationRef.ETGsettingType settings, ref EurotaxValuationRef.ValuationType valuation, long sysId);

        /// <summary>
        /// interface method to call Eurotax Webservice method GetHistoricalForecast()
        /// </summary>
        /// <param name="header"></param>
        /// <param name="settings"></param>
        /// <param name="extendedVehicleType"></param>
        void getHistoricalForecast(EurotaxRef.ETGHeaderType header, EurotaxRef.ETGsettingType settings, EurotaxRef.ExtendedVehicleType extendedVehicleType);

        /// <summary>
        /// Eurotax WebService getVinDecode
        /// </summary>
        /// <param name="header"></param>
        /// <param name="settings"></param>
        /// <param name="vinDecodeInput"></param>
        /// <param name="sysId"></param>
        void getVinDecode(ref EurotaxVinRef.VinDecodeRequest request1, ref EurotaxVinRef.VinDecodeOutputType response);

        /// <summary>
        ///  getSoapXMLDto;
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);

        
    }
}
