using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Kremo Budgetcalculation Result Object
    /// </summary>
    public class okremoGetBudget : oBaseDto
    {
        public BudgetDto budget { get; set; }
    }
}