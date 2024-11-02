using System;
using System.IO;
using System.IO.Compression;

namespace LanCloud.Domain.Share
{
    public class FileBitStreamWriter : Stream, IDisposable
    {
        public FileInfo Info { get; }
        public FileStream OuterStream { get; }
        public ZipArchive Archive { get; }
        public ZipArchiveEntry ReadmeEntry { get; }
        public Stream Stream { get; }

        public FileBitStreamWriter(FileInfo info)
        {
            if (info.Exists) info.Delete();
            OuterStream = Info.OpenWrite();
            Archive = new ZipArchive(OuterStream, ZipArchiveMode.Create);
            ReadmeEntry = Archive.CreateEntry("data.bin");
            Stream = ReadmeEntry.Open();
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
            Archive.Dispose();
            OuterStream.Dispose();
        }
    }
}