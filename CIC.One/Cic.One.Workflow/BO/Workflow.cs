using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading;
using System.Activities.Tracking;
using System.IO;
using Cic.One.DTO;
using System.Activities.Hosting;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Devart.Data.Oracle.DurableInstancing;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// WF4 Workflow for synchronous calling/resuming a WF4 Workflow Application
    /// Allows tracking of the internal workflow context variables
    /// 
    /// Designer:
    /// http://www.codeproject.com/Articles/375034/A-dynamic-Rehosted-Workflow-Designer-for-WF-4
    /// http://social.msdn.microsoft.com/Forums/vstudio/en-US/07f89fc0-81ff-4e85-a974-013b15b62f50/rehosting-question-intellisense-support?forum=wfprerelease
    /// http://blog.actiprosoftware.com/post/2013/08/26/Enhancing-the-Windows-Workflow-Designer.aspx
    /// </summary>
    public class Workflow
    {
        public Guid wfid = Guid.Empty;
        public WorkflowContext wfContext;
        private WorkflowApplication instance;
        private AutoResetEvent syncEvent;
        public bool finished = false;
        private bool DBSTORE = false;//switch here to use oracle for workflow-persistence
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates a new Workflow, starting from the beginning with the given context data
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="ctx"></param>
        /// <param name="saveable"></param>
        public Workflow(Activity workflow, WorkflowContext ctx, bool saveable)
        {
            wfContext = ctx;
            init(workflow, saveable);
        }


        /// <summary>
        /// Creates a new Workflow, starting from the beginning with the given context data
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="ctx"></param>
        public Workflow(Activity workflow, WorkflowContext ctx)
        {
            wfContext = ctx;
            init(workflow,true);
        }

        /// <summary>
        /// Creates a new Workflow for an already persisted Workflow with GUID
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="wfid"></param>
        public Workflow(Activity workflow, Guid wfid)
        {
            this.wfid = wfid;
            wfContext = new WorkflowContext();
            init(workflow,true);
        }

        /// <summary>
        /// Initializes the WF4 WorkflowApplication Engine
        /// </summary>
        /// <param name="workflow"></param>
        private void init(Activity workflow, bool saveable)
        {
            // input parameters for the WF program
            IDictionary<string, object> inputs = new Dictionary<string, object>();
            inputs.Add("input", wfContext);

            // create and run the WF instance
            Activity wf = workflow;

            if (wfid == Guid.Empty)
            {
                instance = new WorkflowApplication(wf, inputs)
                {
                    PersistableIdle = OnIdleAndPersistable,
                    Idle = OnIdle
                };
                wfid = instance.Id;
            }
            else
            {
                instance = new WorkflowApplication(wf)
                {
                    PersistableIdle = OnIdleAndPersistable,
                    Idle = OnIdle
                };
            }

            if (saveable)
            {

               

                if(DBSTORE)
                { 
                    OracleInstanceStore store = new OracleInstanceStore(Configuration.DeliverOpenLeaseConnectionString());
                    store.InstanceEncodingOption = InstanceEncodingOption.None;
                    store.InstanceCompletionAction = InstanceCompletionAction.DeleteNothing;
                    instance.InstanceStore = store;
                }
                else
                {
                    XmlWorkflowInstanceStore storeFile = new XmlWorkflowInstanceStore(wfid);
                    instance.InstanceStore = storeFile;
                }
                

                //Create the persistence Participant and add it to the workflow instance
                XmlPersistenceParticipant xmlPersistenceParticipant = new XmlPersistenceParticipant(wfid);
                instance.Extensions.Add(xmlPersistenceParticipant);
            }
            /* TrackingProfile fileTrackingProfile = new TrackingProfile();
             fileTrackingProfile.Queries.Add(new WorkflowInstanceQuery
             {
                 States = { "*" }
             });
             fileTrackingProfile.Queries.Add(new ActivityStateQuery()
             {
                 ActivityName = "*",
                 States = { "*" },
                 Arguments = { { "*" } },
                
                 // You can use the following to specify specific stages:
                 // States = {
                 // ActivityStates.Executing,
                 // ActivityStates.Closed
                 //},
                 Variables = {{ "*" }} // or you can enter specific variable names instead of "*"

             });
             fileTrackingProfile.Queries.Add(new CustomTrackingQuery()
             {
                 ActivityName = "*",
                 Name = "*"
             });*/

            ContextFetcher fetcher = new ContextFetcher(this);
            //fileTrackingParticipant.TrackingProfile = fileTrackingProfile;
            // add a tracking participant
            instance.Extensions.Add(fetcher);
            instance.Aborted = delegate(WorkflowApplicationAbortedEventArgs e)
            {
                finished = true;
                _log.Error("Workflow was aborted: "+e.InstanceId+" Reason: " + e.Reason);
                // Signal the host that the workflow is complete.
                syncEvent.Set();
            };
            // Handle the desired lifecycle events.
            instance.Completed = delegate(WorkflowApplicationCompletedEventArgs e)
            {
                finished = true;
                
                //other than the usual user-Interaction which will write back the workflowcontext inside the ContextFetcher
                //upon completion we write back the context here, assuming that in every workflow our workflow context is named "input"
                wfContext = (WorkflowContext)e.Outputs["input"];
                // Signal the host that the workflow is complete.
                syncEvent.Set();
            };
            instance.OnUnhandledException = delegate(WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    Console.WriteLine(e.UnhandledException.Message.ToString());
                    return UnhandledExceptionAction.Abort;
                };
 
            instance.Unloaded = delegate(WorkflowApplicationEventArgs e)
            {
                Console.WriteLine("Unloaded with current view: " + wfContext.wfvid);

                // Signal the host that the workflow is complete.
                syncEvent.Set();
            };
        }

        

        /// <summary>
        /// Starts the Workflow and returns upon workflow termination or bookmark
        /// Returns the updated WorkflowContext
        /// </summary>
        public WorkflowContext start()
        {
            syncEvent = new AutoResetEvent(false);
            instance.Run();
            wfid = instance.Id;
            syncEvent.WaitOne();
            return wfContext;
        }

        /// <summary>
        /// Resumes a Workflow that has been bookmarked and returns upon workflow termination or bookmark
        /// Returns the updated WorkflowContext
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="context"></param>
        public WorkflowContext resume(String bookmark, WorkflowContext context)
        {
            syncEvent = new AutoResetEvent(false);
            try
            {
                instance.Load(wfid);
            }
            catch (Exception e)
            {
                String msg = "Resuming Workflow "+ wfid+" failed. Probably already finished";
                _log.Error(msg, e);
                throw new Exception(msg);
            }
            bool found = false;
            StringBuilder sb = new StringBuilder();
            foreach (BookmarkInfo bi in instance.GetBookmarks())
            {
                if (bi.BookmarkName.Equals(bookmark))
                {
                    found = true;
                }
                sb.Append(bi.BookmarkName);
                sb.Append(", ");
            }
            if (!found) throw new Exception("Bookmark " + bookmark + " not found for Workflow " + wfid+" Available Bookmarks are: "+sb.ToString());
            cleanupContext(context);
            
            instance.ResumeBookmark(bookmark, context);
            syncEvent.WaitOne();
            return wfContext;
        }

        /// <summary>
        /// Removes all non-persistent buttons and messages and the guiprompt
        /// </summary>
        /// <param name="ctx"></param>
        private void cleanupContext(WorkflowContext ctx)
        {
            if (ctx == null) return;
            ctx.outputCommand = null;

            if (ctx.buttons != null)
            {
                ctx.buttons.RemoveAll(a => a.persist == 0);
            }
            if (ctx.messages != null)
            {
                ctx.messages.RemoveAll(a => a.persist == 0);
            }
            if (ctx.prompt != null)
            {
               if(ctx.prompt.isInput>0)//when we have an input promt, use its value as inputcommand
                   ctx.inputCommand = ctx.prompt.value;
                ctx.prompt = null;
            }

        }

       
        // executed when instance goes idle
        private void OnIdle(WorkflowApplicationIdleEventArgs e)
        {
            

        }

        private PersistableIdleAction OnIdleAndPersistable(WorkflowApplicationIdleEventArgs e)
        {
            //here we control the behaviour upon idle-state like the one from UserInteraction-Activity
            return PersistableIdleAction.Unload;//we want the workflow to be persistet AND unloaded
        }
    }

    
}
