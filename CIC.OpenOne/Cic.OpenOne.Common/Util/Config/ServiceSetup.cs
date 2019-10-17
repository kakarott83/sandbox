using Cic.OpenOne.Common.Util.Logging;
using Devart.Data.Oracle.Entity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.Util.Config
{
    /// <summary>
    /// Class initializing a Webservice Project
    ///  * DEVART
    ///  * SSL Certificate Handling for dev servers
    /// </summary>
    public class ServiceSetup
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceSetup()
        {
            initDevart();
            initSecurity();
        }

        /// <summary>
        /// initializes the Devart driver behaviour
        /// </summary>
        private void initDevart()
        {
           
            // Turn on the Batch Updates mode:
            var config = OracleEntityProviderConfig.Instance;
            config.DmlOptions.BatchUpdates.Enabled = true;

            // If necessary, enable the mode of re-using parameters with the same values:
            config.DmlOptions.ReuseParameters = true;

            // If object has a lot of nullable properties, and significant part of them are not set (i.e., nulls), omitting explicit insert of NULL-values will decrease greatly the size of generated SQL:
            // config.DmlOptions.InsertNullBehaviour = InsertNullBehaviour.Omit;
            config.SqlFormatting.Disable();
        }

        /// <summary>
        /// initializes the ssl certificate handling
        /// </summary>
        private void initSecurity()
        {
            //disable unsecure sslv3 protocol:
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
        }

        /// <summary>
        ///     Trust local SSL Certs to IBANKernel
        /// </summary>
        private static bool RemoteCertificateValidate(
            object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors policyErrors)
        {
            if (policyErrors != SslPolicyErrors.None)
            {
                if (sender is HttpWebRequest && chain.ChainStatus.Length > 0)
                {
                    _log.Warn("Connection to " + ((HttpWebRequest)sender).RequestUri.AbsolutePath + " with Status " + policyErrors + " and " + chain.ChainStatus[0].Status);
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("IBANKernel") > -1 &&
                         (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot || chain.ChainStatus[0].Status == X509ChainStatusFlags.PartialChain))
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("CASKernel") > -1 &&
                        (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot || chain.ChainStatus[0].Status == X509ChainStatusFlags.PartialChain))
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("MediatorService") > -1 &&
                        (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot || chain.ChainStatus[0].Status == X509ChainStatusFlags.PartialChain))
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("SimpleService") > -1 &&
                       (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot || chain.ChainStatus[0].Status == X509ChainStatusFlags.PartialChain))
                        return true;
                }
                else if (sender is HttpWebRequest && policyErrors != SslPolicyErrors.None)
                {
                    _log.Warn("Connection to " + ((HttpWebRequest)sender).RequestUri.AbsolutePath + " with Status " + policyErrors);
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("IBANKernel") > -1)
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("CASKernel") > -1)
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("MediatorService") > -1)
                        return true;
                    if (((HttpWebRequest)sender).RequestUri.AbsolutePath.IndexOf("SimpleService") > -1)
                        return true;
                }
            }

            if (policyErrors == SslPolicyErrors.None)
                return true;

            _log.Error("Failure validating Certificate: " + policyErrors + " " + chain + " " + cert + " " + sender);
            return false;
        }

      
    }
}
