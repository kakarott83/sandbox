using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ist der Output einer SyncItems Aktion
    /// </summary>
    public class osyncItemsDto : oBaseDto
    {
        /// <summary>
        /// Neuer synchronisations Status
        /// </summary>
        public string SyncState { get; set; }

        /// <summary>
        /// Geänderte Items
        /// </summary>
        public List<MItemChangeDto> Changed { get; set; }
    }
}