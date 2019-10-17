using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa
{
    public class SchufaTAbrufManuelleWeiterverarbeitung
    {
        /// <summary>
        /// 8-stelliges alphanumerisches Feld
        /// Dient zur Identifikation einer Bonitäts-Anfrage
        /// </summary>
        public string Abrufreferenz { get; set; }

        public SchufaTAktionsHeader AktionsHeader { get; set; }
    }
}