using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Object type Data Access Object Interface
    /// </summary>
    public interface IObTypDao
    {
        /// <summary>
        /// Object Type Descendenants
        /// </summary>
        /// <param name="sysobtyp">Object type</param>
        /// <returns>List of Object Descendant types</returns>
        List<long> getObTypDescendants(long sysobtyp);

        /// <summary>
        /// Object Type Ancestors
        /// </summary>
        /// <param name="sysobtyp">Object Type</param>
        /// <returns>List of Object Types Ancestors</returns>
        List<long> getObTypAscendants(long sysobtyp);

        /// <summary>
        /// Prhgroups for the role and obtyp
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        List<long> getPrhGroups(long sysperole, long sysobtyp, DateTime perDate);

    }
}
