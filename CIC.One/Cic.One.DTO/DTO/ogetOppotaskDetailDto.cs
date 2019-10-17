using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;


namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Activty
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetOppotaskDetailDto : oBaseDto
    {
        public OppotaskDto oppotask
        {
            get;
            set;
        }
    }
}