namespace Cic.One.DTO
{
    /// <summary>
    /// Enthält die Informationen zu einer EmailAddresse
    /// </summary>
    public class MEmailAddressDto
    {
        // Summary:
        //     Gets or sets the actual address associated with the e-mail address. The type
        //     of the Address property must match the specified routing type. If RoutingType
        //     is not set, Address is assumed to be an SMTP address.
        public string Address { get; set; }

        //
        // Summary:
        //     Gets or sets the Id of the contact the e-mail address represents. When Id
        //     is specified, Address should be set to null.
        public string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the type of the e-mail address.
        public MMailboxTypeEnum? MailboxType { get; set; }

        //
        // Summary:
        //     Gets or sets the name associated with the e-mail address.
        public string Name { get; set; }

        //
        // Summary:
        //     Gets or sets the routing type associated with the e-mail address. If RoutingType
        //     is not set, Address is assumed to be an SMTP address.
        public string RoutingType { get; set; }
    }
}