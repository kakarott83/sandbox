using System;

namespace Cic.One.DTO
{
    public class MTaskDto : MItemDto
    {
        //Microsoft.Exchange.WebServices.Data.Task

        /// <summary>
        /// Due Date
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Ist fertig
        /// TODO: hat eigentlich nur getter
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Ist Team Task
        /// TODO: hat eigentlich nur getter
        /// </summary>
        public bool IsTeamTask { get; set; }

        /// <summary>
        /// Task Modus
        /// TODO: hat eigentlich nur getter
        /// </summary>
        public MTaskModeEnum Mode { get; set; }

        /// <summary>
        /// Task Status
        /// </summary>
        public MTaskStatusEnum Status { get; set; }

        /// <summary>
        /// Fertigstellungsdatum
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// Startdatum
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Enthält das Muster für die Recurrence (entspricht der Datenstruktur aus der Datenbank)
        /// </summary>
        public MRecurrence Recurrence { get; set; }

    }
}