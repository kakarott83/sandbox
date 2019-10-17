namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Ändern von einem Item
    /// </summary>
    public class ichangeItem
    {
        /// <summary>
        /// Item welches verändert werden soll
        /// </summary>
        public MItemDto Item { get; set; }

        /// <summary>
        /// Gibt an, was bei einem Konflikt gemacht werden soll.
        /// (Wird bei jeder Art von Item verwendet)
        /// </summary>
        public MConflictResolutionModeEnum ConflictResolution { get; set; }

        /// <summary>
        /// Wird nur bei Appointments verwendet
        /// </summary>
        public MSendInvitationsOrCancellationsModeEnum SendInvitationsOrCancellationsMode { get; set; }

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
        /// Legt fest ob eine Mail geschickt werden soll, falls es eine ist
        /// </summary>
        public bool SendMail { get; set; }
    }
}