using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Behaviour;
//using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// 
    /// </summary>
    public class RISKEWBS1WSDao : IRISKEWBS1WSDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        RISKEWBS1Ref.CicServiceClient Client;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CallSSS1Test()
        {
            Client = new RISKEWBS1Ref.CicServiceClient();
            _log.Info("cicSSS1Test Webserviceaufruf gestartet.");
            DateTime startTime = DateTime.Now;
            string response = Client.Test();
            _log.Info("cicSSS1Test Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inputData"></param>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        public long CallSSS1CalculateWB(long sysAuskunft, string inputData, long sysWFUser)
        {
            
            Client = new RISKEWBS1Ref.CicServiceClient();
            //cctx.
            RISKEWBS1Ref.SsInputParams ssInputDto = new RISKEWBS1Ref.SsInputParams();
            ssInputDto.SysAuskunft = sysAuskunft;
            //ssInputDto.SysWfUser = sysWFUser;
            //ssInputDto.Pwd = "XAKLOP901ASDDDA";
            //ssInputDto.InputString = inputData;

            _log.Info("cicSSS1CalculateWB Webserviceaufruf gestartet.");
            DateTime startTime = DateTime.Now;
            //Client.CalculateWB(sysAuskunft,inputStream);
            _log.Info("cicSSS1CalculateWB Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));

            return 0;
        }
    }
}
