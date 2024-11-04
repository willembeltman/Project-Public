using System.Threading;

namespace LanCloud.Domain.IO
{
    public class FtpStreamReaderFileRefBit
    {
        public FtpStreamReaderFileRefBit(FtpStreamReader ftpStreamReader, FileRefBit fileRefBit)
        {
            FtpStreamReader = ftpStreamReader;
            FileRefBit = fileRefBit;
        }
        public FtpStreamReader FtpStreamReader { get; }
        public FileRefBit FileRefBit { get; }
        public AutoResetEvent ReadingIsDone { get; } = new AutoResetEvent(false);
        public AutoResetEvent StartNext { get; } = new AutoResetEvent(true);
        private bool KillSwitch { get; set; } = false;
        public FileRef FileRef => FtpStreamReader.FtpFileInfo.FileRef;
        public int[] Indexes => FileRefBit.Parts;


    }
}