using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetDdlkprubDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of Rubriken
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public DdlkprubDto ddlkprub
        {
            get;
            set;
        }
    }

}