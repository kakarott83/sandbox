using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class SanityCheckInfoDto
    {
        public String status { get; set; }
        public String setting { get; set; }
        public String description { get; set; }
        public String configlocation { get; set; }
        public String fix { get; set; }
        public String error { get; set; }
        public bool ok { get; set; }
        /// <summary>
        /// Constructor for Module Status
        /// </summary>
        /// <param name="description"></param>
        /// <param name="setting"></param>
        /// <param name="configlocation"></param>
        /// <param name="fix"></param>
        /// <param name="ok"></param>
        public SanityCheckInfoDto(String description, String status,String fix, bool ok)
        {
            this.description = description;
            this.status = status;
            this.fix = fix;
            this.ok = ok;
        }
        /// <summary>
        /// Constructor for config setting
        /// </summary>
        /// <param name="description"></param>
        /// <param name="setting"></param>
        /// <param name="configlocation"></param>
        /// <param name="fix"></param>
        /// <param name="ok"></param>
        public SanityCheckInfoDto(String description, String setting, String configlocation, String fix, bool ok)
        {
            this.description = description;
            this.setting = setting;
            this.configlocation = configlocation;
            this.fix = fix;
            this.ok = ok;
        }
        /// <summary>
        /// Constructor for Exception
        /// </summary>
        /// <param name="description"></param>
        /// <param name="error"></param>
        /// <param name="fix"></param>
        public SanityCheckInfoDto(String description, String error, String fix)
        {
            this.description = description;
            this.error = error;
            this.fix = fix;
            this.ok = false;
        }

        public override string ToString()
        {
            String rval = null;
            if(error!=null)
            {
                return description +" failed with "+error+" - "+fix+" - FAILURE";

            }
            else if (status != null)
            {
                rval = description + ": " + status;

            }
            else
            {
              rval = description + " is set to '" + setting + "' (in " + configlocation + ") ";
            }
            if (ok)
                rval += " - OK";
            else rval += " - " + fix + " - WARNING";
            return rval;
        }
    }
}
