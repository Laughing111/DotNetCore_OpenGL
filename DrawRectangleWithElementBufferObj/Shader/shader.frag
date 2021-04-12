#version 330

out vec4 outputColor;

uniform vec4 colorAttr;

//in vec3 color;

void main()
{
    outputColor = colorAttr;//vec4(color,1.f);
}