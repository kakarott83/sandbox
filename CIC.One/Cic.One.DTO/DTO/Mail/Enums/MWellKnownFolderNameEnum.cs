using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    // Summary:
    //     Defines well known folder names.
    public enum MWellKnownFolderNameEnum : int
    {
        // Summary:
        //     The Calendar folder.
        Calendar = 0,

        //
        // Summary:
        //     The Contacts folder.
        Contacts = 1,

        //
        // Summary:
        //     The Deleted Items folder
        DeletedItems = 2,

        //
        // Summary:
        //     The Drafts folder.
        Drafts = 3,

        //
        // Summary:
        //     The Inbox folder.
        Inbox = 4,

        //
        // Summary:
        //     The Journal folder.
        Journal = 5,

        //
        // Summary:
        //     The Notes folder.
        Notes = 6,

        //
        // Summary:
        //     The Outbox folder.
        Outbox = 7,

        //
        // Summary:
        //     The Sent Items folder.
        SentItems = 8,

        //
        // Summary:
        //     The Tasks folder.
        Tasks = 9,

        //
        // Summary:
        //     The message folder root.
        MsgFolderRoot = 10,

        //
        // Summary:
        //     The root of the mailbox.
        Root = 12,

        //
        // Summary:
        //     The Junk E-mail folder.
        JunkEmail = 13,

        //
        // Summary:
        //     The Search Folders folder, also known as the Finder folder.
        SearchFolders = 14,

        //
        // Summary:
        //     The Voicemail folder.
        VoiceMail = 15,
    }
}
