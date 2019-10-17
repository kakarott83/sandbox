using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// PE User DAO Schnittstelle
    /// </summary>
    public interface IPeuserDao
    {
        /// <summary>
        /// Verfügbare Nutzer auflisten
        /// </summary>
        /// <param name="sysperole">Personenrolle ID</param>
        /// <returns></returns>
        DropListDto[] listAvailableUser(long sysperole);

        /// <summary>
        /// Verfügbare Nutzer auflisten with Filter activeStatus
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="activeStatus"></param>
        /// <returns></returns>
        DropListDto[] listAvailableUser(long sysperole, PeroleActiveStatus activeStatus);

       


    }
}
