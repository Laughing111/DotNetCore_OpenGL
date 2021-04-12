//#version statement
#version 330 core

//This defines where this input variable will be located, which is needed for GL.VertexAttribPointer
//the keyword "in" defines this as an input variable.
layout(location = 0) in vec3 position;

//layout(location = 1) in vec3 colorAttr;

out vec3 color;

void main()
{
    //color = colorAttr;
    gl_Position = vec4(position,1.0);
}