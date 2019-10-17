using System;

namespace Cic.One.DTO
{
    public class MAttendeeDto : MEmailAddressDto
    {
        /// <summary>
        /// Letzte Antwortzeit
        /// </summary>
        public DateTime? LastResponseTime { get; set; }

        /// <summary>
        /// Art der Antwort
        /// </summary>
        public MMeetingResponseTypeEnum ResponseType { get; set; }
    }
}