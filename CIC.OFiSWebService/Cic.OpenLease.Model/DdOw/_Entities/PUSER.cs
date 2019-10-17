// OWNER MK, 18-09-2008
namespace Cic.OpenLease.Model.DdOw
{
	// NOTE BK, CLSCompliant ist set by the generated partial class
	public partial class PUSER
    {
        #region Properties
        public bool IsDisabled
        {
            // TEST MK 0 BK, Not tested
            get
            {
                if (this.DISABLED.GetValueOrDefault(1) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion
    }
}
