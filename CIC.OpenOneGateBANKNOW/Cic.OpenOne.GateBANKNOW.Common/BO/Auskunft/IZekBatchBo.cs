using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// IZekBatchBo interface for ZEKBatch Webservice methods EC5 und EC7
    /// </summary>
    public interface IZekBatchBo
    {
        /// <summary>
        /// Saves Auskunft and Zek-Input, sends closeContractsBatch request (EC5) away and saves response
        /// Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeContractsBatch(ZekInDto inDto);

        /// <summary>
        /// Collects input from database by sysAuskunft, maps it to ZektInDto, maps ZekInDto to closeContractsBatch request (EC5), 
        /// sends request away and maps response to ZekOutDto.
        /// Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeContractsBatch(long sysAuskunft);

        /// <summary>
        /// Saves Auskunft and Zek-Input, sends updateContractsBatch request (EC7) away and saves response
        /// Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateContractsBatch(ZekInDto inDto);

        /// <summary>
        /// Collects input from database by sysAuskunft, maps it to ZektInDto, maps ZekInDto to updateContractsBatch request (EC7), 
        /// sends request away and maps response to ZekOutDto.
        /// Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateContractsBatch(long sysAuskunft);
    }
}