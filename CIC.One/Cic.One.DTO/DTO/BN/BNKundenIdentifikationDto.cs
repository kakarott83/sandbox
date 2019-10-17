
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class BNKundenIdentifikationDto : EntityDto
    {
        public long sysangebot { get; set; }

        public BNKundeDto kunde { get; set; }
        public BNKundeDto mitantragsteller { get; set; }

        public override long getEntityId()
        {
            return sysangebot;
        }
    }
}