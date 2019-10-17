using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter getDMSUrl Methode
    /// </summary>
    public class ogetDMSUrlDto : oBaseDto
    {
        /// <summary>
        /// URL to document
        /// </summary>
       public String url {get;set;}
    }
}
