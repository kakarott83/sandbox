using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLeaseAuskunftManagement;


namespace Cic.One.DTO
{
    public class igetBonitaetDto  
    {
        //Suchart
        public String searchtype { get; set; }

        //Idennummber
        public string identificationnumber { get; set; } //Muss-Feld für Suchart 00005

        //FirmaName
        public string companyname { get; set; } // Muss-Feld $ generische Suche erlaubt 

        //Strasse
        public string street { get; set; }

        //Hausnummer
        public string housenumber { get; set; }

        //Hausnummerzusatz
        public string housenumberaffix { get; set; }

        //Postleitzahl
        public string postcode { get; set; } //Muss-Feld für Suchart 00002, 00003

        //Ort
        public string city { get; set; } // Muss-Feld für Suchart 00004

        //Land
        public string country { get; set; }

        //Rechtsform
        public Cic.OpenLeaseAuskunftManagement.DTO.CrefoRechtsform legalform { get; set; }

        //Vorwahl
        public string diallingcode { get; set; } // Muss-Feld für 00006

        //Telefonnummer
        public string phonenumber { get; set; }

        //Email
        public string email { get; set; }

        //Webseite
        public string webseit { get; set; }

        //RegisterArt
        public string registertype { get; set; }

        //HandelsregisterNummer
        public string registerid { get; set; }// Muss-Feld für 00007


    }
}