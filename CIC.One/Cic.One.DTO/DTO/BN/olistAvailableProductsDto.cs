using Cic.OpenOne.Common.DTO;
using System.Collections.Generic;

namespace Cic.One.DTO.BN
{
    public class olistAvailableProductsDto : oBaseDto
    {
        public DropListDto[] products { get; set; }
    }
}
