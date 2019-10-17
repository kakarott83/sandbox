using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class EquipmentValidationDto
    {
        //true if the equipment is overall valid
        public bool valid { get; set; }
        //...more details, list of items each containing the dependent required/relationerrors to others
    }
}