using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ConsoleApp1
{

    class VideoPlayer : GameWindow
    {
        private int textureID;
        private byte[] pixelData;
        private int width = 640, height = 480;
        byte Red { get; set; }
        byte Green { get; set; }
        byte Blue { get; set; }
        Random Random { get; set; } = new Random();
        Color GetColor()
        {
            switch (Random.Next(5))
            {
                case 0:
                    if (Red == 255)
                        Red--;
                    else
                        Red++;
                    break;
                case 1:
                    if (Red == 0)
                        Red++;
                    else
                        Red--;
                    break;
                case 2:
                    if (Green == 255)
                        Green--;
                    else
                        Green++;
                    break;
                case 3:
                    if (Green == 0)
                        Green++;
                    else
                        Green--;
                    break;
                case 4:
                    if (Blue == 255)
                        Blue--;
                    else
                        Blue++;
                    break;
                case 5:
                    if (Blue == 0)
                        Blue++;
                    else
                        Blue--;
                    break;
            }

            return Color.FromArgb(Red, Green, Blue);
        }

        public VideoPlayer() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            pixelData = new byte[width * height * 4]; // 4 bytes per pixel (RGBA)
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // OpenGL texture genereren
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Eerste lege texture uploaden
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                          PixelFormat.Bgra, PixelType.UnsignedByte, pixelData);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var color = GetColor();

            // Simulatie van een video-frame update: een kleurpatroon
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;
                    pixelData[index + 0] = color.B; // B
                    pixelData[index + 1] = color.G; // G
                    pixelData[index + 2] = color.R; // R
                    pixelData[index + 3] = color.A; // A
                }
            }

            // Update de OpenGL texture met de nieuwe pixeldata
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixelData);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Simpel vierkant renderen dat de texture weergeeft
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(-1, -1);
            GL.TexCoord2(1, 0); GL.Vertex2(1, -1);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 1);
            GL.TexCoord2(0, 1); GL.Vertex2(-1, 1);
            GL.End();

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteTexture(textureID);
        }
    }
}
