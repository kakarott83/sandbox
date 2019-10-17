
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetForecast"/> Methode
    /// EurotaxForecastDto ist Teil der oEurotaxForecastDto-Klasse
    /// </summary>
    public class EurotaxGetForecastDto
    {
        /// <summary>
        /// Restwert Brutto (incl Mwst) (GetForecast)
        /// </summary>
        public double TradeAmount { get; set; }

        /// <summary>
        /// Restwert "Handel" in Prozent (GetForecast)
        /// </summary>
        public double TradeValueInPercentage { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" Brutto (incl Mwst) (GetForecast)
        /// </summary>
        public double RetailAmount { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" in Prozent (GetForecast)
        /// </summary>
        public double RetailValueInPercentage { get; set; }

        /// <summary>
        /// Fehlerbeschreibung (GetForecast)
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Fehlercode (GetForecast)
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// ForecastPeriod
        /// </summary>
        public long ForecastPeriod { get; set; }
    }
}