using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using Cic.One.DTO;

namespace Cic.One.Workflow.Activities
{
    /// <summary>
    /// Activity that adds a Message-String in the workflow-contexts message list
    /// </summary>
    public sealed class AddMessage : CodeActivity
    {
        [RequiredArgument]
        public InArgument<WorkflowContext> wfcontext { get; set; }
        [RequiredArgument]
        public InArgument<String> message { get; set; }
        public InArgument<int> persist { get; set; }
        public InArgument<int> type { get; set; }
        

        protected override void Execute(CodeActivityContext context)
        {
             
            WorkflowContext ctx = wfcontext.Get(context);
            if(ctx.messages==null)
                ctx.messages = new List<WorkflowMessageDto>();
            WorkflowMessageDto msg = new WorkflowMessageDto();
            msg.message = message.Get(context);
            msg.persist = persist.Get(context);
            msg.title = this.DisplayName;
            msg.type = type.Get(context);
            
            
            ctx.messages.Add(msg);
            
        }

       
    }
}
