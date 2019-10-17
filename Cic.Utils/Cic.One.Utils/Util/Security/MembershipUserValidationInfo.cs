
namespace Cic.OpenOne.Common.Util.Security
{
    /// <summary>
    /// MembershipUserValidationStatus-Enum
    /// </summary>
    public enum MembershipUserValidationStatus
    {
        /// <summary>
        /// Valid
        /// </summary>
        Valid,

        /// <summary>
        /// NotValid
        /// </summary>
        NotValid,

        /// <summary>
        /// UserNameNotValid
        /// </summary>
        UserNameNotValid,

        /// <summary>
        /// PasswordNotValid
        /// </summary>
        PasswordNotValid,

        /// <summary>
        /// ValidWorkflowUserNotFound
        /// </summary>
        ValidWorkflowUserNotFound,

        /// <summary>
        /// ValidRoleNotFound
        /// </summary>
        ValidRoleNotFound,

        /// <summary>
        /// ValidPersonNotFound
        /// </summary>
        ValidPersonNotFound,

        /// <summary>
        /// ValidBrandNotFound
        /// </summary>
        ValidBrandNotFound,

        /// <summary>
        /// UserDisabled
        /// </summary>
        UserDisabled,

        /// <summary>
        /// Systemlocked
        /// </summary>
        Systemlocked,
    }

    /// <summary>
    /// MembershipUserValidationInfo-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public sealed class MembershipUserValidationInfo
    {
        /// <summary>
        /// MembershipUserValidationInfo
        /// </summary>
        public MembershipUserValidationInfo()
        {
            // Set defaults
            this.MembershipUserValidationStatus = MembershipUserValidationStatus.NotValid;
        }

        #region Properties

        /// <summary>
        /// ISOLanguageCode
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ISOLanguageCode
        {
            get;
            set;
        }

        /// <summary>
        /// sysPEROLE
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysPEROLE
        {
            get;
            set;
        }

        /// <summary>
        /// sysBRAND
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysBRAND
        {
            get;
            set;
        }

        /// <summary>
        /// sysPERSON
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysPERSON
        {
            get;
            set;
        }

        /// <summary>
        /// sysPUSER
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysPUSER
        {
            get;
            set;
        }

        /// <summary>
        /// sysWFUSER
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysWFUSER
        {
            get;
            set;
        }

        /// <summary>
        /// systemlocked
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool systemlocked
        {
            get;
            set;
        }

        /// <summary>
        /// MembershipUserValidationStatusReason
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string MembershipUserValidationStatusReason
        {
            get;
            set;
        }

        /// <summary>
        /// MembershipUserValidationStatus
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public MembershipUserValidationStatus MembershipUserValidationStatus
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// IsInternalMitarbeiter
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool IsInternalMitarbeiter { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string WFUSERCODE
        {
            get;
            set;
        }
    }
}