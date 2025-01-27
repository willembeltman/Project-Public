namespace VideoEditor;

public class Project
{
    public Project()
    {
        CurrentTimeline = new Timeline(this);
        Timelines.Add(CurrentTimeline);
    }

    public ConcurrentArray<File> Files { get; } = [];
    public ConcurrentArray<Timeline> Timelines { get; } = [];

    public string? Path { get; set; }
    public Timeline CurrentTimeline { get; set; }

    //TimelineVideoClip[] _VideoClips { get; set; } = { };
    //public TimelineVideoClip[] VideoClips
    //{
    //    get
    //    {
    //        TimelineVideoClip[]? res = null;
    //        lock (this)
    //        {
    //            res = _VideoClips;
    //        }
    //        return res;
    //    }
    //}
    //public void AddVideoClip(TimelineVideoClip clip)
    //{
    //    lock (this)
    //    {
    //        var newlist = new TimelineVideoClip[_VideoClips.Length + 1];
    //        Array.Copy(_VideoClips, newlist, _VideoClips.Length);
    //        newlist[newlist.Length - 1] = clip;
    //        _VideoClips = newlist;
    //    }
    //}
    //public void RemoveVideoClip(TimelineVideoClip clip)
    //{
    //    lock (this)
    //    {
    //        var newlist = new TimelineVideoClip[_VideoClips.Length - 1];
    //        var index = 0;
    //        foreach (var item in _VideoClips)
    //        {
    //            if (item == clip)
    //                continue;

    //            newlist[index] = item;
    //            index++;
    //        }
    //        if (index == _VideoClips.Length)
    //        {
    //            _VideoClips = newlist;
    //        }
    //    }
    //}

}

