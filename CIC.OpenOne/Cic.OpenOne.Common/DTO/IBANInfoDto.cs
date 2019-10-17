using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DTO
{
    public class IBANInfoDto
    {
        public String countryCode { get; set; }
        public int inputFields { get; set; }
        public int length { get; set; }
    }
}
