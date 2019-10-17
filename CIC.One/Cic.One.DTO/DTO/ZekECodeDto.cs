using System;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ZekECodeDto : EntityDto
    {
        /// <summary>
        /// Getter/Setter Ecode178id
        /// </summary>
        public string Ecode178id { get; set; }

        /// <summary>
        /// Getter/Setter Fzstammnummer
        /// </summary>
        public string Fzstammnummer { get; set; }

        /// <summary>
        /// Getter/Setter Ecodestatus
        /// </summary>
        public string Ecodestatus { get; set; }

        /// <summary>
        /// Getter/Setter Haendlenummer
        /// </summary>
        public string Haendlenummer { get; set; }

        /// <summary>
        /// Getter/Setter Chassisnummer
        /// </summary>
        public string Chassisnummer { get; set; }

        /// <summary>
        /// Getter/Setter Datumgueltigbis
        /// </summary>
        public DateTime? Datumgueltigbis { get; set; } //Format: YYYY-MM-DD

        /// <summary>
        /// Getter/Setter Datumgueltigab
        /// </summary>
        public DateTime? Datumgueltigab { get; set; }  //Format: YYYY-MM-DD

        /// <summary>
        /// Getter/Setter stvaNummerField
        /// </summary>
        public string StvaNummer { get; set; } 


        public override long getEntityId()
        {
            try
            {
                return Convert.ToInt64(Ecode178id);
            }
            catch
            {
                return 0;
            }
        }
    }
}
