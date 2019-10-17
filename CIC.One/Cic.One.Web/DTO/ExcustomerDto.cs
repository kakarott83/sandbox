using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// External app customer data
    /// </summary>
    public class ExcustomerDto
    {
        public long Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String City { get; set; }
        public String ZipCode { get; set; }
        public String Country { get; set; }
        public String BankName { get; set; }
        public String AccountNumber { get; set; }
        public String BankNumber { get; set; }
        public String Street { get; set; }

    }
}
