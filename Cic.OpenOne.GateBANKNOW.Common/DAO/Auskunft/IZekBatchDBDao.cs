using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEKBatch Data Access Object Interface
    /// </summary>
    public interface IZekBatchDBDao
    {
        /// <summary>
        /// updateZEK für alle Zek-Sätze im Batch
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekDto"></param>
        /// <returns></returns>
        int updateAllZEK(long sysAuskunft, ZekDto zekDto);

        /// <summary>
        /// updateZEK für alle Zek-Sätze im Batch
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekDto"></param>
        /// <returns></returns>
        int updateAllZEKEC7(long sysAuskunft, ZekDto zekDto);

        /// <summary>
        /// Setzt das Zek.ZekActionFlag, für die Zeks in der ErrorList stehen, auf Error
        /// und die restlichen auf Processed
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekBatchOutDto"></param>
        /// <returns></returns>
        int updateZEKErrorListCloseBatch(long sysAuskunft, ZekBatchOutDto zekBatchOutDto);

        /// <summary>
        /// Setzt das Zek.ZekActionFlag, für die Zeks in der ErrorList stehen, auf Error
        /// und die restlichen auf Processed
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekBatchOutDto"></param>
        /// <returns></returns>
        int updateZEKErrorListUpdateBatch(long sysAuskunft, ZekBatchOutDto zekBatchOutDto);

        /// <summary>
        /// Gets USERNAME and PASSWORD from AUSKUNFTCFG for ZekBatch Services
        /// </summary>
        /// <returns>IdentityDescriptor, filled with username and password</returns>
        ZEKBatchRef.IdentityDescriptor GetIdentityDescriptor();

        /// <summary>
        /// Saves Batch Vertragsänderung (EC7) Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveUpdateContractsBatchInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// Saves Batch Vertragsabmeldung (EC5) Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveCloseContractsBatchInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// Saves Output from Batch operations: close and update
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveContractsBatchOutput(long sysAuskunft, ZekBatchInstructionResponseDto outDto);

        /// <summary>
        /// Saves Status of Batch operations (close and update)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveBatchStatusOutput(long sysAuskunft, ZekBatchStatusResponseDto outDto);

        /// <summary>
        /// Finds ZEK Input data by sysAuskunft and maps it to ZekInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>ZekInDto, filled with input for ZEK WS</returns>
        ZekInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// Find batchrequestId for sysAuskunft
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        string FindBatchRequestId(long sysAuskunft);

        /// <summary>
        /// Find batchrequestId for sysAuskunft
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        int getContractsCount(long sysAuskunft);
    }
}