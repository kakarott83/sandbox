// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common
{
    
	[System.CLSCompliant(true)]
	public interface IParameterBase
	{
		#region Properties
        [System.Runtime.Serialization.DataMember]
		string Category
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        [System.Runtime.Serialization.DataMember]
		string Name
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        [System.Runtime.Serialization.DataMember]
		string Value
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
