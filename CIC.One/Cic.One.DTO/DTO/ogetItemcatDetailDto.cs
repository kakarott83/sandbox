using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetItemcatDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of Kategorien
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public ItemcatDto itemcat
        {
            get;
            set;
        }
    }
}