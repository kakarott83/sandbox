using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output für die DocumentLoad-Methode
    /// </summary>
    public class oDocumentLoadDto : oBaseDto
    {
        /// <summary>
        /// Ergebnis-Dokument als byte[] kann auch eine Fehlermeldung als PDF sein.
        /// </summary>
        public byte[] Result { get; set; }
    }
}