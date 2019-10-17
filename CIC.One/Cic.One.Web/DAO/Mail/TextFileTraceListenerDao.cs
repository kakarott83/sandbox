using System;
using System.IO;

namespace Cic.One.Web.DAO.Mail
{
    /// <summary>
    /// Speichert die Traces in Textdateien ab.
    /// </summary>
    public class TextFileTraceListenerDao : ITraceListenerDao
    {
        private string path = "";

        public TextFileTraceListenerDao(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// wird aufgerufen von dem jeweiligen Mailservice
        /// </summary>
        /// <param name="traceType">Was für eine Art war der Trace? Request/Response?</param>
        /// <param name="traceMessage">Nachricht des Traces</param>
        public void Trace(string traceType, string traceMessage)
        {
            CreateXMLTextFile(traceType, traceMessage);
        }

        /// <summary>
        /// Speichert den Trace in einer Datei ab
        /// </summary>
        /// <param name="fileName">Enthält den Tracetyp, welcher als Dateiname verwendet wird</param>
        /// <param name="traceContent">Enthält die Nachricht des Traces</param>
        private void CreateXMLTextFile(string fileName, string traceContent)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                System.IO.File.WriteAllText(path + fileName + DateTime.Now.Ticks + ".txt", traceContent);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}