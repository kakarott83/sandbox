using System.Collections.Generic;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// RightsMap holen
    /// </summary>
    public class RightsMapBo : AbstractRightsMapBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rightsMapDao">rightsMapDao</param>
        public RightsMapBo(IRightsMapDao rightsMapDao)
            : base(rightsMapDao)
        {
        }

        /// <summary>
        /// RightsMap für einen WFUser holen
        /// </summary>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        public override List<RightsMap> getRightsForWFUser(long sysWFUser)
        {
            IRightsMapDao rightsMapDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getRightsMapDao();
            return rightsMapDao.getRightsForWFUser(sysWFUser);
        }
    }
}
