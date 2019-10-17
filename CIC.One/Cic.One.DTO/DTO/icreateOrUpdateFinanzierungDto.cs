using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class icreateOrUpdateFinanzierungDto
    {
        public FinanzierungDto finanzierung { get; set; }
        /// <summary>
        /// 0=default
        /// 1=USE WIT Interface
        /// 2=Use HCE Interface
        /// </summary>
        public int saveMode { get; set; }
    }
}