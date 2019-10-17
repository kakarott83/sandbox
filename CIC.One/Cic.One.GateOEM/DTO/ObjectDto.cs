using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    public class ObjectDto
    {
        /*	e.g. “12345” unique Offer number external id	*/
        public String OfferNumber { get; set; }
        /*	Defines the dealer 	*/
        public String Dealer { get; set; }
        /*	DealerId	*/
        public long Dealerid { get; set; }
        /*	ISO language code of supported language(s).	*/
        public String Language { get; set; }
        /*	Car total price = sales gross price (car net price + options net price + accessories net price + miscellaneous net price + transportcost – discount net price + taxes) e.g. “249000” (car price 249.000,00). Trade-ins and downpayment are not included	*/
        public double TotalPrice { get; set; }
        /*	Total discount on Model, Options and Accessories as amount (not %) (no taxes considered). Market specific discount items are considered as well.	*/
        public double Discount { get; set; }
        /*	Gross price of downpayment	*/
        public double DownPayment { get; set; }
        /*	Defines all the vehicle model information.	*/
        public String Vehicle { get; set; }
        /*	Internal Code	*/
        public String InternalCode { get; set; }
        /*	”1” = New Car ”2” = Used Car 	*/
        public long Type { get; set; }
        /*	KIA, Hyundai	*/
        public String Brand { get; set; }
        /*	Description of vehicle	*/
        public String VehicleName { get; set; }
        /*	Catalogue price of model without discount and taxes. Model price = net price	*/
        public double ModelPrice { get; set; }
        /*	Admission fee of the vehicle 	*/
        public double AdmissionFee { get; set; }
        /*	Transport cost of the vehicle.	*/
        public double TransportCost { get; set; }
        /*	vat in percentage of the model 	*/
        public double VATPercentage { get; set; }

        /*	Specific added tax value as percentage value	*/
        public double AddTax { get; set; }
        /*	Specific added tax value as price	*/
        public double AddTaxValue { get; set; }
        /*	order number	*/
        public String OrderNumber { get; set; }
        /*	additional information of the configuration	*/
        public String Information { get; set; }
        /*	Mileage of the vehicle	*/
        public long Km { get; set; }
        /*	Date of the registration	*/
        public DateTime? RegistrationDate { get; set; }
        /*	Price is a DecimalValue with in a given currency.	*/
        public double Price { get; set; }
        /*	Currencies will be given in the standard 3 character notation (EUR, DEM,…).	*/
        public String Currency { get; set; }
        /// <summary>
        /// Eurotax code
        /// </summary>
        public String Schwacke { get; set; }
        /// <summary>
        /// Modelcode
        /// </summary>
        public String Modelcode { get; set; }
        /// <summary>
        /// Power
        /// </summary>
        public long kwh { get; set; }
        /// <summary>
        /// Vehicle Options
        /// </summary>
        public OptionDto[] Options { get; set; }


    }

    public class OptionDto
    {
        /*	Option code	*/
        public long InternalCode { get; set; }
        /*	Type of option (COLOR, TRIM, OPTION, ACCESSORY, MISCELLANEOUS)	*/
        public long Type { get; set; }
        /*	Defines if option is included in package (true) or not (false).	*/
        public bool Package { get; set; }
        /*	Description of option	*/
        public String OptionName { get; set; }
        /*	Catalogue price of option without discount and taxes. Option price = net price 	*/
        public double OptionPrice { get; set; }
    }
}