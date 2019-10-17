namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Erstellen von Items
    /// </summary>
    public class icreateItem
    {
        /// <summary>
        /// Item, welches gespeichert werden soll
        /// </summary>
        public MItemDto Item { get; set; }

        /// <summary>
        /// Legt fest, ob der Standardornder gewählt wird.
        /// Falls es um das senden von Emails geht, bedeutet das, ob die Email im Sent Folder gespeichert werden soll
        /// </summary>
        public bool UseStandardFolder { get; set; }

        /// <summary>
        /// Falls dies leer oder null ist, wird der WellKnownFolderName genommen
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// In welchen Ordner das Item erstellt werden soll
        /// Falls es null ist, wird der default-Ordner verwendet
        /// (Dazu muss die FolderId auch null sein)
        /// </summary>
        public MWellKnownFolderNameEnum? WellKnownFolderName { get; set; }

        /// <summary>
        /// Wird nur bei Appointments verwendet
        /// </summary>
        public MSendInvitationsModeEnum SendInvitationsMode { get; set; }
    }
}