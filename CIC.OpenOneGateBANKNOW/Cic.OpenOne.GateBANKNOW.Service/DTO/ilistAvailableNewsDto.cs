using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableNews"/> Methode
    /// </summary>
    public class ilistAvailableNewsDto
    {
        /// <summary>
        /// Brand
        /// </summary>
        public long sysbrand
        {
            get;
            set;
        }
        /// <summary>
        /// Kanal
        /// </summary>
        public long sysprchannel
        {
            get;
            set;
        }

        /// <summary>
        /// if true binary data will be delivered
        /// </summary>
        public bool binaryData
        {
            get;
            set;
        }
    }
}
