using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Outputparameter für listAvailableLenker 
    /// </summary>
    public class olistAvailableLenkerDto : oBaseDto
    {
        /// <summary>
        /// Array von Lenker
        /// </summary>
        public DropListDto[] lenker
        {
            get;
            set;
        }
    }
}