using Emgu.CV;
using MyVideoEditor.Models;
using MyVideoEditor.Services;
using MyVideoEditor.VideoObjects;
using System.Data;
using System.Runtime.InteropServices;

namespace MyVideoEditor.Forms
{
    public partial class FindVisualStudioForm : Form
    {
        public FindVisualStudioForm(MediaContainerService mediaContainerService, ConsoleForm consoleForm)
        {
            MediaContainerService = mediaContainerService;
            ConsoleForm = consoleForm;
            InitializeComponent();
        }

        public MediaContainerService MediaContainerService { get; }
        public ConsoleForm ConsoleForm { get; }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = @"D:\Willem\Videos\BSD Website";
            var titlefilename = @"D:\Willem\Desktop\Windows\VisualStudioCodeTitle.PNG";
            var logofilename = @"D:\Willem\Desktop\Windows\VisualStudioCodeLogo.PNG";
            var titleImage = Image.FromFile(titlefilename);
            var logoImage = Image.FromFile(logofilename);
            var titleFrame = VideoFrame.FromImage(titleImage as Bitmap);
            var logoFrame = VideoFrame.FromImage(logoImage as Bitmap);

            ShowFrame(titleFrame, "title");
            ShowFrame(logoFrame, "logo");

            var dirinfo = new DirectoryInfo(path);
            if (dirinfo.Exists)
            {
                var ffmpeginfo = ConsoleForm.MainForm.FfmpegExecuteblesPaths.FFMpeg;

                using (var writer = new VideoStreamWriter(ffmpeginfo.Directory, "D:\\test.mp4", 3840, 2160))
                {
                    var fileinfo = dirinfo.GetFiles().OrderBy(a => a.Name).Skip(7).FirstOrDefault();
                    {
                        var mediacontainer = MediaContainerService.Open(fileinfo.FullName);
                        var videostream = mediacontainer.VideoStreams.FirstOrDefault();
                        if (videostream != null)
                        {
                            ProcessVideo(titleImage, titleFrame, logoFrame, videostream);
                        }
                    }
                }
            }
        }

        private void ProcessVideo(Image titleImage, VideoFrame titleFrame, VideoFrame logoFrame, VideoStreamReader? videostream)
        {
            long frameindex = 0;
            VideoFrame? frame = null;
            while (true)
            {
                frame = videostream.GetFrame(frameindex);
                //ShowFrame(ResizeFrame(frame), "main");

                if (frameindex > 50)
                {
                    if (frame == null) break;

                    Point? findtitle = FindImage(frame, titleFrame);
                    if (findtitle != null)
                    {
                        Rectangle filterrect = new Rectangle(0, findtitle.Value.Y - 5, frame.Width, titleImage.Height + 10);
                        Point? findtitle2 = FindImage(frame, logoFrame, filterrect);
                    }
                }
                frameindex++;
            }
        }

        private Point? FindImage(VideoFrame frame, VideoFrame img, Rectangle? filterrect = null)
        {
            var rect = filterrect ?? new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = frame.Width,
                Height = frame.Height
            };
            for (var y = rect.Y; y < rect.Y + rect.Height - img.Height; y++)
                for (var x = rect.X; x < rect.X + rect.Width - img.Width; x++)
                {
                    var found = CheckOnLocation(frame, img, y, x);
                    if (found)
                    {
                        return new Point(x, y);
                    }
                }
            return null;
        }
        
        private bool CheckOnLocation(VideoFrame frame, VideoFrame img, int y, int x)
        {
            //var clonedframe = frame.Clone();
            var rtn = true;
            var marge = 88;

            for (var imgy = 0; imgy < img.Height; imgy++)
            {
                for (var imgx = 0; imgx < img.Width; imgx++)
                {
                    var framex = x + imgx;
                    var framey = y + imgy;

                    var framepixel = frame.GetPixel(framex, framey);
                    var imgpixel = img.GetPixel(imgx, imgy);

                    var difpixel = Diff(framepixel, imgpixel);

                    //clonedframe.SetPixel(framex, framey, Color.FromArgb(255 - difpixel.R, 255 - difpixel.G, 255 - difpixel.B));


                    if (difpixel.R < marge &&
                        difpixel.G < marge &&
                        difpixel.B < marge)
                    {
                        continue;
                    }
                    else
                    {
                        rtn = false;
                        break;
                    }
                }
                if (rtn == false)
                    break;
            }

            //ShowFrame(clonedframe, "main");
            return rtn;
        }

        private Color Diff(Color framepixel, Color imgpixel)
        {
            return Color.FromArgb(
                Diff(framepixel.R, imgpixel.R),
                Diff(framepixel.G, imgpixel.G),
                Diff(framepixel.B, imgpixel.B));
        }

        private byte Diff(byte framepixel, byte imgpixel)
        {
            //if (framepixel > imgpixel)
            //    return Convert.ToByte(framepixel - imgpixel);
            //return 0;

            if (framepixel > imgpixel)
            {
                return Convert.ToByte(framepixel - imgpixel);
            }
            else if (imgpixel > framepixel)
            {
                return Convert.ToByte(imgpixel - framepixel);
            }
            return 0;
        }



        VideoFrame ResizeFrame(VideoFrame frame)
        {
            var width = frame.Width / 2;
            var height = frame.Height / 2;

            var newdata = new byte[width * height * 3];
            var newframe = new VideoFrame(newdata, width, height, frame.Index);
            for (var y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    var c1 = frame.GetPixel(x * 2 + 0, y * 2 + 0);
                    var c2 = frame.GetPixel(x * 2 + 1, y * 2 + 0);
                    var c3 = frame.GetPixel(x * 2 + 0, y * 2 + 1);
                    var c4 = frame.GetPixel(x * 2 + 1, y * 2 + 1);

                    var r = (Convert.ToInt32(c1.R) + c2.R + c3.R + c4.R) / 4;
                    var g = (Convert.ToInt32(c1.G) + c2.G + c3.G + c4.G) / 4;
                    var b = (Convert.ToInt32(c1.B) + c2.B + c3.B + c4.B) / 4;
                    var newcolor = Color.FromArgb(r, g, b);

                    newframe.SetPixel(x, y, newcolor);
                }

            return newframe;
        }
        void ShowFrame(VideoFrame? frame, string title = "test")
        {
            ShowFrame(frame.FrameData, frame.Width, frame.Height, title);
        }
        void ShowFrame(byte[] frameData, int frameWidth, int frameHeight, string title = "test")
        {
            using (var mat = new Mat(frameHeight, frameWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 3))
            {
                Marshal.Copy(frameData, 0, mat.DataPointer, frameData.Length);
                CvInvoke.Imshow(title, mat);
                CvInvoke.WaitKey(1);
            }
        }
    }
}
