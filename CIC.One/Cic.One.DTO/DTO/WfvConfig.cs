using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /**
     * Helper Class to hold all ViewConfig's in the XML as List
     * @author admin
     */
    public class WfvConfig
    {
        public List<WfvEntry> entries;
        public List<WfvConfigEntry> configentries;
        public List<VlmTableDto> vlmtables;
        public List<CustomerConfig> customerconfigs;
        public List<LUConfig> luconfigs;
    }
}