using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ContactDto : EntityDto
    {
        public long sysContact { get; set; }

        public long sysContactTp { get; set; }

        public String contactTpName { get; set; }

        public long sysPerson { get; set; }
        /* Ansprechpartner */
        public long sysPartner { get; set; }

        /* Ansprechpartner name*/
        public String partnerName { get; set; }

        public String personName { get; set; }

        public String personVorname { get; set; }
        
        public String oppoName { get; set; }

        public long sysCamp { get; set; }

        public long sysOppo { get; set; }

        public long sysAngebot { get; set; }

        public long sysAntrag { get; set; }

        public long sysVt { get; set; }
        /*	Inhaber	*/
        public long sysOwner { get; set; }
        /*	Privat	*/
        public int privateFlag { get; set; }
        /*	Kontaktweg	*/
        public int way { get; set; }
        /*	Richtung	*/
        public int direction { get; set; }
        /*	Grund	*/
        public String reason { get; set; }
        /*		*/
        public String reasonCode { get; set; }
        /*	Ort	*/
        public String place { get; set; }
        /*		*/
        public String placeCode { get; set; }
        /*	Datum	*/
        public DateTime? comDate { get; set; }
        /*	Uhrzeit	*/
        public long comTime { get; set; }
        /*	Uhrzeit b2b	*/
        public DateTime? comTimeb2b
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)comTime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    comTime = (long)val.Value;

                else
                    comTime = 0;
            }
        }
        /*	Dauer in Minuten	*/
        public int comDura { get; set; }
        /*	Ergebnis	*/
        public String result { get; set; }
        /*	Bemerkung	*/
        public String memo { get; set; }
        /*	Nächste Aktion	*/
        public String nextStep { get; set; }
        /* Ende */
        public String ende { get; set; }

        override public long getEntityId()
        {
            return sysContact;
        }


        public override string getEntityBezeichnung()
        {
            string fullname = personName;
            if (fullname == null) fullname = "";

            string vorname = personVorname;
            if (vorname == null) vorname = "";

            if (vorname.Length > 0)
            {
                if (fullname.Length > 0)
                {
                    fullname += ", " + vorname;
                }
                else
                {
                    fullname = vorname;
                }
            }
            if (fullname == null || fullname.Length == 0)
            {
                fullname = result;
                if (fullname != null && fullname.Length > 32)
                    fullname = fullname.Substring(0, 32);
            }
            return fullname;
        }

        public long sysPtrelate { get; set; }

    }
}