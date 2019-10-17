using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    ///  Workflow-Engine output
    /// </summary>
    public class oprocessWorkflowDto : oBaseDto
    {
        /// <summary>
        /// The GUID of the current workflow state to resume later
        /// will be empty if workflow was finihed during last call
        /// </summary>
        public Guid? workflowId { get; set; }

        /// <summary>
        /// BPE Listener id
        /// </summary>
        public long sysbplistener { get; set; }

        /// <summary>
        /// The current workflow Context state
        /// </summary>
        public WorkflowContext workflowContext {get;set;}

        
    }
}
