using System;
using System.ServiceModel;

namespace IBANKernel
{

    [ServiceContract(Name = "IBANKernel", Namespace = "http://cic-software.de/IBANKernel")]
    public interface IBANKernel
    {

        [OperationContract]
        IBANInfo getIBANInfo(String kontoNummer, String bcpcNummer);

        [OperationContract]
        IBANVersionInfo getIBANVersion();       

    }


}
