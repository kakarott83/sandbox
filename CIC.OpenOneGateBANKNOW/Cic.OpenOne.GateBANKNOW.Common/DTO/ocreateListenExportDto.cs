using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für Listenexport
    /// </summary>
    public class ocreateListenExportDto : oBaseDto
    {
        /// <summary>
        /// Die sysId vom neu erstellten EAIHOT-Datensatz
        /// </summary>
        public long sysEaiHot { get; set; }
    }
}