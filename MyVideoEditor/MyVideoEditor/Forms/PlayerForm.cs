using MyVideoEditor.DTOs;
using MyVideoEditor.Services;
using MyVideoEditor.VideoObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVideoEditor.Forms
{
    public partial class PlayerForm : Form
    {
        public PlayerForm()
        {
            InitializeComponent();
        }

        private async void PlayerForm_Load(object sender, EventArgs e)
        {

            var FfmpegExecuteblesPaths =
                new FfmpegExecuteblesPaths(Environment.ProcessPath, "Executebles");
            var service = new MediaContainerService(FfmpegExecuteblesPaths);
            var fullname = @"D:\Willem\Videos\2024-04-19 20-18-38.mkv";
            var container = service.Open(fullname);
            var videoinfo = container.VideoInfos?.FirstOrDefault();
            var videostream = new VideoStreamReader(FfmpegExecuteblesPaths, fullname, videoinfo);
            var i = 0;
            VideoFrame? frame = null;

            while (true)
            {
                frame = videostream.GetFrame(i);
                if (frame == null) break;
                direct3dVideoDisplay1.SetImageData(frame.FrameData);
            }
        }
    }
}
