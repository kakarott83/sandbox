using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class ogetCreditLimitsGUIDto
    {
        public List<ProductCreditInfoDto> productCreditInfoDto
        {

            set;
            get;
        }
        public string XMLDto
        {
            get;
            set;
        }
    }
}
