using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Model.DdEurotax
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class DdEurotaxExtended : EurotaxEntities
    {
        /// <summary>
        /// DdEurotaxExtended-Konstruktor
        /// </summary>
        public DdEurotaxExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof(EurotaxEntities), "DdEurotax"))
        {
        }
    }
}
