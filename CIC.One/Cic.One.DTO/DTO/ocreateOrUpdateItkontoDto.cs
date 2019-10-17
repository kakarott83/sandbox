using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Itkonto
    /// </summary>
    public class ocreateOrUpdateItkontoDto : oBaseDto
    {
        public ItkontoDto itkonto { get; set; }
    }
}