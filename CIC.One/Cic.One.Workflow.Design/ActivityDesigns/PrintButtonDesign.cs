using System.Activities;
using System.Windows;
using System.Activities.Presentation;
using Cic.One.Workflow.Activities;
using Cic.One.DTO;
using Microsoft.VisualBasic.Activities;
using Cic.One.Workflow.Icons;
using System;

namespace Cic.One.Workflow.Design.ActivityDesigns
{
     /// <summary>
    /// This class allows to inject a default value expression into the designer upon creation of a new activity of this type
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(IconResourceAnchor), "CICOne.png")]
    public class PrintButton : IActivityTemplateFactory
    {
        /// <summary>
        /// When the user drags the PrintButton from toolbox, creates the activity returned here
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Activity Create(DependencyObject target)
        {
            return new Cic.One.Workflow.Activities.PrintButton
            {
                wfcontext = new InArgument<WorkflowContext>(new VisualBasicValue<WorkflowContext>("input"))
            };
        }
    }
}
