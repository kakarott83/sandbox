using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Cic.OpenOne.Service.DTO;
using Cic.OpenOne.Common.Util.SOAP.Annotations;

namespace Cic.OpenOne.Service.Contracts
{
    
    /// <summary>
    /// Sample Service Contract
    /// </summary>
    [ServiceContract]
    public interface ISampleService
    {
        /// <summary>
        /// Sample Service Method
        /// </summary>
        /// <param name="sampleParameter">service Parameter</param>
        /// <returns>service return value</returns>
        [OperationContract]
        [ExportWsdlWithXsdAnnotationsOperationBehavior()]
        oSampleMethodDto sampleMethod(iSampleMethodDto sampleParameter);

    }


}
