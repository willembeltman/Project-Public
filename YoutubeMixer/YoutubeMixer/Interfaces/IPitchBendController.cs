using YoutubeMixer.AudioSources;
using YoutubeMixer.Models;

namespace YoutubeMixer.Interfaces
{
    public interface IPitchBendController
    {
        PitchBendState GetPitchbendState();
    }
}
