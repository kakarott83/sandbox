using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.searchUpload"/> Methode
    /// </summary>
    public class osearchUploadDto : oBaseDto
    {
        /// <summary>
        /// Search Result for Uploads
        /// </summary>
        public oSearchDto<FileDto> result
        {
            get;
            set;
        }

    }
}
