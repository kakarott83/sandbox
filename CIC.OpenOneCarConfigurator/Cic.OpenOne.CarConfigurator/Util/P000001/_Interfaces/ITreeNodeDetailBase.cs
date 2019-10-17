// OWNER: BK, 28-08-2008
namespace Cic.P000001.Common
{
    
	[System.CLSCompliant(true)]
	public interface ITreeNodeDetailBase
	{
		#region Properties
        
		Cic.P000001.Common.TreeNodeDetailTypeConstants TreeNodeDetailTypeConstant
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        
		Cic.P000001.Common.TreeNodeDetailValueTypeConstants TreeNodeDetailValueTypeConstant
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        

		string Category
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        
		string Name
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
        
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
