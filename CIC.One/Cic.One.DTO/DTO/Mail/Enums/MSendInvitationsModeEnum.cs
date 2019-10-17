using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MSendInvitationsModeEnum
    {
        // Summary:
        //     No meeting invitation is sent.
        SendToNone = 0,
        //
        // Summary:
        //     Meeting invitations are sent to all attendees.
        SendOnlyToAll = 1,
        //
        // Summary:
        //     Meeting invitations are sent to all attendees and a copy of the invitation
        //     message is saved.
        SendToAllAndSaveCopy = 2,
    }
}
