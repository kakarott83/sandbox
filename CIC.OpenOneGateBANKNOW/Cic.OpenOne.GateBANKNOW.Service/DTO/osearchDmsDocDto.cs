using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für searchDmsdoc
    /// </summary>
    public class osearchDmsDocDto : oBaseDto
    {
        /// <summary>
        /// Search Results
        /// </summary>
        public oSearchDto<DmsDocDto> result
        {
            get;
            set;
        }

    }
}
