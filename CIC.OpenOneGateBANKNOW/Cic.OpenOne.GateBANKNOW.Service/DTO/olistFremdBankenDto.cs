using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter 
    /// </summary>
    public class olistFremdBankenDto : oBaseDto
    {
        /// <summary>
        /// Array von Fremdbanken
        /// </summary>
        public FremdbankDto[] fremdbanken
        {
            get;
            set;
        }
    }
}
