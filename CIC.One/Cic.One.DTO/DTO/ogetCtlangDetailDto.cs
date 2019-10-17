using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetCtlangDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of Sprachen
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public CtlangDto ctlang
        {
            get;
            set;
        }
    }
}