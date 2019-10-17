using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public class ServiceFaultHandler<X, Y>// where X : System.ServiceModel.ClientBase<X>
    {
        private int RETRYCOUNT = 3;
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public delegate Y ProcessServiceCall(X input);
        /// <summary>
        /// Defaults to 3 retries on error
        /// </summary>
        public ServiceFaultHandler()
        {

        }
        public ServiceFaultHandler(int retrycount)
        {
            this.RETRYCOUNT = retrycount;
        }
        public Y call(String name, Cic.OpenOne.Common.DTO.SoapXMLDto soapXML, ProcessServiceCall c)
        {

            for (int i = 0; i < RETRYCOUNT; i++)
            {

                // AuskuftTypen in die LOGDUMP-Tabelle geschrieben, für die das Feld AuskunftTyp.LOGDUMPFLAG auf True gesetzt ist oder Flag SoapLoggingAuskunftEnabled gesetzt ist.
                Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft beh = new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXML);



                var factory = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.ServiceClientFactory.GetFactory();
                X client = factory.GetClientWithBeh<X>(beh);
                if (client == null)
                    throw new Exception("Client for " + name + " not created");

                ServiceEndpoint endpoint = factory.GetEndpoint<X>(client);

                
                _log.Info(name + " Webserviceaufruf gestartet (" + i + ") url: "+ endpoint.Address.Uri);
                DateTime startTime = DateTime.Now;
                try
                {
                    Y rval = c(client);
                    _log.Info(name + " Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));

                    return rval;

                }
                catch (Exception ze)
                {
                    _log.Warn("Catched Error " + ze.Message + " - retry ");
                    if (i == RETRYCOUNT - 1) throw ze;
                }
            }
            throw new Exception("Retries failed");
        }
    }
}
