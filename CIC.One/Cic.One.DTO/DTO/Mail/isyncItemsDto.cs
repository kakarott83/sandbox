using System.Collections.Generic;

namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Synchronisieren von Items
    /// </summary>
    public class isyncItemsDto
    {
        /// <summary>
        /// Id's welche ignoriert werden sollen
        /// </summary>
        public List<string> IdsToIgnore { get; set; }

        /// <summary>
        /// Status ab welchem synchronisiert werden soll
        /// </summary>
        public string SyncState { get; set; }

        /// <summary>
        /// Falls dies leer oder null ist, wird der WellKnownFolderName genommen
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// In welchem Ordner die Items synchronisiert werden sollen
        /// Falls es null ist, wird der AKF-Ordner synchronisiert
        /// (Dazu muss die FolderId auch null sein)
        /// </summary>
        public MWellKnownFolderNameEnum? WellKnownFolderName { get; set; }
    }
}