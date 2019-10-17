using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter 
    /// </summary>
    public class olistInboxServicesDto : oBaseDto
    {
        /// <summary>
        /// Eine Liste der relevanten EAIHOT-Datensätze zur Anzeige im GUI
        /// </summary>
        public List<EaihotDto> eaiHotListe { get; set; }
    }
}