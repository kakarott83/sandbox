using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnittstelle Person User BO
    /// </summary>
    public interface IPeuserBo
    {
        /// <summary>
        /// verfügbare Personen auflisten
        /// </summary>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Drop-Liste</returns>
        DropListDto[] listAvailableUser(long sysperole);

        /// <summary>
        /// verfügbare Personen auflisten mit Filter activeStatus
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="activeStatus"></param>
        /// <returns></returns>
        DropListDto[] listAvailableUser(long sysperole, PeroleActiveStatus activeStatus);
    }
}
