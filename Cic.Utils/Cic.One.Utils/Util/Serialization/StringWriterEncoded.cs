using System.IO;
using System.Text;

namespace Cic.OpenOne.Common.Util.Serialization
{
    /// <summary>
    /// StringWriterEncoded-Klasse
    /// </summary>
    public sealed class StringWriterEncoded : StringWriter
    {
        private readonly Encoding encoding;

        /// <summary>
        /// StringWriterEncoded
        /// </summary>
        /// <param name="encoding"></param>
        public StringWriterEncoded(Encoding encoding)
        {
            this.encoding = encoding;
        }

        /// <summary>
        /// Encoding
        /// </summary>
        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}