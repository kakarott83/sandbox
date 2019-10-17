using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Interface von RightsMapDao
    /// </summary>
    public interface IRightsMapDao
    {
        /// <summary>
        /// RightsMap für einen WFUser holen
        /// </summary>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        List<RightsMap> getRightsForWFUser(long sysWFUser);
    }
}
