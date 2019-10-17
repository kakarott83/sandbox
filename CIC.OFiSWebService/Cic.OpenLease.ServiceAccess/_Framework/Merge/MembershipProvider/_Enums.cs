// OWNER MK, 29-10-2008
namespace Cic.OpenLease.ServiceAccess.Merge.MembershipProvider
{
    [System.CLSCompliant(true)]
    public enum MembershipUserValidationStatus
    {
        Valid,
        NotValid,
        UserNameNotValid,
        PasswordNotValid,
        ValidWorkflowUserNotFound,
        ValidRoleNotFound,        
        ValidPersonNotFound,
        ValidBrandNotFound,
        UserDisabled,
    }
}
