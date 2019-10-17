// OWNER MK, 12-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class ADRESSE
    {
		#region Flag properties
		public bool IsActive
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.AKTIVKZ);
            }
        }
        #endregion
    }
}
