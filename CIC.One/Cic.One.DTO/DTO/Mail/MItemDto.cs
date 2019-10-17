using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Cic.One.DTO
{
    /// <summary>
    /// Basisklasse für alle Items, es müssen alle davon abgeleitete Klassen angegeben werden.
    /// </summary>
    [KnownType(typeof(MAppointmentDto))]
    [KnownType(typeof(MContactDto))]
    [KnownType(typeof(MEmailMessageDto))]
    [KnownType(typeof(MContactGroupDto))]
    [KnownType(typeof(MTaskDto))]
    public class MItemDto
    {
        //Microsoft.Exchange.WebServices.Data.Item
        /// <summary>
        /// Enthält die Kategorien
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Enthält den Titel des Items
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Enthält die eindeutige Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Enthält den Körper des Items
        /// </summary>
        public MMessageBodyDto Body { get; set; }

        /// <summary>
        /// Wie wichtig das Item ist (Priority)
        /// </summary>
        public MImportanceEnum? Importance { get; set; }

        /// <summary>
        /// Sensitivität
        /// </summary>
        public MSensitivityEnum? Sensitivity { get; set; }

        /// <summary>
        /// Datetime gesendet
        /// TODO: hat nur getter
        /// </summary>
        public DateTime? DateTimeSent { get; set; }

        /// <summary>
        /// Datetime empfangen
        /// TODO: hat nur getter
        /// </summary>
        public DateTime? DateTimeReceived { get; set; }

        //
        // Summary:
        //     Gets a list of the attachments to this item.
        public List<MFileAttachement> Attachments { get; set; }

        /// <summary>
        /// Enthält den Owner an welchen es gebunden wird.
        /// </summary>
        public long SysOwner { get; set; }
    }
}