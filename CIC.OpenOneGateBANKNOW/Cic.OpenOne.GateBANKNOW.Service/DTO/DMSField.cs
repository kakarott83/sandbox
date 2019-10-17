using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Supported DMS Datatypes
    /// </summary>
    public enum DMSFieldType
    {
        String=0,
        Int=1,
        Long=2,
        Double=3,
        DateTime=4,
        Bytes=5,
        Date=6,
        Time=7
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
    }
}