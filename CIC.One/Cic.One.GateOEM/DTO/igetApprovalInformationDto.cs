﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    public class igetApprovalInformationDto
    {
       
        /// <summary>
        /// String
        /// </summary>
        public String username
        {
            get;
            set;
        }
        
        /// <summary>
        /// String
        /// </summary>
        public String password
        {
            get;
            set;
        }

        /// <summary>
        /// technical Key of Approval
        /// </summary>
        public long sysid { get; set; }
    }
}