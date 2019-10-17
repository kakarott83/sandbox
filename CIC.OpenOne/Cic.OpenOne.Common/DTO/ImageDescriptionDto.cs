using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Wird verwendet, damit der HtmlParser erkennt, dass er ein Bild als Base-64-Codierten string einsetzen muss.
    /// </summary>
    public class ImageDescriptionDto
    {
        /// <summary>
        /// Enthält die Daten von dem Bild
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Enthält den Base64 codierten String, falls einer vorhanden ist,
        /// ist nicht von außerhalb des Projekts zugänglich
        /// </summary>
        internal string ReplacementData { get; set; }
    }
}
