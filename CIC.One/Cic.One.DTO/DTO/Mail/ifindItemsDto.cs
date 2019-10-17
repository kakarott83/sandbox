using Microsoft.Exchange.WebServices.Data;

namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Finden von Items
    /// </summary>
    public class ifindItemsDto
    {
        /// <summary>
        /// Falls dies leer oder null ist, wird der WellKnownFolderName genommen
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// In welchen Ordner das Item gesucht werden soll
        /// Falls es null ist, wird in dem AKF-Ordner gesucht
        /// (Dazu muss die FolderId auch null sein)
        /// </summary>
        public MWellKnownFolderNameEnum? WellKnownFolderName { get; set; }

        /// <summary>
        /// Wie groß eine Seite sein soll
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Die Seitennummer, welche geladen werden soll
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Enthält den kompletten Suchfilter. (Wird erst ab Exchange 2010 unterstützt)
        /// siehe http://msdn.microsoft.com/en-us/library/ee693615(EXCHG.140).aspx
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// Gibt an ob die Suchterme im Ergebnis gehighlightet werden sollen.
        /// (Funktioniert nur mit der Suche über einen Querystring)
        /// </summary>
        public bool HighlightTerms { get; set; }

        /// <summary>
        /// Der Suchfilter, welcher verwendet werden soll.
        /// Er wird verwendet, falls die Exchange Version zu alt ist oder kein Suchfilder angegeben wurde.
        /// Falls kein SuchFilter angegeben wurde, werden einfach alle Elemente aus dem Ordner zurückgegeben
        /// </summary>
        public SearchFilter Search { get; set; }
    }
}