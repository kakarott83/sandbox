namespace Cic.One.DTO
{
    /// <summary>
    /// MNameResolutionDto
    /// </summary>
    public class MNameResolutionDto
    {
        // Summary:
        //     Gets the contact information of the suggested resolved name. This property
        //     is only available when ResolveName is called with returnContactDetails =
        //     true.
        public MContactDto Contact { get; set; }

        //
        // Summary:
        //     Gets the mailbox of the suggested resolved name.
        public MEmailAddressDto Mailbox { get; set; }
    }
}