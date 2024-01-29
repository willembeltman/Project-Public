using MyVideoEditor.DTOs;
using MyVideoEditor.Enums;
using MyVideoEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoEditor.Services
{
    public class TimelineService
    {
        public TimelineService()
        {

        }

        public Timeline? GetCurrentTimeline(Project project, Timeline timeline)
        {
            if (project == null) return null;
            return project.Timelines.First(a => a.Id == project.CurrentTimelineId);
        }

        public bool IsPlaying(Project project, Timeline timeline)
        {
            return false;
        }
        public bool IsPaused(Project project, Timeline timeline)
        {
            return true;
        }

        public void Play(Project project, Timeline? timeline)
        {
            throw new NotImplementedException();
        }

        public void Pause(Project project, Timeline? timeline)
        {
            throw new NotImplementedException();
        }

        public void Forward(Project project, Timeline? timeline)
        {
            throw new NotImplementedException();
        }

        public void Backward(Project project, Timeline? timeline)
        {
            throw new NotImplementedException();
        }
    }
}
