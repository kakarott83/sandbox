using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class ogetQuotesDto : oBaseDto
    {
        /// <summary>
        /// Liste von Quoten/Konstanten gültig ab jetzt
        /// </summary>
        public List<QuoteInfoDto> quotes { get; set; }
    }
}
