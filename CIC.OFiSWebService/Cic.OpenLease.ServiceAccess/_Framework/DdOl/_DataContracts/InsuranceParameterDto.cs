using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class InsuranceParameterDto
    {
        /// <summary>
        /// Kaufpreis Rabattiert - ANGOBAHKEXTERNBRUTTO
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal Kaufpreis
        {
            get;
            set;
        }
        /// <summary>
        /// Erstzulassung - ANGOBINIERSTZUL
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime? erstzulassung
        {
            get;
            set;
        }

        //Kasko
        /// <summary>
        /// Unrabattiert inkl. Ust + nova + novazuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Listenpreis
        {
            get;
            set;
        }

        /// <summary>
        /// Unrabattiert inkl. Ust + nova + novazuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Sonderausstattung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysPrProduct
        {
            get;
            set;
        }

        /// Unrabattiert inkl. Ust + nova + novazuschlag
        
        [System.Runtime.Serialization.DataMember]
        public decimal SAPakete
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ZubehoerFinanziert
        {
            get;
            set;
        }

        /// <summary>
        /// Nova Bonus Malus (ANGOBNOVAZUABBRUTTO) inkl. Zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Nova
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long Zahlmodus
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long Laufzeit
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long LaufzeitFinanzierung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long Praemienstufe
        {
            get;
            set;
        }

        //Haftpflicht
        
        [System.Runtime.Serialization.DataMember]
        public decimal KW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Hubraum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Deckungssumme
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PolKennzeichen
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Vorversicherung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Fremdversicherung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Nachlass
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Finanzierungssumme
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal FinanzierungssummeNetto
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal MonatsrateBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Zinssatz
        {
            get;
            set;
        }

       
        public decimal ZinssatzDefault
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal GAPSumme
        {
            get;
            set;
        }

        public decimal ZinssatzNominal
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal Zielrate
        {
            get;
            set;
        }

        //Primärschlüssel
        
        [System.Runtime.Serialization.DataMember]
        public long SysVSTYP //Variante
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public long sysObTyp //fahrzeugart
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysObArt //vorführ/neu/gebraucht
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysKdTyp //privat/unternehmen/freiberuflich
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string CODEMETHOD
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool calcProvision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int specialcalcStatus
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public DateTime? lieferdatum
        {
            get;
            set;
        }
        public bool isCredit { get; set; }

    }
}
