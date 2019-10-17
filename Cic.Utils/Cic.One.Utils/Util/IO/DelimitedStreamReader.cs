using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.OpenOne.Common.Util.IO
{
    /// <summary>
    /// Reads the given Stream, delimited by a given delimiter (will be excluded), calling a given delegate-function with the resulting lines
    /// The Encoding controls the interpretation of the data stream
    /// maxLength defines the maximum length of a readed line
    /// </summary>
    public class DelimitedStreamReader
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Stream stream;
        private int maxLength;
        private byte delimiter;
        private lineRead processor;
        private Encoding streamEncoding;
        private bool finished = false;
        private byte[] eof;

        public long bytesTotal { get; set; }
        public delegate void lineRead(DelimitedStreamReader reader, String line);

        public DelimitedStreamReader(Stream stream, byte delimiter, int maxLength, Encoding streamEncoding, lineRead processor, byte[] eof)
        {
            this.stream = stream;
            this.delimiter = delimiter;
            this.maxLength = maxLength;
            this.processor = processor;
            this.streamEncoding = streamEncoding;
            this.eof = eof;
        }

        public void start()
        {
            byte[] buffer = new byte[64000];
            byte[] lineBuffer = new byte[maxLength];
            int read = 0;
            byte e1=0, e2=0, e3=0;
            if (bytesTotal == 0)
                _log.Debug("Start receiving Data");

            int lBufIdx = 0;//current lineBuffer Index
            while ((read = stream.Read(buffer, 0, maxLength)) > 0)
            {
                bytesTotal += read;
                for (int i = 0; i < read; i++)
                {

                    if (buffer[i] == delimiter) //found delimiter token
                    {
                        String readStr = streamEncoding.GetString(lineBuffer, 0, lBufIdx);
                        processor(this, readStr);
                        lBufIdx = 0;
                    }
                    else lineBuffer[lBufIdx++] = buffer[i];

                    e3 = e2;
                    e2 = e1;
                    e1 = buffer[i];
                    
                }
                if (bytesTotal % 1048576 == 0)
                    _log.Debug("Read " + bytesTotal);

                if (eof!=null && e1 == eof[0] && e2 == eof[1] && e3 == eof[2])
                {
                    _log.Debug("Read EOF");
                    lBufIdx = 0;
                    break;
                }
				if (eof!=null && e1 == 0 && e2 == eof[1] && e3 == eof[2])
                {
                    _log.Debug("Read EOF Zero");
                    lBufIdx = 0;
                    break;
                }
            }
            _log.Debug("Closing Stream");
            if (lBufIdx > 0)
            {
                String readStrEnd = streamEncoding.GetString(lineBuffer, 0, lBufIdx);
                if(readStrEnd!=null && readStrEnd.Length!=1 && readStrEnd[0]>0)
                    processor(this, readStrEnd);
            }
            finished = true;
            stream.Close();
            _log.Debug("Stream closed");
        }
        public bool isFinished()
        {
            return finished;
        }
    }
}
