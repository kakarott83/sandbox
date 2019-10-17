// OWNER MK, 23-11-2008
namespace Cic.OpenLease.Model.DdOw
{
    #region Using directives
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    [System.CLSCompliant(true)]
    public static class JobServerQueueHelper
    {
        #region Methods
        public static JSQ SelectJobsServerQueueItem(OwExtendedEntities owExtendedEntities, long sysJobServerQueueItem)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JSQ> Query;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check context
            if (sysJobServerQueueItem == 0)
            {
                // Throw exception
                throw new System.ArgumentException("sysJobServerQueueItem");
            }

            Query = owExtendedEntities.JSQ;

            // Select JobServer
            Query = Query.Where<JSQ>(par => par.SYSJSQ == sysJobServerQueueItem);

            try
            {
                return Query.FirstOrDefault();
            }
            catch
            {
                throw;
            }

        }

        // Selets the Job from JSQ. Takes only the ready and resubmission.
        public static JSQ SelectJobsServerQueueBySysJs(OwExtendedEntities owExtendedEntities, long sysJS, DateTime? referenceDateTime)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JSQ> Query;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            Query = owExtendedEntities.JSQ;

            // Select JobServer
            Query = Query.Where<JSQ>(par => par.JS.SYSJS == sysJS);

            // Select proper job status (rady = 0 and resubmission= 4)
            //Query.Where<JobServerQueue>(par => ((par.JobStatus == 0) || (!par.JobStatus.HasValue)));
            Query = Query.Where<JSQ>(par => par.JOBSTATUS == 0 || par.JOBSTATUS == 4);

            // Select by reference time
            if (referenceDateTime.HasValue)
            {
                // Get clarion date time
                var ReferenceClarionDateTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateTime(referenceDateTime.Value);
                var ReferenceClationTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(referenceDateTime.Value);

                // Check date scope
                if (ReferenceClarionDateTime >= Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinDate())
                {

                    // Get clarion values
                    DateTime? MinDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMinDateForClarion();
                    DateTime? MaxDateForClarion = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverMaxDateForClarion();
                    long ClarionMinTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinTime();
                    long ClarionMaxTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxTime();

                    // Get helper values
                    DateTime? CurrentDay = DateTime.Now.Date;
                    DateTime? NextDay = CurrentDay.Value.AddDays(1);

                    // Check date scope
                    Query = Query.Where<JSQ>(par => par.WAITDATE.HasValue);
                    Query = Query.Where<JSQ>(par => par.WAITDATE >= MinDateForClarion);
                    Query = Query.Where<JSQ>(par => par.WAITDATE <= MaxDateForClarion);
                    Query = Query.Where<JSQ>(par => par.WAITTIME.HasValue);
                    Query = Query.Where<JSQ>(par => par.WAITTIME >= ClarionMinTime);
                    Query = Query.Where<JSQ>(par => par.WAITTIME <= ClarionMaxTime);

                    //// Search submit date and time
                    Query = Query.Where<JSQ>(par => ((par.WAITDATE < CurrentDay) || ((par.WAITDATE >= CurrentDay) && (par.WAITDATE < NextDay) && (par.WAITTIME < ReferenceClationTime))));

                }

            }

            // Order
            Query = Query.OrderByDescending(par => par.PRIORITAET).ThenBy(par => par.POSITION).ThenBy(par => par.WAITDATE).ThenBy(par => par.WAITTIME);

            try
            {
                return Query.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public static void InsertJobServerQueueFromPlan(OwExtendedEntities owExtendedEntities, WFJPLAN[] jobPlan, long sysJS)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check jobPlan
            if (jobPlan == null || jobPlan.Length == 0)
            {
                // NOTE MK, Eraly return
                return;
            }

            foreach (WFJPLAN JobPlanItem in jobPlan)
            {
                JSQ JobServerQueueItem = MyCreateJobServerQueue(owExtendedEntities, JobPlanItem, sysJS);

                owExtendedEntities.AddToJSQ(JobServerQueueItem);
            }

            owExtendedEntities.SaveChanges();
        }

