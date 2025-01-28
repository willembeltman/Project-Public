using System;
using System.Threading.Channels;
using VideoEditor.Enums;
using VideoEditor.Dtos;
using VideoEditor.Static;

namespace VideoEditor.Info;

public class StreamInfo
{
    public StreamInfo(File file, FFProbeStream stream)
    {
        File = file;
        Index = stream.index;
        CodecName = stream.codec_name;
        CodecLongName = stream.codec_long_name;
        CodecType = stream.codec_type == "video" ? CodecTypeEnum.Video : CodecTypeEnum.Audio;

        // Video
        Resolution = Resolution.TryParse(stream.width, stream.height, out Resolution? resolution) ? resolution : null;
        Fps = Fps.TryParse(stream.avg_frame_rate, out Fps? fps) ? fps : null;

        // Audio
        Title = stream.tags?.title;
        Channels = stream.channels;
        SampleRate = FFHelpers.TryParseToInt(stream.sample_rate, out int sampleRate) ? sampleRate : null;
    }

    public File File { get; }
    public int Index { get; }
    public string? Title { get; }
    public string? CodecName { get; }
    public string? CodecLongName { get; }
    public CodecTypeEnum CodecType { get; }

    public Resolution? Resolution { get; }
    public Fps? Fps { get;  }
    public int? SampleRate { get; }
    public int? Channels { get; }

    public override string ToString()
    {
        if (CodecType == CodecTypeEnum.Video)
        {
            return $"{Index} {CodecName} {Resolution}px {Fps}fps";
        }
        else
        {
            return $"{Index} {CodecName} {Title} {SampleRate}hz {Channels}ch";

        }
    }
}