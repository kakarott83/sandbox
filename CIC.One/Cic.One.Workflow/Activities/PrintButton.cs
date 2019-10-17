using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using Cic.One.DTO;
using System.ComponentModel;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;


namespace Cic.One.Workflow.Activities
{

   

    /// <summary>
    /// Activity that will result in displaying a Button in the GUI to control that Workflow
    /// 
    /// The button has a mandatory Text (the Activity-Name) and a mandatory command that will be sent back
    /// 
    /// in the returning workflow-context as inputCommand
    /// 
    /// </summary>
    public sealed class PrintButton : CodeActivity
    {
        //designer-properties
        [RequiredArgument]
        public InArgument<WorkflowContext> wfcontext { get; set; }
        public InArgument<int> disabled { get; set; }
        public InArgument<int> type { get; set; }
        public InArgument<int> persist { get; set; }

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String CMD_PRINT = "PRINT_CICONE";

        [RequiredArgument]
        public InArgument<String> docarea { get; set; }
        public List<PrintDocumentDto>  templates { get; set; }//holds the templates available at design time, depended of the user-selected docarea

        /// <summary>
        /// executes a button during the runtime of a worklfow
        /// fetches the button from the context and 
        /// </summary>
        /// <param name="context"></param>
        protected override void Execute(CodeActivityContext context)
        {
             
            WorkflowContext ctx = wfcontext.Get(context);
            if(ctx.buttons==null)
                ctx.buttons = new List<WorkflowButtonDto>();
            
            
            WorkflowButtonDto button = null;
            try
            {
                int curtype = type.Get(context);
                //find the corresponding WorkflowButtonDto from the context that is responsible for this activity
               
                //Only one button with the same command is allowed at the same time!

                button = ctx.buttons.Where(a => a.type == curtype && a.command.Equals(CMD_PRINT)).FirstOrDefault();
                if (button == null)
                {
                    button = new WorkflowButtonDto();
                    button.items = new List<WorkflowButtonDto>();
                    //add sub-structure for button menu with all templates
                    foreach(PrintDocumentDto pd in templates)
                    {
                        WorkflowButtonDto subbutton = new WorkflowButtonDto();
                        subbutton.text = pd.title;
                        subbutton.command = pd.code;//we use the template code as command of the subbutton
                        subbutton.area = pd.area;//we use the document area also as the area for the print-call
                        button.items.Add(subbutton);
                    }
                    ctx.buttons.Add(button);
                    
                }
                button.text = this.DisplayName;
                button.command =CMD_PRINT;
                button.disabled = disabled.Get(context);
                button.area = docarea.Get(context);//we use the document area also as the area for the print-call
                button.type = 4;//Menu-Button
                button.persist = persist.Get(context);
            }
            catch (Exception e)
            {
                _log.Error("WorkflowButton failure (probably expression evaluated to null)", e);
            }
        }

       
    }
}
