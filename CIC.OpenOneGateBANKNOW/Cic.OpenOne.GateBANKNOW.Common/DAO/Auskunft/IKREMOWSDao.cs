using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKREMOWSDao
    {
        /// <summary>
        /// interface method to call KREMOWebService CallKREMObyValues()
        /// </summary>
        /// <param name="in_Value">Input Value</param>
        /// <param name="out_Value">Output Value</param>
        /// <returns>Value</returns>
        long CallKremoByValues(ref KREMORef.ArrayOfDouble in_Value, ref KREMORef.ArrayOfDouble out_Value);
        
        /// <summary>
        /// interface method to call KREMOWebService CallKREMOgetVersion()
        /// </summary>
        /// <returns>Version</returns>
        string CallKremoGetVersion();

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);

    }
}
