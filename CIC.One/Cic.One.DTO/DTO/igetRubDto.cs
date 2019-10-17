using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.DTO
{
   
    public class igetRubDto
    {
        /// <summary>
        /// The code of ddlkprub, may be null to deliver all rubs for the area
        /// </summary>
        public String rubcode
        {
            get;
            set;
        }
        public String rubarea
        {
            get;
            set;
        }

        /// <summary>
        /// The area, may be null
        /// </summary>
        public String area
        {
            get;
            set;
        }
        /// <summary>
        /// id of the area
        /// </summary>
        public long areaid
        {
            get;
            set;
        }
    }
}
