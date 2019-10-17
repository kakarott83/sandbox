namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class EaiErrorDto
    {
        #region Properties
        public int ErrorNumber
        {
            get;
            set;
        }

        public string ErrorAction
        {
            get;
            set;
        }

        public string ErrorText
        {
            get;
            set;
        }

        public DateTime ErrorTime
        {
            get;
            set;
        }
        #endregion
    }
}
