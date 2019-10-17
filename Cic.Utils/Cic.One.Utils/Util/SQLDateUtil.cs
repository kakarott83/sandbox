using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.Utils.Util
{
    /// <summary>
    /// —————————————————————————————————————————————————————————————————————————————————————
    /// Create string to query clarion date
    /// —————————————————————————————————————————————————————————————————————————————————————
    /// IsBeforeValidFrom (dateInQuestion, tableName)
    /// IsBeforeValidUntil (dateInQuestion, tableName)
    /// CheckCurrentSysDate --> checks if SYSDATE is IN BETWEEN ValidFrom AND ValidUntil
    /// CheckDate --> checks if dateInQuestion is IN BETWEEN ValidFrom AND ValidUntil
    /// —————————————————————————————————————————————————————————————————————————————————————
    /// rh: 20161206
    /// —————————————————————————————————————————————————————————————————————————————————————
    /// </summary>
    public static class SQLDateUtil
    {
        #region Private Contstants

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string validA = "VALIDFROM";
        private const string validO = "VALIDUNTIL";

        #endregion

        #region Methods


        /// <summary>
        /// Check Current Date (SYSDATE) Valid
        /// </summary>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string CheckCurrentSysDate(string targetTableName)
        {
            return CheckDate("", targetTableName);
        }

        /// <summary>
        /// Check dateInQuestion IN BETWEEN ValidFrom AND ValidUntil
        /// </summary>
        /// <param name="dateInQuestion"></param>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string CheckDate(string dateInQuestion, string targetTableName)
        {
            return CheckBeforeValidFrom(dateInQuestion, targetTableName) + " AND " + CheckAfterValidUntil(dateInQuestion, targetTableName);
        }

        /// <summary>
        /// Create string to query clarion date ValidFrom
        /// (t.validA is null or t.validA <= dateInQuestion or t.validA <= to_date ('01.01.0111', 'dd.MM.yyyy'))
        /// </summary>
        /// <param name="dateInQuestion"></param>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string CheckBeforeValidFrom(string dateInQuestion, string targetTableName)
        {
            return CheckDateValid(true, dateInQuestion, targetTableName, validA);
        }

        /// <summary>
        /// Create string to query clarion date ValidUntil
        /// (t.validA is null or t.validA <= dateInQuestion or t.validA <= to_date ('01.01.0111', 'dd.MM.yyyy'))
        /// </summary>
        /// <param name="dateInQuestion"></param>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string CheckAfterValidUntil(string dateInQuestion, string targetTableName)
        {
            return CheckDateValid(false, dateInQuestion, targetTableName, validO);
        }

        /// <summary>
        /// CheckDateValid
        /// ASSERTS dateInQuestion IS NOT NULL or Empty => "SYSDATE";
        /// </summary>
        /// <param name="before"></param>
        /// <param name="dateInQuestion"></param>
        /// <param name="targetTableName"></param>
        /// <param name="targetDate"></param>
        /// <returns></returns>
        private static string CheckDateValid(bool before, string dateInQuestion, string targetTableName, string targetDate)
        {
            try
            {
                // CHECK if required tablename is filled (HINT: we could check for existance, too!?!) 
                if (string.IsNullOrEmpty(targetTableName))
                {
                    // we CANNOT CONTINUE here! --> Throw an exception
                    _log.Error("CheckDateValid" + (before ? "From" : "Until") + ": targetTableName seems NOT determined! (NULL OR Empty)");
                    throw new ApplicationException("SQL-FROM-DateCheck: Target-TableName NOT SET!");
                }
                string dateQuery = string.Empty;
                string tableDate = targetTableName + "." + targetDate;

                // ReACT ON empty date in question
                if (string.IsNullOrEmpty(dateInQuestion))
                {
                    dateInQuestion = "SYSDATE";
                    // _log.Debug ("rh DEBUG: CheckDateValid: dateInQuestion ADJUSTED to SYSDATE!");
                }

                if (before)
                {
                    // BEFORE / FROM / validA :
                    //  (t.VALIDFROM IS NULL OR (t.VALIDFROM IS NOT NULL AND t.VALIDFROM <= TO_DATE ('01.01.0111', 'dd.MM.yyyy'))
                    //		OR (t.VALIDFROM IS NOT NULL AND t.VALIDFROM <= TRUNC(&&myDate)) )
                    dateQuery = " (" + tableDate + " IS NULL OR (" + tableDate + " IS NOT NULL AND " + tableDate + " <= TO_DATE ('01.01.0111', 'dd.MM.yyyy')) ";
                    dateQuery += " OR (" + tableDate + " IS NOT NULL AND " + tableDate + "<= TRUNC(" + dateInQuestion + "))) ";
                }
                else
                {
                    // AFTER / UNTIL / validO :
                    //  ((t.VALIDUNTIL IS NULL OR (t.VALIDUNTIL IS NOT NULL AND t.VALIDUNTIL <= TO_DATE ('01.01.0111', 'dd.MM.yyyy')))
                    //		OR (t.VALIDUNTIL > TO_DATE ('01.01.0111', 'dd.MM.yyyy') AND t.VALIDUNTIL >= TRUNC(&&myDate))) 
                    dateQuery = " ((" + tableDate + " IS NULL OR (" + tableDate + " IS NOT NULL AND " + tableDate + " <= TO_DATE ('01.01.0111', 'dd.MM.yyyy'))) ";
                    dateQuery += " OR (" + tableDate + " > TO_DATE ('01.01.0111', 'dd.MM.yyyy') AND " + tableDate + " >= TRUNC(" + dateInQuestion + "))) ";
                }

                // _log.Debug("rh DEBUG: dateQuery: <" + dateQuery + ">");

                return dateQuery;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not generate the CheckDateValid-QueryString!", e);
            }
        }
        #endregion
    }
}
