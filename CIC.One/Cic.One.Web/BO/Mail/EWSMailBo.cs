using Cic.One.Web.DAO.Mail;

namespace Cic.One.Web.BO.Mail
{
    public class EWSMailBo : AbstractMailBo
    {
        public EWSMailBo(IMailDao ewsDao)
            : base(ewsDao)
        {
        }
    }
}