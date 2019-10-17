using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Holds the data for one insurance calculation
    /// * InsuranceParameter holds the user-entered Data
    /// * InsuranceResult holds the calculated results for the user-data
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class InsuranceDto
    {
        [System.Runtime.Serialization.DataMember]
        public long SysAngVs
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public InsuranceParameterDto InsuranceParameter
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public InsuranceResultDto InsuranceResult
        {
            get;
            set;
        }
    }
}
