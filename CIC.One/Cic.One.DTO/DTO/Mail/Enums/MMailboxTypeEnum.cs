using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    // Summary:
    //     Defines the type of an EmailAddress object.
    public enum MMailboxTypeEnum
    {
        // Summary:
        //     Unknown mailbox type (Exchange 2010 or later).
        //[RequiredServerVersion(ExchangeVersion.Exchange2010)]
        Unknown = 0,

        //
        // Summary:
        //     The EmailAddress represents a one-off contact (Exchange 2010 or later).
        //[RequiredServerVersion(ExchangeVersion.Exchange2010)]
        OneOff = 1,

        //
        // Summary:
        //     The EmailAddress represents a mailbox.
        Mailbox = 2,

        //
        // Summary:
        //     The EmailAddress represents a public folder.
        //[RequiredServerVersion(ExchangeVersion.Exchange2007_SP1)]
        PublicFolder = 3,

        //
        // Summary:
        //     The EmailAddress represents a Public Group.
        //[EwsEnum("PublicDL")]
        PublicGroup = 4,

        //
        // Summary:
        //     The EmailAddress represents a Contact Group.
        //[EwsEnum("PrivateDL")]
        ContactGroup = 5,

        //
        // Summary:
        //     The EmailAddress represents a store contact or AD mail contact.
        Contact = 6,
    }
}
