using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace LanCloud.Domain.VirtualFtp
{
    public class FileRefReader : Stream
    {
        public FileRefReader(PathFileInfo pathInfo, ILogger logger)
        {
            PathInfo = pathInfo;
            Logger = logger;

            FileBitReaders = FileRef.Bits
                .Select(fileRefBit =>
                {
                    var fileBit = Application
                        .FindFileBits(PathInfo.Extention, FileRef, fileRefBit)
                        .FirstOrDefault();
                    return new FileBitReader(this, fileBit, Logger);
                })
                .Where(fileBit => fileBit.Exception == null)
                .OrderBy(a => a.Indexes.Length)
                .ThenBy(a => a.Indexes.OrderBy(b => b).First())
                .ToArray();
            AllIndexes = FileBitReaders
                .SelectMany(a => a.Indexes)
                .GroupBy(a => a)
                .Select(a => a.Key)
                .OrderBy(a => a)
                .ToArray();
            Buffer = new DoubleBuffer(AllIndexes.Length);

            Thread = new Thread(new ThreadStart(Start));
            Thread.Start();
        }

        public PathFileInfo PathInfo { get; }
        public ILogger Logger { get; }
        internal FileBitReader[] FileBitReaders { get; }
        public byte[] AllIndexes { get; }
        public DoubleBuffer Buffer { get; }
        public Thread Thread { get; }
        public bool Disposed { get; private set; }
        public override long Position { get; set; }
        private bool BufferInitialized { get; set; }
        private int BufferPosition { get; set; }
        private int BufferCounter { get; set; }

        private AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private AutoResetEvent BufferIsWritten { get; } = new AutoResetEvent(false);

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => PathInfo.Length.Value;

        public LocalApplication Application => PathInfo.Application;
        public FileRef FileRef => PathInfo.FileRef;
        bool KillSwitch { get; set; }

        private void Start()
        {
            while (!KillSwitch)
            {
                if (StartNext.WaitOne(100))
                {
                    Buffer.WriteBufferPosition = 0;

                    var goodReaders = FileBitReaders.Where(a => a.Exception == null).ToArray();
                    if (goodReaders.Length < AllIndexes.Length)
                    {
                        throw GeneralException();
                    }
                    if (goodReaders.Length >= AllIndexes.Length)
                    {
                        ReadAll(goodReaders);
                    }

                    BufferCounter++;
                    BufferIsWritten.Set();
                }
            }
        }
        private void ReadAll(FileBitReader[] goodReaders)
        {
            foreach (var item in goodReaders)
            {
                item.FlipBuffer(BufferCounter);
            }
            goodReaders = goodReaders.Where(a => a.Exception == null).ToArray();

            var normals = AllIndexes
                .Select(index => new
                {
                    Index = index,
                    Readers = goodReaders
                        .Where(a => 
                            a.Indexes.Length == 1 && 
                            a.Indexes.First() == index)
                        .ToArray()
                })
                .Where(a => a.Readers.Any())
                .OrderBy(a => a.Index)
                .ToArray();

            if (normals.Length == AllIndexes.Length)
            {
                foreach (var item in normals)
                {
                    ReadItem(item.Readers
                        .OrderByDescending(a => a.Speed)
                        .FirstOrDefault());
                }
            }
            else
            {
                var parityReaders = goodReaders
                    .Where(reader => !normals.Any(normal => normal.Readers.Any(normalReader => normalReader == reader)))
                    .ToArray();

                var missingIndexes = AllIndexes
                    .Where(index => !normals.Any(b => b.Index == index))
                    .ToArray();

                var missingIndexReaders = missingIndexes
                    .Select(missingIndex => new
                    {
                        MissingIndex = missingIndex,
                        Readers = parityReaders
                            .Where(other =>
                                other.Indexes.Length > 1 &&
                                other.Indexes.Contains(missingIndex) &&
                                other.Indexes.Any(otherIndex => normals.Any(foundNormal => foundNormal.Index == otherIndex)))
                            .ToArray()
                    })
                    .Where(a => a.Readers.Any())
                    .ToArray();

                if (missingIndexReaders.Length == missingIndexes.Length)
                {
                    foreach (var index in AllIndexes)
                    {
                        var normal = normals.FirstOrDefault(a => a.Index == index);
                        if (normal != null)
                        {
                            ReadItem(normal.Readers
                                .OrderByDescending(a => a.Speed)
                                .FirstOrDefault());
                        }
                        else
                        {
                            var parity = missingIndexReaders
                                .FirstOrDefault(a => a.MissingIndex == index);
                            if (parity != null)
                            {
                                var parityreader = parity.Readers
                                    .OrderByDescending(a => a.Speed)
                                    .FirstOrDefault(a => a.Indexes.Any(b => normals.Any(c => c.Index == b)));
                                if (parityreader != null)
                                {
                                    var buffer = parityreader.Buffer.ReadBuffer;
                                    var length = parityreader.Buffer.ReadBufferPosition;

                                    var otherNormals = normals
                                        .Where(foundNormal => parityreader.Indexes.Contains(foundNormal.Index))
                                        .ToArray();
                                    if (otherNormals.Length + 1 == parityreader.Indexes.Length)
                                    {
                                        // XOR data from indexes on to own buffer
                                        foreach (var other in otherNormals)
                                        {
                                            var otherReader = other.Readers
                                                .OrderByDescending(a => a.Speed)
                                                .FirstOrDefault();
                                            var otherBuffer = otherReader.Buffer.ReadBuffer;
                                            for (var i = 0; i < length; i++)
                                            {
                                                buffer[i] ^= otherBuffer[i];
                                            }
                                        }

                                        Array.Copy(buffer, 0, Buffer.WriteBuffer, Buffer.WriteBufferPosition, length);

                                        Buffer.WriteBufferPosition += length;
                                    }
                                    else
                                    {
                                        throw GeneralException();
                                    }
                                }
                                else
                                {
                                    throw GeneralException();
                                }
                            }
                            else
                            {
                                throw GeneralException();
                            }
                        }
                    }
                }
                else
                {
                    throw GeneralException();
                }
            }
        }

        private void ReadItem(FileBitReader item)
        {
            if (item.Buffer.ReadBufferPosition > 0)
            {
                var length = item.Buffer.ReadBufferPosition;

                Array.Copy(item.Buffer.ReadBuffer, 0, Buffer.WriteBuffer, Buffer.WriteBufferPosition, length);

                Buffer.WriteBufferPosition += length;
            }
        }

        private void FlipBuffer()
        {
            BufferIsWritten.WaitOne();
            Buffer.Flip();
            StartNext.Set();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!BufferInitialized)
            {
                FlipBuffer();
                BufferInitialized = true;
            }

            var read = 0;
            while (read < count && BufferPosition < Buffer.ReadBufferPosition)
            {
                var availableSpace = Buffer.ReadBufferPosition - BufferPosition;
                int bytesToWrite = Math.Min(count - read, availableSpace);

                Array.Copy(Buffer.ReadBuffer, BufferPosition, buffer, offset + read, bytesToWrite);

                read += bytesToWrite;
                BufferPosition += bytesToWrite;
                Position += bytesToWrite;

                if (BufferPosition == Buffer.ReadBuffer.Length)
                {
                    FlipBuffer();
                    BufferPosition = 0;
                }
            }
            return read;
        }

        private Exception GeneralException()
        {
            var badReaders = FileBitReaders.Where(a => a.Exception != null).ToArray();
            var message = $"Data is lost...{Environment.NewLine}";
            message += $"Path: {PathInfo.Path}{Environment.NewLine}";
            foreach (var badReader in badReaders)
            {
                message += $"{Environment.NewLine}";
                message += $"Indexes: {string.Join(", ", badReader.FileBit.Indexes)}{Environment.NewLine}";
                message += $"FilePart: {badReader.FileBit.Info.FullName}{Environment.NewLine}";
                message += $"Error: {badReader.Exception.Message}{Environment.NewLine}";
                message += $"Stacktrace: {badReader.Exception.StackTrace}{Environment.NewLine}";
            }
            return new Exception(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                KillSwitch = true;
                Disposed = true;

                foreach (var part in FileBitReaders)
                {
                    part.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Not implemented

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
