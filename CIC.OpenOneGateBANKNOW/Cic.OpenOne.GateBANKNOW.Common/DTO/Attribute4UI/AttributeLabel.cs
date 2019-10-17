using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI
{
    /// <summary>
    /// Attribute Label
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class AttributeLabel : System.Attribute
    {
        /// <summary>
        /// Label Attribut
        /// </summary>
        public readonly string Label;

        /// <summary>
        /// Set Attribute Property
        /// </summary>
        /// <param name="label">Label Descriptor</param>
        public AttributeLabel(string label)  //
        {
            this.Label = label;
        }


    }

}