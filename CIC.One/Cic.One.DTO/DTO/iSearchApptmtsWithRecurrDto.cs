using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class iSearchApptmtsWithRecurrDto
    {
        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public iSearchDto SearchParameter { get; set; }

    }
}
