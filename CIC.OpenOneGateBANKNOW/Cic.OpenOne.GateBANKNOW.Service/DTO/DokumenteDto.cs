namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für Dokumente
    /// </summary>
    public class DokumenteDto
    {
        /// <summary>
        /// Bezeichnnung
        /// </summary>
        public string Bezeichnung { get; set; }
        /// <summary>
        /// EAI Quote ID
        /// </summary>
        public long sysEaiquo { get; set; }
        /// <summary>
        /// Dokumenten ID
        /// </summary>
        public long DokumentenID { get; set; }
        /// <summary>
        /// VOrgabesprache
        /// </summary>
        public string DefaultSprache { get; set; }
        /// <summary>
        /// VOrgabesprache
        /// </summary>
        public string AdditionalSprache { get; set; }
        /// <summary>
        /// Kundenexemplar
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
    }
}