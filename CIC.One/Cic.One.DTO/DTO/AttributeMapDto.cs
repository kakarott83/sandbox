using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Cic.One.DTO
{
    public class AttributeValue
    {
        public String value { get; set; }
        public String id { get; set; }
        public int flag1 { get; set; }
        public int flag2 { get; set; }
        public String str1 { get; set; }
        public String str2 { get; set; }
    }

    /// <summary>
    /// Parameterklasse Attributemap
    /// </summary>
    public class AttributeMapDto
    {
        /// <summary>
        /// Attributname
        /// </summary>
        public AttributeName attributeName;
        /// <summary>
        /// Attributwerte
        /// </summary>
        public List<AttributeValue> attributeValues;
        /// <summary>
        /// Vorgabewert
        /// </summary>
        public String defaultValue;
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

        
        [Description("BildweltCode")]
        BildweltCode,

        /// <summary>
        /// Chat Code
        /// </summary>
        [Description("ChatCode")]
        ChatCode,

        /// <summary>
        /// Vertriebskanäle
        /// </summary>
        [Description("Channels")]
        Channels,


        /// <summary>
        /// KAM
        /// </summary>
        [Description("KAM")]
        KAM,

        /// <summary>
        /// BRAND
        /// </summary>
        [Description("BRAND")]
        BRAND,

        /// <summary>
        /// EPOS_ADMIN
        /// </summary>
        [Description("EPOS_ADMIN")]
        EPOS_ADMIN,

        /// <summary>
        /// ABWICKLUNGSORT
        /// </summary>
        [Description("ABWICKLUNGSORT")]
        ABWICKLUNGSORT,

        /// <summary>
        /// EPOS_BEDINGUNGEN
        /// </summary>
        [Description("EPOS_BEDINGUNGEN")]
        EPOS_BEDINGUNGEN,

        /// <summary>
        /// BPEROLES
        /// </summary>
        [Description("BPEROLES")]
        BPEROLES,

        /// <summary>
        /// BPELANES
        /// </summary>
        [Description("BPELANES")]
        BPELANES,

		/// <summary>
		/// USER'S FILIALEN
		/// </summary>
		[Description ("USERFILIALEN")]
		USERFILIALEN,


        /// <summary>
        /// Default HG (SYSPRHGROUP)
        /// </summary>
        [Description("STANDARD_HG")]
        STANDARD_HG,

        /// <summary>
        /// MA HG (Sysprhgroup)
        /// </summary>
        [Description("MA_HG")]
        MA_HG

    }
}