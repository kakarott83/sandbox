using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Security;
using System.Runtime.Serialization;

namespace Cic.One.DTO
{
    /// <summary>
    /// Basic Workflow Object holding all variables used in the workflows
    /// </summary>
    public class WorkflowContext
    {
        /// <summary>
        /// The Workflow view id to display in the GUI, set by the workflow by the customizer
        /// </summary>
        public String wfvid {get;set;}
        /// <summary>
        /// the configured view id to forward on workflow end, if any is defined
        /// </summary>
        public String wfvforwardid { get; set; }

        //GUI-User-Input, used by workflow when control goes back to workflow to determine what the user choose
        //Startpoint in workflow (BPE)
        public String inputCommand { get; set; }
        //Command from Workflow to the GUI to exec when wf-call resumes in GUI
        public String outputCommand { get; set; }
        //js commands from WF for the GUI
        public String[] jsCommands { get; set; }

        //Workflow-GUI-Control Output
        public List<WorkflowButtonDto> buttons {get;set;}
        //Generic GUI Field descriptions
        public List<Viewfield> inputfields { get;set;}

        public GUIPromptDto prompt { get; set; }

        //temporary pointer for WF4-Engine to access the workflow-Dao from BOS-class
        [IgnoreDataMember]
        public IocBos iocBos { get; set; }
        

        /// <summary>
        /// Contains all entities a workflow can work with
        /// </summary>
        public EntityContainer entities { get; set; }
        /// <summary>
        /// List of messages to display in the GUI
        /// </summary>
        public List<WorkflowMessageDto> messages { get; set; }
        /// <summary>
        /// Complementary List of variables to use in workflow/gui
        /// </summary>
        public ContextVariableDto[] context { get; set; }

        /// <summary>
        /// Only used internally
        /// </summary>
        public ContextVariableDto[] contextInternal { get; set; }

        public CASEvaluateResult[] casResults {get;set;}

        /// <summary>
        /// area the wf is working with e.g. VT, ANGEBOT
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// id of area the wf is working with
        /// </summary>
        public String areaid { get; set; }

        public long sysWFUSER {get;set;}
        public long sysPEROLE { get; set; }

        public String isocode { get; set; }

        /// <summary>
        /// Mandant
        /// </summary>
        public long syslsadd { get; set; }

        /// <summary>
        /// Workflow-Results
        /// </summary>
        public List<String> results {get;set;}

        /// <summary>
        /// BPE Process instance id
        /// </summary>
        [IgnoreDataMember]
        public long sysbpprocinst { get; set; }

        /// <summary>
        /// BPListener-Info
        /// </summary>
        public BPListenerDto bplistener { get; set; }

        /// <summary>
        /// when set to true during BOS-Evaluation (like deeplink) the BPE will be forced to continue
        /// </summary>
        [IgnoreDataMember]
        public bool continueWF { get; set; }

        public WorkflowContext()
        {
            context =new ContextVariableDto[0];
            entities = new EntityContainer();
            messages = new List<WorkflowMessageDto>();
        }

    }
}
