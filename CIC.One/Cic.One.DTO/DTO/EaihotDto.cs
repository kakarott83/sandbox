using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// EaihotDto
    /// </summary>
    public class EaihotDto : EntityDto 
    {

        /// <summary>
        /// primary key
        /// </summary>
        public long syseaihot { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public String code { get; set; }

        /// <summary>
        /// oltable
        /// </summary>
        public String oltable { get; set; }

        /// <summary>
        /// sysoltable
        /// </summary>
        public long? sysoltable { get; set; }

        /// <summary>
        /// prozessstatus
        /// </summary>
        public int? prozessstatus { get; set; }

        /// <summary>
        /// sysportal
        /// </summary>
        public long? sysportal { get; set; }

        /// <summary>
        /// syseaiart
        /// </summary>
        public long? syseaiart { get; set; }

        /// <summary>
        /// fileflagin
        /// </summary>
        public int? fileflagin { get; set; }

        /// <summary>
        /// fileflagout
        /// </summary>
        public int? fileflagout { get; set; }

        /// <summary>
        /// execpriority
        /// </summary>
        public int? execpriority { get; set; }

        /// <summary>
        /// evalexpression
        /// </summary>
        public String evalexpression { get; set; }

        /// <summary>
        /// inputparameter1
        /// </summary>
        public String inputparameter1 { get; set; }

        /// <summary>
        /// inputparameter2
        /// </summary>
        public String inputparameter2 { get; set; }

        /// <summary>
        /// inputparameter3
        /// </summary>
        public String inputparameter3 { get; set; }

        /// <summary>
        /// inputparameter4
        /// </summary>
        public String inputparameter4 { get; set; }

        /// <summary>
        /// inputparameter5
        /// </summary>
        public String inputparameter5 { get; set; }

        /// <summary>
        /// outputparameter1
        /// </summary>
        public String outputparameter1 { get; set; }

        /// <summary>
        /// outputparameter2
        /// </summary>
        public String outputparameter2 { get; set; }

        /// <summary>
        /// outputparameter3
        /// </summary>
        public String outputparameter3 { get; set; }

        /// <summary>
        /// outputparameter4
        /// </summary>
        public String outputparameter4 { get; set; }

        /// <summary>
        /// outputparameter5
        /// </summary>
        public String outputparameter5 { get; set; }

        /// <summary>
        /// submitdate
        /// </summary>
        public int? submitdate { get; set; }

        /// <summary>
        /// submittime
        /// </summary>
        public long? submittime { get; set; }

        /// <summary>
        /// finishdate
        /// </summary>
        public int? finishdate { get; set; }

        /// <summary>
        /// finishtime
        /// </summary>
        public long? finishtime { get; set; }

        /// <summary>
        /// syswfuser
        /// </summary>
        public long? syswfuser { get; set; }

        /// <summary>
        /// syswfexec
        /// </summary>
        public long? syswfexec { get; set; }

        /// <summary>
        /// wferror
        /// </summary>
        public long? wferror { get; set; }

        /// <summary>
        /// returnparameter
        /// </summary>
        public String returnparameter { get; set; }

        /// <summary>
        /// guilanguage
        /// </summary>
        public String guilanguage { get; set; }

        /// <summary>
        /// comlanguage
        /// </summary>
        public String comlanguage { get; set; }

        /// <summary>
        /// eve
        /// </summary>
        public int eve { get; set; }

        /// <summary>
        /// computername
        /// </summary>
        public String computername { get; set; }

        /// <summary>
        /// startdate
        /// </summary>
        public int? startdate { get; set; }

        /// <summary>
        /// starttime
        /// </summary>
        public long? starttime { get; set; }

        /// <summary>
        /// transactionflag
        /// </summary>
        public int? transactionflag { get; set; }

        /// <summary>
        /// hostcomputer
        /// </summary>
        public String hostcomputer { get; set; }

        /// <summary>
        /// All Files attached to the eaihot
        /// </summary>
        public List<EaihfileDto> files { get; set; }

        public int clientart { get; set; }

        public override long getEntityId()
        {
            return syseaihot;
        }
      
    }
}