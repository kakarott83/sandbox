using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// "Generic" Dto for generic View
    /// 
    /// </summary>
    public class GviewDto : EntityDto
    {
        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysId { get; set; }

        /// <summary>
        /// OL Entity Area
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// Id of generic view setup
        /// </summary>
        public String gviewId { get; set; }
        
        /// <summary>
        /// Contains all fields including data for this generic view
        /// </summary>
        public List<Viewfield> fields { get; set; }

        /// <summary>
        /// Contains all primary keys of this generics data base tables
        /// </summary>
        public List<Pkey> pkeys { get; set; }
        
        override public long getEntityId()
        {
            return sysId;
        }
        public override string getEntityBezeichnung()
        {
            return "GV"+ sysId;
        }

    }
}