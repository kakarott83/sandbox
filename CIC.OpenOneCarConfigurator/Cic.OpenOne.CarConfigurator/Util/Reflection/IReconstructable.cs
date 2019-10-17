// OWNER: BK, 22-08-2008
namespace Cic.OpenOne.Util.Reflection
{
    /// <summary>
    /// Supports validation of the object.
    /// </summary>
	[System.CLSCompliant(true)]
	public interface IReconstructable
	{
		#region Methods
		// TEST BK 0 BK, Not tested
        /// <summary>
        /// Validates the object.
        /// </summary>
		void Reconstruct();
		#endregion
	}
}
