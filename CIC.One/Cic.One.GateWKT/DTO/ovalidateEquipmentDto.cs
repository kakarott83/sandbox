using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output for calculation of all rate prices
    /// </summary>
    public class ovalidateEquipmentDto : oBaseDto
    {
        public EquipmentValidationDto result {get;set;}
    }
}