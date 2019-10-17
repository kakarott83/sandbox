namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Delta Vista Address Description Data Transfer Object
    /// </summary>
    public class DVAddressDescriptionDto
    {
        /// <summary>
        /// Getter/Setter Birthdate
        /// </summary>
        public string Birthdate { get; set; }

        /// <summary>
        /// Getter/Setter City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Getter/Setter Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Getter/Setter First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Getter/Setter House Number
        /// </summary>
        public string Housenumber { get; set; }

        /// <summary>
        /// Getter/Setter Legal Form
        /// </summary>
        public int LegalForm { get; set; }

        /// <summary>
        /// Getter/Setter Maiden name
        /// </summary>
        public string MaidenName { get; set; }

        /// <summary>
        /// Getter/Setter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Getter/Setter Sex
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// Getter/Setter Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Getter/Setter  ZIP
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Getter/Setter Telephone
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Getter/Setter Fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Getter/Setter Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Getter/Setter UIDNummer 
        /// Unternehmen ID: Diese neue UID-Nummer ersetzt die alte sechsstellige MWST-Nummer.
        /// </summary>
        public string UIDNummer { get; set; }
    }
}
