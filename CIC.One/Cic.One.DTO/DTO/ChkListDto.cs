using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.Mediator;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ChklistDateDto
    {
        public String art { get; set; }
        public DateTime receiveDate { get; set; }
        public DateTime? vtDate { get; set; }
        public long? vtclatime { get; set; }
    }
    public class ChklistDto : EntityDto
    {

        public long sysid { get;set;}
        public int salesFlag { get; set; }
        /// <summary>
        /// Vertrag erhalten EDATUM
        /// </summary>
        public DateTime? receiveDate { get; set; }
        /// <summary>
        /// Vertragsdatum UDATUM/ANTOPTION.ULON05
        /// </summary>
        public DateTime? vtDate { get; set; }

        /// <summary>
        /// Prüfung erfolgt Sales
        /// </summary>
        public int pruefung { get; set; }
        /// <summary>
        /// Geldfluss Flag Payment
        /// </summary>
        public int? geldfluss { get; set; }
        /// <summary>
        /// Schlusskontrolle Flag Payment
        /// </summary>
        public int? schlusskontrolle { get; set; }

        /// <summary>
        /// From antoption STR07
        /// </summary>
        public String art { get; set; }
        /// <summary>
        /// Allows writing to STR07
        /// </summary>
        public String artNew { get; set; }

        public List<ChklistEntryDto> rows { get; set; }
        override public long getEntityId()
        {
            return sysid;
        }

    }
  /*  
--checkboxes - write to
--ratingauflage syschguser, syschgdate, syschgtime, fullfilled
--prunstep: sales: flagok,syschguserok,chgdateok, chgtimeok
          --payments: flagnok, syschgusernok, chgdatenok, chgtimenok
--auflage - ratingauflage
--formalität - prunstep.flagok = 1 für alle, dann ratingauflage.fulfilled=1, flagnok
*/
    public class ChklistEntryDto 
    {
        public long sysprunsteppos { get; set; }
        public long sysprunstep { get; set; }
        public long sysratingauflage { get; set; }
        public String name { get; set; }
        public String pp { get; set; }
        public String code { get; set; }
        public String description { get; set; }
        public String deeplnk { get; set; }
        public String doctype { get; set; }
        public String pnlCmd { get; set; }
        public String psart { get; set; }
        //1 oder 2
        public int owner { get; set; }
         //--2=formalität 1=Auflag
        public int art { get; set; }
        public int flagok { get; set; }
        public int flagnok { get; set; }
        public int? flagnokstep { get; set; }
        public int fullfilled { get; set; }

        public DateTime? createDate
        {
            get
            {
                return DateTimeHelper.CreateDate(syscrtdate,syscrttime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    syscrttime = (long)val.Value;
                else
                    syscrttime = 0;
                syscrtdate = value;
            }
        }
        public String createUser { get; set; }
        public DateTime? finishDate {
            get
            {
                return DateTimeHelper.CreateDate(syschgdate, syschgtime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    syschgtime = (long)val.Value;
                else
                    syschgtime = 0;
                syschgdate = value;
            }
        }
        public String finishUser { get; set; }

        public long syschguser { get; set; }
        public long syscrtuser { get; set; }
        public DateTime? syschgdate { get; set; }
        public DateTime? syscrtdate { get; set; }
        public long? syschgtime { get; set; }
        public long? syscrttime { get; set; }
    }

}
