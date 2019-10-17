
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.printAngebotService.listAvailableDokumente"/> Methode
    /// </summary>
    public class ilistAvailableDokumenteDto
    {
        /// <summary>
        /// Dokumentenkontext
        /// </summary>
        public docKontextDto kontext
        {
            get;
            set;
        }
    }
}