using System.Diagnostics;

namespace MyVideoEditor.Models
{
    public class VideoStreamReader : IDisposable
    {
        public VideoStreamReader(DirectoryInfo ffmpegDirectory, MediaContainer mediaContainer, StreamInfo streaminfo)
        {
            if (streaminfo.Resolution == null) throw new Exception("Resolution must be known");
            if (streaminfo.FramerateBase == null) throw new Exception("Framerate must be known");
            if (streaminfo.FramerateDivider == null) throw new Exception("Framerate must be known");

            FFMpegDirectory = ffmpegDirectory;
            MediaContainer = mediaContainer;
            StreamInfo = streaminfo;
        }

        public DirectoryInfo FFMpegDirectory { get; }
        public MediaContainer MediaContainer { get; }
        public StreamInfo StreamInfo { get; }

        public int CurrentFrameIndex { get; set; }
        private int? RealFrameIndex { get; set; }

        private Process? Process { get; set; }
        private BinaryReader? StreamReader { get; set; }

        public int FrameWidth => StreamInfo.Resolution.Value.Width;
        public int FrameHeight => StreamInfo.Resolution.Value.Width;
        public int FramerateBase => StreamInfo.FramerateBase.Value;
        public int FramerateDivider => StreamInfo.FramerateDivider.Value;

        public VideoFrame? GetNextFrame(int? index = null)
        {
            if (index != null)
                CurrentFrameIndex = index.Value;

            if (CurrentFrameIndex != RealFrameIndex)
                RealFrameIndex = StartProcess(CurrentFrameIndex);

            var frame = ReadFrame();
            CurrentFrameIndex++;
            RealFrameIndex++;
            return frame;
        }

        private VideoFrame? ReadFrame()
        {
            int frameSizeInBytes = FrameWidth * FrameHeight * 3;
            int bytesRead;
            int totalBytesRead = 0;
            byte[] frameData = new byte[frameSizeInBytes];
            
            while ((bytesRead = StreamReader.Read(frameData, totalBytesRead, frameData.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;
                if (totalBytesRead == frameData.Length)
                    return new VideoFrame(frameData, FrameWidth, FrameHeight);
            }
            return null;
        }

        private int StartProcess(int startframeindex)
        {
            if (Process != null || StreamReader != null)
            {
                Dispose();
            }

            string ss = GetTimeStamp(startframeindex, FramerateBase, FramerateDivider);

            Process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe"),
                    WorkingDirectory = FFMpegDirectory.FullName,
                    Arguments = $"-i \"{MediaContainer.FullName}\" -ss {ss} -f rawvideo -pix_fmt rgb24 -",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            Process.Start();
            StreamReader = new BinaryReader(Process.StandardOutput.BaseStream);

            return startframeindex;
        }
        private string GetTimeStamp(int frameIndex, int framerateBase, int framerateDivider)
        {
            double seconds = (double)frameIndex / (framerateBase / (double)framerateDivider);
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"hh\:mm\:ss\.fff");
        }


        public void Dispose()
        {
            StreamReader?.Dispose();
            Process?.Dispose();
        }
    }
}