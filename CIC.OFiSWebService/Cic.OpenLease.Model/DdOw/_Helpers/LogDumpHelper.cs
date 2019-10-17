namespace Cic.OpenLease.Model.DdOw
{
    #region Using
    using System;
    using System.Linq;
    #endregion

    public class LogDumpHelper
    {

        #region Enums
        public enum LogDumpTypes : int
        {
            OlOutBound = 0,
            OlInbound = 1,
        }
        #endregion

        #region Methods
        public static LOGDUMP CreateLogDump(OwExtendedEntities owExtendedEntities, string description, string dump, LogDumpTypes logDumpType, string url)
        {
            DateTime CurrentDateTime = DateTime.Now;

            LOGDUMP LogDump = new LOGDUMP();
            LogDump.ART = 0;
            LogDump.DESCRIPTION = description;
            LogDump.DUMPDATE = CurrentDateTime;
            LogDump.DUMPTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(CurrentDateTime);
            LogDump.DUMPVALUE = dump;
            LogDump.INPUTFLAG = (int)logDumpType;
            LogDump.URL = url;

            return LogDump;
        }

        public static void UpdateSysCicLog(OwExtendedEntities context, long sysLogDump, long sysCicLog)
        {
            if (sysCicLog > 0 && sysLogDump > 0)
            {
                LOGDUMP LogDump = context.LOGDUMP.Where(p => p.SYSLOGDUMP == sysLogDump).FirstOrDefault<LOGDUMP>();
                if (LogDump != null)
                {
                    LogDump.SYSCICLOG = sysCicLog;
                    context.SaveChanges();
                }
            }
        }

        public static void UpdateSysEaiHot(OwExtendedEntities context, long sysLogDump, long sysEaiHot)
        {
            if (sysEaiHot > 0 && sysLogDump > 0)
            {
                LOGDUMP LogDump = context.LOGDUMP.Where(p => p.SYSLOGDUMP == sysLogDump).FirstOrDefault<LOGDUMP>();
                if (LogDump != null)
                {
                    LogDump.SYSEAIHOT = sysEaiHot;
                    context.SaveChanges();
                }
            }
        }
        #endregion
    }
}
