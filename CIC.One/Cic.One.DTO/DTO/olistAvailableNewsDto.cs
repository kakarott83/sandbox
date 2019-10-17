using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// OutputParameter for listAvailableNews
    /// </summary>
    public class olistAvailableNewsDto : oBaseDto
    {
        /// <summary>
        /// Array von News
        /// </summary>
        public AvailableNewsDto[] news
        {
            get;
            set;
        }
    }
}
