using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// 
    /// </summary>
     public enum ArtOfContract
     {
        amtsinformationContracts,
        bardarlehenContracts,
        festkreditContracts,
        gesamtEngagement,
        kartenengagementContracts,
        karteninformationContracts,
        kontokorrentkreditContracts,
        kreditGesuchContracts,
        leasingMietvertragContracts,
        solidarschuldnerContracts,
        teilzahlungskreditContracts,
        ueberziehungskreditContracts,
        eCode178DtoContracts
    }


        /// <summary>
        /// 
        /// </summary>
        public class ZekOLAmtsinformationDescriptionDto : ZekAmtsinformationDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ZekOLBardarlehenDescriptionDto : ZekBardarlehenDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        /// Festkredit Contract
        /// </summary>
        public class ZekOLFestkreditDescriptionDto : ZekFestkreditDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        /// Kartenengagement Contract
        /// </summary>
        public class ZekOLKartenengagementDescriptionDto : ZekKartenengagementDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }


        /// <summary>
        /// Karteninformations Contract
        /// </summary>
        public class ZekOLKarteninformationDescriptionDto : ZekKarteninformationDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }

     
        /// <summary>
        ///  Getter/Setter  Kontokorrentkredit Contracts
        /// </summary>
        public class ZekOLKontokorrentkreditDescriptionDto: ZekKontokorrentkreditDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Kreditgesuch Contracts
        /// </summary>
        public class ZekOLKreditgesuchDescriptionDto: ZekKreditgesuchDescriptionDto
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Leasing/Mietvertrag Contracts
        /// </summary>
        public class ZekOLLeasingMietvertragDescriptionDto: ZekLeasingMietvertragDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Solidarschuldner Contracts
        /// </summary>
        public class ZekOLSolidarschuldnerDescriptionDto: ZekSolidarschuldnerDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Teilzahlungskredit Contracts
        /// </summary>
        public class ZekOLTeilzahlungskreditDescriptionDto: ZekTeilzahlungskreditDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Teilzahlungsvertrag Contracts
        /// </summary>
        public class ZekOLTeilzahlungsvertragDescriptionDto: ZekTeilzahlungsvertragDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Überziehungskredit Contracts
        /// </summary>
        public class ZekOLUeberziehungskreditDescriptionDto: ZekUeberziehungskreditDescriptionDto 
        {
            public ArtOfContract artOfContract { get; set; }
        }

        /// <summary>
        ///  Getter/Setter Überziehungskredit Contracts
        /// </summary>
        public class ZekOLeCode178DtoDescriptionDto : ZekeCode178Dto
        {
            public ArtOfContract artOfContract { get; set; }
        }

       
}
