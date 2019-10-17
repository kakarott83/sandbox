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


namespace Cic.One.GateWKT.BO
{
    /// <summary>
    /// Factory of all Business Objects for WKT
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
            return new XproSearchBo(new XproLoaderDao(), Cic.One.GateWKT.BO.Search.XproInfoFactory.getInstance());
        }

        /// <summary>
        /// creates the WKT BO
        /// </summary>
        /// <returns></returns>
        public IWktBO getWKTBO()
        {
            return new WktBO();
        }

        /// <summary>
        /// returns the Report BO
        /// </summary>
        /// <returns></returns>
        public override IReportBo getReportBo()
        {
            return new ReportBo();
        }

        /// <summary>
        /// returns the CRM Entity BO repsonsible for all create, update, get operations of Entities
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override IEntityBo getEntityBO(MembershipUserValidationInfo info)
        {
            IEntityDao crmEntityDao = new Cic.One.GateWKT.DAO.EntityDao();
            if (info != null)
            {
                crmEntityDao.setSysPerole(info.sysPEROLE);
                crmEntityDao.setSysWfuser(info.sysWFUSER);
                crmEntityDao.setISOLanguage(info.ISOLanguageCode);
            }
            
            EntityBo bo = new Cic.One.GateWKT.BO.EntityBo(crmEntityDao, getAppSettingsBO(), getCASBo());
            if (info != null)
            {
                bo.setSysWfUser(info.sysWFUSER);
                bo.SysPerole = info.sysPEROLE;
            }
            return bo;
        }
       
    }
}