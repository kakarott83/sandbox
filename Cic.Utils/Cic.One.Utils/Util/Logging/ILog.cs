using System;

namespace Cic.OpenOne.Common.Util.Logging
{
    /// <summary>
    /// Logging interface all log implementations shall use
    /// </summary>
    public interface ILog
    {
        #region Methods

        /// <summary>
        /// LogLoggerInformation
        /// </summary>
        void LogLoggerInformation();

        /// <summary>
        /// dumpObject
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        String dumpObject(Object o);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(object message, System.Exception exception);

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        void DebugFormat(string format, object argument0);

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void DebugFormat(string format, params object[] arguments);

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void DebugFormat(System.IFormatProvider formatProvider, string format, params object[] arguments);

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        void DebugFormat(string format, object argument0, object argument1);

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        void DebugFormat(string format, object argument0, object argument1, object argument2);

        //The "Error"-Method is specified by the log4net.ILog interface. Do not change it.
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(object message);

        //The "Error"-Method is specified by the log4net.ILog interface. Do not change it.
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(object message, System.Exception exception);

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        void ErrorFormat(string format, object argument0);

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void ErrorFormat(string format, params object[] arguments);

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void ErrorFormat(System.IFormatProvider formatProvider, string format, params object[] arguments);

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        void ErrorFormat(string format, object argument0, object argument1);

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        void ErrorFormat(string format, object argument0, object argument1, object argument2);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        void Fatal(object message);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Fatal(object message, System.Exception exception);

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        void FatalFormat(string format, object argument0);

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void FatalFormat(string format, params object[] arguments);

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void FatalFormat(System.IFormatProvider formatProvider, string format, params object[] arguments);

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        void FatalFormat(string format, object argument0, object argument1);

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        void FatalFormat(string format, object argument0, object argument1, object argument2);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        void Info(object message);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Info(object message, System.Exception exception);

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        void InfoFormat(string format, object argument0);

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void InfoFormat(string format, params object[] arguments);

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void InfoFormat(System.IFormatProvider formatProvider, string format, params object[] arguments);

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        void InfoFormat(string format, object argument0, object argument1);

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        void InfoFormat(string format, object argument0, object argument1, object argument2);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        void Warn(object message);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Warn(object message, System.Exception exception);

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        void WarnFormat(string format, object argument0);

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void WarnFormat(string format, params object[] arguments);

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        void WarnFormat(System.IFormatProvider formatProvider, string format, params object[] arguments);

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        void WarnFormat(string format, object argument0, object argument1);

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        void WarnFormat(string format, object argument0, object argument1, object argument2);
        #endregion

        #region Properties

        /// <summary>
        /// Checks if this logger is enabled for the Debug level. 
        /// </summary>
        bool IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the Error level. 
        /// </summary>
        bool IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the Fatal level. 
        /// </summary> 
        bool IsFatalEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the Info level. 
        /// </summary>
        bool IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the Warn level. 
        /// </summary>
        bool IsWarnEnabled
        {
            get;
        }
        #endregion
    }
}