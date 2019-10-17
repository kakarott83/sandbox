
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System.ServiceModel;
namespace Cic.OpenOne.GateSCORE.Service.Contract
{

	/// <summary>
	/// Das Interface IScoreService stellt die Schnittstelle für alle ..., z.B. 
	/// 
	/// </summary>
	[ServiceContract (Name = "IScoreService", Namespace = "http://cic-software.de/GateSCORE")]
	public interface IScoreService
	{
		/// <summary>
		/// updateDunningLevel
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		osetDunningLevelDto updateDunningLevel (isetDunningLevelDto input);

		/// <summary>
		/// updateArrangement
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		osetArrangementDto updateArrangement (isetArrangementDto input);

		/// <summary>
		/// request2ndDD
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		oset2ndDDDto request2ndDD (iset2ndDDDto input);

		/// <summary>
		/// requestPosting
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		osetPostingDto requestPosting (isetPostingDto input);

		/// <summary>
		/// requestLateInterest
		/// </summary>
		/// <param input=" "> irequestLateInterestDto </param>
		/// <returns> orequestLateInterestDto </returns>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		orequestLateInterestDto requestLateInterest (irequestLateInterestDto input);

		/// <summary>
		/// readPartner
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		ogetPartnerDto readPartner (igetPartnerDto input);

		/// <summary>
		/// getContractDetails
		/// Splitted in getContractDetailsNonEAI (getContractFromDB) 
		/// and getContractPrecalcFields (getting  eaihot-OutputParams without waiting loop)
		/// </summary>
		/// <param name="input"></param>
		[OperationContract]
		[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
		ogetContractDto getContractDetails (igetContractDto input);

	}
}
