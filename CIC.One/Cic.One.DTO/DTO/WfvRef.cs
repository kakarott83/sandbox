using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    /// <summary>
    /// References to another WfvEntry
    /// </summary>
    public class WfvRef
    {
        /// <summary>
        /// name of the template
        /// </summary>
        public String syscode { get; set; }
        /// <summary>
        /// column in the dashboard
        /// </summary>
        public int column { get; set; }
        /// <summary>
        /// Row in the dashboard
        /// </summary>
        public int row { get; set; }
        /// <summary>
        /// Panel visible in dashboard
        /// </summary>
        public int visibility { get; set; }
        /// <summary>
        /// Panel collapsed
        /// </summary>
        public int collapsed { get; set; }
        /// <summary>
        /// Panel only shows as popup
        /// </summary>
        public int popup { get; set; }
        /// <summary>
        /// Panel is the dashboards main reference (AREA)
        /// </summary>
        public int mainref { get; set; }

        /// <summary>
        /// Filtering a list of this view by the given filter criteria of another dashboard item
        /// </summary>
        public RefFilter filter { get; set; }

        /// <summary>
        /// Allows overriding the wfv's configuration
        /// </summary>
        public CustomEntry customentry { get; set; }
    }
}