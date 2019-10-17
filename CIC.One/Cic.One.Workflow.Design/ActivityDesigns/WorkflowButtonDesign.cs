using System.Activities;
using System.Windows;
using System.Activities.Presentation;
using Cic.One.Workflow.Activities;
using Cic.One.DTO;
using Microsoft.VisualBasic.Activities;
using Cic.One.Workflow.Icons;
using System.Configuration;
using System;

namespace Cic.One.Workflow.Design.ActivityDesigns
{
     /// <summary>
    /// This class allows to inject a default value expression into the designer upon creation of a new activity of this type
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(IconResourceAnchor), "CICOne.png")]
    public class WorkflowButton : IActivityTemplateFactory
    {
        public Activity Create(DependencyObject target)
        {
           // Cic.OpenOne.Common.Util.Config.Configuration.setDBSettings();
          /*  Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.GetSection("applicationSettings");
            String t = config.GetSection("applicationSettings").CurrentConfiguration.GetSection.Sections["Cic.OpenOne.Common.Properties.Config"].CurrentConfiguration.AppSettings["OpenLeaseConnectionStringDataSource"];
            */
            return new Cic.One.Workflow.Activities.WorkflowButton
            {
                wfcontext = new InArgument<WorkflowContext>(new VisualBasicValue<WorkflowContext>("input")),
                //DisplayName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
                
                //DisplayName = Cic.OpenOne.Common.Util.Config.Configuration.DeliverOpenLeaseConnectionString()
                //DisplayName = "TEST: "+t
            };
        }
    }
}
