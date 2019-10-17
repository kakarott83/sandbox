using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetCrmnmDetailDto : oBaseDto
    {
        /// <summary>
        /// Returns the detail of PartnerBeziehungen
        /// derives from oBaseDto to include Error and runtime information
        /// </summary>
        public CrmnmDto crmnm
        {
            get;
            set;
        }

    }
}