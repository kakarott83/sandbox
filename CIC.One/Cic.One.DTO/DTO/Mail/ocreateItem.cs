using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ist der Output einer CreateItem Aktion
    /// </summary>
    public class ocreateItem : oBaseDto
    {
        /// <summary>
        /// Enthält die Id des erstellten Elements
        /// </summary>
        public string Id { get; set; }
    }
}