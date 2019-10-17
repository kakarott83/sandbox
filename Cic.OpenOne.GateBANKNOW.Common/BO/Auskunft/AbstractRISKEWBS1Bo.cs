using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: S1 Business Object
    /// </summary>
    public abstract class AbstractRISKEWBS1Bo : IRISKEWBS1Bo
    {
        /// <summary>
        /// S1 DB Data Access Object
        /// </summary>
        protected RISKEWBS1DBDto s1InDto;

        /// <summary>
        /// S1 DB Data Access Object
        /// </summary>
        protected RISKEWBS1DBDao s1DBDao;

        /// <summary>
        /// 
        /// </summary>
        protected RISKEWBS1WSDao s1WSDao;

        /// <summary>
        /// Send Data to S1
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto sendData(long sysAuskunft);
    }
}
