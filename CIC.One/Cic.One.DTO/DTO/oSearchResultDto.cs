using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Generic Base-Class for all Webservice Search-Results 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class oSearchResultDto<T> : oBaseDto
    {
        public oSearchDto<T> result
        {
            get;
            set;
        }
    }
}
