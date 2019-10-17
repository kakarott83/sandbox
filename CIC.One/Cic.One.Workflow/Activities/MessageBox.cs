using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using Cic.One.DTO;
using System.ComponentModel;


namespace Cic.One.Workflow.Activities
{
  
    /// <summary>
    /// Activity that will result in displaying a MessageBox to the user
    /// 
    /// 
    /// </summary>
    public sealed class MessageBox : CodeActivity
    {
        [RequiredArgument]
        public InArgument<WorkflowContext> wfcontext { get; set; }

        
        public InArgument<String> icon { get; set; }
        [RequiredArgument]
        public InArgument<String> text { get; set; }
        public InArgument<String> defaultValue { get; set; }

        public InArgument<int> isInput { get; set; }



        
        protected override void Execute(CodeActivityContext context)
        {
             
            WorkflowContext ctx = wfcontext.Get(context);
            ctx.prompt = new GUIPromptDto();
            ctx.prompt.title = this.DisplayName;
            ctx.prompt.icon = icon.Get(context);
            ctx.prompt.text = text.Get(context);
            ctx.prompt.defaultValue = defaultValue.Get(context);
            ctx.prompt.isInput = isInput.Get(context);

        }

       
    }
}
