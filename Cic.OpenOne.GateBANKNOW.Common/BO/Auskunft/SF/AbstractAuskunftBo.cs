using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Abstract class which implements IAuskunftBo
    /// </summary>
    /// <typeparam name="InDto"></typeparam>
    /// <typeparam name="OutDto"></typeparam>
    public abstract class AbstractAuskunftBo<InDto, OutDto> : IAuskunftBo
    {
        public Cic.OpenOne.AuskunftManagement.Common.DTO.AuskunftBaseDto performAuskunft(long sysAuskunft)
        {
            return doAuskunft(sysAuskunft);
        }

        /// <summary>
        /// IAuskunftBo method which gets input values by sysAuskunft and performs a webservice call
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto doAuskunft(long sysAuskunft);

        /// <summary>
        ///  IAuskunftBo method which gets values from database by Area and sysId, inserts new datasets and performs a webservice call
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public abstract AuskunftDto doAuskunft(string area, long sysId);

        /// <summary>
        /// abstract method which gets values by a filled inDto and performs a webservice call
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract OutDto doAuskunft(InDto inDto);
    }
}