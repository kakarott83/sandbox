using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableUser"/> Methode
    /// </summary>
    public class olistAvailableUserDto : oBaseDto
    {
        /// <summary>
        /// Array von User
        /// </summary>
        public DropListDto[] user
        {
            get;
            set;
        }
    }
}