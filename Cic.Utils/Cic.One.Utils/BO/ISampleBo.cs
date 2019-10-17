using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Sample Bo Interface
    /// </summary>
    public interface ISampleBo
    {
        /// <summary>
        /// sample Method
        /// </summary>
        /// <param name="sampleParameter">sample parameter</param>
        /// <returns>sample return value</returns>
        oSampleDto sampleMethod(iSampleDto sampleParameter);
    }
}
