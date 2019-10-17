using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetDdlkpcolDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of Wertebereiche
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public DdlkpcolDto ddlkpcol
        {
            get;
            set;
        }
    }
}