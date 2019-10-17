using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Eurotax DB Data Access Obejct Interface
    /// </summary>
    [System.CLSCompliant(false)]
    public interface IEurotaxDBDao
    {
        /// <summary>
        /// Save Eurotax Input Forecast to DB
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        long SaveEurotaxInpFc(long sysAuskunft);

        /// <summary>
        /// Save Eurotax Input in Data Transfer Object
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="sysEtgInp"></param>
        void SaveEurotaxInDto(EurotaxInDto inDto, long sysEtgInp);

        /// <summary>
        /// Save Eurotax Forecast Output to DB
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveEurotaxForecastOutput(long sysAuskunft, EurotaxOutDto outDto);

        /// <summary>
        /// Find Eurotax Input Data by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        EurotaxInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// Get Eurotax Access Data
        /// </summary>
        /// <param name="bez"></param>
        /// <returns></returns>
        EurotaxLoginDataDto GetEurotaxAccessData(String bez);

        /// <summary>
        /// Save Eurotax Vauation Input Data in Data Transfer Object
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveEurotaxValuationInput(long sysAuskunft, EurotaxInDto inDto);

        /// <summary>
        /// Save Eurotax Valuation Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveEurotaxValuationOutput(long sysAuskunft, EurotaxOutDto outDto);

        /// <summary>
        /// Find Valuation Input Data by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        EurotaxInDto FindValuationInputBySysId(long sysAuskunft);

        /// <summary>
        /// Ermitteln des Neuwerts eines Fahrzeugs.
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <returns>Netto Neupreis Betrag</returns>
        double evaluateFzNeupreis(long sysobtyp);


        /// <summary>
        /// Ermitteln des Restwertes eines Fahrzeugs
        /// </summary>
        /// <param name="input">EingabeDaten</param>
        /// <returns>Restwert in Prozent</returns>
        double evaluateFzRestwert(RestwertRequestDto input);

        /// <summary>
        /// Ermitteln des Restwert-Settings
        /// </summary>
        /// <param name="sysOBTyp">Objecttyp</param>
        /// <param name="perDate">perDate</param>
        /// <returns>Einstellungen</returns>
        RestWertSettingsDto getRestwertSettings(long sysOBTyp, DateTime perDate);

        /// <summary>
        /// Ermitteln des Restwert-Settings sysvgrw2
        /// </summary>
        /// <param name="sysOBTyp">Objecttyp</param>
        /// <param name="perDate">perDate</param>
        /// <returns>Einstellungen</returns>
        RestWertSettingsDto getRestwertSettings2(long sysOBTyp, DateTime perDate);


        /// <summary>
        /// Ermitteln des Aktwert-Settings
        /// </summary>
        /// <param name="sysobtyp">Objecttyp</param>
        /// <param name="perDate">perDate</param>
        /// <returns>Einstellungen</returns>
        NeuwertSettingsDto getAktwertSettings(long sysobtyp, DateTime perDate);

        /// <summary>
        /// returns true, if the obtyp is a root-vehicle-tree id
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        bool isLevelOne(long sysobtyp);

        /// <summary>
        /// return aram4GetELDto
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        ELInDto getELInDtoAusDB(long sysantrag, long sysprproduct, long sysperole);


        /// <summary>
        /// Ermitteln der Objekttypdaten eines Fahrzeugs nach Nationalem Fahrzeugcode
        /// </summary>
        /// <param name="nationalVC"></param>
        /// <returns></returns>
        ObtypDataRestwertDto getObTypDataByNVCByString(string nationalVC);

        /// <summary>
        /// return Daten für getELDto aus Antrag und kalkulation 
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="angAntKalkDto"></param>
        /// <returns></returns>
        ELInDto getELInDtoAusAntrag(long sysantrag, AngAntKalkDto angAntKalkDto);

        /// <summary>
        /// Returns the vgref value group for the given context
        /// </summary>
        /// <param name="vgtype"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="sysbrand"></param>
        /// <param name="sysprhgroup"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        RestWertSettingsDto getSysVGForVGREFType(VGRefType vgtype, long sysobtyp, long sysbrand, long sysprhgroup, DateTime perDate);
    }
}
