using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.Util.Exceptions
{
    public class ExceptionUtil
    {
        /// <summary>
        /// Delivers all Exception Messages of the Exception Stack as String
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string DeliverFlatExceptionMessage(System.Exception exception)
        {
            string FlatExceptionMessage = string.Empty;
            // NOTE MK, Eraly return
            if (exception == null)
            {
                return string.Empty;
            }

            FlatExceptionMessage += exception.Message;

            if (exception.InnerException != null)
            {
                FlatExceptionMessage += System.Environment.NewLine;
                FlatExceptionMessage += DeliverFlatExceptionMessage(exception.InnerException);
            }

            return FlatExceptionMessage;
        }
    }
}
