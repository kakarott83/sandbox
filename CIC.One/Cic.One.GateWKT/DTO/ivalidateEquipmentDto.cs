using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for validating selected equipment
    /// </summary>
    public class ivalidateEquipmentDto
    {
        public String eurotaxNr {get;set;}
        /// <summary>
        /// if >0 it will only checked if this item might be added to the other
        /// </summary>
        public long checkAddEquipmentId { get; set; }
        /// <summary>
        /// if>0 it will only be checked if this item might be deleted from the others
        /// </summary>
        public long checkRemoveEquipmentId { get; set; }
        /// <summary>
        /// if the checkAdd/Remove-Item is empty, each item of this list will be checked if it could be added
        /// </summary>
        public long[] equipmentIds { get; set; }
    }
}