
namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// DTO to store Message Translation Data
    /// </summary>
    public class MessageTranslateDto
    {
        /// <summary>
        /// MessageCode
        /// </summary>
        public string MessageCode { get; set; }
        /// <summary>
        /// MessageArea
        /// </summary>
        public string MessageArea { get; set; }
        /// <summary>
        /// MessageTyp
        /// </summary>
        public long MessageTyp { get; set; }
        /// <summary>
        /// Replacement Message
        /// </summary>
        public string ReplacementMessageText { get; set; }
        /// <summary>
        /// Replacement Title
        /// </summary>
        public string ReplacementMessageTitle { get; set; }
        /// <summary>
        /// ISO Sprachencode
        /// </summary>
        public string IsoCode { get; set; }
    }
}