using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MConflictResolutionModeEnum
    {
        // Summary:
        //     Local property changes are discarded.
        NeverOverwrite = 0,
        //
        // Summary:
        //     Local property changes are applied to the server unless the server-side copy
        //     is more recent than the local copy.
        AutoResolve = 1,
        //
        // Summary:
        //     Local property changes overwrite server-side changes.
        AlwaysOverwrite = 2,
    }
}
