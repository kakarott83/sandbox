using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	class CreateColCaseOutDto
	{
		public string RecordID { get; set; }

		public string vorgangsnummer { get; set; }

		public bool hasError { get; set; }

		public string errorCode { get; set; }

		public string errorMessage { get; set; }

		public string errorRecordID { get; set; }
	}
}
