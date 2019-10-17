using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EQUIFAX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
   
    public class EQUIFAXInDto
    {
        public String idType { get; set; }
        public String idCode { get; set; }
        public String postalCode { get; set; }
        public DateTime? dateofBirth { get; set; }

        public long sysauskunft { get; set; }

    }
    public class EQUIFAXOutDto
    {
        public String errorMessage { get; set; }
        public RISK data { get; set; }
        public String errorCode { get; set; }

        public String transactionId { get; set; }
        public String transactionState { get; set; }
        public String timestamp { get; set; }
    }
    public class EQUIFAXDao : IEQUIFAXDao
    {
        private ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sends data to InterCOnnect Equifax Service to receive risk data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EQUIFAXOutDto requestRiskData(AuskunftCFGDto config, EQUIFAXInDto input)
        {
            InterConnectInputDto inputDto = new InterConnectInputDto(input.idType, input.idCode);
            //inputDto.applicants.primaryConsumer.personalInformation.postalCode = input.postalCode;
            inputDto.applicants.primaryConsumer.personalInformation.addresses = new List<Address>();
            inputDto.applicants.primaryConsumer.personalInformation.addresses.Add(new Address());
            inputDto.applicants.primaryConsumer.personalInformation.addresses[0].postalCode = input.postalCode;
            if (input.dateofBirth.HasValue)
                inputDto.applicants.primaryConsumer.personalInformation.addresses[0].dateOfBirth = input.dateofBirth.Value.ToString("yyyyMMdd");

            var inputRequest = JsonConvert.SerializeObject(inputDto);
            log.Debug("Request: " + inputRequest);
            //as of .net 4.7 the defaults are the best settings with the strongest sec protocol  choosen by the os
            //System.Net.ServicePointManager.Expect100Continue = true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(config.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Headers.Add("dptOrchestrationCode", config.dataobject);
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(config.username + ":" + config.keyvalue));
            httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);

            HttpWebResponse httpResponse = null;
            
            EQUIFAXOutDto rval = new EQUIFAXOutDto();
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(inputRequest);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var resultString = streamReader.ReadToEnd();
                    log.Debug("Response: " + resultString);
                    InterConnectInputDto result = JsonConvert.DeserializeObject<InterConnectInputDto>(resultString);
                    rval.errorCode = null;

                    rval.transactionId = result.transactionId;
                    rval.transactionState = result.transactionState;
                    rval.timestamp = result.timestamp;
                    if (result.transactionState != null && result.transactionState.IndexOf("ERROR") > -1)
                    {
                        foreach (EquifaxError err in result.errors)
                        {
                            if (rval.errorCode == null)
                            {
                                rval.errorCode = err.code;
                                rval.errorMessage = err.message;
                            }
                            log.Warn("Equifax-Transaction-Error: " + err.code + ":" + err.message);
                        }
                    }
                    if (result.applicants.primaryConsumer.dataSourceResponses!=null && result.applicants.primaryConsumer.dataSourceResponses.errors != null && result.applicants.primaryConsumer.dataSourceResponses.errors.Count > 0)
                    {
                        foreach (EquifaxError err in result.applicants.primaryConsumer.dataSourceResponses.errors)
                        {
                            log.Warn("Equifax-Datasource-Error: " + err.code + ":" + err.message);
                        }
                    }
                    if (result.applicants.primaryConsumer.dataSourceResponses != null && result.applicants.primaryConsumer.dataSourceResponses.EIPG!=null && result.applicants.primaryConsumer.dataSourceResponses.EIPG.RISK!=null)
                    {
                        rval.data= result.applicants.primaryConsumer.dataSourceResponses.EIPG.RISK;
                        rval.errorCode = rval.data.returnCode;
                        if ("000".Equals(rval.errorCode))
                            rval.errorMessage = "Error from Remote Service";
                     }
                    
                    
                }
            }
            catch (WebException e)
            {
                log.Error("Could not send request to EQUIFAX", e);
                httpResponse = (HttpWebResponse)e.Response;
                rval.errorCode = "177";
                if(httpResponse!=null)
                    rval.errorCode=""+ httpResponse.StatusCode;
                rval.errorMessage = "Could not send request to EQUIFAX: "+e.Message;
            }
            catch (Exception e)
            {
                log.Error("Could not send request to EQUIFAX", e);
                rval.errorMessage = "Could not send request to EQUIFAX: "+e.Message;
                rval.errorCode = "177";
            }
            return rval;
        }

    }
}
