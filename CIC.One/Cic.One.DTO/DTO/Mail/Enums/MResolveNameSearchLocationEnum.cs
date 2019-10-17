using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{

    public enum MResolveNameSearchLocationEnum
    {
        // Summary:
        //     The name is resolved against the Global Address List.
        DirectoryOnly = 0,
        //
        // Summary:
        //     The name is resolved against the Global Address List and then against the
        //     Contacts folder if no match was found.
        DirectoryThenContacts = 1,
        //
        // Summary:
        //     The name is resolved against the Contacts folder.
        ContactsOnly = 2,
        //
        // Summary:
        //     The name is resolved against the Contacts folder and then against the Global
        //     Address List if no match was found.
        ContactsThenDirectory = 3,
    }
}
