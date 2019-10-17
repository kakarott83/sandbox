using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    public class SchufaTAktionsHeader
    {
        public string RueckgabeAktionsdaten { get; set; }

        public string timestamp { get; set; }

        public string Teilnehmerreferenz { get; set; }
    }
}
