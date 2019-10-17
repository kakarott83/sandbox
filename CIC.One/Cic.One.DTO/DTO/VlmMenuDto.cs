using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public enum MenuArt
    {
        MENU,
        TOOLBAR,
        CONTEXTMENU
    }
    
    public enum MenuType
    {
        DETAIL,
        BROWSE
    }

    public class VlmMenuDto
    {
        public long sysvlmmenu { get; set; }
        public String code { get; set; }
        public String text {get;set;}
        public List<VlmMenuItemDto> items { get; set; }        
        public MenuArt art {get;set;}
        public MenuType typ { get; set; }
        public String coderfu { get; set; }
        public String codermo { get; set; }
    }
}
