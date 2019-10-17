using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI
{
    /// <summary>
    /// Attribute filter class
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class AttributeFilter: System.Attribute
    {
        /// <summary>
        /// Filter Statement member
        /// </summary>
        public readonly string Filter;

        /// <summary>
        /// Attribute filter operation
        /// </summary>
        /// <param name="filter">Filter statement</param>
        public AttributeFilter(string filter)  //
        {
            this.Filter = filter;
        }


    }

}