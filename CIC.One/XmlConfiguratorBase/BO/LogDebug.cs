using System;
using System.IO;
using System.Reflection;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.BO
{
    internal class LogDebug
    {
        internal static string GetError(Exception e)
        {
            if (e == null)
                return "";

            string error = "Error encountered " + DateTime.Now.ToString() + ": " + e.Source + "\r\n";
            string stacktrace = "";
            while (e != null)
            {
                error += e.Message + "\r\n";
                stacktrace += "\r\n" + e.StackTrace;

                if (e is ReflectionTypeLoadException)
                {
                    error += "Loader Exceptions: [";
                    error += GetLoaderExceptions((ReflectionTypeLoadException)e);
                    error += "]";
                }

                e = e.InnerException;
            }

            if (ConstantsDto.DEBUGGING && stacktrace.Trim().Length > 0)
            {
                error += "\r\n\r\n" + "Stack trace:" + stacktrace + "\r\n)";
            }
            return error;
        }

        private static string GetLoaderExceptions(ReflectionTypeLoadException e)
        {
            string error = "";
            foreach (Exception e2 in e.LoaderExceptions)
            {
                error += e2 + "\r\n";
            }
            return error;
        }
    }
}
