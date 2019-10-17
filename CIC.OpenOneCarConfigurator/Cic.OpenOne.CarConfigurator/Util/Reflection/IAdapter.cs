
namespace Cic.OpenOne.Util.Reflection
{
	// NOTE BK, This empty interface is needed to identify an adapter by reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
	[System.CLSCompliant(true)]
	public interface IAdapter
	{
        Cic.P000001.Common.AdapterState DeliverAdapterState();
	}
}
