using Cic.OpenOne.AuskunftManagement.Common.BO.SF;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// AuskunftBo Interface 
    /// </summary>
    public interface IAuskunftBo  : ICommonAuskunftBo 
    {
        /// <summary>
        /// Gets input values by sysAuskunft and performs a webservice call 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        AuskunftDto doAuskunft(long sysAuskunft);

        /// <summary>
        /// Gets values from database by Area and sysId, inserts new datasets and performs a webservice call
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        AuskunftDto doAuskunft(string area, long sysId);
    }
}