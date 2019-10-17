using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Parameterklasse für AvailableServiceDto
    /// </summary>
    public class AvailableProduktDto : DropListDto
    {
        /// <summary>
        /// Indentation
        /// </summary>
        public long indent
        {
            get;
            set;
        }

        /// <summary>
        /// Code of the Products VTTYP
        /// </summary>
        public String vttypcode { get; set; }

        /// <summary>
        /// Product type id
        /// </summary>
        public long sysprprodtype { get; set; }
      
		
    }
}
