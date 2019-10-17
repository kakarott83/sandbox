using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Score
{
	public interface IInsolvencyDao
	{
		SoapXMLDto getSoapXMLDto ();
		void setSoapXMLDto (Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
	}

	public class InsolvencyDao : IInsolvencyDao
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
	}
}
