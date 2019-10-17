using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Prisma DAO Factory
    /// </summary>
    public class PrismaDaoFactory
    {
        private static volatile PrismaDaoFactory _self;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// Instanz der Prisma DAO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static PrismaDaoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new PrismaDaoFactory();
                }
            }
            return _self;
        }

        /// <summary>
        /// Prisma DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IPrismaDao getPrismaDao()
        {
            return new CachedPrismaDao();
        }

        /// <summary>
        /// Provisionen DAO erzeugen
        /// </summary>
        /// <returns></returns>
        public IProvisionDao getProvisionDao()
        {
            return new CachedProvisionDao();
        }

        /// <summary>
        /// Subvention DAO erzeugen
        /// </summary>
        /// <returns></returns>
        public ISubventionDao getSubventionDao()
        {
            return new CachedSubventionDao();
        }

        /// <summary>
        /// PrismaServiceDao DAO erzeugen
        /// </summary>
        /// <returns></returns>
        public IPrismaServiceDao getPrismaServiceDao()
        {
            return new CachedPrismaServiceDao();
        }
    }
}
