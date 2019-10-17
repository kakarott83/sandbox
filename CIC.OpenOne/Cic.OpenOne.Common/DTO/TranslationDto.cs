using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Holds translation Information for one GUI Field
    /// </summary>
    public class TranslationDto
    {
        /// <summary>
        /// Front ID
        /// </summary>
        public String frontId { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public int typ { get; set; }
        /// <summary>
        /// Master 
        /// </summary>
        public String master { get; set; }

        /// <summary>
        /// Holds all available Languages for the master field
        /// </summary>
        public List<TranslationValue> translations { get; set; }
    }

    /// <summary>
    /// Holds the translation for one GUI Field
    /// </summary>
    public class TranslationValue
    {
        /// <summary>
        /// FrontID
        /// </summary>
        public String frontId { get; set; }
        /// <summary>
        /// Iso Code
        /// </summary>
        public String isoCode { get; set; }
        /// <summary>
        /// Üebersetzung
        /// </summary>
        public String translation { get; set; }

        /// <summary>
        /// Translation content for very long data
        /// </summary>
        public byte[] longTranslation { get; set; }
    }
}
