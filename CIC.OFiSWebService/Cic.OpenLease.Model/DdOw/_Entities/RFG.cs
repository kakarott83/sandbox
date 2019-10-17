// OWNER MK, 09-07-2009
namespace Cic.OpenLease.Model.DdOw
{
    // NOTE BK, CLSCompliant ist set by the generated partial class
    public partial class RFG
    {
        #region Flag properties
        public bool IsViewAllowed
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.SEHEN);
            }
        }

        public bool IsExecuteAllowed
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.AUSFUEHREN);
            }
        }

        public bool IsEditAllowed
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.BEARBEITEN);
            }
        }

        public bool IsInsertAllowed
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.EINFUEGEN);
            }
        }

        public bool IsDeleteAllowed
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.LOESCHEN);
            }
        }


        #endregion
    }
}
