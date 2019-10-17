namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    using System.Collections.Generic;

    public class osearchKundeExternNonGenericDto
    {
        /// <summary>
        /// addressMatchResultType
        /// </summary>
        public string ergebnis { get; set; }
        
        /// <summary>
        /// nameHint
        /// </summary>
        public string namehint { get; set; }

        /// <summary>
        /// CRIF locationIdentification.locationIdentificationType
        /// </summary>
        public string adresstyp { get; set; }

        /// <summary>
        /// CRIF locationIdentification.houseType
        /// </summary>
        public string haustyp { get; set; }

        /// <summary>
        /// character
        /// </summary>
        public string character { get; set; }

        /// <summary>
        /// Liste mit extern gefundenen Personen
        /// </summary>
        public List<KundeExternResultDto> result { get; set; }
    }
}