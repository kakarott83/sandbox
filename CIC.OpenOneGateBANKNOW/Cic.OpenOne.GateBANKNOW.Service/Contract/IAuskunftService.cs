
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System.ServiceModel;
namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface IAuskunftService stellt die Schnittstelle für alle extern angebundenen Auskunftsdienste zur Verfügung, z.B. 
    /// 
    /// Eurotax|Mail|ZEK|Deltavista|Kremo
    /// 
    /// </summary>
    [ServiceContract(Name = "IAuskunftService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IAuskunftService
    {
        /// <summary>
        /// Die Methode instantiiert die im Auskunfttypen angegebene ServiceFacade und ruft deren Methode doAuskunft auf
        /// </summary>
        /// <param name="sysAuskunft">Datenbank ID des Auskunftssatzes</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        [OperationContract]
        oMessagingDto doAuskunft(long sysAuskunft);

        /// <summary>
        /// Web interface to call Mail sending
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Subject">Subject of the mail</param>
        /// <param name="Data">Data body data (intended to be a PDF datastream)</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        [OperationContract]
        oMessagingDto sendMail(string To, string From, string Subject, byte[] Data);
        
        /// <summary>
        /// Web interface to call the Fax sending
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Data">Fax Body (PDF datastream)</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        [OperationContract]
        oMessagingDto sendFax(string To, string From, byte[] Data);
        
        /// <summary>
        /// Web Interface to send an SMS
        /// </summary>
        /// <param name="To">Recipient</param>
        /// <param name="From">Sender</param>
        /// <param name="Body">SMS Text as a String</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        [OperationContract]
        oMessagingDto sendSms(string To, string From, string Body);

        /// <summary>
        /// Web Interface to send an E-Mail, SMS or Fax via DB settings
        /// </summary>
        /// <param name="ID">DB ID</param>
        /// <returns>oMessagingDto (Success = true, Failure = false)</returns>
        [OperationContract]
        oMessagingDto sendDbNotification(int ID);

        /// <summary>
        /// Eurotax GetForecast Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxDto</param>
        /// <returns>oEurotaxDto</returns>        
        [OperationContract]
        oEurotaxGetForecastDto EurotaxGetForecast(iEurotaxGetForecastDto input);

        /// <summary>
        /// Eurotax GetForecast Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxDto</param>
        /// <returns>oEurotaxDto</returns>        
       [OperationContract]
        oEurotaxGetForecastDto EurotaxGetRemo(iEurotaxGetForecastDto input);

        /// <summary>
        /// Eurotax GetValuation Auskunft einholen
        /// </summary>
        /// <param name="input">iEurotaxDto</param>
        /// <returns>oEurotaxDto</returns>        
        [OperationContract]
        oEurotaxGetValuationDto EurotaxGetValuation(iEurotaxGetValuationDto input);

        /// <summary>
        /// S1 Get Response
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oMessagingDto getAuskunftS1(S1AnswerInputData input);

        /// <summary>
        /// GetEL
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        [OperationContract]
        ogetELDto getEL(long sysantrag);

    }
}
