using System;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstract Nummernkreis-Builder
    /// </summary>
    public abstract class AbstractNkBuilder : INkBuilder
    {
        /// <summary>
        /// creates the next unique id 
        /// </summary>       
        /// <returns></returns>
        abstract public String getNextNumber();
    }
}