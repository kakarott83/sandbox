using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    /// <summary>
    /// Datastructure for combined list of WFMMEMO, BPCASESTEP,REMINDER,OFFERWF
    /// select syswfmmemo historyid,createdate datum, createtime zeit,wfuser.name||' '||wfuser.vorname benutzer,0 as art, '' casestep, '' caseid, kurzbeschreibung titel,syswfmmkat kategorie, wftable.syscode area, syslease sysid  from wfmmemo,wfuser,wftable where wftable.syswftable=wfmmemo.syswfmtable and wfuser.syswfuser=wfmmemo.createuser and syswfmmkat in (94,5,4,103);
    /// select sysbpcasestep historyid,stepdate datum, steptime zeit,wfuser.name||' '||wfuser.vorname benutzer, 1 as art,bpcasedef.namebpcasedef casestep,bpcasestep.sysbpcase caseid, steptext titel, 0 kategorie, oltable area, sysoltable sysid  from bpcasestep,wfuser,bpcase,bpcasedef where wfuser.syswfuser=stepbyuser and bpcase.sysbpcase=bpcasestep.sysbpcase and bpcasedef.sysbpcasedef=bpcase.sysbpcasedef;
    /// select sysreminder historyid, duedate datum, duetime zeit, wfuser.name||' '||wfuser.vorname benutzer, 2 as art,'' casestep,0 caseid, description titel, 0 kategorie, area, sysid from reminder,wfuser where wfuser.syswfuser=reminder.syswfuser;
    /// select sysstatealrt historyid, alertdate datum, alerttime zeit, info02 benutzer, 3 as art, '' casestep, 0 caseid, zustand titel, 0 kategorie, area, syslease sysid from STATEALRT;
    /// 
    /// 
    /*
     * create or replace view VC_HISTORY as 
        select 10000000000+syswfmmemo historyid,createdate datum, createtime zeit,wfuser.name||' '||wfuser.vorname benutzer,0 as art, '' casestep, 0 caseid, kurzbeschreibung titel,syswfmmkat kategorie, wftable.syscode area, syslease sysid  from wfmmemo,wfuser,wftable where wftable.syswftable=wfmmemo.syswfmtable and wfuser.syswfuser=wfmmemo.createuser and syswfmmkat in (94,5,4,103)
    union all select  20000000000+sysbpcasestep historyid,stepdate datum, steptime zeit,wfuser.name||' '||wfuser.vorname benutzer, 1 as art,bpcasedef.namebpcasedef casestep,bpcasestep.sysbpcase caseid, steptext titel, 0 kategorie, oltable area, sysoltable sysid  from bpcasestep,wfuser,bpcase,bpcasedef where wfuser.syswfuser=stepbyuser and bpcase.sysbpcase=bpcasestep.sysbpcase and bpcasedef.sysbpcasedef=bpcase.sysbpcasedef
    union all select  30000000000+sysreminder historyid, duedate datum, duetime zeit, wfuser.name||' '||wfuser.vorname benutzer, 2 as art,'' casestep,0 caseid, description titel, 0 kategorie, area, sysid from reminder,wfuser where wfuser.syswfuser=reminder.syswfuser
    union all select  40000000000+sysstatealrt historyid, alertdate datum, alerttime zeit, info02 benutzer, 3 as art, '' casestep, 0 caseid, zustand titel, 0 kategorie, area, syslease sysid from STATEALRT;
    */

	/// rh 20170103: actual statement with sorting
	/// SELECT 'MEMO' historyarea, syswfmmemo historyid, createdate datum, createtime zeit, wfuser.name||' '||wfuser.vorname benutzer, 0 AS art, '' casestep, 0 caseid, kurzbeschreibung titel, 
	///  	syswfmmkat kategorie, wftable.syscode area, syslease sysid  
	///    FROM cic.wfmmemo, cic.wfuser, cic.wftable 
	///    WHERE wftable.syswftable = wfmmemo.syswfmtable 
	///  	AND wfuser.syswfuser = wfmmemo.createuser 
	///  	AND syswfmmkat IN (  
	///  	  SELECT cfgvar.wert FROM cic.cfgvar, cic.cfgsec, cic.cfg 
	///  		WHERE cfgsec.code = 'MEMO__KAT_ANZEIGE' and cfg.code = 'ANTRAGSASSISTENT' AND cfgsec.syscfg = cfg.syscfg AND cfgvar.syscfgsec = cfgsec.syscfgsec )
	///  UNION ALL SELECT 'TIMESTAMP' historyarea, bpcasestep.sysbpcasestep historyid, bpcasestep.stepdate datum, bpcasestep.steptime zeit, wfuser.name||' '||wfuser.vorname benutzer, 1 AS art, bpcasedef.namebpcasedef casestep, 
	///    bpcasestep.sysbpcase caseid, bpcasestep.steptext titel, 0 kategorie, bpcase.oltable area, bpcase.sysoltable sysid 
	///    FROM cic.bpcasestep, cic.wfuser, cic.bpcase, cic.bpcasedef 
	///    WHERE wfuser.syswfuser=stepbyuser AND bpcase.sysbpcase=bpcasestep.sysbpcase AND bpcasedef.sysbpcasedef = bpcase.sysbpcasedef
	///  UNION ALL SELECT 'REMINDER' historyarea, sysreminder historyid, duedate datum, duetime zeit, wfuser.name||' '||wfuser.vorname benutzer, 2 AS art,'' casestep, 0 caseid, description titel, 0 kategorie, area, sysid 
	///    FROM cic.reminder, cic.wfuser 
	///    WHERE wfuser.syswfuser = reminder.syswfuser
	///  UNION ALL SELECT 'INFO' historyarea, sysstatealrt historyid, alertdate datum, alerttime zeit, info02 benutzer, 3 AS art, '' casestep, 0 caseid, zustand titel, 0 kategorie, area, syslease sysid 
	///    FROM cic.STATEALRT
	///  ORDER BY datum DESC, zeit DESC;
	  


    /// </summary>
    public class HistoryDto : EntityDto
    {
        
        public DateTime? datum { get; set; }
        public long zeit { get; set; }
        public String benutzer { get; set; }
        public String historyarea { get; set; }
        /// <summary>
        /// ID for art
        /// 0=WFMMEMO -> syswfmmemo
        /// 1=BPCASESTEP -> sysbpcasestep
        /// 2=REMINDER -> sysreminder
        /// 3=STATEALRT -> sysstatealrt
        /// </summary>
        public int art { get; set; }
        public long historyid { get; set; }
        public String casestep { get; set; }
        public String caseid { get; set; }
        public String titel { get; set; }
        public int kategorie { get; set; }
        public String processdefcode { get; set; }
        public String description { get; set; }
        public String area { get; set; }
        public long sysid { get; set; }

        /// <summary>
        /// Zeit als DateTime
        /// </summary>
        public DateTime? zeitGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)zeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    zeit = (long)val.Value;
                else
                    zeit = 0;
            }
        }

        override public long getEntityId()
        {
            return historyid;
        }

    }
}