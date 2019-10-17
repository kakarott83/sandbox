using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ReminderDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysReminder { get; set; }

        /*Verweis zur E-Mail */
        public long sysMailMsg { get; set; }

        /*Verweis zum Termin */
        public long sysApptmt { get; set; }

        /*Verweis zur Aufgabe */
        public long sysPtask { get; set; }

        /*Gebiet (zb E-Mail, Termin, Aufgabe ...) */
        public String area { get; set; }

        /*Verweis zum Satz im Gebiet */
        public long sysId { get; set; }

        /*Erinnerungsdatum */
        public DateTime? startDate { get; set; }

        /*Erinnerungszeitpunkt */
        public long startTime { get; set; }

        /// <summary>
        /// Fälligkeitsdatum
        /// </summary>
        public DateTime? duedate { get; set; }

        /// <summary>
        /// Fälligkeitszeit
        /// </summary>
        public long duetime { get; set; }

        /// <summary>
        /// Benutzer, für den die Wiedervorlage ist
        /// </summary>
        public long sysWfuser { get; set; }

        /// <summary>
        /// Rolle, für die die Wiedervorlage ist
        /// </summary>
        public long sysBprole { get; set; }

        /// <summary>
        /// Priorität
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// Private Flag
        /// </summary>
        public long privateflag { get; set; }

        /// <summary>
        /// Titel
        /// </summary>
        public String title { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public String description { get; set; }

        /// <summary>
        /// Clarification Memo an diesem Reminder
        /// </summary>
        public long syswfmmemo { get; set; }

        /// <summary>
        /// dringend
        /// </summary>
        public long important
        {
            get
            {
                return priority > 0 ? 1 : 0;
            }
        }

        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? startTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)startTime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    startTime = (long)val.Value;

                else
                    startTime = 0;
            }
        }

        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? dueTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)duetime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    duetime = (long)val.Value;

                else
                    duetime = 0;
            }
        }


        override public long getEntityId()
        {
            return sysReminder;
        }

    }
}