using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AdmaddDto:EntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long sysAdmadd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sysPerson { get; set; }

        public string orga { get; set; }

        public override long getEntityId()
        {
            return sysAdmadd;
        }
    }
}