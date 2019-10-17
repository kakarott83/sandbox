using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO

{
    public class IBANkonto : oBaseDto
    {

        public String kontonummer { get; set; }

        public String BLZ { get; set; }

        public String BIC { get; set; }

        public String IBAN { get; set; }

        
    }
}
