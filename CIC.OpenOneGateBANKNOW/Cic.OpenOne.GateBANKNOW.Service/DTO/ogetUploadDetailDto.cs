
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
   /// <summary>
   /// Output of getUploadDetail Webservice
   /// </summary>
    public class ogetUploadDetailDto : oBaseDto
    {
        /// <summary>
        /// File Data
        /// </summary>
        public FileDto file
        {
            get;
            set;
        }
    }
}