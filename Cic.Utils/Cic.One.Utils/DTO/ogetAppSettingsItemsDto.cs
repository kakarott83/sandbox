using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Cic.OpenOne.Common.DTO
{
    public class ogetAppSettingsServiceDto : oBaseDto
    {
        public ogetAppSettingsItemsDto item { set; get; }

    }

    public class ogetAppSettingsItemsDto 
    {
        public RegVarDto dto { get; set; }
        public RegVarDto[] dtos { get; set; }
        public RegVarPaths pathsInfo { get; set; }

        public ogetAppSettingsItemsDto()
        {
            pathsInfo = RegVarPaths.getInstance();
        }

     
    }

    public class Inner
    {
        public RegVarDto[] result { get; set; }
        public List<Inner> moreresults { get; set; }

    }
}
  