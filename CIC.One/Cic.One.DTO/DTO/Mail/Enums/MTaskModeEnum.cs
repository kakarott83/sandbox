using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MTaskModeEnum
    {
        // Summary:
        //     The task is normal
        Normal = 0,
        //
        // Summary:
        //     The task is a task assignment request
        Request = 1,
        //
        // Summary:
        //     The task assignment request was accepted
        RequestAccepted = 2,
        //
        // Summary:
        //     The task assignment request was declined
        RequestDeclined = 3,
        //
        // Summary:
        //     The task has been updated
        Update = 4,
        //
        // Summary:
        //     The task is self delegated
        SelfDelegated = 5,
    }
}
