using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    // Summary:
    //     Defines the type of change of a synchronization event.
    public enum MChangeTypeEnum
    {
        // Summary:
        //     An item or folder was created.
        Create = 0,
        //
        // Summary:
        //     An item or folder was modified.
        Update = 1,
        //
        // Summary:
        //     An item or folder was deleted.
        Delete = 2,
        //
        // Summary:
        //     An item's IsRead flag was changed.
        ReadFlagChange = 3,
    }
}
