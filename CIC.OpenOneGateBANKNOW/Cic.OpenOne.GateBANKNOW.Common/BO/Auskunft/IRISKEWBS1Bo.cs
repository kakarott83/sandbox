using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// S1 Bo Interface
    /// </summary>
    public interface IRISKEWBS1Bo
    {
        /// <summary>
        /// Send Info
        /// </summary>
        /// <param name="sysAuskunft">Sys ID from AUSKUNF</param>
        /// <returns></returns>
        AuskunftDto sendData(long sysAuskunft);
    }
}
