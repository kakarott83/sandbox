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
    /// PE User BO
    /// </summary>
    public class PeuserBo : AbstractPeuserBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peuserDao"></param>
        public PeuserBo(IPeuserDao peuserDao)
            : base(peuserDao)
        {
        }

        /// <summary>
        /// verfügbare Benutzer auflisten
        /// </summary>
        /// <param name="sysperole">Personenrolle</param>
        /// <returns></returns>
        public override DropListDto[] listAvailableUser(long sysperole)
        {
            return peuserDao.listAvailableUser(sysperole);
        }

        /// <summary>
        /// verfügbare Benutzer auflisten
        /// </summary>
        /// <param name="sysperole">Personenrolle</param>
        /// <param name="activeStatus">Perole Status</param>
        /// <returns></returns>
        public override DropListDto[] listAvailableUser(long sysperole, PeroleActiveStatus activeStatus)
        {
            return peuserDao.listAvailableUser(sysperole,activeStatus);
        }
    }
}
