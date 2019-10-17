using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetDdlkpposDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of Werte
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public DdlkpposDto ddlkppos
        {
            get;
            set;
        }
    }

}