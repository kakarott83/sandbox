using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// Aktivität
    /// </summary>
    public class OppotaskDto : EntityDto
    {
        //select crt.name||' '||crt.vorname crtuserName,wf.name||' '||wf.vorname wfuserName, vt.vertrag gebietName, kd.name||' '||kd.vorname kdName,vk.name||' '||vk.vorname vkName, hd.name||' '||hd.vorname hdName,hd.ort hdOrt from vt, person kd, person vk, person hd, activity, wfuser wf, wfuser crt where kd.sysperson=vt.syskd and vk.sysperson=vt.sysberatadda and hd.sysperson=vt.sysvpfil and wf.syswfuser=activity.syswfuser and crt.syswfuser=activity.syscrtuser and vt.sysid=activity.sysid and activity.area='VT';

        /// <summary>
        /// BMW-AIDA2 relevante Zusatzinformationen
        /// </summary>
        public String wfuserName {get;set;}
        public String crtuserName { get; set; }
        public String kdName { get; set; }
        public String gebietName { get; set; }
        public String vkName { get; set; }
        public String hdName { get; set; }
        public String hdOrt { get; set; }
        public String angebot { get; set; }
        public String sa3angebot { get; set; }
        public long sysangebot { get; set; }

        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysOppotask { get; set; }
        /// <summary>
        /// Fremdschlüssel auf Interaktionsmuster
        /// </summary>
        public long sysIamOppotask { get; set; }
        /// <summary>
        /// Verantwortlicher
        /// </summary>
        public long syswfuser { get; set; }
        /// <summary>
        /// Erzeugt von
        /// </summary>
        public long syscrtuser { get; set; }
        /// <summary>
        /// Fremdschlüssel Opportunity
        /// </summary>
        public long sysoppo { get; set; }
        /// <summary>
        /// Art der Aktivität
        /// </summary>
        public long sysOppotaskType { get; set; }
        /// <summary>
        /// Gebiets-ID
        /// </summary>
        public long sysid { get; set; }
        
        /// <summary>
        /// Kommentar Händler
        /// </summary>
        public String notiz1 { get; set; }
        /// <summary>
        /// Kommentar Bankmitarbeiter
        /// </summary>
        public String notiz2 { get; set; }
        /// <summary>
        /// Ergebnis Kunde
        /// </summary>
        public String resultat { get; set; }
        /// <summary>
        /// Gebiet
        /// </summary>
        public String area { get; set; }
      
        /// <summary>
        /// Art der Aktivität
        /// DDLKPPOS<->ACTIVITYTYPE
        /// </summary>
        public int art { get; set; }
        /// <summary>
        /// Phase grün gelb rot
        /// </summary>
        public int phase { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Geändert Flag
        /// </summary>
        public int changedflag { get; set; }
        /// <summary>
        /// Erstellzeitpunkt
        /// </summary>
        public DateTime crtdate { get; set; }
        /// <summary>
        /// Fälligkeitsdatum
        /// </summary>
        public DateTime duedate { get; set; }

        /// <summary>
        /// 1 wenn syschguser ein interner Mitarbeiter ist
        /// </summary>
        public int ownershipsf { get; set; }

        /// <summary>
        /// Art der zugehörigen Oppo
        /// </summary>
        public String oppoiamcode { get; set; }

        override public long getEntityId()
        {
            return sysOppotask;
        }
        public override string getEntityBezeichnung()
        {
            return "A" + sysOppotask;
        }

    }
}