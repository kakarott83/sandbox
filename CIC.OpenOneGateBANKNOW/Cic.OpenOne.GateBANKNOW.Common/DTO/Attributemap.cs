using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// AttributeValue-Klasse
    /// </summary>
    public class AttributeValue
    {
        /// <summary>
        /// value
        /// </summary>
        public String value { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public long id { get; set; }
    }

    /// <summary>
    /// Parameterklasse Attributemap
    /// </summary>
    public class AttributeMap
    {
        /// <summary>
        /// Attributname
        /// </summary>
        public AttributeName attributeName { get; set; }
        /// <summary>
        /// Attributwerte
        /// </summary>
        public List<AttributeValue> attributeValues { get; set; }
        /// <summary>
        /// Vorgabewert
        /// </summary>
        public String defaultValue { get; set; }
    }

    /// <summary>
    /// Enum Attributnamen 
    /// </summary>
    public enum AttributeName
    {
        /// <summary>
        /// ISO Language Code
        /// </summary>
        [Description("ISOLanguageCode")]
        ISOLanguageCode,

        ///NEU:
        [Description("BildweltCode")]
        BildweltCode
    }
}