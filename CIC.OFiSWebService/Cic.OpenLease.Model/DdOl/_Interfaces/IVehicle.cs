// OWNER: BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	[System.CLSCompliant(true)]
	public interface IVehicle
	{
		#region Properties
		// Identification
		long ExtId { get; }
		// ObjektZustand
		string OBJEKTZUSTAND { get; set; }								// - Objektart
		// Type
		string BEZEICHNUNG { get; set; }						// - Bezeichnung
		string ExtConfigurationKey { get; set; }				// - Konfigurationsschlüssel
		string HERSTELLER { get; set; }						// - Marke/Hersteller
		string FABRIKAT { get; set; }								// - Modell
		string TYP { get; set; }								// - Typ
		// Data
		string SCHWACKE { get; set; }							// - Schwacke
		string EUROTAX { get; set; }							// - Eurotax
		// Registration
		string FGNR { get; set; }						// - Fahrgestellnummer
        //System.DateTime? erstzul { get; set; }	// - Erstzulassung
        //long? kmstand { get; set; }								// - Kilometerstand
        //string vorbesitzer { get; set; }						// - Vorbesitzer
		// Prices
		decimal? GRUND { get; set; }							// - Listenpreis
        decimal? ZUBEHOER { get; set; }					// - Zubehörpreis
        decimal? NEBENKOSTEN { get; set; }					// - Nebenkosten
        decimal? SUBVENTION { get; set; }						// - Subventionen
        decimal? GESAMT { get; set; }						// - Gesamtpreis
		#endregion
	}
}
