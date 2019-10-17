using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.OpenOne.Common.Util.IO
{
    /// <summary>
    /// Provides the enqueued Byte-Data for streaming
    /// The Byte-Data is produced and enqueued in an async Thread
    /// Example usage:
    /// 
    //return new AsyncLoadedDataStream(delegate(BlockingCollection<byte> queue)
    //   {

    //       byte[] mybytes = FileUtils.loadData(@"C:\temp\a.xml"); //ASCIIEncoding.Default.GetBytes("blafasel");
    //       foreach (byte b in mybytes) queue.Add(b);

    //   });
    /// </summary>
    public class AsyncLoadedDataStream : Stream
    {
        private long internalPos = 0;
        private ConcurrentQueue<byte> queue;
        private BlockingCollection<byte> blockQueue;
        private long bytesSent = 0;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void StreamProducer(BlockingCollection<byte> queue);

        public AsyncLoadedDataStream(StreamProducer workMethod)
        {
            internalPos = 0;

            queue = new System.Collections.Concurrent.ConcurrentQueue<byte>();
            blockQueue = new System.Collections.Concurrent.BlockingCollection<byte>(queue);

            //Start the given work-Method
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() => workMethod(blockQueue));//, _cancelToken
            //mark the producer/queue as finished
            task.ContinueWith((System.Threading.Tasks.Task t) => { blockQueue.CompleteAdding(); });
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new Exception("This stream does not support writing.");
        }

        public override long Length
        {

            get { throw new Exception("This stream does not support writing."); }
        }

        public override long Position
        {
            get
            {
                return internalPos;
            }
            set
            {
                throw new Exception("This stream does not support setting the Position property.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int rlen = 0;
            while (!blockQueue.IsCompleted)
            {
                if (rlen == count) break;//read max amount of chars

                // Blocks if number.Count == 0 
                // IOE means that Take() was called on a completed collection. 
                // Some other thread can call CompleteAdding after we pass the 
                // IsCompleted check but before we call Take.  
                // In this example, we can simply catch the exception since the  
                // loop will break on the next iteration. 
                try
                {
                    buffer[offset++] = blockQueue.Take();
                    rlen++;
                }
                catch (InvalidOperationException ex) {
                    _log.Debug("IOE in AsyncDataStream: " + ex.Message);
                    offset--;//reset offset
                }
            }
            if(bytesSent==0)
                _log.Debug("Start sending Async Data");

            bytesSent += rlen;
            if(bytesSent%1048576==0)
                _log.Debug("Sent Async " + bytesSent);
            if(rlen<count)
                _log.Debug("Sent Async " + bytesSent + " (" + rlen + "<" + count + ") and no more Data");

            return rlen;

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("This stream does not support seeking.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("This stream does not support setting the Length.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("This stream does not support writing.");
        }
        public override void Close()
        {

            base.Close();
        }
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

    }
}
