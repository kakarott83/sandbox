using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Decision Engine DB Data Access Object
    /// </summary>
    public interface IDecisionEngineDBDao
    {
        /// <summary>
        /// Save Decision Engine Input to Database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveDecisionEngineInput(long sysAuskunft, DecisionEngineInDto inDto);

        /// <summary>
        /// Save Decision Engien Output to Database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveDecisionEngineOutput(long sysAuskunft, DecisionEngineOutDto outDto);

        /// <summary>
        /// Find DE Input by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>Decision Engine Input searched</returns>
        DecisionEngineInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// saveRatingergebnis
        /// </summary>
        /// <param name="outDto">outDto</param>
        /// <param name="sysauskunft">sysauskunft</param>
        void saveRatingergebnis(DecisionEngineOutDto outDto, long sysauskunft);


        /// <summary>
        /// getGetroffenenRegelnCode
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        List<string> getGetroffenenRegelnCode(long sysAuskunft);

        /// <summary>
        /// getGetroffenenRegelnAnzahl
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        int getGetroffenenRegelnAnzahl(long sysAuskunft);

      
    }
}
