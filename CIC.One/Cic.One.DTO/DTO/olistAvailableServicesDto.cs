using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.One.GateBANKNOW.createOrUpdateAngebotService.listAvailableServices"/> Methode und
    /// <see cref="Cic.One.GateBANKNOW.createSchnellkalkulationService.listAvailableServices"/> Methode
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