        public static void Delete(OwExtendedEntities owExtendedEntities, long sysJsq)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JSQ> Query;
            Cic.OpenLease.Model.DdOw.JSQ JobServerQueue;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check SYSJSQ
            if (sysJsq == 0)
            {
                // Throw exception
                throw new System.ArgumentException("sysJsq");
            }

            Query = owExtendedEntities.JSQ;

            // Select JobServerQueue
            Query = Query.Where<JSQ>(par => par.SYSJSQ == sysJsq);

            try
            {
                JobServerQueue = Query.FirstOrDefault();
            }
            catch
            {
                throw;
            }

            // JobServerQueue exists - delete it
            if (JobServerQueue != null)
            {
                try
                {
                    owExtendedEntities.DeleteObject(JobServerQueue);
                    owExtendedEntities.SaveChanges();
                } 
                catch
                {
                    throw;
                }
           }
        }

        public static void UpdateStatus(OwExtendedEntities owExtendedEntities, long sysJsq, int status)
        {
            Cic.OpenLease.Model.DdOw.JSQ JobServerQueueItem;
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JSQ> Query;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check SYSJSQ
            if (sysJsq == 0)
            {
                // Throw exception
                throw new System.ArgumentException("sysJsq");
            }

            Query = owExtendedEntities.JSQ;

            // Select JobServerQueue
            Query = Query.Where<JSQ>(par => par.SYSJSQ == sysJsq);

            JobServerQueueItem = Query.FirstOrDefault<JSQ>();

            if (JobServerQueueItem != null)
            {
                // Set JobStatus
                JobServerQueueItem.JOBSTATUS = status;
                owExtendedEntities.SaveChanges();
            }

        }
        #endregion

        #region My methods
        private static JSQ MyCreateJobServerQueue(OwExtendedEntities owExtendedEntities, WFJPLAN JobPlanItem, long sysJS)
        {
            JSQ JobServerQueueItem;

            if (JobPlanItem == null)
            {
                // NOTE MK, Eraly return
                return null;
            }

            JobServerQueueItem = new JSQ();

            try
            {
                JobServerQueueItem.SYSGEBIET = JobPlanItem.SYSGEBIET;
                JobServerQueueItem.GEBIET = JobPlanItem.GEBIET;
                JobServerQueueItem.DESCRIPTION = JobPlanItem.BESCHREIBUNG;

                if (JobPlanItem.EXECUTEDDATE.HasValue && JobPlanItem.EXECUTEDDATE.Value <= Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxDate())
                {
                    JobServerQueueItem.WAITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDateTime((int)JobPlanItem.EXECDATE.Value);
                }

                JobServerQueueItem.WAITTIME = JobPlanItem.EXECTIME;
                JobServerQueueItem.SUBMITDATE = DateTime.Now.Date;
                JobServerQueueItem.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                JobServerQueueItem.PRIORITAET = 1;
                JobServerQueueItem.POSITION = 1;
                //JobServerQueueItem.JobServerReference = new System.Data.EntityKey("DdOw.JobServer", "SysJS", sysJS);
                JobServerQueueItem.JS = owExtendedEntities.JS.Where<JS>(par => par.SYSJS == sysJS).First<JS>();
                JobServerQueueItem.JOBSTATUS = 0;


                if (JobPlanItem.JOBDATE.HasValue && JobPlanItem.JOBDATE.Value <= Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxDate())
                {
                    JobServerQueueItem.JOBDATE = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDateTime((int)JobPlanItem.JOBDATE.Value);
                }

                JobServerQueueItem.SYSWFJPLAN = JobPlanItem.SYSWFJPLAN;
            }
            catch
            {
                // TODO MK 0 MK, What to do??
                throw;
            }

            return JobServerQueueItem;
        }
        #endregion

    }
}
