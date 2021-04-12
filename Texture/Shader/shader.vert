//#version statement
#version 330 core

//This defines where this input variable will be located, which is needed for GL.VertexAttribPointer
//the keyword "in" defines this as an input variable.
in vec3 position;

in vec2 texcoord0;

out vec2 uv;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    uv = texcoord0;
    gl_Position = vec4(position,1.0) * model* view*projection;
}