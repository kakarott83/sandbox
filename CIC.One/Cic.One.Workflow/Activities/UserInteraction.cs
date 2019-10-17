using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using Cic.One.DTO;
using Cic.One.Workflow.BO;
using Cic.One.Workflow.Icons;

using System.Workflow.ComponentModel;
using System.ComponentModel;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;



namespace Cic.One.Workflow.Activities
{
    /// <summary>
    /// Activity that creates a Bookmark and raises an event containing the current workflow context
    /// The Bookmark will lead the workflow to idle-state, thus allowing some user interaction
    /// the idle-state will be captured as event and lead to persisting the wf and unloading it
    /// 
    /// The bookmark will always be named after the constant BOOKMARK_ID !!!!
    /// 
    /// Upon resuming the wf the context will be overwritten with the value passed in from the resume call, if a value was given
    /// 
    /// TODO: Custom Editor for Activity:
    /// http://msdn.microsoft.com/en-us/library/gg417193.aspx
    /// http://blogs.msdn.com/b/tilovell/archive/2011/04/06/wf4-showing-an-inargument-lt-bool-gt-as-a-checkbox-in-the-workflow-designer-property-grid.aspx
    /// http://www.biztalkgurus.com/windows_workflow/b/workflow-syn/archive/2011/04/06/wf4-showing-an-inargument-lt-bool-gt-as-a-checkbox-in-the-workflow-designer-property-grid.aspx
    /// 
    /// Combobox
    /// http://msdn.microsoft.com/en-us/library/vstudio/gg417193%28v=vs.100%29.aspx
    /// </summary>
    //[System.Drawing.ToolboxBitmap(typeof(IconResourceAnchor), "CICOne.png")]
    
    public class UserInteraction : NativeActivity<WorkflowContext>
    {
        public const String BOOKMARK_ID = "uiBookmark";
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public InArgument<WorkflowContext> wfcontext { get; set; }
        [RequiredArgument]
        public InArgument<String> workflowViewId { get; set; }
        
        public InArgument<bool> endWorkflow { get; set; }
     
        protected override void Execute(NativeActivityContext context)
        {
            // Create a Bookmark with the name of WFVID and wait for it to be resumed.
            //context.CreateBookmark(wfcontext.Get(context).wfvid, new BookmarkCallback(OnResumeBookmark));
            
            wfcontext.Get(context).wfvid = workflowViewId.Get(context);

            
            if (endWorkflow != null)
            {
                try
                {
                    bool endWF = endWorkflow.Get(context);
                    if (!endWF)
                        context.CreateBookmark(BOOKMARK_ID, new BookmarkCallback(OnResumeBookmark));
                }
                catch (Exception e)
                {
                    _log.Error("Problem reading endWorkflow-Argument: " + e.Message);
                }
            }
            else context.CreateBookmark(BOOKMARK_ID, new BookmarkCallback(OnResumeBookmark));
            //Raise an Tracking-Event containing our Workflow-Context which was passed in here by the workflow-Activity            
            context.Track(new ContextTracker(wfcontext.Get(context)));
        }

        // NativeActivity derived activities that do asynchronous operations by calling 
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext 
        // must override the CanInduceIdle property and return true.
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        public void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
        {
          
            // When the Bookmark is resumed, assign its value to
            // the Result argument.
            if(obj!=null)
                Result.Set(context,obj);
        }
    }
}
