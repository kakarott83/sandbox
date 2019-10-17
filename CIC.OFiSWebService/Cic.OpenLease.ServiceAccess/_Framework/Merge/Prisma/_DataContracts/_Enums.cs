// OWNER JJ, 15-09-2009
namespace Cic.OpenLease.ServiceAccess.Merge.Prisma
{
    /// <summary>
    /// FieldName
    /// JJ
    /// </summary>
    public enum FieldName : long
    {
        /// <summary>
        /// Kreditbetrag
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Kreditbetrag = 1,
        /// <summary>
        /// Laufzeit
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Laufzeit,
        /// <summary>
        /// Laufleistung
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Laufleistung
    }

    /// <summary>
    /// InterestType
    /// JJ
    /// </summary>
    public enum InterestType : long
    {
        /// <summary>
        /// Unbekannt
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Unbekannt = 0,
        /// <summary>
        /// Effektiv
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Effektiv,
        /// <summary>
        /// Nominal
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Nominal
    }

    /// <summary>
    /// ProvisionBase
    /// JJ
    /// </summary>
    public enum ProvisionBase : long
    {
        /// <summary>
        /// Unbekannt
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Unbekannt = 0,
        /// <summary>
        /// Listenpreis
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        Listenpreis,
        /// <summary>
        /// GesamtpreisKreditbetrag
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        GesamtpreisKreditbetrag,
        /// <summary>
        /// GesamtpreisKreditbetrag
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        ExterneBerechnungsgrundlage
    }
}