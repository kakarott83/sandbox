using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// InputParameter für createAdresse Methode
    /// </summary>
    public class icreateApprovalDto
    {
        /// <summary>
        /// String
        /// </summary>
        public String username
        {
            get;
            set;
        }/// <summary>
        /// String
        /// </summary>
        public String password
        {
            get;
            set;
        }
        /// <summary>
        /// Customer
        /// </summary>
        public CustomerDto customer
        {
            get;
            set;
        }
        /// <summary>
        /// Guarantor
        /// </summary>
        public CustomerDto guarantor
        {
            get;
            set;
        }

        /// <summary>
        /// Object
        /// </summary>
        public ObjectDto obj
        {
            get;
            set;
        }
    }
}
