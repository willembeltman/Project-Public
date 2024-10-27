﻿using System;

namespace LanCloud
{
    public class FileBitInformation
    {
        public string[] Paths { get; set; } = Array.Empty<string>();
        public long Part { get; set; }
        public int Size { get; set; }
        public string Hash { get; set; }
        public long OriginalSize { get; internal set; }
        public string Extention { get; internal set; }
    }
}