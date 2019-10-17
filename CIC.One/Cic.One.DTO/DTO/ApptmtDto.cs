using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ApptmtDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysApptmt { get; set; }
        /*Verweis zum Vorgänger */
        public long sysApptmtParent { get; set; }
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
        /*Verweis zu Wfuser, Termininhaber */
        public long sysOwner { get; set; }

        /*Verweis zu dem früherern Wfuser */
        public long sysOwnerOld { get; set; }

        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }
        /*Exchange ID */
        public String itemId { get; set; }
        ///*Quelle analog Exchange (1=Normal 2=Request, 3=RequestAccepted, 4=RequestDeclined, 5=SelfDelegated, 6=Update) */
        //public int taskMode { get; set; }
        /*Beginndatum */
        public DateTime? startDate { get; set; }
        /*Beginnuhrzeit */
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
        /*Endedatum */
        public DateTime? endDate { get; set; }
        /*Endeuhrzeit */
        public long endTime { get; set; }
        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? endTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)endTime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    endTime = (long)val.Value;
                else
                    endTime = 0;
            }
        }
        /*Ganztätiger Termin */
        public int allDayFlag { get; set; }
        /*Ort */
        public String location { get; set; }
        /*Anzeigen als (1=Frei, 2=mit Vorbehalt, 3=Beschäftigt, 4=Abwesend) */
        public int showAs { get; set; }
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

        /// <summary>
        /// Gibt an, ob das Apptmt ein Recurring Apptmt ist
        /// </summary>
        public bool recurring { get; set; }

        override public long getEntityId()
        {
            return sysApptmt;
        }

        override public String getEntityBezeichnung()
        {
            return subject;
        }

    }
}