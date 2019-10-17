namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für Dokumente
    /// </summary>
    public class DokumenteDto
    {
        /// <summary>
        /// Bezeichnung
        /// </summary>
        public string Bezeichnung { get; set; }
        /// <summary>
        /// EAI Queue Out ID
        /// </summary>
        public long sysEaiquo { get; set; }
        /// <summary>
        /// Dokumenten ID
        /// </summary>
        public long DokumentenID { get; set; }
        /// <summary>
        /// Default Sprache
        /// </summary>
        public string DefaultSprache { get; set; }
        /// <summary>
        /// Default Sprache
        /// </summary>
        public string AdditionalSprache { get; set; }
        /// <summary>
        /// Kunden Exemplar
        /// </summary>
        public int KundenExemplar { get; set; }
        /// <summary>
        /// Vertriebspartner Exemplar
        /// </summary>
        public int VertriebspatnerExemplar { get; set; }
        /// <summary>
        /// Bank Now Mitarbeiter Exemplar
        /// </summary>
        public int BnowMitarbeiterExemplar { get; set; }
        /// <summary>
        /// Druck
        /// </summary>
        public int Druck { get; set; }
        /// <summary>
        /// Exemplar Mitantragsteller
        /// </summary>
        public int MAExemplar { get; set; }
        /// <summary>
        /// Exemplar Bürge
        /// </summary>
        public int BGExemplar { get; set; }
    }
}