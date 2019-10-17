using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public enum WorkflowType
    {
        //WorkflowFoundation
        WF4=0,
        //BPEL Engine
        BPE=1
    }

    /// <summary>
    /// Workflow-Engine input
    /// </summary>
    public class iprocessWorkflowDto
    {
        /// <summary>
        /// GUID of workflow, if null a new workflow will be started, if set the workflow with this id will be continued
        /// </summary>
        public Guid? workflowId { get; set; }
        
        /// <summary>
        /// Name of the wf4 workflow (template) or name of the process (BPE)
        /// </summary>
        public String workflowName {get;set;}

        /// <summary>
        /// Type of workflow to use
        /// </summary>
        public WorkflowType type { get; set; }

        /// <summary>
        /// Revision of the workflow
        /// </summary>
        public int workflowVersion { get; set; }

        /// <summary>
        /// If true, the workflow-engine will use the workflowContext of this Dto to overwrite all Engine Context values
        /// </summary>
        public bool overwriteContext { get; set; }

        /// <summary>
        /// The workflow Context to work with, if null the workflow will continue with the previously stored valued upon bookmark
        /// Must not be null when starting a new Workflow (when workflowId is null)
        /// </summary>
        public WorkflowContext workflowContext { get; set; }

        /// <summary>
        /// BPE Listener id
        /// </summary>
        public long sysbplistener { get; set; }
    }
}
