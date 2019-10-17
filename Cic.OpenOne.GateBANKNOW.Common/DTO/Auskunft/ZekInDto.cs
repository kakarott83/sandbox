using System.Collections.Generic;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Getter/Setter for ZekInDto
    /// </summary>
    public class ZekInDto
    {
        /// <summary>
        /// used for EC3
        /// </summary>
        [AttributeFilter("zekKreditgesuchAblehnen")]
        public int Ablehnungsgrund { get; set; }
        /// <summary>
        /// used for EC1, EC2
        /// </summary>
        [AttributeFilter("zekInformativabfrage,zekKreditgesuchNeu")]
        public int Anfragegrund { get; set; }
        /// <summary>
        /// used for EC3
        /// </summary>
        [AttributeFilter("zekKreditgesuchAblehnen")]
        public string DatumAblehnung { get; set; }
        /// <summary>
        /// used for EC3, EC4
        /// </summary>
        [AttributeFilter("zekKreditgesuchNeu,zekKreditgesuchAblehnen,zekRegisterBardarlehen,zekRegisterFestkredit,zekRegisterKontokorrent,zekRegisterLM,zekRegisterTZ," +
                         "zekMeldungKarten,zekmMeldungUeberziehungskredit,zekECode178Anmelden")]
        public string KreditgesuchID { get; set; }
        /// <summary>
        /// used for EC1
        /// </summary>
        [AttributeFilter("zekKreditgesuchNeu")]
        public string PreviousKreditgesuchID { get; set; }
        /// <summary>
        /// used for EC1, EC2, EC4
        /// </summary>
        [AttributeFilter("zekInformativabfrage,zekKreditgesuchNeu,zekRegisterBardarlehen,zekRegisterFestkredit,zekRegisterKontokorrent,zekRegisterLM,zekRegisterTZ," +
                         "zekMeldungKarten,zekmMeldungUeberziehungskredit")]
        public int Zielverein { get; set; }
        /// <summary>
        /// used for EC1, EC5, EC7
        /// </summary>        
        [AttributeFilter("zekKreditgesuchNeu,zekRegisterBardarlehen,zekRegisterFestkredit,zekRegisterKontokorrent,zekRegisterLM,zekRegisterTZ,zekMeldungKarten," +
                         "zekmMeldungUeberziehungskredit,zekUpdateBd,zekUpdateFk,zekUpdateKK,zekUpdateLM,zekUpdateTz,zekCloseBardarlehen,zekCloseLeasingMietvertrag," +
                         "zekCloseFestkredit,zekCloseTeilzahlungskredit,zekCloseKontokorrentkredit")]
        public List<ZekRequestEntityDto> RequestEntities { get; set; }
        /// <summary>
        /// used for EC2, EC3, EC4, EC6, EC7
        /// </summary>
        [AttributeFilter("zekInformativabfrage,zekKreditgesuchAblehnen,zekUpdateAd,zekECode178Anmelden")]
        public ZekRequestEntityDto RequestEntity { get; set; }
        /// <summary>
        /// used for EC6
        /// </summary>
        [AttributeFilter("zekUpdateAd")]
        public ZekRequestEntityDto RequestEntityNew { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekRegisterBardarlehen,zekUpdateBd,zekCloseBardarlehen")]
        public ZekBardarlehenDescriptionDto Bardarlehen { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateBd,zekUpdateBd")]
        public ZekBardarlehenDescriptionDto BardarlehenNew { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekRegisterFestkredit,zekUpdateFk,zekCloseFestkredit")]
        public ZekFestkreditDescriptionDto Festkredit { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateFk")]
        public ZekFestkreditDescriptionDto FestkreditNew { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekRegisterLM,zekUpdateLM,zekCloseLeasingMietvertrag")]
        public ZekLeasingMietvertragDescriptionDto LeasingMietvertrag { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateLM")]
        public ZekLeasingMietvertragDescriptionDto LeasingMietvertragNew { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekRegisterBardarlehen,zekUpdateTz,zekCloseTeilzahlungskredit")]
        public ZekTeilzahlungskreditDescriptionDto Teilzahlung { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekCloseTeilzahlungsvertrag")]
        public ZekTeilzahlungsvertragDescriptionDto Teilzahlungvertrag { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateTz")]
        public ZekTeilzahlungskreditDescriptionDto TeilzahlungNew { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateTz")]
        public ZekTeilzahlungsvertragDescriptionDto TeilzahlungvertragNew { get; set; }
        /// <summary>
        /// used for EC4, EC5, EC7
        /// </summary>
        [AttributeFilter("zekRegisterKontokorrent,zekUpdateKK,zekCloseKontokorrentkredit")]
        public ZekKontokorrentkreditDescriptionDto Kontokorrent { get; set; }
        /// <summary>
        /// used for EC7
        /// </summary>
        [AttributeFilter("zekUpdateKK")]
        public ZekKontokorrentkreditDescriptionDto KontokorrentNew { get; set; }
        /// <summary>
        /// used for EC4
        /// </summary>
        [AttributeFilter("zekMeldungKarten")]
        public ZekKartenengagementDescriptionDto Kartenengagement { get; set; }
        /// <summary>
        /// used for EC4
        /// </summary>
        [AttributeFilter("zekmMeldungUeberziehungskredit")]
        public ZekUeberziehungskreditDescriptionDto Ueberziehungskredit { get; set; }

        // ZEK Batch Webservices

        /// <summary>
        /// Massen-Vertragsabmeldung (EC5)
        /// Bei einer Vertragsauflösung muss eine Meldung an ZEK versendet werden.
        /// ZEKBatchServiceService.closeContractsBatch
        /// </summary>
        [AttributeFilter("zekCloseContractsBatch")]
        public ZekBatchContractClosureInstructionDto[] BatchVertragsabmeldung { get; set; }

        /// <summary>
        /// Zek-Batch Mutation Vertragsdaten (EC7) 
        /// Änderungen der Vertragsdaten (Änderung Bonitätscode, Vertragsverlängerung etc.).
        /// ZEKBatchServiceService.updateContractsBatch
        /// </summary>
        [AttributeFilter("zekUpdateContractsBatch")]
        public ZekBatchContractUpdateInstructionDto[] BatchMutationVertragsdaten { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /*CRMGT00027719 eCode178 in 3.5.1.2	in  Vertragsanmeldung 
        [AttributeFilter("zekeCode178Anmelden,zekeCode178Mutieren,zekeCode178Abmelden,zekeCode178Abfrage,zekRegisterBardarlehen,zekRegisterFestkredit,zekRegisterLM,zekRegisterTZ,zekRegisterKontokorrent,zekCloseBardarlehen,zekCloseLeasingMietvertrag," +
                         "zekCloseFestkredit,zekCloseTeilzahlungskredit,zekCloseKontokorrentkredit")] */
        [AttributeFilter("zekeCode178Anmelden,zekeCode178Mutieren,zekeCode178Abmelden,zekeCode178Abfrage")]
        public ZekeCode178Dto ZekeCode178Dto { get; set; }

        /// <summary>
        /// Used for eCode178Anmelden
        /// </summary>
        [AttributeFilter("zekeCode178Anmelden")]
        public string ContractId { get; set; }

        /// <summary>
        /// Used for getARMs
        /// </summary>
        [AttributeFilter("zekGetARMs")]
        public string DateLastSuccessfullRequest { get; set; } //Format: YYYY-MM-DDTHH:MM:SS.sss Year, month, day, hour,

        /// <summary>
        /// User's comment to the zek request
        /// </summary>
        public string Bemerkung { get; set; }

        /// <summary>
        /// "sprechende" Vertragnummer
        /// </summary>
        public string vertragnummer { get; set; }

        /// <summary>
        /// "sprechende" Antragnummer
        /// </summary>
        public string antragnummer { get; set; }

        /// <summary>
        /// Vertriebspartnernummer
        /// </summary>
        public string vpnummer { get; set; }
    }
}