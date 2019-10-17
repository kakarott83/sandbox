using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Reflection;

using System.Timers;

using System.IO;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Service.DTO;
using Cic.OpenOne.Common.Util.Security;

namespace Cic.OpenOne.Service
{
    /// <summary>
    /// Global Application Listener
    /// </summary>
    public class Global : System.Web.HttpApplication
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String APPNAME = "Cic.OpenOne.Service";
        public static Otis.Configuration OTISCONFIG;

        protected void Application_Start(object sender, EventArgs e)
        {
            _log.Info("Deploying " + APPNAME + "...");
            //_log.Info("Database-Connection: " + Configuration.DeliverOpenLeaseConnectionString());
            OTISCONFIG = new Otis.Configuration();            // instantiate a new Configuration, one per application is needed
            //OTISCONFIG.AddType(typeof(iSampleMethodDto));                       // initialize it using type metadata, but easier is 
            
            OTISCONFIG.AddAssembly(Assembly.GetExecutingAssembly()); // to register all types at once
            string CnstBlowfishKey = "C.I.C.-S0ftwareGmbH1987Muenchen0";
            Blowfish b = new Blowfish(CnstBlowfishKey);
            String test =b.Encode("wimpern9");
        }



        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            _log.Debug(APPNAME + " App Warning: ", Server.GetLastError());
            _log.Debug("Reason: ", Server.GetLastError().InnerException);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            _log.Info("Undeploying " + APPNAME + "...");
        }
    }
}