using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// PermissionBo-Klasse
    /// </summary>
    public class PermissionBo : AbstractPermissionBo
    {
        /// <summary>
        /// PermissionBo-Konstruktor
        /// </summary>
        /// <param name="dao"></param>
        public PermissionBo(IPermissionDao dao)
            : base(dao)
        {
        }

        /// <summary>
        /// Returns a list of Modules with permissions
        /// </summary>
        /// <returns></returns>
        override
        public List<String> getModules()
        {
            return dao.getModules();
        }

        /// <summary>
        /// Returns a list of Functions for a module
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        override
        public List<String> getFunctions(String module)
        {
            return dao.getFunctions(module);
        }

        /// <summary>
        /// Returns the permission flags for a module, function and user
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        override
        public PermissionDto getPermission(String module, String function, long syswfuser)
        {
            return dao.getPermission(module, function, syswfuser);
        }
    }
}