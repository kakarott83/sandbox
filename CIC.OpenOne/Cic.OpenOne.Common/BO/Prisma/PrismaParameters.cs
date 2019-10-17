using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Defines all Prisma Parameters for BNOW
    /// </summary>
    public enum PrismaParameters
    {
        /// <summary>
        /// Ausfschub
        /// </summary>
        [StringValueAttribute("GESCH_MARK_AUFSCHUB")]
        Aufschub,
        /// <summary>
        /// Satz Mehrkilometer
        /// </summary>
        [StringValueAttribute("OB_MARK_SATZMEHRKM")]
        SatzMehrKm,
        /// <summary>
        /// Kalkulationsgrenze Kaufsumme Intern
        /// </summary>
        [StringValueAttribute("KALK_BORDER_BGINTERN")]
        KalkBorderBgIntern,
        /// <summary>
        /// Kalkulationgrenze Rate
        /// </summary>
        [StringValueAttribute("KALK_BORDER_RATE")]
        KalkBorderRate,
        /// <summary>
        /// Kalkulationsgrenze Endalterkunde
        /// </summary>
        [StringValueAttribute("KALK_BORDER_ENDALTERKUNDE")]
        KalkBorderEndalterKunde,
        /// <summary>
        /// Kalkulationgrenze Kundenscore
        /// </summary>
        [StringValueAttribute("KALK_BORDER_KUNDENSCORE")]
        KalkBorderKundenScore,
        /// <summary>
        /// Kalkulationsgrenze Restwert
        /// </summary>
        [StringValueAttribute("KALK_BORDER_RW")]
        kalkBorderRW,
        /// <summary>
        /// Kalkulationsgrenze EndLL
        /// </summary>
        [StringValueAttribute("KALK_BORDER_ENDLL")]
        KalkBorderEndll,

        /// <summary>
        /// Kalkulationsgenze Laufzeit
        /// </summary>
        [StringValueAttribute("KALK_BORDER_LZ")]
        kalkBorderLz,

        /// <summary>
        /// Kilometerstand am Anfang
        /// </summary>
        [StringValueAttribute("KALK_BORDER_UBNAHMEKM")]
        kalkBorderUbnahmekm,

        /// <summary>
        /// Fahrzeugalter Anfang (zB Leasing)
        /// </summary>
        [StringValueAttribute("KALK_BORDER_UBALTEROBJ")]
        kalkBorderUbAlterObj,

        /// <summary>
        /// Fahrzeugalter Ende (zB Leasing)
        /// </summary>
        [StringValueAttribute("KALK_BORDER_ENDALTEROBJ")]
        kalkBorderEndAlterObj,

        /// <summary>
        /// Kaution ab RW (Leasing)
        /// </summary>
        [StringValueAttribute("KALK_RW_SCHWELLE_KAUTION")]
        kalkBorderRWschwelleKaution,

        /// <summary>
        /// Sonderzahlung in Prozent (z.B. Erste Rate bei FD1)
        /// </summary>
        [StringValueAttribute("KALK_BORDER_SZ_PROZENT")]
        kalkBorderSonderZahlungProzent,

        /// <summary>
        /// maximalalter für Versicherungen
        /// </summary>
        [StringValueAttribute("KALK_BORDER_ENDALTERKUNDE_AUA")]
        KalkBorderEndalterAUA,

        /// <summary>
        /// maximalalter für Versicherungen
        /// </summary>
        [StringValueAttribute("KALK_BORDER_ENDALTERKUNDE_RIP")]
        KalkBorderEndalterRIP,

        /// <summary>
        /// Prozentualer Anteil der Kreditlimite, den der Kreditbetrag nicht überschreiten darf
        /// (Eingegebener Kreditbetrag muss kleiner/gleich z.B. 40% der Kreditlimite sein)
        /// </summary>
        [StringValueAttribute("MAXLIMITPROZENT")]
        MaxLimitProzent,

        /// <summary>
        /// Initialladung Dispo Karte
        /// </summary>
        [StringValueAttribute("KALK_BORDER_INITLADUNG")]
        KalkBorderInitLadung,

        /// <summary>
        /// CasaEigentümerSeitTagen
        /// </summary>
        [StringValueAttribute("CASA_EIGENTUEMER_SEIT")]
        CasaEigentümerSeitTagen,

        /// <summary>
        /// DiplomaKundenalter
        /// </summary>
        [StringValueAttribute("DIPLOMA_KUNDENALTER")]
        DiplomaKundenalter,

        /// <summary>
        /// EL_WERTEGRUPPE für Expected Loss
        /// </summary>
        [StringValueAttribute("EL_WERTEGRUPPE")]
        EL_WERTEGRUPPE,

        /// <summary>
        /// ZFAKTOR
        /// </summary>
        [StringValueAttribute("ZFAKTOR")]
        ZFAKTOR,

        /// <summary>
        /// Laufzeit für Ratenabsicherung AUA
        /// </summary>
        [StringValueAttribute("KALK_BORDER_LZ_AUA")]
        KALK_BORDER_LZ_AUA,

        /// <summary>
        /// Laufzeit für Ratenabsicherung RIP
        /// </summary>
        [StringValueAttribute("KALK_BORDER_LZ_RIP")]
        KALK_BORDER_LZ_RIP,

        /// <summary>
        /// Maximalabweichung Rate für PartnerService
        /// </summary>
        [StringValueAttribute("KALK_RATE_DERIVATION")]
        KALK_RATE_DERIVATION

    }
}