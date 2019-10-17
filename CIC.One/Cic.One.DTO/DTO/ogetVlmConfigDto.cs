using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;


namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Vlm
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetVlmConfigDto : oBaseDto
    {
        public List<WfvEntry> entries
        {
            get;
            set;
        }
        public List<VlmMenuDto> menus
        {
            get;
            set;
        }
        
    
        
    }
}