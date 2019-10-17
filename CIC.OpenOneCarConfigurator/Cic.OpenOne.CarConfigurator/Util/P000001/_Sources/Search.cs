using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [Serializable]
    public class Search
    {

        #region IInputData methods
        public void CheckProperties()
        {
            try
            {
                // TODO MK 0 MK, check
            }
            // TODO BK 0 BK, catch specific exceptions
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region Properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public SearchParam[] SearchParams
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public SimpleSearchParam SimpleSearchParam
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public bool SimpleSearch
        {
            get;
            set;
        }
        #endregion
    }
}
