// OWNER MK, 24-11-2008
namespace Cic.OpenLease.Model.DdOw
{
    #region Using directives
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    [System.CLSCompliant(true)]
    public static class JobPlanHelper
    {
        #region Methods
        public static WFJPLAN[] SelectJobPlan(OwExtendedEntities owExtendedEntities, long sysJS, DateTime? referenceDateTime, int pageIndex, int pageSize)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.WFJPLAN> Query;
            System.Linq.IQueryable<long> JobServerQueueIdQuery;
            long[] JobServerQueueIds;

            List<WFJPLAN> JobPlanList;

            int Skip;
            int Top;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageIndex");
            }

            // Check page index
            if (pageSize < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageSize");
            }

            // Set default values
            Skip = 0;
            Top = 0;

            // Check page size
            if (pageSize > 0)
            {
                // Set values
                Skip = (pageIndex * pageSize);
                Top = pageSize;
            }

            Query = owExtendedEntities.WFJPLAN;

            // Select JobServer
            Query = Query.Where<WFJPLAN>(par => par.JS.SYSJS == sysJS);

            // Select proper job status (ready for execution or resubmission)
            // TODO MK 0 MK, Make it better!
            //Query.Where<JobPlan>(par => ((par.ExecutedFlag == 0) || (!par.ExecutedFlag.HasValue)));
            Query = Query.Where<WFJPLAN>(par => par.FLAGEXECUTED == 0);
            //Query = Query.Where<JobPlan>(par => par.ExecutedFlag.GetValueOrDefault(0) == 0);

            // Select by reference time
            if (referenceDateTime.HasValue)
            {
                // Get clarion date time
                var ReferenceClarionDateTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateTime(referenceDateTime.Value);

                // Check date scope
                if (ReferenceClarionDateTime >= Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinDate())
                {

                    // Get clarion values
                    var ClarionDateMultiplier = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionDateMultiplier();
                    var ClarionMinDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinDate();
                    var ClarionMaxDate = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxDate();
                    var ClarionMinTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMinTime();
                    var ClarionMaxTime = Cic.OpenOne.Common.Util.DateTimeHelper.DeliverClarionMaxTime();

                    // Check date scope
                    Query = Query.Where<WFJPLAN>(par => par.EXECDATE.HasValue);
                    Query = Query.Where<WFJPLAN>(par => par.EXECDATE >= ClarionMinDate);
                    Query = Query.Where<WFJPLAN>(par => par.EXECDATE <= ClarionMaxDate);
                    Query = Query.Where<WFJPLAN>(par => par.EXECTIME.HasValue);
                    Query = Query.Where<WFJPLAN>(par => par.EXECTIME >= ClarionMinTime);
                    Query = Query.Where<WFJPLAN>(par => par.EXECTIME <= ClarionMaxTime);

                    //// Search submit date and time
                    Query = Query.Where<WFJPLAN>(par => ((par.EXECDATE * ClarionDateMultiplier) + par.EXECTIME) < ReferenceClarionDateTime);
                }
            }

            // Order
            Query = Query.OrderBy(par => par.EXECDATE).OrderBy(par => par.EXECTIME);

            // Check top
            if (Top > 0)
            {
                // TODO MK 0 BK, Check exceptions (Maybe change Dynamics.cs)
                Query = Query.Skip<WFJPLAN>(Skip);

                Query = Query.Take<WFJPLAN>(Top);
            }

            try
            {

                // Select wfjplan ids that where already copied to jsq
                JobServerQueueIdQuery = from jsq in owExtendedEntities.JSQ
                                        where jsq.SYSWFJPLAN.HasValue
                                        select jsq.SYSWFJPLAN.Value;


                // Execute
                JobServerQueueIds = JobServerQueueIdQuery.ToArray<long>();

                // Get all  jobs that are due to now
                JobPlanList = Query.ToList<WFJPLAN>();

                // Get all jobs that are due and were not copied to jsq
                var listQuery = from wfjplan in JobPlanList
                        where !JobServerQueueIds.Contains(wfjplan.SYSWFJPLAN)
                        select wfjplan;

                return listQuery.ToArray<WFJPLAN>();

            }
            catch
            {
                throw;
            }
        }


        public static void UpdateExecutedFlag(OwExtendedEntities owExtendedEntities, long sysWFJPlan, int executedFlag)
        {
            try
            {
                var q2 = from wfjplan in owExtendedEntities.WFJPLAN
                         where wfjplan.SYSWFJPLAN == sysWFJPlan
                         select wfjplan;

                WFJPLAN JobPlan = q2.First<WFJPLAN>();

                JobPlan.FLAGEXECUTED = executedFlag;

                if (executedFlag != 0)
                {
                    JobPlan.EXECUTEDDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                    JobPlan.EXECUTEDTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                }

                owExtendedEntities.SaveChanges();
            }
            catch (ArgumentNullException)
            {
                // Nothing to update. WFJPlan was probably deleted
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
