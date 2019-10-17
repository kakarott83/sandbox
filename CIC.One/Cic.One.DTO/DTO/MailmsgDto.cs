using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class MailmsgDto : EntityDto
    {

        /*Primärschlüssel */
        public long sysMailmsg { get; set; }
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
        /*Verweis zu Wfuser, Sender */
        public long sysOwner { get; set; }
        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }
        /*Exchange ID */
        public String itemId { get; set; }
        /*Gesendet an */
        public String sentTo { get; set; }
        /*Gesendet an in Kopie */
        public String sentToCC { get; set; }
        /*Gesendet an in Blindkopie */
        public String sentToBCC { get; set; }
        /*Sendedatum */
        public DateTime? sentDate { get; set; }
        /*Sendeuhrzeit */
        public long sentTime { get; set; }



        /*Empfangen von */
        public String recvFrom { get; set; }
        /*Empfangsdatum */
        public DateTime? recvDate { get; set; }
        /*Empfangsurhzeit */
        public long recvTime { get; set; }
        /*Gelesen */
        public bool readFlag { get; set; }
        /*Antwort erforderlich */
        public int respReqFlag { get; set; }
        /*Versendebestätigung erforderlich */
        public int delReqFlag { get; set; }
        /*Empfangsbestätigung erforderlich */
        public int readRecptReqFlag { get; set; }
        /*Priorität (0=Keine, 1=hoch, 2=niedrig) */
        public int priority { get; set; }
        /*Betreff */
        public String subject { get; set; }
        /*Inhalt */
        public String content { get; set; }

        /// <summary>
        /// Gibt an ob die Mail ein Draft ist
        /// </summary>
        public int IsDraft { get; set; }

        override public long getEntityId()
        {
            return sysMailmsg;
        }
        override public String getEntityBezeichnung()
        {
            return subject;
        }

        /*Sendeuhrzeit GUI*/
        public DateTime? sentTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)sentTime);
            }
            set
            {
                sentTime = sentTime;
            }

        }

        /*Empfangsuhrzeit GUI*/
        public DateTime? recvTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)recvTime);
            }
            set
            {
                recvTime = recvTime;
            }

        }

        /// <summary>
        /// flag if this item was just created
        /// </summary>
        public bool isNew { get; set; }

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
        /// number of attachments
        /// </summary>
        public int attachments { get; set; }

    }
}