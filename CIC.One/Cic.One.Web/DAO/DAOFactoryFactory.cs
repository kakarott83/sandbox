using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Mail;
using Cic.One.Web.DAO.Mail;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;


namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Factory for the Factory of all DA Objects
    /// </summary>
    public class DAOFactoryFactory 
    {
        private static IDAOFactory usedFactory = null;
        private static string LOCK = "LOCK";

        /// <summary>
        /// Access to the currently used BO Factory
        ///  per Default the One.Web Factory will be used
        /// </summary>
        /// <returns></returns>
        public static IDAOFactory getInstance()
        {
            lock (LOCK)
            {
                if (usedFactory == null)
                    usedFactory = new DAOFactory();
            }
            return usedFactory;
        }

        /// <summary>
        /// sets the used factory for creating all bo's
        /// </summary>
        /// <param name="factory"></param>
        public static void setFactory(IDAOFactory factory)
        {
            usedFactory = factory;
        }
       
    }
}