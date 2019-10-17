using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Activities;
using System.Activities.XamlIntegration;
using Cic.OpenOne.Common.Util;
using System.Xaml;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using System.IO;
using Cic.One.Workflow.DAO;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// Factory for WF4 Workflow Activities
    /// </summary>
    public class WorkflowFactory
    {
        private static volatile WorkflowFactory _self = null;
        private static readonly object InstanceLocker = new Object();
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WorkflowDao dao;

        public static WorkflowFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new WorkflowFactory();
                }
            }
            return _self;
        }

        public WorkflowFactory()
        {
            this.dao = new WorkflowDao();
        }
        /// <summary>
        /// Delivers a DB based wfvEntry from the syscode or NULL
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public System.IO.Stream getWfvConfig(String syscode)
        {
            return new MemoryStream(getWfvConfigBytes(syscode));
        }

        /// <summary>
        /// returns the einrichtung as utf8-byte for the wfv syscode
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public byte[] getWfvConfigBytes(String syscode)
        {
            return System.Text.Encoding.UTF8.GetBytes(getWfvEinrichtung(syscode));
        }

        /// <summary>
        /// deliver the einrichtung string for the wfv syscode
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public String getWfvEinrichtung(String syscode)
        {

            return dao.getWfvEinrichtung(syscode);
        }

        /// <summary>
        /// Returns an instance of the given workflow and workflow revision
        /// throws an Exception if workflow not found
        /// </summary>
        /// <param name="name">the wfv syscode</param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public Activity getWorkflow(String name, int revision)
        {

           

            name = name.Replace("/", "").Replace("\\", "").Replace(".", "");
            String path = "";
            if (revision > 0)
            {
                path = FileUtils.getCurrentPath() + @"\..\Workflows\" + name + "_" + revision + ".xaml";
                if (!File.Exists(path))
                {
                    path = FileUtils.getCurrentPath() + @"\..\..\Workflows\" + name + "_" + revision + ".xaml";
                }
            }
            else
            {
                path = FileUtils.getCurrentPath() + @"\..\Workflows\" + name + ".xaml";
                if (!File.Exists(path))
                {
                    path = FileUtils.getCurrentPath() + @"\..\..\Workflows\" + name + ".xaml";
                }
            }

            try
            {
                //ATTENTION, this is only needed when workflow is inside the same assembly, when outside, local: wont be inside the xaml!
                return ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(path, new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));
                
                //return ActivityXamlServices.Load(path);
            }
            catch (Exception)
            {
                try
                {
                    return ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(getWfvConfig(name), new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));
                }
                catch (Exception ex)
                {
                    _log.Debug("Workflow " + name + " version " + revision + " not found in Database, Reason: "+ex.Message+" Fallback 1 local...");
                    try
                    {
                        path = FileUtils.getCurrentPath() + @"\..\Workflows\DefaultWorkflow.xaml";
                        if (!File.Exists(path))
                        {
                            path = FileUtils.getCurrentPath() + @"\..\..\Workflows\DefaultWorkflow.xaml";
                        }
                        //return ActivityXamlServices.Load(path);
                        return ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(path, new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));

                    }
                    catch (Exception e)
                    {

                        _log.Debug("Workflow " + name + " version " + revision + " not found locally, Reason: " + e.Message + " Fallback Workflow.dll");

                            try
                            {
                                return (Activity)Activator.CreateInstance("Workflows", "Workflows." + name).Unwrap();
                            }
                            catch (Exception)
                            {
                                _log.Warn("Internal Workflow not found: " + name + " (reason: " + e.Message + ")");
                                throw new Exception("Internal Workflow not found: " + name );
                                
                            }
                           
                        
                        
                    }
                }
               
            }
         
        }
    }
}