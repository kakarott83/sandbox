using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter
    /// </summary>
    public class odeleteListeExportDto : oBaseDto
    {
        /// <summary>
        /// True wenn die Liste gelöscht wurde, sonst False.
        /// </summary>
        public bool result { get; set; }
    }
}