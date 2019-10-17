using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: ZEKBatch Business Object
    /// </summary>
    public abstract class AbstractZekBatchBo : IZekBatchBo
    {
        /// <summary>
        /// ZEKBatch Web Service Data Access Object
        /// </summary>
        protected IZekBatchWSDao zekBatchWSDao;

        /// <summary>
        /// ZEKBatch DB Data Access Object
        /// </summary>
        protected IZekBatchDBDao zekBatchDBDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="zekBatchWSDao">ZEKBatch Web Service Data Access Object</param>
        /// <param name="zekBatchDBDao">ZEKBatch DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public AbstractZekBatchBo(IZekBatchWSDao zekBatchWSDao, IZekBatchDBDao zekBatchDBDao, IAuskunftDao auskunftDao)
        {
            this.zekBatchWSDao = zekBatchWSDao;
            this.zekBatchDBDao = zekBatchDBDao;
            this.auskunftDao = auskunftDao;
        }


        /// <summary>
        /// Saves Auskunft and Zek-Input, sends closeContractsBatch request (EC5) away and saves response
        /// Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeContractsBatch(ZekInDto inDto);

        /// <summary>
        /// Collects input from database by sysAuskunft, maps it to ZektInDto, maps ZekInDto to closeContractsBatch request (EC5), 
        /// sends request away and maps response to ZekOutDto.
        /// Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto closeContractsBatch(long sysAuskunft);


        /// <summary>
        /// Saves Auskunft and Zek-Input, sends updateContractsBatch request (EC7) away and saves response
        /// Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateContractsBatch(ZekInDto inDto);

        /// <summary>
        /// Collects input from database by sysAuskunft, maps it to ZektInDto, maps ZekInDto to updateContractsBatch request (EC7), 
        /// sends request away and maps response to ZekOutDto.
        /// Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto updateContractsBatch(long sysAuskunft);
    }
}