using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;

    using BO;

    using DTO;

    using Newtonsoft.Json;

    using OpenOne.Common.Util.Logging;

    public interface IDMRDao
    {
        DMROutputDto sendToDmr(DMRConfig config, DMRInputDto inputDto);
    }

    public class DMRDao : IDMRDao
    {
        private ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DMROutputDto sendToDmr(DMRConfig config, DMRInputDto inputDto)
        {
            var inputRequest = JsonConvert.SerializeObject(inputDto);
            Debug.WriteLine(inputRequest);
            log.Debug("Request: "+ inputRequest);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(config.Url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            
            HttpWebResponse httpResponse = null;
            string errorMessage = null;

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
                    var result = JsonConvert.DeserializeObject<DMROutputDto>(resultString);
                    result.StatusCode = (long)httpResponse.StatusCode;
                    return result;
                }
            }
            catch (WebException e)
            {
                log.Error("Could not send request to DMR", e);
                httpResponse = (HttpWebResponse)e.Response;
                errorMessage = e.Message;
            }
            catch (Exception e)
            {
                log.Error("Could not send request to DMR", e);
                errorMessage = e.Message;
            }

            if (httpResponse != null)
            {
                return new DMROutputDto()
                {
                    StatusCode = (long)httpResponse.StatusCode,
                    ErrorMessage = errorMessage
                };
            }

            return new DMROutputDto()
            {
                StatusCode = -1,
                ErrorMessage = errorMessage
            };
        }
    }
}
