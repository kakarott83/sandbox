using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// ZEK Found Contracts Data Transfer Object
    /// </summary>
    public class ZekFoundContractsDto
    {
        /// <summary>
        /// Getter/Setter Gesamt Engagement
        /// </summary>
        public int? GesamtEngagement { get; set; }

        /// <summary>
        /// Getter/Setter Amtsinformation Contracts
        /// </summary>
        public List<ZekAmtsinformationDescriptionDto> AmtsinformationContracts { get; set; }

        /// <summary>
        /// Getter/Setter Bardarlehen Contracts
        /// </summary>
        public List<ZekBardarlehenDescriptionDto> BardarlehenContracts { get; set; }

        /// <summary>
        /// Getter/Setter Festkredit Contracts
        /// </summary>
        public List<ZekFestkreditDescriptionDto> FestkreditContracts { get; set; }

        /// <summary>
        /// Getter/Setter Kartenengagement Contracts
        /// </summary>
        public List<ZekKartenengagementDescriptionDto> KartenengagementContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Karteninformations Contracts
        /// </summary>
        public List<ZekKarteninformationDescriptionDto> KarteninformationContracts { get; set; }

        /// <summary>
        ///  Getter/Setter  Kontokorrentkredit Contracts
        /// </summary>
        public List<ZekKontokorrentkreditDescriptionDto> KontokorrentkreditContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Kreditgesuch Contracts
        /// </summary>
        public List<ZekKreditgesuchDescriptionDto> KreditgesuchContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Leasing/Mietvertrag Contracts
        /// </summary>
        public List<ZekLeasingMietvertragDescriptionDto> LeasingMietvertragContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Solidarschuldner Contracts
        /// </summary>
        public List<ZekSolidarschuldnerDescriptionDto> SolidarschuldnerContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Teilzahlungskredit Contracts
        /// </summary>
        public List<ZekTeilzahlungskreditDescriptionDto> TeilzahlungskreditContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Teilzahlungsvertrag Contracts
        /// </summary>
        public List<ZekTeilzahlungsvertragDescriptionDto> TeilzahlungsvertragContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Überziehungskredit Contracts
        /// </summary>
        public List<ZekUeberziehungskreditDescriptionDto> UeberziehnungskreditContracts { get; set; }

        /// <summary>
        /// Getter/Setter Ecode
        /// </summary>
        public List<ZekeCode178Dto> ECode178Contracts { get; set; }
    }
}
