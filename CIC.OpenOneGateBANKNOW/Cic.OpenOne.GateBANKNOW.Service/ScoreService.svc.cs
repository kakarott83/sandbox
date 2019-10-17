using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Reflection;
using System.ServiceModel;

namespace Cic.OpenOne.GateSCORE.Service
{
	using Cic.OpenOne.GateBANKNOW.Common.BO;
	using Cic.OpenOne.GateBANKNOW.Common.DAO.Score;
	using Cic.OpenOne.GateBANKNOW.Common.DTO;
	using Cic.OpenOne.GateBANKNOW.Service.BO;
	using Cic.OpenOne.GateSCORE.Service.Contract;

	/// <summary>
	/// SCORE Service Endpoint for BMW Integration Layer
	/// BMW Tallyman ←→ Integration Layer ←→ Score Service 
	/// </summary>
	[ServiceBehavior (Namespace = "http://cic-software.de/GateSCORE")]
	[Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
	[XmlSerializerFormat (Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
	public class ScoreService : IScoreService
	{
		/// <summary>
		/// Logger
		/// </summary>
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
		//set to true to enable validation of user
		private static bool enableUserValidation = true;

        /// <summary>
        /// Writes the updated Dunning Level Info into Openlease EAI
        /// the relevant fields are:
        /// VTMAHN:MSTUFE
        /// VTMAHN:MAHNDATUM
        /// VTMAHN:MZaehler1
        /// VTMAHN:MZaehler2
        /// VTMAHN:MZaehler3
        /// PEMAHN:MSTUFE
        /// PEMAHN:MAHNDATUM
        /// PEMAHN:MZaehler1
        /// PEMAHN:MZaehler2
        /// VTMAHN:MZaehler3
        ///	RN:MAHNSTUFE
        /// vllt: 
        ///		RN:MAHNDATUM (if needed)
        ///	updateDunningLevel
        /// FROM public ObDto createOrUpdateHEKOb (ObDto objekt)
        /// </summary>
        /// <param name="isetDunningLevel"></param>
        public osetDunningLevelDto updateDunningLevel (isetDunningLevelDto isetDunningLevel)
		{
			ServiceHandler<isetDunningLevelDto, osetDunningLevelDto> ew = new ServiceHandler <isetDunningLevelDto, osetDunningLevelDto> (isetDunningLevel);
			return ew.process (delegate (isetDunningLevelDto input, osetDunningLevelDto rval)
			{
				 if (input == null || input.DunningLevel == null)
					 throw new ArgumentException ("No valid input");

				 DunningLevelDao dao = new DunningLevelDao ();
				 dao.createOrUpdateDunningLevel (input.DunningLevel);

			}, enableUserValidation);
		}

		/// <summary>
		/// Writes the updated Arrangement Info into Openlease EAI
		///  
		/// </summary>
		/// <param name="isetArrangement"></param>
		public osetArrangementDto updateArrangement (isetArrangementDto isetArrangement)
		{
			ServiceHandler <isetArrangementDto, osetArrangementDto> ew = new ServiceHandler <isetArrangementDto, osetArrangementDto> (isetArrangement);
			return ew.process (delegate (isetArrangementDto input, osetArrangementDto rval)
			{
				 if (input == null || input.arrangement == null)
					 throw new ArgumentException ("No valid input");

				 ArrangementDao dao = new ArrangementDao ();
				 dao.createOrUpdateArrangementDto (input.arrangement);

			}, enableUserValidation);

		}

		/// <summary>
		/// request 2nd Direct Debit
		/// generate a 2ndDD table and returns OL-reference
		/// </summary>
		/// <param name="iset2ndDD"></param>
		public oset2ndDDDto request2ndDD (iset2ndDDDto iset2ndDD)
		{
			ServiceHandler <iset2ndDDDto, oset2ndDDDto> ew = new ServiceHandler <iset2ndDDDto, oset2ndDDDto> (iset2ndDD);
			return ew.process (delegate (iset2ndDDDto input, oset2ndDDDto rval)
			{
				 if (input == null || input.DDebit == null)
					 throw new ArgumentException ("No valid input");

				 DDebitDao dao = new DDebitDao ();
				 dao.createOrUpdateDDebitDto (input.DDebit);

			}, enableUserValidation);

		}
		/// <summary>
		/// request Posting
		/// </summary>
		/// <param name="isetPosting"></param>
		public osetPostingDto requestPosting (isetPostingDto isetPosting)
		{
			ServiceHandler <isetPostingDto, osetPostingDto> ew = new ServiceHandler <isetPostingDto, osetPostingDto> (isetPosting);
			return ew.process (delegate (isetPostingDto input, osetPostingDto rval)
			{
				 if (input == null || input.Posting == null)
					 throw new ArgumentException ("No valid input");

				 PostingDao dao = new PostingDao ();
				 dao.createOrUpdatePostingDto (input.Posting);

				 rval.ContractReference = input.Posting.ContractReference;

			}, enableUserValidation);

		}

		/// <summary>
		/// request Late Interest
		///  
		/// </summary>
		/// <param name="irequestLateInterest"></param>
		public orequestLateInterestDto requestLateInterest (irequestLateInterestDto irequestLateInterest)
		{
			ServiceHandler <irequestLateInterestDto, orequestLateInterestDto> ew = new ServiceHandler <irequestLateInterestDto, orequestLateInterestDto> (irequestLateInterest);
			return ew.process (delegate (irequestLateInterestDto input, orequestLateInterestDto rval)
			{

				 if (input == null || input.LateInterest == null)
					 throw new ArgumentException ("No valid input");


				 LateInterestDao dao = new LateInterestDao ();
				// REACT ON input
				dao.createOrUpdateLateInterestDto (input.LateInterest);

			}, enableUserValidation);
		}

		/// <summary>
		/// GET ContractDetails WITH EAIHOT-Interface
		///  
		/// </summary>
		/// <param name="igetContractDetails"></param>
		public ogetContractDto getContractDetails (igetContractDto igetContractDetails)
		{
			ServiceHandler <igetContractDto, ogetContractDto> ew = new ServiceHandler <igetContractDto, ogetContractDto> (igetContractDetails);
			return ew.process (delegate (igetContractDto input, ogetContractDto rval)
			{
				 if (input == null || string.IsNullOrEmpty (input.ContractReference))
					 throw new ArgumentException ("No valid Contract input");

				 ScoreContractBO ex = new ScoreContractBO ();
				 rval.Contract = ex.getContractFromDB (input.ContractReference);


				 ContractDao dao = new ContractDao ();
				 dao.getContractDto (ref rval);

			}, enableUserValidation);

		}


		/// <summary>
		/// GET Partner/Customer - Info
		/// </summary>
		/// <param name="inputPartner"></param>
		public ogetPartnerDto readPartner (igetPartnerDto inputPartner)
		{
			ServiceHandler <igetPartnerDto, ogetPartnerDto> ew = new ServiceHandler <igetPartnerDto, ogetPartnerDto> (inputPartner);
			return ew.process (delegate (igetPartnerDto input, ogetPartnerDto rval)
			 {
				 ScorePersonBO ex = new ScorePersonBO ();
				 rval.Person = ex.getPerson (input.CustomerReference);

			 }, enableUserValidation);

		}
	}
}