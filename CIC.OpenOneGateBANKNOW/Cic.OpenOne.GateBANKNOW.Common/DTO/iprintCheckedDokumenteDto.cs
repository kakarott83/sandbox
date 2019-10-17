using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für printCheckedDokumente Methode
    /// </summary>
    public class iprintCheckedDokumenteDto
    {
        /// <summary>
        /// Allgemeines Dokumentenobjekt
        /// </summary>
        public DTO.DokumenteDto[] dokumente
        {
            get;
            set;
        }
    }
}
