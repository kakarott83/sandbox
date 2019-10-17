using System;
using System.Reflection;
using System.ServiceModel;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Service for providing information about the configuration and the service itself
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]

    class StateService : IStateService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Delivers Service State information like response time, db version etc
        /// </summary>
        /// <returns></returns>
        public ServiceInformation DeliverServiceInformation()
        {
            ServiceInformation Info = new ServiceInformation();
            CredentialContext cctx = new CredentialContext();
            try
            {
                StateServiceBo bo = new StateServiceBo();

                bo.getServiceInformation(Info);

                Info.success();
                return Info;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(Info, e, "F_00004_DatabaseUnreachableException");
                return Info;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(Info, e);
                return Info;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(Info, e, "F_00001_GeneralError");
                return Info;
            }
        }

        /// <summary>
        /// delivers the last CHUNKSIZE chars of the current logfile
        /// </summary>
        /// <returns></returns>
        public LogInfo getLogData()
        {
            int CHUNKSIZE = 0;
            LogInfo Info = new LogInfo();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                Info.data = "SETUP.NET - LOG - LOGDATASIZE is 0";

                String defSize = AppConfig.getValueFromDb("SETUP.NET", "LOG", "LOGDATASIZE");
                if (defSize != null)
                {
                    CHUNKSIZE = int.Parse(defSize);
                    if (CHUNKSIZE > 0)
                        Info.data = LogUtil.getLogFileEnd(CHUNKSIZE);
                }
                Info.success();
                return Info;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(Info, e, "F_00004_DatabaseUnreachableException");
                return Info;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(Info, e);
                return Info;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(Info, e, "F_00001_GeneralError");
                return Info;
            }
        }

        /// <summary>
        /// delivers the web.config
        /// </summary>
        /// <returns></returns>
        public ConfigInfo getConfigData()
        {
            ConfigInfo Info = new ConfigInfo();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                Info.data = "SETUP.NET - LOG - CONFIGDATAENABLED is 0";
                String enabled = AppConfig.getValueFromDb("SETUP.NET", "LOG", "CONFIGDATAENABLED");
                if (enabled != null && "1".Equals(enabled))
                {
                    String cfile = @"\..\web.config";
                    byte[] data = FileUtils.loadData(FileUtils.getCurrentPath() + cfile);
                    Info.data = System.Text.Encoding.ASCII.GetString(data);
                }
                Info.success();
                return Info;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(Info, e, "F_00004_DatabaseUnreachableException");
                return Info;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(Info, e);
                return Info;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(Info, e, "F_00001_GeneralError");
                return Info;
            }
        }

        /// <summary>
        /// Flushes the cache
        /// </summary>
        public void flushCache()
        {
            CacheManager.getInstance().flush(0);
            AppConfig.Instance.Init();
        }

        /*
        /// <summary>
        /// Returns the message Header for debug purposes
        /// </summary>
        /// <returns></returns>
        public DefaultMessageHeader getHeaderInfo()
        {
            Cic.OpenOne.Common.Util.SOAP.MessageHeader<DefaultMessageHeader> mh = new Cic.OpenOne.Common.Util.SOAP.MessageHeader<DefaultMessageHeader>();
            DefaultMessageHeader dmh = mh.ReadHeader();
            return dmh;
        }
         */
    }
}