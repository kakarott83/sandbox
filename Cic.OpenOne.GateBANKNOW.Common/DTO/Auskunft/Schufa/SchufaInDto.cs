using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    public class SchufaInDto
    {
        public SchufaMeldungInDto Meldung { get; set; }

        public SchufaAbrufNachmeldungInDto AbrufNachmeldung { get; set; }

        public SchufaAnfrageBonitaetsauskunftInDto AnfrageBonitaetsauskunft { get; set; }

        public SchufaAbrufManuelleWeiterverarbeitungInDto AbrufManuelleWeiterverarbeitung { get; set; }

        public long SysId { get; set; }
    }
}
