using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    public class CustomerDto
    {
        /*	ID of the customer 	*/
        public String ID { get; set; }
        /*	Private = 1, Business = 2	*/
        public short Type { get; set; }
        /*	Male = 1, Female =2, Unknown = 0	*/
        public short Gender { get; set; }
        /*	e.g. "Dr.”, “Prof.”	*/
        public String Title { get; set; }
        /*		*/
        public String Addition { get; set; }
        /*	Also used as companyname	*/
        public String Name { get; set; }
        /*		*/
        public String Firstname { get; set; }
        /*	VAT Registration Number	*/
        public String VATRegNumber { get; set; }
        /*	VAT Group key	*/
        public short VATGroup { get; set; }
        /*	Address without number	*/
        public String Street { get; set; }
        /*	Street is a concatenation of addressfield 1, addressfield 2 and addressfield 3	*/
        public String StreetNumber { get; set; }
        /*	street number 	*/
        public String PoBox { get; set; }
        /*	post-office box	*/
        public String Zip { get; set; }
        /*	Beispielstadt	*/
        public String City { get; set; }
        /*	country codes 	*/
        public String Country { get; set; }
        /*	e.g. "123444”	*/
        public String Phone { get; set; }
        /*	e.g. "44277 »	*/
        public String Fax { get; set; }
        /*	e.g. marta.Mustermann@hotmail.com. 	*/
        public String Email { get; set; }
        /*	Address field 1	*/
        public String AddressField1 { get; set; }
        /*	Address field 2	*/
        public String AddressField2 { get; set; }
        /*	Address field 3	*/
        public String AddressField3 { get; set; }
        /*	private telephone number	*/
        public String Phone1 { get; set; }
        /*	commercial telephone number	*/
        public String Phone2 { get; set; }
        /*	mobile phone number	*/
        public String Phone3 { get; set; }
        /*	salutation text	*/
        public String Salutation { get; set; }
        /*	Branch	*/
        public String Branch { get; set; }
        /*	legalform	*/
        public String Legalform { get; set; }
        /*	Birthday	*/
        public DateTime? Birthday { get; set; }
        /*	ISO language code of the customer	*/
        public String CustomerLanguage { get; set; }
        /*	Last change date	*/
        public DateTime? Day { get; set; }
        /*	Last change date	*/
        public DateTime? Month { get; set; }
        /*	Last change date	*/
        public DateTime? Year { get; set; }


    }
}