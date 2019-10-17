using System.Collections.Generic;

namespace Cic.One.DTO
{
    /// <summary>
    /// Enthält alle Informationen zu einer Kontaktgruppe
    /// </summary>
    public class MContactGroupDto : MItemDto
    {
        // Summary:
        //     Gets or sets the display name of the contact group.
        public string DisplayName { get; set; }

        // Summary:
        //     Gets the number of members that were returned by the ExpandGroup operation.
        //     Count might be less than the total number of members in the group, in which
        //     case the value of the IncludesAllMembers is false.
        public int Count { get; set; }

        //
        // Summary:
        //     Gets a value indicating whether all the members of the group have been returned
        //     by ExpandGroup.
        public bool IncludesAllMembers { get; set; }

        /// <summary>
        /// Enthält die EmailAddressen von Personen
        /// </summary>
        public List<MEmailAddressDto> Members { get; set; }
    }
}