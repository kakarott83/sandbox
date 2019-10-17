// OWNER: BK, 25-11-2008
namespace Cic.OpenLease.Model.DdOl
{
	#region Enums
	// TEST BK 0 BK, Not tested
	[System.CLSCompliant(true)]
	public enum ZekProcessConstants : int
	{
		Deactivated = 0,
		ReadyToProcess = 1,
		InProcess = 2,
		Processed = 3,
		Error = 4,
		Delay = 5,
	}

	// TEST BK 0 BK, Not tested
	[System.CLSCompliant(true)]
	public enum KREMOProcessConstants : int
	{
		Deactivated = 0,
		ReadyToProcess = 1,
		InProcess = 2,
		Processed = 3,
		Error = 4,
	}

	// TEST BK 0 BK, Not tested
	[System.CLSCompliant(true)]
	public enum ADRVProcessConstants : int
	{
		Deactivated = 0,
		ReadyToProcess = 1,
		InProcess = 2,
		Processed = 3,
		Error = 4,
	}

	// TEST BK 0 BK, Not tested
	[System.CLSCompliant(true)]
	public enum PEINFOProcessConstants : int
	{
		Deactivated = 0,
		ReadyToProcess = 1,
		InProcess = 2,
		Processed = 3,
		Error = 4,
	}
	#endregion
}
