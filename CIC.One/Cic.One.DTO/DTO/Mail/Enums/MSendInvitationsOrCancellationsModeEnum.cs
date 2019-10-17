using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MSendInvitationsOrCancellationsModeEnum
    {
        // Summary:
        //     No meeting invitation/cancellation is sent.
        SendToNone = 0,
        //
        // Summary:
        //     Meeting invitations/cancellations are sent to all attendees.
        SendOnlyToAll = 1,
        //
        // Summary:
        //     Meeting invitations/cancellations are sent only to attendees that have been
        //     added or modified.
        SendOnlyToChanged = 2,
        //
        // Summary:
        //     Meeting invitations/cancellations are sent to all attendees and a copy is
        //     saved in the organizer's Sent Items folder.
        SendToAllAndSaveCopy = 3,
        //
        // Summary:
        //     Meeting invitations/cancellations are sent only to attendees that have been
        //     added or modified and a copy is saved in the organizer's Sent Items folder.
        SendToChangedAndSaveCopy = 4,
    }
}
