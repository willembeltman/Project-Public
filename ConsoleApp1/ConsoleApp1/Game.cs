using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1;

public class Game : GameWindow
{
    public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(width, height));
        Height = height;
        Width = width;
    }

    int Width { get; set; }
    int Height { get; set; }

    float[] verticies => new float[9]
    {
        0f,     0.5f,   0f,
        -0.5f,  -0.5f,  0f,
        0.5f,   -0.5f,  0f
    };

    int vao;
    int shaderProgram;

    protected override void OnResize(ResizeEventArgs e)
    {
        Width = e.Width;
        Height = e.Height;

        base.OnResize(e);

        GL.Viewport(0, 0, Width, Height);
    }
    protected override void OnLoad()
    {
        base.OnLoad();

        vao = GL.GenVertexArray();

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float), verticies, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vao, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        shaderProgram = GL.CreateProgram();
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
        GL.CompileShader(fragmentShader);

        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(fragmentShader, vertexShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }
    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(vao);
        GL.DeleteProgram(shaderProgram);
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(Color4.AliceBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    public static string LoadShaderSource(string filePath)
    {
        var root = new FileInfo(Environment.ProcessPath!);
        return File.ReadAllText(Path.Combine(root.Directory!.FullName, Path.Combine("Shaders", filePath)));
    }
}
