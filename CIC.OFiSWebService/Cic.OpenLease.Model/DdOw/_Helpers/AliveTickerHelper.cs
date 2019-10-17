// OWNER MK, 27-11-2008
namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using System.Collections;
    using Cic.OpenOne.Common.Util;
    #endregion

    [System.CLSCompliant(true)]
    public static class AliveTickerHelper
    {
        #region Methods
        public static void Create(OwExtendedEntities owExtendedEntities, JS jobServer, string aliveTickerStatus, string workflowUserCode)
        {
            WFALIVE AliveTicker;
            DateTime NowDateTime;

            // Check parameters
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            if (jobServer == null)
            {
                throw new System.ArgumentException("jobServer");
            }

            AliveTicker = MyGetAliveTicker(owExtendedEntities, jobServer);

            // AliveTicker already exists!
            if (AliveTicker != null)
            {
                throw new System.ArgumentException("AliveTicker");
            }

            NowDateTime = DateTime.Now;

            AliveTicker = new WFALIVE();
            AliveTicker.STARTDATUM = NowDateTime;
            AliveTicker.STARTZEIT = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(NowDateTime);
            AliveTicker.BENUTZER = workflowUserCode;
            AliveTicker.COMPUTERNAME = jobServer.PCNAME;
            AliveTicker.SERVERART = jobServer.CODE;
            AliveTicker.STATUS = aliveTickerStatus;

            AliveTicker.LETZTESDATUM = NowDateTime;
            AliveTicker.LETZTEZEIT = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(NowDateTime);

            owExtendedEntities.AddToWFALIVE(AliveTicker);
            owExtendedEntities.SaveChanges();

        }

        public static void Update(OwExtendedEntities owExtendedEntities, JS jobServer, JSQ jobServerQueueItem, string workflowUserCode)
        {
            WFALIVE AliveTicker;
            DateTime NowDateTime;

            // Check parameters
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            if (jobServer == null)
            {
                throw new System.ArgumentException("jobServer");
            }

            if (jobServerQueueItem == null)
            {
                throw new System.ArgumentException("jobServerQueueItem");
            }

            AliveTicker = MyGetAliveTicker(owExtendedEntities, jobServer);

            if (AliveTicker == null)
            {
                throw new System.ArgumentException("AliveTicker");
            }

            NowDateTime = DateTime.Now;

            AliveTicker.LETZTESDATUM = NowDateTime;
            AliveTicker.LETZTEZEIT = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(NowDateTime);

            // TODO MK 0 MK, Take from properties
            AliveTicker.JOBNAME = StringUtil.GetNullStartIndexSubstring(jobServerQueueItem.DESCRIPTION, 30);
            AliveTicker.JOBSTARTDATUM = jobServerQueueItem.WAITDATE;
            AliveTicker.JOBSTARTZEIT = jobServerQueueItem.WAITTIME;

            AliveTicker.GEBIET = jobServerQueueItem.GEBIET;
            AliveTicker.GEBIETID = jobServerQueueItem.SYSGEBIET;
            AliveTicker.WFJOBDATE = jobServerQueueItem.JOBDATE;

            owExtendedEntities.SaveChanges();
        }


        public static string UpdateStatus(OwExtendedEntities owExtendedEntities, JS jobServer, string jobStatus, string jobControl)
        {
            WFALIVE AliveTicker;
            string JobControlStatus = string.Empty;

            // Check parameters
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            if (jobServer == null)
            {
                throw new System.ArgumentException("jobServer");
            }


            AliveTicker = MyGetAliveTicker(owExtendedEntities, jobServer);

            // Get JobStatus
            JobControlStatus = AliveTicker.JOBCONTROL;

            if (AliveTicker != null)
            {
                try
                {
                    AliveTicker.LETZTESDATUM = DateTime.Now;
                    AliveTicker.LETZTEZEIT = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    AliveTicker.STATUS = jobStatus;
                    AliveTicker.JOBCONTROL = jobControl;

                    // Refresh because of concurency
                    owExtendedEntities.SaveChanges();
                }
                catch (System.Data.OptimisticConcurrencyException)
                {
                    // Changed by another station - ignore
                }
            }
            return JobControlStatus;
        }

        public static void Delete(OwExtendedEntities owExtendedEntities, JS jobServer)
        {
            WFALIVE AliveTicker;

            AliveTicker = MyGetAliveTicker(owExtendedEntities, jobServer);

            if (AliveTicker != null)
            {
                owExtendedEntities.DeleteObject(AliveTicker);
                owExtendedEntities.SaveChanges();
            }
            
        }
        #endregion

        #region My methods
        private static WFALIVE MyGetAliveTicker(OwExtendedEntities owExtendedEntities, JS jobServer)
        {
            WFALIVE AliveTicker;

            // Unique constraint WFALIVE_UK_COMPANDART
            var Query = from wfalive in owExtendedEntities.WFALIVE
                        where wfalive.COMPUTERNAME == jobServer.PCNAME &&
                        wfalive.SERVERART == jobServer.CODE
                        orderby wfalive.SYSWFALIVE descending
                        select wfalive;

            try
            {
                AliveTicker = Query.FirstOrDefault<WFALIVE>();
            }
            catch
            {
                AliveTicker = null;
            }

            return AliveTicker;
        }
        #endregion
    }
}
