namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Parameterklasse für RightsMap
    /// </summary>
    public class RightsMap
    {
        /// <summary>
        /// id = codeRMO + codeRFU
        /// </summary>
        public string rightsMapId { get; set; }

        /// <summary>
        /// Code für Rechte-Modul, z.B. „B2B“
        /// </summary>
        public string codeRMO { get; set; }
        /// <summary>
        /// Code für Rechte-Funktion, z.B. „Vertrag_Vertragslisten“
        /// </summary>
        public string codeRFU { get; set; }

        /// <summary>
        /// Eine Kodierung der RFU- und RRO-Rechte im Format "1010110000"
        /// </summary>
        public string rechte { get; set; }

        /// <summary>
        /// Rechtefunktion Select
        /// </summary>
        public bool rfuS { get; set; }
        /// <summary>
        /// Rechtefunktion Change
        /// </summary>
        public bool rfuC { get; set; }
        /// <summary>
        /// Rechtefunktion Delete
        /// </summary>
        public bool rfuD { get; set; }
        /// <summary>
        /// Rechtefunktion Insert
        /// </summary>
        public bool rfuI { get; set; }
        /// <summary>
        /// Rechtefunktion Execute
        /// </summary>
        public bool rfuX { get; set; }

        /// <summary>
        /// Rollenfunktion Select
        /// </summary>
        public bool rroS { get; set; }
        /// <summary>
        /// Rollenfunktion Change
        /// </summary>
        public bool rroC { get; set; }
        /// <summary>
        /// Rollenfunktion Delete
        /// </summary>
        public bool rroD { get; set; }
        /// <summary>
        /// Rollenfunktion Insert
        /// </summary>
        public bool rroI { get; set; }
        /// <summary>
        /// Rollenfunktion Execute
        /// </summary>
        public bool rroX { get; set; }
    }
}