using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Output of createUploadDto Webservice
    /// </summary>
    public class ocreateUploadDto : oBaseDto
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
