
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    /// <summary>
    /// Allgemeines Schufa Transaction Error DTO
    /// </summary>
    public class SchufaTAusnahmeDto
    {
        /// <summary>
        /// Code
        /// </summary>
        public long Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        public string SchufaReferenz { get; set; }

        public string Teilnehmerreferenz { get; set; }

        public System.Collections.Generic.List<SchufaTFehler> FehlerlisteFachlich { get; set; }

        public System.Collections.Generic.List<SchufaTFehler> FehlerlisteTechnisch { get; set; }
    }
}
