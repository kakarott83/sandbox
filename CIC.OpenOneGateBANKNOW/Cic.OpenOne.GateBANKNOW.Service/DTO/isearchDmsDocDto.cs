using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für serachDmsdoc
    /// </summary>
    public class isearchDmsDocDto
    {
        public bool rollenAttributRechte { get; set; }

        /// <summary>
        /// Allgemeines Suchobjekt
        /// </summary>
        public iSearchDto searchInput
        {
            get;
            set;
        }
    }
}
