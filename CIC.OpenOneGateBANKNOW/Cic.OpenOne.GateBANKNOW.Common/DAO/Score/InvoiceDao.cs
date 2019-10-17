using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Score
{
	public interface IInvoiceDao
	{
		SoapXMLDto getSoapXMLDto ();
		void setSoapXMLDto (Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
	}

	public class InvoiceDao : IInvoiceDao
	{
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
		private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto ();

		public SoapXMLDto getSoapXMLDto ()
		{
			return this.soapXMLDto;
		}

		public void setSoapXMLDto (Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
		{
			this.soapXMLDto = soapXMLDto;
		}

		public void createOrUpdateInvoiceDto (DTO.ScoreInvoiceDto invoiceDto)
		{
			ScoreEntityDao entity = new ScoreEntityDao ();
			entity.createOrUpdateInvoice (invoiceDto);
		}
	}
}
