using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.listAvailableServices"/> Methode und
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createSchnellkalkulationService.listAvailableServices"/> Methode
    /// </summary>
    public class olistAvailableServicesDto : oBaseDto
    {
        /// <summary>
        /// Array von Services
        /// </summary>
        public AvailableServiceDto[] services
        {
            get;
            set;
        }
    }
}