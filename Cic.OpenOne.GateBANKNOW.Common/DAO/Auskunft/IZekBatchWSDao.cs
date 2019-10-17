
namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEKBatch Web Service Data Access Object Interface
    /// </summary>
    public interface IZekBatchWSDao
    {
        /// <summary>
        /// Vertragsabmeldung (EC5)
        /// Calls ZEK Batch Webservice closeContractsBatch
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <param name="contracts"></param>
        /// <returns>BatchInstructionResponse</returns>
        ZEKBatchRef.BatchInstructionResponse closeContractsBatch(ZEKBatchRef.IdentityDescriptor idDesc,
                                                                 string batchRequestId,
                                                                 ZEKBatchRef.ContractClosureInstruction[] contracts);

        /// <summary>
        /// Mutation Vertragsdaten (EC7) 
        /// Calls ZEK Batch Webservice updateContractsBatch
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <param name="contracts"></param>
        /// <returns>BatchInstructionResponse</returns>
        ZEKBatchRef.BatchInstructionResponse updateContractsBatch(ZEKBatchRef.IdentityDescriptor idDesc,
                                                                  string batchRequestId,
                                                                  ZEKBatchRef.ContractUpdateInstruction[] contracts);

        /// <summary>
        /// Get batch Status
        /// Calls ZEK Batch Webservice batchStatus
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <returns>BatchStatusResponse</returns>
        ZEKBatchRef.BatchStatusResponse batchStatus(ZEKBatchRef.IdentityDescriptor idDesc, string batchRequestId);



        /// <summary>
        /// setSysAuskunft
        /// </summary>
        /// <param name="sysauskunft"></param>
        void MySetSysAuskunft(long sysauskunft);


        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
    }
}