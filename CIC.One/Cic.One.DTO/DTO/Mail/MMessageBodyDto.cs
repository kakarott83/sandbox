namespace Cic.One.DTO
{
    /// <summary>
    /// Ist der Body einer Mail oder eines Kalendereintrags
    /// </summary>
    public class MMessageBodyDto
    {
        public MMessageBodyDto()
        {
            BodyType = MBodyTypeEnum.HTML;
        }

        /// <summary>
        /// Typ des Body: HTML oder TEXT
        /// </summary>
        public MBodyTypeEnum BodyType { get; set; }

        /// <summary>
        /// Enthält den Bodytext
        /// </summary>
        public string Text { get; set; }
    }
}