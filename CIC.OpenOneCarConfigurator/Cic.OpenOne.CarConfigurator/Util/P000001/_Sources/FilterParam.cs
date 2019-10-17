using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    /// <summary>
    /// ONE FilterParam for a given Level must be met for a node to be displayed
    /// equals OR-Search
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    public class FilterParam : Cic.P000001.Common.IFilterParam
    {
        #region Constructors
        public FilterParam()
        {
        }

        public FilterParam(Cic.P000001.Common.Level filterAtLevel, string filterPattern)
        {
            this.FilterAtLevel = filterAtLevel;
            this.Filter = filterPattern;
        }
        #endregion

        #region IInputData methods
        public void CheckProperties()
        {
            try
            {
                // TODO MK 0 MK, Check
            }
            // TODO BK 0 BK, catch specific exceptions
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region IFilterParam properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public string Filter
        {
            get;
            set;
        }
        #endregion

        #region Properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public Cic.P000001.Common.Level FilterAtLevel
        {
            get;
            set;
        }
        #endregion
    }
}
