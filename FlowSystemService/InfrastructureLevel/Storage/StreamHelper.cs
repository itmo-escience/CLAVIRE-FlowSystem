using System;
using System.IO;

namespace Easis.Wfs.FlowSystemService.PESAbstraction.Storage
{
    /// <summary>
    /// Helps to copy data between stream.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Copies all bytes from source source to destination stream.<br/>
        /// This method doesn't close streams: you should do it by yourself.
        /// </summary>
        /// <param name="source">Source stream.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="bufferSize">Buffer size in bytes.</param>
        /// <returns>Bytes copied.</returns>
        public static long Copy(Stream source, Stream destination, int bufferSize = 1024 * 1000)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "Wrong buffer size (<=0).");

            if (!source.CanRead)
                throw new InvalidOperationException("Source stream can't be read.");

            if (!destination.CanWrite)
                throw new InvalidOperationException("Destination stream can't write.");

            byte[] buffer = new byte[bufferSize];

            long overallReadCount = 0;
            int chunkReadCount;
            while ((chunkReadCount = source.Read(buffer, 0, bufferSize)) > 0)
            {
                destination.Write(buffer, 0, chunkReadCount);

                overallReadCount += chunkReadCount;
                if (chunkReadCount < bufferSize)
                    break;
            }

            return overallReadCount;
        }
    }
}
