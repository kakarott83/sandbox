
namespace Cic.OpenOne.Common.Model.DdOl
{
    /// <summary>
    /// Rollentyp Klasse
    /// </summary>
    [System.CLSCompliant(false)]
    public class RoleType
    {
    }

    /// <summary>
    /// Enum Rollentypen
    /// </summary>
    public enum RoleTypeTyp
    {
        /// <summary>
        /// undefined
        /// </summary>
        UNBESTIMMT = 0,
        /// <summary>
        /// Bank
        /// </summary>
        BANK = 1,
        /// <summary>
        /// Subsidiary (BANKNOW: Abwicklungsort)
        /// </summary>
        GESCHAEFTSSTELLE = 2,
        /// <summary>
        /// Employee
        /// </summary>
        BANKMITARBEITER = 3,
        /// <summary>
        /// Field staff
        /// </summary>
        AUSSENDIENSTMITARBEITER = 4,
        /// <summary>
        /// Broker
        /// </summary>
        VERMITTLER = 5,
        /// <summary>
        /// Reseller/Händler
        /// </summary>
        HAENDLER = 6,
        /// <summary>
        /// Seller (BANKNOW: Mitarbeiter)
        /// </summary>
        VERKAEUFER = 7,
        /// <summary>
        /// Department
        /// </summary>
        ABTEILUNG = 8,
        /// <summary>
        /// Regional Manager (BANKNOW: KAM)
        /// </summary>
        GEBIETSLEITER = 9,
        /// <summary>
        /// CEO
        /// </summary>
        GESCHAEFTSFUEHRER = 10,
        /// <summary>
        /// Sales manager
        /// </summary>
        VERKAUFSLEITER = 11,
        /// <summary>
        /// Reseller Group
        /// </summary>
        HAENDLERGRUPPE = 12,
        /// <summary>
        /// region
        /// </summary>
        REGION = 13,
        /// <summary>
        /// Business Line
        /// </summary>
        BUSINESSLINE = 14,
        /// <summary>
        /// Country
        /// </summary>
        LAND = 15,
        /// <summary>
        /// Corporation
        /// </summary>
        KONZERN = 16,
        /// <summary>
        /// Reserve 1
        /// </summary>
        RESERVE1 = 17,
        /// <summary>
        /// Reserve 2
        /// </summary>
        RESERVE2 = 18
    }
}