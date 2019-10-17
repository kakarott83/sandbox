using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAngebotService.processAngebotToAntrag"/> Methode
    /// </summary>
    public class oprocessAngebotToAntragDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Antrag
        /// </summary>
        public AntragDto antrag
        {
            get;
            set;
        }
    }
}
