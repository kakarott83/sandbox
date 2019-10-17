// OWNER MK, 25-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    [System.CLSCompliant(true)]
    public interface ICalculation
    {
        // Identification
        long ExtId { get; }

        // Prices
        decimal? GESAMT { get; set; }						// - Gesamtpreis, Objekt (GESAMT)
        // Rebates
        decimal? RABATTO { get; set; }						// - Rabatt, offen (RABATTO) 
        decimal? RABATTOP { get; set; }				// - Rabatt, offen, in Prozent (RABATTOP)
        decimal? RABATTV { get; set; }						// - Rabatt, verdeckt (RABATTV)
        decimal? RABATTVP { get; set; }				// - Rabatt, verdeckt, in Prozent (RABATTVP)

        // Provisions
        decimal? PROVISION { get; set; }							// - Provision, Händler/Vertriebspartner (PROVISION)
        decimal? PROVISIONP { get; set; }				// - Provision, Händler/

        // Conditions
        decimal? ZINS { get; set; }							// - Zins, nominal (ZINS)
        decimal? ZINSEFF { get; set; }					// - Zins, effective (ZINSEFF)
        int? PPY { get; set; }						// - Raten pro Jahr (PPY)
        decimal? MARGE { get; set; }							// - Marge (MARGE)
        decimal? MARGEP { get; set; }					// - Marge, in Prozent (MARGEP)

        // Calculation
        decimal? BGEXTERN { get; set; }			// - Berechnungsgrundlage (BGEXTERN)
        decimal? SZ { get; set; }						// - Sonderzahlung (SZ)
        decimal? SZP { get; set; }				// - Sonderzahlung, in Prozent (SZP)
        int? LZ { get; set; }									// - Laufzeit (LZ)
        decimal? RW { get; set; }						// - Restwert (RW)
        decimal? RWP { get; set; }			// - Restwert, in Prozent (RWP)
        decimal? RATE { get; set; }								// - Leasingrate (RATE)

        int? CALCTARGET { get; set; }
        int? HOLDFIELDS { get; set; }

        string SCHWACKE { get; set; }
    }
}
