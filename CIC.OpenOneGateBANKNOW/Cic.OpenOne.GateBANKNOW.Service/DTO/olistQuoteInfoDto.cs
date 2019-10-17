using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class olistQuoteInfoDto : oBaseDto
    {
        /// <summary>
        /// Liste von Quoten/Konstanten gültig ab jetzt
        /// </summary>
        public List<QuoteInfoDto> quotes { get; set; }
    }
}