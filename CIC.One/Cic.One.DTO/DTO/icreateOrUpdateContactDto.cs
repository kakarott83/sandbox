using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class icreateOrUpdateContactDto
    {
        public ContactDto contact { get; set; }
        public String forSQL { get; set; }
    }
}
