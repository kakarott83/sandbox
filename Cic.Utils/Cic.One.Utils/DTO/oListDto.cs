using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// OutputParameter for common droplist entries
    /// </summary>
    public class olistDto : oBaseDto
    {
        /// <summary>
        /// Array of entries
        /// </summary>
        public DropListDto[] entries
        {
            get;
            set;
        }
    }
}
