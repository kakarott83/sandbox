using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created Angebot
    /// </summary>
    public class ocreateOrUpdateBNAntragDto : oBaseDto
    {
        public BNAntragDto antrag { get; set; }
    }
}