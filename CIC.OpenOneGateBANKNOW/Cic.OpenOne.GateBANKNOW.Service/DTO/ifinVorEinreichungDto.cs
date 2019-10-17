using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für Finanzierungsvorschlag Einreichung Methode
    /// </summary>
    public class ifinVorEinreichungDto
    {

        /// <summary>
        /// antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }


        /// <summary>
        /// gewählte Variante (1,2,3)
        /// </summary>
        public int finanzierungsVariante
        {
            get;
            set;
        }
    }
}