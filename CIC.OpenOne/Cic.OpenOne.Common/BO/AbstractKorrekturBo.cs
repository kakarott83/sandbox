using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// abstract business object for Korrektur
    /// </summary>
    public abstract class AbstractKorrekturBo : IKorrekturBo
    {
        /// <summary>
        /// the data access object to use
        /// </summary>
        protected IKorrekturDao dao;


        /// <summary>
        /// constructs a business object
        /// </summary>
        /// <param name="dao">the data access object to use</param>
        public AbstractKorrekturBo(IKorrekturDao dao)
        {
            this.dao = dao;

        }

        /// <summary>
        /// corresponds to the clarion correct method, selecting the type - parameters automatically
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public abstract double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2);

        /// <summary>
        /// corresponds to the clarion correct method
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public abstract double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2, KorrekturType type1, KorrekturType type2);
    }
}