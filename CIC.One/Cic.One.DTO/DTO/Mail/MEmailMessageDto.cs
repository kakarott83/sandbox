using System.Collections.Generic;
using Cic.One.DTO;


namespace Cic.One.DTO
{
    public class MEmailMessageDto : MItemDto
    {
        /// <summary>
        /// Sender der Mail
        /// </summary>
        public MEmailAddressDto From { get; set; }

        /// <summary>
        /// Die Empfänger
        /// </summary>
        public List<MEmailAddressDto> ToRecipients { get; set; }

        /// <summary>
        /// Bcc
        /// </summary>
        public List<MEmailAddressDto> BccRecipients { get; set; }

        /// <summary>
        /// Cc
        /// </summary>
        public List<MEmailAddressDto> CcRecipients { get; set; }

        /// <summary>
        /// Ob die Email gelesen wurde
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Ablieferung Bestätigung Angefordert
        /// </summary>
        public bool? IsDeliveryReceiptRequested { get; set; }

        /// <summary>
        /// Gelesen Bestätigung Angefordert
        /// </summary>
        public bool? IsReadReceiptRequested { get; set; }

        /// <summary>
        /// Antwort angefordert
        /// </summary>
        public bool? IsResponseRequested { get; set; }

    }
}