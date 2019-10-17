using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableBrands"/> Methode
    /// </summary>
    public class olistAvailableBrandsDto : oBaseDto
    {
        /// <summary>
        /// Array von Brands
        /// </summary>
        public DropListDto[] brands
        {
            get;
            set;
        }
    }
}
