using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für processAntragToVertrag Methode
    /// </summary>
    [DataContract]
    public class iprocessAntragToVertragDto
    {
        /// <summary>
        /// Persistenzobjekt Antrag
        /// </summary>
        [DataMember]
        public DTO.AntragDto antrag
        {
            get;
            set;
        }
    }
}
