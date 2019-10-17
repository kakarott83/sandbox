using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte PE User BO Klasse
    /// </summary>
    public abstract class AbstractPeuserBo : IPeuserBo
    {
        /// <summary>
        /// PE User DAO
        /// </summary>
        protected IPeuserDao peuserDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peuserDao">PE User DAO</param>
        public AbstractPeuserBo(IPeuserDao peuserDao)
        {
            this.peuserDao = peuserDao;
        }

        /// <summary>
        /// Verfügbare Personen auflisten
        /// </summary>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Dropliste</returns>
        public abstract DropListDto[] listAvailableUser(long sysperole);

        /// <summary>
        /// listAvailableUser
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="activeStatus"></param>
        /// <returns></returns>
        public abstract DropListDto[] listAvailableUser(long sysperole, PeroleActiveStatus activeStatus);
    }
}
