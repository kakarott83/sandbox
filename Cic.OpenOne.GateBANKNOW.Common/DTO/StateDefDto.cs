using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// StateDefDto-Klasse
    /// </summary>
    public class StateDefDto
    {
        /// <summary>
        /// ALERTDATE
        /// </summary>
        public DateTime? ALERTDATE { get; set; }

        /// <summary>
        /// ALERTTIME
        /// </summary>
        public long? ALERTTIME { get; set; }

        /// <summary>
        /// AREA
        /// </summary>
        public string AREA { get; set; }

        /// <summary>
        /// INFO01
        /// </summary>
        public string INFO01 { get; set; }

        /// <summary>
        /// INFO02
        /// </summary>
        public string INFO02 { get; set; }

        /// <summary>
        /// INFO03
        /// </summary>
        public string INFO03 { get; set; }

        /// <summary>
        /// INFO04
        /// </summary>
        public string INFO04 { get; set; }

        /// <summary>
        /// INFO05
        /// </summary>
        public string INFO05 { get; set; }

        /// <summary>
        /// INFO06
        /// </summary>
        public string INFO06 { get; set; }

        /// <summary>
        /// READDATE
        /// </summary>
        public DateTime? READDATE { get; set; }

        /// <summary>
        /// READER
        /// </summary>
        public string READER { get; set; }

        /// <summary>
        /// READTIME
        /// </summary>
        public long? READTIME { get; set; }

        /// <summary>
        /// SYSLEASE
        /// </summary>
        public long? SYSLEASE { get; set; }

        /// <summary>
        /// SYSSTATEALRT
        /// </summary>
        public long SYSSTATEALRT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ZUSTAND { get; set; }

        /// <summary>
        /// ZUSTANDALT
        /// </summary>
        public string ZUSTANDALT { get; set; }

        /// <summary>
        /// ANTRAG
        /// </summary>
        public string ANTRAG { get; set; }
    }
}