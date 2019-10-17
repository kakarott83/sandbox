using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Cic.OpenOne.Common.Util.Behaviour;
using Cic.OpenOne.Common.Util.Logging;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;

using Cic.One.Utils.Util;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Eurotax Web Service Data Access Object Interface
    /// </summary>
    public class EurotaxWSDao : IEurotaxWSDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        EurotaxRef.ForecastPortDocumentClient ForecastClient;
        EurotaxValuationRef.ValuationSoapPortClient ValuationClient;
        

        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();

        internal class FaultFormatingBehavior : IEndpointBehavior
        {
            public void Validate(ServiceEndpoint endpoint){}

            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters){}

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher){}

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {            
                clientRuntime.MessageInspectors.Add(new FaultMessageInspector());         
            }
        }

        internal class FaultMessageInspector : IClientMessageInspector
        {
            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                return null;
            }

            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
                if (reply.IsFault)
                {
                    try
                    {
                        String response = reply.ToString();
                        if (response.IndexOf("Message with code: -42") > -1)
                        {
                            FaultCode fc = new FaultCode("-42");

                            Message newFaultMessage = Message.CreateMessage(reply.Version, fc, "Processing Error", "");
                            reply = newFaultMessage;
                        }
                        else
                        {
                            XElement document = XElement.Parse(response);

                            var faultNode = document.Descendants().FirstOrDefault(a => a.Name.LocalName == "Fault");

                            if (faultNode != null)
                            {
                                var faultCode = GetText(faultNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "faultcode"));
                                var faultstring = GetText(faultNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "faultstring"));

                                dynamic error = new ExpandoObject();

                                var errorNode = document.Descendants().FirstOrDefault(a => a.Name.LocalName == "Error");
                                if (errorNode != null)
                                {
                                    error.ErrorCode = GetText(errorNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "ErrorCode"));
                                    error.Severity = GetText(errorNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "Severity"));
                                    error.ErrorMessage = GetText(errorNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "ErrorMessage"));
                                    error.ErrorDetail = GetText(errorNode.Descendants().FirstOrDefault(a => a.Name.LocalName == "ErrorDetail"));
                                }

                                FaultCode fc = new FaultCode(faultCode);

                                var fault = MessageFault.CreateFault(fc, new FaultReason(faultstring), error);

                                Message newFaultMessage = Message.CreateMessage(reply.Version, fault, "");
                                reply = newFaultMessage;
                            }
                        }


                    }catch(Exception e)
                    {
                        _log.Error("Error reading Error", e);
                    }

                }
            }

            private string GetText(XElement node)
            {
                return node == null ? null : node.Value;
            }
        }
        /// <summary>
        /// Calls Eurotax Webservice method GetForecast()
        /// </summary>
        /// <param name="request1">Request</param>
        /// <param name="sysId">sysId vom ObTyp</param>
        public void getForecast(ref EurotaxRef.GetForecastRequest1 request1, long sysId)
        {
            try
            {
                _log.Info("Eurotax getForecast Webservice Aufbau Verbindung");
                ForecastClient = new EurotaxRef.ForecastPortDocumentClient();
                // Schreibe Log-Einträge in die Log-Datei und/oder in die LogDump-Tabelle, abhängig von den Flags in web.config
                ForecastClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));
                ForecastClient.Endpoint.Behaviors.Add(new FaultFormatingBehavior());
        /*       Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.GetForecastRequest1 inValue = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.GetForecastRequest1();
                inValue.ETGHeader = request1.ETGHeader;
                inValue.Settings = request1.Settings;
                inValue.Vehicle = request1.Vehicle;

                

                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.GetForecastResponse1 retVal = ((Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ForecastPortDocument)(ForecastClient)).GetForecast(inValue);
                request1.ETGHeader = retVal.ETGHeader;
                request1.Settings = retVal.Settings;
                request1.Vehicle = retVal.Vehicle;*/

                
                DateTime startTime = DateTime.Now;
                _log.Info("Eurotax getForecast Webserviceaufruf gestartet. URL: " + ForecastClient.Endpoint.Address.Uri.OriginalString);
                ForecastClient.GetForecast(ref request1.ETGHeader, ref request1.Settings, ref request1.Vehicle);
                
                _log.Info("Eurotax getForecast Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));
            }
            catch (TimeoutException ex)
            {
                _log.Error("getForecast Webserviceaufruf Timed out. ");
                _log.dumpObject(ex);
                throw new EuroTaxTimeoutException("Timeout bei Eurotax-getForecast", ex);
            }
            catch (EndpointNotFoundException ex)
            {
                _log.Error("Endpunkt nicht gefunden bei getForecast Webserviceaufruf. ");
                _log.dumpObject(ex);
                throw new EuroTaxCallException("Endpunkt nicht gefunden bei Eurotax-getForecast Aufruf", ex);
            }
            catch (CommunicationException ex)
            {
                /* Fehler nur im nicht gruppierten Aufruf verfügbar:
                    MAX_FC_USED_CAR_AGE, MIN_FC_PERIOD, TYPENR NOT FOUND
                */
                EuroTaxErrorType et = EuroTaxErrorType.UNKNOWN;
               
                if(ex is FaultException)
                {
                    FaultException fe = (FaultException) ex;
                    FaultCode fc = fe.Code;
                    if("-42".Equals(fc.Name))
                        et = EuroTaxErrorType.PROCESSING_ERROR;
                }
                _log.Info("Fehler "+ex.Message);
                try
                {
                    object reply = ex.Message;
                    _log.Info("Kommunikationsfehler im Eurotax getForecast Webserviceaufruf: " + et);
                }
                catch (Exception ex2)
                {
                    _log.Info("Kommunikationsfehler im Eurotax getForecast Webserviceaufruf: ", ex2);
                }
                throw new EuroTaxCommException("Kommunikationsfehler bei Eurotax-getForecast", ex, et);
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Eurotax getForecast Webserviceaufruf. "+_log.dumpObject(ex),ex);
                
                throw new Exception("getForecast des Eurotax-Dienstes liefert eine Exception zurück: "+ex.Message, ex);
            }
        }

        /// <summary>
        /// get Valuation
        /// </summary>
        /// <param name="header">type Header</param>
        /// <param name="settings">Settings</param>
        /// <param name="valuation">Valuation</param>
        /// <param name="sysId">sysId vom ObTyp</param>
        public void getValuation(ref EurotaxValuationRef.ETGHeaderType header, ref EurotaxValuationRef.ETGsettingType settings, ref EurotaxValuationRef.ValuationType valuation, long sysId)
        {
            try
            {
                ValuationClient = new EurotaxValuationRef.ValuationSoapPortClient();

                // Schreibe Log-Einträge in die Log-Datei und/oder in die LogDump-Tabelle, abhängig von den Flags in web.config
                ValuationClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Eurotax getValuation Webserviceaufruf gestartet. URL: " + ValuationClient.Endpoint.Address.Uri.OriginalString);
                DateTime startTime = DateTime.Now;
                ValuationClient.GetValuation(ref header, ref settings, ref valuation);
                _log.Info("Eurotax getValuation Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));
            }
            catch (TimeoutException ex)
            {
                _log.Error("getValuation Webserviceaufruf Timed out. ");
                _log.dumpObject(ex);
                throw new EuroTaxTimeoutException("Timeout bei Eurotax-getValuation", ex);
            }
            catch (EndpointNotFoundException ex)
            {
                _log.Error("Endpunkt nicht gefunden bei getValuation Webserviceaufruf. ");
                _log.dumpObject(ex);
                throw new EuroTaxCallException("Endpunkt nicht gefunden bei Eurotax-getValuation Aufruf", ex);
            }
            catch (CommunicationException ex)
            {
                _log.Info("Kommunikationsfehler im Eurotax getValuation Webserviceaufruf. ");
                _log.dumpObject(ex);
                throw new EuroTaxCommException("Kommunikationsfehler bei Eurotax-getValuation", ex, EuroTaxErrorType.UNKNOWN);
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Eurotax getValuation Webserviceaufruf. ");
                _log.dumpObject(ex);
                throw new Exception("getValuation des Eurotax-Dienstes liefert eine Exception zurück", ex);
            }
        }

        /// <summary>
        /// Calls Eurotax Webservice method GetHistoricalForecast()
        /// </summary>
        /// <param name="header">type Header</param>
        /// <param name="settings">Settings</param>
        /// <param name="extendedVehicleType">extended Vehicle type</param>
        public void getHistoricalForecast(EurotaxRef.ETGHeaderType header, EurotaxRef.ETGsettingType settings, EurotaxRef.ExtendedVehicleType extendedVehicleType)
        {
            throw new NotImplementedException();
        }



        public void getVinDecode(ref EurotaxVinRef.VinDecodeRequest request1, ref EurotaxVinRef.VinDecodeOutputType response)
        {

            try
            {
                EurotaxVinRef.VinDecodeRequest requestRval=request1;
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.vinsearchSoapPort, EurotaxVinRef.VinDecodeOutputType> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.vinsearchSoapPort, EurotaxVinRef.VinDecodeOutputType>(1);
                response =  t.call("Eurotax getVinDecode", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.vinsearchSoapPort client)
                {
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeRequest inValue = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeRequest();
                    inValue.ETGHeader = requestRval.ETGHeader;
                    inValue.Request = requestRval.Request;
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.VinDecodeResponse retVal =client.VinDecode(inValue);
                    requestRval.ETGHeader = retVal.ETGHeader;
                    return client.VinDecode(inValue).Result; 
                });
                request1.ETGHeader = requestRval.ETGHeader;

                /*vinClient = new EurotaxVinRef.vinsearchSoapPortClient();

                // Schreibe Log-Einträge in die Log-Datei und/oder in die LogDump-Tabelle, abhängig von den Flags in web.config
                vinClient.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("Eurotax getVinDecode Webserviceaufruf gestartet. URL: " + vinClient.Endpoint.Address.Uri.OriginalString);
                DateTime startTime = DateTime.Now;
                response = vinClient.VinDecode(ref request1.ETGHeader, request1.Request);
                _log.Info("Eurotax getVinDecode Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));*/
            }
            catch (TimeoutException ex)
            {
                _log.Error("getVinDecode Webserviceaufruf Timed out. ",ex);
                
                throw new EuroTaxTimeoutException("Timeout bei Eurotax-getVinDecode",ex);
            }
            catch (EndpointNotFoundException ex)
            {
                _log.Error("Endpunkt nicht gefunden bei getVinDecode Webserviceaufruf. ",ex);
                
                throw new EuroTaxCallException("Endpunkt nicht gefunden bei Eurotax-getVinDecode Aufruf", ex);
            }
            catch (CommunicationException ex)
            {
                _log.Info("Kommunikationsfehler im Eurotax getVinDecode Webserviceaufruf. ",ex);
                
                throw new EuroTaxCommException("Kommunikationsfehler bei Eurotax-getVinDecode", ex, EuroTaxErrorType.UNKNOWN);
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Eurotax getVinDecode Webserviceaufruf. ",ex);
                
                throw new Exception("getVinDecode des Eurotax-Dienstes liefert eine Exception zurück", ex);
            }
        }

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto()
        {
            return this.soapXMLDto;
        }


        /// <summary>
        /// etSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }

    }

    /// <summary>
    /// Exception für Commfehler bei Eurotax
    /// </summary>
    public class EuroTaxCommException : Exception
    {
        private EuroTaxErrorType errorType;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="message">Nachricht</param>
        /// <param name="inner">erzeugende Exception</param>
        /// <param name="errType">EuroTaxErrorType</param>
        public EuroTaxCommException(string message, Exception inner, EuroTaxErrorType errType)
            : base(message, inner)
        {
            this.errorType = errType;
        }

        /// <summary>
        /// getErrorType
        /// </summary>
        /// <returns></returns>
        public EuroTaxErrorType getErrorType()
        {
            return errorType;
        }
    }

    /// <summary>
    /// EuroTaxErrorType enum
    /// </summary>
    public enum EuroTaxErrorType
    {
        /// <summary>
        /// UNKNOWN
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// TYPENR_NOT_FOUND
        /// </summary>
        TYPENR_NOT_FOUND = 1,

        /// <summary>
        /// MAX_FC_USED_CAR_AGE
        /// </summary>
        MAX_FC_USED_CAR_AGE = 2,

        /// <summary>
        /// MIN_FC_PERIOD
        /// </summary>
        MIN_FC_PERIOD = 3,

        /// <summary>
        /// Processing_Error (occurs when blocked call is too big - this might be changed at eurotax sometime to deliver a specific error for the case!)
        /// </summary>
        PROCESSING_ERROR = -5
    }

    /// <summary>
    /// Exception für Aufrufexceptions bei Eurotax
    /// </summary>
    public class EuroTaxCallException : Exception
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="message">Nachricht</param>
        /// <param name="inner">erzeugende Exception</param>
        public EuroTaxCallException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Exception für Aufrufexceptions bei Eurotax
    /// </summary>
    public class EuroTaxTimeoutException : Exception
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="message">Nachricht</param>
        /// <param name="inner">erzeugende Exception</param>
        public EuroTaxTimeoutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}