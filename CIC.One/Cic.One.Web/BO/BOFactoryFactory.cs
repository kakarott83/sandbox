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


namespace Cic.One.Web.BO
{
    /// <summary>
    /// Factory for the Factory of all Business Objects
    /// </summary>
    public class BOFactoryFactory 
    {
        private static IBOFactory usedFactory = null;
        private static string LOCK = "LOCK";

        /// <summary>
        /// Access to the currently used BO Factory
        ///  per Default the One.Web Factory will be used
        /// </summary>
        /// <returns></returns>
        public static IBOFactory getInstance()
        {
            lock (LOCK)
            {
                if (usedFactory == null)
                    usedFactory = new BOFactory();
            }
            return usedFactory;
        }

        /// <summary>
        /// sets the used factory for creating all bo's
        /// </summary>
        /// <param name="factory"></param>
        public static void setFactory(IBOFactory factory)
        {
            usedFactory = factory;
        }
       
    }
}