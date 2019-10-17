using System;

namespace Cic.OpenOne.Common.Util
{
    /// <summary>
    /// DateTimeHelper-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public static class DateTimeHelper
    {
        #region Private constant
        // 01.01.1801 = 1. Jan 1801
        private const int CnstMinDateTimeForClarionDay = 1;
        private const int CnstMinDateTimeForClarionMonth = 1;
        private const int CnstMinDateTimeForClarionYear = 1801;
        private const int CnstClarionMinDate = 4;
        private const int CnstClarionMaxDate = 2994626;
        private const int CnstClarionMinTime = 1;
        private const int CnstClarionMaxTime = 8639999;
        private const int CnstClarionDateMultiplier = 10000000;
        private const int CnstMinDateTimeForJavaDay = 1;
        private const int CnstMinDateTimeForJavaMonth = 1;
        private const int CnstMinDateTimeForJavaYear = 1970;
        #endregion

        #region Methods

        /// <summary>
        /// DeliverMinDateForClarion
        /// </summary>
        /// <returns></returns>
        public static System.DateTime DeliverMinDateForClarion()
        {
            return new System.DateTime(CnstMinDateTimeForClarionYear, CnstMinDateTimeForClarionMonth, CnstMinDateTimeForClarionDay);
        }

        /// <summary>
        /// DeliverClarionDateMultiplier
        /// </summary>
        /// <returns></returns>
        public static int DeliverClarionDateMultiplier()
        {
            return CnstClarionDateMultiplier;
        }

        /// <summary>
        /// DeliverMaxDateForClarion
        /// </summary>
        /// <returns></returns>
        public static System.DateTime DeliverMaxDateForClarion()
        {
            return DeliverMinDateForClarion().AddDays(CnstClarionMaxDate - CnstClarionMinDate);
        }


        /// <summary>
        /// DeliverMinDateForJava
        /// </summary>
        /// <returns></returns>
        public static System.DateTime DeliverMinDateForJava()
        {
            return new System.DateTime(CnstMinDateTimeForJavaYear, CnstMinDateTimeForJavaMonth, CnstMinDateTimeForJavaDay);
        }


        /// <summary>
        /// DeliverClarionMinDate
        /// </summary>
        /// <returns></returns>
        public static int DeliverClarionMinDate()
        {
            return CnstClarionMinDate;
        }

        /// <summary>
        /// DeliverClarionMaxDate
        /// </summary>
        /// <returns></returns>
        public static int DeliverClarionMaxDate()
        {
            return CnstClarionMaxDate;
        }

        /// <summary>
        /// DeliverClarionMinTime
        /// </summary>
        /// <returns></returns>
        public static int DeliverClarionMinTime()
        {
            return CnstClarionMinTime;
        }

        /// <summary>
        /// DeliverClarionMaxTime
        /// </summary>
        /// <returns></returns>
        public static int DeliverClarionMaxTime()
        {
            return CnstClarionMaxTime;
        }

        /// <summary>
        /// ValidateDateTimeForClarion
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool ValidateDateTimeForClarion(System.DateTime dateTime)
        {
            return ((dateTime >= DeliverMinDateForClarion()) && (dateTime <= DeliverMaxDateForClarion()));
        }

        /// <summary>
        /// ValidateClarionDate
        /// </summary>
        /// <param name="clarionDate"></param>
        /// <returns></returns>
        public static bool ValidateClarionDate(int clarionDate)
        {
            return ((clarionDate >= CnstClarionMinDate) && (clarionDate <= CnstClarionMaxDate));
        }

        /// <summary>
        /// ValidateClarionTime
        /// </summary>
        /// <param name="clarionTime"></param>
        /// <returns></returns>
        public static bool ValidateClarionTime(int clarionTime)
        {
            return ((clarionTime >= CnstClarionMinTime) && (clarionTime <= CnstClarionMaxTime));
        }

        /// <summary>
        /// Creates a DateTime for Clarion DateTime fields
        /// a Date without time information, time will always be zero
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? DateTimeToClarionDateNoTime(DateTime date)
        {
            if (date == null) return null;
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        /// <summary>
        /// DateTimeToClarionDate
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateTimeToClarionDate(System.DateTime dateTime)
        {
            System.DateTime MinDateForClarion;
            System.TimeSpan TimeSpan;

            // Clear time
            dateTime = dateTime.Date;

            // Get min date time
            MinDateForClarion = DeliverMinDateForClarion();

            // Check date time
            if ((dateTime < MinDateForClarion) || (dateTime > DeliverMaxDateForClarion()))
            {
                // Throw exception
                throw new System.ArgumentOutOfRangeException("dateTime");
            }
            // Get time span
            TimeSpan = (dateTime - MinDateForClarion);

            // Return
            return (TimeSpan.Days + CnstClarionMinDate);
        }

        /// <summary>
        /// DateTimeToClarionTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateTimeToClarionTime(System.DateTime dateTime)
        {
            int ClarionTime;

            ClarionTime = (dateTime.Hour * 360000);
            ClarionTime = (ClarionTime + (dateTime.Minute * 6000));
            ClarionTime = (ClarionTime + (dateTime.Second * 100));
            ClarionTime = (ClarionTime + (dateTime.Millisecond / 10));
            ClarionTime = (ClarionTime + CnstClarionMinTime);

            return ClarionTime;
        }

        /// <summary>
        /// DateTimeToClarionDateAndTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="clarionDate"></param>
        /// <param name="clarionTime"></param>
        public static void DateTimeToClarionDateAndTime(System.DateTime dateTime, out int clarionDate, out int clarionTime)
        {
            try
            {
                // Set value
                clarionDate = DateTimeToClarionDate(dateTime);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
            // Set value
            clarionTime = DateTimeToClarionTime(dateTime);
        }

        /// <summary>
        /// ClarionDateToDateTime
        /// </summary>
        /// <param name="clarionDate"></param>
        /// <returns></returns>
        public static System.DateTime ClarionDateToDateTime(int clarionDate)
        {
            // Check clarion date
            if ((clarionDate < CnstClarionMinDate) || (clarionDate > CnstClarionMaxDate))
            {
                // Throw exception
                throw new ArgumentOutOfRangeException("clarionDate");
            }
            // Return
            return DeliverMinDateForClarion().AddDays(clarionDate - CnstClarionMinDate);
        }

        /// <summary>
        /// ClarionTimeToDateTime
        /// </summary>
        /// <param name="clarionTime"></param>
        /// <returns></returns>
        public static System.DateTime ClarionTimeToDateTime(int clarionTime)
        {
            System.DateTime DateTime;

            // Check clarion time
            if ((clarionTime < CnstClarionMinTime) || (clarionTime > CnstClarionMaxTime))
            {
                // Throw exception
                throw new ArgumentOutOfRangeException("clarionTime");
            }

            // Set min date time
            DateTime = DeliverMinDateForClarion();
            // Remove min value
            clarionTime = (clarionTime - CnstClarionMinTime);

            // Return
            return DateTime.AddMilliseconds(clarionTime * 10);
        }


        /// <summary>
        /// ClarionDateAndTimeToDateTime
        /// </summary>
        /// <param name="clarionDate"></param>
        /// <param name="clarionTime"></param>
        /// <returns></returns>
        public static System.DateTime ClarionDateAndTimeToDateTime(int clarionDate, int clarionTime)
        {
            System.DateTime DateDateTime;
            System.DateTime TimeDateTime;

            try
            {
                // Set values
                DateDateTime = ClarionDateToDateTime(clarionDate);
                TimeDateTime = ClarionTimeToDateTime(clarionTime);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
            // Return
            return DateDateTime.Add(TimeDateTime.TimeOfDay);
        }

        /// <summary>
        /// ClarionDateAndTimeToClarionDateTime
        /// </summary>
        /// <param name="clarionDate"></param>
        /// <param name="clarionTime"></param>
        /// <returns></returns>
        public static long ClarionDateAndTimeToClarionDateTime(int clarionDate, int clarionTime)
        {
            // Check clarion date
            if ((clarionDate < CnstClarionMinDate) || (clarionDate > CnstClarionMaxDate))
            {
                // Throw exception
                throw new ArgumentOutOfRangeException("clarionDate");
            }
            // Check clarion time
            if ((clarionTime < CnstClarionMinTime) || (clarionTime > CnstClarionMaxTime))
            {
                // Throw exception
                throw new ArgumentOutOfRangeException("clarionTime");
            }

            // Return
            return ((clarionDate * CnstClarionDateMultiplier) + clarionTime);
        }

        /// <summary>
        /// DateTimeToClarionDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToClarionDateTime(System.DateTime dateTime)
        {
            long ClarionDate;
            long ClarionTime;

            try
            {
                // Set value
                ClarionDate = DateTimeToClarionDate(dateTime);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
            // Set value
            ClarionTime = DateTimeToClarionTime(dateTime);
            // Return
            return ((ClarionDate * CnstClarionDateMultiplier) + ClarionTime);
        }

        /// <summary>
        /// ClarionDateToDtoDate
        /// </summary>
        /// <param name="dtoDate"></param>
        /// <returns></returns>
        public static System.DateTime? ClarionDateToDtoDate(System.DateTime? dtoDate)
        {
            System.Collections.Generic.List<System.DateTime?> BadDates = new System.Collections.Generic.List<System.DateTime?>();
            BadDates.Add(new System.DateTime?(new System.DateTime(111, 1, 1)));

            System.DateTime? returnDate = null;

            //Check if dtoDate is in list of bad dates
            if (BadDates.Contains(dtoDate))
            {
                returnDate = null;
            }
            else
            {
                returnDate = dtoDate;
            }
            return returnDate;
        }
        #endregion


        /// <summary>
        /// Convert clarion Time to DateTime nullable without an exception
        /// </summary>
        /// <param name="clarionTime"></param>
        /// <returns></returns>
        public static System.DateTime? ClarionTimeToDateTimeNoException(int clarionTime)
        {
            if ((clarionTime < CnstClarionMinTime) || (clarionTime > CnstClarionMaxTime))
            {
                return null;
            }
            else
                return ClarionTimeToDateTime(clarionTime);
        }


        /// <summary>
        /// Convert Date Time to Clarion Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DateTimeToClarionTime(System.DateTime? dateTime)
        {

            if (!dateTime.HasValue)
                return 0;

            return Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(dateTime.Value);
        }

        /// <summary>
        /// erzeugt ein DateTime aus einem Datum und einer ClarionZeit
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeClarion"></param>
        /// <returns></returns>
        public static DateTime? CreateDate(DateTime? date, long? timeClarion)
        {
            if (!date.HasValue)
                return null;

            if (timeClarion.HasValue)
            {
                if (timeClarion.Value == 0)
                    return date;

                DateTime time = ClarionTimeToDateTime((int)timeClarion.Value);
                DateTime dateReal = date.Value;
                return
                    new DateTime(dateReal.Year, dateReal.Month, dateReal.Day, time.Hour, time.Minute, time.Second);
            }
            else
                return date;
        }

        /// <summary>
        /// Wandelt ein DateTime? in eine ClarionTime(long?) um
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long? DateTimeNullableToClarionTime(DateTime? date)
        {
            if (date.HasValue)
            {
                return DateTimeToClarionTime(date.Value);
            }
            else
                return null;
        }

    }
}