using System.Diagnostics;

namespace Cic.One.Web.DAO.Mail
{
    /// <summary>
    /// Speichert die Traces in Textdateien ab.
    /// </summary>
    public class DebugTraceListenerDao : ITraceListenerDao
    {
        /// <summary>
        /// wird aufgerufen von dem jeweiligen Mailservice
        /// </summary>
        /// <param name="traceType">Was für eine Art war der Trace? Request/Response?</param>
        /// <param name="traceMessage">Nachricht des Traces</param>
        public void Trace(string traceType, string traceMessage)
        {
            Debug.WriteLine("");
            Debug.WriteLine(traceType);
            Debug.WriteLine(traceMessage);
            Debug.WriteLine("");
        }
    }
}