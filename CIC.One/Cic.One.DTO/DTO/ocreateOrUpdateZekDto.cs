using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// delivers recently updated or created ZEK request
    /// </summary>
    public class ocreateOrUpdateZekDto : oBaseDto
    {
        public ZekDto zek { get; set; }
    }
}
