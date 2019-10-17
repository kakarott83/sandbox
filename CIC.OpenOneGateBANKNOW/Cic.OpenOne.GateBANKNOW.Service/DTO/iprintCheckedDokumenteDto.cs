using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.printAngebotService.printCheckedDokumente"/> Methode
    /// </summary>
    public class iprintCheckedDokumenteDto
    {
        /// <summary>
        /// Allgemeines Dokumentenobjekt
        /// </summary>
        public DokumenteDto[] dokumente
        {
            get;
            set;
        }

        /// <summary>
        /// SYSID (Primary Key)
        /// </summary>
        public long sysid
        {
            get;
            set;
        }


        /// <summary>
        /// SYSID (Primary Key)
        /// </summary>
        public long? variantenid
        {
            get;
            set;
        }
        /// <summary>
        /// Boolean eCodeEintragung
        /// Der Webservice überträgt diesen Parameter in das Feld EAIHOT:INPUTPARAMETER2 des angelegten EAIHOT-Satzes.
        /// </summary>
        public bool eCodeEintragung
        {
            get;
            set;
        }
    }
}
