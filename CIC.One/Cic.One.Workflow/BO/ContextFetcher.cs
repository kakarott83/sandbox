using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Tracking;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// Create a tracking participant that writes back the WorkflowContext into the Workflow
    /// </summary>
    public class ContextFetcher : TrackingParticipant
    {
        private Workflow wf;

        public ContextFetcher(Workflow wf)
        {
            this.wf = wf;
        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            ContextTracker currentRecord = record as ContextTracker;
            if (currentRecord != null)
            {
                wf.wfContext = currentRecord.wfcontext;
            }
            /* 
             // get the tracking path
             string fileName = IOHelper.GetTrackingFilePath(record.InstanceId);

             // create a writer and open the file
             using (StreamWriter tw = File.AppendText(fileName))
             {
                 // write a line of text to the file
                 tw.WriteLine(record.ToString());
             }*/
        }
    }

}
