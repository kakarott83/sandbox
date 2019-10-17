using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Bank-Now Inbox Data
    /// </summary>
    public class InboxDto : EntityDto
    {
        public override long getEntityId()
        {
            return sysId;
        }
        override public String getEntityBezeichnung()
        {
            return prozess + " " + nummer + " " + kunde;
        }

        /// <summary>
        /// BPPROCDEF.DESCRIPTION
        /// </summary>
        public String description { get; set; }
        /// <summary>
        /// BP EVAL/EVALUATEDEF.DESCRIPTION
        /// </summary>
        public String stepdescription { get; set; }

        public String bezeichnung { get; set; }

        public long sysId { get; set; }
        public String oltable { get; set; }
        public long sysoltable { get; set; }
        public String processdefcode { get; set; }
        public String eventcode { get; set; }
        public String evaluatecode { get; set; }


        public String nummer { get; set; }
        public String prozess { get; set; }
        public String vertriebsweg { get; set; }
        public String vertragsart { get; set; }
        public String kunde { get; set; }
		public long sysIt { get; set; }
        public String ort { get; set; }
        public String haendler { get; set; }
        public String verkaeufer { get; set; }
        public String zustand { get; set; }
        public String attribut { get; set; }
        public String objektart { get; set; }
        public String objektbez { get; set; }
        public String fabrikat { get; set; }
        public double summe { get; set; }
        //DB Date only
        public DateTime? erfassung { get; set; }
        //DB CLA TIme only
        public long? erfassungzeit { get; set; }
        //Auto-combinded of above two fields
        public DateTime? erfassungsDatum
        {
            get
            {
                return DateTimeHelper.CreateDate(erfassung, erfassungzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erfassungzeit = (long)val.Value;
                else
                    erfassungzeit = 0;
                erfassung = value;
            }
        }
        /// <summary>
        /// Einreichungszeiten
        /// </summary>
        public DateTime? dateinreichung { get; set; }
        public long? dateinreichungzeit { get; set; }
        //Auto-combinded of above two fields
        public DateTime? einreichungsDatum
        {
            get
            {
                return DateTimeHelper.CreateDate(dateinreichung, dateinreichungzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    dateinreichungzeit = (long)val.Value;
                else
                    dateinreichungzeit = 0;
                dateinreichung = value;
            }
        }

        public DateTime? bearbeitung { get; set; }

        private DateTime? faellig_am;
        public DateTime? faellig
        {
            get { return faellig_am; }
            set
            {
                if (value == null || !value.HasValue) indicatorContent = "";
                else if (value.Value.CompareTo(DateTime.Now.AddDays(-1)) < 0)
                    indicatorContent = "red";
                else if (value.Value.CompareTo(DateTime.Now.AddDays(1)) < 0)
                    indicatorContent = "orange"; 
                else indicatorContent = "";
                faellig_am = value;
            }
        }
        public String berater { get; set; }
        public String bearbeiter { get; set; }
        public String abwicklungsort { get; set; }
        public String vp { get; set; }
        public String vk { get; set; }
        public String vip { get; set; }
        public String quelle { get; set; }
        public DateTime? auszahlungsdatum { get; set; }
        public String sprache { get; set; }
        public String personal { get; set; }
        public int lagerwagen { get; set; }
        public long syshaendler { get; set; }
        public int prio { get; set; }
		public int syscamp { get; set; }
		public int syscamptp { get; set; }					// rh 20170328: neu

        public String sla { get; set; }						// rh 20170404: neu
		public string slaindicator							// rh 20170404: neu
		{
			get { return sla; }
			set
			{
				string currentIndicator = indicatorContent;	// respect other indicators
				if (string.IsNullOrEmpty (value))
					indicatorContent = "";
				else if (value.Equals ("red"))
					indicatorContent = "red";
				else if (value.Equals ("orange") && !currentIndicator.Equals ("red"))
					indicatorContent = "orange";
				else
				{
					if (!(currentIndicator.Equals ("red") || currentIndicator.Equals ("orange")))
						indicatorContent = "";
				}
				sla = value;
			}
		}

		public int erfassungsclient { get; set; }
        /// <summary>
        /// PEOPTION.PEOPTION2
        /// </summary>
        public String segment { get; set; }
        /// <summary>
        /// PEOPTION.STR02
        /// </summary>
        public String kategorie { get; set; }

		/// <summary>
		/// GELESEN/UNGELESEN-FLAG 
		/// this accepts null, but if NOT 1 it is normalized to 0 
		/// </summary>
		public int? readFlag  
		{
			get 
			{
				if (preadFlag != 1)		// if NOT 1 normalize to 0 
					preadFlag = 0;
				return preadFlag; 
			}
			set
			{
				preadFlag = value;
				if (preadFlag != 1)		// if NOT 1 normalize to 0
					preadFlag = 0;
			}
		}
    }
}
