
namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für listAvailableDokumente Methode
    /// </summary>
    public class olistAvailableDokumenteDto
    {
        /// <summary>
        /// Allgemeines Messageobjekt
        /// </summary>
        public DTO.Message message
        {
            get;
            set;
        }

        /// <summary>
        /// Allgemeines Dokumentenobjekt
        /// </summary>
        public DTO.DokumenteDto[] dokumente
        {
            get;
            set;
        }
    }
}