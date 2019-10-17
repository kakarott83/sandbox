namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    #endregion
    public class ITConfiguration
    {
         #region Properties
        public string ITId
        {
            get;
            set;
        }

        
        public int PartnerTyp
        {
            get;
            set;
        }

        public int Flag
        {
            get;
            set;
        }

        public string Gender
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Prename
        {
            get;
            set;
        }

       
        public string Surname
        {
            get;
            set;
        }


        public string Surname2
        {
            get;
            set;
        }

        public string VATRegNumber
        {
            get;
            set;
        }

        public string VATRegNumberInternational
        {
            get;
            set;
        }

        public int VATGroupKey
        {
            get;
            set;
        }

       
        public string Street
        {
            get;
            set;
        }

        public string StreetNumber
        {
            get;
            set;
        }

        public string PoBox
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Fax
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string AddressField1
        {
            get;
            set;
        }

        public string AddressField2
        {
            get;
            set;
        }

        public string AddressField3
        {
            get;
            set;
        }

        public string AddressType
        {
            get;
            set;
        }
        
        public string Phone1
        {
            get;
            set;
        }

        public string Phone2
        {
            get;
            set;
        }

        public string Phone3
        {
            get;
            set;
        }

        public string Salutation
        {
            get;
            set;
        }

        public string Branch
        {
            get;
            set;
        }

        public int BirthDay
        {
            get;
            set;
        }

        public int BirthMonth
        {
            get;
            set;
        }
        public DateTime? gebDatum { get; set; }
        public int BirthYear
        {
            get;
            set;
        }

        public string CostumerLanguage
        {
            get;
            set;
        }

        public int Day
        {
            get;
            set;
        }

        public int Month
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }





        public ITTypeConstants Type
        {
            get;
            private set;
        }
       
        #endregion

        #region Constructors
        public ITConfiguration(ITTypeConstants type)
        {
            this.Type = type;
        }
        #endregion

        #region My methods
       
        #endregion


    }
}