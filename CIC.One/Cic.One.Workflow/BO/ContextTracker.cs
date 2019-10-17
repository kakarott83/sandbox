using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Tracking;
using Cic.One.DTO;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// TrackingRecord that contains the current workflow Context
    /// </summary>
    public class ContextTracker : CustomTrackingRecord
    {
        public WorkflowContext wfcontext { get; set; }
        public ContextTracker(WorkflowContext ctx)
            : base("context")
        {
            this.wfcontext = ctx;
        }
        
    }
}
