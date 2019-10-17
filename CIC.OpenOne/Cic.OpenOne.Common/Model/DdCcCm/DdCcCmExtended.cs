using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Model.DdCcCm
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class DdCcCmExtended : CcCmEntities
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DdCcCmExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof(CcCmEntities), "DdCcCm"))
        {
        }
    }
}
