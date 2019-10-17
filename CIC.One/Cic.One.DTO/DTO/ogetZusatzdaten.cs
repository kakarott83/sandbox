using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetZusatzdaten : oBaseDto
    {
        public PkzDto pkz { get; set; }
        public UkzDto ukz { get; set; }
    }
}
