namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    public class SchufaTPerson
    {
        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Geschlecht { get; set; }

        public System.DateTime? GeburtsdatumIso { get; set; }

        public SchufaTAdresse AktuelleAdresse { get; set; }

        public SchufaTAdresse Voradresse { get; set; }

        public string Namenszusatz { get; set; }

        public string SCHUFAID { get; set; }

        public string TelefonNr { get; set; }

        public string Bankleitzahl { get; set; }

        public string Kontonummer { get; set; }

        public string Geburtsort { get; set; }

        public string Geburtsname { get; set; }
    }
}
