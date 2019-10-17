// OWNER MK, 03-02-2008
namespace Cic.OpenLease.ServiceAccess.Merge.MembershipProvider
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using System;
    #endregion

    [System.CLSCompliant(true)]
	public sealed class MembershipUserValidationInfo
	{
		#region Private variables
		private Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus _MembershipUserValidationStatus;
		#endregion

		#region Constructors
		public MembershipUserValidationInfo()
		{
			// Set defaults
            _MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.NotValid;
		}
		#endregion

       

		#region Properties
        
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.ServiceAccess.DdOw.PUSERDto PUSERDto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public  bool systemlocked
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string MembershipUserValidationStatusReason
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.ServiceAccess.DdOw.WFUSERDto WFUSERDto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public BRANDDto[] BRANDDto
        {
            get;
            set;
        }
        
        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus MembershipUserValidationStatus
		{
			get 
			{
				return _MembershipUserValidationStatus; 
			}
			set 
			{
				_MembershipUserValidationStatus = value; 
			}
		}
		#endregion

        
        [System.Runtime.Serialization.DataMember]
        public bool IsInternalMitarbeiter { get; set; }

        /// <summary>
        /// All roletypes of the user
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int[] roletypes {get;set;}

        [System.Runtime.Serialization.DataMember]
        public String BMWICalcButton { get; set; }

        
        [System.Runtime.Serialization.DataMember]
        public String BMWICalcURL { get; set; }

        [System.Runtime.Serialization.DataMember]
        public String MANDATORT { get; set; }
    }
}
