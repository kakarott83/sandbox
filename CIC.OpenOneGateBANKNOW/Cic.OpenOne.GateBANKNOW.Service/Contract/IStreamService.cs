using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System.IO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface searchVertragService stellt die Methoden zur Service-Statusabfrage bereit
    /// </summary>
    [ServiceContract(Name = "IStreamService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IStreamService
    {

       /* [OperationContract]
        Stream getData(int id);

        [OperationContract]
        void setData(RemoteDataInfo data);*/

        /// <summary>
        /// receives the input of ss
        /// </summary>
        /// <param name="input"></param>
        [OperationContract(IsOneWay=true)]
        void setAuskunftS1(S1InputData input);

        /// <summary>
        /// performs a connection Test
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        oMessagingDto connectionTest();
    }
}