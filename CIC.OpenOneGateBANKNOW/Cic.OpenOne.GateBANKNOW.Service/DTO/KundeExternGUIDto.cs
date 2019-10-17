using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO {
    public class KundeExternGUIDto {
        /// <summary>
        /// Kundentyp
        /// </summary>
        public long syskdtyp { get; set; }

        /// <summary>
        /// Kundentyp-Bezeichnung
        /// </summary>
        public String kdtypBezeichnung { get; set; }


        // CRIF  class TypeBaseRequest: application id, customer id or request id
        /// <summary>
        /// refenceNummer
        /// </summary>
        public String refenceNummer { get; set; }


        #region CRIF_class_Person/CompanyAddressDescription
        
        /// <summary>
        /// VornameCRIF class PersonAddressDescription / string firstNameField;
        /// </summary>
        public String vorname { get; set; }

        /// <summary>
        /// CRIF class PersonAddressDescription / string lastNameField;CRIF class CompanyAddressDescription / string companyNameField;
        /// </summary>
        public String name { get; set; }

        
        /// <summary>
        /// maedchenName CRIF class PersonAddressDescription / string maidenNameField;
        /// </summary>
        public String geburtsName { get; set; }

        
        /// <summary>
        /// Vorname2CRIF class PersonAddressDescription / string middleNameField;
        /// </summary>
        public String vorname2 { get; set; }


        /// <summary>
        /// Vorname CRIF class PersonAddressDescription / string  coNameField;CRIF class CompanyAddressDescription / string coNameField;
        /// </summary>
        public String coName { get; set; }

        ///CRIF class PersonAddressDescription / sexField;
        /// <summary>
        /// Geschlecht
        /// </summary>
        public string geschlecht { get; set; }

        ///CRIF class PersonAddressDescription / string birthDateField;
        /// <summary>
        /// Geburtsdatum
        /// </summary>
        public DateTime? gebdatum { get; set; }
        #endregion CRIF_class_Person/CompanyAddressDescription

        #region CRIF_class_Location
        ///CRIF class Location  /string streetField;
        /// <summary>
        /// Strasse
        /// </summary>
        public String strasse { get; set; }

        ///CRIF class Location  / string houseNumberField;
        /// <summary>
        /// Hausnummer
        /// </summary>
        public String hsnr { get; set; }


        
        // apartment CRIF class Location  /  apartmentField;
        // private string apartment { get; set; }


        
        /// <summary>
        /// Postleitzahl CRIF class Location  / string zipField;
        /// </summary>
        public String plz { get; set; }

        
        /// <summary>
        /// Staat CRIF class Location  / string cityField;
        /// </summary>
        public long sysstaat { get; set; }
        /// <summary>
        /// Staat-Bezeichnung
        /// </summary>
        public String staatBezeichnung { get; set; }

        /// <summary>
        /// Ort
        /// </summary>
        public String ort { get; set; }

        //CRIF class Location/  string regionCodeField;
        /// <summary>
        /// region
        /// </summary>
        public String region { get; set; }

        
        /// <summary>
        /// subRegionCode CRIF class Location/  private string subRegionCodeField;
        /// </summary>
        public String subRegionCode { get; set; }

        
        /// <summary>
        /// Land CRIF class PersonAddressDescription /  string countryField;
        /// </summary>
        public long sysland { get; set; }

        /// <summary>
        /// Land (sysland) Bezeichnung
        /// </summary>
        public String landBezeichnung { get; set; }

        #endregion CRIF_class_Location

         



    }
}