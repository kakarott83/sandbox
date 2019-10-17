using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public class DecisionEngineGuardeanInDto
    {
        //when set the BO can read the auskunft itself
        public long sysauskunft { get; set; }
        //must always be set, the antrag sysid
        public long sysid { get; set; }

        public String XMLREQUEST { get; set; }
        //public long sysperson { get; set; }
        public int isMa { get; set; }
        public long syskdtyp { get; set; }

        /*
        public String CITY { get; set; }
        public String COUNTRY { get; set; }
        public String HOUSENUMBER { get; set; }
        public String HOUSENUMBERAFFIX { get; set; }
        public String POSTALCODE { get; set; }
        public String STREET { get; set; }
        public String CURRENCY { get; set; }
        public String EMPLOYERCONTACTPERSON { get; set; }
        public String EMPLOYERNAME { get; set; }
        public DateTime EMPLOYMENTENDDATE { get; set; }
        public long EMPLOYMENTMONTHSSINCE { get; set; }
        public String EMPLOYMENTSECTOR { get; set; }
        public DateTime EMPLOYMENTSSTARTDATE { get; set; }
        public String EMPLOYMENTTYPE { get; set; }
        public long EXPERIENCEMONTHSSINCE { get; set; }
        public int ISCURRENTEMPLOYMENT { get; set; }
        public int ISSELFEMPLOYMENT { get; set; }
        public String PROFESSION { get; set; }
        public String TYPEOFJOBCONTRACT { get; set; }
        public String DEBTINCOMERATIO { get; set; }
        public decimal EXPENCE { get; set; }
        public String INCOME { get; set; }
        public decimal TOTALEXPENSEAMOUNT { get; set; }
        public String TOTALGROSSINCOME { get; set; }
        public decimal TOTALNETINCOME { get; set; }
        public DateTime DOCUMENTISSUEDATE { get; set; }
        public DateTime DOCUMENTVALIDTILL { get; set; }
        public int ISVERYFIED { get; set; }
        public String ISSUINGAUTHORITY { get; set; }
        public int RESIDENCEPERMITION { get; set; }
        public DateTime RESIDENCEPERMITIONARRIVALDATE { get; set; }
        public DateTime RESIDENCEPERMITIONUNTIL { get; set; }
        public DateTime APPLICATIONDATE { get; set; }
        public String APPLICATIONID { get; set; }
        public String BIC { get; set; }
        public String IBAN { get; set; }
        public String BANKACCOUNTNUMBER { get; set; }
        public String BANKACCOUNTOWNER { get; set; }
        public DateTime BANKACCOUNTSINCE { get; set; }
        public String BANKACCOUNTTYPE { get; set; }
        public String BANKCITY { get; set; }
        public String BANKCODE { get; set; }
        public String BANKNAME { get; set; }
        public String PAYMENTSMETHOD { get; set; }
        public String NICHTGEDECKTERFEHLBETRAG { get; set; }
        public String NICHTGEDECKTERFEHLBETRAGVORH { get; set; }
        public String NICHTGEDECKTERFEHLBETRAGVORZ { get; set; }
        public decimal SHAREDCAPITEL { get; set; }
        public decimal TURNOVER { get; set; }
        public String CONTACTTYPE { get; set; }
        public String EMAIL { get; set; }
        public String FAXNUMBER { get; set; }
        public String FAXNUMBERBUSINESS { get; set; }
        public String MOBILENUMBER { get; set; }
        public String MOBILENUMBERBUSINESS { get; set; }
        public String PHONENUMBER { get; set; }
        public String PHONENUMBERBUSINESS { get; set; }
        public int PPI_FLAG_PAKET1 { get; set; }
        public int PPI_FLAG_PAKET2 { get; set; }
        public decimal BASEINTERESTRATE { get; set; }
        public decimal DOWNPAYMENT { get; set; }
        public decimal FINANCEDAMOUNT { get; set; }
        public decimal TERM { get; set; }
        public decimal TOTALMONTHLYPAYMENT { get; set; }
        public String DEALERCOMPANY { get; set; }
        public String DEALERGROUP { get; set; }
        public String DEALERRATING { get; set; }
        public String DEALERTYPE { get; set; }
        public String ADDADRESSINFO { get; set; }
        public String ADDRESSEXISTS { get; set; }
        public DateTime ADDRESSSINCE { get; set; }
        public String ADDRESSTYPE { get; set; }
        public String ADDRESSVALIDATED { get; set; }
        public String CITY_2 { get; set; }
        public String COUNTRY_2 { get; set; }
        public String HOUSENUMBER_2 { get; set; }
        public String HOUSENUMBERAFFIX_2 { get; set; }
        public String ISCURRENTADDRESS { get; set; }
        public String STREET_2 { get; set; }
        public String POSTALCODE_2 { get; set; }
        public String TYPEOFLIVING { get; set; }
        public String VATNUMBER { get; set; }
        public String COMPANYNAME { get; set; }
        public String CREFOID { get; set; }
        public String LEGALFORM { get; set; }
        public long NUMBEROFEMPLOYEES { get; set; }
        public String REGISTERNUMBER { get; set; }
        public String BIRTHNAME { get; set; }
        public String CITIZENSHIP { get; set; }
        public String COUNTRYOFBIRTH { get; set; }
        public String EDUCATIONTYPE { get; set; }
        public int EMPLOYEE { get; set; }
        public String FIRSTNAME { get; set; }
        public String GENDER { get; set; }
        public String LASTNAME { get; set; }
        public String MARITALSTATUS { get; set; }
        public long NUMBEROFCHILDRENTYPE1 { get; set; }
        public long NUMBEROFCHILDRENTYPE2 { get; set; }
        public long NUMBEROFCHILDRENTYPE3 { get; set; }
        public long NUMBEROFCHILDRENTYPE4 { get; set; }
        public String DEPENDENTS { get; set; }
        public String PLACEOFBIRTH { get; set; }
        public long RESIDENCYMONTHSINCE { get; set; }
        public DateTime RESIDENCYSINCE { get; set; }
        public String RESIDENCYSTATUS { get; set; }
        public String SALUTATION { get; set; }
        public String SCHUFAID { get; set; }
        public String TITLE { get; set; }
        public decimal AMOUNTFORECLOSURE { get; set; }
        
        public DateTime CUSTOMERSINCE { get; set; }
        public String LANGUAGE { get; set; }
        public String SECURITYTYPE { get; set; }
        public String STATE { get; set; }
        public String ACCOUNTNUMBER { get; set; }
        public String DATE_F { get; set; }
        public String DESCRIPTION { get; set; }
        public String FEATURECODE { get; set; }
        public String FEATUREWITHOUTDATEOFBIRTH { get; set; }
        public String INSTALLMENTNUMBER { get; set; }
        public String INSTALLMENTTYPE { get; set; }
        public String OWNFEATURE { get; set; }
        public String TEXT { get; set; }
        public String TYPE { get; set; }
        public decimal GROSSINCOME { get; set; }
        public DateTime INCOMESINCE { get; set; }
        public String INCOMETERM { get; set; }
        public String INCOMETYPE { get; set; }
        public decimal NETINCOME { get; set; }
        public decimal ANNUALPROFIT { get; set; }
        public decimal LIQUIDASSETS { get; set; }
        public decimal SHORTTERMLIABILITIES { get; set; }
        public decimal TOTALASSETS { get; set; }
        public decimal TURNOVER_1 { get; set; }
        public String COMPANYNAME_1 { get; set; }
        public String CREFOID_1 { get; set; }
        public String LEGALFORM_1 { get; set; }
        public DateTime DATEOFBIRTH { get; set; }*/

    }
}