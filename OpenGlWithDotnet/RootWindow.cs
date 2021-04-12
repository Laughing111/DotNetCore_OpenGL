using Common;

using OpenTK.Graphics.OpenGL4;  //opengl 4.x
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System.IO;

namespace GeneralUI
{
    public class RootWindow :GameWindow
    {

        private const string ShaderResPath = "../../Shader/";
        private readonly float[] vertices =
        {
            -0.5f,-0.5f,0.0f,      //左下
            0.5f,-0.5f,0.0f,       //右下
            0.0f,0.5f,0.0f,        //顶中           
        };

        private int vertexBufferObj;            //vbo
        private int vertexArrayObj;             //vao

        private Shader shader;

        public RootWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings):base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //1.we need to send our vertices over to the graphics card so OpenGL can use them
            //2.To do this,we need to create whats called a Vertex Buffer Object (VBO)
            //3.These allow you to upload a bunch of data to a buffer,and send the buffer to the graphics card
            //4.This effectively sends all the vertices at the same time    大量顶点数据 同时传递给gpu

            //1.first,need to create a buff,this function returns a handle to it   返回的是内存地址
            vertexBufferObj = GL.GenBuffer();

            //2.bind the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObj);

            //3.upload the vertices to the buffer
            //how the buffer will be used,so that openGl can write the data to the proper memory space on the gpu
            //and three  bufferUsagehints for drawing
            //1.Static Draw : rarely change  
            //2.Dynamic Draw : change frequently
            //3.Stream Draw: change on the frame
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //4.tell gpu how to use th vbo(divided  up into vertices)
            //and vao helps to do it
            vertexArrayObj = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObj);

            //4.1 interpret it  in the way we specified
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //4.2 enable variable 0 in vertex shader
            GL.EnableVertexAttribArray(0);

            //5. create shader
            shader = new Shader(Path.Combine(ShaderResPath,"shader.vert"), Path.Combine(ShaderResPath, "shader.frag"));

            //6.use shader to draw
            shader.Use();


            base.OnLoad();
        }

        //create our render loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            //1. first clear the color bufferBit
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //2.bind shader
            shader?.Use();

            //3. bind vao
            GL.BindVertexArray(vertexArrayObj);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //swap frameBufferBit
            SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            //1.Unbind all resources by binding the targets to 0/null
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(vertexBufferObj);
            GL.DeleteVertexArray(vertexArrayObj);

            GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }
    }
}
