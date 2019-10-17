using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI
{ 

    /// <summary>
    /// AttributeRequired
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class AttributeRequired: System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly bool Required;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="required"></param>
        public AttributeRequired(bool required)  //
        {
            this.Required = required;
        }


    }

}

