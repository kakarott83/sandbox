using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    /// <summary>
    /// Holds the current value for a generic view input field
    /// </summary>
    public class ViewValue
    {
        public ViewValue() { }

        public ViewValue clone()
        {
            ViewValue rval = new ViewValue();
            rval.l = l;
            rval.d = d;
            rval.t = t;
            rval.i = i;
            rval.s = s;
            rval.data = data;
            return rval;
        }
        public long? l { get; set; }
        
        public double? d { get; set; }
       
        public DateTime? t { get; set; }
       
        public int? i { get; set; }

        public String s { get; set; }

        public byte[] data { get; set; }
    }
}
