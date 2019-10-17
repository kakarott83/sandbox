
namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getAntragDetail Methode
    /// </summary>
    public class ogetAntragDetailDto
    {
        /// <summary>
        /// Persistenzobjekt Kunde zu Antrag
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }

        /// <summary>
        /// Persistenzobjekt Objekt zu Antrag
        /// </summary>
        public DTO.ObjektDto objekt
        {
            get;
            set;
        }

        /// <summary>
        /// Persistenzobjekt Kalkulation zu Antrag
        /// </summary>
        public DTO.KalkulationDto kalkulation
        {
            get;
            set;
        }
    }
}