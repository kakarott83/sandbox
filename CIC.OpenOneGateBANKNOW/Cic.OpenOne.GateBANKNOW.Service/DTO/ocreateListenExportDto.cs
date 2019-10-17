using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchVertragService.createListenExport"/> Methode
    /// </summary>
    public class ocreateListenExportDto : oBaseDto
    {
        /// <summary>
        /// Die sysId vom neu erstellten EAIHOT-Datensatz
        /// </summary>
        public long sysEaiHot { get; set; }
    }
}