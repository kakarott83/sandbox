using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{

    /// <summary>
    /// BO for Management of Disclaimers
    /// </summary>
    public class DisclaimerBo : IDisclaimerBo
    {
        private DisclaimerDao dao = new DisclaimerDao();


        public DisclaimerBo()
        {

        }

        /// <summary>
        /// Creates a disclaimer-Text for the given area/id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dt"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="inhalt"></param>
        public void createDisclaimer(String area, DisclaimerType dt, long sysid, long syswfuser, string inhalt)
        {
            dao.createDisclaimer(area, dt, sysid, syswfuser, inhalt);

        }
    }
}
