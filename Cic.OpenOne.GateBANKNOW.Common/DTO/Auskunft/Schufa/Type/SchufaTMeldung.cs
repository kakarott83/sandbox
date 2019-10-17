namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type
{
    public class SchufaTMeldung
    {
        public SchufaTAktionsHeader AktionsHeader { get; set; }

        public SchufaTVerbraucherdaten Verbraucherdaten { get; set; }

        public SchufaTSachbearbeiter Ansprechpartner { get; set; }

        public SchufaTAdresse NeueAdresse { get; set; }

        public SchufaTVerbraucherdaten KorrigierteVerbraucherdaten { get; set; }

        public SchufaTSterbedaten Sterbedaten { get; set; }

        public SchufaTMerkmal Meldemerkmal { get; set; }

        public string Meldeart { get; set; }

        public string Zusatzinformation { get; set; }

        public string NeuerNachname { get; set; }

    }
}
