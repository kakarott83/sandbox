using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
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

        /// <summary>
        /// user
        /// </summary>
        public long user
        {
            get;
            set;
        }

        /// <summary>
        /// isocode
        /// </summary>
        public string isocode
        {
            get;
            set;
        }

    }
}
