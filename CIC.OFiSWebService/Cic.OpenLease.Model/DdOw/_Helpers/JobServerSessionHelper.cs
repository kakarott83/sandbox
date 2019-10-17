// OWNER MK, 05-01-2009
namespace Cic.OpenLease.Model.DdOw
{
    #region Using directives
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion
    
    [System.CLSCompliant(true)]
    public class JobServerSessionHelper
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

        public enum SessionControls
        {
            Kill,
            Restart,
        }

        public enum SessionStatuses
        {
            Active,
            Killed,
            Closed,
            Interrupted,
            Broken,
            Working,
            Unknown
        }
        #endregion

        public static JSESSION CreateJobServerSession(OwExtendedEntities owExtendedEntities, JS jobServer)
        {
            JSESSION JobServerSession = null;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check jobServer
            if (jobServer == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedjobServerEntities");
            }

            JobServerSession = GetJobServerSession(owExtendedEntities, jobServer);

            if (JobServerSession != null)
            {
                CloseJobServerSession(owExtendedEntities, jobServer, SessionStatuses.Broken);
            }

            JobServerSession = new JSESSION();
            JobServerSession.CODE = jobServer.CODE;
            JobServerSession.PCNAME = jobServer.PCNAME;
            JobServerSession.DATABASE = jobServer.DATENBANK;
            JobServerSession.DESCRIPTION = jobServer.DESCRIPTION;
            JobServerSession.JS = jobServer;
            JobServerSession.SESSIONSTATUS = SessionStatuses.Active.ToString();
            JobServerSession.SERVERART = jobServer.SERVERART;
            JobServerSession.SERVERUSER = jobServer.SERVERUSER;
            JobServerSession.STARTDATE = DateTime.Now.Date;
            JobServerSession.STARTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
            JobServerSession.SYSACTIVEJS = jobServer.SYSJS;

            try
            {
                owExtendedEntities.SaveChanges();
            }
            catch (System.Data.UpdateException)
            {
                throw;
            }
            
            return JobServerSession;
        }

        public static void CloseJobServerSession(OwExtendedEntities owExtendedEntities, JS jobServer, SessionStatuses status)
        {
            JSESSION JobServerSession = null;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check jobServer
            if (jobServer == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedjobServerEntities");
            }

            JobServerSession = GetJobServerSession(owExtendedEntities, jobServer);

            if (JobServerSession != null)
            {
                JobServerSession.SESSIONSTATUS = status.ToString();
                JobServerSession.SYSACTIVEJS = null;
                JobServerSession.ENDDATE = DateTime.Now.Date;
                JobServerSession.ENDTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

                try
                {
                    owExtendedEntities.SaveChanges();
                }
                catch (System.Data.OptimisticConcurrencyException)
                {
                    owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, JobServerSession);
                    owExtendedEntities.SaveChanges();
                }
            }
        }

        public static JSESSION GetJobServerSession(OwExtendedEntities owExtendedEntities, JS jobServer)
        {
            System.Linq.IQueryable<JSESSION> Query;
            JSESSION JobServerSession = null;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedEntities");
            }

            // Check jobServer
            if (jobServer == null)
            {
                // Throw exception
                throw new System.ArgumentException("owExtendedjobServerEntities");
            }

            Query = owExtendedEntities.JSESSION;

            Query = Query.Where<JSESSION>(par => par.SYSACTIVEJS == jobServer.SYSJS);

            //try
            //{
            // INDEX CIC.JSESSION_UK_ACTIVEJS
            JobServerSession = Query.FirstOrDefault<JSESSION>();
            //}
            //catch
            //{
            //    throw;
            //}

            return JobServerSession;
        }

        public static void SaveJobServerSession(OwExtendedEntities owExtendedEntities, JSESSION jobServerSession)
        {
            try
            {
                owExtendedEntities.SaveChanges();
            }
            catch (System.Data.OptimisticConcurrencyException)
            {
                owExtendedEntities.Refresh(System.Data.Objects.RefreshMode.ClientWins, jobServerSession);

                owExtendedEntities.SaveChanges();
            }
        }

        public static void UpdateStatus(OwExtendedEntities owExtendedEntities, JS jobServer, SessionStatuses sessionStatuses)
        {
            JSESSION JobServerSession;

            // Get JobServer status
            JobServerSession = JobServerSessionHelper.GetJobServerSession(owExtendedEntities, jobServer);

            if (JobServerSession != null)
            {
                JobServerSession.SESSIONSTATUS = sessionStatuses.ToString();

                JobServerSessionHelper.SaveJobServerSession(owExtendedEntities, JobServerSession);
            }

        }

        public static void UpdateCicLog(OwExtendedEntities owExtendedEntities, JS jobServer, long sysCicLog)
        {
            JSESSION JobServerSession;

            // Get JobServer status
            JobServerSession = JobServerSessionHelper.GetJobServerSession(owExtendedEntities, jobServer);

            if (JobServerSession != null)
            {
                JobServerSession.SYSCICLOG = sysCicLog;

                JobServerSessionHelper.SaveJobServerSession(owExtendedEntities, JobServerSession);
            }            
        }
    }
}
