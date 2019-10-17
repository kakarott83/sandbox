// OWNER MK, 08-07-2009
using Cic.One.Utils.Util.Exceptions;
namespace Cic.OpenLease.Service
{
    [System.CLSCompliant(true)]
    public class ServiceException : System.ServiceModel.FaultException
    {
        #region Constructors
        public ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes code)
            : base(code.ToString(), new System.ServiceModel.FaultCode(((int)code).ToString()))
        {
        }

        public ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes code, string message)
            : base(message, new System.ServiceModel.FaultCode(((int)code).ToString()))
        {
        }

        public ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes code, System.Exception exception)
            : base(ExceptionUtil.DeliverFlatExceptionMessage(exception), new System.ServiceModel.FaultCode(((int)code).ToString()))
        {
        }
        #endregion
    }
}
