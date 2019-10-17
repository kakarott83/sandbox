using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class CrmnmDto : EntityDto
    {
        /// <summary>
        /// typCode
        /// </summary>
        public string typCode { get; set; }

        /*Zusatzinformation */
        public String additionalInfo { get; set; }

        public long syscrmnm { get; set; }
        public long sysidparent { get; set; }
        public long sysidchild { get; set; }
        public String parentarea { get; set; }
        public String childarea { get; set; }
        public int activeFlag { get; set; }

    


        override public long getEntityId()
        {
            return syscrmnm;
        }


        //flag if relation to add or remove
        public int addToPerson { get; set; }
    }
}