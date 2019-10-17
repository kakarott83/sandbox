using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class igetBonitaetSchufaDto
    {
        public OpenLeaseAuskunftManagement.DTO.SchufaTMerkmal Anfragemerkmal { get; set; }

        public OpenLeaseAuskunftManagement.DTO.SchufaTVerbraucherdaten Verbraucherdaten { get; set; }
    }
}
