
namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// SoapXMLDto-Klasse
    /// </summary>
    public class SoapXMLDto
    {
        /// <summary>
        /// sysid
        /// </summary>
        public long sysid { set; get; }

        /// <summary>
        /// entity
        /// </summary>
        public string entity { set; get; }

        /// <summary>
        /// code
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// logDumpFlag: wird beim Schreiben in die LogDump bei ZekBatch-Anfragen gebraucht
        /// </summary>
        public bool logDumpFlag { set; get; }

        /// <summary>
        /// requestXML
        /// </summary>
        public string requestXML { set; get; }

        /// <summary>
        /// responseXML
        /// </summary>
        public string responseXML { set; get; }
    }
}
