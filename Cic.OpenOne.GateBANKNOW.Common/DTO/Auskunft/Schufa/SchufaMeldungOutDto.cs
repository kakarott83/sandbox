namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

    public class SchufaMeldungOutDto : MeldungsbestaetigungType
    {
        public string SchufaReferenz { get; set; }

        public string Teilnehmerreferenz { get; set; }
    }
}
