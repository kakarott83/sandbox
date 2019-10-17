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

    public enum ButtonType
    {  //buttonbar-button, id=0
        COMMAND,
        INFORMATION,
        OVERLAY,
        HELP,
        MENU
    }

    /// <summary>
    /// Activity that will result in displaying a Button in the GUI to control that Workflow
    /// 
    /// The button has a mandatory Text (the Activity-Name) and a mandatory command that will be sent back
    /// 
    /// in the returning workflow-context as inputCommand
    /// 
    /// </summary>
    public class WorkflowButton : CodeActivity
    {
        [RequiredArgument]
        public InArgument<WorkflowContext> wfcontext { get; set; }
        [RequiredArgument]
        public InArgument<String> command { get; set; }
        public InArgument<String> desc1 { get; set; }
        public InArgument<String> desc2 { get; set; }
        public InArgument<String> desc3 { get; set; }
        public InArgument<String> area { get; set; }
        public InArgument<String> areaid { get; set; }
        public InArgument<String> icon { get; set; }
        [RequiredArgument]
        public ButtonType buttonType { get; set; }
        public InArgument<int> disabled { get; set; }
        public InArgument<int> type { get; set; }
        public InArgument<int> persist { get; set; }

        public String test { get; set; }
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                if (area.Get(context) != null && areaid.Get(context) != null)
                {//Only one button with the same area/areaid is allowed at the same time!
                    button = ctx.buttons.Where(a => a.type == curtype && a.area.Equals(area.Get(context)) && a.areaid.Equals(areaid.Get(context))).FirstOrDefault();
                }
                else
                {
                    //Only one button with the same command is allowed at the same time!
                    button = ctx.buttons.Where(a => a.type == curtype && a.command.Equals(command.Get(context))).FirstOrDefault();
                }
                bool newButton = false;
                if (button == null)
                {
                    button = new WorkflowButtonDto();
                    ctx.buttons.Add(button);
                    newButton = true;
                }
                button.text = this.DisplayName;

                button.disabled = disabled.Get(context);
                button.command = command.Get(context);
                button.desc1 = desc1.Get(context);
                button.desc2 = desc2.Get(context);
                button.desc3 = desc3.Get(context);
                button.area = area.Get(context);
                button.areaid = areaid.Get(context);
                button.icon = icon.Get(context);
                if (buttonType == ButtonType.COMMAND)
                    button.type = 0;
                else if (buttonType == ButtonType.INFORMATION)
                    button.type = 1;
                if (buttonType == ButtonType.OVERLAY)
                    button.type = 2;
                if (buttonType == ButtonType.HELP)
                    button.type = 3;
                if(newButton)
                    button.persist = persist.Get(context);
                else if(button.persist==0)//when button is already in persist-mode from outside, dont change that
                    button.persist = persist.Get(context);
            }
            catch (Exception e)
            {
                _log.Error("WorkflowButton failure (probably expression evaluated to null)", e);
            }
        }

       
    }
}
