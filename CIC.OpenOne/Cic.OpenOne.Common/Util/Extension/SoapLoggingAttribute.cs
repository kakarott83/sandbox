using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace Cic.OpenOne.Common.Util.Extension
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SoapLoggingAttribute:Attribute
    {
     
        /// <summary>
        /// Disables the logging for this method
        /// </summary>
        public bool disable { get;set;}

      
    }
}
