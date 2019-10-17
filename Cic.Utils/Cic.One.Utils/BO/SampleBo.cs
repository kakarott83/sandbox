using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// sample Bo class
    /// </summary>
    public class SampleBo : AbstractSampleBo
    {
        /// <summary>
        /// SampleBo-Klasse
        /// </summary>
        /// <param name="dao"></param>
        public SampleBo(ISampleDao dao)
            : base(dao)
        {
        }

        /// <summary>
        /// sample Method 
        /// </summary>
        /// <param name="sampleParameter">sample parameter</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>sample value</returns>
        public override oSampleDto sampleMethod(iSampleDto sampleParameter)
        {
            return dao.sampleMethod(sampleParameter);
        }
    }
}