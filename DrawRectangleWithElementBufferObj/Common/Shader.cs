using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    /// <summary>
    /// a simple class meant to help create shader
    /// </summary>
    public class Shader
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> uniformLocations;

        public Shader(string vertexPath,string fragPath)
        {

            //1.vertex shader
            var shaderSource = File.ReadAllText(vertexPath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            //bind the GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);

            CompileShader(vertexShader);

            //2.fragment shader
            shaderSource = File.ReadAllText(fragPath);
            var fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShader, shaderSource);
            CompileShader(fragShader);

            //3.merge two shaders
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle,vertexShader);
            GL.AttachShader(Handle,fragShader);

            LinkProgram(Handle);

            //4.Detach And Delete
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragShader);


            //5. cache all the shader uniform locations
            //5.1 First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            //5.2 Next, allocate the dictionary to hold the locations.
            uniformLocations = new Dictionary<string, int>();

            for(int i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                uniformLocations.Add(key, location);
            }
        }


        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLoation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetInt(string name,int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(uniformLocations[name], data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(uniformLocations[name], data);
        }

    }
}
