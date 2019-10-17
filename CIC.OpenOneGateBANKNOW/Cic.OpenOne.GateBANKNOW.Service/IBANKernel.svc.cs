using System.ServiceModel;

namespace IBANKernel
{
    [ServiceBehavior(Namespace = "http://cic-software.de/IBANKernel")]
    public class IBANService : IBANKernel
    {
        public IBANInfo getIBANInfo(string kontoNummer, string bcpcNummer)
        {
            return IBANKernelAccess.getIBANInfo(kontoNummer, bcpcNummer);
        }
        public IBANVersionInfo getIBANVersion()
        {
            return IBANKernelAccess.getIBANVersion();
        }

    }
}
