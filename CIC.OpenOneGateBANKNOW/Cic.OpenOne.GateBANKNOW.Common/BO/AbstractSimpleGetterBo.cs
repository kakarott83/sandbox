using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// abstract business object getting additional information about the current user
    /// </summary>
    public abstract class AbstractSimpleGetterBo : ISimpleGetterBo
    {
        /// <summary>
        /// the data access object to use
        /// </summary>
        protected ISimpleGetterDao simpleGetterDao;

        /// <summary>
        /// constructs a abstract simpleGetter business object
        /// </summary>
        /// <param name="simpleGetterDao">data access object to use</param>
        public AbstractSimpleGetterBo(ISimpleGetterDao simpleGetterDao)
        {
            this.simpleGetterDao = simpleGetterDao;
        }

        /// <summary>
        /// get profil of the current user
        /// </summary>
        /// <returns>ogetProfilDto</returns>
        public abstract ProfilDto getProfil(long sysVpPerole);

        /// <summary>
        /// get key account manager of the current user
        /// </summary>
        /// <returns>ogetKamDto</returns>
        public abstract KamDto getKam(long sysVpPerole);

        /// <summary>
        /// get abwicklungsort of the current user
        /// </summary>
        /// <returns>ogetAbwicklungsortDto</returns>
        public abstract AbwicklungsortDto getAbwicklungsort(long sysVpPerole);

       
    }
}