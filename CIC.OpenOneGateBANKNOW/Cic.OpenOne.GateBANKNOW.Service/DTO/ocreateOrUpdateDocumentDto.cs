using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für createOrUpdateDocument Methode
    /// </summary>
    public class ocreateOrUpdateDocumentDto : oBaseDto
    {
        /// <summary>
        /// The document
        /// </summary>
        public DmsDocDto document { get; set; }  
    }
}
