using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.printAngebotService.printCheckedDokumente"/> Methode
    /// </summary>
    public class oprintCheckedDokumenteDto : oBaseDto
    {
        /// <summary>
        /// Alle Dokumente, die gedruckt wurden, als bytearray
        /// </summary>
        public byte[] file
        {
            get;
            set;
        }
    }
}
