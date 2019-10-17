using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Permission Handling DAO Interface
    /// </summary>
    public interface IPermissionDao
    {
        /// <summary>
        /// Returns a list of Modules with permissions
        /// </summary>
        /// <returns></returns>
        List<String> getModules();


        /// <summary>
        /// Returns a list of Functions for a module
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        List<String> getFunctions(String module);

        /// <summary>
        /// Returns the permission flags for a module, function and user
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        PermissionDto getPermission(String module, String function, long syswfuser);
    }
}
