// OWNER MK, DD-MM-200Y
namespace Cic.OpenLease.Service
{
    // [System.CLSCompliant(true)]
    internal static class MembershipProviderHelper
    {
        #region Constants
        private const string _CnstStrMembershipProviderName = "OpenLeaseMembershipProvider";
        #endregion

        #region Methods
        internal static Cic.OpenLease.Common.MembershipProvider DeliverProvider()
        {
            Cic.OpenLease.Common.MembershipProvider Provider = null;

            try
            {
                // Get the OpenLeaseMembershipProvider from collection
                if (System.Web.Security.Membership.Providers.Count > 0)
                {
                    Provider = System.Web.Security.Membership.Providers[_CnstStrMembershipProviderName] as Cic.OpenLease.Common.MembershipProvider;
                }

            }
            catch
            {
                // Ignore exception
            }

            // Retrun
            return Provider;
        }
        #endregion
    }
}
