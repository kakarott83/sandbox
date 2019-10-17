using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class PtaskDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPtask { get; set; }
        /*Verweis zum Vorgänger */
        public long sysPtastParent { get; set; }
        /*Verweis zum Checklistentyp */
        public long sysPtype { get; set; }
        /*Verweis zum Checklistentyp */
        public long sysPtasktp { get; set; }
        /*Checklistentyp bezeichnung */
        public long ptypeBezeichnung { get; set; }
        /*Verweis zum Aufgabentyp */
        public long sysPtasktyp { get; set; }
        /*Verweis zur Person */
        public long sysPerson { get; set; }
        /* Ansprechpartner */
        public long sysPartner { get; set; }
        /*Verweis zum Kontakt */
        public long sysContact { get; set; }
        /*Verweis zur Opportunity */
        public long sysOppo { get; set; }
        /*Verweis zur Kampagne */
        public long sysCamp { get; set; }
        /*Verweis zum Angebot */
        public long sysAngebot { get; set; }
        /*Verweis zum Antrag */
        public long sysAntrag { get; set; }
        /*Verweis zum Vertrag */
        public long sysVt { get; set; }
        /*Verweis zu Wfuser, Aufgabeninhaber */
        public long sysOwner { get; set; }


        /*Verweis zu dem früherern Wfuser */
        public long sysOwnerOld { get; set; }

        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }
        /*Exchange ID */
        public String itemId { get; set; }
        /*Quelle analog Exchange (1=Normal 2=Request, 3=RequestAccepted, 4=RequestDeclined, 5=SelfDelegated, 6=Update) */
        public int taskMode { get; set; }
        /*Beginndatum */
        public DateTime? startDate { get; set; }
        /*Fälligkeitszeit */
        public long startTime { get; set; }
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
        /*Fälligkeitsdatum */
        public DateTime? dueDate { get; set; }
        /*Fälligkeitszeit */
        public long dueTime { get; set; }
        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? dueTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)dueTime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    dueTime = (long)val.Value;

                else
                    dueTime = 0;
            }
        }
        /*(0=nicht begkonnen, 1=in Bearbeitung, 2=erledigt, 3=warten auf jemand anderen, 4=zurückgestellt) */
        public int prozessStatus { get; set; }
        /*Erledigt */
        public int completeFlag { get; set; }
        /*Teamaufgabe */
        public int teamFlag { get; set; }
        /*Priorität (0=Keine, 1=hoch, 2=niedrig) */
        public int priority { get; set; }
        /*Betreff */
        public String subject { get; set; }
        /*Inhalt */
        public String content { get; set; }


        /* Ansprechpartner name*/
        public String partnerName { get; set; }

        /// <summary>
        /// Name des Accounts
        /// </summary>
        public String personName { get; set; }

        /// <summary>
        /// Namenskürzel des Owners
        /// </summary>
        public String wfuserName { get; set; }

        override public long getEntityId()
        {
            return sysPtask;
        }
        override public String getEntityBezeichnung()
        {
            return subject;
        }
    }
}