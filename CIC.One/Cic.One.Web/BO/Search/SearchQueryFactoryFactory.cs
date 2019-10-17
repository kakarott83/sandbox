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
    public class SearchQueryFactoryFactory 
    {
        private static volatile ISearchQueryInfoFactory usedFactory = null;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// Access to the currently used BO Factory
        ///  per Default the One.Web Factory will be used
        /// </summary>
        /// <returns></returns>
        public static ISearchQueryInfoFactory getInstance()
        {
            if (usedFactory == null)
            {
                lock (InstanceLocker)
                {
                    if (usedFactory == null)
                        usedFactory = new SearchQueryInfoFactory();
                }
            }
            return usedFactory;
        }

        /// <summary>
        /// sets the used factory for search queries
        /// </summary>
        /// <param name="factory"></param>
        public static void setFactory(ISearchQueryInfoFactory factory)
        {
            usedFactory = factory;
        }
       
    }
}