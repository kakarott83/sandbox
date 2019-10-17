using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt  getAuskunftS1 Methode
    /// </summary>
    public class S1AnswerInputData
    {
        /// <summary>
        /// SysID from AUSKUNFT
        /// </summary>
        public long SysAuskunft { get; set; }

        /// <summary>
        /// String with result from S1
        /// </summary>
        public string OutputString { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public long ErrorCode { get; set; }

        /// <summary>
        /// Error Text
        /// </summary>
        public string ErrorText { get; set; }
    }
}