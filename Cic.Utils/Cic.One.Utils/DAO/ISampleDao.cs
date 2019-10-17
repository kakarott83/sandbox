using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Sample Dao Interface
    /// </summary>
    public interface ISampleDao
    {
        /// <summary>
        /// sample Method
        /// </summary>
        /// <param name="sampleParameter">sample Parameter</param>
        /// <returns>sample Value</returns>
        oSampleDto sampleMethod(iSampleDto sampleParameter);
    }
}
