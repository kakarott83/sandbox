using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Eurotac Business Object Interface
    /// </summary>
    [System.CLSCompliant(false)]
    public interface IEurotaxBo
    {
        ///// <summary>
        ///// Get Forecast
        ///// </summary>
        ///// <param name="inDto">In Data</param>
        ///// <returns>Out Data</returns>
        // EurotaxOutDto GetForecast(EurotaxInDto inDto);

        /// <summary>
        /// Get Forecast
        /// </summary>
        /// <param name="inDto">In Data</param>
        /// <returns>Out Data List</returns>
        List<EurotaxOutDto> GetForecast(EurotaxInDto inDto);

        /// <summary>
        /// Get Remo
        /// </summary>
        /// <param name="inDto">In Data</param>
        /// <returns>Out Data List</returns>
        List<EurotaxOutDto> GetRemo(EurotaxInDto inDto);

        /// <summary>
        /// Get forecast
        /// </summary>
        /// <param name="sysAuskunft">Information code</param>
        /// <returns>Output Data</returns>
        AuskunftDto GetForecast(long sysAuskunft);

        /// <summary>
        /// Get Valuation
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        EurotaxOutDto GetValuation(EurotaxInDto inDto);

        /// <summary>
        /// Get Valuation
        /// </summary>
        /// <param name="sysAuskunft">Information Code</param>
        /// <returns>Output Data</returns>
        AuskunftDto GetValuation(long sysAuskunft);

        /// <summary>
        /// Ermitteln des Restwertes eines Fahrzeugs
        /// </summary>
        /// <param name="input">Eingabedaten</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <returns>Restwert in Prozent</returns>
        double evaluateRestwert(RestwertRequestDto input, long sysvg);

        /// <summary>
        /// Ermittlen des Neuwerts eines Fahrzeugs.
        /// </summary>
        /// <param name="sysobtyp">Objekttyp</param>
        /// <returns>Netto Neupreis Betrag</returns>
        double evaluateNeupreis(long sysobtyp);

        // EurotaxOutDto GetHistoricalForecast(EurotaxInDto inDto);


        /// <summary>
        /// GetVinDecode
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        EurotaxVinOutDto GetVinDecode(EurotaxVinInDto inDto);

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sysobtyp"></param>
       /// <param name="laufzeit"></param>
       /// <param name="neupreis"></param>
       /// <param name="neupreisDefault"></param>
       /// <param name="neupreisDefaultIW"></param>
       /// <param name="kmStand"></param>
       /// <param name="zubehoer"></param>
       /// <param name="erstzulassung"></param>
       /// <param name="schwacke"></param>
       /// <param name="jahresKm"></param>
       /// <returns></returns>
        List<EurotaxOutDto> getEurotaxOutList(long sysobtyp, int laufzeit, double neupreis, double neupreisDefault, double neupreisDefaultIW, double neupreisDefaultVGREF, double kmStand, double zubehoer, DateTime? erstzulassung, string schwacke, long jahresKm);


        /// <summary>
        /// Fetch the eurotax forecast value
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        EurotaxOutDto getEurotaxForecast(EurotaxInDto inDto);

        /// <summary>
        /// Provides access data (username, password, signature)
        /// </summary>
        Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGHeaderType MyGetHeaderType(EurotaxVinInDto inDto);

        /// <summary>
        /// Fills Setting Type
        /// </summary>
        /// <param name="inDto"></param>
        Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef.ETGsettingType MyGetSettingType(EurotaxVinInDto inDto);

        string MyGetServiceId(EurotaxVinInDto inDto);

        /// <summary>
        /// Gets username and password from db and fills header
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        DAO.Auskunft.EurotaxValuationRef.ETGHeaderType MyGetHeaderTypeForValuation(EurotaxInDto inDto);

        /// <summary>
        /// Gets setting for Valuation
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        DAO.Auskunft.EurotaxValuationRef.ETGsettingType MyGetSettingTypeForValuation(EurotaxInDto inDto);
    }
}
