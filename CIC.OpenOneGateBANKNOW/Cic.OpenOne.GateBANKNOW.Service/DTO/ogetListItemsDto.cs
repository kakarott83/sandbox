using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für getListItems
    /// </summary>
    public class ogetListItemsDto : oBaseDto
    {
        /// <summary>
        /// Output-Items for fetching a list by code
        /// </summary>
        public DropListDto[] items
        {
            get;
            set;
        }
    }
}
