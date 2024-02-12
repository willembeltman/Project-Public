using Microsoft.VisualBasic.Devices;
using MyVideoEditor.Controls;
using MyVideoEditor.DTOs;
using MyVideoEditor.Forms;
using MyVideoEditor.Models;
using MyVideoEditor.VideoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoEditor.Services
{
    public class ProjectService
    {
        #region Props 

        MainForm MainForm { get; }

        MediaContainerService MediaContainerService => MainForm.MediaContainerService;
        TimelineService TimelineService => MainForm.TimelineService;
        TimeStampService TimeStampService => MainForm.TimeStampService;

        Project? Project => MainForm.Project;
        Timeline? Timeline => MainForm.Timeline;

        public ProjectService(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        #endregion

        public bool Close()
        {
            if (HasProjectFile())
            {
                SaveProjectClicked();
                return true;
            }
            else
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "*.wjvproj";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(dialog.FileName);
                    return true;
                }
            }
            return false;
        }

        private string[] GetDragAndDropFiles(DragEventArgs e)
        {
            if (e == null || e.Data == null)
                return Array.Empty<string>();

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return Array.Empty<string>();

            var filesobj = e.Data.GetData(DataFormats.FileDrop);
            if (filesobj == null)
                return Array.Empty<string>();

            var files = (string[])filesobj;
            if (files == null)
                return Array.Empty<string>();

            return files;
            //return MediaContainerService.Open(files);
        }

        public bool HasProjectFile()
        {
            throw new NotImplementedException();
        }
        public void SaveProjectClicked()
        {
            throw new NotImplementedException();
        }
        public void SaveProject(string fileName)
        {
            throw new NotImplementedException();
        }

        public void DragEnter(object sender, DragEventArgs e)
        {
            var files = GetDragAndDropFiles(e);
            if (files.Any())
                e.Effect = DragDropEffects.Copy;
        }
        public void DragDrop(object sender, DragEventArgs e)
        {
            var files = GetDragAndDropFiles(e);
            if (files.Any())
            {
                InsertVideos(files);
            }
        }

        public Project NewProjectClicked_AfterConfirm()
        {
            var defaultTimeline = new Timeline()
            {
                Name = "Default",
            };
            return new Project()
            {
                Timelines = new List<Timeline>() { defaultTimeline },
                Medias = new List<DTOs.Container>(),
                Width = 1920,
                Height = 1080,
                FramerateBase = 60,
                FramerateDivider = 1,
                CurrentTimelineId = defaultTimeline.Id
            };
        }
        public Project OpenProjectClicked_AfterConfirm()
        {
            throw new NotImplementedException();
        }

        public void SaveAsProjectClicked()
        {
            throw new NotImplementedException();
        }

        public void InsertVideosButtonClicked()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InsertVideos(openFileDialog.FileNames);
            }
        }
        private void InsertVideos(string[] files)
        {
            if (Project == null) return;
            if (Timeline == null) return;

            var mediaContainers = MediaContainerService.Open(files);
            if (mediaContainers.Any())
            {
                var timelinestart = Timeline.TotalLength;

                foreach (var mediaContainer in mediaContainers)
                {
                    var groupid = Guid.NewGuid();
                    var duration = 0d;

                    var media = new DTOs.Container()
                    {
                        FullName = mediaContainer.FullName
                    };
                    Project.Medias.Add(media);

                    foreach (var videostream in mediaContainer.VideoStreams)
                    {
                        if (duration == 0d)
                        {
                            if (videostream.StreamInfo.Duration == null) throw new Exception("Duration cannot be read");
                            duration = videostream.StreamInfo.Duration.Value;
                        }

                        // Blauwdruk aanmaken
                        var mediavideo = new ContainerVideo()
                        {
                            MediaId = media.Id,
                            StreamIndex = videostream.StreamInfo.Index,
                            Duration = videostream.StreamInfo.Duration,
                            Resolution = videostream.StreamInfo.Resolution,
                            FramerateBase = videostream.StreamInfo.FramerateBase,
                            FramerateDivider = videostream.StreamInfo.FramerateDivider
                        };
                        media.Videos.Add(mediavideo);

                        var videoitem = new TimelineVideo()
                        {
                            GroupId = groupid,
                            MediaVideoId = media.Id,
                            TimelineStartTime = timelinestart,
                            TimelineEndTime = timelinestart + duration,
                            MediaStartTime = 0,
                            MediaEndTime = duration,
                        };
                        Timeline.TimelineVideos.Add(videoitem);

                        // Control aanmaken
                        MainForm.MainTimelineControl.TimelineControl.AddTimelineVideoControl(mediavideo, videoitem, videostream);
                    }
                    foreach (var audiostream in mediaContainer.AudioStreams)
                    {
                        if (duration == 0d)
                        {
                            if (audiostream.StreamInfo.Duration == null) throw new Exception("Duration cannot be read");
                            duration = audiostream.StreamInfo.Duration.Value;
                        }

                        var mediaaudio = new ContainerAudio()
                        {
                            MediaId = media.Id,
                            StreamIndex = audiostream.StreamInfo.Index,
                            Duration = audiostream.StreamInfo.Duration,
                        };
                        media.Audios.Add(mediaaudio);

                        var audioitem = new TimelineAudio()
                        {
                            GroupId = groupid,
                            MediaAudioId = media.Id,
                            TimelineStartTime = timelinestart,
                            TimelineEndTime = timelinestart + duration,
                            MediaStartTime = 0,
                            MediaEndTime = duration,
                        };
                        Timeline.TimelineAudios.Add(audioitem);

                        // Control aanmaken
                        MainForm.MainTimelineControl.TimelineControl.AddTimelineAudioControl(mediaaudio, audioitem, audiostream);
                    }

                    timelinestart += duration;
                }

                Timeline.TotalLength = timelinestart;

                MainForm.MainTimelineControl.TimelineControl.Invalidate();
            }
        }
    }
}
