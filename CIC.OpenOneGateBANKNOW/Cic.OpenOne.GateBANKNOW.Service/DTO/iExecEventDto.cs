using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für ExecEvent Methode
    /// </summary>
    public class iExecEventDto
    {
        /// <summary>
        /// area id (optional)
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// area like VT,ANTRAG,ANGEBOT (optional)
        /// </summary>
        public String area { get; set; }

        /// <summary>
        /// EAIART Code
        /// </summary>
        public String code { get; set; }

        /// <summary>
        /// additional parameter for the code (e.g. Vorgangnr)
        /// </summary>
        public String eventCode1 { get; set; }
        /// <summary>
        /// additional parameter for the code (e.g. documenttype)
        /// </summary>
        public String eventCode2 { get; set; }
        /// <summary>
        /// additional parameter for the code 
        /// </summary>
        public String eventCode3 { get; set; }
    }
}
