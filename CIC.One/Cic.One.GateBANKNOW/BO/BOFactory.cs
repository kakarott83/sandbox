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
using Cic.One.Web.BO;


namespace Cic.One.GateBANKNOW.BO
{
    /// <summary>
    /// Factory of all Business Objects for BANKNOW 
    /// </summary>
    public class BOFactory : Cic.One.Web.BO.BOFactory
    {
        
        public BOFactory() { }

        /// <summary>
        /// returns the xpro search bo
        /// </summary>
        /// <returns></returns>
        public override IXproSearchBo getXproSearchBo()
        {
            return new XproSearchBo(new XproLoaderDao(), Cic.One.GateBANKNOW.BO.Search.XproInfoFactory.getInstance());
        }

        /// <summary>
        /// returns the CRM Entity BO repsonsible for all create, update, get operations of Entities
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override IEntityBo getEntityBO(MembershipUserValidationInfo info)
        {
            IEntityDao crmEntityDao = new Cic.One.GateBANKNOW.DAO.EntityDao();

            if (info != null)
            {
                crmEntityDao.setSysPerole(info.sysPEROLE);
                crmEntityDao.setSysWfuser(info.sysWFUSER);
                crmEntityDao.setISOLanguage(info.ISOLanguageCode);
            }
            EntityBo bo = new Cic.One.GateBANKNOW.BO.EntityBo(crmEntityDao, getAppSettingsBO(), getCASBo());
            if (info != null)
            {
                bo.setSysWfUser(info.sysWFUSER);
                bo.SysPerole = info.sysPEROLE;
            }
            return bo;
        }

        /// <summary>
        /// Create new zek management object
        /// </summary>
        /// <param name="sysWfUser">logged in workflow user</param>
        /// <returns>zek bo</returns>
        public ZekBo getZekBO(long sysWfUser)
        {
            return new ZekBo(sysWfUser);
        }
       
    }
}