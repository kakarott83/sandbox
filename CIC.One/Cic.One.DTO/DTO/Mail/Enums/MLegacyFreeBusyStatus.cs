using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public enum MLegacyFreeBusyStatus
    {
        // Summary:
        //     The time slot associated with the appointment appears as free.
        Free = 0,
        //
        // Summary:
        //     The time slot associated with the appointment appears as tentative.
        Tentative = 1,
        //
        // Summary:
        //     The time slot associated with the appointment appears as busy.
        Busy = 2,
        //
        // Summary:
        //     The time slot associated with the appointment appears as Out of Office.
        OOF = 3,
        //
        // Summary:
        //     The time slot associated with the appointment appears as working else where.
        WorkingElsewhere = 4,
        //
        // Summary:
        //     No free/busy status is associated with the appointment.
        NoData = 5,
    }
}