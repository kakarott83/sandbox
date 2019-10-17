namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Address Description Data Transfer Object
    /// </summary>
    public class ZekAddressDescriptionDto
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
        /// Getter/Setter Company Addition
        /// </summary>
        public string FirmaZusatz { get; set; }

        /// <summary>
        /// Getter/Setter First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Getter/Setter Founding date
        /// </summary>
        public string FoundingDate { get; set; }

        /// <summary>
        /// Getter/Setter House number
        /// </summary>
        public string Housenumber { get; set; }

        /// <summary>
        /// Getter/Setter Customer ID
        /// </summary>
        public string KundenId { get; set; }

        /// <summary>
        /// Getter/Setter Legal Form
        /// </summary>
        public int LegalForm { get; set; }

        /// <summary>
        /// Getter/Setter Maiden Name
        /// </summary>
        public string MaidenName { get; set; }

        /// <summary>
        /// Getter/Setter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Getter/Setter Nationality
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Getter/Setter Profession
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// Getter/Setter Sex
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// Getter/Setter Zip
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Getter/Setter Zip Addition
        /// </summary>
        public string ZipAdd { get; set; }

        /// <summary>
        /// Getter/Setter Civil state Code
        /// </summary>
        public int Zivilstandscode { get; set; }

        /// <summary>
        /// Getter/Setter Noga Code
        /// </summary>
        public string NogaCode { get; set; }

        /// <summary>
        /// Getter/Setter Living since
        /// </summary>
        public string DatumWohnhaftSeit { get; set; }

        /// <summary>
        /// Getter/Setter Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Getter/Setter UIDNummer 
        /// Unternehmen ID: Diese neue UID-Nummer ersetzt die alte sechsstellige MWST-Nummer.
        /// </summary>
        public string UIDNummer { get; set; }
    }
}
