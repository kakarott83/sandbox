namespace Cic.One.DTO
{
    public class MFileAttachement
    {
        // Summary:
        //     Gets or sets the content Id of the attachment. ContentId can be used as a
        //     custom way to identify an attachment in order to reference it from within
        //     the body of the item the attachment belongs to.
        public string ContentId { get; set; }

        //
        // Summary:
        //     Gets or sets the content location of the attachment. ContentLocation can
        //     be used to associate an attachment with a Url defining its location on the
        //     Web.
        public string ContentLocation { get; set; }

        //
        // Summary:
        //     Gets or sets the content type of the attachment.
        public string ContentType { get; set; }

        //
        // Summary:
        //     Gets the Id of the attachment.
        public string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the name of the attachment.
        public string Name { get; set; }

        //
        // Summary:
        //     Gets the size of the attachment.
        public int Size { get; set; }

        // Summary:
        //     Gets the content of the attachment into memory. Content is set only when
        //     Load() is called.
        public byte[] Content { get; set; }

        //
        // Summary:
        //     Gets the name of the file the attachment is linked to.
        public string FileName { get; set; }
    }
}