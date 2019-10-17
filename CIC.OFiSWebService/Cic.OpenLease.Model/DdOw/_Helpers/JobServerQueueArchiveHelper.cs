// OWNER MK, 26-11-2008
namespace Cic.OpenLease.Model.DdOw
{
    #region Using directives
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class JobServerQueueArchiveHelper
    {
        #region Methods
        public static void SaveCopy(OwExtendedEntities owExtendedEntities, JSQ jobServerQueueItem)
        {
            try
            {
                ARCJSQ Arcjsq;

                Arcjsq = new ARCJSQ();

                Arcjsq.DESCRIPTION = jobServerQueueItem.DESCRIPTION;
                Arcjsq.JOBKETTE = jobServerQueueItem.JOBKETTE;
                Arcjsq.JOBDATE = jobServerQueueItem.JOBDATE;
                Arcjsq.JOBSTATUS = jobServerQueueItem.JOBSTATUS;
                Arcjsq.POSITION = jobServerQueueItem.POSITION;
                Arcjsq.PRIORITAET = jobServerQueueItem.PRIORITAET;
                Arcjsq.WIEDERHOLUNG = jobServerQueueItem.WIEDERHOLUNG;
                Arcjsq.SUBMITDATE = jobServerQueueItem.SUBMITDATE;
                Arcjsq.SUBMITTIME = jobServerQueueItem.SUBMITTIME;
                Arcjsq.WAITDATE = jobServerQueueItem.WAITDATE;
                Arcjsq.WAITTIME = jobServerQueueItem.WAITTIME;
                Arcjsq.GEBIET = jobServerQueueItem.GEBIET
;
                Arcjsq.SYSGEBIET = jobServerQueueItem.SYSGEBIET;
                Arcjsq.SYSWFJPLAN = jobServerQueueItem.SYSWFJPLAN;
                Arcjsq.SYSJSQ = jobServerQueueItem.SYSJSQ;
                Arcjsq.SYSJSESSION = jobServerQueueItem.SYSJSESSION;
                Arcjsq.STARTTIME = jobServerQueueItem.STARTTIME;
                Arcjsq.STARTDATE = jobServerQueueItem.STARTDATE;

                owExtendedEntities.AddToARCJSQ(Arcjsq);
                owExtendedEntities.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
