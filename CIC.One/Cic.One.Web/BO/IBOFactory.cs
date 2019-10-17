using System;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
namespace Cic.One.Web.BO
{
    /// <summary>
    /// Factory Interface for all BO's of the system
    /// </summary>
    public interface IBOFactory
    {
        /// <summary>
        /// returns the Guardean BO
        /// </summary>
        /// <returns></returns>
        IGuardeanBo getGuardeanBo();

        /// <summary>
        /// returns the Application Settings BO
        /// </summary>
        /// <returns></returns>
        Cic.OpenOne.Common.BO.IAppSettingsBo getAppSettingsBO();

        /// <summary>
        /// returns the Authentication BO
        /// </summary>
        /// <returns></returns>
        IAuthenticationBo getAuthenticationBo();

        /// <summary>
        /// returns the reporting bo
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        IReportBo getReportBo();

        /// <summary>
        /// returns the prisma product resolution bo
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        IPrismaProductBo getProductBo(String language);

        /// <summary>
        /// returns the prisma product parameter resolution bo
        /// </summary>
        /// <returns></returns>
        IPrismaParameterBo getProductParameterBo();

        /// <summary>
        /// returns the Clarion Application Service BO
        /// </summary>
        /// <returns></returns>
        Cic.OpenOne.Common.BO.ICASBo getCASBo();

        /// <summary>
        /// returns the CRM Entity BO repsonsible for all create, update, get operations of Entities
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IEntityBo getEntityBO(Cic.OpenOne.Common.Util.Security.MembershipUserValidationInfo info);


        /// <summary>
        /// return the BO for Mail Management
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Cic.One.Web.BO.Mail.IEntityMailBo getEntityMailBO(Cic.OpenOne.Common.Util.Security.MembershipUserValidationInfo info);

        /// <summary>
        /// return the BO for Mail Management for a certain user
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Cic.One.Web.BO.Mail.IEntityMailBo getEntityMailBO(long syswfuser);

        /// <summary>
        /// returns the BO for document management search
        /// </summary>
        /// <returns></returns>
        Cic.One.Web.BO.Search.IDocumentSearchBo getDocumentSearchBO();

        /// <summary>
        /// returns Print BO
        /// </summary>
        /// <returns></returns>
        IPrintBo getPrintBo();

        /// <summary>
        /// returns the State Service BO
        /// </summary>
        /// <returns></returns>
        IStateServiceBo getStateServiceBo();

        /// <summary>
        /// returns the Clarion Application Service BO
        /// </summary>
        /// <returns></returns>
        IWorkflowBo getWorkflowBo();

        /// <summary>
        /// returns the xpro search bo
        /// </summary>
        /// <returns></returns>
        Cic.One.Web.BO.Search.IXproSearchBo getXproSearchBo();

         /// <summary>
        /// RightsMap holen
        /// </summary>
        /// <returns>IVertragsListenBo</returns>
        IRightsMapBo createRightsMapBo();


        /// <summary>
        /// returns the calculation business object
        /// </summary>
        /// <returns></returns>
        ICalculationBo getCalculationBo();
    }
}
