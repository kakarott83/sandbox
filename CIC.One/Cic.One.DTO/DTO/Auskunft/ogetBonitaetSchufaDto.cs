using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetBonitaetSchufaDto : oBaseDto
    {
        public string SchufaReferenz { get; set; }

        public string Teilnehmerreferenz { get; set; }

        public OpenLeaseAuskunftManagement.DTO.SchufaTBonitaetsauskunft Reaktion { get; set; }
    }
}
