using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchVertragService.getVertragDetail"/> Methode
    /// </summary>
    public class ogetVertragDetailDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde zu Antrag
        /// </summary>
        public AntragDto antrag
        {
            get;
            set;
        }

        public VertragDto vertrag
        {
            get;
            set;
        }
    }
}