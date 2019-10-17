// OWNER MK, 07-08-2009
/*namespace Cic.OpenLease.ServiceAccess
{
    [System.CLSCompliant(true)]
    public class PdfThumbnailHelper
    {
        #region Private constants
        // Scale in percent
        private const float CnstScale = 0.15F;
        #endregion

        #region Private variables
        // Thumbnail image format
        private static System.Drawing.Imaging.ImageFormat _ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        #endregion

        #region Methods
        public static byte[] DeliverJpgThumbnail(byte[] pdfByteArray)
        {
            try
            {
                TallComponents.PDF.Rasterizer.Document Pdf = new TallComponents.PDF.Rasterizer.Document(new System.IO.MemoryStream(pdfByteArray));
                if (Pdf.Pages.Count > 0)
                {
                    TallComponents.PDF.Rasterizer.Page Page = Pdf.Pages[0];

                    using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)(Page.Width * CnstScale), (int)(Page.Height * CnstScale)))
                    {
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.Clear(System.Drawing.Color.White);
                        graphics.ScaleTransform(CnstScale, CnstScale);
                        Page.Draw(graphics);
                        System.IO.MemoryStream OutputStream = new System.IO.MemoryStream();
                        bitmap.Save(OutputStream, _ImageFormat);
                        return OutputStream.ToArray();
                    }
                }
            }
            catch
            {
                // Ignore errors
                throw;
            }

            return null;
        }
        #endregion
    }
}
*/