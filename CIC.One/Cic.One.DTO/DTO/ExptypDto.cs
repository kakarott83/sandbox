using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Methods to recalc the Indicator
    /// </summary>
    public enum ExptypMethod
    {
        //manually triggerd by user
        MANUAL = 0,
        //every time accessed (when daysvalid exceeded)
        AUTO = 1,
        //other event updates the value (eg job)
        EXTERNAL = 2,
        //user enters the value
        USER=3
    }

    /// <summary>
    /// Indicator
    /// </summary>
    public class ExptypDto : EntityDto
    {
        public long sysexptyp { get; set; }
        public String area { get; set; }
        public long rang { get; set; }
        public String description { get; set; }

        /// <summary>
        /// Modes for calculation
        /// 
        /// </summary>
        public ExptypMethod methodType { get { return (ExptypMethod)method;  }  }

        public int method { get; set; }

        /// <summary>
        /// max age of calculated value
        /// </summary>
        public long daysvalid { get; set; }

        /// <summary>
        /// Calculation expression
        /// </summary>
        public String expression { get; set; }
        public double minval { get; set; }
        public double maxval { get; set; }
        public int aktivkz { get; set; }
        public int visibilityflag { get; set; }

        /// <summary>
        /// values will be appended to expvalar-table
        /// </summary>
        public int archivflag { get; set; }

        /// <summary>
        /// Default Indicator for the area (e.g. used to show in lists)
        /// </summary>
        public int areadefaultflag { get; set; }

        public List<ExprangeDto> ranges { get;set;}

        public override long getEntityId()
        {
            return sysexptyp;
        }
    }
}
