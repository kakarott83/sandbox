using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    public class SchufaTAbrufNachmeldungPI
    {
        public SchufaTAktionsHeader AktionsHeader { get; set; }

        public SchufaTProduktinformationen Produktinformationen { get; set; }
    }
}