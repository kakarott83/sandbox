namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabeparameter für das Verschieben von Elementen
    /// </summary>
    public class imoveItem
    {
        /// <summary>
        /// Enthält die Id des Items, welches verschoben werden soll
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Falls dies leer oder null ist, wird der WellKnownFolderName genommen
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// In welchen Ordner das Item geschoben werden soll
        /// Falls es null ist, wird es in den AKF-Ordner geschoben
        /// (Dazu muss die FolderId auch null sein)
        /// </summary>
        public MWellKnownFolderNameEnum? WellKnownFolderName { get; set; }
    }
}