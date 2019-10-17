namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    public class SchufaTAdresse
    {
        public string Strasse { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }

        /// <summary>
        /// Als Angabe muss ein dreistelliger Code nach ISO 3166 ALPHA-3 geliefert werden.
        /// </summary>
        public string Land { get; set; }
    }
}
