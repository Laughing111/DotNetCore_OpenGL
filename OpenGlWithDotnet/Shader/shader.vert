//#version statement
#version 330 core

//This defines where this input variable will be located, which is needed for GL.VertexAttribPointer
//the keyword "in" defines this as an input variable.
layout(location = 0) in vec3 position;

out vec4 vertexColor;

void main()
{
    gl_Position = vec4(position,1.0);
    vertexColor = vec4(0.5,0,0,1);
}