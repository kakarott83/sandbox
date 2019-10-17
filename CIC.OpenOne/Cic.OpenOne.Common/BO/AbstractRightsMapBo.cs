using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstrakte RightsMapBo-Klasse
    /// </summary>
    public abstract class AbstractRightsMapBo : IRightsMapBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IRightsMapDao rightsMapDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rightsMapDao">rightsMapDao</param>
        public AbstractRightsMapBo(IRightsMapDao rightsMapDao)
        {
            this.rightsMapDao = rightsMapDao;
        }

        /// <summary>
        /// RightsMap für einen WFUser holen
        /// </summary>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        public abstract List<RightsMap> getRightsForWFUser(long sysWFUser);
    }
}