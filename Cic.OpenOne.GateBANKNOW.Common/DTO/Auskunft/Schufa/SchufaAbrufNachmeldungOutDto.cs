using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    public class SchufaAbrufNachmeldungOutDto : NachmeldungType
    {
        public string SchufaReferenz { get; set; }

        public string Teilnehmerreferenz { get; set; }

        public bool KeineNachrichtenVerfuegbar { get; set; }
    }
}