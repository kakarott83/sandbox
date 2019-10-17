using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class osetArrangementDto : oBaseDto
    {
		//// ArrangementDto
		///// <summary>
		///// Liste Persistenzobjekte arrangement
		///// </summary>
		//public ScoreArrangementDto arrangement { get; set; }
		
		/// <summary>
		/// customerReference 
		/// </summary>
		public long CustomerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public long ContractReference { get; set; }
		
	}
}