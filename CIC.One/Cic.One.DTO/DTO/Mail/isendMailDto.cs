namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabe für das Senden von Mails
    /// </summary>
    public class isendMailDto
    {
        public isendMailDto()
        {
        }

        /// <summary>
        /// Legt fest ob die Mail in dem Sendfolder gespeichert werden soll.
        /// Wird nur für das Senden von Mails verwendet
        /// </summary>
        public bool UseStandardFolder { get; set; }

        /// <summary>
        /// Beinhaltet die Mail die gesendet werden soll
        /// </summary>
        public MEmailMessageDto Mail { get; set; }

        /// <summary>
        /// Falls SaveInSendFolder der Mail gesetzt wurde, wird die Mail dort gespeichert.
        /// Falls dies leer oder null ist, wird der WellKnownFolderName genommen
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// Falls es null ist, wird die neue Mail im AKF-Ordner gespeichert
        /// (Dazu muss die FolderId null sein)
        /// </summary>
        public MWellKnownFolderNameEnum? WellKnownFolderName { get; set; }
    }
}