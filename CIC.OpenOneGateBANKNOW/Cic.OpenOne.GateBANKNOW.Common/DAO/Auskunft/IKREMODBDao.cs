using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// KREMO DB Data Access Object Interface
    /// </summary>
    public interface IKREMODBDao
    {
        /// <summary>
        /// Creates or Updates KREMO, filled with data from KREMOInDto
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>SYSKREMO</returns>
        long CreateOrUpdateKREMOInDto(KREMOInDto inDto);

        /// <summary>
        /// Create new KREMO Input
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>KREMO ID</returns>
        long SaveKREMOInDto(KREMOInDto inDto);

        /// <summary>
        /// Save KREMO Input
        /// </summary>
        /// <param name="sysAuskuft">Information ID</param>
        /// <param name="sysKremo">KREMO ID</param>
        void SaveKREMOInp(long sysAuskuft, long sysKremo);

        /// <summary>
        /// Save KREMO Output
        /// </summary>
        /// <param name="outDto">Output Data</param>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="sysKremo">KREMO ID</param>
        void SaveKREMOOutDto(KREMOOutDto outDto, long sysAuskunft, long sysKremo);

        /// <summary>
        /// Find by Information ID
        /// </summary>
        /// <param name="sysAuskunft">Infomration ID</param>
        /// <returns>Input Data</returns>
        KREMOInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// Returns the latest Kremo for the offer
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        KREMOInDto FindBySysAngebot(long sysAngebot);

        /// <summary>
        /// Returns the latest Kremo for the proposal
        /// </summary>
        /// <param name="sysAntrag"></param>
        /// <returns></returns>
        KREMOInDto FindBySysAntrag(long sysAntrag);
    }
}
