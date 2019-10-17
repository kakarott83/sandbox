using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// defines the interface for data access objects providing simple getters
    /// </summary>
    public interface ISimpleGetterDao
    {
        /// <summary>
        /// get the associated person of the perole
        /// </summary>
        /// <param name="sysperole">Rolle</param>
        /// <returns>Person</returns>
        PERSON findPersonBySysperole(long sysperole);

        /// <summary>
        /// get the associated puser of the person
        /// </summary>
        /// <param name="person">person</param>
        /// <returns>puser</returns>
        PUSER getPuser(PERSON person);
        
        /// <summary>
        /// get the kam person of the given perole (Händlerrolle)
        /// </summary>
        /// <param name="sysVpPerole">Händlerrolle</param>
        /// <returns>person</returns>
        PERSON findKamPersonBySysperole(long sysVpPerole);

        /// <summary>
        /// get the abwicklungsort person of the given perole (Händlerrolle)
        /// </summary>
        /// <param name="sysVpPerole">Händlerrolle</param>
        /// <returns>person</returns>
        PERSON findAbwicklungsortPersonBySysperole(long sysVpPerole);


        /// <summary>
        /// Get The information channel to communicate with this person
        /// </summary>
        /// <param name="sysid">PersonID</param>
        /// <returns>Channelstring</returns>
        string getinformationUeberBySysPerson(long sysid);

    }
}