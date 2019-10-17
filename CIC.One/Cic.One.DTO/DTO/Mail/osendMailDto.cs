using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ist der Output einer SendMail Aktion
    /// </summary>
    public class osendMailDto : oBaseDto
    {
        /// <summary>
        /// Enthält die Id der erstellten Mail
        /// </summary>
        public string Id { get; set; }
    }
}