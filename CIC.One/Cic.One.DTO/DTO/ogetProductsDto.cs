using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetProductsDto : oBaseDto
    {
        public AvailableProduktDto[] produkte
        {
            get;
            set;
        }
        public List<PrproductDto> productData { get;set;}
        public List<VartDto> vartData { get; set; }
        
    }
}
