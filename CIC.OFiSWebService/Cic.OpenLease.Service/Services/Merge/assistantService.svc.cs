using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Cic.OpenLease.ServiceAccess.Merge.DTO;


using System.Reflection;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util.Exceptions;

using Cic.OpenLease.ServiceAccess.Merge.Contract;

using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenLease.Service.Merge
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "assistantService" in code, svc and config file together.
    /// <summary>
    /// Service for providing information about the configuration and the service itself
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/contract")]
    public class assistantService : IassistantService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Liefert alle PIDs zu denen es aktuelle Logfiles gibt
        /// </summary>
        /// <returns>ogetLogFilePidDto</returns>
        public ogetLogFilePidDto getLogFilePid()
        {
            ogetLogFilePidDto rval = new ogetLogFilePidDto();
            CredentialContext cctx = new CredentialContext();
            assistantServiceBo bo = new assistantServiceBo();
            try
            {
                rval.pids = bo.getLogFilePids(LogUtil.getFileAppenderFile());

                return rval;
            }

            catch (Exception e)//unhandled exception - should not happen!
            {
                _log.Error("getLogFilePid failed", e);
                throw e;
            }
        }

        /// <summary>
        /// Liefert ein Logfile oder teile aus einem Logfile
        /// </summary>
        /// <param name="input">igetLogFileDto</param>
        /// <returns>ogetLogFileDto</returns>
        public ogetLogFileDto getLogFile(igetLogFileDto input)
        {
            ogetLogFileDto rval = new ogetLogFileDto();
            int CHUNKSIZE = 1024000;
            string logFile = LogUtil.getFileAppenderFile();
            CredentialContext cctx = new CredentialContext();
            assistantServiceBo bo = new assistantServiceBo();
            try
            {
                if (input.pid > 0)
                {
                    int[] pids = bo.getLogFilePids(LogUtil.getFileAppenderFile());
                    if (pids != null && pids.Length > 0)
                    {
                        bool foundPid = false;
                        for (int i = 0; i < pids.Length; i++)
                        {
                            if (pids[i].ToString().Contains(input.pid.ToString()))
                            {
                                logFile = logFile.Substring(0, logFile.IndexOf("-[") + 2) + input.pid + "].log";
                                foundPid = true;
                            }
                        }
                        if (!foundPid)
                        {
                            throw new ArgumentException("Es wurde kein Logfile zur PID: " + input.pid + " gefunden");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Es wurden keine Logfiles gefunden");
                    }
                    rval.pid = input.pid;
                    rval.requests = bo.getLogFile(LogUtil.getLogFileEnd(CHUNKSIZE, logFile), input);

                    return rval;
                }
                else
                {
                    rval.pid = input.pid;
                    rval.requests = bo.getLogFile(LogUtil.getLogFileEnd(CHUNKSIZE), input);

                    return rval;
                }
            }

            catch (Exception e)//unhandled exception - should not happen!
            {
                _log.Error("getLogFile failed", e);
                throw e;
            }
        }

        /// <summary>
        /// Liefert die default PID vom Logfile
        /// </summary>
        /// <returns></returns>
        public ogetLogFileDefaultPid getLogFileDefaultPid()
        {
            string logFile = LogUtil.getFileAppenderFile();
            CredentialContext cctx = new CredentialContext();
            assistantServiceBo bo = new assistantServiceBo();
            ogetLogFileDefaultPid rval = new ogetLogFileDefaultPid();
            try
            {
                if (logFile.Contains("-[") && logFile.Contains("]"))
                {
                    logFile = logFile.Substring(logFile.IndexOf("-[") + 2, logFile.IndexOf("]") - (logFile.IndexOf("-[") + 2));
                    rval.pid = Convert.ToInt32(logFile);

                    return rval;
                }
                else
                {
                    rval.pid = 0;

                    return rval;
                }
            }

            catch (Exception e)//unhandled exception - should not happen!
            {
                _log.Error("getLogFileDefaultPid failed", e);
                throw e;
            }
        }
    }
}
