
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Postleitzahldto mit Zusatzdaten wie Land Kanton und Ort
    /// </summary>
    public class PlzDto
    {
        /// <summary>
        /// Postleitzahl
        /// </summary>
        public string plz { get; set; }

        /// <summary>
        /// Kanton
        /// </summary>
        public string bezirk { get; set; }

        /// <summary>
        /// Ort
        /// </summary>
        public string ort { get; set; }

        /// <summary>
        /// Land
        /// </summary>
        public string land { get; set; }

        /// <summary>
        /// sysland
        /// </summary>
        public long sysland { get; set; }
    }
}