using Microsoft.VisualBasic.Devices;
using MyVideoEditor.Controls;
using MyVideoEditor.DTOs;
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
        public FfmpegExecuteblesPaths FfmpegExecuteblesPaths { get; }
        public MediaContainerService MediaContainerService { get; }

        public ProjectService(
            FfmpegExecuteblesPaths ffmpegExecuteblesPaths, 
            MediaContainerService mediaContainerService)
        {
            FfmpegExecuteblesPaths = ffmpegExecuteblesPaths;
            MediaContainerService = mediaContainerService;
        }


        public bool Close(Project project)
        {
            if (HasProjectFile(project))
            {
                SaveProjectClicked(project);
                return true;
            }
            else
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "*.wjvproj";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(project, dialog.FileName);
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

        public bool HasProjectFile(Project project)
        {
            throw new NotImplementedException();
        }
        public void SaveProjectClicked(Project project)
        {
            throw new NotImplementedException();
        }
        public void SaveProject(Project project, string fileName)
        {
            throw new NotImplementedException();
        }

        public void DragEnter(Project project, Timeline timeline, object sender, DragEventArgs e)
        {
            var files = GetDragAndDropFiles(e);
            if (files.Any())
                e.Effect = DragDropEffects.Copy;
        }
        public void DragDrop(Project project, Timeline timeline, object sender, DragEventArgs e)
        {
            var files = GetDragAndDropFiles(e);
            if (files.Any())
            {
                InsertVideos(project, timeline, files);
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
                Medias = new List<Media>(),
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

        public void SaveAsProjectClicked(Project project)
        {
            throw new NotImplementedException();
        }

        public void InsertVideosButtonClicked(Project project, Timeline timeline)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InsertVideos(project, timeline, openFileDialog.FileNames);
            }
        }
        private void InsertVideos(Project project, Timeline timeline, string[] files)
        {
            var mediaContainers = MediaContainerService.Open(files);
            if (mediaContainers.Any())
            {
                var timelinestart = timeline.TotalLength;

                foreach (var mediaContainer in mediaContainers)
                {
                    var groupid = Guid.NewGuid();
                    var duration = 0d;

                    var media = new Media()
                    {
                        FullName = mediaContainer.FullName
                    };
                    project.Medias.Add(media);

                    foreach (var videostream in mediaContainer.VideoStreams)
                    {
                        if (duration == 0d)
                        {
                            if (videostream.StreamInfo.Duration == null) throw new Exception("Duration cannot be read");
                            duration = videostream.StreamInfo.Duration.Value;
                        }

                        var mediavideo = new MediaVideo()
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
                        timeline.TimelineVideos.Add(videoitem);
                    }
                    foreach (var audiostream in mediaContainer.AudioStreams)
                    {
                        if (duration == 0d)
                        {
                            if (audiostream.StreamInfo.Duration == null) throw new Exception("Duration cannot be read");
                            duration = audiostream.StreamInfo.Duration.Value;
                        }

                        var mediaaudio = new MediaAudio()
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
                        timeline.TimelineAudios.Add(audioitem);
                    }

                    timelinestart += duration;
                }

                timeline.TotalLength = timelinestart;
            }
        }
    }
}
