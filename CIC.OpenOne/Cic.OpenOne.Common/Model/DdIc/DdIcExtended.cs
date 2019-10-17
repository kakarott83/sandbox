using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Model.DdIc
{
    /// <summary>
    /// DDIC Extended Entities class
    /// </summary>
    public class DdIcExtended : IcEntities
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DdIcExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof(IcEntities), "DdIc"))
        {
        }
    }
}

