using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Mögliche Antworten auf Meetingeinladungen
    /// </summary>
    public enum MMeetingResponseTypeEnum
    {
        // Summary:
        //     The response type is inknown.
        Unknown = 0,

        //
        // Summary:
        //     There was no response. The authenticated is the organizer of the meeting.
        Organizer = 1,

        //
        // Summary:
        //     The meeting was tentatively accepted.
        Tentative = 2,

        //
        // Summary:
        //     The meeting was accepted.
        Accept = 3,

        //
        // Summary:
        //     The meeting was declined.
        Decline = 4,

        //
        // Summary:
        //     No response was received for the meeting.
        NoResponseReceived = 5,
    }
}
