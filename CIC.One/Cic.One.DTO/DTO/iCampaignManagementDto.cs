using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.DTO
{
   
    /// <summary>
    /// Input-Parameters for Campaign Handling
    /// </summary>
    public class iCampaignManagementDto
    {
        /// <summary>
        /// Campaign id
        /// </summary>
       public long syscamp {get;set;}
        /// <summary>
        /// Search Filters for selecting contracts
        /// </summary>
       public iSearchDto iSearch {get;set;}
        /// <summary>
        /// CSV-File for upload
        /// </summary>
       public FileattDto fileData { get; set; }
    }
}
