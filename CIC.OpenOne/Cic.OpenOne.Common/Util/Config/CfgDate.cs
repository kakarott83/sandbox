using System;
using System.Globalization;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.Util.Config
{
    /// <summary>
    /// CfgDate-Klasse
    /// </summary>
    public static class CfgDate
    {
        /// <summary>
        /// checkPerDate
        /// </summary>
        /// <param name="dto"></param>
        public static void checkPerDate(KontextDto dto)
        {
            dto.perDate = verifyPerDate(dto.perDate);
        }

        /// <summary>
        /// verifyPerDate
        /// returns the current timestamp (date+time), or the overriden configured timestamp for promote
        /// DONT USE THIS for comparisons with validfrom-db-datestamps - use verifyPerDateNoTime there, else border-cases wont be handled correctly!
        /// </summary>
        /// <param name="dtoPerDate"></param>
        /// <returns>DateTime</returns>
        public static DateTime verifyPerDate(DateTime? dtoPerDate)
        {
            DateTime dbDate = DateTime.Now;

            if (dtoPerDate != null)
            {
                // CFG = "SETUP", CFGSEC = "PROMOTE", CFGVAR = "PERDATE
                string dbDateString = AppConfig.Instance.GetEntry("PROMOTE", "PERDATE", "", "SETUP");

                if (dbDateString != null && dbDateString.Length > 6 && DateTime.TryParse(dbDateString, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None,out dbDate))   //if this fails the date from the parameter will be used!
                    return dbDate;

                DateTime nullDate = new DateTime(1800, 1, 1);
                if (dtoPerDate != null && ((DateTime)dtoPerDate).Year > nullDate.Year)
                {
                    dbDate = (DateTime)dtoPerDate;
                }
            }

            return dbDate;
        }

        /// <summary>
        /// verifyPerDate
        /// returns the current timestamp (date only, no time), or the overriden configured timestamp for promote
        /// DONT USE THIS for time-span-calculations like timeouts!
        /// </summary>
        /// <param name="dtoPerDate"></param>
        /// <returns>DateTime</returns>
        public static DateTime verifyPerDateNoTime(DateTime? dtoPerDate)
        {
            DateTime dbDate = DateTime.Today;

            if (dtoPerDate != null)
            {
                // CFG = "SETUP", CFGSEC = "PROMOTE", CFGVAR = "PERDATE
                string dbDateString = AppConfig.Instance.GetEntry("PROMOTE", "PERDATE", "", "SETUP");

                if (dbDateString != null && dbDateString.Length > 6 && DateTime.TryParse(dbDateString, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out dbDate))   //if this fails the date from the parameter will be used!
                {
                    dbDate = new DateTime(dbDate.Year, dbDate.Month, dbDate.Day);
                    return dbDate;
                }

                DateTime nullDate = new DateTime(1800, 1, 1);
                if (dtoPerDate != null && ((DateTime)dtoPerDate).Year > nullDate.Year)
                {
                    dbDate = (DateTime)dtoPerDate;
                    dbDate = new DateTime(dbDate.Year, dbDate.Month, dbDate.Day);
                }
            }

            return dbDate;
        }

    }

}
