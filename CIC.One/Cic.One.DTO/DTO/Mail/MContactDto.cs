using System.Collections.Generic;

namespace Cic.One.DTO
{
    public class MContactDto : MItemDto
    {
        //Microsoft.Exchange.WebServices.Data.Contact

        /// <summary>
        /// Enthält den angezeigten Namen eines Kontaktes.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Enthält den gegebenen Namen eines Kontaktes
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Enthält die EmailAddressen eines Kontaktes
        /// </summary>
        public List<MEmailAddressDto> EmailAddresses { get; set; }

        //
        // Summary:
        //     Gets the source of the contact.
        public MContactSourceEnum? ContactSource { get; set; }

        //
        // Summary:
        //     Gets or sets the department of the contact.
        public string Department { get; set; }

        //
        // Summary:
        //     Gets or sets the surname of the contact.
        public string Surname { get; set; }

        //
        // Summary:
        //     Gets or sets the compnay name of the contact.
        public string CompanyName { get; set; }

        //
        // Summary:
        //     Gets or sets the initials of the contact.
        public string Initials { get; set; }

        //
        // Summary:
        //     Gets an indexed list of physical addresses for the contact. For example,
        //     to set the business address, use the following syntax: PhysicalAddresses[PhysicalAddressKey.Business]
        //     = new PhysicalAddressEntry()
        public Dictionary<string, MPhysicalAddress> PhysicalAddresses { get; set; }

        //
        // Summary:
        //     Gets an indexed list of phone numbers for the contact. For example, to set
        //     the home phone number, use the following syntax: PhoneNumbers[PhoneNumberKey.HomePhone]
        //     = "phone number"
        public Dictionary<string, string> PhoneNumbers { get; set; }
    }
}