using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Foodas returnvalue baseclass with error informations
    /// </summary>
    public class FoodasOutDto
    {
        public bool hasError { get; set; }
        public String errorCode { get; set; }
        private String errMsg;
        public String errorMessage
        {
            get
            {
                if (errMsg == null) return null;
                if (errMsg.Length > 255) return errMsg.Substring(0, 255);
                return errMsg;
            }
            set
            {
                errMsg = value;
            }
        }
        public String errorRecordID { get; set; }
        public String RecordID { get; set; }
    }
}
