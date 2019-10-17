// OWNER MK, 24-11-2008
namespace Cic.OpenLease.Model.DdOw
{
    #region Using directives
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Cic.OpenOne.Common.Util;
    #endregion

    [System.CLSCompliant(true)]
    public static class JobServerHelper
    {
        #region Enums
        public enum ServerArts : int
        {
            JSStandard = 0,
            JSWinexec = 1,
            JSService = 2,
            EventEngine = 3,
            Jobhost = 4,
            Unknown = 99,
        }

        public enum Statuses
        {
            Working,
            Paused,
            Aborted,
            Pausing,
            Closed,
        }

        public enum ServerControls
        {
            Start,
            Pause,
            Abort,
        }

        public static Statuses DeliverStatus(string value)
        {
            if (StringUtil.IsTrimedNullOrEmpty(value))
            {
                throw new System.ArgumentException("status");
            }

            try
            {
                return (Statuses)System.Enum.Parse(typeof(Statuses), value);
            }
            catch
            {
                // Do something
                throw;
            }
        }
        #endregion

        #region Methods
        public static JS SelectJobsServer(OwExtendedEntities owExtendedEntities, string code, string computerName)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JS> Query;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check parameter
            if (StringUtil.IsTrimedNullOrEmpty(computerName))
            {
                // Throw exception
                throw new System.ArgumentException("computerName");
            }

            // Check code
            if (StringUtil.IsTrimedNullOrEmpty(code))
            {
                // Throw exception
                throw new System.ArgumentException("code");
            }

            Query = owExtendedEntities.JS;

            // Select JobServer from pcname and code
            Query = Query.Where<JS>(par => par.PCNAME.Trim().ToUpper() == computerName.Trim().ToUpper());
            Query = Query.Where<JS>(par => par.CODE.Trim().ToUpper() == code.Trim().ToUpper());

            try
            {
                return Query.FirstOrDefault<JS>();
            }
            catch
            {
                throw;
            }
        }

        public static JS Select(OwExtendedEntities owExtendedEntities, long sysJs)
        {
            System.Linq.IQueryable<Cic.OpenLease.Model.DdOw.JS> Query;
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            Query = owExtendedEntities.JS;

            // Select JobServer from pcname and code
            Query = Query.Where<JS>(par => par.SYSJS == sysJs);

            try
            {
                return Query.FirstOrDefault<JS>();
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateLastDateTime(OwExtendedEntities owExtendedEntities, JS jobServer, DateTime referenceDateTime)
        {
            try
            {
                jobServer.LASTDATE = referenceDateTime.Date;
                jobServer.LASTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(referenceDateTime);

                owExtendedEntities.SaveChanges();
            }
            catch (System.Data.OptimisticConcurrencyException)
            {
                // It it fails also well tehn we have a problem
                owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, jobServer);
                owExtendedEntities.SaveChanges();

            }
        }



        public static void SaveJobServer(OwExtendedEntities owExtendedEntities, JS jobServer)
        {
            try
            {
                owExtendedEntities.SaveChanges();
            }
            catch (System.Data.OptimisticConcurrencyException)
            {
                owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, jobServer);

                owExtendedEntities.SaveChanges();
            }
        }

        public static void UpdateCicLog(OwExtendedEntities owExtendedEntities, long sysJs, long sysCicLog)
        {
            JS JobServer;

            // Get JobServer
            JobServer = Select(owExtendedEntities, sysJs);

            if (JobServer != null)
            {
                JobServer.SYSCICLOG = sysCicLog;
                SaveJobServer(owExtendedEntities, JobServer);
            }
        }

        public static void UpdateStatus(OwExtendedEntities owExtendedEntities, long sysJs, Statuses status)
        {
            JS JobServer;

            // Get JobServer
            JobServer = Select(owExtendedEntities, sysJs);

            if (JobServer != null)
            {
                JobServer.STATUS = status.ToString();
                SaveJobServer(owExtendedEntities, JobServer);
            }
        }

        #endregion
    }
}
