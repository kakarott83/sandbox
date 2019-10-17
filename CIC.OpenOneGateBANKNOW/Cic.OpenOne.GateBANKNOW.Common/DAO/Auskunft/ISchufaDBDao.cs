namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System.Net;

    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa;

    public interface ISchufaDBDao
    {
        /// <summary>
        /// Find Input by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="auskunftTyp"></param>
        /// <returns>in Data Acces Object</returns>
        SchufaInDto FindBySysId(long sysAuskunft, string auskunftTyp = null);
        
        /// <summary>
        /// Speichert den Output
        /// </summary>
        /// <param name="sysAuskunft">Id</param>
        /// <param name="outDto">SchufaOutDto</param>
        /// <param name="auskunftTyp">Typ, welcher gespeichert werden soll</param>
        void SaveOutput(long sysAuskunft, SchufaOutDto outDto, string auskunftTyp);

        /// <summary>
        /// Speichert den Input
        /// </summary>
        /// <param name="sysAuskunft">Id</param>
        /// <param name="inDto">SchufaInDto</param>
        /// <param name="auskunftTyp">Typ, welcher gespeichert werden soll</param>
        void SaveInput(long sysAuskunft, SchufaInDto inDto, string auskunftTyp);

        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        NetworkCredential GetCredentials();
    }
}
