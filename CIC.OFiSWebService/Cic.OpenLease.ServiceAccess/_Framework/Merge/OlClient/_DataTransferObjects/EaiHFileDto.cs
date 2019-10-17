namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class EaiHFileDto
    {
        #region Properties
        public string Code
        {
            get;
            set;
        }
        
        public string FileName
        {
            get;
            set;
        }

        public byte[] FileContents
        {
            get;
            set;
        }
        #endregion
    }
}
