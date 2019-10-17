using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    public class ViewMeta
    {
        public Query query { get; set; }
        public List<Table> tables { get; set; }
        public List<Viewfield> fields { get; set; }
        /// <summary>
        /// OL AREA
        /// </summary>
        public String area  { get; set; }



    }
}
