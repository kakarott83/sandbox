using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;


namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Finanzierung
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetFinanzierungDetailDto : oBaseDto
    {
        public FinanzierungDto finanzierung
        {
            get;
            set;
        }
    }
}