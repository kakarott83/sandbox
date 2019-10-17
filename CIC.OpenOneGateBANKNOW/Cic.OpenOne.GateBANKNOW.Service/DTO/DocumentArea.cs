
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Enumeration of Document area types used in docKontextDto
    /// </summary>
    public enum DocumentArea
    {
        /// <summary>
        /// It
        /// </summary>
        It,

        /// <summary>
        /// Angebot
        /// </summary>
        Angebot,

        /// <summary>
        /// Antrag
        /// </summary>
        Antrag
    }

    /// <summary>
    /// Enumeration of Document area sub types used in docKontextDto
    /// this can be used to print only a subset of documents for the area
    /// </summary>
    public enum DocumentSubArea
    {
        /// <summary>
        /// Alle Dokumente auflisten/drucken
        /// </summary>
        All,

        /// <summary>
        /// Nur Antragsformulare auflisten/drucken
        /// </summary>
        Antragsformular
    }
}