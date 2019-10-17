using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Schnitstelle des Eaipar DAO
    /// </summary>
    public interface IEaiparDao
    {

        /// <summary>
        /// ParamFile des EAIPAR auslesen
        /// </summary>
        /// <param name="entryCode"></param>
        /// <param name="defaultValue"></param>
        /// <returns>ParamFile als String</returns>
        String getEaiParFileByCode(string entryCode, string defaultValue);
 
    }
   
}
