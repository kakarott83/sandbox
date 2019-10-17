using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// sample Bo class
    /// </summary>
    public abstract class AbstractSampleBo : ISampleBo
    {
        /// <summary>
        /// SampleDao
        /// </summary>
        protected ISampleDao dao;

        /// <summary>
        /// SampleBo Constructor
        /// </summary>
        /// <param name="daoParameter"></param>
        public AbstractSampleBo(ISampleDao daoParameter)
        {
            this.dao = daoParameter;
        }

        /// <summary>
        /// sample Method 
        /// </summary>
        /// <param name="sampleParameter">sample parameter</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>sample value</returns>
        public abstract oSampleDto sampleMethod(iSampleDto sampleParameter);
    }
}
