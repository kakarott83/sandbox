using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers the id of the created antrag
    /// </summary>
    public class ocreateAntragFromNkkDto : oBaseDto
    {
        public long sysantrag { get; set; }
    }
}