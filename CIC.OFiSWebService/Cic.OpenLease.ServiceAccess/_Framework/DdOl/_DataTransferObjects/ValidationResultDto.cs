namespace Cic.OpenLease.ServiceAccess.DdOl
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;



    public class ValidationResultDto
    {

        #region properties

        [System.Runtime.Serialization.DataMember]
        public ValidationStatus validationId
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public bool valid
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public bool hasMessage
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public String Message
        {
            get;
            set;
        }

        #endregion
    }

}