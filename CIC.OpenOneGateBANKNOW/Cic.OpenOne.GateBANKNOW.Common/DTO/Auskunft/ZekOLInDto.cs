using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class ZekOLInDto
    {

        /// <summary>
        /// "1 = Neugeschäft
        ///2 = Wiederholungsprüfung
        ///3 = Abklärung"
        /// </summary>
        public int Anfragegrund { get; set; }

        /// <summary>
        /// "0 = nicht IKO-pflichtig
        /// 1 = IKO-pflichtig (nur privat)"
        /// </summary>
        public int Zielverein { get; set; }


        public ZekRequestEntityDto RequestEntity { get; set; }
    
    }
}