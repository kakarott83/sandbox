namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Löschen von einem Item
    /// </summary>
    public class ideleteItem
    {
        /// <summary>
        /// Id des zu löschenden Items
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Wie das Item gelöscht werden soll
        /// (Wird bei jeder Art von Item verwendet)
        /// </summary>
        public MDeleteModeEnum DeleteMode { get; set; }

        /// <summary>
        /// Wird nur bei Appointments verwendet
        /// </summary>
        public MSendCancellationModeEnum SendInvitationsOrCancellationsMode { get; set; }

        /// <summary>
        /// Enthält den Owner, welcher das Item hat
        /// </summary>
        public long? SysOwner { get; set; }
    }
}