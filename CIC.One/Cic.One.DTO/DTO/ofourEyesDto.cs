using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ofourEyesDto : oBaseDto
    {
        public long syswer1 { get; set; }
        public long syswer2 { get; set; }
        public String benutzer1 { get; set; }
        public String benutzer2 { get; set; }
        public String bemerkung1 { get; set; }
        public String bemerkung2 { get; set; }

        public long value1 { get; set; }
        public long value2 { get; set; }
        public String attribut1 { get; set; }
        public String attribut2 { get; set; }
    }
}
