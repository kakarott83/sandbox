using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    public class igetRubDto
    {
        /// <summary>
        /// The crmarea, may not be null
        /// </summary>
        public String crmarea
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
