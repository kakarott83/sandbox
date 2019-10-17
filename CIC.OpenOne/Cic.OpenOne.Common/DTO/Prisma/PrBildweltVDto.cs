using System;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// PrBildweltVDto-Klasse
    /// </summary>
    public class PrBildweltVDto
    {
        /// <summary>
        /// sysprbildwelt
        /// </summary>
        public long sysprbildwelt { get; set; }

        /// <summary>
        /// sysprbildweltv
        /// </summary>
        public long sysprbildweltv { get; set; }

        /// <summary>
        /// sysvart
        /// </summary>
        public long sysvart { get; set; }

        /// <summary>
        /// bezeichnung
        /// </summary>
        public String bezeichnung { get; set; }

        /// <summary>
        /// translation
        /// </summary>
        public TranslationDto translation { get; set; }

        /// <summary>
        /// code
        /// </summary>
        public String code { get; set; }
    }
}
