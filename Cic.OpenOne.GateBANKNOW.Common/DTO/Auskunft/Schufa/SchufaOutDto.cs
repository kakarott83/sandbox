namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    using Type;

    public class SchufaOutDto
    {
        public SchufaMeldungOutDto Meldung;

        public SchufaAbrufNachmeldungOutDto AbrufNachmeldung;

        public SchufaAnfrageBonitaetsauskunftOutDto AnfrageBonitaetsauskunft;
        
        public SchufaTAusnahmeDto Error;
    }
}
