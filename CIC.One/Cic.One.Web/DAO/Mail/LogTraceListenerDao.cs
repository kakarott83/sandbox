using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.One.Web.DAO.Mail
{
    public class LogTraceListenerDao : ITraceListenerDao
    {
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Trace(string traceType, string traceMessage)
        {
            //TODO
        }
    }
}