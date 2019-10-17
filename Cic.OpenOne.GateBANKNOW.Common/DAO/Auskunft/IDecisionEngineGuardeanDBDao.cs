using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System.Collections.Generic;
using System.Net;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public interface IDecisionEngineGuardeanDBDao
    {
        /// <summary>
        /// returns the currently configured states map mapping from shs state to zustand.attribut
        /// </summary>
        /// <returns></returns>
        List<StateStruct> getStatesMap();

        /// <summary>
        /// Save Decision Engine Input to Database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveDecisionEngineInput(long sysAuskunft, DecisionEngineGuardeanInDto inDto);

        /// <summary>
        /// Save Decision Engien Output to Database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveDecisionEngineOutput(long sysAuskunft, DecisionEngineGuardeanOutDto outDto);

        /// <summary>
        /// Find DE Input by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>Decision Engine Input searched</returns>
        DecisionEngineGuardeanInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        NetworkCredential getCredentials();

        /// <summary>
        /// Fills the InputDto from Antrag
        /// </summary>
        /// <param name="sysAntrag"></param>
        /// <param name="ma"></param>
        /// <returns></returns>
        DecisionEngineGuardeanInDto fillFromAntrag(long sysAntrag, bool ma);
    }
}