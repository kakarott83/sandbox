using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// defines interface for business objects getting additional information about the current user
    /// </summary>
    public interface ISimpleGetterBo
    {
        /// <summary>
        /// get the profil of the current user
        /// </summary>
        /// <returns>ogetProfilDto</returns>
        ProfilDto getProfil(long sysVpPerole);
        
        /// <summary>
        /// get the key account manager of the current user
        /// </summary>
        /// <returns>ogetKamDto (key account manager)</returns>
        KamDto getKam(long sysVpPerole);

        /// <summary>
        /// get the abwicklungsort of the current user
        /// </summary>
        /// <returns>ogetAbwicklungsortDto</returns>
        AbwicklungsortDto getAbwicklungsort(long sysVpPerole);
        
    }
}
