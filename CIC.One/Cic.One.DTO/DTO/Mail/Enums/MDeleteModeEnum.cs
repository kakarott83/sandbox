using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MDeleteModeEnum
    {
        // Summary:
        //     The item or folder will be permanently deleted.
        HardDelete = 0,
        //
        // Summary:
        //     The item or folder will be moved to the dumpster. Items and folders in the
        //     dumpster can be recovered.
        SoftDelete = 1,
        //
        // Summary:
        //     The item or folder will be moved to the mailbox' Deleted Items folder.
        MoveToDeletedItems = 2,
    }
}
