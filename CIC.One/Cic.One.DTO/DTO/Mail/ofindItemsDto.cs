using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ofindItemsDto : oBaseDto
    {
        public List<MItemDto> Items { get; set; }
    }
}