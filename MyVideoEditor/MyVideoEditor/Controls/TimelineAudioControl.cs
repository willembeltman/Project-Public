using MyVideoEditor.DTOs;
using MyVideoEditor.VideoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoEditor.Controls
{
    public partial class TimelineAudioControl : UserControl
    {
        public TimelineAudioControl(TimelineAudio timelineAudio, ContainerAudio mediaAudio, AudioStreamReader audioStreamReader)
        {
            TimelineAudio = timelineAudio;
            MediaAudio = mediaAudio;
            AudioStreamReader = audioStreamReader;
            BackColor = Color.Gray;
        }

        public TimelineAudio TimelineAudio { get; }
        public ContainerAudio MediaAudio { get; }
        public AudioStreamReader AudioStreamReader { get; }

    }
}
