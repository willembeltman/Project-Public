using MyVideoEditor.Enums;
using System.Diagnostics;
using System.Linq;

namespace MyVideoEditor.Models
{
    public class MediaContainer
    {
        public MediaContainer(FfmpegExecuteblesPaths ffmpegExecuteblesPaths, string fullName)
        {
            FFMpegPaths = ffmpegExecuteblesPaths;
            FullName = fullName;

            var startinfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(FFMpegPaths.FFMpeg.FullName, "ffprobe.exe"),
                WorkingDirectory = FFMpegPaths.FFMpeg.FullName,
                Arguments = " -v error -show_entries stream=codec_long_name,codec_type,width,height,r_frame_rate,duration,bit_rate -of csv=s=,:p=0 \"" + FullName + "\"",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            using (var cmd = new Process() { StartInfo = startinfo })
            {
                cmd.Start();

                var streaminfos = new List<StreamInfo>();
                var reader = cmd.StandardOutput;
                var streamindex = 0;
                do
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parameters = line.Split(new string[] { "," }, StringSplitOptions.None);
                        if (parameters.Length > 1)
                        {
                            var streaminfo = new StreamInfo(streamindex, parameters);
                            streaminfos.Add(streaminfo);
                            streamindex++;
                        }
                    }
                } while (!reader.EndOfStream);

                VideoStreams = streaminfos
                    .Where(a => a.CodecType == CodecTypeEnum.Video)
                    .Select(a => new VideoStreamReader(ffmpegExecuteblesPaths, this, a))
                    .ToArray();

                AudioStreams = streaminfos
                    .Where(a => a.CodecType == CodecTypeEnum.Audio)
                    .Select(a => new AudioStreamReader(ffmpegExecuteblesPaths, this, a))
                    .ToArray();
            }
        }

        public FfmpegExecuteblesPaths FFMpegPaths { get; }
        public string FullName { get; }
        public VideoStreamReader[] VideoStreams { get; } 
        public AudioStreamReader[] AudioStreams { get; } 
    }
}