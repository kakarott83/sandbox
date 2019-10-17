using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public enum DMSFieldType
    {
        String=0,
        Int=1,
        Long=2,
        Double=3,
        DateTime=4,
        Bytes=5
    }

    /// <summary>
    /// Describes a generic typed dms field/value pair
    /// </summary>
    public class DMSField
    {
        /// <summary>
        /// Name of the Field
        /// </summary>
        [XmlAttribute]
        public String name { get; set; }
        /// <summary>
        /// Type of the Fieldvalue
        /// </summary>
        [XmlAttribute]
        public DMSFieldType type { get; set; }
        /// <summary>
        /// Value of the Field
        /// </summary>
        public DMSValue value { get; set; }
    }

    /// <summary>
    /// Holds values for supported types
    /// </summary>
    public class DMSValue
    {
        public long? l { get; set; }

        public double? d { get; set; }

        public DateTime? t { get; set; }

        public int? i { get; set; }

        public String s { get; set; }

        public byte[] data { get; set; }

        public String getValue(DMSFieldType type)
        {
            if(type==DMSFieldType.Bytes)
                return data == null ? null : System.Text.Encoding.UTF8.GetString(data);
            if (type == DMSFieldType.DateTime)
                return t == null || !t.HasValue ? null : t.Value.ToString("yyyyMMdd HH:mm:ss.fffffffZ");
            if (type == DMSFieldType.Double)
                return d == null || !d.HasValue ? null : d.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (type == DMSFieldType.Int)
                return i==null||!i.HasValue?null:""+i;
            if (type == DMSFieldType.Long)
                return l==null||!l.HasValue?null:""+l;
            if (type == DMSFieldType.String)
                return s;

            return null;
        }
    }
}