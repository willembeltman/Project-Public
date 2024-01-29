﻿using MyVideoEditor.Models;

namespace MyVideoEditor.DTOs
{
    public class MediaAudio
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MediaId { get; set; }
        public int StreamIndex { get; set; }
        public double? Duration { get; set; }
    }
}