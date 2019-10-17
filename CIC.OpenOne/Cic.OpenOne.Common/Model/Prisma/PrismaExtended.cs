using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Extension;
using System.Data;

namespace Cic.OpenOne.Common.Model.Prisma
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class PrismaExtended : PrismaEntities,IAlteredSession
    {
        /// <summary>
        /// PrismaExtended-Konstruktor
        /// </summary>
        public PrismaExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof (PrismaEntities), "Prisma"))
        {
            ((System.Data.Objects.ObjectContext)this).RegisterObjectContext(this);
        }
        public void EntityConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            this.PerformStateChange(e);
        }
    }
}