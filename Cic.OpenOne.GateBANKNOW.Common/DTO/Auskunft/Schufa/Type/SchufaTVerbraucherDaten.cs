namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    public class SchufaTVerbraucherdaten
    {
        public string SCHUFAID { get; set; }

        public string Titel { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Geschlecht { get; set; }

        public string Geburtsdatum { get; set; }

        public string Geburtsort { get; set; }

        public SchufaTAdresse AktuelleAdresse { get; set; }

        public SchufaTAdresse Voradresse { get; set; }

        public string SchufaKlauselDatum { get; set; }
    }
}
