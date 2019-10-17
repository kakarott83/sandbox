using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class ZekOLFoundContractsDto
    {

        /// <summary>
        /// Getter/Setter Gesamt Engagement
        /// </summary>
        public int? GesamtEngagement { get; set; }

        /// <summary>
        /// Getter/Setter Amtsinformation Contracts
        /// </summary>
        public List<ZekOLAmtsinformationDescriptionDto> AmtsinformationContracts { get; set; }

        /// <summary>
        /// Getter/Setter Bardarlehen Contracts
        /// </summary>
        public List<ZekOLBardarlehenDescriptionDto> BardarlehenContracts { get; set; }

        /// <summary>
        /// Getter/Setter Festkredit Contracts
        /// </summary>
        public List<ZekOLFestkreditDescriptionDto> FestkreditContracts { get; set; }

        /// <summary>
        /// Getter/Setter Kartenengagement Contracts
        /// </summary>
        public List<ZekOLKartenengagementDescriptionDto> KartenengagementContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Karteninformations Contracts
        /// </summary>
        public List<ZekOLKarteninformationDescriptionDto> KarteninformationContracts { get; set; }

        /// <summary>
        ///  Getter/Setter  Kontokorrentkredit Contracts
        /// </summary>
        public List<ZekOLKontokorrentkreditDescriptionDto> KontokorrentkreditContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Kreditgesuch Contracts
        /// </summary>
        public List<ZekOLKreditgesuchDescriptionDto> KreditgesuchContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Leasing/Mietvertrag Contracts
        /// </summary>
        public List<ZekOLLeasingMietvertragDescriptionDto> LeasingMietvertragContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Solidarschuldner Contracts
        /// </summary>
        public List<ZekOLSolidarschuldnerDescriptionDto> SolidarschuldnerContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Teilzahlungskredit Contracts
        /// </summary>
        public List<ZekOLTeilzahlungskreditDescriptionDto> TeilzahlungskreditContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Teilzahlungsvertrag Contracts
        /// </summary>
        public List<ZekOLTeilzahlungsvertragDescriptionDto> TeilzahlungsvertragContracts { get; set; }

        /// <summary>
        ///  Getter/Setter Überziehungskredit Contracts
        /// </summary>
        public List<ZekOLUeberziehungskreditDescriptionDto> UeberziehnungskreditContracts { get; set; }

        /// <summary>
        /// Getter/Setter ECode178Contracts 
        /// </summary>
        public List<ZekOLeCode178DtoDescriptionDto> ECode178Contracts { get; set; }
    }
}
