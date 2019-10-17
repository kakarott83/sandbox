using Microsoft.Exchange.WebServices.Data;

namespace Cic.One.Web.DAO.Mail
{
    /// <summary>
    /// Interface für den Tracer
    /// </summary>
    public interface ITraceListenerDao : ITraceListener
    {
        /// <summary>
        /// wird aufgerufen von dem jeweiligen Mailservice
        /// </summary>
        /// <param name="traceType">Was für eine Art war der Trace? Request/Response?</param>
        /// <param name="traceMessage">Nachricht des Traces</param>
        new void Trace(string traceType, string traceMessage);
    }
}