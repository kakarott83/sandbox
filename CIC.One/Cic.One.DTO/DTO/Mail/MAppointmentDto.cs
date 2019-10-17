using System;
using System.Collections.Generic;

namespace Cic.One.DTO
{
    public class MAppointmentDto : MItemDto
    {
        /// <summary>
        /// Enthält die Startzeit
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Enthält die Endzeit
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Ganztägiger Termin
        /// </summary>
        public bool IsAllDayEvent { get; set; }

        /// <summary>
        /// Ort
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Enthält die benötigten Teilnehmer
        /// </summary>
        public List<MAttendeeDto> RequiredAttendees { get; set; }

        /// <summary>
        /// ~~~Enthält die anwesenden Resourcen
        /// </summary>
        public List<MAttendeeDto> Resources { get; set; }

        /// <summary>
        /// Enthält die optionalen Teilnehmer
        /// </summary>
        public List<MAttendeeDto> OptionalAttendees { get; set; }

        /// <summary>
        /// Gibt an, welche Art das meeting hat (Anzeigen als (1=Frei, 2=mit Vorbehalt, 3=Beschäftigt, 4=Abwesend))
        /// </summary>
        public MLegacyFreeBusyStatus? LegacyFreeBusyStatus { get; set; }

        /// <summary>
        /// Enthält das Muster für die Recurrence (entspricht der Datenstruktur aus der Datenbank)
        /// </summary>
        public MRecurrence Recurrence { get; set; }

    }
}