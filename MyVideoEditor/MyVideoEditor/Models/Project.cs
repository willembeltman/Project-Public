using MyVideoEditor.Controls;
using MyVideoEditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoEditor.Models
{
    public class Project
    {
        public Project(FfmpegExecuteblesPaths ffmpegExecuteblesPaths)
        {
            FfmpegExecuteblesPaths = ffmpegExecuteblesPaths;

            MediaControl = new MediaControl(this);
            TimelinesControl = new TimelinesControl(this);
            TimelineControl = new TimelineControl(this);

            CurrentTimeline = new Timeline(this, "Current");
            Timelines = new List<Timeline>()
            {
                CurrentTimeline
            };
            MediaContainers = new List<MediaContainer>();
        }

        public FfmpegExecuteblesPaths FfmpegExecuteblesPaths { get; }

        public List<Timeline> Timelines { get; set; }
        public List<MediaContainer> MediaContainers { get; set; }
        public Timeline CurrentTimeline { get; set; }

        public MediaControl MediaControl { get; }
        public TimelinesControl TimelinesControl { get; }
        public TimelineControl TimelineControl { get; }
        public bool Playing { get; private set; }

        public void Close()
        {
            if (HasProjectFile())
            {
                SaveProject();
            }
            else
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "*.wjvproj";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(dialog.FileName);
                }
            }
        }
        public bool HasProjectFile()
        {
            throw new NotImplementedException();
        }
        public void SaveProject()
        {
            throw new NotImplementedException();
        }
        public void SaveProject(string fileName)
        {
            throw new NotImplementedException();
        }

        public void DragEnter(object sender, DragEventArgs e)
        {
            var files = CheckFiles(e);
            if (files.Any())
                e.Effect = DragDropEffects.Copy;
        }
        public void DragDrop(object sender, DragEventArgs e)
        {
            var files = CheckFiles(e);
            if (files.Any())
            {
                InsertVideo(files);
            }
        }
        private MediaContainer[] CheckFiles(DragEventArgs e)
        {
            if (e == null || e.Data == null)
                return Array.Empty<MediaContainer>();

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return Array.Empty<MediaContainer>();

            var filesobj = e.Data.GetData(DataFormats.FileDrop);
            if (filesobj == null)
                return Array.Empty<MediaContainer>();

            var files = (string[])filesobj;
            if (files == null)
                return Array.Empty<MediaContainer>();

            return ConvertToMediaContainer(files);
        }
        private MediaContainer[] ConvertToMediaContainer(string[] files)
        {
            var filteredfiles = CheckFileType.Filter(files);
            if (!filteredfiles.Any())
                return Array.Empty<MediaContainer>();

            return filteredfiles
                .Select(a => new MediaContainer(FfmpegExecuteblesPaths, a))
                .ToArray();
        }

        public void InsertVideo(string[] files)
        {
            var mediaContainers = ConvertToMediaContainer(files);
            if (mediaContainers.Any())
            {
                InsertVideo(mediaContainers);
            }
        }
        public void InsertVideo(MediaContainer[] files)
        {


        }

        public bool IsPlaying()
        {
            return Playing;
        }
        public bool IsPaused()
        {
            return !Playing;
        }

        internal void Play()
        {
            throw new NotImplementedException();
        }

        internal void Pause()
        {
            throw new NotImplementedException();
        }

        internal void Forward()
        {
            throw new NotImplementedException();
        }

        internal void Backward()
        {
            throw new NotImplementedException();
        }
    }
}
