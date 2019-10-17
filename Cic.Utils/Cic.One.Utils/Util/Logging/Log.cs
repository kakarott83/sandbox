using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cic.OpenOne.Common.Util.Logging
{
    /// <summary>
    /// Central Logging class using Log4NET
    /// Usage:
    /// private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    /// </summary>
    public sealed class Log : ILog, System.ICloneable
    {
        #region Private enums

        private enum ContructionTypeConstants : int
        {
            LoggerName = 1,
            LoggerNameFileInfo = 2,
        }
        #endregion

        #region Private variables
        private Log.ContructionTypeConstants _ContructionTypeConstant;
        private string _LoggerName;
        private System.IO.FileInfo _FileInfo;
        private log4net.ILog _Log;
        private bool _Configured = false;
        #endregion

        #region Constructors

        /// <summary>
        /// Log-Konstruktor
        /// </summary>
        /// <param name="loggerName"></param>
        public Log(string loggerName)
        {
            try
            {
                // Set constant
                _ContructionTypeConstant = Log.ContructionTypeConstants.LoggerName;
                // Internal
                MyConstruct(loggerName, null);
                // Set values
                _LoggerName = loggerName;
                _FileInfo = null;
            }
            catch
            {
                // Throw caught excpetion
                throw;
            }
        }

        /// <summary>
        /// Returns a new Logger Instance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILog GetLogger(Type type)
        {
            return new Log(type);
        }

        /// <summary>
        /// Creates a Log Facade for the given type
        /// Use by e.g. declaring
        /// private static readonly ILog Log = new Log(MethodBase.GetCurrentMethod().DeclaringType);
        /// </summary>
        /// <param name="type"></param>
        public Log(Type type)
        {
            try
            {
                // Set constant
                _ContructionTypeConstant = Log.ContructionTypeConstants.LoggerName;
                // Internal
                _Log = log4net.LogManager.GetLogger(type);
                _Configured = true;
                // Set values
                _LoggerName = type.FullName;
                _FileInfo = null;
            }
            catch
            {
                // Throw caught excpetion
                throw;
            }
        }

        /// <summary>
        /// Log-Konstruktor
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="fileInfo"></param>
        public Log(string loggerName, System.IO.FileInfo fileInfo)
        {
            try
            {
                // Set constant
                _ContructionTypeConstant = Log.ContructionTypeConstants.LoggerNameFileInfo;
                // Internal
                MyConstruct(loggerName, fileInfo);
                // Set values
                _LoggerName = loggerName;
                _FileInfo = fileInfo;
            }
            catch
            {
                // Throw caught excpetion
                throw;
            }
        }
        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Log Log = null;

            // Check constant
            switch (_ContructionTypeConstant)
            {
                //LoggerName = 1,
                case Log.ContructionTypeConstants.LoggerName:
                    Log = new Log(_LoggerName);
                    break;
                //LoggerNameFileInfo = 2,
                case Log.ContructionTypeConstants.LoggerNameFileInfo:
                    Log = new Log(_LoggerName, _FileInfo);
                    break;
            }
            return Log;
        }
        #endregion

        #region ILog methods

        /// <summary>
        /// LogLoggerInformation
        /// </summary>
        public void LogLoggerInformation()
        {
            string DeliverLoggerInformation;

            // Check state
            if (_Configured)
            {
                // Get logger information
                DeliverLoggerInformation = MyDeliverLoggerInformation();
                // Check string
                if (!string.IsNullOrEmpty(DeliverLoggerInformation))
                {
                    // TODO BK 0 BK, Localize text
                    DeliverLoggerInformation = ("Configured logger: " + DeliverLoggerInformation);
                    // Invoke method
                    _Log.Info(DeliverLoggerInformation);
                }
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Debug(message);
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(object message, System.Exception exception)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Debug(message, exception);
            }
        }

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        public void DebugFormat(string format, object argument0)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.DebugFormat(format, argument0);
            }
        }

        // NOTE BK, This method is defined by log4net and it is overloaded
        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.DebugFormat(System.String,System.Object[])")]
        public void DebugFormat(string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.DebugFormat(format, arguments);
            }
        }

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public void DebugFormat(System.IFormatProvider formatProvider, string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.DebugFormat(formatProvider, format, arguments);
            }
        }

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        public void DebugFormat(string format, object argument0, object argument1)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.DebugFormat(format, argument0, argument1);
            }
        }

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public void DebugFormat(string format, object argument0, object argument1, object argument2)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.DebugFormat(format, argument0, argument1, argument2);
            }
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Error(message);
            }
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, System.Exception exception)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Error(message, exception);
            }
        }

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        public void ErrorFormat(string format, object argument0)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.ErrorFormat(format, argument0);
            }
        }

        // NOTE BK, This method is defined by log4net and it is overloaded
        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.ErrorFormat(System.String,System.Object[])")]
        public void ErrorFormat(string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.ErrorFormat(format, arguments);
            }
        }

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public void ErrorFormat(System.IFormatProvider formatProvider, string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.ErrorFormat(formatProvider, format, arguments);
            }
        }

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        public void ErrorFormat(string format, object argument0, object argument1)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.ErrorFormat(format, argument0, argument1);
            }
        }

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public void ErrorFormat(string format, object argument0, object argument1, object argument2)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.ErrorFormat(format, argument0, argument1, argument2);
            }
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Fatal(message);
            }
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, System.Exception exception)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Fatal(message, exception);
            }
        }

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        public void FatalFormat(string format, object argument0)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.FatalFormat(format, argument0);
            }
        }

        // NOTE BK, This method is defined by log4net and it is overloaded
        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.FatalFormat(System.String,System.Object[])")]
        public void FatalFormat(string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.FatalFormat(format, arguments);
            }
        }

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public void FatalFormat(System.IFormatProvider formatProvider, string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.FatalFormat(formatProvider, format, arguments);
            }
        }

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        public void FatalFormat(string format, object argument0, object argument1)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.FatalFormat(format, argument0, argument1);
            }
        }

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public void FatalFormat(string format, object argument0, object argument1, object argument2)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.FatalFormat(format, argument0, argument1, argument2);
            }
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Info(message);
            }
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, System.Exception exception)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Info(message, exception);
            }
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        public void InfoFormat(string format, object argument0)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.InfoFormat(format, argument0);
            }
        }


        // NOTE BK, This method is defined by log4net and it is overloaded
        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.InfoFormat(System.String,System.Object[])")]
        public void InfoFormat(string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.InfoFormat(format, arguments);
            }
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public void InfoFormat(System.IFormatProvider formatProvider, string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.InfoFormat(formatProvider, format, arguments);
            }
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        public void InfoFormat(string format, object argument0, object argument1)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.InfoFormat(format, argument0, argument1);
            }
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public void InfoFormat(string format, object argument0, object argument1, object argument2)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.InfoFormat(format, argument0, argument1, argument2);
            }
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Warn(message);
            }
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, System.Exception exception)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.Warn(message, exception);
            }
        }

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        public void WarnFormat(string format, object argument0)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.WarnFormat(format, argument0);
            }
        }


        // NOTE BK, This method is defined by log4net and it is overloaded
        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.WarnFormat(System.String,System.Object[])")]
        public void WarnFormat(string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.WarnFormat(format, arguments);
            }
        }

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        public void WarnFormat(System.IFormatProvider formatProvider, string format, params object[] arguments)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.WarnFormat(formatProvider, format, arguments);
            }
        }

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        public void WarnFormat(string format, object argument0, object argument1)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.WarnFormat(format, argument0, argument1);
            }
        }

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public void WarnFormat(string format, object argument0, object argument1, object argument2)
        {
            // Check state
            if (_Configured)
            {
                // Invoke method
                _Log.WarnFormat(format, argument0, argument1, argument2);
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Configured
        /// </summary>
        public bool Configured
        {
            get
            {
                return _Configured;
            }
        }

        /// <summary>
        /// IsDebugEnabled
        /// </summary>
        public bool IsDebugEnabled
        {
            get
            {
                // Check state
                if (_Configured)
                {
                    // Return
                    // NOTE BK, Two returns
                    return _Log.IsDebugEnabled;
                }
                else
                {
                    // Return
                    // NOTE BK, Two returns
                    return false;
                }
            }
        }

        /// <summary>
        /// IsErrorEnabled
        /// </summary>
        public bool IsErrorEnabled
        {
            get
            {
                // Check state
                if (_Configured)
                {
                    // Return
                    // NOTE BK, Two returns
                    return _Log.IsErrorEnabled;
                }
                else
                {
                    // Return
                    // NOTE BK, Two returns
                    return false;
                }
            }
        }

        /// <summary>
        /// IsFatalEnabled
        /// </summary>
        public bool IsFatalEnabled
        {
            get
            {
                // Check state
                if (_Configured)
                {
                    // Return
                    // NOTE BK, Two returns
                    return _Log.IsFatalEnabled;
                }
                else
                {
                    // Return
                    // NOTE BK, Two returns
                    return false;
                }
            }
        }

        /// <summary>
        /// IsInfoEnabled
        /// </summary>
        public bool IsInfoEnabled
        {
            get
            {
                // Check state
                if (_Configured)
                {
                    // Return
                    // NOTE BK, Two returns
                    return _Log.IsInfoEnabled;
                }
                else
                {
                    // Return
                    // NOTE BK, Two returns
                    return false;
                }
            }
        }

        /// <summary>
        /// IsWarnEnabled
        /// </summary>
        public bool IsWarnEnabled
        {
            get
            {
                // Check state
                if (_Configured)
                {
                    return _Log.IsWarnEnabled;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Dumps all infos of an object
        /// </summary>
        /// <param name="o">the object to dump</param>
        /// <returns>returns the dump string</returns>
        public String dumpObject(Object o)
        {
            if (o == null || !IsDebugEnabled) return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Dump of " + o.GetType() + Environment.NewLine);
            Type t = o.GetType();
            PropertyInfo[] pis = t.GetProperties();
            List<PropertyInfo> l = new List<PropertyInfo>(pis);

            var order = from pi in l
                        select pi;
            IEnumerable orderedProperties = order.OrderBy(r => r.Name);

            foreach (PropertyInfo pi in orderedProperties)
            {
                try
                {
                    sb.AppendFormat("{0}: {1}" + Environment.NewLine, pi.Name, pi.GetValue(o, new object[] { }));
                }
                catch (Exception)
                {
                }
            }
            return sb.ToString();
        }

        #endregion

        #region My methods

        private void MyConstruct(string loggerName, System.IO.FileInfo fileInfo)
        {
            log4net.Appender.EventLogAppender EventLogAppender;
            log4net.Filter.LevelRangeFilter LevelRangeFilter;
            log4net.Layout.SimpleLayout SimpleLayout;
            log4net.Repository.Hierarchy.Logger Logger;
            System.IO.FileStream FileStream = null;

            // Check logger name
            if (!(string.IsNullOrEmpty(loggerName) || string.IsNullOrEmpty(loggerName.Trim())))
            {
                // Trim logger name
                loggerName = loggerName.Trim();

                try
                {
                    // Get logger
                    _Log = log4net.LogManager.GetLogger(loggerName);
                }
                catch
                {
                    // Ignore exception
                }

                // Check object
                if (_Log != null)
                {
                    // Check file info
                    if ((fileInfo != null) && fileInfo.Exists)
                    {
                        try
                        {
                            // Open stream
                            FileStream = fileInfo.OpenRead();
                            // Configure
                            // NOTE: Use file stream, to avoid a console message, if the file is used by another process
                            log4net.Config.XmlConfigurator.Configure(FileStream);
                            // Get logger
                            Logger = (log4net.Repository.Hierarchy.Logger)_Log.Logger;
                            // Set state
                            _Configured = (Logger.Repository.Configured && (Logger.Appenders.Count > 0));
                        }
                        catch
                        {
                            // Ignore exception
                        }
                        finally
                        {
                            // Checkl stream
                            if (FileStream != null)
                            {
                                // Close
                                FileStream.Close();
                            }
                        }
                    }
                    // Check state
                    if (!_Configured)
                    {
                        // Create simple layout
                        SimpleLayout = new log4net.Layout.SimpleLayout();
                        // Activate options
                        SimpleLayout.ActivateOptions();

                        // Create level range filter
                        LevelRangeFilter = new log4net.Filter.LevelRangeFilter();
                        // Set min level
                        LevelRangeFilter.LevelMin = log4net.Core.Level.Info;
                        // Set max level
                        LevelRangeFilter.LevelMax = log4net.Core.Level.Fatal;
                        // Activate options
                        LevelRangeFilter.ActivateOptions();

                        // Create event log appender
                        EventLogAppender = new log4net.Appender.EventLogAppender();
                        // Set name
                        EventLogAppender.Name = "EventLogAppender";
                        // Set application name
                        EventLogAppender.ApplicationName = loggerName;
                        // Set layout
                        EventLogAppender.Layout = SimpleLayout;
                        // Clear filters
                        EventLogAppender.ClearFilters();
                        // Add filter
                        EventLogAppender.AddFilter(LevelRangeFilter);
                        // Activate options
                        EventLogAppender.ActivateOptions();

                        // Get logger
                        Logger = (log4net.Repository.Hierarchy.Logger)_Log.Logger;
                        // Additivity
                        Logger.Additivity = false;
                        // Remove all appenders
                        Logger.RemoveAllAppenders();
                        // Add appender
                        Logger.AddAppender(EventLogAppender);

                        // Configure
                        log4net.Config.BasicConfigurator.Configure(Logger.Repository, EventLogAppender);
                        // Set state
                        _Configured = (Logger.Repository.Configured && (Logger.Appenders.Count > 0));
                    }
                }
            }
        }

        private string MyDeliverLoggerInformation()
        {
            log4net.Repository.Hierarchy.Logger Logger;
            string LoggerInformation = null;

            // Check state
            if (_Configured)
            {
                // Initialize string
                LoggerInformation = string.Empty;
                // Get logger
                Logger = (log4net.Repository.Hierarchy.Logger)_Log.Logger;
                // Loop through appenders
                foreach (log4net.Appender.IAppender LoopAppender in Logger.Appenders)
                {
                    // Add informations
                    LoggerInformation = (LoggerInformation + " [" + LoopAppender.Name + ", " + LoopAppender.ToString() + "]");
                }
                // Invoke method
                LoggerInformation = (Logger.Name + LoggerInformation);
            }
            return LoggerInformation;
        }
        #endregion
    }
}