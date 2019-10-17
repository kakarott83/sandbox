using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstract Permission BO
    /// </summary>
    public abstract class AbstractPermissionBo : IPermissionBo
    {
        /// <summary>
        /// PermissionDao
        /// </summary>
        protected IPermissionDao dao;

        /// <summary>
        /// AbstractPermissionBo
        /// </summary>
        /// <param name="dao"></param>
        public AbstractPermissionBo(IPermissionDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Returns a list of Modules with permissions
        /// </summary>
        /// <returns></returns>
        public abstract List<String> getModules();

        /// <summary>
        /// Returns a list of Functions for a module
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public abstract List<String> getFunctions(String module);

        /// <summary>
        /// Returns the permission flags for a module, function and user
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract PermissionDto getPermission(String module, String function, long syswfuser);
    }
}