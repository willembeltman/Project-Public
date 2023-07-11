using MyVideoEditor.Enums;

namespace MyVideoEditor.Models
{
    public class StreamInfo
    {
        public StreamInfo(int streamindex, string[] parameters)
        {
            Index = streamindex;
            Codec = parameters[0];
            CodecTypeString = parameters[1];

            if (parameters.Length == 5)
            {
                if (double.TryParse(parameters[3]
                    .Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out double duration))
                    Duration = duration;

                if (int.TryParse(parameters[4], out int bitrate))
                    Bitrate = bitrate;
            }

            if (parameters.Length == 7)
            {
                if (int.TryParse(parameters[2], out int width) &&
                    int.TryParse(parameters[3], out int height))
                    Resolution = new Size(width, height);

                var fpsparameters = parameters[4].Split(new string[] { "/" }, StringSplitOptions.None);
                if (fpsparameters.Length == 2)
                {
                    if (int.TryParse(fpsparameters[0], out int fpsbase))
                        FramerateBase = fpsbase;
                    if (int.TryParse(fpsparameters[1], out int fpsdivider))
                        FramerateDivider = fpsdivider;
                }

                if (double.TryParse(parameters[5]
                    .Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out double duration))
                    Duration = duration;

                if (int.TryParse(parameters[6], out int bitrate))
                    Bitrate = bitrate;
            }
        }

        public int Index { get; set; }
        public string Codec { get; set; }
        public string CodecTypeString { get; set; }
        public CodecTypeEnum CodecType => CodecTypeString == "video" ? CodecTypeEnum.Video : CodecTypeEnum.Audio;
        public double? Duration { get; set; }
        public int? Bitrate { get; set; }
        public Size? Resolution { get; set; }
        public int? FramerateBase { get; set; }
        public int? FramerateDivider { get; set; }
        public double? Framerate => FramerateBase / FramerateDivider;
    }
}