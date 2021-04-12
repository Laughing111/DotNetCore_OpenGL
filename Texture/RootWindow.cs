using Common;
using OpenTK.Graphics.OpenGL4;  //opengl 4.x
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System.IO;

namespace GeneralUI
{
    public class RootWindow :GameWindow
    {

        private const string ShaderResPath = "../../Shader/";
        private const string ResPath = "../../Res/";
        private readonly float[] vertices =
        {
             /*position                                 texture coordinates*/
            0.5f,0.5f,0f,                                1f,1f,                    //右上
            0.5f,-0.5f,0.0f,                            1f,0f,                  //右下
            -0.5f,-0.5f,0.0f,                           0f,0f,                 //左下
            -0.5f,0.5f,0.0f,                            0f,1f,                 //左上              
        };

        //draw index help ebo
        private readonly uint[] indices =
        {
            0,1,3,   //the first 右上 右下 左上
            1,2,3,   //second 右下 左下 左上
        };

        private int vertexBufferObj;            //vbo
        private int vertexArrayObj;             //vao
        private int elementBufferObj;       //ebo

        private Shader shader;
        private Texture textureA;
        private Texture textureB;

        private Matrix4 view;
        private Matrix4 projection;
        private double time;

        public RootWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings):base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            vertexArrayObj = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObj);

            vertexBufferObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObj);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            elementBufferObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObj);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader(Path.Combine(ShaderResPath, "shader.vert"), Path.Combine(ShaderResPath, "shader.frag"));
            shader.Use();

            var vertexLocation = shader.GetAttribLoation("position");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLoation("texcoord0");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            textureA = Texture.LoadFromFile(Path.Combine(ResPath, "container.png"));
            textureA.Use(TextureUnit.Texture0);
            textureB = Texture.LoadFromFile(Path.Combine(ResPath, "awesomeface.png"));
            textureB.Use(TextureUnit.Texture1);

            shader.SetInt("ourTexA", 0);
            shader.SetInt("ourTexB", 1);

            base.OnLoad();
        }

        //create our render loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            time += 40.0 * args.Time;
            //1. first clear the color bufferBit
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //3. bind vao
            GL.BindVertexArray(vertexArrayObj);

            view = Matrix4.CreateTranslation(0f, 0f, -3f);

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);


            textureA.Use(TextureUnit.Texture0);
            textureB.Use(TextureUnit.Texture1);
            shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
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
            GL.DeleteTexture(textureA.Handle);
            GL.DeleteTexture(textureB.Handle);

            base.OnUnload();
        }
    }
}
