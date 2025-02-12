﻿using VideoEditor.Enums;
using VideoEditor.Dtos;
using VideoEditor.Static;
using VideoEditor.Types;

namespace VideoEditor;

public class StreamInfo
{
    public StreamInfo(File file, FFProbeStream stream)
    {
        File = file;
        Index = stream.index;
        CodecName = stream.codec_name;
        CodecLongName = stream.codec_long_name;
        CodecType = stream.codec_type == "video" ? CodecType.Video : CodecType.Audio;

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
    public CodecType CodecType { get; }

    public Resolution? Resolution { get; }
    public Fps? Fps { get; }

    public int? SampleRate { get; }
    public int? Channels { get; }

    public override string ToString()
    {
        if (CodecType == CodecType.Video)
        {
            return $"{Index} {CodecName} {Resolution}px {Fps}fps";
        }
        else
        {
            return $"{Index} {CodecName} {Title} {SampleRate}hz {Channels}ch";

        }
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is StreamInfo)) return false;

        var other = obj as StreamInfo;
        if (!File.Equals(other.File)) return false;
        if (Index != other.Index) return false;
        if (Title != other.Title) return false;
        if (CodecName != other.CodecName) return false;
        if (CodecLongName != other.CodecLongName) return false;
        if (CodecType != other.CodecType) return false;
        if (CodecType == CodecType.Video && !Resolution.Equals(other.Resolution)) return false;
        if (CodecType == CodecType.Video && !Fps.Equals(other.Fps)) return false;
        if (CodecType == CodecType.Audio && SampleRate != other.SampleRate) return false;
        if (CodecType == CodecType.Audio && Channels != other.Channels) return false;

        return true;
    }
}