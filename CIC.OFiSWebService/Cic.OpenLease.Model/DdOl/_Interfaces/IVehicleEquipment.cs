// OWNER: BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	[System.CLSCompliant(true)]
	public interface IVehicleEquipment
	{
		#region Properties
		// Identification
		long ExtId { get; }
		// State
		int? FLAGPACKET { get; set; }
		bool IsPackage { get; }
		// Data
		string SNR { get; set; }
        string BESCHREIBUNG { get; set; }
		decimal? BETRAG { get; set; }
		#endregion
	}
}
