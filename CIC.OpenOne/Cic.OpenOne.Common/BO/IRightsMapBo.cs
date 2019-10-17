using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Schnittstelle für RightsMap
    /// </summary>
    public interface IRightsMapBo
    {
        /// <summary>
        /// RightsMap für einen WFUser holen
        /// </summary>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        List<RightsMap> getRightsForWFUser(long sysWFUser);
    }
}