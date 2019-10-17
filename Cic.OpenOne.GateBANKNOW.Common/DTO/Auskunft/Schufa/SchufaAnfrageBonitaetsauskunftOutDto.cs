using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    public class SchufaAnfrageBonitaetsauskunftOutDto : BonitaetsauskunftType
    {
        public string SchufaReferenz { get; set; }

        public string Teilnehmerreferenz { get; set; }

        public string Abrufreferenz { get; set; }

        public string Abrufversion { get; set; }
    }
}