using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
{
    
    interface INkBuilder
    {
        /// <summary>
        /// creates the next unique id 
        /// </summary>       
        /// <returns></returns>
        String getNextNumber();
    }
}
