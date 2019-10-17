namespace Cic.One.DTO
{
    public class MItemChangeDto : MChangeDto
    {
        // Summary:
        //     Gets the IsRead property for the item that the change applies to. IsRead
        //     is only valid when ChangeType is equal to ChangeType.ReadFlagChange.
        public bool IsRead { get; set; }

        //
        // Summary:
        //     Gets the item the change applies to. Item is null when ChangeType is equal
        //     to either ChangeType.Delete or ChangeType.ReadFlagChange. In those cases,
        //     use the ItemId property to retrieve the Id of the item that was deleted or
        //     whose IsRead property changed.
        public MItemDto Item { get; set; }

        //
        // Summary:
        //     Gets the Id of the item the change applies to.
        public string ItemId { get; set; }

    }
}