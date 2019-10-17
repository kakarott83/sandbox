using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAntragService.getAntragDetail"/> Methode
    /// </summary>
    public class ogetAntragDetailDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }

    }
}