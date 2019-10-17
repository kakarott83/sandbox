using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using System.Web;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using Cic.One.Utils.Util.Behaviour;

namespace Cic.One.GateBANKNOW
{
    /// <summary>
    ///     Dummyclass for reflection access to this assemblys'Type
    /// </summary>
    public class VersionHandle
    {
    }

   
    /// <summary>
    ///     Global
    /// </summary>
    public class Global : HttpApplication
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      
       
        /// <summary>
        /// While usually Global.asax is the initial entry point in a web application, when this class-Library is used as Reference in another web application, the listeners here wont
        /// be called, so we register a call to this method in AssemblyInfo
        /// </summary>
        public static void Start()
        {
            _log.Info("Deploying GateBANKNOW");
            try
            {

             /*   WsdlExporter exporter = new WsdlExporter();
                //or
                //public void ExportContract(WsdlExporter exporter, 
                // WsdlContractConversionContext context) { ... }
                object dataContractExporter;
                XsdDataContractExporter xsdInventoryExporter;
                if (!exporter.State.TryGetValue(typeof(XsdDataContractExporter),
                    out dataContractExporter))
                {
                    xsdInventoryExporter = new XsdDataContractExporter(exporter.GeneratedXmlSchemas);
                }
                else
                    xsdInventoryExporter = (XsdDataContractExporter)dataContractExporter;
                exporter.State.Add(typeof(XsdDataContractExporter), xsdInventoryExporter);
                


                if (xsdInventoryExporter.Options == null)
                    xsdInventoryExporter.Options = new ExportOptions();
                xsdInventoryExporter.Options.DataContractSurrogate = new StringSurrogated();
                */

                Mapper.AddProfile<MappingProfile>();
                Mapper.AddProfile<Cic.OpenOne.GateBANKNOW.Common.DTO.BankNowModelProfile>();
            }catch(Exception ex)
            { }
            
        }
        /// <summary>
        ///     Application Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs evt)
        {
            _log.Info("Deploying GateBANKNOW");
            
        }

       
        /// <summary>
        ///     Session Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Begin Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Authentication Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Session End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Application End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }

}