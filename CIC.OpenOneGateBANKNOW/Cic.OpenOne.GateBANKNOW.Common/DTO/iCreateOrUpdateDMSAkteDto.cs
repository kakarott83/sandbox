using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für CreateOrUpdateDMSAkte Methode
    /// </summary>
    public class icreateOrUpdateDMSAkteDto
    {
        /// <summary>
        /// id of dmsakte containing all input information
        /// </summary>
        public long sysdmsakte { get; set; }





    }

    /// <summary>
    /// DMS Batch processing input
    /// </summary>
    public class icreateOrUpdateDMSAkteBatchDto
    {
        /// <summary>
        /// id of dmsbatch containing all input information
        /// </summary>
        public long sysdmsbatch { get; set; }



    }

    /// <summary>
    /// DMS Batch processing output
    /// </summary>
    public class ocreateOrUpdateDMSAkteBatchDto
    {
        public String retcode { get; set; }
        public String errcode { get; set; }
        public String errmessage { get; set; }
    }
}