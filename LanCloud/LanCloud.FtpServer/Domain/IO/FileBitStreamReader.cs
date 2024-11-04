using System;
using System.IO;
using System.IO.Compression;

namespace LanCloud.Domain.IO
{
    public class FileBitStreamReader : Stream, IDisposable
    {
        public FileInfo Info { get; }
        //public FileStream OuterStream { get; }
        //public ZipArchive Archive { get; }
        //public ZipArchiveEntry ReadmeEntry { get; }
        public Stream Stream { get; }

        public FileBitStreamReader(FileInfo info)
        {
            //OuterStream = Info.OpenRead();
            //Archive = new ZipArchive(OuterStream, ZipArchiveMode.Read);
            //ReadmeEntry = Archive.GetEntry("data.bin");
            //Stream = ReadmeEntry.Open();

            Stream = Info.OpenRead();
        }

        public override void Flush()
        {
            Stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return Stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            Stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Stream.Write(buffer, offset, count);
        }

        public override bool CanRead => Stream.CanRead;

        public override bool CanSeek => Stream.CanSeek;

        public override bool CanWrite => Stream.CanWrite;

        public override long Length => Stream.Length;

        public override long Position { get => Stream.Position; set => Stream.Position = value; }

        public new void Dispose()
        {
            Stream.Dispose();
            //Archive.Dispose();
            //OuterStream.Dispose();
        }
    }
}