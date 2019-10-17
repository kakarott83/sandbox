using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Score
{
	public interface IContractDao
	{
		SoapXMLDto getSoapXMLDto ();
		void setSoapXMLDto (Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
	}

	public class ContractDao : IContractDao
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

		// public void getContractDto (long contractReference)
		// public DTO.ScoreContractDto getContractDto (DTO.ScoreContractDto contractDto)
		// public void getContractDto (ref DTO.ScoreContractDto contractDto)
		public void getContractDto (ref DTO.ogetContractDto contractDto)
		{
			ScoreEntityDao entity = new ScoreEntityDao ();
			// rh: handle as ref obj, as we already have DB-data in it
			entity.getContract (ref contractDto);
		}
	}
}
