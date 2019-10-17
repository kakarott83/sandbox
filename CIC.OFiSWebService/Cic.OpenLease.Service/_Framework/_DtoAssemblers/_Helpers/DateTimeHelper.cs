// OWNER JJ, 07-12-2009
using System;
namespace Cic.OpenLease.Service
{
    [System.CLSCompliant(true)]
    public static class DateTimeHelper
    {
        #region Methods
        public static long? DateTimeToClarionTime(System.DateTime? dateTime)
        {
            long? Time = null;

            if (dateTime.HasValue)
            {
                try
                {                   
                    // Convert
                    Time = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime((System.DateTime)dateTime);
                }
                catch
                {
                    // Ignore exception
                }
            }

            return Time;
        }
        /*

        public static DateTime ClarionTimeToDate(DateTime date, long clarionTime)
        {
            DateTime? tempTime = DateTimeHelper.ClarionTimeToDateTime(clarionTime);
            if (tempTime.HasValue)
            {
                return  new DateTime(date.Year, date.Month, date.Day, tempTime.Value.Hour, tempTime.Value.Minute, tempTime.Value.Second);
            }
            return date;
        }

        public static System.DateTime? ClarionTimeToDateTime(long? clarionTime)
        {
            System.DateTime? DateTime = null;

            if (clarionTime.HasValue)
            {
                try
                {
                    // Convert
                    DateTime = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime((int)clarionTime);                    
                }
                catch
                {
                    // Ignore exception
                }
            }

            return DateTime;
        }*/


        public static DateTime getUltimo(DateTime dtDate)
        {


            DateTime dtTo = dtDate;



            dtTo = dtTo.AddMonths(1);



            dtTo = dtTo.AddDays(-(dtTo.Day));



            return dtTo;

        }
        public static DateTime getUltimoN(DateTime dtDate, int n)
        {


            DateTime dtTo = dtDate;
            dtTo = dtTo.AddMonths(1 + n);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;

        }
        #endregion
    }
}