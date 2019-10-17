using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Haendler
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetHaendlerDetailDto : oBaseDto
    {
        public HaendlerDto haendler
        {
            get;
            set;
        }
    }
}