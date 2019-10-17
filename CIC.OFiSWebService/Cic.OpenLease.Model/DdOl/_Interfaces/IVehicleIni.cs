// OWNER: BK, 01-04-2009
namespace Cic.OpenLease.Model.DdOl
{
	[System.CLSCompliant(true)]
	public interface IVehicleIni
	{
		#region Properties
		// Identification
		long ExtId { get; }
		// Registration
		System.DateTime? ERSTZUL { get; set; }	// - Erstzulassung
		long? KMSTAND { get; set; }								// - Kilometerstand
		string VORBESITZER { get; set; }						// - Vorbesitzer
		#endregion
	}
}
