using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ist der Output einer FindContact Aktion
    /// </summary>
    public class ofindContact : oBaseDto
    {
        /// <summary>
        /// Gefundene Kontakte
        /// </summary>
        public List<MNameResolutionDto> Items { get; set; }
    }
}