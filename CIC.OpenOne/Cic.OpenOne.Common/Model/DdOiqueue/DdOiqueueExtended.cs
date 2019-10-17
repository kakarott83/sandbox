using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Model.DdOiqueue
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class DdOiQueueExtended : OiqueueEntities
    {
        /// <summary>
        /// DdOiQueueExtended-Konstruktor
        /// </summary>
        public DdOiQueueExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof(OiqueueEntities), "DdOiqueue"))
        {
        }
    }
}