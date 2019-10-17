using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class Query
    {
        public String table { get; set; }
        public String pkey { get; set; }
        public String tables { get; set; }
        public String fields { get; set; }
        public String where { get; set; }
        public String group { get; set; }
        /// <summary>
        /// Complete Query, no separation of sql sections
        /// </summary>
        public String query { get; set; }
        public String permissioncondition { get; set; }

		/// <summary>
		/// Post process (CAS calls, ...) 
		/// </summary>
		public List<PostprocessCommand> postprocess { get; set; }

        /// <summary>
        /// Parameter replacement Expressions for the Query, where the parameters in the Query must be named p1, p2.... (source: WFLIST)
        /// </summary>
        public List<String> expressions { get; set; }

    }
	public class PostprocessCommand
	{
		[XmlAttribute]
		public String type { get; set; }
		
		public String area { get; set; }
		public String field { get; set; }

		public String command { get; set; }
	}
}
